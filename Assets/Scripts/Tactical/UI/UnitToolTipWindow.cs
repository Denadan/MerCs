using System.Collections.Generic;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class UnitToolTipWindow: MonoBehaviour, IUnitDamaged, IUnitEventReceiver, IUnitStateWindow, IPointerDownHandler
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

        [SerializeField] private Transform StringContainer;
        [SerializeField] private GameObject StringPrefab;
        [SerializeField] private CanvasGroup Group;
        [SerializeField] private GameObject Window;

        private Dictionary<Parts, UnitPartStateBase> parts = new Dictionary<Parts, UnitPartStateBase>();
        private UnitInfo current_unit, selected_unit;
        private UnitInfo info;

        private Parts selected;

        public void SetUnit(UnitInfo info)
        {
            this.info = info;
            parts.Clear();
            foreach (Transform child in PartsHolder)
                Destroy(child.gameObject);

            Events.EventHandler.SubscribeUnitHp(info, this.gameObject);

            CaptionText.text = $"{info.PilotName}({CONST.Class(info.Weight)} {info.Type})";

            foreach (var part in info.UnitHP.AllParts)
            {
                var p = Instantiate(info.Vision == Visibility.Visual ? HiddenPartPrefab : VissiblePartPrefab, PartsHolder, false).GetComponent<UnitPartStateBase>();
                p.Init(this, part, info.UnitHP);
                parts.Add(part, p);
            }

            foreach (Transform child in StringContainer)
                Destroy(child.gameObject);

            if(info.Vision == Visibility.Visual)
                Shield.ShowHidden();
            else
                Shield.Show();

            UnitDamage(info.UnitHP);

        }

        private void ShowPart(Parts part)
        {
            foreach (Transform child in StringContainer)
                Destroy(child.gameObject);

            var list = part == Parts.None ? info.Modules : info.Modules[part];

            foreach(var module in list)
            {
                if (info.Vision == Visibility.Visual)
                {
                    if (CONST.Visible(module))
                    {
                        var str = Instantiate(StringPrefab, StringContainer).GetComponent<IconText>();
                        str.Icon = module.Icon;
                        str.Text = module.BaseName;
                    }
                }
                else
                {
                    if (selected != Parts.None || module.ModType == Items.ModuleType.Weapon || module.ModType == Items.ModuleType.AmmoPod)
                    {
                        var str = Instantiate(StringPrefab, StringContainer).GetComponent<IconText>();
                        str.Icon = module.Icon;
                        str.Text = module.ShortName;
                    }
                }
            }
        }
        

        public void ShowPartDetail(Parts part)
        {
            selected = part;
            UnitDamage(info.UnitHP);
        }

        public void HidePartDetail()
        {
            selected = Parts.None;
            UnitDamage(info.UnitHP);
        }


        public void UnitDamage(UnitHp hp)
        {
            Shield.MaxValue = hp.MaxShield;
            Shield.Value = hp.Shield;

            if (selected == Parts.None)
            {
                if (info.Vision == Visibility.Visual)
                {
                    Structure.ShowHidden();
                    Armor.ShowHidden();
                    ArmorB.Hide();
                    ArmorF.Hide();
                }
                else
                {
                    Structure.Show();
                    Armor.Show();
                    ArmorB.Hide();
                    ArmorF.Hide();
                }

                Structure.MaxValue = hp.MaxStructure;
                Structure.Value = hp.TotalStructure;

                Armor.MaxValue = hp.MaxArmor;
                Armor.Value = hp.TotalArmor;
            }
            else
            {
                var max = hp.MaxHp(selected);
                var current = hp.CurrentHp(selected);

                if (info.Vision == Visibility.Visual)
                {
                    Structure.ShowHidden();
                    Armor.ShowHidden();
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
                        ArmorF.Show();
                        ArmorB.Show();
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
                        Armor.Show();
                        Armor.MaxValue = max.armor;
                        Armor.Value = current.armor;
                    }
                }
            }

            ShowPart(selected);
        }

        public void MouseUnitEnter(UnitInfo unit)
        {
            if (unit.Vision < Visibility.Visual)
                return;
            if(selected_unit != null)
                Events.EventHandler.UnsubscribeUnitHp(selected_unit, this.gameObject);
            Group.interactable = false;
            Group.blocksRaycasts = false;
            Window.SetActive(true);
            SetUnit(unit);
            current_unit = unit;

        }

        public void MouseUnitLeave(UnitInfo unit)
        {
            if (current_unit == unit)
            {
                Events.EventHandler.UnsubscribeUnitHp(current_unit, this.gameObject);
                current_unit = null;

                if (selected_unit == null)
                {
                    Window.SetActive(false);
                }
                else
                {
                    Window.SetActive(true);
                    Group.interactable = true;
                    Group.blocksRaycasts = true;
                    SetUnit(selected_unit);
                }
            }
        }

        public void MouseUnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Right && unit.Vision >= Visibility.Visual)
            {
                if (selected_unit != null)
                    Events.EventHandler.UnsubscribeUnitHp(selected_unit, this.gameObject);

                Window.SetActive(true);
                selected_unit = info;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if(selected_unit != null)
                    Events.EventHandler.UnsubscribeUnitHp(selected_unit, this.gameObject);
                selected_unit = null;
                Window.SetActive(false);
                Group.interactable = false;
                Group.blocksRaycasts = false;
            }
        }
    }
}