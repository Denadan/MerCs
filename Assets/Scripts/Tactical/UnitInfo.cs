using System.Collections;
using System.Collections.Generic;
using Mercs.Items;
using Mercs.Sprites;
using Mercs.Tactical.Buffs;
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
        public UnitHp UnitHP;
        public BuffList Buffs;


        public Visibility Vision;
        

        public Faction Faction { get; set; }
        public bool Active { get; set; }
        public bool Reserve { get; set; }
        public string PilotName { get; set; }
        public int Weight { get; set; }
        public bool Selectable { get; set; }
        public List<IModuleInfo> Modules { get; set; }
        public Reactor Engine { get; set; }
        public MercClass Class => CONST.Class(Weight);
        public UnitType Type { get; set; }

        private void Start()
        {
            var camo = GetComponent<CamoSprite>();
            UnityEngine.Debug.Log($"{PilotName}: {camo}");
            camo.CamoTemplate = Faction.CamoTemplate;
            camo.ColorB = Faction.CamoColorB;
            camo.ColorR = Faction.CamoColorR;
            camo.ColorG = Faction.CamoColorG;
        }

    }

}