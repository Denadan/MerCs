

namespace Mercs.Tactical.States
{
    public class PlayerEndTurnState : TacticalStateHandler
    {
        private PhasePrepareState state;

        public override TacticalState State => TacticalState.PlayerEndTurn;

        public PlayerEndTurnState(PhasePrepareState state)
        {
            this.state = state;
        }


        public override void OnLoad()
        {
            var unit = TacticalController.Instance.SelectedUnit;

            state.ActiveUnits.Remove(unit);
            unit.Selectable = false;
            unit.Active = false;

            TacticalController.Instance.SelectedUnit = null;
            TacticalUIController.Instance.ShowActionBar();

            if (state.ActiveUnits.Count > 0)
                SwitchTo(TacticalState.PhaseSelectFaction);
            else
                SwitchTo(TacticalState.PhasePrepare);

        }
    }
}
