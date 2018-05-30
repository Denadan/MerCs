﻿using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    [RequireComponent(typeof(PilotDamagedSubscriber))]
    public class UnitSelectButton : MonoBehaviour, IPilotDamaged
    {
        public Image unitImage;
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text bottomText;
        [SerializeField]
        private PilotHPBar HpBar;

        public Image Background;


        public UnitInfo Unit;


        public Text BottomText { get => bottomText; set => bottomText = value; }

        private void Start()
        {
            if (Unit == null)
                return;
            unitImage.sprite = Unit.GetComponent<SpriteRenderer>().sprite;
            unitImage.material = Unit.Faction.CamoMaterial;
            nameText.text = Unit.PilotName;

            HpBar.MaxHP = Unit.PilotHP.MaxHp;
            HpBar.addHP = Unit.PilotHP.AddHp;
            HpBar.HP = Unit.PilotHP.Hp;
        }

        public void PilotDamaged(UnitInfo unit)
        {
            if (unit == Unit)
            {
                HpBar.AddHP = Unit.PilotHP.AddHp;
                HpBar.HP = Unit.PilotHP.Hp;
            }
        }
    }
}
