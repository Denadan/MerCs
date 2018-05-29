using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class TileInfoWindow : MonoBehaviour, ITileEventReceiver
    {
        [SerializeField]
        private GameObject Window;
        [SerializeField]
        private Image TileImage;
        [SerializeField]
        private Text NameText;
        [SerializeField]
        private Text MoveText;
        [SerializeField]
        private Text CoordText;

        void Start()
        {
            Window.SetActive(false);
        }

        public void MouseTileEnter(Vector2Int coord)
        {

            var tile = TacticalController.Instance.Grid[coord];
            if (tile == null)
                return;
            var info = tile.GetComponent<TileInfoComponent>();
            var image = tile.GetComponent<SpriteRenderer>();

            if (info == null || image == null)
                return;
            TileImage.sprite = image.sprite;
            NameText.text = info.Info.Feature.ToString();
            MoveText.text = $"Move: {info.Info.PathTileCost}";
            CoordText.text = info.Info.CellCoord.ToString();

            Window.SetActive(true);
        }

        public void MouseTileLeave(Vector2Int coord)
        {
            Window.SetActive(false);
        }

        public void MouseTileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
        }
    }
}