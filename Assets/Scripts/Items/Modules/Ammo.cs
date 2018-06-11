using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Ammo", menuName = "MerCs/Module/Ammo")]
    public class Ammo : ItemInfo<AmmoTemplate>
    {
        public float Damage => EDamage + MDamage + BDamage;

        public float EDamage { get; private set; }
        public float MDamage { get; private set; }
        public float BDamage { get; private set; }

        public float HeatDamage { get; private set; }
        public float StabDamage { get; private set; }

        public float RangeMod { get; private set; }
        public float AimMod { get; private set; }

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            EDamage = upgrade(UpgradeType.Damage, Template.EDamage);
            MDamage = upgrade(UpgradeType.Damage, Template.MDamage);
            BDamage = upgrade(UpgradeType.Damage, Template.BDamage);
            HeatDamage = upgrade(UpgradeType.HeatDamage, Template.HeatDamage);
            StabDamage = upgrade(UpgradeType.StabDamage, Template.StabDamage);

            RangeMod = upgrade(UpgradeType.Range, Template.RangeMod);
            AimMod = upgrade(UpgradeType.Accuracy, Template.AimMod);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();
            sb.Append("Ammo\n");
            sb.Append(base.ToString());
            sb.Append($"\nDamage: {Damage:F2} (E:{EDamage:F2} B:{BDamage:F2} M:{MDamage:F2})\n");
            sb.Append($"Heat: {HeatDamage:F2}  Stab:{StabDamage:F2}\n");
            sb.Append($"RangeMod: {RangeMod:P}  AimMod:{AimMod:P}");

            return sb.ToString();
        }
#endif 
    }
}
