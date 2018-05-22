using System;
using UnityEngine;

namespace Tools
{
    public class SceneSingleton<T> : MonoBehaviour
        where T : SceneSingleton<T>
    {
        private static T instance;

        protected bool destroing = false;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        return null;
                    }

                    if (instance.destroing)
                        return null;
                }

                return instance;
            }
        }

        private void OnDestroy()
        {
            destroing = true;
            instance = null;
        }
    }
}