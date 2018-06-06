using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static Mercs.Tactical.PathMap;

namespace Mercs.Tactical.States
{
    public abstract class PlayerSelectMovementBase : TacticalStateHandler
    {
        protected PhasePrepareState state;

        public PlayerSelectMovementBase(PhasePrepareState state)
        {
            this.state = state;
        }

        public override void OnLoad()
        {
            TacticalController.Instance.Overlay.HideAll();
            if (TacticalController.Instance.Path.Ready)
                ShowOverlay();
            else
                TacticalController.Instance.StateMachine.StartCoroutine(wait_for_path(TacticalController.Instance.SelectedUnit));
        }

        private IEnumerator wait_for_path(UnitInfo info)
        {
            float start = TacticalController.Instance.Path.DEBUG_TimeStart;

            UnityEngine.Debug.Log($"Started");
            float end = 0;
            while (!TacticalController.Instance.Path.Ready)
            {
                if (TacticalController.Instance.StateMachine.State != State ||
                    TacticalController.Instance.SelectedUnit != info)
                {
                    end = Time.realtimeSinceStartup;
                    UnityEngine.Debug.Log($"State changed\nStart at {start:0.000}s\nEnd at {end:0.000}s\nTotal: {end - start:0.000}s");

                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
            end = Time.realtimeSinceStartup;
            UnityEngine.Debug.Log($"Task Completed\nStart at {start:0.000}s\nEnd at {end:0.000}s\nTotal: {end - start:0.000}s");
            ShowOverlay();
        }

        public override void UnitEnter(UnitInfo unit)
        {
            if (unit.Selectable)
                TacticalController.Instance.HighlightUnit(unit);
        }

        public override void UnitLeave(UnitInfo unit)
        {
            TacticalController.Instance.HideHighlatedUnit(unit);
        }

        public override void TileEnter(Vector2Int coord)
        {
            var list = GetPath(coord);
            if (list != null && list.Count >= 2)
            {
                var line = TacticalUIController.Instance.MoveLine;
                line.gameObject.SetActive(true);
                line.startColor = Color.red;
                line.endColor = Color.green;
                var points = (from v2 in list
                              select TacticalController.Instance.Grid.CellToWorld(v2.coord)).ToArray();
                line.positionCount = points.Length;
                line.SetPositions(points);
            }
        }

        public override void TileLeave(Vector2Int coord)
        {
            TacticalUIController.Instance.MoveLine.gameObject.SetActive(false);
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            if (unit.Selectable && unit != TacticalController.Instance.SelectedUnit && button == PointerEventData.InputButton.Left)
            {
                TacticalController.Instance[TacticalController.Instance.SelectedUnit].Background.color = Color.white;
                TacticalController.Instance.SelectedUnit = unit;
                TacticalUIController.Instance.ShowActionBarButtons(unit);
                TacticalUIController.Instance.ShowActionBarButton(ActionButton.Cancel);
                TacticalController.Instance[unit].Background.color = Color.green;
                TacticalController.Instance.Path.MakePathMap(unit);

                OnLoad();
            }

        }

        public override void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Right &&
                state.ActiveUnits.Count(unit => unit.Faction == GameController.Instance.PlayerFaction) > 1)
            {
                CancelSelection();
            }
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(1) &&
               state.ActiveUnits.Count(unit => unit.Faction == GameController.Instance.PlayerFaction) > 1 &&
               !EventSystem.current.IsPointerOverGameObject())
                CancelSelection();
        }

        private void CancelSelection()
        {
            TacticalUIController.Instance.HideActionBar();
            TacticalController.Instance[TacticalController.Instance.SelectedUnit].Background.color = Color.white;
            TacticalController.Instance.SelectedUnit = null;
            TacticalController.Instance.Path.MakePathMap(null);

            SwitchTo(TacticalState.SelectUnit);
        }

        protected abstract void ShowOverlay();
        protected abstract List<path_node> GetPath(Vector2Int coord);

    }
}