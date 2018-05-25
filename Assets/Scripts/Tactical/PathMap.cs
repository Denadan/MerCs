using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical
{
    public class PathMap : MonoBehaviour
    {
        private Map map;
        private Dir[] dirs;

        

        public void CreatePathMap()
        {
            map = GetComponent<Map>();
            var hex_map = GetComponent<HexGrid>();
            dirs = CONST.AllDirs;

            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    var tile = map[i, j];
                    if (tile == null)
                        return;
                    tile.PathTileCost = CONST.MoveCose[tile.Feature];
                }
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    var tile = map[i, j];
                    tile.PathList = new Dictionary<Dir, TileInfo.PathInfo>();
                    foreach (var dir in dirs)
                    {
                        var shift = CONST.GetDirShift(i, j, dir);
                        //UnityEngine.Debug.Log($"{i} {j} {dir} {shift}");
                        var neighbour = map[i + shift.x, j + shift.y];
                        if(neighbour == null)
                            continue;
                        int dif = Math.Abs(neighbour.Height - tile.Height);
                        if(dif > 2)
                            continue;
                        tile.PathList.Add( dir, new TileInfo.PathInfo { Cost = dif + neighbour.PathTileCost, Neighbour = neighbour });
                    }
                }
        }
    }
}
