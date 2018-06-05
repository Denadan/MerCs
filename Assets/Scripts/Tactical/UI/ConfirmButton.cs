using Mercs.Tactical.States;
using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class ConfirmButton : MonoBehaviour
    {
        public void OnClick()
        {
            switch (TacticalController.Instance.StateMachine.State)
            {
                case TacticalState.ConfirmGuard:
                    TacticalController.Instance.StateMachine.State = TacticalState.PlayerEndTurn;
                    break;
            }

        }
        
    }
}