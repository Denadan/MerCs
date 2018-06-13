
using System;

namespace Mercs.Items
{
    /// <summary>
    /// базовый класс для модуля 
    /// </summary>
    /// <typeparam name="T">тип используемого шаблона</typeparam>
    public abstract class ModuleInfo<T> : ItemInfo<T>, IModuleInfo
        where T : ModuleTemplate
    {
        [NonSerialized]
        private float weight;

        /// <summary>
        /// вес моудля
        /// </summary>
        public virtual float Weight => weight;
        /// <summary>
        /// занимаемые слоты
        /// </summary>
        public SlotSize Slot => Template.slots;
        /// <summary>
        /// максимально число критов
        /// </summary>
        public virtual int Crit { get => Template.Crits; }

        /// <summary>
        /// тип модуля
        /// </summary>
        public abstract ModuleType ModType { get; }

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            weight = upgrade(UpgradeType.Weight, Template.Weight);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            var name = base.ToString();
            return name + $"\nWeight: {Weight:F2} Size:{Slot} Crit:{Crit}";
        }
#endif 
    }
}
