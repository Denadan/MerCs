#pragma warning disable 649

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class WeaponShotBar : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]private Image Back;

        public delegate void click(int n);

        public click OnClick { get; set; }
        public int Value { get; set; }
        public Color BackColor
        {
            set => Back.color = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left && OnClick != null)
                OnClick(Value);
        }
    }
}