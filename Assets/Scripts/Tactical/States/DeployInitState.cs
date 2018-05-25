using System.Collections.Generic;
using System.Linq;
using Mercs.Tactical.UI;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class DeployInitState : TacticalStateHandler
    {
        public class unit
        {
            public UnitInfo info;
            public SpriteRenderer renderer;
            public UnitSelectButton button;
            public CellPosition Position;
        }

        public List<unit> units;
        public unit unit_in_hand = null;
        public RectInt DeployZone;

        public override TacticalState State => TacticalState.DeployInit;

        public override void OnLoad()
        {
            base.OnLoad();
            TacticalUIController.Instance.ClearUnitList();
            TacticalUIController.Instance.ShowUnitList();
            units = (from unit_info in TacticalController.Instance.Units
                select new unit
                {
                    info = unit_info,
                    renderer = unit_info.GetComponent<SpriteRenderer>(),
                    button = TacticalUIController.Instance.AddUnit(unit_info),
                    Position = unit_info.GetComponent<CellPosition>()
                }).ToList();
            foreach (var unit in units)
                unit.button.BottomText.text = "RESERVE";

            DeployZone.width = TacticalController.Instance.Map.SizeX / 2;
            DeployZone.x = DeployZone.width / 2;
            DeployZone.y = 0;
            DeployZone.height = 2;
            var center =
                TacticalController.Instance.Grid.CellToWorld(new Vector3Int((int)DeployZone.center.x,
                    (int)DeployZone.center.y, 0));

            Camera.main.transform.position = new Vector3(center.x, center.y, Camera.main.transform.position.z);

            for (int i = DeployZone.xMin; i < DeployZone.xMax; i++)
            for (int j = DeployZone.yMin; j < DeployZone.yMax; j++)
            {
                TacticalController.Instance.Overlay.ShowTile(new Vector2Int(i, j), Color.yellow, 1, 1f);
            }

            TacticalController.Instance.StateMachine.State = TacticalState.DeploySelectUnit;
        }
    }
}