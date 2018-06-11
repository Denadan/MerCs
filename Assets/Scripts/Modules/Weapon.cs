using System.Text;
using UnityEngine;
using System.Linq;

namespace Mercs
{
    public partial class Weapon : ScriptableObject, IModule
    {
        public WeaponTemplate Template;

        public string ShortName;
        public string Name;

        public Upgrade[] Upgrades;


        private float weight;

        public float Weight => weight;
        public SlotSize Slot => Template.slots;

        public void ApplyUpgrade()
        {
            if (Template == null)
                return;
            weight = Template.Weight + Template.Upgrades.Sum(Upgrades.Find(UpgradeType.Weight), Template.Weight);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Weapon:\n");
            sb.Append($"{Name}({ShortName})\n");
            ApplyUpgrade();
            if (Template != null)
            {
                sb.Append($"Class: {Template.Type}({Template.DamageType})\n");
                sb.Append($"Weight: {weight:F2}\n");
                if (Template.Ammo != AmmoType.None) 
                    sb.Append($"Ammo: {Template.Ammo}\n");
                sb.Append($"Shots: {Template.Shots} {(Template.VariableShots ? '~' : ' ')}\n");
                sb.Append($"Damage: D:{Template.Damage:F2} H:{Template.Heat:F2} S:{Template.Stability:F2}\n");

            }
            else
            {
                sb.Append("No Template!");
            }

            return sb.ToString();
        }
#endif
    }
}