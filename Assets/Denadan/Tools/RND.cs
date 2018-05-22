using UnityEngine;

namespace Tools
{
    public static class RND
    {
        public static T Rnd<T>(this T[] array, T def_value = default(T))
        {
            if (array == null || array.Length == 0)
                return def_value;
            return array[Random.Range(0, array.Length)];
        }

        public static Vector2 Vector2RND()
        {
            var angle = Random.Range(0, Mathf.PI * 2);
            return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)).normalized;
        }
        public static Vector2Int Vector2IntRND(int maxx, int maxy)
        {
            return new Vector2Int(Random.Range(0, maxx), Random.Range(0, maxy));
        }

    }
}