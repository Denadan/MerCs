using Mercs.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Mercs
{
    /// <summary>
    /// особености тайла
    /// </summary>
    public enum TileFeature
    {
        None, Road, Forest, Rough, Water, Cover, Pillar
    }

    public enum MercClass
    {
        Scout = 0,      // 25- 34
        Recon = 1,      // 35- 44
        Light = 2,      // 45- 54

        Medium1 = 3,    // 55- 64
        Medium2 = 4,    // 65- 74

        Heavy1 = 5,     // 75- 84
        Heavy2 = 6,     // 85- 94

        Assault = 7,    // 96-104
        Dreadnought = 8,//105-114
        Behemot = 9     //115-125
    }

    /// <summary>
    /// Кнопки Экшен бара
    /// </summary>
    public enum ActionButton
    {
        Move,
        Run,
        Jump,
        Fire,
        Guard,
        Cancel,
        Evade,
    }

    public enum TacticalButton
    {
        Confirm,
        Reserve,
        Done,
        Fire,
        EngineOn,
        StandUp,
    }
    /// <summary>
    /// Части юнитов
    /// </summary>
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
        public static readonly Dictionary<TileFeature, int> MoveCost = new Dictionary<TileFeature, int>
        {
            [TileFeature.None] = 1,
            [TileFeature.Road] = 1,
            [TileFeature.Water] = 2,
            [TileFeature.Rough] = 2,
            [TileFeature.Forest] = 2,
            [TileFeature.Cover] = 1,
            [TileFeature.Pillar] = -1,
        };

        public static float ClassToEvasion(MercClass Class)
        {
            if (Class == MercClass.Medium2 || Class == MercClass.Heavy1)
                return 0;

            if (Class < MercClass.Medium1)
                return (float) (Class - MercClass.Medium2) / 2f;

            return (float) (Class - MercClass.Heavy1) / 2f;
        }

        /// <summary>
        /// возвращает цвет по количеству хп
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Color GetColor(float cur, float max)
        {
            if (max == 0 || cur == 0)
                return Color.black;

            switch (cur / max)
            {
                case float i when i >= 0.95f:
                    return Color.white;
                case float i when i >= 0.8f:
                    return Color.green;
                case float i when i >= 0.5f:
                    return Color.yellow;
                case float i when i > 0.25f:
                    return new Color(1f, 0.33f, 0);
                default:
                    return Color.red;
            }
        }

        /// <summary>
        /// класс по весу
        /// </summary>
        /// <param name="Weight"></param>
        /// <returns></returns>
        public static MercClass Class(int Weight)
        {
            return (MercClass)((Weight - 25) / 10);
        }

        /// <summary>
        /// инциатитва по классу
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        public static int Initiative(MercClass Class)
        {
            switch (Class)
            {
                case MercClass w when w <= MercClass.Recon:
                    return 1;
                case MercClass w when w <= MercClass.Medium1:
                    return 2;
                case MercClass w when w <= MercClass.Heavy1:
                    return 3;
                case MercClass w when w <= MercClass.Assault:
                    return 4;
                default:
                    return 5;
            }

        }

        /// <summary>
        /// видимость модуля снаружи
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static bool Visible(IModuleInfo module)
        {
            return module.Slot <= SlotSize.Five;
        }

        /// <summary>
        /// точка пересечения прямых
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="b1"></param>
        /// <param name="a2"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static (bool intersect, Vector2 point) Inersect(Vector2 a1, Vector2 b1, Vector2 a2, Vector2 b2)
        {
            float px1 = a1.x - b1.x;
            float py1 = a1.y - b1.y;

            float px2 = a2.x - b2.x;
            float py2 = a2.y - b2.y;

            float d = px1 * py2 - px2 * py1;
            if (d == 0)
                return (false, Vector2.zero);

            float d1 = a1.x * b1.y - a1.y * b1.x;
            float d2 = a2.x * b2.y - a2.y * b2.x;

            return (true, new Vector2((d1 * px2 - d2 * px1) / d, (d1 * py2 - d2 * py1) / d));
        }

        /// <summary>
        /// возвращает параметрическое положение точки на прямой между a(t=0) и b(t=1)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float T(Vector2 point, Vector2 a, Vector2 b)
        {
            if (b.x - a.x == 0)
                if (b.y - a.y == 0)
                    return 0f;
                else
                    return (point.y - a.y) / (b.y - a.y);
            else
                return (point.x - a.x) / (b.x - a.x);

        }

        /// <summary>
        /// возвращает положение точки на прямой между a(t=0) и b(t=1)
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector2 T(float t,Vector2 a, Vector2 b)
        {
            return a + (b - a) * t;
        }
    }
}
