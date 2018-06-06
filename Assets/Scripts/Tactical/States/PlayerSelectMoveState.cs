using System.Linq;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Mercs.Tactical.States
{
    public class PlayerSelectMoveState : PlayerSelectMovementBase
    {
        public override TacticalState State => TacticalState.SelectMovement;

        public PlayerSelectMoveState(PhasePrepareState state) : base(state)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();
            TacticalUIController.Instance.HighlightActionBarButton(ActionButton.Move);
        }

        protected override void ShowOverlay()
        {
            TacticalController.Instance.Overlay.ShowMoveMapMove();
        }

        protected override List<PathMap.path_node> GetPath(Vector2Int coord)
        {
            if (!TacticalController.Instance.Path.Ready || TacticalController.Instance.Path.MoveList == null)
                return null;
            var start = TacticalController.Instance.Path.MoveList.Find(item => item.coord == coord)?.fast_path;
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
    }
}
