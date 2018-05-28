using System.Collections.Generic;
using System.Linq;
using Mercs.Tactical.States;
using Tools;
using UnityEngine;


namespace Mercs.Tactical
{
    public class TacticalController : SceneSingleton<TacticalController>
    {
        public Map Map { get; private set; }
        public HexGrid Grid { get; private set; }
        public MapOverlay Overlay { get; private set; }
        public TacticalStateMachine StateMachine { get; private set; }
        public bool Ready { get; private set; }

        [HideInInspector]
        public List<UnitInfo> Units = new List<UnitInfo>();
        public UnitInfo SelectedUnit { get; private set; }

        [SerializeField]
        private Transform SelectionMark;
        [SerializeField]
        private Transform TargetMark;
        
        public GameObject MapPrefab;
        [SerializeField]
        private GameObject MechPrefab;

        public void Start()
        {
            var map_obj = Instantiate(MapPrefab);
            Map = map_obj.GetComponent<Map>();
            Grid = map_obj.GetComponent<HexGrid>();
            StateMachine = GetComponent<TacticalStateMachine>();
            Overlay = map_obj.GetComponent<MapOverlay>();
            UnityEngine.Debug.Log(Overlay.ToString());

            Units.Clear();
            foreach(var item in GameController.Instance.Mechs)
            {
                var unit = Instantiate(MechPrefab, Grid.UnitsParent, false);
                unit.GetComponent<SpriteRenderer>().sprite = item.MechSprite;
                var move = unit.GetComponent<MovementData>();
                move.MoveMp = item.MovePoints;
                move.JumpMP = item.JumpPoints;
                move.RunMP = item.RunPoints;
                var info = unit.GetComponent<UnitInfo>();
                info.Faction = GameController.Instance.PlayerFaction;
                info.PilotName = item.Name;
                info.gameObject.SetActive(false);
                info.Active = false;
                unit.GetComponent<CellPosition>().position = new Vector2Int(-1, -1);

                Units.Add(info);
            }
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
                TargetMark.gameObject.SetActive(true);
                TargetMark.SetParent(info.transform,false);
                TargetMark.transform.position = new Vector3(0,0,0);
            }
        }

        public void HideHighlatedUnit(UnitInfo info)
        {
            TargetMark.SetParent(this.transform);
            TargetMark.gameObject.SetActive(false);
        }

        public void FinishDeploy()
        {
            Overlay.HideAll();
            TacticalUIController.Instance.HideDeployWindow();
            StateMachine.State = TacticalState.SelectUnit;
        }

    }
}