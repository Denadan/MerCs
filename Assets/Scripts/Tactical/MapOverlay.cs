#pragma warning disable 649

using System;
using System.Collections.Generic;
using System.Linq;
using Denadan.Sprites;
using UnityEngine;



namespace Mercs.Tactical
{
    [RequireComponent(typeof(HexGrid))]
    [RequireComponent(typeof(Map))]
    public class MapOverlay : MonoBehaviour
    {
        public enum Texture
        {
            White25 = 0, White50 = 1, White100 = 2, GridMark = 3, Stroke = 4,

            SectorN = 5, SectorNE = 6, SectorSE = 7, SectorS = 8, SectorSW = 9, SectorNW = 10,
            ArrowN = 11, ArrowNE = 12, ArrowSE = 13, ArrowS = 14, ArrowSW = 16, ArrowNW = 16,
            HArrowN = 17, HArrowNE = 18, HArrowSE = 19, HArrowS = 20, HArrowSW = 21, HArrowNW = 22,
            Sector2N = 23, Sector2NE = 24, Sector2SE = 25, Sector2S = 26, Sector2SW = 27, Sector2NW = 28,
            Sector3N = 29, Sector3NE = 30, Sector3SE = 31, Sector3S = 32, Sector3SW = 33, Sector3NW = 34,
            SectorFN = 35, SectorFNE = 36, SectorFSE = 37, SectorFS = 38, SectorFSW = 39, SectorFNW = 40,
        }

        private Map map;
        private HexGrid grid;
        private OverlayedSprite[,] sprites;

        [Header("Colors")]
        [SerializeField]
        private Color MoveColor = new Color(0,1,0,0.33f);
        [SerializeField]
        private Color RunColor = new Color(1, 1, 0, 0.33f);
        [SerializeField]
        private Color JumpColor = new Color(1, 0.5f, 0, 0.33f);

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

        [Header("Directions")]
        [SerializeField]
        private Texture2D[] Sectors;
        [SerializeField]
        private Texture2D[] Arrows;
        [SerializeField]
        private Texture2D[] HArrows;
        [SerializeField]
        private Texture2D[] Sectors2;
        [SerializeField]
        private Texture2D[] Sectors3;
        [SerializeField]
        private Texture2D[] SectorsF;
        [SerializeField]
        private Texture2D[] HexParts;

        private Texture2D[] textures;

        public bool Ready { get; private set; } = false;

        public static Texture Arrow(Dir dir)
        {
            return Texture.ArrowN + (int) dir - 1;
        }
        public static Texture HArrow(Dir dir)
        {
            return Texture.HArrowN + (int)dir - 1;
        }
        public static Texture Sector(Dir dir)
        {
            return Texture.SectorN + (int)dir - 1;
        }
        public static Texture Sector2(Dir dir)
        {
            return Texture.Sector2N + (int)dir - 1;
        }
        public static Texture Sector3(Dir dir)
        {
            return Texture.Sector3N + (int)dir - 1;
        }
        public static Texture SectorF(Dir dir)
        {
            return Texture.SectorFN + (int)dir - 1;
        }

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
                White25,White50,White100,GridMark,Stroke,
                Sectors[0], Sectors[1],Sectors[2], Sectors[3],Sectors[4], Sectors[5],
                Arrows[0], Arrows[1],Arrows[2], Arrows[3],Arrows[4], Arrows[5],
                HArrows[0], HArrows[1],HArrows[2], HArrows[3],HArrows[4], HArrows[5],
                Sectors2[0], Sectors2[1],Sectors2[2], Sectors2[3],Sectors2[4], Sectors2[5],
                Sectors3[0], Sectors3[1],Sectors3[2], Sectors3[3],Sectors3[4], Sectors3[5],
                SectorsF[0], SectorsF[1],SectorsF[2], SectorsF[3],SectorsF[4], SectorsF[5],
            };
        }

        /// <summary>
        /// убрать всю подсветку
        /// </summary>
        public void HideAll()
        {
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    sprites[i, j].MaskColor = Invisible;
                }
        }

        /// <summary>
        /// скрыть подсветку тайла
        /// </summary>
        /// <param name="coord"></param>
        public void HideTile(Vector2Int coord)
        {
            if (coord.x < 0 || coord.y < 0 || coord.x >= map.SizeX || coord.y >= map.SizeY)
                return;

            sprites[coord.x, coord.y].MaskColor = Invisible;

        }

        /// <summary>
        /// подсветить тайл
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        public void ShowTile(int x, int y, Color color, Texture2D texture)
        {
            if (x < 0 || y < 0 || x >= map.SizeX || y >= map.SizeY)
                return;
            sprites[x, y].Mask = texture;
            sprites[x, y].MaskColor = color;
        }

        /// <summary>
        /// подсветить тайл
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        public void ShowTile(Vector2Int coord, Color color, Texture2D texture)
        {
            ShowTile(coord.x, coord.y, color, texture);
        }

        /// <summary>
        /// подсветить список тайлов
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        public void ShowZone(List<Vector2Int> zone, Color color, Texture2D texture)
        {
            foreach (var item in zone)
                ShowTile(item, color, texture);
        }

        /// <summary>
        /// подсветить тайл
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        public void ShowTile(int x, int y, Color color, Texture texture)
        {
            ShowTile(x, y, color, textures[(int)texture]);
        }

        /// <summary>
        /// подсветить тайл
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        public void ShowTile(Vector2Int coord, Color color, Texture texture)
        {
            ShowTile(coord.x, coord.y, color, textures[(int)texture]);
        }

        /// <summary>
        /// подсветить список тайлов
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="color"></param>
        /// <param name="texture"></param>
        public void ShowZone(List<Vector2Int> zone, Color color, Texture texture)
        {
            foreach (var item in zone)
                ShowTile(item, color, textures[(int)texture]);
        }

        private Texture2D get_tex(PathMap.path_target item)
        {
            int n = 0;
            var dirs = item.AllowedDir;
            if (dirs.Contains(Dir.N))
                n += 1;
            if (dirs.Contains(Dir.NE))
                n += 2;
            if (dirs.Contains(Dir.SE))
                n += 4;
            if (dirs.Contains(Dir.S))
                n += 8;
            if (dirs.Contains(Dir.SW))
                n += 16;
            if (dirs.Contains(Dir.NW))
                n += 32;
            return HexParts[n];
        }

        /// <summary>
        /// нарисовать карту путей для бега
        /// </summary>
        public void ShowMoveMapRun()
        {
            if (TacticalController.Instance.Path.RunList != null)
                foreach (var item in TacticalController.Instance.Path.RunList)
                {
                    sprites[item.coord.x, item.coord.y].Mask = get_tex(item);
                    sprites[item.coord.x, item.coord.y].MaskColor = RunColor;
                }
        }

        /// <summary>
        /// нарисовать карту путей для бега
        /// </summary>
        public void ShowMoveMapMove()
        {
            if (TacticalController.Instance.Path.MoveList != null)
                foreach (var item in TacticalController.Instance.Path.MoveList)
                {
                    sprites[item.coord.x, item.coord.y].Mask = get_tex(item);
                    sprites[item.coord.x, item.coord.y].MaskColor = MoveColor;
                }
        }

        /// <summary>
        /// нарисовать карту путей для бега
        /// </summary>
        public void ShowMoveMapJump()
        {
            if (TacticalController.Instance.Path.RunList != null)
                foreach (var item in TacticalController.Instance.Path.JumpList)
                {
                    sprites[item.coord.x, item.coord.y].Mask = HexParts[63];
                    sprites[item.coord.x, item.coord.y].MaskColor = JumpColor;
                }
        }
    }
}
