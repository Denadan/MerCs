using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "MerCs/ItemUpgrade")]
    public class Upgrade : ScriptableObject
    {
        [Serializable]
        public class UItem
        {
            public UpgradeType type;
            public int value;
        }

        public Rarity Rarity;
        public String ShortNameTemplate;
        public String NameTemplate;

        public List<UItem> Upgrades = new List<UItem>();

        public UItem this[UpgradeType type] => Upgrades.FirstOrDefault(i => i.type == type);
    }
}