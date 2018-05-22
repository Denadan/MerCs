using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(Map))]
    [RequireComponent(typeof(HexGrid))]
    public class PlaceUnit : MonoBehaviour
    {
        public Sprite[] MechSprites;
        public GameObject MechPrefab;

        private Map map;
        private HexGrid grid;


        private void Start()
        {
            map = GetComponent<Map>();
            grid = GetComponent<HexGrid>();
        }

        public void Place()
        {
            if (TacticalController.Instance.Units.Count > 10)
                return;
            Vector2Int coord;
            do
            {
                coord = Tools.RND.Vector2IntRND(map.SizeX, map.SizeY);
            } while (TacticalController.Instance.Units.Find(i => i.Position.position == coord) != null);
            var facing = CONST.VerticalDir.Rnd();
            var unit = Instantiate(MechPrefab, grid.CellToWorld(new Vector3Int(coord.x, coord.y, 0)),
                Quaternion.Euler(0,0,CONST.GetAngleV(facing)), TacticalController.Instance.Grid.UnitsParent);
            var pos = unit.GetComponent<CellPosition>();
            unit.GetComponent<SpriteRenderer>().sprite = MechSprites.Rnd();
            unit.AddComponent<PolygonCollider2D>();
            pos.Facing = facing;
            pos.position = coord;
            TacticalController.Instance.Units.Add(unit.GetComponent<UnitInfo>());

        }

        public void Clear()
        {
            foreach (var item in TacticalController.Instance.Units)
                Destroy(item.gameObject);
            TacticalController.Instance.Units.Clear();
        }
    }
}
