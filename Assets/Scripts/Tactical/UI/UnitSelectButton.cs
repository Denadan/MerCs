﻿using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class UnitSelectButton : MonoBehaviour
    {
        public Image unitImage;
        [SerializeField]
        private Text nameText;
        [SerializeField]
        private Text bottomText;

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
            Background = GetComponent<Image>();
        }
    }
}
