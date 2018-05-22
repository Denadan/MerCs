using Mercs.Tactical.Events;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    [RequireComponent(typeof(TileSubscriber))]
    [RequireComponent(typeof(UnitSubscriber))]
    public class TacticalStateMachine : StateMachine<TacticalState, TacticalStateHandler>, ITileEventReceiver, IUnitEventReceiver
    {
        public void MouseTileEnter(Vector2Int coord)
        {
            curStateHandler.TileEnter(coord);
        }

        public void MouseTileLeave(Vector2Int coord)
        {
            curStateHandler.TileLeave(coord);
        }

        public void MouseTileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
            curStateHandler.TileClick(coord, button);
        }

        public void MouseUnitEnter(UnitInfo unit)
        {
            curStateHandler.UnitEnter(unit);
        }

        public void MouseUnitLeave(UnitInfo unit)
        {
            curStateHandler.UnitLeave(unit);
        }

        public void MouseUnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            curStateHandler.UnitClick(unit, button);
        }
    }
}