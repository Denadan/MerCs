using System.Collections.Generic;
using Mercs.Tactical.Events;
using Mercs.Tactical.States;
using Tools;
using UnityEngine;


namespace Mercs.Tactical
{
    public class TacticalController : SceneSingleton<TacticalController>
    {
        public Map Map { get; set; }
        public HexGrid Grid { get; set; }
        public List<UnitInfo> Units = new List<UnitInfo>();
        public UnitInfo SelectedUnit { get; private set; }
        public TacticalStateMachine StateMachine { get; private set; }

        public GameObject MapPrefab;

        public void Start()
        {
            var map_obj = Instantiate(MapPrefab);
            Map = map_obj.GetComponent<Map>();
            Grid = map_obj.GetComponent<HexGrid>();
            StateMachine = GetComponent<TacticalStateMachine>();
            Units.Clear();
        }
            
        public bool SelectUnit(UnitInfo info)
        {
            if(info.Active)
            {
                SelectedUnit = info;
                return false;
            }
            return false;

        }
    }
}