using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "MerCs/Module/Weapon")]
    public partial class Weapon : ModuleInfo<WeaponTemplate>, IHeatProducer
    {
        public override ModuleType ModType => ModuleType.Weapon;

        public float DamageMult { get; private set; }
        public float Damage => EDamage + BDamage + MDamage;

        public float BDamage => Template.StockAmmo.BDamage * DamageMult;
        public float EDamage => Template.StockAmmo.EDamage * DamageMult;
        public float MDamage => Template.StockAmmo.MDamage * DamageMult;

        public float HeatDamage => Template.StockAmmo.HeatDamage * DamageMult;
        public float StabDamage => Template.StockAmmo.StabDamage * DamageMult;

        public float MinRange { get; private set; }
        public float Optimal { get; private set; }
        public float Falloff { get; private set; }

        public int Shots { get; private set; }

        public float HeatForShot { get; private set; }

        public float Heat => Template.VariableShots ? Shots * HeatForShot : HeatForShot;

        public override void ApplyUpgrade()
        {
            if (Template == null || Template == null || Template.StockAmmo == null)
                return;

            base.ApplyUpgrade();
            DamageMult = upgrade(UpgradeType.Damage, Template.DamageMult);
            MinRange = upgrade(UpgradeType.Range, Template.MinRange);
            Optimal = upgrade(UpgradeType.Range, Template.Optimal);
            Falloff = upgrade(UpgradeType.Range, Template.Falloff);
            HeatForShot = upgrade(UpgradeType.Heat, Template.HeatForShot);
            Shots = (int)upgrade(UpgradeType.Shots, Template.Shots);

            Name = Name.Replace("%SHOT%", Shots.ToString());
            ShortName = ShortName.Replace("%SHOT%", Shots.ToString());
            BaseName = BaseName.Replace("%SHOT%", Shots.ToString());
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null || Template.StockAmmo == null)
                return "NO TEMPLATE!";


            StringBuilder sb = new StringBuilder();
            sb.Append("Weapon\n");
            sb.Append(base.ToString());
            sb.Append($"\nClass: {Template.Type}({Template.DamageType})\n");
            //if (Template.Ammo != AmmoType.None)
            //{
            sb.Append($"================ AMMO =============\n");
            sb.Append(Template.StockAmmo.ToString());
            sb.Append($"\n==================================\n");
            //}

            sb.Append($"Damage multuplier: {DamageMult:F2}\n");
            sb.Append($"Damage: {Damage:F2} (E:{EDamage:F2} B:{BDamage:F2} M:{MDamage:F2})\n");
            sb.Append($"Heat: {HeatDamage:F2}  Stab:{StabDamage:F2}\n");
            sb.Append($"Shots: {Shots} {(Template.VariableShots ? '~' : ' ')}\n");
            sb.Append($"Heat Generate:{HeatForShot:F2}\n");
            sb.Append($"Range: {MinRange}/{Optimal}/{Falloff}\n");
            sb.Append($"FireMode:");
            if (Template.DirectFire)
                sb.Append(" direct");
            if (Template.IndirectFire)
                sb.Append(" indirect");
            if (Template.SupportFire)
                sb.Append(" support");

            return sb.ToString();
        }
#endif
    }
}