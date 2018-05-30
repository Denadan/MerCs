using System.Collections.Generic;
using Tools;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Mercs.Tactical.Events
{
    public class EventHandler : SceneSingleton<EventHandler>
    {
        private List<GameObject> TileSubscribers = new List<GameObject>();
        private List<GameObject> UnitSubscribers = new List<GameObject>();
        private List<GameObject> PilotHpSubscribers = new List<GameObject>();

        public static void UnitPointerClick(PointerEventData data, UnitInfo Unit)
        {
            if (Instance == null)
                return;
            if (Unit != null)
            {
                foreach (var unitSubscriber in Instance.UnitSubscribers)
                {
                    ExecuteEvents.Execute<IUnitEventReceiver>(unitSubscriber, data,
                        (unit, eventdata) => unit.MouseUnitClick(Unit, data.button));
                }
            }
        }
        public static void UnitPointerEnter(PointerEventData data, UnitInfo Unit)
        {
            if (Instance == null)
                return;
            if (Unit != null)
            {
                foreach (var unitSubscriber in Instance.UnitSubscribers)
                {
                    ExecuteEvents.Execute<IUnitEventReceiver>(unitSubscriber, data,
                        (unit, eventdata) => unit.MouseUnitEnter(Unit));
                }
                var coord = Unit.GetComponent<CellPosition>();
                if (coord != null)
                    foreach (var tileSubscriber in Instance.TileSubscribers)
                    {
                        ExecuteEvents.Execute<ITileEventReceiver>(tileSubscriber, data,
                            (handler, eventData) => handler.MouseTileEnter(coord.position));
                    }
            }
        }
        public static void UnitPointerLeave(PointerEventData data, UnitInfo Unit)
        {
            if (Instance == null)
                return;
            if (Unit != null)
            {
                foreach (var unitSubscriber in Instance.UnitSubscribers)
                {
                    ExecuteEvents.Execute<IUnitEventReceiver>(unitSubscriber, data,
                        (unit, eventdata) => unit.MouseUnitLeave(Unit));
                }
                var coord = Unit.GetComponent<CellPosition>();
                if (coord != null)
                    foreach (var tileSubscriber in Instance.TileSubscribers)
                    {
                        ExecuteEvents.Execute<ITileEventReceiver>(tileSubscriber, data,
                            (handler, eventData) => handler.MouseTileLeave(coord.position));
                    }
            }
        }
        public static void TilePointerClick(PointerEventData data, Vector2Int coord)
        {
            if (Instance == null)
                return;
            foreach (var tileSubscriber in Instance.TileSubscribers)
            {
                ExecuteEvents.Execute<ITileEventReceiver>(tileSubscriber, data,
                    (handler, eventData) => handler.MouseTileClick(coord, data.button));
            }
        }
        public static void TilePointerEnter(PointerEventData data, Vector2Int coord)
        {
            if (Instance == null)
                return;
            foreach (var tileSubscriber in Instance.TileSubscribers)
            {
                ExecuteEvents.Execute<ITileEventReceiver>(tileSubscriber, data,
                    (handler, eventData) => handler.MouseTileEnter(coord));
            }
        }
        public static void TilePointerLeave(PointerEventData data, Vector2Int coord)
        {
            if (Instance == null)
                return;
            foreach (var tileSubscriber in Instance.TileSubscribers)
            {
                ExecuteEvents.Execute<ITileEventReceiver>(tileSubscriber, data,
                    (handler, eventData) => handler.MouseTileLeave(coord));
            }
        }

        public static void PilotHpChange(UnitInfo unit)
        {
            if (Instance == null)
                return;
            foreach (var subscriber in Instance.PilotHpSubscribers)
            {
                ExecuteEvents.Execute<IPilotDamaged>(subscriber, null,
                    (handler, eventData) => handler.PilotDamaged(unit));
            }
        }

        public static void TileSubscribe(GameObject go)
        {
            if (Instance == null)
                return;
            Instance.TileSubscribers.Add(go);
        }

        public static void TileUnSubscribe(GameObject go)
        {
            if (Instance == null)
                return;
            Instance.TileSubscribers.Remove(go);
        }

        public static void UnitSubscribe(GameObject go)
        {
            if (Instance == null)
                return;
            Instance.UnitSubscribers.Add(go);
        }

        public static void UnitUnSubscribe(GameObject go)
        {
            if (Instance == null)
                return;
            Instance.UnitSubscribers.Remove(go);
        }

        public static void PilotHpSubscribe(GameObject go)
        {
            if (Instance == null)
                return;
            Instance.PilotHpSubscribers.Add(go);
        }
        public static void PilotHpUnSubscribe(GameObject go)
        {
            if (Instance == null)
                return;
            Instance.PilotHpSubscribers.Remove(go);
        }
    }
}
