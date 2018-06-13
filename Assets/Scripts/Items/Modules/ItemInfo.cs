using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs.Items
{
    /// <summary>
    /// базовый класс для любого предмета
    /// </summary>
    /// <typeparam name="T">тип шаблона предмета</typeparam>
    public abstract class ItemInfo<T> : ScriptableObject
        where T : ItemTemplate
    {
        /// <summary>
        /// шаблон предмета
        /// </summary>
        public T Template;
        /// <summary>
        /// список модификаций
        /// </summary>
        public Upgrade Upgrade;

        /// <summary>
        /// молное имя
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// краткое имя для боя
        /// </summary>
        public string ShortName { get; protected set; }

        /// <summary>
        /// имя темплейта для неопределенных модулей
        /// </summary>
        public string BaseName { get; protected set; }

        public virtual Sprite Icon => Template.Icon;

        /// <summary>
        /// если не равно "" - игнорирование сгенерированного имени
        /// </summary>
        public string ItemName;
        /// <summary>
        /// если не равно "" - игнорирование сгенерированного краткого имени
        /// </summary>
        public string ItemShortName;
        
        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа предмета не верна!
        /// </summary>
        public virtual void ApplyUpgrade()
        {
            if (Template == null || Template == null)
                return;
            Name = ItemName != "" ? ItemName : string.Format(Upgrade.NameTemplate, Template.Name);
            ShortName = ItemShortName != "" ? ItemShortName : string.Format(Upgrade.ShortNameTemplate, Template.ShortName);
            BaseName = Template.ShortName;
        }
        /// <summary>
        /// посчтиать улучшения параметра
        /// </summary>
        /// <param name="type"></param>
        /// <param name="base_value"></param>
        /// <returns></returns>
        protected float upgrade(UpgradeType type, float base_value)
        {
            return Template.Upgrades.Sum(Upgrade[type], base_value);

        }

        /// <summary>
        /// вытянуть Enum из апгрейдов
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        protected TEnum upgrade<TEnum>(UpgradeType type)
        {
            if (Template.Upgrades.All(i => i.Type != type))
                return default(TEnum);

            var item = Upgrade[type];
            if (item == null)
                return default(TEnum);

            return  (TEnum)(object)(item.value);
        }

        /// <summary>
        /// почтитать сумму апгредйов без базового значения
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected float upgrade(UpgradeType type)
        {
            return Template.Upgrades.Sum(Upgrade[type]);
        }


#if UNITY_EDITOR
        public override string ToString()
        {
            ApplyUpgrade();
            return $"{Name} ({ShortName})";
        }
#endif
    }
}
