using UnityEngine;

namespace Mercs.Tactical.Events
{
    public class UnitSubscriber : MonoBehaviour
    {
        private void Start()
        {
            EventHandler.UnitSubscribe(gameObject);
        }

        private void OnDestroy()
        {
            EventHandler.UnitUnSubscribe(gameObject);
        }
    }
}