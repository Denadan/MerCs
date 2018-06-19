using UnityEngine;

namespace Mercs.Tactical.States
{
    public class ConfirmGuardState : TacticalStateHandler
    {
        public override TacticalState State => TacticalState.Confirm;

        public override void OnLoad()
        {
            TacticalUIController.Instance.ShowButton(TacticalButton.Confirm);
        }

        public override void OnUnload()
        {
            TacticalUIController.Instance.HideButton(TacticalButton.Confirm);
        }
    }
}
