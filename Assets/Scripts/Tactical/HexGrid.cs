using System;
using System.Collections.Generic;
using UnityEngine;


namespace Mercs.Tactical
{
    public abstract class HexGrid : MonoBehaviour
    {
        public abstract Vector3 CellToWorld(Vector3Int tile_coord);
        public abstract Vector2 CellToMap(Vector2Int tile_coord);
        private float sx, sy;
        


        protected abstract void MakeTiles();

        protected Map map;
        protected PathMap path;

        [SerializeField]
        protected Transform TilesParent;
        [SerializeField]
        protected Transform PathCostParent;
        [SerializeField]
        protected Transform LinksCostParent;
        public Transform UnitsParent;

               
        protected GameObject[,] tile_map;

        public GameObject this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= map.SizeX || y < 0 || y >= map.SizeY)
                    return null;
                return tile_map[x, y];
            }
        }

        public GameObject this[Vector2Int coord]
        {
            get
            {
                if (coord.x < 0 || coord.x >= map.SizeX || coord.y < 0 || coord.y >= map.SizeY)
                    return null;
                return tile_map[coord.x, coord.y];
            }
        }

        public float MapSX => sx;
        public float MapSy => sy;


        // Use this for initialization
        void Start()
        {

            map = GetComponent<Map>();
            path = GetComponent<PathMap>();
            sy = 1;
            sx = 1 * Mathf.Sin(Mathf.PI / 3);

            tile_map = new GameObject[map.SizeX, map.SizeY];

            map.Generate(this);

            path.CreatePathMap();
            MakeTiles();

            GetComponent<MapOverlay>().Init();
        }

        void Clear()
        {
            foreach (Transform child in TilesParent)
                Destroy(child.gameObject);
            foreach (Transform child in PathCostParent)
                Destroy(child.gameObject);
            foreach (Transform child in LinksCostParent)
                Destroy(child.gameObject);
        }

        public Vector3 CellToWorld(Vector2Int tile_coord)
        {
            return CellToWorld(new Vector3Int(tile_coord.x, tile_coord.y, 0));
        }
        public Vector3 CellToWorld(int x, int y)
        {
            return CellToWorld(new Vector3Int(x, y, 0));
        }
        public Vector2 CellToMap(int x, int y) => CellToMap(new Vector2Int(x, y));


        public float MapDistance(Vector2Int from, Vector2Int to)
        {
            var t_from = CellToMap(from);
            var t_to = CellToMap(to);

            return Vector2.Distance(t_from, t_to);
        }


        public List<(float t, Vector2Int point)> Trace(Vector2Int from, Vector2Int to)
        {
            var w_from = CellToMap(from);
            var w_to = CellToMap(to);

            var res = new List<(float, Vector2Int)>();
            res.Add((0, from));
            var dirs = DirHelper.GetLineDirs(from, to);

            if (dirs.main == dirs.second)
            {
                //UnityEngine.Debug.Log("eq");
                var dir = dirs.main;
                var current = from;
                while(current != to)
                {
                    current = current.ShiftTo(dir);
                    var w_cur = CellToMap(current);
                    res.Add((CONST.T(w_cur, w_from, w_to), current));
                    if (res.Count > 50)
                        return res;
                }
            }
            else
            {
                //UnityEngine.Debug.Log("neq");
                var current = from;
                while(current != to)
                {
                    var main = current.ShiftTo(dirs.main);
                    var second = current.ShiftTo(dirs.second);
                    var w_main = CellToMap(main);
                    var w_second = CellToMap(second);
                    var intersect = CONST.Inersect(w_from, w_to, w_main, w_second);

                    var t1 = CONST.T(intersect.point, w_main, w_second);
                    var t = (CONST.T(intersect.point, w_from, w_to));

                    //UnityEngine.Debug.Log($"{t:F2}  {current}  {from}-{to}/{main}-{second}  {w_from}-{w_to}/{w_main}-{w_second} = {intersect}  {t1:F2} ");

                    if (t1 < 0.5f)
                        current = main;
                    else
                        current = second;
                    res.Add((t, current));
                    if (res.Count > 50)
                        return res;
                }
            }

            return res;
        }

    }

}