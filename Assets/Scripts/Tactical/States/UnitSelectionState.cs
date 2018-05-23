using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    public class UnitSelectionState : TacticalStateHandler
    {
        public override void TileEnter(Vector2Int coord)
        {
        }

        public override void TileLeave(Vector2Int coord)
        {
        }

        public override void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
        }

        public override void UnitEnter(UnitInfo unit)
        {
            TacticalController.Instance.HighlightUnit(unit);
        }

        public override void UnitLeave(UnitInfo unit)
        {
            TacticalController.Instance.HideHighlatedUnit(unit);
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            if(button == PointerEventData.InputButton.Left && TacticalController.Instance.SelectUnit(unit))
            {
                //if (unit.Movement == null)
                //    TacticalController.Instance.StateMachine.State = TacticalState.SelectRotation;
                //else
                //    TacticalController.Instance.StateMachine.State = TacticalState.SelectMovement;
            }
        }

        public override TacticalState State => TacticalState.SelectUnit;
    }
}