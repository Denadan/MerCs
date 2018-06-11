using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercs.Items
{
    public abstract class ModuleTemplate : ItemTemplate
    {
        public float Weight;
        public SlotSize slots;
    }
}
