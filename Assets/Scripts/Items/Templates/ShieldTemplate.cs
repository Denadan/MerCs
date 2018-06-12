using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Shield", menuName = "MerCs/Template/Shield")]
    public class ShieldTemplate : ModuleTemplate
    {
        public float Shield;
        public float ShieldRegen;
        public float Heat;
    }
}