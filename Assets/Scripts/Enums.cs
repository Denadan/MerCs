﻿using System.Collections.Generic;
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

    public enum Visibility
    {
        None, Sensor, Visual, Scanned, Friendly
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
    }
}
