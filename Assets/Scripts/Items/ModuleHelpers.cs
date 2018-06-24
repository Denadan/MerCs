using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs.Items
{
    public static class ModuleHelpers
    {
        public static float Sum(this IEnumerable<UpgradeTemplate> list, Upgrade.UItem upgrade, float base_value)
        {
            if (base_value == 0)
                return 0;

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

        public static float Sum(this IEnumerable<UpgradeTemplate> list, Upgrade.UItem upgrade)
        {

            float val = 0;

            if (list == null || upgrade == null)
                return 0;
            var item = list.FirstOrDefault(i => i.Type == upgrade.type);

            if (item == null)
                return 0;

            int value = upgrade.value < -item.Min ? -item.Min : upgrade.value;
            value = value > item.Max ? item.Max : value;


            if (value > 0)
            {
                if (item.AddValue != 0)
                    val += item.AddValue * value;
            }
            else if (value < 0)
            {
                if (item.SubValue != 0)
                    val += item.SubValue * value;
            }

            return val;
        }

        public static int SumNZ(this IEnumerable<UpgradeTemplate> list, Upgrade.UItem upgrade, int base_value)
        {

            if (list == null || upgrade == null)
                return base_value;
            var item = list.FirstOrDefault(i => i.Type == upgrade.type);
            if (item == null)
                return base_value;

            int value = upgrade.value < -item.Min ? -item.Min : upgrade.value;
            value = value > item.Max ? item.Max : value;


            if (value != 0)
            {
                if (item.AddValue != 0)
                    base_value += (int)item.AddValue * value;
            }
            else if (value < 0)
            {
                if (item.SubValue != 0)
                    base_value += (int)item.SubValue * value;
            }

            return base_value;
        }

    }
}