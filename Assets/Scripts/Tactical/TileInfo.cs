using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical
{
    public class TileInfo
    {
        public class PathInfo
        {
            public TileInfo Neighbour;
            public int Cost;
        }

        public Vector3Int CellCoord { get; set; }
        public Vector2 WorldCoord { get; set; }
        public int Height => CellCoord.z;

        public TileFeature Feature { get; set; }

        public bool HasCover => Feature == TileFeature.Forest || Feature == TileFeature.Cover;

        public int PathTileCost { get; set; }
        public Dictionary<Dir, PathInfo> PathList;

        public Visibility Vision;
    }
}
