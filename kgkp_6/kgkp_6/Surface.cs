using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kgkp_6
{
    public class Surface
    {
        public Matrix[] bezier; //points for bezier curve
        public float cardSz; //size of cardioid

        public Surface()
        {
            bezier = new Matrix[4];
            bezier[0] = new Matrix(0, -0.9f, -0.2f);
            bezier[1] = new Matrix(0, -0.4f, -0.9f);
            bezier[2] = new Matrix(0, 0.4f, 0.9f);
            bezier[3] = new Matrix(0, 0.9f, 0.2f);

            cardSz = 0.5f;
        }
        public Matrix Get(float u, float v)
        {
            return Cardioid(u) + Bezier(v);
        }

        private Matrix Cardioid(float t)
        {
            float c = (float)Math.Cos(2 * Math.PI * t);
            float s = (float)Math.Sin(2 * Math.PI * t);

            float r = cardSz * (1 - c);
            return new Matrix(r * c, 0, r * s);
        }
        private Matrix Bezier(float t)
        {
            return (1 - t) * (1 - t) * (1 - t) * bezier[0] + 3 * t * (1 - t) * (1 - t) * bezier[1] +
                3 * t * t * (1 - t) * bezier[2] + t * t * t * bezier[3];
        }
    }
}
