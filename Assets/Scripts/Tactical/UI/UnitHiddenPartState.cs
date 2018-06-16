#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    /// <summary>
    ///отображает приблизительная информация о части
    /// </summary>
    public class UnitHiddenPartState : UnitPartStateBase
    {
        /// <summary>
        /// название части
        /// </summary>
        [SerializeField]private Text PartText;
        /// <summary>
        /// состояние части
        /// </summary>
        [SerializeField]private Image PartStateBack;
        /// <summary>
        /// состояние структуры
        /// </summary>
        [SerializeField]private Image StructureBar;
        /// <summary>
        /// состояние брони
        /// </summary>
        [SerializeField]private Image ArmorFullBar;

        private float max_str, max_arm;

        /// <summary>
        /// инициализация
        /// </summary>
        /// <param name="window"></param>
        /// <param name="part"></param>
        /// <param name="hp"></param>
        public override void Init(IUnitStateWindow window, Parts part, UnitHp hp)
        {
            base.Init(window, part, hp);

            var max = hp.MaxHp(part);
            PartText.text = part.ToString();

            max_str = max.structure;
            if (max_str > 0)
                StructureBar.gameObject.SetActive(true);
            else
                StructureBar.gameObject.SetActive(false);
            ArmorFullBar.gameObject.SetActive(true);
            max_arm = max.has_back_armor ? max.armor + max.back_armor : max.armor;
            UpdateValues(hp);
        }
        /// <summary>
        /// получение урона - обновление данных
        /// </summary>
        /// <param name="hp"></param>
        protected override void UpdateValues(UnitHp hp)
        {
            if (hp.PartDestroyed(part))
            {
                PartStateBack.color = Color.black;
                StructureBar.gameObject.SetActive(true);
                ArmorFullBar.gameObject.SetActive(true);
            }
            else
            {
                var current = hp.CurrentHp(part);
                if (max_str > 0)
                    StructureBar.color = CONST.GetColor(current.structure, max_str);
                ArmorFullBar.color = CONST.GetColor(current.has_back_armor ? current.armor + current.back_armor : current.armor, max_arm);
            }
        }
    }
}