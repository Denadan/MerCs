using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    /// <summary>
    /// контейнер для боеприпасов
    /// </summary>
    [CreateAssetMenu(fileName = "AmmoPod", menuName = "MerCs/Module/AmmoPod")]
    public class AmmoPod : ModuleInfo<AmmoPodTemplate>
    {
        public override ModuleType ModType => ModuleType.AmmoPod;

        /// <summary>
        /// вместимость контейнера
        /// </summary>
        public int Capacity { get; private set; }
        /// <summary>
        /// процент урона при взрыве 
        /// </summary>
        public float DamageTransfer { get; private set; }
        /// <summary>
        /// процент урона переводимый в перегрев
        /// </summary>
        public float DamageToHeat { get; private set; }
        /// <summary>
        /// процент урона переводимый в стабильность
        /// </summary>
        public float DamageToStab { get; private set; }
        /// <summary>
        /// тип боеприпасов
        /// </summary>
        public AmmoType Type => Template.Type;
        
        #region game_data
        /// <summary>
        /// боеприпасы в поде
        /// </summary>
        public Ammo LoadedAmmo { get; set; }
        /// <summary>
        /// оставшиеся боеприпасы
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// приоритет использования одинаковых подов
        /// </summary>
        public int Priority { get; set; }
        #endregion

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Capacity = (int)upgrade(UpgradeType.Capacity, Template.Capacity);
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
