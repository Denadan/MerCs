using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "HeatSink", menuName = "MerCs/Module/HeatSink")]
    public class HeatSink : ModuleInfo<HeatSinkTemplate>
    {
        public float HeatDissipation { get; private set; }
        public float HeatCapacity { get; private set; }


        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            HeatCapacity = upgrade(UpgradeType.HeatCapacity, Template.HeatCapacity);
            HeatDissipation = upgrade(UpgradeType.Heat, Template.HeatDissipation);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append("HeatSink\n");
            sb.Append(base.ToString());
            sb.Append($"\nHeat: {HeatCapacity:F2}-{HeatDissipation:F2}\n");

            return sb.ToString();
        }
#endif 
    }
}
