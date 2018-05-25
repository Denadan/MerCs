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
//            TacticalUIController.Instance.DebugMenu.Rotation.text += $"  {original.Facing}";
            dest.z = origin.z;

 //           line.SetPosition(1, dest);
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

            float alpha = 1;
            var coord_main = original.position;
            var coord_left = original.position;
            var coord_right = original.position;
            Dir left_back = CONST.Inverse(Right);
            Dir right_back = CONST.Inverse(Left);
            int n = 1;
            while (n < 15)
            {
                coord_main += CONST.GetDirShift(coord_main, Main);
                var c_left = coord_main;
                var c_right = coord_main;

                (var tex, var color) = TileInfo(coord_main, true);
                color.a *= alpha;
                TacticalController.Instance.Overlay.ShowTile(coord_main, color, tex);

                for (int i = 0; i < n; i++)
                {
                    c_left += CONST.GetDirShift(c_left, left_back);
                    c_right += CONST.GetDirShift(c_right, right_back);


                    (tex, color) = TileInfo(c_left);
                    color.a *= alpha;
                    TacticalController.Instance.Overlay.ShowTile(c_left, color, tex);
                    (tex, color) = TileInfo(c_right);
                    color.a *= alpha;
                    TacticalController.Instance.Overlay.ShowTile(c_right, color, tex);
                }

                alpha -= .05f;
                n += 1;
            }
        }

        protected virtual (MapOverlay.Texture tex, Color color) TileInfo(Vector2Int coord, bool main = false)
        {
            if (main)
                return (MapOverlay.Texture.White50, Color.white);
            else
                return (MapOverlay.Texture.White25, Color.white);
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