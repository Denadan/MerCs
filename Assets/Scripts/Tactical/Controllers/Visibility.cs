﻿using System.Collections.Generic;
using System.Linq;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical
{
    public class Visibility
        : MonoBehaviour
    {
        public enum Level
        {
            /// <summary>
            /// dont show on map
            /// </summary>
            None,
            /// <summary>
            /// show blip, cannot attack
            /// </summary>
            Sensor,
            /// <summary>
            /// show limited info, only source can fire
            /// </summary>
            Visual,
            /// <summary>
            /// show full info, all network can fire
            /// </summary>
            Scanned,
            /// <summary>
            /// friendly unit, show full
            /// </summary>
            Friendly
        }

        public enum Line { Indirect, Partial, Dirrect }

        public class LoS
        {
            public UnitInfo Target;
            public float Distance;
            public Level Level;
            public Line Line;
        }

        private class vision_info
        {
            public UnitInfo from;
            public UnitInfo to;
            public Level level;
            public Line direct;
            public float distance;
        }

        private List<vision_info> units;

        private Dictionary<UnitInfo, List<GameObject>> subscribers = new Dictionary<UnitInfo, List<GameObject>>();

        private Dictionary<UnitInfo, List<vision_info>> looked_from;
        private Dictionary<UnitInfo, List<vision_info>> looked_to;

        private HexGrid grid;
        private Map map;

        public void Init()
        {
            grid = GetComponent<HexGrid>();
            map = GetComponent<Map>();

            units = new List<vision_info>();

            foreach (var unitfrom in TacticalController.Instance.Units)
                foreach (var unitto in TacticalController.Instance.Units.Where(u => u.Faction != unitfrom.Faction))
                {
                    var info = new vision_info
                    {
                        from = unitfrom,
                        to = unitto
                    };

                    (info.level, info.direct, info.distance) = calc_vision(info);

                    units.Add(info);
                }

            rebuild_dictionary();

            foreach (var unit in TacticalController.Instance.Units)
            {
                unit.CurrentVision = GetLevelFor(unit);
                EventHandler.Raise<IVisionChanged>((i, d) => i.VisionChanged(unit, unit.CurrentVision));
            }
        }

        private (Level, Line, float) calc_vision(vision_info info) =>
            calc_vision(info.@from, info.@from.Position.position, info.to, info.to.Position.position);
        private (Level, Line, float) calc_vision(vision_info info, Vector2Int f_pos) =>
            calc_vision(info.@from, f_pos, info.to, info.to.Position.position);
        private (Level, Line, float) calc_vision_to(vision_info info, Vector2Int pos) =>
            calc_vision(info.@from, info.@from.Position.position, info.to, pos);


        private (Level, Line, float) calc_vision(UnitInfo from, Vector2Int f_pos, UnitInfo to, Vector2Int t_pos)
        {
            var dist = grid.MapDistance(f_pos, t_pos);
            if (!map.OnMap(f_pos) || !map.OnMap(t_pos))
                return (Level.None, Line.Indirect, dist);

            var direct = HaveDirect(f_pos, from.Height, t_pos, to.Height);

            if (dist > from.RadarRange && dist > from.VisualRange)
                return (Level.None, direct, dist);

            if (direct == Line.Indirect)
                return (dist > from.RadarRange || to.Buffs.Shutdown ? Level.None : Level.Sensor, direct, dist);

            if (dist > from.VisualRange)
                return (dist > from.ScanRange ? (to.Buffs.Shutdown ? Level.None : Level.Sensor) : Level.Scanned, direct, dist);
            else
                return (dist > from.ScanRange ? Level.Visual : Level.Scanned, direct, dist);
        }


        /// <summary>
        /// calc if have direct vission
        /// </summary>
        /// <param name="from"></param>
        /// <param name="from_height"></param>
        /// <param name="to"></param>
        /// <param name="to_height"></param>
        /// <returns></returns>
        private Line HaveDirect(Vector2Int from, float from_height, Vector2Int to, float to_height)
        {

            var trace = grid.Trace(from, to);

            if (trace.Count <= 2)
                return Line.Dirrect;

            from_height += map[from].Height;
            to_height += map[to].Height;
            var to0_height = map[to].Height;

            var diff = to_height - from_height;
            var diff0 = to0_height - from_height;

            bool full = true;
            int forest = 0;

            for (int i = 1; i < trace.Count - 1; i++)
            {
                var tile = map[trace[i].point];

                if (tile == null)
                    continue;

                float h1 = from_height + diff * trace[i].t;
                float h0 = from_height + diff0 * trace[i].t;

                if (tile.Height > h1)
                    return Line.Indirect;
                if (tile.Height + tile.AddedHeight > h1)
                    if (tile.HasCover)
                        forest += 1;
                    else
                        return Line.Indirect;

                if (full & (tile.Height > h0 || !tile.HasCover && tile.Height + tile.AddedHeight > h0))
                    full = false;
            }

            if (forest > 3)
                return Line.Indirect;

            if (forest > 2)
                return Line.Partial;

            return full ? Line.Dirrect : Line.Partial;
        }

        private void rebuild_dictionary()
        {
            looked_from = units.GroupBy(i => i.@from).ToDictionary(i => i.Key, i => i.ToList());
            looked_to = units.GroupBy(i => i.to).ToDictionary(i => i.Key, i => i.ToList());
        }


        public void RecalcVision(UnitInfo unit)
        {

            if (looked_from.TryGetValue(unit, out var list_f))
                foreach (var info in list_f)
                {
                    (info.level, info.direct, info.distance) = calc_vision(info);

                    var l = GetLevelFor(info.to);
                    if (info.to.CurrentVision != l)
                    {
                        info.to.CurrentVision = l;
                        EventHandler.Raise<IVisionChanged>((i, d) => i.VisionChanged(info.to, l));
                    }
                }

            if (looked_to.TryGetValue(unit, out var list_t))
                foreach (var info in list_t)
                    (info.level, info.direct, info.distance) = calc_vision(info);

            var new_level = GetLevelFor(unit);
            if (unit.CurrentVision != new_level)
            {
                unit.CurrentVision = new_level;
                EventHandler.Raise<IVisionChanged>((i, d) => i.VisionChanged(unit, new_level));
            }
        }

        public void RecalcVision(UnitInfo unit, Vector2Int pos)
        {

            if (looked_from.TryGetValue(unit, out var list_f))
                foreach (var info in list_f)
                {
                    (info.level, info.direct, info.distance) = calc_vision(info, pos);

                    var l = GetLevelFor(info.to);
                    if (info.to.CurrentVision != l)
                    {
                        info.to.CurrentVision = l;
                        EventHandler.Raise<IVisionChanged>((i, d) => i.VisionChanged(info.to, l));
                    }
                }

            if (looked_to.TryGetValue(unit, out var list_t))
                foreach (var info in list_t)
                    (info.level, info.direct, info.distance) = calc_vision_to(info, pos);

            var new_level = GetLevelFor(unit);
            if (unit.CurrentVision != new_level)
            {
                unit.CurrentVision = new_level;
                EventHandler.Raise<IVisionChanged>((i, d) => i.VisionChanged(unit, new_level));
            }
        }


        public void RemoveUnit(UnitInfo unit)
        {
            units.RemoveAll(u => u.@from == unit || u.to == unit);
            rebuild_dictionary();
        }

        public IEnumerable<LoS> GetFrom(UnitInfo unit)
        {
            return looked_from.TryGetValue(unit, out var list)
                ? from item in list
                  select new LoS
                  {
                      Distance = item.distance,
                      Level = item.level,
                      Line = item.direct,
                      Target = item.to
                  }
                : Enumerable.Empty<LoS>();
        }

        public IEnumerable<LoS> GetTo(UnitInfo unit)
        {
            return looked_to.TryGetValue(unit, out var list)
                ? from item in list
                  select new LoS
                  {
                      Distance = item.distance,
                      Level = item.level,
                      Line = item.direct,
                      Target = item.@from
                  }
                : Enumerable.Empty<LoS>(); ;
        }

        public IEnumerable<LoS> CalcFrom(UnitInfo unit, Vector2Int pos)
        {
            var res = new List<LoS>();

            if (looked_from.TryGetValue(unit, out var list))
                foreach (var info in list)
                {
                    var val = calc_vision(info, pos);
                    res.Add(new LoS
                    {
                        Distance = val.Item3,
                        Level = val.Item1,
                        Line = val.Item2,
                        Target = info.to
                    });
                }
            return res;
        }

        private Level GetLevelFor(UnitInfo info)
        {
            if (looked_to.TryGetValue(info, out var list))
                return list.Max(i => i.level);
            else
                return Level.None;
        }

        public Level GetLevelFor(UnitInfo info, UnitInfo from, Level level)
        {
            var l = looked_to.TryGetValue(info, out var list) ?
                list.Where(i => i.from != from).Max(i => i.level) :
                Level.None;

            return level > l ? level : l;
        }

    }
}