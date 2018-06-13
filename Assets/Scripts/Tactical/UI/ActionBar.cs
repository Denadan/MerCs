using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class ActionBar : MonoBehaviour
    {
        public void B_Move()
        {
            if (TacticalController.Instance.StateMachine.State != States.TacticalState.SelectMovement)
                TacticalController.Instance.StateMachine.State = States.TacticalState.SelectMovement;
        }

        public void B_Run()
        {
            if (TacticalController.Instance.StateMachine.State != States.TacticalState.SelectRun)
                TacticalController.Instance.StateMachine.State = States.TacticalState.SelectRun;
        }

        public void B_Evasive()
        {
            if (TacticalController.Instance.StateMachine.State != States.TacticalState.SelectEvasive)
                TacticalController.Instance.StateMachine.State = States.TacticalState.SelectEvasive;
        }

        public void B_Fire()
        {

        }

        public void B_Jump()
        {
            if (TacticalController.Instance.StateMachine.State != States.TacticalState.SelectJump)
                TacticalController.Instance.StateMachine.State = States.TacticalState.SelectJump;
        }

        public void B_Guard()
        {
            if (TacticalController.Instance.StateMachine.State != States.TacticalState.ConfirmGuard)
                TacticalController.Instance.StateMachine.State = States.TacticalState.ConfirmGuard;
        }

        public void B_Cancel()
        {
            TacticalUIController.Instance.HideActionBar();
            TacticalController.Instance[TacticalController.Instance.SelectedUnit].Background.color = Color.white;
            TacticalController.Instance.SelectedUnit = null;
            TacticalController.Instance.Path.MakePathMap(null);

            TacticalController.Instance.StateMachine.State = States.TacticalState.SelectUnit;
        }
    }
}
