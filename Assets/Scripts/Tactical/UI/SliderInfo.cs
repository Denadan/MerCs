using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    [Serializable]
    public class SliderInfo
    {
        [SerializeField] private GameObject Bar;
        [SerializeField] private UnitPartStateSlider slider;
        [SerializeField] private Text text;


        private float max = 0, current = 0;
        private bool hidden = false;
        private Outline outline;


        public void ShowHidden()
        {
            if (outline == null)
                outline = text.GetComponent<Outline>();
            outline.enabled = false;
            Bar.SetActive(false);
            text.color = Color.black;
            text.gameObject.SetActive(true);
            hidden = true;
        }

        public void Show()
        {
            if (outline == null)
                outline = text.GetComponent<Outline>();
            outline.enabled = true;
            Bar.SetActive(true);
            text.color = Color.black;
            text.gameObject.SetActive(true);
            hidden = false;
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
                if (max <= 0)
                {
                    Bar.SetActive(false);
                    text.text = "NONE";
                }
                else
                {
                    slider.MaxValue = max;
                    if(!hidden)
                        text.text = $"{current:F1}/{max:F1}";
                }
            }
        }

        public float Hp
        {
            set
            {
                current = value;
                if (hidden)
                {
                    if (max <= 0)
                        text.text = "NONE";
                    else
                    {
                        switch (current / max)
                        {
                            case float i when i >= 0.95f:
                                text.text = "FULL";
                                break;
                            case float i when i >= 0.8f:
                                text.text = "SCRATCHED";
                                break;
                            case float i when i >= 0.5f:
                                text.text = "DAMAGED";
                                break;
                            case float i when i > 0.25f:
                                text.text = "SHATTERED";
                                break;
                            case float i when i > 0:
                                text.text = "CRITICAL";
                                break;
                            default:
                                text.text = "DESTROYED";
                                break;
                        }

                        text.color = CONST.GetColor(current,max);
                    }
                }
                else
                {
                    slider.Value = current;
                    if (max == 0)
                        text.text = "NONE";
                    else
                        text.text = $"{current:F1}/{max:F1}";
                }
            }
        }

    }
}