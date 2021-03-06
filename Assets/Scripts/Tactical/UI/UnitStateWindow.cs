﻿#pragma warning disable 649


using System.Collections.Generic;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public partial class UnitStateWindow : MonoBehaviour, IUnitDamaged, IUnitStateWindow
    {
        [SerializeField] private GameObject PartPrefab;
        [SerializeField] private Transform PartsHolder;
        [SerializeField] private Text CaptionText;

        [SerializeField] private ShieldSliderInfo Shield;
        [SerializeField] private HpSliderInfo Armor;
        [SerializeField] private HpSliderInfo ArmorF;
        [SerializeField] private HpSliderInfo ArmorB;
        [SerializeField] private HpSliderInfo Structure;

        [SerializeField] private Transform StringContainer;
        [SerializeField] private GameObject StringPrefab;

        [SerializeField] private Sprite MoveSprite;
        [SerializeField] private Sprite RunSprite;
        [SerializeField] private Sprite JumpSprite;

        [SerializeField] private Sprite RadarSprite;
        [SerializeField] private Sprite VisualSprite;
        [SerializeField] private Sprite LockedSprite;


        private Dictionary<Parts, UnitPartStateBase> parts = new Dictionary<Parts, UnitPartStateBase>();
        private UnitInfo info;
        private Parts selected;

        public void SetUnit(UnitInfo info)
        {
            parts.Clear();
            foreach (Transform child in PartsHolder)
                Destroy(child.gameObject);

            if (info == null)
            {
                Armor.Hide();
                Shield.Hide();
                ArmorB.Hide();
                ArmorF.Hide();
                Structure.Hide();

                return;
            }
            CaptionText.text = $"{info.PilotName}({CONST.Class(info.Weight)} {info.Type})";
            this.info = info;

            foreach (var part in info.UnitHP.AllParts)
            {
                var p = Instantiate(PartPrefab, PartsHolder, false).GetComponent<UnitPartStateBase>();
                p.Init(this, part, info);
                parts.Add(part, p);
            }


            UnitDamage(info, info.UnitHP);
        }


        private void ShowPartData()
        {
            foreach (Transform child in StringContainer)
                Destroy(child.gameObject);

            var part = Instantiate(StringPrefab, StringContainer).GetComponent<IconText>();
            part.Text = $"{selected} modules";

            foreach (var module in info.Modules[selected])
            {
                var str = Instantiate(StringPrefab, StringContainer).GetComponent<IconText>();
                str.Icon = module.Icon;
                str.Text = module.ShortName;
            }
        }

        private void ShowMovementData()
        {
            foreach (Transform child in StringContainer)
                Destroy(child.gameObject);
            if (info.Movement.MoveMp > 0)
            {
                var item = Instantiate(StringPrefab, StringContainer, false).GetComponent<IconText>();
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
        }

        private void ShowRadarData()
        {
            var item = Instantiate(StringPrefab, StringContainer, false).GetComponent<IconText>();
            item.Icon = RadarSprite;
            item.Text = info.RadarRange.ToString();

            if (info.VisualRange > 0)
            {
                item = Instantiate(StringPrefab, StringContainer, false).GetComponent<IconText>();
                item.Icon = VisualSprite;
                item.Text = info.VisualRange.ToString();
            }

            item = Instantiate(StringPrefab, StringContainer, false).GetComponent<IconText>();
            item.Icon = LockedSprite;
            item.Text = info.ScanRange.ToString();
        }

        private void OnDisable()
        {
            selected = Parts.None;
            info = null;
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

        public void UnitDamage(UnitInfo unit, UnitHp hp)
        {
            if (info != unit)
                return;

            Shield.Show();
            Shield.MaxValue = hp.MaxShield;
            Shield.Value = hp.Shield;

            if (selected == Parts.None)
            {
                Structure.Show();
                Armor.Show();
                ArmorB.Hide();
                ArmorF.Hide();


                Structure.MaxValue = hp.MaxStructure;
                Structure.Value = hp.TotalStructure;

                Armor.MaxValue = hp.MaxArmor;
                Armor.Value = hp.TotalArmor;

                ShowMovementData();
                ShowRadarData();
            }
            else
            {
                var max = hp.MaxHp(selected);
                var current = hp.CurrentHp(selected);

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
                ShowPartData();
            }
        }
    }
}