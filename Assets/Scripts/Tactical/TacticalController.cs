#pragma warning disable 649

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mercs.Tactical.Buffs;
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
        private UnitInfo selected;

        #region Properties
        public Map Map { get; private set; }
        public HexGrid Grid { get; private set; }
        public MapOverlay Overlay { get; private set; }
        public PathMap Path { get; private set; }
        public TacticalStateMachine StateMachine { get; private set; }
        public bool Ready { get; private set; }

        public UnitInfo SelectedUnit
        {
            get => selected;
            set
            {
                selected = value;
                if (value == null)
                {
                    SelectionMark.SetParent(transform, false);
                    SelectionMark.gameObject.SetActive(false);
                    TacticalUIController.Instance.HideSelectedUnitWindow();
                }
                else
                {
                    SelectionMark.SetParent(selected.transform, false);
                    SelectionMark.gameObject.SetActive(true);
                    if (value.Faction == GameController.Instance.PlayerFaction)
                    {
                        TacticalUIController.Instance.ShowSelectedUnitWindow(value);
                        TacticalUIController.Instance.MoveCameraTo(value);
                    }
                    else if(value.Vision >= Visibility.Sensor)
                        TacticalUIController.Instance.MoveCameraTo(value);
                }
            }
        }
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


        public IEnumerable<UnitInfo> PlayerUnits { get; private set; }
        public IEnumerable<UnitInfo> EnemyUnits { get; private set; }

        #region Inspector
        [SerializeField]
        private Transform SelectionMark;
        [SerializeField]
        private Transform TargetMark;

        public GameObject MapPrefab;
        [SerializeField]
        private GameObject PlayerMechPrefab;
        [SerializeField]
        private GameObject EnemyMechPrefab;
        #endregion


        public void Start()
        {
            var map_obj = Instantiate(MapPrefab);
            Map = map_obj.GetComponent<Map>();
            Grid = map_obj.GetComponent<HexGrid>();
            StateMachine = GetComponent<TacticalStateMachine>();
            Overlay = map_obj.GetComponent<MapOverlay>();
            Path = map_obj.GetComponent<PathMap>();

            Units.Clear();
            foreach (var item in GameController.Instance.Mechs)
            {
                UnitInfo info = UnitContructor.Build(PlayerMechPrefab, item.Merc, item.Pilot);
                info.transform.SetParent(Grid.UnitsParent, false);
                info.gameObject.SetActive(false);
                info.Faction = GameController.Instance.PlayerFaction;

                Units.Add(info);
            }

            PlayerUnits = from unit in Units
                          where unit.Faction == GameController.Instance.PlayerFaction
                          select unit;

            EnemyUnits = from unit in Units
                         where unit.Faction == GameController.Instance.EnemyFaction
                         select unit;

        }



        public void HighlightUnit(UnitInfo info)
        {
            if (SelectedUnit != info)
            {
                TargetMark.gameObject.SetActive(true);
                TargetMark.SetParent(info.transform, false);
            }
        }

        public void HideHighlatedUnit(UnitInfo info)
        {
            TargetMark.SetParent(this.transform, false);
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

            foreach (var item in GameController.Instance.EnemyMechs)
            {
                var unit = UnitContructor.Build(EnemyMechPrefab, item.Merc, item.Pilot);
                unit.transform.SetParent(Grid.UnitsParent, false);
                unit.Faction = GameController.Instance.EnemyFaction;
                // TODO: VISIBILITY!
                unit.Vision = Visibility.Visual;

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

                Units.Add(unit);
            }
        }

        public void StartMovement(MovementStateData data, UnitInfo unit)
        {
            StartMovementWait(data, unit, TacticalState.NotReady);
        }

        public void StartMovementWait(MovementStateData data, UnitInfo unit, TacticalState state)
        {
            if (data.Type == MovementStateData.MoveType.Jump)
                StartCoroutine(Jump(data,unit,state));
            else
                StartCoroutine(Move(data, unit, state));
        }

        private IEnumerator Move(MovementStateData data, UnitInfo info, TacticalState state)
        {
            var dir = data.dir;
            var path = data.path;
            path[0].facing = dir;
            var runninig = data.Type == MovementStateData.MoveType.Run;
            int length = path.Count - 1;
            var speed = runninig ? 0.4f : 0.6f;
            var total_time = speed * length;
            info.Position.position = data.target.coord;
            info.Position.Facing = data.dir;

            var start_time = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < start_time + total_time)
            {
                var t = (1 - (Time.realtimeSinceStartup - start_time) / total_time) * length - 0.0001f;


                var ns = path[(int) t];
                var ne = path[(int) t + 1];
                var t1 = t - (int) t;

                info.transform.position = Vector3.Lerp(
                    Grid.CellToWorld(ns.coord), Grid.CellToWorld(ne.coord), t1
                    );

                if (!runninig)
                    t1 = Mathf.Clamp(t1 * 2 - 1, 0, 1);
                var angle_start = Quaternion.Euler(0, 0, ns.facing.GetAngleV());
                var angle_end = Quaternion.Euler(0, 0, ne.facing.GetAngleV());
                info.transform.rotation = Quaternion.Lerp(angle_start, angle_end, t1);
                
                yield return 0;
            }

            //if (data.dir != path[0].facing)
            //{
            //    total_time = 0.5f;
            //    start_time = Time.realtimeSinceStartup;
            //    var angle_start = Quaternion.Euler(0, 0, path[0].facing.GetAngleV());
            //    var angle_end = Quaternion.Euler(0, 0, data.dir.GetAngleV());

            //    while (Time.realtimeSinceStartup < start_time + total_time)
            //    {
            //        var t = (Time.realtimeSinceStartup - start_time) / total_time;
            //        info.transform.rotation = Quaternion.Lerp(angle_start, angle_end, t);

            //        yield return 0;
            //    }
            //}

            info.transform.position = Grid.CellToWorld(path[0].coord);
            info.Position.SetFacing(dir);
            if (state != TacticalState.NotReady)
            {
                yield return new WaitForSeconds(0.5f);
                StateMachine.State = state;
            }
        }

        private IEnumerator Jump(MovementStateData data, UnitInfo info, TacticalState state)
        {
            var target_dir = data.dir;
            var source_dir = info.Position.Facing;
            var source_coord = info.Position.position;
            var target_coord = data.target.coord;
            info.Position.position = target_coord;
            info.Position.Facing = target_dir;


            var start = Grid.CellToWorld(source_coord);
            var end = Grid.CellToWorld(target_coord);

            var start_angle = Quaternion.Euler(new Vector3(0, 0, source_dir.GetAngleV()));
            var end_angle = Quaternion.Euler(new Vector3(0, 0, target_dir.GetAngleV()));

            float total_time = 0.75f + (end - start).magnitude / 4;
            float start_time = Time.realtimeSinceStartup;

            yield return 0;

            while (Time.realtimeSinceStartup < start_time + total_time)
            {
                float time = (Time.realtimeSinceStartup - start_time) / total_time;
                info.transform.rotation = Quaternion.Lerp(start_angle, end_angle, time);
                info.transform.position = Vector3.Lerp(start, end, time);
                yield return 0;
            }

            info.transform.position = end;
            info.Position.SetFacing(target_dir);
            if (state != TacticalState.NotReady)
            {
                yield return new WaitForSeconds(0.5f);
                StateMachine.State = state;
            }
        }

        internal UnitInfo UnitAt(Vector2Int p)
        {
            return Units.Find(i => i.Position.position == p);
        }
    }
}