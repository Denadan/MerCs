using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Ammo", menuName = "MerCs/Template/Ammo")]
    public class AmmoTemplate : ItemTemplate
    {
        public AmmoType Type;

        public float EDamage;
        public float MDamage;
        public float BDamage;

        public float HeatDamage;
        public float StabDamage;

        [Range(0.5f,2f)]
        public float RangeMod = 1;
        [Range(0.5f, 2f)]
        public float AimMod = 1;
        [Range(0.5f, 2f)]
        public float BallisticArmorDamage = 0.5f;
    }
}