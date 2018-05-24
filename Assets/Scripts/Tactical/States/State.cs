using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{

    public enum TacticalState
    {
        NotReady = 0,
        DeployInit = 10,
        DeploySelectUnit = 11,
        DeployPlaceUnit = 12,
        SelectUnit = 20,
        SelectMovement = 30,
        SelectRun = 31,
        SelectJump = 32,
        SelectRotation = 33
    }

    public abstract class TacticalStateHandler : IState<TacticalState>
    {
        public abstract TacticalState State { get; }

        public virtual void OnLoad()
        {
        }

        public virtual void OnUnload()
        {
        }

        public virtual void TileEnter(Vector2Int coord)
        {
        }

        public virtual void TileLeave(Vector2Int coord)
        {
        }

        public virtual void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
        }

        public virtual void UnitEnter(UnitInfo unit)
        {
        }

        public virtual void UnitLeave(UnitInfo unit)
        {
        }

        public virtual void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
        }

        public virtual void Update()
        { }
    }
}