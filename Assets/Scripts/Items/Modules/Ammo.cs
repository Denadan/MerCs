using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs.Items
{
    /// <summary>
    /// Боеприпасы
    /// </summary>
    [CreateAssetMenu(fileName = "Ammo", menuName = "MerCs/Module/Ammo")]
    public class Ammo : ItemInfo<AmmoTemplate>
    {
        public AmmoType Type => Template.Type;

        /// <summary>
        /// базовый урон 
        /// </summary>
        public float Damage => EDamage + MDamage + BDamage;

        /// <summary>
        /// энергетический урон
        /// </summary>
        public float EDamage { get; private set; }
        /// <summary>
        /// взрывной урон
        /// </summary>
        public float MDamage { get; private set; }
        /// <summary>
        /// кинетический урон
        /// </summary>
        public float BDamage { get; private set; }

        /// <summary>
        /// нагрев при попадании
        /// </summary>
        public float HeatDamage { get; private set; }
        /// <summary>
        /// урон стабильности
        /// </summary>
        public float StabDamage { get; private set; }

        /// <summary>
        /// модификация дальности
        /// </summary>
        public float RangeMod { get; private set; }
        /// <summary>
        /// модификация точности
        /// </summary>
        public int AimMod { get; private set; }
        /// <summary>
        /// поглощение кинетического урона броней
        /// </summary>
        public float BallisticArmorDamage => Template.BallisticArmorDamage;

        /// <summary>
        /// система наведения для ракет
        /// </summary>
        public GuidanceSystem Guidance { get; private set; }
        /// <summary>
        /// бонус системы наведения
        /// </summary>
        public float GuidanceBonus { get; private set; }

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            EDamage = upgrade(UpgradeType.Damage, Template.EDamage);
            MDamage = upgrade(UpgradeType.Damage, Template.MDamage);
            BDamage = upgrade(UpgradeType.Damage, Template.BDamage);
            HeatDamage = upgrade(UpgradeType.HeatDamage, Template.HeatDamage);
            StabDamage = upgrade(UpgradeType.StabDamage, Template.StabDamage);

            RangeMod = upgrade(UpgradeType.Range, Template.RangeMod);
            AimMod = upgrade_non_zero(UpgradeType.Accuracy, Template.AimMod);

            Guidance = upgrade<GuidanceSystem>(UpgradeType.GuidanceSystem);
            GuidanceBonus = upgrade(UpgradeType.GuidanceBonus);
        }


#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();
            sb.Append($"Ammo ({Type})\n");
            sb.Append(base.ToString());
            sb.Append($"\nDamage: {Damage:F2} (E:{EDamage:F2} B:{BDamage:F2} M:{MDamage:F2})\n");
            sb.Append($"Heat: {HeatDamage:F2}  Stab:{StabDamage:F2}\n");
            if(Type == AmmoType.Rocket || Type == AmmoType.Missile)
            {
                sb.Append($"GuidanceSystem: {Guidance} {GuidanceBonus:+0.00;-0.00}\n");
            }

            sb.Append($"RangeMod: {RangeMod:P}  AimMod:{AimMod:P}  BvsArmor:{BallisticArmorDamage:P}");


            return sb.ToString();
        }
#endif 
    }
}
