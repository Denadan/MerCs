using UnityEngine;

namespace Mercs.Tactical.Debug
{
    public class RegenerateMap : MonoBehaviour
    {
        public void OnButtonPress()
        {
            gameObject.SendMessage("Clear");
            gameObject.SendMessage("Start");
        }

        public void OnHidePath(bool value)
        {
            var item = transform.Find("Path");
            UnityEngine.Debug.Log(item);
            if(item != null)
                item.gameObject.SetActive(value);
        }

        public void OnHideLinks(bool value)
        {
            var item = transform.Find("Link");
            UnityEngine.Debug.Log(item);
            if (item != null)
                item.gameObject.SetActive(value);
        }
    }
}