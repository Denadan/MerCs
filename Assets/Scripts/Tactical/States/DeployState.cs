using System.Collections;
using System.Linq;
using Mercs.Tactical.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngineInternal.Input;

namespace Mercs.Tactical.States
{
    public class DeployInitState : TacticalStateHandler
    {
        public class unit
        {
            public UnitInfo info;
            public SpriteRenderer renderer;
            public UnitSelectButton button;
            public CellPosition Position;
        }

        public List<unit> units;
        public unit unit_in_hand = null;
        public RectInt DeployZone;

        public override TacticalState State => TacticalState.DeployInit;

        public override void OnLoad()
        {
            base.OnLoad();
            TacticalUIController.Instance.ClearUnitList();
            TacticalUIController.Instance.ShowUnitList();
            units = (from unit_info in TacticalController.Instance.Units
                     select new unit
                     {
                         info = unit_info,
                         renderer = unit_info.GetComponent<SpriteRenderer>(),
                         button = TacticalUIController.Instance.AddUnit(unit_info),
                         Position = unit_info.GetComponent<CellPosition>()
                     }).ToList();
            foreach (var unit in units)
                unit.button.BottomText.text = "RESERVE";

            DeployZone.width = TacticalController.Instance.Map.SizeX / 2;
            DeployZone.x = DeployZone.width / 2;
            DeployZone.y = 0;
            DeployZone.height = 2;
            var center =
                TacticalController.Instance.Grid.CellToWorld(new Vector3Int((int)DeployZone.center.x,
                    (int)DeployZone.center.y, 0));

            Camera.main.transform.position = new Vector3(center.x, center.y, Camera.main.transform.position.z);

            for (int i = DeployZone.xMin; i < DeployZone.xMax; i++)
                for (int j = DeployZone.yMin; j < DeployZone.yMax; j++)
                {
                    TacticalController.Instance.Overlay.ShowTile(new Vector2Int(i, j), Color.yellow, 1, 1f);
                }

            TacticalController.Instance.StateMachine.State = TacticalState.DeploySelectUnit;
        }
    }

    public class DeploySelectUnitState : TacticalStateHandler
    {
        private DeployInitState state;
        public override TacticalState State => TacticalState.DeploySelectUnit;

        public DeploySelectUnitState(DeployInitState state)
        {
            this.state = state;
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left)
            {


                var unit_to_add = state.units.Find(u => u.info == unit);
                if (unit_to_add == null)
                    return;
                if (unit.Active)
                    TacticalController.Instance.Overlay.ShowTile(unit_to_add.Position.position, Color.yellow, 1, 1f);

                Object.Destroy(unit.GetComponent<PolygonCollider2D>());
                unit.Active = false;
                unit_to_add.button.Background.color = Color.yellow;
                state.unit_in_hand = unit_to_add;
                unit_to_add.button.BottomText.text = "RESERVE";
                unit_to_add.renderer.sortingOrder = 10;
                TacticalController.Instance.StateMachine.State = TacticalState.DeployPlaceUnit;
            }
            else if (button == PointerEventData.InputButton.Right)
            {
                if (unit.Active)
                {
                    var unit_to_remove = state.units.Find(u => u.info == unit);
                    if (unit_to_remove == null)
                        return;

                    TacticalController.Instance.Overlay.ShowTile(unit_to_remove.Position.position, Color.yellow, 1, 1f);

                    unit_to_remove.info.gameObject.SetActive(false);
                    unit_to_remove.Position.position = new Vector2Int(-1, -1);
                    unit_to_remove.info.Active = false;
                    Object.Destroy(unit.GetComponent<PolygonCollider2D>());
                    unit_to_remove.button.Background.color = Color.white;
                    unit_to_remove.button.BottomText.text = "RESERVE";
                }
            }
        }

    }

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
            state.unit_in_hand.renderer.color =
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
            if (button != PointerEventData.InputButton.Left)
                return;
            if (state.units.Find(u => u.Position.position == coord) != null)
                return;


            state.unit_in_hand.info.gameObject.AddComponent<PolygonCollider2D>();
            state.unit_in_hand.info.Active = true;
            state.unit_in_hand.button.Background.color = Color.green;
            state.unit_in_hand.button.BottomText.text = "DEPLOY";
            state.unit_in_hand.renderer.color = Color.white;
            state.unit_in_hand.renderer.sortingOrder = 0;
            state.unit_in_hand.Position.position = coord;
            state.unit_in_hand = null;

            TacticalController.Instance.Overlay.HideTile(coord);
            TacticalController.Instance.StateMachine.State = TacticalState.DeploySelectUnit;
        }


        public override void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                state.unit_in_hand.info.gameObject.SetActive(false);
                state.unit_in_hand.button.Background.color = Color.white;
                state.unit_in_hand = null;

                TacticalController.Instance.StateMachine.State = TacticalState.DeploySelectUnit;
            }

        }
    }
}
