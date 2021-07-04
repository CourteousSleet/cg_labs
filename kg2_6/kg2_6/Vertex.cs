using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg2_6
{
    public class Vertex
    {
        public Matrix pos;

        public Vertex(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            pos = new Matrix(x, y, z);
        }
    }
}
