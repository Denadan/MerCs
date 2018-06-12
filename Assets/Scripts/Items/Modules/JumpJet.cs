using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "JumpJet", menuName = "MerCs/Module/JumpJet")]
    public class JumpJet : ModuleInfo<JumpJetTemplate>, IHeatProducer, IHeatContainer
    {
        public override ModuleType ModType => ModuleType.JumpJet;


        public float Heat { get; private set; }
        public int EngineRating { get; private set; }
        public float HeatCapacity { get; private set; }

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Heat = upgrade(UpgradeType.Heat, Template.Heat);
            EngineRating = (int)upgrade(UpgradeType.EngineRating, Template.EngineRating);
            HeatCapacity = upgrade(UpgradeType.HeatCapacity, Template.HeatCapacity);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append("JumpDrive\n");
            sb.Append(base.ToString());
            sb.Append($"\nER: {EngineRating} Heat: {Heat:F2} HeatCapacity: {HeatCapacity:F2}\n");

            return sb.ToString();
        }
#endif    
    }
}
