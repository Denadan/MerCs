using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class UnitPartStateSlider : MonoBehaviour
    {
        public float MaxValue;
        public float Gradient => transform.localScale.x;

        public float Value
        {
            set => transform.localScale = MaxValue <= 0 ? 
                new Vector3(1,1,1) : 
                new Vector3(Mathf.Clamp(value/MaxValue, 0, 1),1,1);
        }
        
    }
}