#pragma warning disable 649

using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{


    public class UnitPartState : UnitPartStateBase
    {
        [SerializeField]
        private Text PartText;
        [SerializeField]
        private Image PartStateBack;
        [SerializeField]
        private GameObject StructureBar;
        [SerializeField]
        private GameObject ArmorFullBar;
        [SerializeField]
        private GameObject ArmorFBar;
        [SerializeField]
        private GameObject ArmorBBar;

        [SerializeField] private UnitPartStateSlider StructureSlider;
        [SerializeField] private UnitPartStateSlider ArmorFullSlider;
        [SerializeField] private UnitPartStateSlider ArmorFSlider;
        [SerializeField] private UnitPartStateSlider ArmorBSlider;


        protected bool has_structure;

        public override void Init(IUnitStateWindow window, Parts part, UnitHp hp)
        {

            base.Init(window, part, hp);

            PartText.text = part.ToString();

            if (hp.PartDestroyed(part))
            {
                PartStateBack.color = Color.black;
                StructureBar.SetActive(false);
                ArmorFullBar.SetActive(false);
                ArmorFBar.SetActive(false);
                ArmorBBar.SetActive(false);
            }
            else
            {
                var max = hp.MaxHp(part);
                has_structure = max.structure > 0;
                if (has_structure)
                {
                    StructureBar.SetActive(true);
                    StructureSlider.MaxValue = max.structure;
                }
                else
                    StructureBar.SetActive(false);

                if (max.has_back_armor)
                {
                    ArmorFullBar.SetActive(false);
                    ArmorFBar.SetActive(true);
                    ArmorBBar.SetActive(true);
                    ArmorBSlider.MaxValue = max.back_armor;
                    ArmorFSlider.MaxValue = max.armor;
                }
                else
                {
                    ArmorFullBar.SetActive(true);
                    ArmorFBar.SetActive(false);
                    ArmorBBar.SetActive(false);
                    ArmorFullSlider.MaxValue = max.armor;
                }
                UpdateValues(hp);
            }
        }


        protected  override void UpdateValues(UnitHp hp)
        {
            if (hp.PartDestroyed(part))
            {
                PartStateBack.color = Color.black;
                StructureBar.SetActive(false);
                ArmorFullBar.SetActive(false);
                ArmorFBar.SetActive(false);
                ArmorBBar.SetActive(false);
            }
            else
            {
                var current = hp.CurrentHp(part);

                if (current.has_back_armor)
                {
                    ArmorFSlider.Value = current.armor;
                    ArmorBSlider.Value = current.back_armor;
                }
                else
                {
                    ArmorFullSlider.Value = current.armor;
                }

                if (has_structure)
                {
                    StructureSlider.Value = current.structure;
                    PartStateBack.color = CONST.GetColor(StructureSlider.Gradient, 1);
                }
                else if (current.has_back_armor)
                    PartStateBack.color = CONST.GetColor(ArmorFSlider.Gradient, 1);
                else
                    PartStateBack.color = CONST.GetColor(ArmorFullSlider.Gradient, 1);
            }
        }
    }
}