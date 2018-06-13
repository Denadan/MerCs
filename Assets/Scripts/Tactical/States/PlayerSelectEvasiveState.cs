using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Mercs.Tactical.States
{
    public class PlayerSelectEvasiveState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectEvasive;


        public PlayerSelectEvasiveState(PhasePrepareState state, MovementStateData data) : base(state,data)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            data.Type = MovementStateData.MoveType.Run;

            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Evade);
        }

        protected override void ShowOverlay()
        {
            TacticalController.Instance.Overlay.ShowMoveMapEvade();
        }

        protected override List<PathMap.path_node> GetPath(Vector2Int coord)
        {
            if (!TacticalController.Instance.Path.Ready || TacticalController.Instance.Path.EvasiveList == null)
                return null;
            var start = TacticalController.Instance.Path.EvasiveList.Find(item => item.coord == coord)?.fast_path;
            if (start == null)
                return null;
            var result = new List<PathMap.path_node>();
            do
            {
                result.Add(start);
                start = start.prev;
            } while (start != null);
            return result;
        }

        protected override PathMap.path_target CanMove(Vector2Int coord)
        {
            return TacticalController.Instance.Path.EvasiveList?.Find(i => i.coord == coord);
        }
    }
}
