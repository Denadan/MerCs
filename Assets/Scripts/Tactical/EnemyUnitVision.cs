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
        private UnitInfo unit;

        private void Awake()
        {
            unit = GetComponent<UnitInfo>();
        }

        public void VisionChanged(UnitInfo unit, Visibility.Level level)
        {
            if (this.unit != unit)
                return;

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