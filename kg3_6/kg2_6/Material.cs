using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg2_6
{
    public class Material
    {
        public Matrix k;
        public float power;

        public Material(Matrix baseColor, float alpha)
        {
            power = alpha;

            k = new Matrix(3, 3);
            for (int i = 0; i < 3; i++)
            {
                k[0, i] = baseColor[i] / 2;
                k[1, i] = baseColor[i];
                k[2, i] = 1;
            }
        }
    }
}
