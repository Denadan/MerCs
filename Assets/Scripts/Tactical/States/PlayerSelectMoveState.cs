﻿using System.Linq;
using System.Collections;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class PlayerSelectMoveState : TacticalStateHandler
    {
        public override TacticalState State => TacticalState.SelectMovement;
        private PhasePrepareState state;

        public PlayerSelectMoveState(PhasePrepareState state)
        {
            this.state = state;
        }

        public override void OnLoad()
        {
            TacticalController.Instance.Overlay.HideAll();
            TacticalController.Instance.StateMachine.StartCoroutine(wait_for_path());
            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Move);
        }

        private IEnumerator wait_for_path()
        {
            float start = TacticalController.Instance.Path.DEBUG_TimeStart;

            UnityEngine.Debug.Log($"Started");
            float end = 0;
            while (!TacticalController.Instance.Path.Ready)
            {
                if (TacticalController.Instance.StateMachine.State != State)
                {
                    end = Time.realtimeSinceStartup;
                    UnityEngine.Debug.Log($"State changed\nStart at {start:0.000}s\nEnd at {end:0.000}s\nTotal: {end - start:0.000}s");

                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
            end = Time.realtimeSinceStartup;
            UnityEngine.Debug.Log($"Task Completed\nStart at {start:0.000}s\nEnd at {end:0.000}s\nTotal: {end - start:0.000}s");
        }


    }
}