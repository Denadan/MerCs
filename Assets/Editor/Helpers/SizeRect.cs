using UnityEngine;


namespace Mercs.Editor
{
    public class SizeRect
    {
        private float w;
        private float x, y, h;

        public SizeRect(Rect source, float size)
        {
            x = source.x;
            y = source.y;
            h = source.height;
            w = source.width / size;

        }

        public Rect Next(float k)
        {
            var res = new Rect(x, y, w * k, h);
            x += w * k;
            return res;
        }
    }
}
