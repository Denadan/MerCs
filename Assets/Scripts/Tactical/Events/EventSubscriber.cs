using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.Events
{
    public abstract class EventSubscriber<T> : MonoBehaviour
        where T : IEventSystemHandler
    {
        protected virtual void Start()
        {
            EventHandler.Subscribe<T>(gameObject);
        }

        private void OnDestroy()
        {
            EventHandler.UnSubscribe<T>(gameObject);
        }
    }
}
