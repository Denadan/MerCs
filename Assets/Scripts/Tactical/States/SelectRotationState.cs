using System;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public abstract class SelectRotationState : TacticalStateHandler
    {
        private CellPosition original;
        private Vector3 origin;
        private LineRenderer line;

        public override void Update()
        {
            var dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var old_facing = original.Facing;
            original.SetFacing(CONST.GetRotation(origin, dest));
            if (old_facing != original.Facing)
                ShowFacing();
            dest.z = origin.z;

            if (Input.GetMouseButtonDown(0))
            {
                Done();
            }
        }

        protected virtual void ShowFacing()
        {
            TacticalController.Instance.Overlay.HideAll();

            Dir Main = original.Facing;
            Dir Left = CONST.TurnLeft(Main);
            Dir Right = CONST.TurnRight(Main);

            var coord_main = original.position;
            Dir left_back = CONST.Inverse(Right);
            Dir right_back = CONST.Inverse(Left);

            TacticalController.Instance.Overlay.ShowTile(coord_main, Color.green, MapOverlay.Sector2(Main));

            for (int n = 1;n<=25;n++)
            { 
                coord_main += CONST.GetDirShift(coord_main, Main);
                var c_left = coord_main;
                var c_right = coord_main;

                (var tex, var color) = TileInfo(coord_main, true);
                TacticalController.Instance.Overlay.ShowTile(coord_main, color, tex);

                for (int i = 0; i < n; i++)
                {
                    c_left += CONST.GetDirShift(c_left, left_back);
                    c_right += CONST.GetDirShift(c_right, right_back);

                    if (TacticalController.Instance.Map.OnMap(c_left))
                    {
                        (tex, color) = TileInfo(c_left);
                        TacticalController.Instance.Overlay.ShowTile(c_left, color, tex);
                    }

                    if (TacticalController.Instance.Map.OnMap(c_right))
                    {
                        (tex, color) = TileInfo(c_right);
                        TacticalController.Instance.Overlay.ShowTile(c_right, color, tex);
                    }
                }
            }
        }

        protected virtual (MapOverlay.Texture tex, Color color) TileInfo(Vector2Int coord, bool main = false)
        {
            Color c = Color.white;

            c.a = Mathf.Clamp(1 - TacticalController.Instance.Map.Distance(original.position, coord) / 10, 0, 1);

            //c.a = TacticalController.Instance.Map.Distance(original.position, coord) < 10 ? 0.5f : 0;

            if (main)
                return (MapOverlay.Texture.White50, c);
            else
                return (MapOverlay.Texture.White25, c);
        }

        public override void OnLoad()
        {
            original = GetOrigin();
            origin = TacticalController.Instance.Grid.CellToWorld(original.position);
            //line = TacticalUIController.Instance.RotationLine;
            //line.gameObject.SetActive(true);
            //line.SetPosition(0, origin);
            //line.SetPosition(1, origin);
            ShowFacing();
        }
        public override void OnUnload()
        {
            //line.gameObject.SetActive(false);
        }

        public abstract CellPosition GetOrigin();
        public abstract void Done();
    }
}