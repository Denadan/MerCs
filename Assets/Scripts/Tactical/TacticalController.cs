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
        [HideInInspector]
        public List<UnitInfo> Units = new List<UnitInfo>();
        public UnitInfo SelectedUnit { get; private set; }
        public TacticalStateMachine StateMachine { get; private set; }

        [SerializeField]
        private Transform SelectionMark;
        [SerializeField]
        private Transform TargetMark;

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
                SelectionMark.SetParent(info.transform, false);
                SelectionMark.gameObject.SetActive(true);
                return false;
            }
            return false;

        }

        public void HighlightUnit(UnitInfo info)
        {
            if(SelectedUnit != info)
            {
                TargetMark.SetParent(info.transform,false);
                TargetMark.gameObject.SetActive(true);
            }
        }

        public void HideHighlatedUnit(UnitInfo info)
        {
            TargetMark.gameObject.SetActive(false);
        }
    }
}