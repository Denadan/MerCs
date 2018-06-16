#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    /// <summary>
    /// иконка с текстом
    /// </summary>
    public class IconText : MonoBehaviour
    {
        /// <summary>
        /// текст
        /// </summary>
        [SerializeField] private Text text;
        /// <summary>
        /// иконка
        /// </summary>
        [SerializeField] private Image icon;

        /// <summary>
        /// задает или получает текст элемента
        /// </summary>
        public string Text
        {
            get => text.text;
            set => text.text = value;
        }

        /// <summary>
        /// задает или получает иконку элемента
        /// </summary>
        public Sprite Icon
        {
            get => icon.sprite;
            set => icon.sprite = value;
        }

        /// <summary>
        /// задает цвет текста
        /// </summary>
        public Color TextColor
        {
            get => text.color;
            set => text.color = value;
        }

        /// <summary>
        /// задает цвет иконки
        /// </summary>
        public Color IconColor
        {
            get => icon.color;
            set => icon.color = value;
        }
    }
}