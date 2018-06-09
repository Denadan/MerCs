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
        public override CellPosition GetOrigin()
        {
            return state.unit_in_hand.Position;
        }

        /// <summary>
        /// устанавливаем
        /// </summary>
        public override void Done()
        {
            state.unit_in_hand = null;
            SwitchTo(TacticalState.DeploySelectUnit);
            TacticalUIController.Instance.HideSelectedUnitWindow();

        }

        //отменяем
        protected override void Cancel()
        {
            GameObject.Destroy(state.unit_in_hand.info.gameObject.GetComponent<PolygonCollider2D>());
            state.unit_in_hand.info.Reserve = true;
            state.unit_in_hand.button.Background.color = Color.yellow;
            state.unit_in_hand.button.BottomText.text = "RESERVE";
            state.unit_in_hand.renderer.sortingOrder = 1;
            state.unit_in_hand.info.gameObject.SetActive(false);
            SwitchTo(TacticalState.DeployPlaceUnit);
        }
    }
}