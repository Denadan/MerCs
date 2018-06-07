using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs.Tactical.States
{

    public class SelectRotationMoveState : SelectRotationState
    {
        private Vector3 old_position_world;
        private Vector2Int old_position_cell;
        private Dir old_facing;
        private MovementStateData data;

        public override TacticalState State => TacticalState.SelectRotation;

        public SelectRotationMoveState(MovementStateData data)
        {
            this.data = data;
        }

        protected override bool Allowed(Dir newFacing)
        {
            return data.target.AllowedDir.Contains(newFacing);
        }

        public override CellPosition GetOrigin()
        {
            var pos = TacticalController.Instance.SelectedUnit.Position;

            old_facing = pos.Facing;
            old_position_cell = pos.position;
            old_position_world = pos.gameObject.transform.position;

            pos.position = data.target.coord;
            pos.transform.position = TacticalController.Instance.Grid.CellToWorld(data.target.coord);
            pos.SetFacing(data.target.fast_path.facing);

            return pos;
        }

        public override void OnUnload()
        {
            base.OnUnload();
            var pos = TacticalController.Instance.SelectedUnit.Position;
            pos.position = old_position_cell;
            pos.transform.position = old_position_world;
            pos.SetFacing(old_facing);

        }

        public override void Done()
        {
            TacticalUIController.Instance.MoveLine.gameObject.SetActive(false);
        }

        private List<PathMap.path_node> get_path()
        {
            var list = new List<PathMap.path_node>();
            var node = data.target.other_path[data.dir];
            while (node != null)
            {
                list.Add(node);
                node = node.prev;
            }

            return list;
        }

        protected override void ShowFacing()
        {
            base.ShowFacing();
            data.dir = TacticalController.Instance.SelectedUnit.Position.Facing;
            data.path = get_path();
            var line = TacticalUIController.Instance.MoveLine;
            line.startColor = Color.red;
            line.endColor = Color.green;
            var points = (from v2 in data.path
                          select TacticalController.Instance.Grid.CellToWorld(v2.coord)).ToArray();
            line.positionCount = points.Length;
            line.SetPositions(points);
            line.gameObject.SetActive(true);
        }

        protected override void Cancel()
        {
            SwitchTo(TacticalController.Instance.StateMachine.LastState);

            TacticalUIController.Instance.MoveLine.gameObject.SetActive(false);
        }
    }
}