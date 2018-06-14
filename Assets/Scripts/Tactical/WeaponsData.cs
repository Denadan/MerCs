using Mercs.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(UnitInfo))]
    public class WeaponsData : MonoBehaviour, IEnumerable<WeaponsData.WeaponInfo>
    {
        private UnitInfo info;


        public class WeaponInfo
        {
            public Weapon Weapon { get;  internal set; }

            public Ammo Ammo { get; internal set; }
            public bool UseAmmo => Weapon.AmmoType != AmmoType.None;
            public int AmmoLeft { get; internal set; }
            public bool Destroyed { get; internal set; }

            public bool CanShoot => !Destroyed && (!UseAmmo || AmmoLeft > 0);
        }

        private Dictionary<Ammo, List<AmmoPod>> PodsByAmmo;
        private Dictionary<Weapon, WeaponInfo> weapons;

        public List<WeaponInfo> Weapons { get; private set; }
        public List<AmmoPod> Pods { get; private set; }

        public int this[Ammo ammo] => PodsByAmmo.TryGetValue(ammo, out var list) ? list.Sum(i => i.Count) : 0;

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

            Weapons = info.Modules.OfType<Weapon>().Select(i => new WeaponInfo {Weapon = i}).ToList();
            weapons = Weapons.ToDictionary(i => i.Weapon);

            foreach (var weaponInfo in Weapons)
            {
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
        }

        public IEnumerator<WeaponInfo> GetEnumerator()
        {
            return Weapons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
