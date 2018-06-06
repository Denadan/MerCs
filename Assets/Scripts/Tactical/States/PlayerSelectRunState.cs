using System.Collections;
using System.Linq;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class PlayerSelectRunState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectRun;
        private PhasePrepareState state;


        public PlayerSelectRunState(PhasePrepareState state) : base(state)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Run);
        }

        protected override void ShowOverlay()
        {
        }

    }
}
