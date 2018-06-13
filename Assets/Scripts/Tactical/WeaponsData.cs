using Mercs.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(UnitInfo))]
    public class WeaponsData : MonoBehaviour
    {
        private UnitInfo info;


        public class WeaponInfo
        {
            public Weapon Weapon { get; private set; }

            public Ammo Ammo { get; private set; }
            public bool UseAmmo => Weapon.Template.Ammo != AmmoType.None;
        }

        public List<Weapon> Weapons { get; private set; }
        public List<AmmoPod> Pods { get; private set; }

        private void Start()
        {
            info = GetComponent<UnitInfo>();

            
        }
    }
}
