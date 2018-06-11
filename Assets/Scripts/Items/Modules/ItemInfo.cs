﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs.Items
{
    public abstract class ItemInfo<T> : ScriptableObject
        where T : ItemTemplate
    {
        public T Template;
        public Upgrade Upgrade;

        public string Name { get; protected set; }
        public string ShortName { get; protected set; }

        public string ItemName;
        public string ItemShortName;

        public virtual void ApplyUpgrade()
        {
            if (Template == null || Template == null)
                return;
            Name = ItemName != "" ? ItemName : string.Format(Upgrade.NameTemplate, Template.Name);
            ShortName = ItemShortName != "" ? ItemShortName : string.Format(Upgrade.ShortNameTemplate, Template.ShortName);
        }

        protected float upgrade(UpgradeType type, float base_value)
        {
            return Template.Upgrades.Sum(Upgrade[type], base_value);

        }

        protected TEnum upgrade<TEnum>(UpgradeType type)
        {
            if (!Template.Upgrades.Any(i => i.Type == type))
                return default(TEnum);

            var item = Upgrade[type];
            if (item == null)
                return default(TEnum);

            return  (TEnum)(object)(item.value);
        }

        protected float upgrade(UpgradeType type)
        {
            return Template.Upgrades.Sum(Upgrade[type]);
        }


#if UNITY_EDITOR
        public override string ToString()
        {
            ApplyUpgrade();
            return $"{Name} ({ShortName})";
        }
#endif
    }
}