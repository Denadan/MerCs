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
            state.ActiveUnits.Remove(TacticalController.Instance.SelectedUnit);
            TacticalController.Instance.SelectedUnit = null;
            if(state.ActiveUnits.Count > 0)
                SwitchTo(TacticalState.PhaseSelectFaction);
            else
                SwitchTo(TacticalState.PhasePrepare);

        }
    }


}
