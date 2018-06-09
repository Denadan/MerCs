using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class UnitHiddenPartState : UnitPartStateBase
    {
        [SerializeField]
        private Text PartText;
        [SerializeField]
        private Image PartStateBack;
        [SerializeField]
        private Image StructureBar;
        [SerializeField]
        private Image ArmorFullBar;

        private float max_str, max_arm;

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