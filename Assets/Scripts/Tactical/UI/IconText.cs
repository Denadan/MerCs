using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class IconText : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Image icon;


        public string Text
        {
            get => text.text;
            set => text.text = value;
        }

        public Sprite Icon
        {
            get => icon.sprite;
            set => icon.sprite = value;
        }

        public Color TextColor
        {
            get => text.color;
            set => text.color = value;
        }

        public Color IconColor
        {
            get => icon.color;
            set => icon.color = value;
        }
    }
}