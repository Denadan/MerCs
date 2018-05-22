using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Mercs.Tactical
{
    public class TileInfoComponent : MonoBehaviour, IPointerEnterHandler
    {
        public TileInfo Info { get; set; }
        public Map Map;

        public void OnPointerEnter(PointerEventData eventData)
        {
        }
    }
}

