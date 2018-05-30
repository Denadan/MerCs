using System;
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
                UnitInfo info = CreateUnit(item, GameController.Instance.PlayerFaction);

                Units.Add(info);
            }
        }

        private UnitInfo CreateUnit(StartMechInfo item, Faction faction)
        {
            var unit = Instantiate(MechPrefab, Grid.UnitsParent, false);
            unit.GetComponent<SpriteRenderer>().sprite = item.MechSprite;
            var move = unit.GetComponent<MovementData>();
            move.MoveMp = item.MovePoints;
            move.JumpMP = item.JumpPoints;
            move.RunMP = item.RunPoints;
            var info = unit.GetComponent<UnitInfo>();
            info.Faction = faction;
            info.PilotName = item.Name;
            info.Active = false;
            info.Position = info.GetComponent<CellPosition>();

            info.Reserve = true;
            info.Position.position = new Vector2Int(-1, -1);
            info.gameObject.SetActive(false);
            return info;
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
            }
        }

        public void HideHighlatedUnit(UnitInfo info)
        {
            TargetMark.SetParent(this.transform,false);
            TargetMark.gameObject.SetActive(false);
        }

        public void FinishDeploy()
        {
            Overlay.HideAll();

            TacticalUIController.Instance.ClearUnitList();
            TacticalUIController.Instance.HideDeployWindow();

            DeployEnemyForce();

            StateMachine.State = TacticalState.SelectUnit;
        }

        private void DeployEnemyForce()
        {
            RectInt deploy_zone = new RectInt();

            deploy_zone.width = Map.SizeX / 2;
            deploy_zone.height = 2;
            deploy_zone.x = Map.SizeX / 4;
            deploy_zone.y = Map.SizeY - 2;

            foreach(var item in GameController.Instance.EnemyMechs)
            {
                var unit = CreateUnit(item, GameController.Instance.EnemyFaction);
                var coord = unit.GetComponent<CellPosition>();
                Vector2Int c = new Vector2Int();
                do
                {
                    c.x = UnityEngine.Random.Range(deploy_zone.xMin, deploy_zone.xMax);
                    c.y = UnityEngine.Random.Range(deploy_zone.yMin, deploy_zone.yMax);


                } while (Units.Find(u => u.Position.position == c));

                unit.Position.position = c;
                unit.Position.SetFacing(Dir.S);
                unit.transform.position = Grid.CellToWorld(c);
                unit.gameObject.AddComponent<PolygonCollider2D>();

                unit.gameObject.SetActive(true);
                Units.Add(unit);

                var a = (1, "test");
                
            }
        }
    }
}