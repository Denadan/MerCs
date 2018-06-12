using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "JumpJet", menuName = "MerCs/Module/JumpJet")]
    public class JumpJet : ModuleInfo<JumpJetTemplate>
    {
        public override ModuleType ModType => ModuleType.JumpJet;


        public float Heat { get; private set; }
        public int EngineRating { get; private set; }


        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Heat = upgrade(UpgradeType.HeatCapacity, Template.Heat);
            EngineRating = (int)upgrade(UpgradeType.Heat, Template.EngineRating);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append("JumpDrive\n");
            sb.Append(base.ToString());
            sb.Append($"\nER: {EngineRating} Heat: {Heat:F2}\n");

            return sb.ToString();
        }
#endif    
    }
}
