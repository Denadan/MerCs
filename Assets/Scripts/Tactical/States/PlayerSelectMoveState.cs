using System.Linq;
using System.Collections;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class PlayerSelectMoveState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectMovement;

        public PlayerSelectMoveState(PhasePrepareState state) : base(state)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Move);
        }

        protected override void ShowOverlay()
        {
        }
    }
}
