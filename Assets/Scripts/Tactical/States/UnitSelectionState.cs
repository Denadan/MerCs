using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    public class UnitSelectionState : TacticalStateHandler
    {
        public override void UnitEnter(UnitInfo unit)
        {
            if(unit.Selectable)
                TacticalController.Instance.HighlightUnit(unit);
        }

        public override void UnitLeave(UnitInfo unit)
        {
            TacticalController.Instance.HideHighlatedUnit(unit);
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            if (unit.Selectable && button == PointerEventData.InputButton.Left)
            {
                TacticalController.Instance.SelectedUnit = unit;
                TacticalUIController.Instance.ShowActionBar();
                TacticalUIController.Instance.ShowActionBarButtons(unit);
                TacticalUIController.Instance.ShowActionBarButton(ActionButton.Cancel);
                TacticalController.Instance[unit].Background.color = Color.green;
                TacticalController.Instance.Path.MakePathMap(unit);

                SwitchTo(TacticalState.SelectMovement);
            }

        }

        public override TacticalState State => TacticalState.SelectUnit;
    }
}