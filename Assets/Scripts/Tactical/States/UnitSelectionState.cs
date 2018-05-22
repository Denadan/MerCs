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
            throw new System.NotImplementedException();
        }

        public override void UnitLeave(UnitInfo unit)
        {
            throw new System.NotImplementedException();
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            throw new System.NotImplementedException();
        }

        public override TacticalState State { get; }
    }
}