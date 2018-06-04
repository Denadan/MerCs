using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class PilotHPBar : MonoBehaviour
    {
        [SerializeField] private Transform BarParent;
        [SerializeField] private GameObject bar;

        private List<Image> bars = new List<Image>();

        [SerializeField]
        private int maxHP;
        [SerializeField]
        private int currentHP;
        [SerializeField]
        private int addHP;


        public int HP
        {
            get => currentHP;
            set
            {
                if (value > MaxHP)
                    value = MaxHP;
                if (value != currentHP)
                {
                    currentHP = value;
                    updateBars();
                }
            }
        }
        public int MaxHP
        {
            get => maxHP;
            set
            {
                if (maxHP != value)
                {
                    maxHP = value;
                    createbars();
                }

            }
        }

        public int AddHP
        {
            get => addHP;
            set
            {
                if (value == addHP)
                    return;
                while (value < addHP)
                {
                    var bar = bars[bars.Count - 1];
                    Destroy(bar.gameObject);
                    bars.RemoveAt(bars.Count - 1);
                    addHP -= 1;
                }

                while (value > addHP)
                {
                    var bar = Instantiate(this.bar, BarParent).GetComponent<Image>();
                    bars.Add(bar);
                    addHP += 1;
                }

                updateBars();
            }
        }

        private void createbars()
        {
            foreach (Transform child in BarParent)
                Destroy(child.gameObject);
            bars.Clear();

            for (int i = 0; i < maxHP + addHP; i++)
            {
                var bar = Instantiate(this.bar, BarParent).GetComponent<Image>();
                bars.Add(bar);
            }
            updateBars();
        }

        private void updateBars()
        {
//            UnityEngine.Debug.Log($"{MaxHP} - {addHP} - {HP}");
            for (int i = 0; i < maxHP; i++)
            {
                bars[i].sprite = i < currentHP ? FullHPSprite : EmptyHPSprite;
                bars[i].color = i < currentHP ? FullColor : EmptyColor;
            }

            for (int i = maxHP; i < maxHP + addHP; i++)
            {
                bars[i].sprite = AddHPSprite;
                bars[i].color = AddColor;
            }
        }

        public Sprite FullHPSprite;
        public Sprite EmptyHPSprite;
        public Sprite AddHPSprite;

        public Color FullColor;
        public Color EmptyColor;
        public Color AddColor;

        public void Start()
        {
            createbars();
        }

    }
}