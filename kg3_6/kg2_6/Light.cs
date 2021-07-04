using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg2_6
{
    public class Light
    {
        public Matrix origin;
        public Matrix props;

        public Light(float x, float y, float z, Matrix baseColor)
        {
            origin = new Matrix(x, y, z);

            props = new Matrix(3, 3);
            for (int i = 0; i < 3; i++)
            {
                props[0, i] = baseColor[i] / 2;
                props[1, i] = baseColor[i];
                props[2, i] = 1;
            }
        }

        private Matrix GetColor(Matrix pos, Matrix N, Material mat)
        {
            Matrix L = origin - pos;
            L /= (float)Math.Sqrt(Matrix.Dot(L, L));

            Matrix V = new Matrix(0, 0, -1);

            float LN = Matrix.Dot(L, N);

            Matrix R = L - 2 * N * LN;
            R /= (float)Math.Sqrt(Matrix.Dot(R, R));

            float RV = Matrix.Dot(R, V);

            if (LN < 0) LN = RV = 0;
            else if (RV < 0) RV = 0;

            return new Matrix(1, LN, (float)Math.Pow(RV, mat.power)) *
                Matrix.ElementMult(mat.k, props);
        }

        //calculate colors of vertices
        public void Colorize(Mesh cone, Matrix transform, bool guro)
        {
            //zero out all normals
            if (guro)
            {
                foreach (Vertex v in cone.vertices)
                    v.normal = new Matrix(0, 0, 0);
            }

            //calculate colors in the middles of faces
            foreach (Face f in cone.faces)
            {
                f.CalculateNormal(cone.vertices, transform);

                //calculate normals for vertices
                if (guro)
                {
                    for (int i = 0; i < f.Length; i++)
                        cone.vertices[f[i]].normal += f.normal;
                }
                else
                //get median point
                {
                    Matrix med = new Matrix(0, 0, 0);
                    for (int i = 0; i < f.Length; i++) med += cone.vertices[f[i]].pos;
                    med *= transform;
                    med /= f.Length;

                    f.color = GetColor(med, f.normal, cone.material);
                }
            }

            //calculate colors for vertices
            if (guro)
                foreach (Vertex v in cone.vertices)
                {
                    v.normal /= (float)Math.Sqrt(Matrix.Dot(v.normal, v.normal));
                    v.color = GetColor(v.pos * transform, v.normal, cone.material);
                }
        }
    }
}
