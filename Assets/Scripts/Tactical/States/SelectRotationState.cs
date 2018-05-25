using UnityEngine;

namespace Mercs.Tactical.States
{
    public abstract class SelectRotationState : TacticalStateHandler
    {
        private CellPosition original;
        private Vector3 origin;

        public override void Update()
        {
            var dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            original.SetFacing(CONST.GetRotation(origin, dest));
            TacticalUIController.Instance.DebugMenu.Rotation.text += $"  {original.Facing}";
            if (Input.GetMouseButtonDown(0))
            {
                Done();
            }
        }

        public override void OnLoad()
        {
            original = GetOrigin();
            origin = TacticalController.Instance.Grid.CellToWorld(original.position);
        }

        public abstract CellPosition GetOrigin();
        public abstract void Done();
    }
}