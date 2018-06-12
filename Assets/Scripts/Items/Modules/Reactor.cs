using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Reactor", menuName = "MerCs/Module/Reactor")]
    public class Reactor : ModuleInfo<ReactorTemplate>, IHeatContainer, IHeatDissipator
    {
        public int Size;

        private float _weight;
        private int _crit;

        public override ModuleType ModType => ModuleType.Reactor;

        public override float Weight => _weight;
        public int FullSize => Size + Template.BaseSize;
        public int EngineRating { get; private set; }
        public float HeatDissipation { get; private set; }
        public int CentralSlot { get; private set; }
        public int SideSlot { get; private set; }
        public float HeatCapacity { get; private set; }
        public override int Crit => _crit;

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            _weight = Template.BaseWeight + Size * (Size * Template.SlotAddWeight + Template.SlotWeight);
            _weight = upgrade(UpgradeType.Weight, _weight);

            Name = Name.Replace("%SIZE%", (Size + Template.BaseSize).ToString());
            ShortName = ShortName.Replace("%SIZE%", (Size + Template.BaseSize).ToString());
            var er = Template.BaseEngineRating + Size * (Template.SlotEngineRating - Template.SlotSubEr * Size);
            EngineRating = (int)upgrade(UpgradeType.EngineRating, er);

            HeatDissipation = upgrade(UpgradeType.Heat, Template.BaseHeatSink + Template.SlotHeatSink * Size);
            HeatCapacity = upgrade(UpgradeType.HeatCapacity, Template.BaseHeatCapacity + Size * Template.SlotHeatCapacity);
            
            SideSlot = Template.BaseExtend + (int) (Size * Template.SlotExtend);
            CentralSlot = FullSize - SideSlot * 2;

            _crit = Template.BaseCrit + Template.ExtendSlotCrit * SideSlot + (int)(CentralSlot * Template.CentralSlotCrit);
            if (_crit < Template.Crits)
                _crit = Template.Crits;
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            if (Size < 0)
                return "WRONG SIZE!";

            StringBuilder sb = new StringBuilder();

            sb.Append("Reactor\n");
            sb.Append(base.ToString());
            sb.Append($"\nSize: {SideSlot}-{CentralSlot}-{SideSlot}");
            sb.Append($"\nER: {EngineRating}, Heat: {HeatCapacity:F2}-{HeatDissipation:F2}\n");
            

            return sb.ToString();
        }
#endif 
    }
}