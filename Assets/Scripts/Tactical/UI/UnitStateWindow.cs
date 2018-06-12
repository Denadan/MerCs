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



        private Dictionary<Parts, UnitPartStateBase> parts = new Dictionary<Parts, UnitPartStateBase>();
        private UnitInfo info;
        private Parts selected;

        public void SetUnit(UnitInfo info)
        {

            if (this.info != null)
                Events.EventHandler.UnsubscribeUnitHp(info, this.gameObject);



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

            Events.EventHandler.SubscribeUnitHp(info, this.gameObject);

            CaptionText.text = $"{info.PilotName}({CONST.Class(info.Weight)} Mech)";
            this.info = info;

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

            UnitDamage(info.UnitHP);
        }

        private void OnDisable()
        {
            selected = Parts.None;
            if (info != null)
            {
                Events.EventHandler.UnsubscribeUnitHp(info, this.gameObject);
                info = null;
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

            }
        }
    }
}