using UnityEngine;

namespace Mercs.Tactical.Events
{
    public class TileSubscriber : MonoBehaviour
    {
        private void Start()
        {
            EventHandler.TileSubscribe(gameObject);
        }

        private void OnDestroy()
        {
            EventHandler.TileUnSubscribe(gameObject);
        }
    }
}