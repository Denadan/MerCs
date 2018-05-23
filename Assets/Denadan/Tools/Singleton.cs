using System;
using UnityEngine;

namespace Tools
{
    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                        throw new InvalidOperationException($"No ${typeof(T)} singleton type found");
                }
                return instance;
            }
        }

        protected virtual void Start()
        {
            if (instance == null)
                instance = this as T;
            if(instance != this)
                Destroy(gameObject);
            else
                DontDestroyOnLoad(gameObject);
        }
    }
}