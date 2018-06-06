using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class PlayerSelectJumpState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectJump;

        public PlayerSelectJumpState(PhasePrepareState state) : base(state)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Jump);
        }

        protected override void ShowOverlay()
        {
            TacticalController.Instance.Overlay.ShowMoveMapJump();
        }

        protected override List<PathMap.path_node> GetPath(Vector2Int coord)
        {
            return null;
        }
    }
}
