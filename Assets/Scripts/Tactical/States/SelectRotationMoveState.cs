using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs.Tactical.States
{

    public class SelectRotationMoveState : SelectRotationState
    {

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

        protected override (Vector2Int, Dir) GetOrigin()
        {
            return (data.target.coord, data.target.fast_path.facing);
        }

        protected override void Done(Dir new_facing)
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

        protected override bool Cancelable => true;
        protected override void SetFacing(Dir new_facing)
        {
            data.dir = new_facing;
             base.SetFacing(new_facing);
       }
    }
}