using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Mercs.Tactical
{
    /// <summary>
    /// компонент для поиска пути
    /// </summary>
    public class PathMap : MonoBehaviour
    {
        /// <summary>
        /// узел карты проходимости
        /// </summary>
        public class pathmap_node
        {
            /// <summary>
            /// переход
            /// </summary>
            public struct path_link
            {
                public Vector2Int target;
                /// <summary>
                /// цена перехода с учетом разницы высот
                /// </summary>
                public int cost;
                public bool Passable { get => cost > 0; }
            }

            /// <summary>
            /// цена перехода на клетку
            /// </summary>
            public int Cost;
            /// <summary>
            /// переходы из клетки
            /// </summary>
            public Dictionary<Dir, path_link> Links = new Dictionary<Dir, path_link>();
        }

        /// <summary>
        /// конечный узел пути
        /// </summary>
        public class path_target
        {
            //координаты
            public Vector2Int coord;
            //самый быстрый путь до точки
            public path_node fast_path;
            //пути до точки с различным поворотом
            public Dictionary<Dir, path_node> other_path = new Dictionary<Dir, path_node>();
            // список направлений, куда можно повернутся дойдя до точки
            public Dir[] AllowedDir
            {
                get => other_path.Keys.ToArray();
            }


        }

        // TODO: убрать? 
        /// <summary>
        /// ? непригодилось?
        /// </summary>
        public enum move
        {
            step_forward,
            step_back,
            turn_left,
            turn_left2,
            turn_right,
            turn_right2,
            turn_left3
        }

        /// <summary>
        /// Узел маршрута
        /// </summary>
        public class path_node
        {
            /// <summary>
            /// координаты
            /// </summary>
            public Vector2Int coord;
            /// <summary>
            /// направление юнита по входу
            /// </summary>
            public Dir facing;
            /// <summary>
            /// оставшееся кол-во очков хода
            /// </summary>
            public int mpleft;

            /// <summary>
            /// прошлый узел пути
            /// </summary>
            public path_node prev;
        }

        /// <summary>
        /// ссылка на карту
        /// </summary>
        private Map map;
        /// <summary>
        /// используемые направления
        /// </summary>
        private Dir[] dirs;
        /// <summary>
        /// карта проходимости
        /// </summary>
        private pathmap_node[,] nodes;
        /// <summary>
        /// таск поиска пути
        /// </summary>
        private Task make_path;
        /// <summary>
        /// токен отмены поиска
        /// </summary>
        private CancellationTokenSource token;

        public float DEBUG_TimeStart, DEBUG_TimeEnd;

        /// <summary>
        /// получить узел карты проходимости по указанным координатам
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public pathmap_node this[int x, int y]
        {
            get
            {
                if (map == null || x < 0 || x >= map.SizeX || y < 0 || y >= map.SizeY)
                    return null;
                return nodes[x, y];
            }
        }
        /// <summary>
        /// получить узел проходимости по указанным координатам
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public pathmap_node this[Vector2Int c] => this[c.x, c.y];

        public List<path_target> RunList { get; private set; }
        public List<path_target> MoveList { get; private set; }
        public List<path_target> JumpList { get; private set; }

        /// <summary>
        /// поиск пути завершен
        /// </summary>
        public bool Ready { get; private set; }
        /// <summary>
        /// юнит, для которого осуществлялся поиск пути
        /// </summary>
        public UnitInfo Unit { get; private set; }
        /// <summary>
        /// оригинальная позиция юнита
        /// </summary>
        public Vector2Int UnitPos { get; private set; }

        /// <summary>
        /// создаем карту проходимости
        /// </summary>
        public void CreatePathMap()
        {
            map = GetComponent<Map>();
            dirs = CONST.AllDirs;
            nodes = new pathmap_node[map.SizeX, map.SizeY];

            //получаем стоимость перехода для каждой точки пути
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    var tile = map[i, j];
                    if (tile == null)
                        throw new InvalidOperationException($"Не могу построить карту путей - пустые ячейка по координатам ({i}, {j}) ");
                    nodes[i, j] = new pathmap_node()
                    { Cost = CONST.MoveCose[tile.Feature] };
                }
            //начинаем строить соединения
            for (int i = 0; i < map.SizeX; i++)
                for (int j = 0; j < map.SizeY; j++)
                {
                    var tile = map[i, j];
                    var node = nodes[i, j];
                    //для каждого доступного направления
                    foreach (var dir in dirs)
                    {
                        //находим соседа
                        var shift = CONST.GetDirShift(i, j, dir);
                        var neighbour = map[i + shift.x, j + shift.y];
                        //соседа нет - пропускаем
                        if (neighbour == null)
                            continue;
                        //разница высот
                        int dif = Math.Abs(neighbour.Height - tile.Height);
                        //если больше 2х - пешком низзя!
                        if (dif > 2)
                            continue;

                        //добавляем линк к узлу
                        var coord = new Vector2Int(neighbour.CellCoord.x, neighbour.CellCoord.y);
                        node.Links.Add(dir, new pathmap_node.path_link
                        {
                            cost = dif + nodes[coord.x, coord.y].Cost,
                            target = coord
                        });
                    }
                }
        }

        /// <summary>
        /// создаем маршруты для указанного юнита
        /// </summary>
        /// <param name="unit">если null - отмена</param>
        public void MakePathMap(UnitInfo unit)
        {
            // если уже идет поиск пути - отменить
            if (make_path != null && make_path.Status == TaskStatus.Running)
                token.Cancel();
            //если null - сбрасываем состояние и выходим
            if (unit == null)
            {
                Ready = true;
                Unit = null;
                return;
            }

            //запускаем поиск
            DEBUG_TimeStart = Time.realtimeSinceStartup;
            Ready = false;
            Unit = unit;
            UnitPos = unit.Position.position;

            token = new CancellationTokenSource();

            make_path = new Task(() => calc_path(token.Token));
            make_path.Start();
        }

        /// <summary>
        /// поиск пути
        /// </summary>
        /// <param name="token">токен отмены</param>
        private void calc_path(CancellationToken token)
        {
            //поиск пути для движения пешком
            if (Unit.Movement.MoveMp > 0)
            {
                //получаем список всех путей и сортируем его по затратам од
                List<path_node> run_list = step_move(new path_node
                {
                    coord = UnitPos,
                    facing = Unit.Position.Facing,
                    mpleft = Unit.Movement.MoveMp,
                    prev = null
                },
                new List<path_node>()).OrderByDescending(item => item.mpleft).ToList();


                //список всех доступных точек
                MoveList = run_list
                    .Select(i => i.coord)
                    .Distinct()
                    .Select(c => new path_target
                    {
                        coord = c,
                        fast_path = run_list.Find(i => i.coord == c)
                    })
                    .ToList();
            }

            //поиск пути для бега
            if (Unit.Movement.RunMP > 0)
            {
                //получаем список всех путей и сортируем его по затратам од
                List<path_node> run_list = step_run(new path_node
                {
                    coord = UnitPos,
                    facing = Unit.Position.Facing,
                    mpleft = Unit.Movement.RunMP,
                    prev = null
                },
                new List<path_node>()).OrderByDescending(item => item.mpleft).ToList();


                //список всех доступных точек
                RunList = run_list
                    .Select(i => i.coord)
                    .Distinct()
                    .Select(c => new path_target
                    {
                        coord = c,
                        fast_path = run_list.Find(i => i.coord == c)
                    })
                    .ToList();

            }

            //поиск пути для прижка
            if (Unit.Movement.JumpMP > 0)
            {

            }

            //закончили, можно пользоваться
            Ready = true;
        }

        /// <summary>
        /// рекурсивный поиск пути для бега
        /// </summary>
        /// <param name="source">стартовый узел</param>
        /// <param name="list">список пройденных узлов</param>
        private List<path_node> step_run(path_node source, List<path_node> list)
        {
            //продолжение поиска в указанном направлении
            void step(Dir facing, int bonus)
            {
                //если есть переход по указанному направлению
                if (this[source.coord].Links.TryGetValue(facing, out var link)
                // и хватает очков движения
                    && link.cost + bonus <= source.mpleft)
                    //переходим
                    step_run(new path_node
                    {
                        coord = link.target,
                        facing = facing,
                        mpleft = source.mpleft - link.cost - bonus,
                        prev = source
                    }, list);
            }

            //добавлеям стартовый узел в список пройденых
            list.Add(source);
            //продолжаем поиск вперед
            step(source.facing, 0);
            //продолжаем поиск влево
            step(CONST.TurnLeft(source.facing), 1);
            //продолжаем поиск вправо
            step(CONST.TurnRight(source.facing), 1);
            return list;
        }

        private List<path_node> step_move(path_node source, List<path_node> list)
        {
            //продолжение поиска в указанном направлении
            void step(Dir facing, int bonus)
            {
                //если есть переход по указанному направлению
                if (this[source.coord].Links.TryGetValue(facing, out var link)
                // и хватает очков движения
                    && link.cost + bonus <= source.mpleft)
                    //переходим
                    step_run(new path_node
                    {
                        coord = link.target,
                        facing = facing,
                        mpleft = source.mpleft - link.cost - bonus,
                        prev = source
                    }, list);
            }

            void step_back(Dir facing, int bonus)
            {
                //если есть переход по указанному направлению
                if (this[source.coord].Links.TryGetValue(CONST.Inverse(facing), out var link)
                // и хватает очков движения
                    && link.cost + bonus <= source.mpleft)
                    //переходим
                    step_run(new path_node
                    {
                        coord = link.target,
                        facing = facing,
                        mpleft = source.mpleft - link.cost - bonus,
                        prev = source
                    }, list);
            }

            //добавлеям стартовый узел в список пройденых
            list.Add(source);
            //поиск движения вперед
            //продолжаем поиск вперед
            step(source.facing, 0);
            //продолжаем поиск влево
            var dl1 = CONST.TurnLeft(source.facing);
            var dl2 = CONST.TurnLeft(dl1);
            step(dl1, 1);
            step(dl2, 2);
            //продолжаем поиск вперед
            var dr1 = CONST.TurnRight(source.facing);
            var dr2 = CONST.TurnRight(dr1);
            step(dr1, 1);
            step(dr2, 2);
            //разворот
            step(CONST.Inverse(source.facing), 3);

            //движения назад
            step_back(source.facing, 0);
            //поворот влево
            step_back(dl1, 1);
            step_back(dl2, 2);
            //поворот вправо
            step_back(dr1, 1);
            step_back(dr2, 2);

            return list;
        }
    }
}
