using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kg7_6
{
    public partial class Form1 : Form
    {
        Point[] points;
        int r = 5, n = 5;

        int mx, my, select = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            points = new Point[n];

            for (int i = 0; i < points.Length; i++)
                points[i] = new Point(30 + 150 * i, 120 + 200 * (i % 2));
        }

        private void Form1_Resize(object sender, EventArgs e) => Refresh();
        private void trackBar1_Scroll(object sender, EventArgs e) => Refresh();
        private void numericUpDown1_ValueChanged(object sender, EventArgs e) => Refresh();

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (Point p in points)
                e.Graphics.DrawEllipse(Pens.Black, p.X - r, p.Y - r, 2 * r, 2 * r);

            for (int i = 0; i < points.Length - 1; i++)
                e.Graphics.DrawLine(Pens.Gray, points[i], points[i + 1]);

            //draw curve
            int n = (int)numericUpDown1.Value;
            for (int i = 0; i < n; i++)
            {
                float t = (float)i / n;
                float tnext = (float)(i + 1) / n;

                e.Graphics.DrawLine(Pens.Red, Bezier.Get(points, t), Bezier.Get(points, tnext));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < points.Length; i++)
                if ((points[i].X - e.X) * (points[i].X - e.X) +
                    (points[i].Y - e.Y) * (points[i].Y - e.Y) < r * r * 2)
                {
                    select = i;
                    mx = e.X;
                    my = e.Y;
                    break;
                }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (select != -1)
            {
                points[select].X += e.X - mx;
                points[select].Y += e.Y - my;
                mx = e.X;
                my = e.Y;
                Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            select = -1;
        }
    }
}
