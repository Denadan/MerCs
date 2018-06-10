using UnityEngine;

namespace Mercs.Tactical.States
{
    /// <summary>
    /// выбор поворота при деплое
    /// </summary>
    public class DeployRotationState : SelectRotationState
    {
        public override TacticalState State => TacticalState.DeployRotation;
        private DeployInitState state;


        public DeployRotationState(DeployInitState deploy)
        {
            this.state = deploy;
        }

        /// <summary>
        /// можно вращаться в любом направлении
        /// </summary>
        /// <param name="newFacing"></param>
        /// <returns></returns>
        protected override bool Allowed(Dir newFacing)
        {
            return true;
        }

        /// <summary>
        /// начальная точка - позиция юнита
        /// </summary>
        /// <returns></returns>
        protected override (Vector2Int, Dir) GetOrigin()
        {
            return (state.unit_in_hand.Position.position, state.unit_in_hand.Position.Facing);
        }

        protected override void Done(Dir new_dir)
        {
            state.unit_in_hand = null;
            SwitchTo(TacticalState.DeploySelectUnit);
            TacticalUIController.Instance.HideSelectedUnitWindow();
        }

        protected override void SetFacing(Dir new_facing)
        {
            base.SetFacing(new_facing);
            state.unit_in_hand.Position.SetFacing(new_facing);
        }


        protected override void Cancel()
        {
        }

        protected override bool Cancelable => false;
    }
}