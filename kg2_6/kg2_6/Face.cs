using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg2_6
{
    public class Face
    {
        private int[] indices;
        public Matrix normal;

        public Face(int[] indx)
        {
            indices = new int[indx.Length];
            for (int i = 0; i < indx.Length; i++) indices[i] = indx[i];
        }

        public void CalculateNormal(Vertex[] vertices, Matrix m)
        {
            Matrix a = vertices[indices[1]].pos * m - vertices[indices[0]].pos * m;
            Matrix b = vertices[indices[2]].pos * m - vertices[indices[1]].pos * m;

            normal = Matrix.Cross(a, b);
            normal /= (float)Math.Sqrt(Matrix.Dot(normal, normal));
        }

        //operators
        public int this[int i]
        {
            get { return indices[i]; }
            set { indices[i] = value; }
        }
        public int Length { get { return indices.Length; } }
    }
}
