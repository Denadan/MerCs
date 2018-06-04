using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class KeepRotation : MonoBehaviour
    {
        private Quaternion rotation;

        private void Awake()
        {
            rotation = transform.rotation;
        }

        private void LateUpdate()
        {
            transform.rotation = rotation;
        }
    }
}
