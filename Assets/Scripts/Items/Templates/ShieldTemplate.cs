using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Shield", menuName = "MerCs/Template/Shield")]
    public class ShieldTemplate : ModuleTemplate
    {
        /// <summary>
        /// объем щита
        /// </summary>
        public float Shield;
        /// <summary>
        /// восстановление щита
        /// </summary>
        public float ShieldRegen;
        /// <summary>
        /// нагрев
        /// </summary>
        public float Heat;
        /// <summary>
        /// задержка перезарядки
        /// </summary>
        internal int ShieldDelay;
    }
}