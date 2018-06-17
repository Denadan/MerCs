using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Mercs.Tactical.States
{
    public class PlayerSelectRunState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectRun;


        public PlayerSelectRunState(PhasePrepareState state, MovementStateData data) : base(state,data)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            data.Type = MovementStateData.MoveType.Run;

            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Run);
        }

        protected override void ShowOverlay()
        {
            TacticalController.Instance.Overlay.ShowMoveMapRun();
        }

        protected override (PathMap.path_target target, List<PathMap.path_node> list) GetPath(Vector2Int coord)
        {
            if (!TacticalController.Instance.Path.Ready || TacticalController.Instance.Path.RunList == null)
                return (null, null);
            var start = TacticalController.Instance.Path.RunList.Find(item => item.coord == coord);
            if (start?.fast_path == null)
                return (null, null);
            var result = new List<PathMap.path_node>();

            var current = start.fast_path;
            do
            {
                result.Add(current);
                current = current.prev;
            } while (current != null);
            return (start, result);
        }

        protected override PathMap.path_target CanMove(Vector2Int coord)
        {
            return TacticalController.Instance.Path.RunList?.Find(i => i.coord == coord);
        }
    }
}
