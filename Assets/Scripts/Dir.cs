using System.Collections.Generic;
using UnityEngine;

namespace Mercs
{
    /// <summary>
    /// Направления 
    /// </summary>
    public enum Dir
    {
        N = 1, S = 4,
        NE = 2, NW = 6, SE = 3, SW = 5,
        E = 10,
        W = 11
    }

    public static class DirHelper
    {
        /// <summary>
        /// список направлений для вертикальной гексагональной сетки
        /// </summary>
        public static readonly Dir[] AllDirs = { Dir.N, Dir.NE, Dir.SE, Dir.S, Dir.SW, Dir.NW };

        private static readonly Dictionary<Dir, Vector2Int> shifts_even = new Dictionary<Dir, Vector2Int>
        {
            [Dir.N] = new Vector2Int(0, 1),
            [Dir.NE] = new Vector2Int(1, 0),
            [Dir.NW] = new Vector2Int(-1, 0),
            [Dir.S] = new Vector2Int(0, -1),
            [Dir.SE] = new Vector2Int(1, -1),
            [Dir.SW] = new Vector2Int(-1, -1)
        };
        private static readonly Dictionary<Dir, Vector2Int> shifts_odd = new Dictionary<Dir, Vector2Int>
        {
            [Dir.N] = new Vector2Int(0, 1),
            [Dir.NE] = new Vector2Int(1, 1),
            [Dir.NW] = new Vector2Int(-1, 1),
            [Dir.S] = new Vector2Int(0, -1),
            [Dir.SE] = new Vector2Int(1, 0),
            [Dir.SW] = new Vector2Int(-1, 0)
        };

        public static Vector2Int GetDirShift(this Dir dir, Vector2Int coord)
        {
            return dir.GetDirShift(coord.x, coord.y);
        }

        public static Vector2Int ShiftTo(this Vector2Int coord, Dir dir)
        {
            return coord + dir.GetDirShift(coord);
        }

        public static Vector2Int GetDirShift(this Dir dir, int x, int y)
        {
            return x % 2 == 0 ? shifts_even[dir] : shifts_odd[dir];
        }

        public static float GetAngleV(this Dir dir)
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

        /// <summary>
        /// получить направление по двум точкам
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public static Dir GetRotation(Vector3 source, Vector3 dest)
        {
            var vector = dest - source;
            vector.z = 0;
            var angle = Vector3.Angle(Vector3.up, vector);
            angle *= Mathf.Sign(vector.x);

            switch (angle)
            {
                case float a when a < 30f && a > -30f:
                    return Dir.N;
                case float a when a < 90f && a >= 30f:
                    return Dir.NE;
                case float a when a < 150f && a >= 90f:
                    return Dir.SE;
                case float a when a > -90f && a <= -30f:
                    return Dir.NW;
                case float a when a > -150f && a <= -90f:
                    return Dir.SW;
                case float a when a >= 150 || a <= -150f:
                    return Dir.S;
            }

            return Dir.N;
        }

        public static (Dir main, Dir second) GetLineDirs(Vector2 source, Vector2 dest)
        {
            var vector = dest - source;
            var angle = Vector2.Angle(Vector2.up, vector);
            angle *= Mathf.Sign(vector.x);

            if (source.x == dest.x)
                if (source.y < dest.y)
                    return (Dir.N, Dir.N);
                else
                    return (Dir.S, Dir.S);

            switch (angle)
            {
                case float a when a < 30f && a > -30f:
                    if (a < 0)
                        return (Dir.N, Dir.NW);
                    else
                        return (Dir.N, Dir.NE);
                case float a when a < 90f && a >= 30f:
                    if (a < 60f)
                        return (Dir.NE, Dir.N);
                    else
                        return (Dir.NE, Dir.SE);
                case float a when a < 150f && a >= 90f:
                    if (a < 120f)
                        return (Dir.SE, Dir.NE);
                    else
                        return (Dir.SE, Dir.S);
                case float a when a > -90f && a <= -30f:
                    if (a > -60f)
                        return (Dir.NW, Dir.N);
                    else
                        return (Dir.NW, Dir.SW);
                case float a when a > -150f && a <= -90f:
                    if (a > -120f)
                        return (Dir.SW, Dir.NW);
                    else
                        return (Dir.SW, Dir.S);
                case float a when a >= 150 || a <= -150f:
                    if (a < 0)
                        return (Dir.S, Dir.SW);
                    else
                        return (Dir.S, Dir.SE);
            }

            return (Dir.N, Dir.N);
        }

        /// <summary>
        /// соседи
        /// </summary>
        private static Dir[] neighbour =
        {
            Dir.NW, Dir.N, Dir.NE, Dir.SE, Dir.S, Dir.SW, Dir.NW, Dir.N
        };

        /// <summary>
        /// обратное направление
        /// </summary>
        private static Dir[] back =
        {
            Dir.S, Dir.SW, Dir.NW, Dir.N, Dir.NE, Dir.SE
        };

        /// <summary>
        /// получить направление слева
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Dir TurnLeft(this Dir dir)
        {
            return neighbour[(int)dir - 1];
        }

        /// <summary>
        /// получить обратне направление
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Dir Inverse(this Dir dir)
        {
            return back[(int)dir - 1];
        }

        /// <summary>
        /// получить направление справа
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Dir TurnRight(this Dir dir)
        {
            return neighbour[(int)dir + 1];
        }
    }
}