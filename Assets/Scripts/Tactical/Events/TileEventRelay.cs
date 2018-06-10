using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.Events
{
    [RequireComponent(typeof(TileInfoComponent))]
    public class TileEventRelay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private TileInfoComponent info;

        public void Start()
        {
            info = GetComponent<TileInfoComponent>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (info.Info != null)
                EventHandler.TilePointerClick(eventData, new Vector2Int(info.Info.CellCoord.x, info.Info.CellCoord.y));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (info.Info != null)
                EventHandler.TilePointerEnter(eventData, new Vector2Int(info.Info.CellCoord.x, info.Info.CellCoord.y));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (info.Info != null)
                EventHandler.TilePointerLeave(eventData, new Vector2Int(info.Info.CellCoord.x, info.Info.CellCoord.y));
        }
    }
}