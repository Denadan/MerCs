using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.Events
{
    [RequireComponent(typeof(TileInfoComponent))]
    [AddComponentMenu("Merc/EventRelay/Tile Mouse Event")]
    public class TileEventRelay : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private TileInfoComponent info;

        public void Awake()
        {
            info = GetComponent<TileInfoComponent>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (info.Info != null)
                EventHandler.Raise<ITileEvent>((i, d) => i.MouseTileClick(new Vector2Int(info.Info.CellCoord.x, info.Info.CellCoord.y), eventData.button));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (info.Info != null)
                EventHandler.Raise<ITileEvent>((i, d) => i.MouseTileEnter(new Vector2Int(info.Info.CellCoord.x, info.Info.CellCoord.y)));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (info.Info != null)
                EventHandler.Raise<ITileEvent>((i, d) => i.MouseTileLeave(new Vector2Int(info.Info.CellCoord.x, info.Info.CellCoord.y)));
        }
    }
}