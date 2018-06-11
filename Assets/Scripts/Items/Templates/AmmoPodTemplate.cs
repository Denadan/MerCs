using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "AmmoPod", menuName = "MerCs/Template/AmmoPod")]
    public class AmmoPodTemplate : ModuleTemplate
    {
        public AmmoType Type;

        public int Capacity;
        [Range(0, 1)]
        public float DamageTransfer = 1;
        [Range(0, 1)]
        public float DamageToHeat = 0;
        [Range(0, 1)]
        public float DamageToStability = 0;
    }
}
