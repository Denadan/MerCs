using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{

    public enum TacticalState
    {
        SelectUnit,
        SelectMovement,
        SelectRun,
        SelectJump
    }

    public abstract class TacticalStateHandler : IState<TacticalState>
    {

        public abstract void TileEnter(Vector2Int coord);
        public abstract void TileLeave(Vector2Int coord);
        public abstract void TileClick(Vector2Int coord, PointerEventData.InputButton button);

        public abstract void UnitEnter(UnitInfo unit);
        public abstract void UnitLeave(UnitInfo unit);
        public abstract void UnitClick(UnitInfo unit, PointerEventData.InputButton button);
        public abstract TacticalState State { get; }
        public virtual void OnLoad()
        {
        }

        public virtual void OnUnload()
        {
        }
    }
}