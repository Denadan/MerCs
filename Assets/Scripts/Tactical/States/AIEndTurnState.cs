namespace Mercs.Tactical.States
{
    public class AIEndTurnState : TacticalStateHandler
    {
        private PhasePrepareState state;

        public override TacticalState State => TacticalState.AIEndTurn;

        public AIEndTurnState(PhasePrepareState state)
        {
            this.state = state;
        }

        public override void OnLoad()
        {
            var unit = TacticalController.Instance.SelectedUnit;

            state.ActiveUnits.Remove(unit);
            unit.Active = false;

            TacticalController.Instance.SelectedUnit = null;

            SwitchTo(state.ActiveUnits.Count > 0 ? TacticalState.PhaseSelectFaction : TacticalState.PhasePrepare);
           
        }
    }


}
