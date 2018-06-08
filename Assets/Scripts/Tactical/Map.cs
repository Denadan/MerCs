using UnityEngine;

namespace Mercs.Tactical
{
    public abstract class Map : MonoBehaviour
    {
        public int SizeX;
        public int SizeY;


        protected TileInfo[,] map;

        public TileInfo this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= SizeX || y < 0 || y >= SizeY)
                    return null;
                return map[x, y];
            }
        }

        public bool OnMap(int x, int y) => x >= 0 && x < SizeX && y >= 0 && y < SizeY;
        public bool OnMap(Vector2Int coord) => OnMap(coord.x, coord.y);

        public TileInfo this[Vector2Int coord] => this[coord.x, coord.y];

        public float Distance(Vector2Int from, Vector2Int to)
        {
            var t_from = this[from];
            var t_to = this[to];
            if (t_from == null || t_to == null)
                return 999f;

            return Vector2.Distance(t_from.WorldCoord, t_to.WorldCoord) * 0.99f;
        }

        public abstract void Generate();
    }
}