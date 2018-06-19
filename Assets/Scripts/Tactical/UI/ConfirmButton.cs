using Mercs.Tactical.States;
using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class ConfirmButton : MonoBehaviour
    {
        public void OnClick()
        {
            switch (TacticalUIController.Instance.HighlightedButton)
            {
                case ActionButton.Guard:
                    TacticalController.Instance.StateMachine.State = TacticalState.PlayerEndTurn;
                    break;
            }
        }
    }
}