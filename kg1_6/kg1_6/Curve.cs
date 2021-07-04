using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg1_6
{
    public static class Curve
    {
        public static float[,] Generate(float a, int steps)
        {
            float[,] points = new float[steps, 2];
            float phi = 0, dphi = 2 * (float)Math.PI / (steps - 1), r;

            for (int i = 0; i < steps; i++)
            {
                r = a - a * (float)Math.Cos(phi);

                points[i, 0] = r * (float)Math.Cos(phi);
                points[i, 1] = r * (float)Math.Sin(phi);

                phi += dphi;
            }
            return points;
        }
    }
}
