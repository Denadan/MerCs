using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        private bool DrawOBorder = false;

        private Dictionary<Dir, Sprite[]> borders = new Dictionary<Dir, Sprite[]>();

        [SerializeField]
        private GameObject[] Marks;
        [SerializeField]
        private GameObject[] Arrows;

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

                    var digit = Instantiate(Marks[map[i, j].PathTileCost - 1], pos, Quaternion.identity, PathCostParent);
                    digit.name = $"mark_{i}_{j}";
                    foreach (var pathInfo in map[i,j].PathList)
                    {
                        var s_pos = CellToWorld(map[i, j].CellCoord);
                        var t_pos = CellToWorld(pathInfo.Value.Neighbour.CellCoord);

                        var arrow = Instantiate(Arrows[pathInfo.Value.Cost - 1], Vector3.Lerp(s_pos, t_pos, 0.35f),
                            Quaternion.Euler(0,0,CONST.GetAngleV(pathInfo.Key)), LinksCostParent);
                        arrow.name = $"arrow_{i}_{j}_{pathInfo.Key}";
                    }

                    foreach(var dir in CONST.AllDirs)
                    {
                        var shift = CONST.GetDirShift(i, j, dir);
                        var neigbour = map[i + shift.x, j + shift.y];
                        if (neigbour == null)
                            continue;
                        if (neigbour.Height < info.Info.Height || (!DrawOBorder && neigbour.Height == info.Info.Height))
                            continue;
                        int hd = neigbour.Height- info.Info.Height;
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