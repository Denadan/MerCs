using Mercs.Tactical.Events;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    [RequireComponent(typeof(TileSubscriber))]
    [RequireComponent(typeof(UnitSubscriber))]
    public class TacticalStateMachine : StateMachine<TacticalState, TacticalStateHandler>, ITileEvent, IUnitEvent
    {
        public void MouseTileEnter(Vector2Int coord)
        {
            curStateHandler?.TileEnter(coord);
        }

        public void MouseTileLeave(Vector2Int coord)
        {
            curStateHandler?.TileLeave(coord);
        }

        public void MouseTileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
            curStateHandler?.TileClick(coord, button);
        }

        public void MouseUnitEnter(UnitInfo unit)
        {
            //UnityEngine.Debug.Log($"Enter {unit}");
            curStateHandler?.UnitEnter(unit);
        }

        public void MouseUnitLeave(UnitInfo unit)
        {
            //UnityEngine.Debug.Log($"Leave {unit}");
            curStateHandler?.UnitLeave(unit);
        }

        public void MouseUnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            //UnityEngine.Debug.Log($"CLick {unit}");

            curStateHandler?.UnitClick(unit, button);
        }

        private void Update()
        {
            curStateHandler?.Update();
        }

        protected override void Start()
        {

            addState(new UnitSelectionState());
            var deploy = new DeployInitState();
            addState(deploy);
            addState(new DeploySelectUnitState(deploy));
            addState(new DeployPlaceUnitState(deploy));
            addState(new DeployRotationState(deploy));

            addState(new TurnPrepareState());
            var prepare = new PhasePrepareState();
            addState(prepare);
            addState(new PhaseSelectFactionState(prepare));
            addState(new AIPrepareState(prepare));
            addState(new AIEndTurnState(prepare));
            addState(new PlayerPrepareState(prepare));
            var movement = new MovementStateData();

            addState(new PlayerSelectMoveState(prepare, movement));
            addState(new PlayerSelectRunState(prepare, movement));
            addState(new PlayerSelectEvasiveState(prepare, movement));
            addState(new PlayerSelectJumpState(prepare, movement));
            addState(new PlayerEndTurnState(prepare));
            addState(new SelectRotationMoveState(prepare, movement));

            addState(new WaitMovementComplete());
            addState(new WaitActionBar());

            addState(new DEBUG_TestLine());

            base.Start();
        }

        protected override bool Ready => TacticalController.Instance.Overlay != null && TacticalController.Instance.Overlay.Ready;
    }
}