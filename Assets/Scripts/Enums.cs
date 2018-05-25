using System;
using System.Collections.Generic;
using Mercs.Tactical;
using UnityEngine;

namespace Mercs
{
    /// <summary>
    /// Направления 
    /// </summary>
    public enum Dir
    {
        N, S, E, W,
        NE, NW, SE, SW
    }

    /// <summary>
    /// Ориентация хексагональной сетки
    /// </summary>
    public enum HexOrinetation { Vertical, Horizontal }

    /// <summary>
    /// особености тайла
    /// </summary>
    public enum TileFeature
    {
        None, Road, Forest, Rough, Water, Cover, Pillar
    }


    public static class CONST
    {
        /// <summary>
        /// список направлений для вертикальной гексагональной сетки
        /// </summary>
        public static readonly Dir[] VerticalDir = { Dir.N, Dir.NE, Dir.SE, Dir.S, Dir.SW, Dir.NW};
        /// <summary>
        /// список направлений для горизонтальной гексагональной сетки
        /// </summary>
        public static readonly Dir[] HorizontalDir = { Dir.E, Dir.SE, Dir.SW, Dir.W, Dir.NW, Dir.NE };
        /// <summary>
        /// Стоимость движения по различным типам тайлов
        /// </summary>
        public static readonly Dictionary<TileFeature, int> MoveCose = new Dictionary<TileFeature, int>
        {
            [TileFeature.None] = 1,
            [TileFeature.Road] = 1,
            [TileFeature.Water] = 2,
            [TileFeature.Rough] = 2,
            [TileFeature.Forest] = 2,
            [TileFeature.Cover] = 1,
            [TileFeature.Pillar] = -1,
        };

        private static readonly Dictionary<Dir, Vector2Int> ver_shifts_even = new Dictionary<Dir, Vector2Int>
        {
            [Dir.N] = new Vector2Int(0, 1),
            [Dir.NE] = new Vector2Int(1, 0),
            [Dir.NW] = new Vector2Int(-1, 0),
            [Dir.S] = new Vector2Int(0, -1),
            [Dir.SE] = new Vector2Int(1, -1),
            [Dir.SW] = new Vector2Int(-1, -1)
        };
        private static readonly Dictionary<Dir, Vector2Int> ver_shifts_odd = new Dictionary<Dir, Vector2Int>
        {
            [Dir.N] = new Vector2Int(0, 1),
            [Dir.NE] = new Vector2Int(1, 1),
            [Dir.NW] = new Vector2Int(-1, 1),
            [Dir.S] = new Vector2Int(0, -1),
            [Dir.SE] = new Vector2Int(1, 0),
            [Dir.SW] = new Vector2Int(-1, 0)
        };
        private static readonly Dictionary<Dir, Vector2Int> hor_shifts_even = new Dictionary<Dir, Vector2Int>
        {
            [Dir.E] = new Vector2Int(1, 0),
            [Dir.NE] = new Vector2Int(0, 1),
            [Dir.NW] = new Vector2Int(-1, 1),
            [Dir.W] = new Vector2Int(-1, 0),
            [Dir.SE] = new Vector2Int(0, -1),
            [Dir.SW] = new Vector2Int(-1, -1)
        };
        private static readonly Dictionary<Dir, Vector2Int> hor_shifts_odd = new Dictionary<Dir, Vector2Int>
        {
            [Dir.E] = new Vector2Int(1, 0),
            [Dir.NE] = new Vector2Int(1, 1),
            [Dir.NW] = new Vector2Int(0, 1),
            [Dir.W] = new Vector2Int(-1, 0),
            [Dir.SE] = new Vector2Int(1, -1),
            [Dir.SW] = new Vector2Int(0, -1)
        };

        public static Vector2Int GetDirShift(int x, int y, Dir dir, HexOrinetation orientation)
        {
            try
            {
                switch (orientation)
                {
                    case HexOrinetation.Horizontal:
                        return y % 2 == 0 ? hor_shifts_even[dir] : hor_shifts_odd[dir];
                    case HexOrinetation.Vertical:
                        return x % 2 == 0 ? ver_shifts_even[dir] : ver_shifts_odd[dir];
                }
            }
            catch (KeyNotFoundException)
            { }
            return Vector2Int.zero;
        }

        public static float GetAngleV(Dir dir)
        {
            var angle = 0f;
            switch (dir)
            {
                case Dir.NE:
                    angle = -60;
                    break;
                case Dir.NW:
                    angle = 60;
                    break;
                case Dir.SE:
                    angle = -120;
                    break;
                case Dir.SW:
                    angle = 120;
                    break;
                case Dir.S:
                    angle = 180;
                    break;
            }
            return angle;
        }

        public static Dir GetRotation(Vector3 source, Vector3 dest)
        {



            var vector = dest - source;
            vector.z = 0;
            TacticalUIController.Instance.DebugMenu.Rotation.text = $"{source} => {dest} = {vector}";
            var angle = Vector3.Angle(Vector3.up, vector);
            TacticalUIController.Instance.DebugMenu.Rotation.text += $"/n{angle}";
            
            switch (angle)
            {
                case float a when a < 30f && a > -30f:
                    return Dir.N;
                case float a when a < 90f && a >= 30f:
                    return Dir.NW;
                case float a when a < 150f && a >= 90f:
                    return Dir.NE;
                case float a when a > -90f && a >= -30f:
                    return Dir.SW;
                case float a when a > -150f && a >= -390f:
                    return Dir.SE;
                case float a when a >= 150 || a <= -150f:
                    return Dir.S;
            }

            return Dir.N;
        }
    }
}
