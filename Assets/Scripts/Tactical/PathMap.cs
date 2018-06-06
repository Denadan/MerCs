using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace Mercs.Tactical
{
    public class PathMap : MonoBehaviour
    {
        public class path_node
        {
            public struct path_link
            {
                public Vector2 target;
                public int cost;
                public bool Passable { get => cost > 0; }
            }

            public int Cost;
            public Dictionary<Dir, path_link> Links = new Dictionary<Dir, path_link>();
            
            public void Clear()
            {

            }
        }

        private Map map;
        private Dir[] dirs;
        private path_node[,] nodes;
        private Task make_path;
        private CancellationTokenSource token;

        public float DEBUG_TimeStart, DEBUG_TimeEnd;
        

        public path_node this[int x, int y]
        {
            get
            {
                if (map == null || x < 0 || x >= map.SizeX || y < 0 || y >= map.SizeY)
                    return null;
                return nodes[x, y];
            }
        }
        public path_node this[Vector2Int c] => this[c.x, c.y];

        public bool Ready { get; private set; }
        public UnitInfo Unit { get; private set; }
        public Vector2Int UnitPos { get; private set; }

        public void CreatePathMap()
        {
            map = GetComponent<Map>();
            dirs = CONST.AllDirs;
            nodes = new path_node[map.SizeX, map.SizeY];

            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    var tile = map[i, j];
                    if (tile == null)
                        throw new InvalidOperationException($"Не могу построить карту путей - пустые ячейка по координатам ({i}, {j}) ");
                    nodes[i,j] = new path_node()
                    { Cost = CONST.MoveCose[tile.Feature] };
                }
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    var tile = map[i, j];
                    var node = nodes[i, j];
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

                        var coord = new Vector2Int(neighbour.CellCoord.x, neighbour.CellCoord.y);
                        node.Links.Add( dir, new path_node.path_link
                            {
                                cost = dif + nodes[coord.x, coord.y].Cost,
                                target = coord
                            });
                    }
                }
        }

        public void MakePathMap(UnitInfo unit)
        {
            if (make_path != null && make_path.Status == TaskStatus.Running)
                token.Cancel();
            if (unit == null)
                return;

            DEBUG_TimeStart = Time.realtimeSinceStartup;
            Ready = false;
            Unit = unit;
            UnitPos = unit.Position.position;

            token = new CancellationTokenSource();

            make_path = new Task(() => calc_path(token.Token, DEBUG_TimeStart));
            make_path.Start();
        }

        private void calc_path(CancellationToken token, float start)
        {
            for (int i = 0; i < 50;i++)
            {
                Thread.Sleep(100);
                if (token.IsCancellationRequested)
                    break;
            }
            Ready = true;
       }
    }
}
