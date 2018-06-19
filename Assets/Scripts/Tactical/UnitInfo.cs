using System.Collections;
using System.Collections.Generic;
using Mercs.Items;
using Mercs.Sprites;
using Mercs.Tactical.Buffs;
using Mercs.Tactical.Events;
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
        public WeaponsData Weapons;
        public ModulesData Modules;
        public UnitGFX GFX;

        public float RadarRange { get; set; }
        public float VisualRange { get; set; }
        public float ScanRange { get; set; }

        public Faction Faction { get; set; }
        public bool Active { get; set; }
        public bool Reserve { get; set; }
        public string PilotName { get; set; }
        public int Weight { get; set; }
        public float Height => Type == UnitType.MerC ? 1.5f + 0.1f * (int)Class : 1f; 
        public bool Selectable { get; set; }
        public MercClass Class => CONST.Class(Weight);
        public UnitType Type { get; set; }

        public Visibility.Level CurrentVision { get; set; }

        private void Start()
        {
            var camo = GetComponent<CamoSprite>();
        }

    }

}