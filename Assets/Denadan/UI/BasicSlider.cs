using UnityEngine;

namespace Denadan.UI
{
    public class BasicSlider : MonoBehaviour
    {
        public virtual float MaxValue { get; set; }
        public virtual float Gradient => transform.localScale.x;

        public virtual float Value
        {
            set => transform.localScale = MaxValue <= 0 ?
                new Vector3(1, 1, 1) :
                new Vector3(Mathf.Clamp(value / MaxValue, 0, 1), 1, 1);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}