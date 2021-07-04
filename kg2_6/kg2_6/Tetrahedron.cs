using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg2_6
{
    public class Tetrahedron
    {
        private Vertex[] vertices;
        private Face[] faces;

        public Tetrahedron()
        {
            float t = 1.0f / 3;

            vertices = new Vertex[]
            {
                new Vertex(-t, -t, -1),
                new Vertex(-t, -1, -t),
                new Vertex(-1, -t, -t),

                new Vertex(t, t, -1),
                new Vertex(1, t, -t),
                new Vertex(t, 1, -t),

                new Vertex(t, -1, t),
                new Vertex(1, -t, t),
                new Vertex(t, -t, 1),

                new Vertex(-1, t, t),
                new Vertex(-t, 1, t),
                new Vertex(-t, t, 1)
            };

            faces = new Face[]
            {
                new Face(new int[] { 0, 1, 2 }),
                new Face(new int[] { 3, 5, 4 }),
                new Face(new int[] { 6, 7, 8 }),
                new Face(new int[] { 9, 11, 10 }),

                new Face(new int[] { 0, 3, 4, 7, 6, 1 }),
                new Face(new int[] { 1, 6, 8, 11, 9, 2 }),
                new Face(new int[] { 5, 10, 11, 8, 7, 4 }),
                new Face(new int[] { 2, 9, 10, 5, 3, 0 })
            };
        }

        public void Draw(Graphics gfx, Matrix m)
        {
            foreach (Face f in faces)
            {
                f.CalculateNormal(vertices, m);
                if (f.normal[2] < 0) continue; //invisible face

                Point[] polygon = new Point[f.Length];
                for (int i = 0; i < f.Length; i++)
                {
                    polygon[i] = vertices[f[i]].pos * m;
                }

                gfx.FillPolygon(Brushes.Gray, polygon);
                gfx.DrawPolygon(Pens.Black, polygon);
            }
        }
    }
}
