using UnityEngine;

namespace Mercs.Tactical.Events
{
    public class PilotDamagedSubscriber : MonoBehaviour
    {

        private void OnDestroy()
        {
            EventHandler.PilotHpUnSubscribe(gameObject);
        }

        private void Start()
        {
            EventHandler.PilotHpSubscribe(gameObject);
        }

    }
}