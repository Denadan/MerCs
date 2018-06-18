#pragma warning disable 649


using Mercs.Tactical.Events;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(UnitInfo))]
    public class EnemyUnitVision : MonoBehaviour, IVisionChanged
    {
        [SerializeField] private GameObject Main;
        [SerializeField] private GameObject Blip;
        [SerializeField] private GameObject Canvas;
        private UnitInfo unit;


        private void OnDestroy()
        {
            EventHandler.UnsubscribeVisionChange(unit, gameObject);
        }

        private void Awake()
        {
            unit = GetComponent<UnitInfo>();
            EventHandler.SubscribeVisionChange(unit, gameObject);
        }


        public void VisionChanged(Visibility.Level level)
        {

            switch (level)
            {
                case Visibility.Level.Visual:
                case Visibility.Level.Scanned:
                case Visibility.Level.Friendly:
                    Blip.gameObject.SetActive(false);
                    Main.gameObject.SetActive(true);
                    break;
                case Visibility.Level.Sensor:
                    Blip.gameObject.SetActive(true);
                    Main.gameObject.SetActive(false);
                    break;
                case Visibility.Level.None:
                    Blip.gameObject.SetActive(false);
                    Main.gameObject.SetActive(false);
                    break;

            }
        }
    }
}