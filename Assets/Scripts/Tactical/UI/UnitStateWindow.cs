using System;
using System.Collections.Generic;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class UnitStateWindow : MonoBehaviour, IUnitDamaged
    {
        [SerializeField] private GameObject PartPrefab;
        [SerializeField] private Transform PartsHolder;
        [SerializeField] private Text CaptionText;

        [Serializable]
        public class SliderInfo
        {
            [SerializeField]
            private GameObject Bar;
            [SerializeField]
            private UnitPartStateSlider slider;
            [SerializeField]
            private Text text;

            private float max = 0, current = 0;

            public void Show()
            {
                Bar.SetActive(true);
                text.gameObject.SetActive(true);
            }

            public void Hide()
            {
                Bar.SetActive(false);
                text.gameObject.SetActive(false);
            }

            public float Max
            {
                set
                {
                    max = value;
                    if (max == 0)
                    {
                        Bar.SetActive(false);
                        text.text = "NONE";
                    }
                    else
                    {
                        slider.MaxValue = max;
                        text.text = $"{current:F1}/{max:F1}";
                    }
                }
            }

            public float Hp
            {
                set
                {
                    slider.Value = current = value;
                    if (max == 0)
                        text.text = "NONE";
                    else
                        text.text = $"{current:F1}/{max:F1}";
                }
            }

        }

        [SerializeField] private SliderInfo Shield;
        [SerializeField] private SliderInfo Armor;
        [SerializeField] private SliderInfo ArmorF;
        [SerializeField] private SliderInfo ArmorB;
        [SerializeField] private SliderInfo Structure;

        [SerializeField] private Transform StringContainer;
        [SerializeField] private GameObject StringPrefab;

        [SerializeField] private Sprite MoveSprite;
        [SerializeField] private Sprite RunSprite;
        [SerializeField] private Sprite JumpSprite;



        private Dictionary<Parts, UnitPartStateBase> parts = new Dictionary<Parts, UnitPartStateBase>();
        private UnitInfo info;
        private Parts selected;

        public void SetUnit(UnitInfo info)
        {
            parts.Clear();
            CaptionText.text = info.PilotName;
            this.info = info;
            foreach (Transform child in PartsHolder)
                Destroy(child.gameObject);
            foreach (var part in info.UnitHP.AllParts)
            {
                var p = Instantiate(PartPrefab, PartsHolder, false).GetComponent<UnitPartStateBase>();
                p.Init(this, part, info.UnitHP);
                parts.Add(part, p);
            }
            foreach (Transform child in StringContainer)
                Destroy(child.gameObject);
            if (info.Movement.MoveMp > 0)
            {
                var item = Instantiate(StringPrefab, StringContainer,false).GetComponent<IconText>();
                item.Icon = MoveSprite;
                item.Text = info.Movement.MoveMp.ToString();
            }

            if (info.Movement.RunMP > 0)
            {
                var item = Instantiate(StringPrefab, StringContainer, false).GetComponent<IconText>();
                item.Icon = RunSprite;
                item.Text = info.Movement.RunMP.ToString();
            }

            if (info.Movement.JumpMP > 0)
            {
                var item = Instantiate(StringPrefab, StringContainer, false).GetComponent<IconText>();
                item.Icon = JumpSprite;
                item.Text = info.Movement.JumpMP.ToString();
            }

            UnitDamage(info.UnitHP);
        }

        private void OnDisable()
        {
            selected = Parts.None;
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
                Structure.Show();
                Armor.Show();
                ArmorB.Hide();
                ArmorF.Hide();


                Structure.Max = hp.MaxStructure;
                Structure.Hp = hp.TotalStructure;

                Armor.Max = hp.MaxArmor;
                Armor.Hp = hp.TotalArmor;
            }
            else
            {
                var max = hp.MaxHp(selected);
                var current = hp.CurrentHp(selected);

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
}