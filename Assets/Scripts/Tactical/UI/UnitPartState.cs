#pragma warning disable 649

using Denadan.UI;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{


    public class UnitPartState : UnitPartStateBase
    {
        [SerializeField]private Text PartText;
        [SerializeField]private Image PartStateBack;


        [SerializeField] private BasicBackSlider StructureSlider;
        [SerializeField] private BasicBackSlider ArmorFullSlider;
        [SerializeField] private BasicBackSlider ArmorFSlider;
        [SerializeField] private BasicSlider ArmorBSlider;



        protected bool has_structure;

        public override void Init(IUnitStateWindow window, Parts part, UnitHp hp)
        {

            base.Init(window, part, hp);

            PartText.text = part.ToString();

            if (hp.PartDestroyed(part))
            {
                PartStateBack.color = Color.black;
                StructureSlider.Hide();
                ArmorFSlider.Hide();
                ArmorBSlider.Hide();
                ArmorFullSlider.Hide();

            }
            else
            {
                var max = hp.MaxHp(part);
                has_structure = max.structure > 0;
                if (has_structure)
                {
                    StructureSlider.Show();
                    StructureSlider.MaxValue = max.structure;
                }
                else
                    StructureSlider.Hide();

                if (max.has_back_armor)
                {
                    ArmorFullSlider.Hide();
                    ArmorFSlider.Show();
                    ArmorBSlider.Show();
                    ArmorBSlider.MaxValue = max.back_armor;
                    ArmorFSlider.MaxValue = max.armor;
                }
                else
                {
                    ArmorFullSlider.Show();
                    ArmorFSlider.Hide();
                    ArmorBSlider.Hide();
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
                StructureSlider.Hide();
                ArmorFSlider.Hide();
                ArmorBSlider.Hide();
                ArmorFullSlider.Hide();
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