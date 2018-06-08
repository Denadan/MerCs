using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Debug
{
    public class ShowName : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            UnityEngine.Debug.Log(gameObject.name + " press");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UnityEngine.Debug.Log(gameObject.name + " enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UnityEngine.Debug.Log(gameObject.name + " leave");
        }
    }
}
