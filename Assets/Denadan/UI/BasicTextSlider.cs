using UnityEngine;
using UnityEngine.UI;

namespace Denadan.UI
{
    public class BasicTextSlider : BasicBackSlider
    {
        [SerializeField] protected Text text;

        public override float Value
        {
            set
            {
                base.Value = value;
                text.text = MakeText(value);
            }
        }


        public virtual string MakeText(float value)
        {
            return $"{value:F1}/{MaxValue:F1}";
        }
    }
}