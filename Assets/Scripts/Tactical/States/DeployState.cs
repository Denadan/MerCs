using System.Linq;
using Mercs.Tactical.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical.States
{
    public class DeployState : TacticalStateHandler
    {
        private class unit
        {
            public UnitInfo info;
            public SpriteRenderer renderer;
            public UnitSelectButton button;
        }

        private List<unit> units;
        private unit unit_in_hand = null;
        public RectInt DeployZone;

        public override TacticalState State => TacticalState.Deploy;

        public override void TileClick(Vector2Int coord, PointerEventData.InputButton button)
        {
        }

        public override void TileEnter(Vector2Int coord)
        {
        }

        public override void TileLeave(Vector2Int coord)
        {
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
        }

        public override void UnitEnter(UnitInfo unit)
        {
        }

        public override void UnitLeave(UnitInfo unit)
        {
        }

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
                         button = TacticalUIController.Instance.AddUnit(unit_info)
                     }).ToList();
            foreach (var unit in units)
                unit.button.BottomText.text = "RESERVE";

            DeployZone.width = TacticalController.Instance.Map.SizeX;
            DeployZone.x = DeployZone.width / 2;
            DeployZone.y = 0;
            DeployZone.height = 2;

            for(int i =DeployZone.xMin;i<=DeployZone.xMax;i++)
                for(int j = DeployZone.yMin; j <= DeployZone.yMax; j++ )
                {
                    TacticalController.Instance.Overlay.ShowTile(new Vector2Int(i, j), Color.yellow, 1);
                }
        }
    }
}
