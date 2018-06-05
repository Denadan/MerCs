using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    public class UnitSelectionState : TacticalStateHandler
    {
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

        }

        public override TacticalState State => TacticalState.SelectUnit;
    }
}