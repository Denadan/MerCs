#pragma warning disable 649

using Denadan.UI;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class UnitSelectButton : MonoBehaviour, IPilotDamaged, IUnitDamaged
    {
        public Image unitImage;
        [SerializeField] private Text nameText;
        [SerializeField] private Text bottomText;
        [SerializeField] private PilotHPBar HpBar;
        [SerializeField] private BasicSlider Shield;
        [SerializeField] private BasicSlider Armor;
        [SerializeField] private BasicSlider Struct;
        public Image Background;

        public UnitInfo Unit;
        public Text BottomText { get => bottomText; set => bottomText = value; }

        private void Start()
        {
            if (Unit == null)
                return;
            unitImage.sprite = Unit.GFX.GetIcon();
            unitImage.material = Unit.Faction.CamoMaterial;
            nameText.text = Unit.PilotName;

            if (Unit.PilotHP == null)
            {
                Destroy(HpBar.gameObject);
                HpBar = null;
            }
            else
            {
                HpBar.MaxHP = Unit.PilotHP.MaxHp;
                HpBar.AddHP = Unit.PilotHP.AddHp;
                HpBar.HP = Unit.PilotHP.Hp;
            }

            if (Unit.UnitHP.MaxShield > 0)
                Shield.MaxValue = Unit.UnitHP.MaxShield;
            else
                Shield.Hide();

            Armor.MaxValue = Unit.UnitHP.MaxArmor;
            Struct.MaxValue = Unit.UnitHP.MaxStructure;

            UnitDamage(Unit, Unit.UnitHP);
        }


        public void PilotDamaged(UnitInfo unit, PilotHp hp)
        {
            if (this.Unit != unit)
                return;

            HpBar.AddHP = Unit.PilotHP.AddHp;
            HpBar.HP = Unit.PilotHP.Hp;

        }

        public void UnitDamage(UnitInfo unit, UnitHp hp)
        {
            if (this.Unit != unit)
                return;

            Shield.Value = hp.Shield;
            Armor.Value = hp.TotalArmor;
            Struct.Value = hp.TotalStructure;
        }
    }
}
