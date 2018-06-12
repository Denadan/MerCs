using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "ShieldGenerator", menuName = "MerCs/Module/ShieldGenerator")]
    public class ShieldGenerator : ModuleInfo<ShieldTemplate>, IShield, IShieldRegenerator, IHeatProducer
    {
        public override ModuleType ModType => ModuleType.Shield;
        public float Shield { get; private set; }
        public float ShieldRegen { get; private set; }
        public float Heat { get; private set; }

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Shield = upgrade(UpgradeType.Stability, Template.Shield);
            ShieldRegen = upgrade(UpgradeType.StabilityRestore, Template.ShieldRegen);
            Heat = upgrade(UpgradeType.MoveMod, Template.Heat);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append($"Shield)\n");
            sb.Append(base.ToString());
            sb.Append($"\nShield: {Shield} + {ShieldRegen}");
            sb.Append($"\nHeat: {Heat}\n");


            return sb.ToString();
        }
#endif 
    }
}