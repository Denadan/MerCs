using System.Collections.Generic;
using System.Linq;

namespace Mercs.Tactical.Buffs
{
    public static class BuffHelper
    {
        public static float SumBuffs(this IEnumerable<BuffDescriptor> items, BuffType type)
        {
            var sum = items.Where(i => i.Type == type && i.Stackable).DefaultIfEmpty().Sum(i => i?.Value ?? 0);
            var max = items.Where(i => i.Type == type && !i.Stackable).DefaultIfEmpty().Max(i => i?.Value ?? 0);

            return max > sum ? max : sum;
        }
    }
}