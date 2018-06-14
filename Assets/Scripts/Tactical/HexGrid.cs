using System;
using System.Collections.Generic;
using UnityEngine;


namespace Mercs.Tactical
{
    /// <summary>
    /// базовый класс для преобразования координат по хексагональной сетке
    /// </summary>
    public abstract class HexGrid : MonoBehaviour
    {
        /// <summary>
        /// переводит кооринаты тайла в координаты сцены
        /// </summary>
        /// <param name="tile_coord"></param>
        /// <returns></returns>
        public abstract Vector3 CellToWorld(Vector3Int tile_coord);
        /// <summary>
        /// переводит координаты тайла в ортогональные координаты на карте 
        /// </summary>
        /// <param name="tile_coord"></param>
        /// <returns></returns>
        public abstract Vector2 CellToMap(Vector2Int tile_coord);

        /// <summary>
        /// заполняет карту тайлами
        /// </summary>
        protected abstract void MakeTiles();

        /// <summary>
        /// карта
        /// </summary>
        protected Map map;
        /// <summary>
        /// маршруты
        /// </summary>
        protected PathMap path;

        /// <summary>
        /// контейнер для тайлов
        /// </summary>
        [SerializeField] protected Transform TilesParent;
        /// <summary>
        /// контейнер для юнитов
        /// </summary>
        [SerializeField] public Transform UnitsParent;

        /// <summary>
        /// тайлы 
        /// </summary>
        protected GameObject[,] tile_map;

        /// <summary>
        /// получить тайл по кординатам
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GameObject this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= map.SizeX || y < 0 || y >= map.SizeY)
                    return null;
                return tile_map[x, y];
            }
        }

        /// <summary>
        /// получить тайл по координатам
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public GameObject this[Vector2Int coord]
        {
            get
            {
                if (coord.x < 0 || coord.x >= map.SizeX || coord.y < 0 || coord.y >= map.SizeY)
                    return null;
                return tile_map[coord.x, coord.y];
            }
        }

        /// <summary>
        /// сдвиг тайла по координате X
        /// </summary>
        public float MapSX { get; private set; }

        /// <summary>
        /// сдвиг тайла по координате y
        /// </summary>
        public float MapSy { get; private set; }


        // Use this for initialization
        void Start()
        {

            map = GetComponent<Map>();
            path = GetComponent<PathMap>();
            MapSy = 1;
            MapSX = 1 * Mathf.Sin(Mathf.PI / 3);

            tile_map = new GameObject[map.SizeX, map.SizeY];

            map.Generate(this);

            path.CreatePathMap();
            MakeTiles();

            GetComponent<MapOverlay>().Init();
        }

        /// <summary>
        /// очистить
        /// </summary>
        void Clear()
        {
            foreach (Transform child in TilesParent)
                Destroy(child.gameObject);
        }

        /// <summary>
        /// переводит кооринаты тайла в координаты сцены
        /// </summary>
        /// <param name="tile_coord"></param>
        /// <returns></returns>
        public Vector3 CellToWorld(Vector2Int tile_coord)
        {
            return CellToWorld(new Vector3Int(tile_coord.x, tile_coord.y, 0));
        }
        /// <summary>
        /// переводит кооринаты тайла в координаты сцены
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 CellToWorld(int x, int y)
        {
            return CellToWorld(new Vector3Int(x, y, 0));
        }
        /// <summary>
        /// переводит кооринаты тайла в координаты капты
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector2 CellToMap(int x, int y) => CellToMap(new Vector2Int(x, y));

        /// <summary>
        /// вычисляет расстояние на карте
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public float MapDistance(Vector2Int from, Vector2Int to)
        {
            var t_from = CellToMap(from);
            var t_to = CellToMap(to);

            return Vector2.Distance(t_from, t_to);
        }

        /// <summary>
        /// возвращает последовательность тайлов между двумя точками. t - положение тайла на прямой где 0=from 1=to
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
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