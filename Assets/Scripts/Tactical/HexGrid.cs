using UnityEngine;


namespace Mercs.Tactical
{
    public abstract class HexGrid : MonoBehaviour
    {
        public abstract Vector3 CellToWorld(Vector3Int tile_coord);
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

        // Use this for initialization
        void Start()
        {
            map = GetComponent<Map>();
            path = GetComponent<PathMap>();

            tile_map = new GameObject[map.SizeX, map.SizeY];


            map.Generate();

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


}

}