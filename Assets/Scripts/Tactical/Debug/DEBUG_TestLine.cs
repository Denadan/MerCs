using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class DEBUG_TestLine : TacticalStateHandler
    {
        public override TacticalState State => TacticalState.DEBUG_TEST_LINE;

        Vector2Int origin;
        LineRenderer line;

        public override void OnLoad()
        {
            origin = new Vector2Int(TacticalController.Instance.Map.SizeX / 2, TacticalController.Instance.Map.SizeY / 2);
            line = TacticalUIController.Instance.MoveLine;
            line.positionCount = 2;
            line.SetPosition(0, TacticalController.Instance.Grid.CellToWorld(origin));
        }

        public override void TileEnter(Vector2Int coord)
        {
            line.SetPosition(1, TacticalController.Instance.Grid.CellToWorld(coord));
            line.gameObject.SetActive(true);
            var list = TacticalController.Instance.Grid.Trace(origin, coord);
            if (list != null)
                foreach (var item in list)
                    if (TacticalController.Instance.Map.OnMap(item.point))
                        TacticalController.Instance.Overlay.ShowTile(item.point, Color.red, MapOverlay.Texture.White50);
        }

        public override void TileLeave(Vector2Int coord)
        {
            base.TileLeave(coord);
            line.gameObject.SetActive(false);
            TacticalController.Instance.Overlay.HideAll();
        }
    }
}
