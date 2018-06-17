using System.Linq;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Mercs.Tactical.States
{
    public class PlayerSelectMoveState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectMovement;

        public PlayerSelectMoveState(PhasePrepareState state, MovementStateData data) : base(state, data)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            data.Type = MovementStateData.MoveType.Move;
            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Move);
        }

        protected override void ShowOverlay()
        {
            TacticalController.Instance.Overlay.ShowMoveMapMove();
        }

        protected override (PathMap.path_target target, List<PathMap.path_node> list) GetPath(Vector2Int coord)
        {
            if (!TacticalController.Instance.Path.Ready || TacticalController.Instance.Path.MoveList == null)
                return (null, null);
            var start = TacticalController.Instance.Path.MoveList.Find(item => item.coord == coord);
            if (start?.fast_path == null)
                return (null, null);
            var result = new List<PathMap.path_node>();

            var current = start.fast_path;
            do
            {
                result.Add(current);
                current = current.prev;
            } while (current != null);
            return (start, result);
        }


        protected override PathMap.path_target CanMove(Vector2Int coord)
        {
            return TacticalController.Instance.Path.MoveList?.Find(i => i.coord == coord);
        }
    }
}
