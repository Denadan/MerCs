#pragma warning disable 649

using Denadan.UI;
using UnityEngine;
using UnityEngine.UI;

using Mercs.Tactical;

namespace Mercs.Tactical.UI
{
    public class HpSliderInfo : BasicTextSlider
    {
        [SerializeField] private GameObject back;

        protected Visibility.Level level;
        private Outline outline;


        public void Show(Visibility.Level level)
        {
            this.level = level;

            if (outline == null)
                outline = text.GetComponent<Outline>();

            switch(level)
            {
                case Visibility.Level.Sensor:
                case Visibility.Level.Visual:
                    outline.enabled = false;
                    back.SetActive(false);
                    text.color = Color.black;
                    text.gameObject.SetActive(true);
                    break;
                case Visibility.Level.Friendly:
                case Visibility.Level.Scanned:
                    outline.enabled = true;
                    back.SetActive(true);
                    text.color = Color.black;
                    text.gameObject.SetActive(true);
                    break;
            }
        }

        public override void Show() => Show(Visibility.Level.Scanned);

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
            switch(level)
            {
                case Visibility.Level.None:
                case Visibility.Level.Sensor:
                    return "???";

                case Visibility.Level.Visual:
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
                default:
                    if (value == 0)
                        return "NONE";
                    else
                        return base.MakeText(value);
            }


        }
    }
}