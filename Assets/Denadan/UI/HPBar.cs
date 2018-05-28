using System.Configuration;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Denadan.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class HPBar : MonoBehaviour
    {
        
        public Image FillerImage;

        public int MaxHP = 10;
        public int HP = 5;
        public int Tiling = 1;

        public Sprite FullHp;
        public Sprite EmptyHp;

        private void Start()
        {

        }

    }
}