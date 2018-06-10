using System;
using UnityEngine;

namespace Mercs
{
    public class Weapon : ScriptableObject
    {
        [Serializable]
        public class Upgrade
        {
            public WeaponUpgradeType type;
            public int value;
        }

        public WeaponTemplate Template;

        public string ShortName;
        public string Name;

        public Upgrade[] Upgrades;
    }
}