﻿using System.Collections.Generic;
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

        private Texture2D[] Textures;

        private static readonly Color Invisible = new Color(0, 0, 0, 0);

        public void Init()
        {
            map = GetComponent<Map>();
            sprites = new OverlayedSprite[map.SizeX, map.SizeY];
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
            {
                sprites[coord.x, coord.y].MaskColor = Invisible;
            }
        }

        private void ShowTile(Vector2Int coord, Color color, int mark)
        {
            if (coord.x < 0 || coord.y < 0 || coord.x >= map.SizeX || coord.y >= map.SizeY)
            {
                sprites[coord.x, coord.y].Mask = Textures[mark];
                sprites[coord.x, coord.y].MaskColor = color;
            }
        }

        private void ShowZone(List<Vector2Int> zone, Color color, int mark)
        {
            foreach (var item in zone)
                ShowTile(item, color, mark);
        }
    }
}
