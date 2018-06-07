#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{

    public class UnitOverlay : MonoBehaviour
    {
        [SerializeField]
        private UnitInfo info;

        [SerializeField]
        private Image InitImage;

        [SerializeField]
        private Sprite[] Active;
        [SerializeField]
        private Sprite[] Moved;


        private void FixedUpdate()
        {
            int n = Mathf.Clamp(info.Movement.Initiative - 1, 0, 4);
            if (info.Active)
                InitImage.sprite = Active[n];
            else
                InitImage.sprite = Moved[n];
        }
    }
}
