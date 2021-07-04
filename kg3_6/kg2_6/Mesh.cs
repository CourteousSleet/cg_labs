using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg2_6
{
    public class Mesh
    {
        public Vertex[] vertices;
        public Face[] faces;
        public Material material;

        private float a, b, height;
        private float lerp(float a, float b, float t) => a + (b - a) * t;

        public Mesh(float a_, float b_, float h, int dr, int dh)
        {
            Build(a_, b_, h, dr, dh);
            
            //colors
            material = new Material(new Matrix(0.5f, 0.5f, 0.5f), 20.0f);
        }
        public void Build(float a_, float b_, float h, int dr, int dh)
        {
            a = a_;
            b = b_;
            height = h;

            vertices = new Vertex[2 + dr * dh];

            vertices[0] = new Vertex(0, height / 2, 0);
            vertices[1] = new Vertex(0, -height / 2, 0);

            for (int i = 0; i < dr; i++)
            {
                float phi = i * (float)Math.PI * 2 / dr;

                for (int j = 0; j < dh; j++)
                {
                    float t = (float)j / dh;
                    float y = lerp(height / 2, -height / 2, t);
                    float r = (float)Math.Sqrt(1 - t);

                    vertices[2 + i + j * dr] = new Vertex(a * r * (float)Math.Cos(phi), y, b * r * (float)Math.Sin(phi));
                }
            }

            faces = new Face[dr * (dh + 1)];

            //top and bottom
            for (int i = 0; i < dr; i++)
            {
                int inext = (i + 1) % dr;
                faces[i] = new Face(new int[] { 0, inext + 2, i + 2 });
                faces[i + dr] = new Face(new int[] { 1, 1 + dr * dh - inext, 1 + dr * dh - i });
            }

            //walls
            for (int i = 0; i < dr; i++)
            {
                int inext = (i + 1) % dr, o = 2;

                for (int j = 0; j < dh - 1; j++)
                {
                    faces[2 * dr + i + j * dr] =
                        new Face(new int[] { i + j * dr + o, inext + j * dr + o,
                            inext + j * dr + dr + o, i + j * dr + dr + o });
                }
            }
        }

        public void Draw(Graphics gfx, Matrix m, bool guro)
        {
            foreach (Face f in faces)
            {
                f.CalculateNormal(vertices, m);
                if (f.normal[2] > 0) continue; //invisible face

                Point[] polygon = new Point[f.Length];
                for (int i = 0; i < f.Length; i++)
                {
                    Vertex v = vertices[f[i]];
                    polygon[i] = v.pos * m;
                }

                if (guro)
                {
                    try
                    {
                        PathGradientBrush pfill = new PathGradientBrush(polygon);

                        Color[] clrs = new Color[polygon.Length];
                        int mR = 0, mG = 0, mB = 0;

                        for (int i = 0; i < clrs.Length; i++)
                        {
                            clrs[i] = vertices[f[i]].color;
                            mR += clrs[i].R;
                            mG += clrs[i].G;
                            mB += clrs[i].B;
                        }

                        pfill.SurroundColors = clrs;
                        pfill.CenterColor = Color.FromArgb(Math.Min(255, mR / clrs.Length),
                            Math.Min(255, mG / clrs.Length), Math.Min(255, mB / clrs.Length));

                        gfx.FillPolygon(pfill, polygon);
                    }
                    catch
                    {

                    }
                }
                else
                    gfx.FillPolygon(new SolidBrush(f.color), polygon);
            }
        }
    }
}
