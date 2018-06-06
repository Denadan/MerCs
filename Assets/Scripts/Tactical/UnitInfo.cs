using System.Collections;
using Mercs.Sprites;
using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(CellPosition))]
    [RequireComponent(typeof(MovementData))]
    public class UnitInfo : MonoBehaviour
    {
        public CellPosition Position;
        public MovementData Movement;
        public PilotHp PilotHP;

        public Faction Faction { get; set; }
        public bool Active { get; set; }
        public bool Reserve { get; set; }
        public string PilotName { get; set; }
        public int Weight { get; set; }
        public bool Selectable { get; set; }


        private void Start()
        {
            var camo = GetComponent<CamoSprite>();
            camo.CamoTemplate = Faction.CamoTemplate;
            camo.ColorB = Faction.CamoColorB;
            camo.ColorR = Faction.CamoColorR;
            camo.ColorG = Faction.CamoColorG;
        }

    }

}