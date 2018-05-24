﻿using Mercs.Sprites;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(CellPosition))]
    [RequireComponent(typeof(MovementData))]
    public class UnitInfo : MonoBehaviour
    {
        public CellPosition Position { get; set; }
        public MovementData Movement { get; set; }
        public Faction Faction { get; set; }
        public bool Active { get; set; }
        public string PilotName { get; set; }

        private void Start()
        {
            Position = GetComponent<CellPosition>();
            Movement = GetComponent<MovementData>();
            var camo = GetComponent<CamoSprite>();
            camo.CamoTemplate = Faction.CamoTemplate;
            camo.ColorB = Faction.CamoColorB;
            camo.ColorR = Faction.CamoColorR;
            camo.ColorG = Faction.CamoColorG;
        }
    }

}