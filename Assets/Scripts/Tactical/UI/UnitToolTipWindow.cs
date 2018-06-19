#pragma warning disable 649

using System.Collections.Generic;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class UnitToolTipWindow : MonoBehaviour, IUnitDamaged, IUnitEvent, IUnitStateWindow, IPointerDownHandler, IVisionChanged
    {

        [SerializeField] private GameObject VissiblePartPrefab;
        [SerializeField] private GameObject HiddenPartPrefab;

        [SerializeField] private Transform PartsHolder;
        [SerializeField] private Text CaptionText;

        [SerializeField] private ShieldSliderInfo Shield;
        [SerializeField] private HpSliderInfo Armor;
        [SerializeField] private HpSliderInfo ArmorF;
        [SerializeField] private HpSliderInfo ArmorB;
        [SerializeField] private HpSliderInfo Structure;

        [SerializeField] private Transform WeaponContainer;
        [SerializeField] private Transform RadarContainer;
        [SerializeField] private GameObject StringPrefab;
        [SerializeField] private CanvasGroup Group;
        [SerializeField] private GameObject Window;

        [SerializeField] private Sprite RadarSprite;
        [SerializeField] private Sprite VisualSprite;
        [SerializeField] private Sprite LockedSprite;


        private UnitInfo current_unit, selected_unit;
        private UnitInfo info;
        private Visibility.Level level;

        private Parts selected;

        public void SetUnit(UnitInfo info)
        {
            this.info = info;
            foreach (Transform child in PartsHolder)
                Destroy(child.gameObject);

            level = level == Visibility.Level.Friendly ?
                Visibility.Level.Friendly :
                info.CurrentVision;

            if (level > Visibility.Level.Sensor)
            {
                CaptionText.text = $"{info.PilotName}({CONST.Class(info.Weight)} {info.Type})";
                foreach (var part in info.UnitHP.AllParts)
                {
                    var p = Instantiate(level == Visibility.Level.Visual ? HiddenPartPrefab : VissiblePartPrefab, PartsHolder, false).GetComponent<UnitPartStateBase>();
                    p.Init(this, part, info);
                }
            }
            else
            {
                CaptionText.text = $"Unknown {CONST.Class(info.Weight)} {info.Type}";
            }

            foreach (Transform child in WeaponContainer)
                Destroy(child.gameObject);

            Shield.Show(level);

            Structure.Show(level);

            selected = Parts.None;

            ShowRadarData();
            UnitDamage(info, info.UnitHP);
        }

        private void ShowPart(Parts part)
        {
            foreach (Transform child in WeaponContainer)
                Destroy(child.gameObject);

            if (level <= Visibility.Level.Sensor)
                return;

            var list = part == Parts.None ? info.Modules : info.Modules[part];
            var capt = Instantiate(StringPrefab, WeaponContainer).GetComponent<IconText>();
            capt.Text = part == Parts.None ? "Modules" : $"{part} modules";


            foreach (var module in list)
            {
                if (level == Visibility.Level.Visual)
                {
                    if (CONST.Visible(module))
                    {
                        var str = Instantiate(StringPrefab, WeaponContainer).GetComponent<IconText>();
                        str.Icon = module.Icon;
                        str.Text = module.BaseName;
                    }
                }
                else
                {
                    if (selected != Parts.None || module.ModType == Items.ModuleType.Weapon || module.ModType == Items.ModuleType.AmmoPod)
                    {
                        var str = Instantiate(StringPrefab, WeaponContainer).GetComponent<IconText>();
                        str.Icon = module.Icon;
                        str.Text = module.ShortName;
                    }
                }
            }
        }

        public void ShowRadarData()
        {
            foreach (Transform child in RadarContainer)
                Destroy(child.gameObject);

            if (level == Visibility.Level.Friendly)
                return;

            var list = TacticalController.Instance.Vision.GetTo(info);
            foreach (var item in list)
            {
                if (item.level > Visibility.Level.None)
                {
                    var str = Instantiate(StringPrefab, RadarContainer).GetComponent<IconText>();
                    str.Text = item.target.PilotName;
                    switch (item.level)
                    {
                        case Visibility.Level.Sensor:
                            str.Icon = RadarSprite;
                            break;
                        case Visibility.Level.Visual:
                            str.Icon = VisualSprite;
                            break;
                        case Visibility.Level.Scanned:
                            str.Icon = LockedSprite;
                            break;
                    }
                }
            }
        }

        public void ShowPartDetail(Parts part)
        {
            selected = part;
            UnitDamage(info, info.UnitHP);
        }

        public void HidePartDetail()
        {
            selected = Parts.None;
            UnitDamage(info, info.UnitHP);
        }

        public void VisionChanged(UnitInfo unit, Visibility.Level l)
        {
            if (info != unit)
                return;

            if (level != Visibility.Level.Friendly && l != level)
                if (l == Visibility.Level.None)
                {
                    selected_unit = null;
                    Window.SetActive(false);
                    Group.interactable = false;
                    Group.blocksRaycasts = false;
                }
                else
                {
                    level = l;
                    SetUnit(info);
                }
        }

        public void UnitDamage(UnitInfo unit, UnitHp hp)
        {
            if (unit != info)
                return;

            Shield.MaxValue = hp.MaxShield;
            Shield.Value = hp.Shield;

            if (selected == Parts.None)
            {
                Armor.Show(level);
                ArmorB.Hide();
                ArmorF.Hide();


                Structure.MaxValue = hp.MaxStructure;
                Structure.Value = hp.TotalStructure;

                Armor.MaxValue = hp.MaxArmor;
                Armor.Value = hp.TotalArmor;
            }
            else
            {
                var max = hp.MaxHp(selected);
                var current = hp.CurrentHp(selected);

                if (level <= Visibility.Level.Visual)
                {
                    Armor.Show(level);
                    ArmorF.Hide();
                    ArmorB.Hide();

                    Armor.MaxValue = max.has_back_armor ? max.armor + max.back_armor : max.armor;
                    Structure.MaxValue = max.structure;

                    Armor.Value = current.has_back_armor ? current.armor + current.back_armor : current.armor;
                    Structure.Value = current.structure;
                }
                else
                {
                    Structure.MaxValue = max.structure;
                    Structure.Value = current.structure;

                    if (max.has_back_armor)
                    {
                        ArmorF.Show(level);
                        ArmorB.Show(level);
                        Armor.Hide();

                        ArmorF.MaxValue = max.armor;
                        ArmorF.Value = current.armor;

                        ArmorB.MaxValue = max.back_armor;
                        ArmorB.Value = current.back_armor;
                    }
                    else
                    {
                        ArmorF.Hide();
                        ArmorB.Hide();
                        Armor.Show(level);
                        Armor.MaxValue = max.armor;
                        Armor.Value = current.armor;
                    }
                }
            }
            ShowPart(selected);
        }

        public void MouseUnitEnter(UnitInfo unit)
        {
            var l = unit.Faction == GameController.Instance.PlayerFaction ?
                Visibility.Level.Friendly : unit.CurrentVision;
            if (l < Visibility.Level.None)
                return;

            Group.interactable = false;
            Group.blocksRaycasts = false;
            Window.SetActive(true);
            level = l;
            SetUnit(unit);
            current_unit = unit;
        }

        public void MouseUnitLeave(UnitInfo unit)
        {
            if (current_unit == unit)
            {
                current_unit = null;

                if (selected_unit == null)
                {
                    Window.SetActive(false);
                }
                else
                {
                    level = selected_unit.Faction == GameController.Instance.PlayerFaction ?
                        Visibility.Level.Friendly : selected_unit.CurrentVision;

                    Window.SetActive(true);
                    Group.interactable = true;
                    Group.blocksRaycasts = true;
                    SetUnit(selected_unit);
                }
            }
        }

        public void MouseUnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            var l = unit.Faction == GameController.Instance.PlayerFaction ?
                Visibility.Level.Friendly : unit.CurrentVision;

            if (button == PointerEventData.InputButton.Right && l != Visibility.Level.None)
            {
                selected_unit = info;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                selected_unit = null;
                Window.SetActive(false);
                Group.interactable = false;
                Group.blocksRaycasts = false;
            }
        }
    }
}