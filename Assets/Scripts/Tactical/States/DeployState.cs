
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    public class DeployState : TacticalStateHandler
    {
        public override TacticalState State => TacticalState.Deploy;

        public override void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
        }

        public override void TileEnter(Vector2Int coord)
        {
        }

        public override void TileLeave(Vector2Int coord)
        {
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
        }

        public override void UnitEnter(UnitInfo unit)
        {
        }

        public override void UnitLeave(UnitInfo unit)
        {
        }
    }
}
