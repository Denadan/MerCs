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

        public override void Init(UnitStateWindow window, Parts part, UnitHp hp)
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

        private Color GetColor(float cur, float max)
        {
            if (max == 0 || cur == 0)
                return Color.black;

            switch (cur / max)
            {
                case float i when i >= 0.95f:
                    return Color.white;
                case float i when i >= 0.8f:
                    return Color.green;
                case float i when i >= 0.5f:
                    return Color.yellow;
                case float i when i > 0.25f:
                    return new Color(1f, 0.33f, 0);
                default:
                    return Color.red;
            }
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
                    StructureBar.color = GetColor(current.structure, max_str);
                ArmorFullBar.color = GetColor(current.has_back_armor ? current.armor + current.back_armor : current.armor, max_arm);
            }
        }
    }
}