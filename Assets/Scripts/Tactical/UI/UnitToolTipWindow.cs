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

        [SerializeField] private SliderInfo Shield;
        [SerializeField] private SliderInfo Armor;
        [SerializeField] private SliderInfo ArmorF;
        [SerializeField] private SliderInfo ArmorB;
        [SerializeField] private SliderInfo Structure;

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

            CaptionText.text = info.PilotName;

            foreach (var part in info.UnitHP.AllParts)
            {
                var p = Instantiate(info.Vision == Visibility.Visual ? HiddenPartPrefab : VissiblePartPrefab, PartsHolder, false).GetComponent<UnitPartStateBase>();
                p.Init(this, part, info.UnitHP);
                parts.Add(part, p);
            }

            foreach (Transform child in StringContainer)
                Destroy(child.gameObject);
            UnitDamage(info.UnitHP);
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
            Shield.Show();
            Shield.Max = 0;
            Shield.Hp = 0;

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

                Structure.Max = hp.MaxStructure;
                Structure.Hp = hp.TotalStructure;

                Armor.Max = hp.MaxArmor;
                Armor.Hp = hp.TotalArmor;
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
                    Structure.Max = max.structure;
                    Structure.Hp = current.structure;

                    Armor.Max = max.has_back_armor ? max.armor + max.back_armor : max.armor;
                    Structure.Max = max.structure;

                    Armor.Hp = current.has_back_armor ? current.armor + current.back_armor : current.armor;
                    Structure.Hp = current.structure;
                }
                else
                {
                    Structure.Max = max.structure;
                    Structure.Hp = current.structure;

                    if (max.has_back_armor)
                    {
                        ArmorF.Show();
                        ArmorB.Show();
                        Armor.Hide();

                        ArmorF.Max = max.armor;
                        ArmorF.Hp = current.armor;

                        ArmorB.Max = max.back_armor;
                        ArmorB.Hp = current.back_armor;
                    }
                    else
                    {
                        ArmorF.Hide();
                        ArmorB.Hide();
                        Armor.Show();
                        Armor.Max = max.armor;
                        Armor.Hp = current.armor;
                    }
                }
            }
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