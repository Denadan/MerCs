#pragma warning disable 649

using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class UnitPartState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPartDamaged
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

        private UnitStateWindow window;
        private Parts part;
        private UnitHp hp;
        private bool has_structure;

 //       private string debug;

        public void Init(UnitStateWindow window, Parts part, UnitHp hp)
        {
            this.window = window;
            this.part = part;
            this.hp = hp;


//            debug = part.ToString();

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
                //debug += "\nmax:" + max.ToString(); 
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
                PartDamaged(hp);
  //              UnityEngine.Debug.Log(debug);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            window.ShowPartDetail(part);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            window.HidePartDetail();
        }

        public void PartDamaged(UnitHp hp)
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

                float gradient = 0;
                if (has_structure)
                {
                    StructureSlider.Value = current.structure;
                    gradient = StructureSlider.Gradient;
                }
                else
                    if (current.has_back_armor)
                        gradient = ArmorFSlider.Gradient;
                    else
                        gradient = ArmorFullSlider.Gradient;

                if (gradient == 0)
                    PartStateBack.color = Color.black;
                else if (gradient >= 1)
                    PartStateBack.color = Color.white;
                else if (gradient > 0.9f)
                    PartStateBack.color = Color.Lerp(Color.green, Color.white, (gradient - 0.9f) / 0.1f);
                else if (gradient > 0.5f)
                    PartStateBack.color = Color.Lerp(Color.yellow, Color.green, (gradient - 0.5f) / 0.4f);
                else if (gradient > 0.1f)
                    PartStateBack.color = Color.Lerp(Color.red, Color.yellow, (gradient - 0.1f) / 0.7f);
                else
                    PartStateBack.color = Color.Lerp(Color.black, Color.red, gradient/ 0.1f);

            }
        }
    }
}