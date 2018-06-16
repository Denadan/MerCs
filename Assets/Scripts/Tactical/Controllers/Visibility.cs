using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mercs.Tactical.States;
using UnityEngine;
using UnityEngine.Experimental.Audio.Google;

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


        


        private class vision_info
        {
            public UnitInfo from;
            public UnitInfo to;
            public Level level;
            public bool direct;
        }


        private List<vision_info> units;

        private Dictionary<UnitInfo, List<GameObject>> subscribers;

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


                    units.Add(new vision_info
                    {
                        from = unitfrom,
                        to = unitto,
                        level = Level.None,
                        direct = false
                    });
                }
        }

        private void rebuild_dictionary()
        {
            looked_from = units.GroupBy(i => i.@from).ToDictionary(i => i.Key, i => i.ToList());
            looked_to = units.GroupBy(i => i.to).ToDictionary(i => i.Key, i => i.ToList());
        }

        private (Level level, bool direct) calc_vision(UnitInfo from_unit, UnitInfo to_unit) =>
            calc_vision(from_unit, from_unit.Position.position, to_unit, to_unit.Position.position);

        private (Level level, bool direct) calc_vision(UnitInfo from_unit, Vector2Int from, UnitInfo to_unit,
            Vector2Int to)
        {
            return (Level.None, false);
        }

        public void RecalcVision(UnitInfo unit)
        {

        }

        public void RemoveUnit(UnitInfo unit)
        {
            units.RemoveAll(u => u.@from == unit || u.to == unit);
            rebuild_dictionary();
        }

        public IEnumerable<( UnitInfo target, Level level, bool direct)> GetFrom(UnitInfo unit)
        {
            return looked_from.TryGetValue(unit, out var list)
                ? from item in list select (target: item.to, level: item.level, direct: item.direct)
                : Enumerable.Empty<(UnitInfo target, Level level, bool direct)> ();
        }

        public IEnumerable<(UnitInfo target, Level level, bool direct)> GetTo(UnitInfo unit)
        {
            return looked_to.TryGetValue(unit, out var list)
                ? from item in list select (target: item.@from, level: item.level, direct: item.direct)
                : Enumerable.Empty<(UnitInfo target, Level level, bool direct)>();
        }

        public IEnumerable<(UnitInfo target, Level level, bool direct)> CalcFrom(UnitInfo unit, Vector2Int pos)
        {
            return null;
        }

        public Level GetLevelFor(UnitInfo info)
        {
            return looked_to.TryGetValue(info, out var list) ? list.Max(i => i.level) : Level.None;
        }

        public void UnSubscribe(UnitInfo selectedUnit, GameObject o)
        {
            if (subscribers.TryGetValue(selectedUnit, out var subs))
            {
                subs = new List<GameObject>();
                subs.Remove(o);
            }
        }

        public void Subscribe(UnitInfo selectedUnit, GameObject o)
        {
            if (!subscribers.TryGetValue(selectedUnit, out var subs))
            {
                subs = new List<GameObject>();
                subscribers.Add(selectedUnit, subs);
            }

            subs.Add(o);
        }
    }
}