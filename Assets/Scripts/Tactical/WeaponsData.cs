using Mercs.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mercs.Tactical.Buffs;
using Mercs.Tactical.States;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(UnitInfo))]
    public class WeaponsData : MonoBehaviour, IEnumerable<WeaponsData.Info>
    {
        private UnitInfo info;


        public class Info
        {
            public Weapon Weapon { get;  internal set; }

            public Ammo Ammo { get; internal set; }
            public bool UseAmmo => Weapon.AmmoType != AmmoType.None;
            public int AmmoLeft { get; internal set; }
            public bool Destroyed { get; internal set; }

            public bool CanShoot => !Destroyed && (!UseAmmo || AmmoLeft > 0);
            public bool Enabled { get; set; }
            public int Target { get; set; }
        }

        private Dictionary<Ammo, List<AmmoPod>> PodsByAmmo;
        private Dictionary<Weapon, Info> weapons;

        public List<Info> Weapons { get; private set; }
        public List<AmmoPod> Pods { get; private set; }

        public int this[Ammo ammo] => PodsByAmmo.TryGetValue(ammo, out var list) ? list.Sum(i => i.Count) : 0;


        public float MaxOptimalRange { get; private set; }
        public float MaxFalloffRange { get; private set; }
        public float MaxIndirectRange { get; private set; }
        public bool HasIndirect => MaxIndirectRange > 0;
        public float MinRange { get; private set; }

        public IEnumerable<(Ammo ammo, int count)> this[AmmoType ammo] =>
            Pods.Where(i => i.Type == ammo)
                .GroupBy(i => i.LoadedAmmo)
                .Select(i => (i.Key, i.Sum(pod => pod.Count)));

        private void Awake()
        {
            info = GetComponent<UnitInfo>();
        }

        public void Build()
        {
            Pods = (from item in info.Modules
                    where item.ModType == ModuleType.AmmoPod
                    let pod = item as AmmoPod
                    where pod.LoadedAmmo != null
                    orderby pod.Priority
                    select pod).ToList();
            PodsByAmmo = Pods.GroupBy(i => i.LoadedAmmo).ToDictionary(i => i.Key, i => i.ToList());
            foreach (var ammo in PodsByAmmo.Keys)
            {
                ammo.ApplyUpgrade();
            }

            Weapons = info.Modules.OfType<Weapon>().Select(i => new Info {Weapon = i}).ToList();
            weapons = Weapons.ToDictionary(i => i.Weapon);

            foreach (var weaponInfo in Weapons)
            {
                weaponInfo.Enabled = true;
                if (weaponInfo.UseAmmo)
                {
                    int count;
                    if (weaponInfo.Weapon.LoadedAmmo == null || (count = this[weaponInfo.Weapon.LoadedAmmo]) == 0)
                    {
                        var ammo = this[weaponInfo.Weapon.AmmoType].FirstOrDefault();
                        weaponInfo.Weapon.LoadAmmo(ammo.ammo);
                        weaponInfo.Ammo = ammo.ammo;
                        weaponInfo.AmmoLeft = ammo.count;
                    }
                    else
                    {
                        weaponInfo.Ammo = weaponInfo.Weapon.LoadedAmmo;
                        weaponInfo.AmmoLeft = count;
                    }
                }
                else
                {
                    weaponInfo.Ammo = null;
                    weaponInfo.AmmoLeft = 0;
                }
            }

            calc_range();
        }

        private void calc_range()
        {
            MaxOptimalRange = Weapons.Max(i => i.Weapon.Optimal);
            MaxFalloffRange = Weapons.Max(i => i.Weapon.Falloff);
            MinRange = Weapons.Min(i => i.Weapon.MinRange);
            MaxIndirectRange = Weapons.Where(i => i.Weapon.IndirectFire).DefaultIfEmpty().Max(i =>  i?.Weapon.Falloff ?? 0);
        }

        public List<(Info, float)> GetChance(Visibility.LoS los)
        {
            var res = new List<(Info, float)>();

            foreach(var data in Weapons)
            {
                //пушка не может стрелять, цель не видима или слишком далеко
                if(!data.CanShoot || los.Distance > data.Weapon.Falloff || los.Level <= Visibility.Level.Sensor
                    || info.Buffs.Shutdown || info.Buffs.Prone)
                    res.Add((data, 0));

                //базовое значение
                float aim = data.Weapon.AimBonus;

                aim += indirect_check(los, data.Weapon);

                if (aim < -100f)
                {
                    res.Add((data, 0));
                    continue;
                }

                if (aim < -100f) continue;

                //бонусы баффов
                aim += info.Buffs.SumBuffs(BuffType.Aim);
                aim -= info.Buffs.SumBuffs(BuffType.Evasion);

                aim *= range_check(los, data.Weapon);

                aim = Mathf.Clamp(0.5f + aim / 0.05f, 0, 1);

                res.Add((data, aim));
            }

            return res;
        }

        public List<(Info, float)> GetChance(Visibility.LoS los, MovementStateData move)
        {
            var res = new List<(Info, float)>();

            foreach (var data in Weapons)
            {
                //пушка не может стрелять, цель не видима или слишком далеко
                if (!data.CanShoot || los.Distance > data.Weapon.Falloff || los.Level <= Visibility.Level.Sensor
                    || info.Buffs.Shutdown || info.Buffs.Prone)
                    res.Add((data, 0));

                //базовое значение
                float aim = data.Weapon.AimBonus;

                aim += indirect_check(los, data.Weapon);

                if (aim < -100f)
                {
                    res.Add((data, 0));
                    continue;
                }
            

            //бонусы баффов
                aim += info.Buffs.Where(i => i.TAG != "move").SumBuffs(BuffType.Aim);
                aim -= info.Buffs.SumBuffs(BuffType.Evasion);

                if(move != null)
                {
                    switch (move.Type)
                    {
                        case MovementStateData.MoveType.Move:
                            aim -= 1;
                            break;
                        case MovementStateData.MoveType.Run:
                            aim -= 2;
                            break;
                        case MovementStateData.MoveType.Jump:
                            aim -= 3;
                            break;
                        case MovementStateData.MoveType.Evasive:
                            aim -= 3;
                            break;
                    }
                }

                aim *= range_check(los, data.Weapon);


                aim = Mathf.Clamp(0.5f + aim / 0.05f, 0, 1);

                res.Add((data, aim));
            }

            return res;
        }

        private float indirect_check(Visibility.LoS los, Weapon weapon)
        {
            //если нет прямой видимости
            if (los.Line == Visibility.Line.Indirect)
            {
                //если оружие не может стрелять по наводке
                if (!weapon.IndirectFire || los.Level <= Visibility.Level.Visual)
                    return -1000f;

                // иначе штраф за наводку
                return -4f;
            }
            //если видииость частичная и пушка может стрелять напрямую
            if (los.Line == Visibility.Line.Partial && weapon.DirectFire)
                return -2f;
            //если пушка может стрелять только по наводке - обычный штраф

            if (!weapon.DirectFire)
                return -4f;
            
            return 0;
        }

        private float range_check(Visibility.LoS los, Weapon weapon)
        {
            if (los.Distance > weapon.Falloff)
                return 0;
            if (los.Distance > weapon.MinRange && los.Distance <= weapon.Optimal)
                return 1;
            if(los.Distance <= weapon.MinRange)
                switch (weapon.Type)
                {
                    case WeaponType.GuidedMissile:
                    case WeaponType.DirectFireMissile:
                        return 0;

                    case WeaponType.IonCannon:
                    case WeaponType.Artillery:
                        var t = los.Distance / weapon.MinRange;
                        return t* t ;

                    case WeaponType.Laser:
                    case WeaponType.Autocannon:
                        return Mathf.Sqrt(los.Distance / weapon.MinRange);
                }
            return 1 - (los.Distance - weapon.Optimal) / (weapon.Falloff - weapon.Optimal);
        }

        #region IEnumerable
        public IEnumerator<Info> GetEnumerator()
        {
            return Weapons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
