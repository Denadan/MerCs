﻿using Mercs.Debug;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    public class DeployPlaceUnitState : TacticalStateHandler
    {
        private DeployInitState state;
        public override TacticalState State => TacticalState.DeployPlaceUnit;

        private bool unit_under_cursor;

        public DeployPlaceUnitState(DeployInitState state)
        {
            this.state = state;
        }

        public override void TileEnter(Vector2Int coord)
        {
            if (coord.x < 0 || coord.y < 0)
                return;

            state.unit_in_hand.info.gameObject.SetActive(true);
            state.unit_in_hand.info.transform.position = TacticalController.Instance.Grid.CellToWorld(
                new Vector3Int(coord.x, coord.y, TacticalController.Instance.Map[coord.x, coord.y].Height));
            state.unit_in_hand.info.GFX.HighlightColor =
                (state.DeployZone.Contains(coord) && !unit_under_cursor) ? Color.green : Color.red;
        }

        public override void TileLeave(Vector2Int coord)
        {
            state.unit_in_hand.info.gameObject.SetActive(false);
        }

        public override void UnitLeave(UnitInfo unit)
        {
            unit_under_cursor = false;
        }

        public override void UnitEnter(UnitInfo unit)
        {
            unit_under_cursor = true;
        }

        public override void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
            if (unit_under_cursor || coord.x < 0 || coord.y < 0 || !state.DeployZone.Contains(coord))
                return;
            if (button == PointerEventData.InputButton.Right)
            {
                Cancel();
                return;
            }
            if (state.units.Find(u => u.Position.position == coord) != null)
                return;

            state.unit_in_hand.info.GFX.AddCollider();
            state.unit_in_hand.info.Reserve = false;
            state.unit_in_hand.button.Background.color = Color.green;
            state.unit_in_hand.button.BottomText.text = "DEPLOY";
            state.unit_in_hand.info.GFX.HighlightColor = Color.white;
            state.unit_in_hand.Position.position = coord;

            TacticalController.Instance.Overlay.HideTile(coord);
            SwitchTo(TacticalState.DeployRotation);
        }


        public override void Update()
        {
            if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
            {
                Cancel();
            }

        }

        private void Cancel()
        {
            state.unit_in_hand.info.gameObject.SetActive(false);
            state.unit_in_hand.button.Background.color = Color.white;
            state.unit_in_hand = null;
            TacticalUIController.Instance.HideSelectedUnitWindow();
            SwitchTo(TacticalState.DeploySelectUnit);
        }
    }
}
