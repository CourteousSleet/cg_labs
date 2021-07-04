using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kg1_6
{
    public partial class Form1 : Form
    {
        float wheel = 1.0f, wheelChange = 1.1f, angle = 0.0f;
        int offX = 0, offY = 0, mx, my;
        bool mouse1 = false, mouse2 = false;

        float[,] points;
        Font font = new Font("Arial", 12);

        public Form1()
        {
            InitializeComponent();
        }

        private void Recalculate()
        {
            points = Curve.Generate((float)na.Value, (int)nn.Value);
            Refresh();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            Recalculate();
        }

        //scale
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) wheel *= wheelChange;
            else wheel /= wheelChange;
            Refresh();
        }
        private void Form1_Resize(object sender, EventArgs e) => Refresh();

        private void na_ValueChanged(object sender, EventArgs e) => Recalculate();
        private void nn_ValueChanged(object sender, EventArgs e) => Recalculate();

        //draw grid lines
        private void DrawLine(Graphics gfx, Pen pen, float startX, float startY, float dirX, float dirY, float w, float h)
        {
            //vector looks right
            if (dirX < 0)
            {
                dirX = -dirX;
                dirY = -dirY;
            }

            //calculate t for collision p = p0 + dir * t with borders
            float[] tIntersects = new float[4]
            { 
                -startY / dirY,
                -startX / dirX,
                (w - startX) / dirX,
                (h - startY) / dirY
            };

            float maxVal = Math.Max(w + Math.Abs(startX), h + Math.Abs(startY));
            float tmin = maxVal, tmax = -maxVal;

            foreach (float t in tIntersects)
            {
                //clamp
                float tn = t;
                if (tn < -maxVal) tn = -maxVal;
                if (tn > maxVal) tn = maxVal;

                tmin = Math.Min(tn, tmin);
                tmax = Math.Max(tn, tmax);
            }

            //if (tmin > tmax) return; //no line

            gfx.DrawLine(pen, startX + dirX * tmin, startY + dirY * tmin,
                startX + dirX * tmax, startY + dirY * tmax);
        }

        //convert coords
        private float W2Sx(float x, float y, float dx, float dy, float scale, float c, float s) =>
            (x * c - y * s) * scale + dx;
        private float W2Sy(float x, float y, float dx, float dy, float scale, float c, float s) =>
            (-x * s - y * c) * scale + dy;

        private float S2Wx(float x, float y, float dx, float dy, float scale, float c, float s) =>
            (x - dx) / scale * c - (y - dy) / scale * s;
        private float S2Wy(float x, float y, float dx, float dy, float scale, float c, float s) =>
            -(x - dx) / scale * s - (y - dy) / scale * c;

        //draw graph
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int w = e.ClipRectangle.Width, h = e.ClipRectangle.Height;
            
            float scale = wheel * 0.25f * Math.Min(w, h);
            int dx = w / 2 + offX, dy = h / 2 + offY;

            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);

            //draw grid
            float minX = 0.0f, minY = 0.0f, maxX = 0.0f, maxY = 0.0f;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                {
                    float x = S2Wx(i * w, j * h, dx, dy, scale, c, s);
                    float y = S2Wy(i * w, j * h, dx, dy, scale, c, s);

                    if (i == 0 && j == 0)
                    {
                        minX = maxX = x;
                        minY = maxY = y;
                    }
                    else
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            int iminX = (int)Math.Truncate(minX), iminY = (int)Math.Truncate(minY),
                imaxX = (int)Math.Truncate(maxX), imaxY = (int)Math.Truncate(maxY);

            for (int i = iminX; i <= imaxX; i++)
            {
                float x = W2Sx(i, 0, dx, dy, scale, c, s), y = W2Sy(i, 0, dx, dy, scale, c, s);

                DrawLine(e.Graphics, (i == 0) ? Pens.Black : Pens.Gray, x, y, s, c, w, h);
                e.Graphics.DrawString(i.ToString(), font, Brushes.Black, x, y);
            }
            for (int j = iminY; j <= imaxY; j++)
            {
                float x = W2Sx(0, j, dx, dy, scale, c, s), y = W2Sy(0, j, dx, dy, scale, c, s);

                DrawLine(e.Graphics, (j == 0) ? Pens.Black : Pens.Gray, x, y, c, -s, w, h);
                if (j != 0) e.Graphics.DrawString(j.ToString(), font, Brushes.Black, x, y);
            }

            //calculate points
            Point[] pointsInt = new Point[points.GetLength(0)];

            for (int i = 0; i < pointsInt.Length; i++)
                pointsInt[i] = new Point(
                    (int)Math.Floor(W2Sx(points[i, 0], points[i, 1], dx, dy, scale, c, s)),
                    (int)Math.Floor(W2Sy(points[i, 0], points[i, 1], dx, dy, scale, c, s))
                );

            //draw points
            for (int i = 0; i < pointsInt.Length - 1; i++)
                e.Graphics.DrawLine(Pens.Red, pointsInt[i], pointsInt[i + 1]);
        }

        //shift
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) mouse1 = true;
            if (e.Button == MouseButtons.Right) mouse2 = true;
            mx = e.X;
            my = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse1)
            {
                offX += e.X - mx;
                offY += e.Y - my;

                Refresh();
            }
            if (mouse2)
            {
                float k = 0.01f;
                angle -= k * (e.X - mx);

                Refresh();
            }
            mx = e.X;
            my = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) mouse1 = false;
            if (e.Button == MouseButtons.Right) mouse2 = false;
        }
    }
}
