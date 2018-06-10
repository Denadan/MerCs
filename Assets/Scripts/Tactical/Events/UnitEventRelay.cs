using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.Events
{
    public class UnitEventRelay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnitInfo OriginalUnit;

        private void Start()
        {
            if (OriginalUnit == null)
                OriginalUnit = GetComponent<UnitInfo>();
            if(OriginalUnit == null)
                UnityEngine.Debug.Log("Не найден объеккт для прикрепленного юнита, " + gameObject.name);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventHandler.UnitPointerClick(eventData, OriginalUnit);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventHandler.UnitPointerEnter(eventData, OriginalUnit);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventHandler.UnitPointerLeave(eventData, OriginalUnit);
        }
    }
}

