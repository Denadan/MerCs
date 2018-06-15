using Mercs.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        public bool HasIndirect { get; private set; }
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
            HasIndirect = Weapons.Any(i => i.Weapon.IndirectFire);
        }

        public IEnumerator<Info> GetEnumerator()
        {
            return Weapons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
