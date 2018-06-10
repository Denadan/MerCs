using UnityEngine;

namespace Denadan.UI
{
    public class BasicBackSlider : BasicSlider
    {
        [SerializeField] protected RectTransform bar;

        public override float Value
        {
            set => bar.transform.localScale = MaxValue <= 0
                ? new Vector3(1, 1, 1)
                : new Vector3(Mathf.Clamp(value / MaxValue, 0, 1), 1, 1);
        }

        public override float Gradient => bar.transform.localScale.x;

    }
}