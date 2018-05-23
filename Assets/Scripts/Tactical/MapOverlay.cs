using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(HexGrid))]
    [RequireComponent(typeof(Map))]
    public class MapOverlay : MonoBehaviour
    {
        public enum Tile { Filled, Hatched }

        private Map map;
        private HexGrid grid;
        private SpriteRenderer[,] sprites;


        [SerializeField]
        private Transform TileParent;
        [SerializeField]
        private GameObject TilePrefab;
        [SerializeField]
        private Sprite FilledSprite;
        [SerializeField]
        private Sprite HatchSprite;



        private void Start()
        {
            map = GetComponent<Map>();
            grid = GetComponent<HexGrid>();
            sprites = new SpriteRenderer[map.SizeX, map.SizeY];

            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    var pos = grid.CellToWorld(new Vector3Int(i, j, 0));
                    var tile = Instantiate(TilePrefab, pos, Quaternion.identity, TileParent);

                    sprites[i, j] = tile.GetComponent<SpriteRenderer>();
                    sprites[i, j].color = new Color(0, 1, 1, 0.3f);
                   // tile.SetActive(false);
                }
        }

        private void Clear()
        {
            foreach (Transform tile in TileParent)
                Destroy(tile.gameObject);
        }

        public void HideAll()
        {
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    sprites[i, j].gameObject.SetActive(false);
                }
        }

        private void ShowTile(Vector2Int coord, Color color)
        {

        }

        private void ShowZone(List<Vector2Int> zone, Color color)
        {
            foreach (var item in zone)
                ShowTile(item, color);
        }
    }
}
