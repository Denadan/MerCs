using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.UI
{
    public abstract class UnitPartStateBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPartDamaged
    {
        private UnitStateWindow window;
        protected Parts part;
        protected UnitHp hp;

        protected abstract void UpdateValues(UnitHp hp);

        public virtual void Init(UnitStateWindow window, Parts part, UnitHp hp)
        {
            this.window = window;
            this.part = part;
            this.hp = hp;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            window.ShowPartDetail(part);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            window.HidePartDetail();
        }

        public void PartDamaged(UnitHp hp)
        {
            UpdateValues(hp);
        }
    }
}