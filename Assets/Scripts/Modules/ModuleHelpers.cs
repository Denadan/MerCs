using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs
{
    public static class ModuleHelpers
    {
        public static float Sum(this IEnumerable<UpgradeTemplate> list, Upgrade upgrade, float base_value)
        {
            if (list == null || upgrade == null)
                return base_value;
            var item = list.FirstOrDefault(i => i.Type == upgrade.type);
            if (item == null)
                return base_value;

            int value = upgrade.value < -item.Min ? -item.Min : upgrade.value;
            value = value > item.Max ? item.Max : value;


            if (value > 0)
            {
                if (item.AddValue != 0)
                    base_value += item.AddValue * value;
                if (item.AddPercent != 0)
                    base_value *= (1 + item.AddPercent * value);
            }
            else if (value < 0)
            {
                if (item.SubValue != 0)
                    base_value += item.SubValue * value;
                if (item.SubPercent != 0)
                    base_value *= (1 + item.SubPercent * value);
            }

            return base_value;
        }

        public static Upgrade Find(this IEnumerable<Upgrade> list, UpgradeType type)
        {
            if (list == null)
                return null;
            return list.FirstOrDefault(i => i.type == type);
        }
    }
}