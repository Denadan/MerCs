namespace Mercs.Tactical.States
{
    public class DeployRotationState : SelectRotationState
    {
        public override TacticalState State => TacticalState.DeployRotation;
        private DeployInitState state;


        public DeployRotationState(DeployInitState deploy)
        {
            this.state = deploy;
        }

        public override CellPosition GetOrigin()
        {
            return state.unit_in_hand.Position;
        }

        public override void Done()
        {
            state.unit_in_hand = null;
            TacticalController.Instance.StateMachine.State = TacticalState.DeploySelectUnit;
        }
    }
}