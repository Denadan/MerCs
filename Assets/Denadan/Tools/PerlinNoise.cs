using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tools
{
    public class PerlinNoise : Noise
    {
        private Vector2[][,] vectors;

        public PerlinNoise(int Size, int Seed = 0)
        {
            if (Size < 0)
                throw new ArgumentOutOfRangeException("Size must be 1 or more");

            var state = Random.state;
            Random.InitState(Seed == 0 ? (int)DateTime.Now.Ticks : Seed);
            vectors = new Vector2[Size][,];
            int s = 2;
            for (int o = 0; o < Size; o++)
            {
                vectors[o] = new Vector2[s, s];
                for (int i = 0; i < s; i++)
                for (int j = 0; j < s; j++)
                    vectors[o][i, j] = RND.Vector2RND();
                s = s * 2 - 1;

            }
            Random.state = state;
        }

        float QunticCurve(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private float Noise(float x, float y, int level)
        {
            int s = vectors[level].GetLength(0);

            float fx = x * (s - 1);
            float fy = y * (s - 1);
            int nx = (int)fx;
            int ny = (int)fy;

            //          Debug.Log($"{x:F2} {y:F2} {s} {nx} {ny}");
            float fx1 = QunticCurve(fx - nx);
            float fy1 = QunticCurve(fy - ny);

            var v_top_left = vectors[level][nx, ny];
            var v_top_right = vectors[level][nx + 1, ny];
            var v_bot_left = vectors[level][nx, ny + 1];
            var v_bot_right = vectors[level][nx + 1, ny + 1];

            var d_top_left = new Vector2(fx - nx, fy - ny);
            var d_top_right = new Vector2(fx - nx - 1, fy - ny);
            var d_bot_left = new Vector2(fx - nx, fy - ny - 1);
            var d_bot_right = new Vector2(fx - nx - 1, fy - ny - 1);

            float top_left = Vector2.Dot(v_top_left, d_top_left);
            float top_right = Vector2.Dot(v_top_right, d_top_right);
            float bot_left = Vector2.Dot(v_bot_left, d_bot_left);
            float bot_right = Vector2.Dot(v_bot_right, d_bot_right);

            float top = Mathf.Lerp(top_left, top_right, fx1);
            float bottom = Mathf.Lerp(bot_left, bot_right, fx1);

            return Mathf.Lerp(top, bottom, fy1);
        }


        public override float this[float x, float y]
        {
            get
            {
                if (x < 0 || x >= 1 || y < 0 || y >= 1)
                    return 0f;

                float weight = 1;
                float max = 0;
                float result = 0;

                for (int i = 0; i < vectors.Length; i++)
                {
                    result += Noise(x, y, i) * weight;
                    max += weight;
                    weight *= 0.8f;
                }

                return result / max;
            }
        }
    }
}