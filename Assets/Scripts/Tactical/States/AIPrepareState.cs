using System.Linq;
using System.Collections;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class AIPrepareState : TacticalStateHandler
    {
        private PhasePrepareState state;


        public override TacticalState State => TacticalState.AIPrepare;

        public AIPrepareState(PhasePrepareState state)
        {
            this.state = state;
        }

        public override void OnLoad()
        {
            TacticalController.Instance.SelectedUnit = state.ActiveUnits.Find(
                unit => unit.Faction == TacticalController.Instance.CurrentFaction);
            TacticalController.Instance.StateMachine.StartCoroutine(Switch());
            TacticalUIController.Instance.MoveCameraTo(TacticalController.Instance.SelectedUnit);

            foreach(var button in TacticalController.Instance.PlayerUnits.Select(unit => TacticalController.Instance[unit]).Where(item => item != null))
            {
                button.Background.color = Color.grey;
            }
        }

     
        public IEnumerator Switch()
        {
            yield return new WaitForSeconds(1f);
            SwitchTo(TacticalState.AIEndTurn);
        }
    }
}
