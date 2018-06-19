using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.UI
{
    public abstract class UnitPartStateBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPartDamaged
    {
        private IUnitStateWindow window;
        protected Parts part;
        protected UnitInfo unit;

        protected abstract void UpdateValues();

        public virtual void Init(IUnitStateWindow window, Parts part, UnitInfo unit)
        {
            this.window = window;
            this.part = part;
            this.unit = unit;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            window.ShowPartDetail(part);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            window.HidePartDetail();
        }

        public void PartDamaged(UnitInfo unit, Parts part)
        {
            if (this.unit != unit || part != this.part)
                return;
            UpdateValues();
        }
    }
}