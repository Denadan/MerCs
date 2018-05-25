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

        public abstract void Generate();
    }
}