using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Reactor", menuName = "MerCs/Module/Reactor")]
    public class Reactor : ModuleInfo<ReactorTemplate>
    {
        public int Size;

        private float _weight;

        public override float Weight => _weight;
        public int FullSize => Size + Template.BaseSize;
        public int EngineRating { get; private set; }
        public float HeatDissipation { get; private set; }
        public int CentralSlot { get; private set; }
        public int SideSlot { get; private set; }

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

            SideSlot = Template.BaseExtend + (int) (Size * Template.SlotExtend);
            CentralSlot = FullSize - SideSlot * 2;

        }

        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            if (Size < 0)
                return "WRONG SIZE!";

            StringBuilder sb = new StringBuilder();

            sb.Append("Reactor\n");
            sb.Append(base.ToString());
            sb.Append($"Size: {SideSlot}-{CentralSlot}-{SideSlot}");
            sb.Append($"\nER: {EngineRating}, Heat: {HeatDissipation}\n");
            
            return sb.ToString();
        }
    }
}