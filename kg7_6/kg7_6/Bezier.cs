using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg7_6
{
    public class Bezier
    {
        public static Point Get(Point[] P, float t)
        {
            float x = 0.0f, y = 0.0f;

            for (int i = 0; i < P.Length; i++)
            {
                float k = S(t, i, P.Length - 1);
                x += P[i].X * k;
                y += P[i].Y * k;
            }
            return new Point((int)Math.Floor(x), (int)Math.Floor(y));
        }

        private static float S(float t, int k, int n) =>
            C(k, n) * (float)Math.Pow(t, k) * (float)Math.Pow(1 - t, n - k);

        private static float C(int k, int n)
        {
            int a = 1, b = 1;
            for (int i = n - k + 1; i <= n; i++) a *= i;
            for (int i = 2; i <= k; i++) b *= i;
            return (float)a / b;
        }
    }
}
