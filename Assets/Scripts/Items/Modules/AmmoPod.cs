using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "AmmoPod", menuName = "MerCs/Module/AmmoPod")]
    public class AmmoPod : ModuleInfo<AmmoPodTemplate>
    {
        public override ModuleType ModType => ModuleType.AmmoPod;

        public float Capacity { get; private set; }
        public float DamageTransfer { get; private set; }
        public float DamageToHeat { get; private set; }
        public float DamageToStab { get; private set; }
        public AmmoType Type => Template.Type;

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Capacity = upgrade(UpgradeType.Capacity, Template.Capacity);
            DamageTransfer = upgrade(UpgradeType.DamageTransfer, Template.DamageTransfer);
            DamageToHeat = upgrade(UpgradeType.DamageToHeat, Template.DamageToHeat);
            DamageToStab = upgrade(UpgradeType.DamageToStab, Template.DamageToStability);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();
            sb.Append("AmmoPod\n");
            sb.Append(base.ToString());
            sb.Append($"\nCapacity: {Capacity:F3}\n");
            sb.Append($"Transfer: {DamageTransfer:P}, to heat:{DamageToHeat:P}, to stab: {DamageToStab}");
            
            return sb.ToString();
        }
#endif 
    }
}
