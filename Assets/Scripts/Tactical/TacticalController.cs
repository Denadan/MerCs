using System;
using System.Collections.Generic;
using System.Linq;
using Mercs.Tactical.States;
using Mercs.Tactical.UI;
using Tools;
using UnityEngine;


namespace Mercs.Tactical
{
    public class TacticalController : SceneSingleton<TacticalController>
    {
        private Dictionary<UnitInfo, UnitSelectButton> unit_buttons;
        private Faction[] factions;
        private int current_faction;

        #region Properties
        public Map Map { get; private set; }
        public HexGrid Grid { get; private set; }
        public MapOverlay Overlay { get; private set; }
        public TacticalStateMachine StateMachine { get; private set; }
        public bool Ready { get; private set; }
        public UnitInfo SelectedUnit { get; private set; }
        public int CurrentPhase { get; set; }
        public int CurrentRound { get; set; }
        public Faction CurrentFaction { get => factions[current_faction]; }


        #endregion

        public UnitSelectButton this[UnitInfo unit]
        {
            get
            {
                if (unit_buttons.TryGetValue(unit, out var item))
                    return item;
                return null;
            }
        }

        [HideInInspector]
        public List<UnitInfo> Units = new List<UnitInfo>();


        #region Inspector
        [SerializeField]
        private Transform SelectionMark;
        [SerializeField]
        private Transform TargetMark;
        
        public GameObject MapPrefab;
        [SerializeField]
        private GameObject MechPrefab;
        #endregion


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
            unit.GetComponent<SpriteRenderer>().sprite = item.Merc.Sprite;
            var info = unit.GetComponent<UnitInfo>();
            info.Faction = faction;
            info.PilotName = item.Pilot.name;
            info.Active = false;
            info.PilotHP.Init(item);
            info.Weight = item.Merc.Weight;
            info.Reserve = true;
            info.Position.position = new Vector2Int(-1, -1);
            info.Movement.MoveMp = item.Merc.MoveSpeed;
            info.Movement.JumpMP = item.Merc.Jumps;
            info.Movement.RunMP = item.Merc.RunSpeed;
            info.gameObject.SetActive(false);
            return info;
        }

        public bool SelectUnit(UnitInfo info)
        {
            if(info.Selectable)
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

            TacticalUIController.Instance.HideDeployWindow();

            DeployEnemyForce();
            MakeButtons();
            CurrentRound = 0;
            factions = new Faction[] { GameController.Instance.PlayerFaction, GameController.Instance.EnemyFaction };
            current_faction = UnityEngine.Random.Range(0, factions.Length);

            StateMachine.State = TacticalState.TurnPrepare;
        }

        public Faction NextFaction()
        {
            current_faction = (current_faction + 1) % factions.Length;
            return factions[current_faction];
        }

        private void MakeButtons()
        {
            TacticalUIController.Instance.ClearUnitList();
            unit_buttons = (
                from unit in Units
                where unit.Faction == GameController.Instance.PlayerFaction
                    && !unit.Reserve
                select TacticalUIController.Instance.AddUnit(unit)).ToDictionary(item => item.Unit);
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
                unit.Reserve = false;

                unit.gameObject.SetActive(true);
                Units.Add(unit);
            }
        }
    }
}