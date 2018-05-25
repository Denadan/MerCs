using System;
using System.Collections.Generic;
using Denadan.Sprites;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(HexGrid))]
    [RequireComponent(typeof(Map))]
    public class MapOverlay : MonoBehaviour
    {
        public enum Texture { White25 = 0, White50 = 1, White100 = 2, GridMark = 3, Stroke = 4 }

        private Map map;
        private HexGrid grid;
        private OverlayedSprite[,] sprites;

        [Header("Textures")]
        [SerializeField]
        private Texture2D White25;
        [SerializeField]
        private Texture2D White50;
        [SerializeField]
        private Texture2D White100;
        [SerializeField]
        private Texture2D GridMark;
        [SerializeField]
        private Texture2D Stroke;

        private Texture2D[] textures;

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
            textures = new Texture2D[]
            {
                White25,White50,White100,GridMark,Stroke
            };
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

        public void ShowTile(int x, int y, Color color, Texture2D texture)
        {
            if (x < 0 || y < 0 || x >= map.SizeX || y >= map.SizeY)
                return;
            sprites[x, y].Mask = texture;
            sprites[x, y].MaskColor = color;
        }

        public void ShowTile(Vector2Int coord, Color color, Texture2D texture)
        {
            ShowTile(coord.x, coord.y, color, texture);
        }

        public void ShowZone(List<Vector2Int> zone, Color color, Texture2D texture)
        {
            foreach (var item in zone)
                ShowTile(item, color, texture);
        }

        public void ShowTile(int x, int y, Color color, Texture texture)
        {
            ShowTile(x, y, color, textures[(int)texture]);
        }

        public void ShowTile(Vector2Int coord, Color color, Texture texture)
        {
            ShowTile(coord.x, coord.y, color, textures[(int)texture]);
        }

        public void ShowZone(List<Vector2Int> zone, Color color, Texture texture)
        {
            foreach (var item in zone)
                ShowTile(item, color, textures[(int)texture]);
        }
    }
}
