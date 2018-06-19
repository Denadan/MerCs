using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Mercs.Tactical.Events
{
    public class EventHandler : SceneSingleton<EventHandler>
    {
        private class list_item : IEnumerable<GameObject>
        {
            private bool in_work = false;
            private List<GameObject> list = new List<GameObject>();
            private List<GameObject> add = new List<GameObject>();
            private List<GameObject> del = new List<GameObject>();
            
            public void Add(GameObject go)
            {

                //UnityEngine.Debug.Log($"ADD {go} work:{in_work}");
                
                (in_work ? add : list).Add(go);

                //UnityEngine.Debug.Log($"{add.Count} {list.Count}");
            }

            public void Del(GameObject go)
            {
                if (in_work)
                    del.Add(go);
                else
                    list.Remove(go);
            }

            public void Complete()
            {
                if (in_work)
                {
                    if (add.Count > 0)
                    {
                        list.AddRange(add);
                        add.Clear();
                    }

                    if(del.Count > 0)
                    {
                        list.RemoveAll(i => del.Contains(i));
                        del.Clear();
                    }
                    in_work = false;
                }
            }

            public IEnumerator<GameObject> GetEnumerator()
            {
                in_work = true;
                return list.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                in_work = true;
                return list.GetEnumerator();
            }
        }

        private Dictionary<Type, list_item> subscribers = new Dictionary<Type, list_item>();


        public static void Subscribe<TEvent>(GameObject go)
            where TEvent : IEventSystemHandler
        {
            //UnityEngine.Debug.Log($"Subscribe -[{go.name}]- to {typeof(TEvent).ToString()} {Instance}");

            if (Instance == null)
                return;

            if (!Instance.subscribers.TryGetValue(typeof(TEvent), out var item))
            {
                item = new list_item();
                Instance.subscribers.Add(typeof(TEvent), item);
            }
            item.Add(go);

        }

        public static void UnSubscribe<TEvent>(GameObject go)
            where TEvent : IEventSystemHandler
        {
            //UnityEngine.Debug.Log($"UnSubscribe -[{go.name}]- to {typeof(TEvent).ToString()} {Instance}");
            if (Instance == null)
                return;

            if (Instance.subscribers.TryGetValue(typeof(TEvent), out var item))
            {
                item.Del(go);
            }
        }


        public static void Raise<TEvent>(ExecuteEvents.EventFunction<TEvent> functor)
            where TEvent : IEventSystemHandler
        {
            //UnityEngine.Debug.Log($"Raise {typeof(TEvent).ToString()} {Instance}");

            if (Instance == null || !Instance.subscribers.TryGetValue(typeof(TEvent), out var item))
                return;

            foreach (var go in item)
            {
                //UnityEngine.Debug.Log($"Raise {typeof(TEvent).ToString()} for {go}");
                ExecuteEvents.Execute<TEvent>(go, null, functor);
            }
            item.Complete();
        }
    }
}
