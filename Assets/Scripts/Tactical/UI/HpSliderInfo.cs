#pragma warning disable 649

using Denadan.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class HpSliderInfo : BasicTextSlider
    {
        [SerializeField] private GameObject back;

        protected bool hidden = false;
        private Outline outline;


        public void ShowHidden()
        {
            if (outline == null)
                outline = text.GetComponent<Outline>();
            outline.enabled = false;

            text.gameObject.SetActive(true);
            back.SetActive(false);


            text.color = Color.black;
            text.gameObject.SetActive(true);
            hidden = true;
        }

        public override void Show()
        {
            if (outline == null)
                outline = text.GetComponent<Outline>();

            outline.enabled = true;
            back.SetActive(true);

            text.color = Color.black;

            text.gameObject.SetActive(true);
            hidden = false;
        }

        public override void Hide()
        {
            back.SetActive(false);
            text.gameObject.SetActive(false);
        }


        public override float MaxValue
        {
            get => base.MaxValue;
            set
            {
                base.MaxValue = value;
                if (value <= 0)
                    back.gameObject.SetActive(false);
                Value = 0;
            }
        }

        public override string MakeText(float value)
        {
            if (hidden)
            {
                if (MaxValue <= 0)
                {
                    text.color = Color.black;
                    return "NONE";
                }
                else
                {
                    text.color = CONST.GetColor(value, MaxValue);
                    switch (value / MaxValue)
                    {
                        case float i when i >= 0.95f:
                            return "FULL";
                        case float i when i >= 0.8f:
                            return "SCRATCHED";
                        case float i when i >= 0.5f:
                            return "DAMAGED";
                        case float i when i > 0.25f:
                            return "SHATTERED";
                        case float i when i > 0:
                            return "CRITICAL";
                        default:
                            return "DESTROYED";
                    }
                }
            }
            else

                if (value == 0)
                    return "NONE";
                else
                    return base.MakeText(value);
        }
    }
}