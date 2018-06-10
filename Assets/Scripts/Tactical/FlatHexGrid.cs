#pragma warning disable 649

using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Mercs.Tactical
{

    public class FlatHexGrid : HexGrid
    {
        [SerializeField]
        private float SizeX = 0.75f;
        [SerializeField]
        private float SizeY = 6f / 7f;

        [SerializeField]
        private GameObject[] NormalTiles;
        [SerializeField]
        private GameObject[] WaterTiles;
        [SerializeField]
        private GameObject[] ForestTiles;
        [SerializeField]
        private GameObject[] RoughTiles;

        [SerializeField]
        private bool DrawOBorder;


        [SerializeField]
        private GameObject BorderPrefab;
        [SerializeField]
        private Sprite[] BorderN;
        [SerializeField]
        private Sprite[] BorderS;
        [SerializeField]
        private Sprite[] BorderSW;
        [SerializeField]
        private Sprite[] BorderSE;
        [SerializeField]
        private Sprite[] BorderNW;
        [SerializeField]
        private Sprite[] BorderNE;
        [SerializeField]

        private Dictionary<Dir, Sprite[]> borders = new Dictionary<Dir, Sprite[]>();

        public override Vector3 CellToWorld(Vector3Int tile_coord)
        {
            float x = SizeX * tile_coord.x;
            float y = SizeY * tile_coord.y + SizeY * 0.5f * (tile_coord.x % 2);
            return new Vector3(x, y, 0);
        }


        override protected void MakeTiles()
        {
            if(borders.Count == 0)
            {
                borders.Add(Dir.N, BorderN);
                borders.Add(Dir.NE, BorderNE);
                borders.Add(Dir.NW, BorderNW);
                borders.Add(Dir.S, BorderS);
                borders.Add(Dir.SE, BorderSE);
                borders.Add(Dir.SW, BorderSW);
            }

            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    GameObject sample = null;
                    switch(map[i,j].Feature)
                    {
                        case TileFeature.Forest:
                            sample = ForestTiles.Rnd();
                            break;
                        case TileFeature.Water:
                            sample = WaterTiles.Rnd();
                            break;
                        case TileFeature.Rough:
                            sample = RoughTiles.Rnd();
                            break;
                        default:
                            sample = NormalTiles.Rnd();
                            break;
                    }


                    var pos = CellToWorld(new Vector3Int(i, j, 0));
                    var tile = Instantiate(sample , pos, Quaternion.identity, TilesParent);
                    tile_map[i, j] = tile;
                    tile.name = $"tile_{i}_{j}";
                    var info = tile.GetComponent<TileInfoComponent>();
                    info.Map = map;
                    info.Info = map[i, j];

                  
                    foreach (var dir in DirHelper.AllDirs)
                    {
                        var shift = dir.GetDirShift(i, j);
                        var neigbour = map[i + shift.x, j + shift.y];
                        if (neigbour == null)
                            continue;
                        if (neigbour.Height < info.Info.Height || (!DrawOBorder && neigbour.Height == info.Info.Height))
                            continue;
                        int hd = neigbour.Height - info.Info.Height;
                        var borderlist = borders[dir];
                        if (hd >= borderlist.Length)
                            hd = borderlist.Length - 1;
                        var border = Instantiate(BorderPrefab, pos, Quaternion.identity, tile.transform);
                        border.GetComponent<SpriteRenderer>().sprite = borderlist[hd];
                    }
                }
        }

    }
}