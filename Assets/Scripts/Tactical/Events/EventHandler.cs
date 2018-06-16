using System;
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
        private Dictionary<UnitInfo, PilotHp> pilotHps = new Dictionary<UnitInfo, PilotHp>();
        private Dictionary<UnitInfo, UnitHp> UnitHPs = new Dictionary<UnitInfo, UnitHp>();

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

        public static void PilotHpSubscribe(GameObject go, UnitInfo unit)
        {
            if (Instance == null)
                return;

            if(Instance.pilotHps.TryGetValue(unit, out var hp))
                hp.Subscribe(go);
        }

        public static void PilotHpUnSubscribe(GameObject go, UnitInfo unit)
        {
            if (Instance == null)
                return;
            if (Instance.pilotHps.TryGetValue(unit, out var hp))
                hp.Unsubscribe(go);
        }

        public static void RegisterPilotHp(UnitInfo unit, PilotHp hp)
        {
            if (Instance == null)
                return;

            Instance.pilotHps.Add(unit,hp);
        }

        public static void UnRegisterPilotHp(UnitInfo unit)
        {
            if (Instance == null || unit == null)
                return;

            Instance.pilotHps.Remove(unit);
        }

        public static void RegisterUnitHp(UnitInfo unit, UnitHp hp)
        {
            if (Instance == null || unit == null)
                return;

            Instance.UnitHPs.Add(unit, hp);
        }

        public static void UnRegisterUnitHp(UnitInfo unit)
        {
            if (Instance == null)
                return;
            Instance.UnitHPs.Remove(unit);
        }

        public static void SubscribePart(UnitInfo unit, Parts part, GameObject go)
        {
            if (Instance == null)
                return;
            if (Instance.UnitHPs.TryGetValue(unit, out var hp))
            {
                hp.Subscribe(go, part);
            }
        }

        public static void UnsubscribePart(UnitInfo unit, Parts part, GameObject go)
        {
            if (Instance == null)
                return;
            if (Instance.UnitHPs.TryGetValue(unit, out var hp))
            {
                hp.UnSubscribe(go, part);
            }
        }

        public static void SubscribeUnitHp(UnitInfo unit, GameObject go)
        {
            if (Instance == null)
                return;
            if (Instance.UnitHPs.TryGetValue(unit, out var hp))
            {
                hp.Subscribe(go);
            }
        }

        public static void UnsubscribeUnitHp(UnitInfo unit, GameObject go)
        {
            if (Instance == null)
                return;
            if (Instance.UnitHPs.TryGetValue(unit, out var hp))
            {
                hp.UnSubscribe(go);
            }
        }

        public static void UnsubscribeVisionChange(UnitInfo selectedUnit, GameObject o)
        {
            if (TacticalController.Instance?.Vision != null)
                TacticalController.Instance.Vision.UnSubscribe(selectedUnit, o);
        }

        public static void SubscribeVisionChange(UnitInfo selectedUnit, GameObject o)
        {
            if (TacticalController.Instance?.Vision != null)
                TacticalController.Instance.Vision.Subscribe(selectedUnit, o);
        }
    }
}
