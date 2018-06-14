using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical
{
    public class TileInfo
    {
        public Vector3Int CellCoord { get; set; }
//        public Vector2 WorldCoord { get; set; }
        public int Height => CellCoord.z;

        public TileFeature Feature { get; set; }

        public bool HasCover => Feature == TileFeature.Forest || Feature == TileFeature.Cover;

        public Visibility Vision;
    }
}
