using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class PlayerSelectJumpState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectJump;

        public PlayerSelectJumpState(PhasePrepareState state, MovementStateData data) : base(state, data)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            data.Type = MovementStateData.MoveType.Jump;

            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Jump);
        }

        protected override void ShowOverlay()
        {
            TacticalController.Instance.Overlay.ShowMoveMapJump();
        }

        protected override List<PathMap.path_node> GetPath(Vector2Int coord)
        {
            var node = TacticalController.Instance.Path.JumpList.Find(i => i.coord == coord);
            if (node == null)
                return null;
            return  new List<PathMap.path_node>
            {
                node.fast_path, node.fast_path.prev
            };
        }

        protected override PathMap.path_target CanMove(Vector2Int coord)
        {
            return TacticalController.Instance.Path.JumpList?.Find(i => i.coord == coord);
        }
    }
}
