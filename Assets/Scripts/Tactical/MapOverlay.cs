using System.Collections.Generic;
using Denadan.Sprites;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(HexGrid))]
    [RequireComponent(typeof(Map))]
    public class MapOverlay : MonoBehaviour
    {
        private Map map;
        private HexGrid grid;
        private OverlayedSprite[,] sprites;

        [SerializeField]
        private Texture2D[] Textures;

        public bool Ready { get; private set; } = false;

        private static readonly Color Invisible = new Color(0, 0, 0, 0);

        public void Init()
        {
            map = GetComponent<Map>();
            grid = GetComponent<HexGrid>();
            sprites = new OverlayedSprite[map.SizeX, map.SizeY];
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    sprites[i, j] = grid[i, j].GetComponent<OverlayedSprite>();
                }

            Ready = true;
        }

        public void HideAll()
        {
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    sprites[i, j].MaskColor = Invisible;
                }
        }

        public void HideTile(Vector2Int coord)
        {
            if (coord.x < 0 || coord.y < 0 || coord.x >= map.SizeX || coord.y >= map.SizeY)
                return;

            sprites[coord.x, coord.y].MaskColor = Invisible;

        }

        public void ShowTile(int x, int y, Color color, int mark, float alpha = 0.5f)
        {
            if (x < 0 || y < 0 || x >= map.SizeX || y >= map.SizeY)
                return;
            color.a *= alpha;
            sprites[x, y].Mask = Textures[mark];
            sprites[x, y].MaskColor = color;
        }

        public  void ShowTile(Vector2Int coord, Color color, int mark, float alpha = 0.5f)
        {
            ShowTile(coord.x, coord.y, color, mark,alpha);
        }

        public void ShowZone(List<Vector2Int> zone, Color color, int mark, float alpha = 0.5f)
        {
            foreach (var item in zone)
                ShowTile(item, color, mark);
        }
    }
}
