namespace Mercs.Tactical.States
{
    public class AIEndTurnState : TacticalStateHandler
    {
        private PhasePrepareState state;

        public override TacticalState State => TacticalState.AIEndTurn;
    }


}
