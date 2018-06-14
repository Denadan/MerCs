using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical
{
    public class TileInfo
    {
        public Vector3Int CellCoord { get; set; }
//        public Vector2 WorldCoord { get; set; }
        public int Height => CellCoord.z;
        public float AddedHeight
        {
            get
            {
                switch(Feature)
                {
                    case TileFeature.Forest:
                        return 2f;
                    case TileFeature.Pillar:
                        return 10f;
                    case TileFeature.Water:
                        return 1f;
                }
                return 0f;
            }
        }

        public TileFeature Feature { get; set; }

        public bool HasCover => Feature == TileFeature.Forest || Feature == TileFeature.Cover;

        public Visibility Vision;
    }
}
