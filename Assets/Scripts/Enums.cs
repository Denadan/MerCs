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
        N = 1, S = 4,
        NE = 2, NW = 6, SE = 3, SW = 5
    }

    /// <summary>
    /// особености тайла
    /// </summary>
    public enum TileFeature
    {
        None, Road, Forest, Rough, Water, Cover, Pillar
    }

    public enum Visibility
    {
        Clear, SensorNear, SensorFar, None
    }

    public enum MercClass
    {
        Recon,  //25-40
        Light,  //45-60
        Medium, //65-80
        Heavy,  //85-100
        Behemot //105-120
    }

    /// <summary>
    /// Кнопки Экшен бара
    /// </summary>
    public enum ActionButton
    {
        Move,
        Run,
        Jump,
        Attack,
        Guard,
        Cancel
    }

    public enum TacticalButton
    {
        Confirm,
        Reserve,
        Done
    }

    /// <summary>
    /// Части юнитов
    /// </summary>
    [Flags]
    public enum Parts
    {
        None = 0,

        //Части Меха
        LS = 1 << 0,
        HD = 1 << 1,
        RS = 1 << 2,
        LT = 1 << 3,
        CT = 1 << 4,
        RT = 1 << 5,
        LH = 1 << 6,
        RH = 1 << 7,
        LL = 1 << 8,
        RL = 1 << 9,

        //Части техники
        LC = 1 << 10,
        RC = 1 << 11,
        FC = 1 << 12,
        BC = 1 << 13,
        TC = 1 << 14,
        LM = 1 << 15,
        VC = 1 << 16,
        
        //Турель
        TT = 1 << 17
    }

    public enum UnitType
    {
        MerC,
        Tank,
        Vehicle,
        Turret
    }

    public static class CONST
    {
        /// <summary>
        /// список направлений для вертикальной гексагональной сетки
        /// </summary>
        public static readonly Dir[] AllDirs = { Dir.N, Dir.NE, Dir.SE, Dir.S, Dir.SW, Dir.NW };
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

        public static Vector2Int GetDirShift(Vector2Int coord, Dir dir)
        {
            return GetDirShift(coord.x, coord.y, dir);
        }

        public static Vector2Int GetDirShift(int x, int y, Dir dir)
        {
            return x % 2 == 0 ? shifts_even[dir] : shifts_odd[dir];
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
        public static Dir TurnLeft(Dir dir)
        {
            return neighbour[(int)dir - 1];
        }

        /// <summary>
        /// получить обратне направление
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Dir Inverse(Dir dir)
        {
            return back[(int)dir - 1];
        }

        /// <summary>
        /// получить направление справа
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static Dir TurnRight(Dir dir)
        {
            return neighbour[(int)dir + 1];
        }

    }
}
