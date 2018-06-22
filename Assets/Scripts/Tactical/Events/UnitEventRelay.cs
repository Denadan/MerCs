using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.Events
{
    [AddComponentMenu("Merc/EventRelay/Unit Mouse Event")]
    public class UnitEventRelay : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private UnitInfo OriginalUnit;

        private void Start()
        {
            if (OriginalUnit == null)
                OriginalUnit = GetComponent<UnitInfo>();
            if (OriginalUnit == null)
            {
                UnityEngine.Debug.Log("Не найден объеккт для прикрепленного юнита, " + gameObject.name);
                Destroy(this);
            }
        }

        public void SetUnit(UnitInfo unit)
        {
            OriginalUnit = unit;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventHandler.Raise<IUnitEvent>((i, d) => i.MouseUnitEnter(OriginalUnit));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventHandler.Raise<IUnitEvent>((i, d) => i.MouseUnitLeave(OriginalUnit));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventHandler.Raise<IUnitEvent>((i, d) => i.MouseUnitClick(OriginalUnit, eventData.button));
        }
    }
}

