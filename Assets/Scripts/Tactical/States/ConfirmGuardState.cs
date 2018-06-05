using UnityEngine;

namespace Mercs.Tactical.States
{
    public class ConfirmGuardState : TacticalStateHandler
    {
        public override TacticalState State => TacticalState.ConfirmGuard;

        public override void OnLoad()
        {
            TacticalController.Instance.Overlay.HideAll();
            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Move);
        }
    }
}
