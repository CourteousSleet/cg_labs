using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kg2_6
{
    public partial class Form1 : Form
    {
        Tetrahedron fig;

        bool mouse = false;
        int mx = 0, my = 0;

        float wheel = 1.0f, wheelChange = 1.1f;
        float angleX, angleY;

        Matrix[] projections;
        int proj_id;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.MouseWheel += PictureBox1_MouseWheel;
        }
        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) wheel *= wheelChange;
            else wheel /= wheelChange;
            Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fig = new Tetrahedron();

            projections = new Matrix[4];

            projections[0] = new Matrix(4, 4);
            projections[0][1, 0] = projections[0][2, 1] = 1;

            projections[1] = new Matrix(4, 4);
            projections[1][2, 0] = projections[1][0, 1] = 1;

            projections[2] = new Matrix(4, 4);
            projections[2][0, 0] = projections[2][1, 1] = 1;

            projections[3] = new Matrix(4, 4);

            float dx = (float)Math.Sqrt(3) / 2;
            projections[3][0, 0] = -dx;
            projections[3][0, 1] = -0.5f;
            projections[3][1, 1] = 1;
            projections[3][2, 0] = dx;
            projections[3][2, 1] = -0.5f;

            projections[0][3, 3] = projections[1][3, 3] = projections[2][3, 3] = projections[3][3, 3] = 1;

            proj_id = 2;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            float w = e.ClipRectangle.Width, h = e.ClipRectangle.Height;
            float scale = 0.25f * wheel * Math.Min(w, h);

            //rotations
            Matrix model = Transforms.RotY(angleY) * Transforms.RotX(angleX);
            Matrix proj = projections[proj_id] *
                Transforms.Scale(scale, -scale, -scale) * Transforms.Move(w / 2, h / 2, 0); //to screen

            Point o = new Matrix(0, 0, 0) * proj;
            e.Graphics.DrawLine(Pens.Red, o, new Matrix(10, 0, 0) * proj);
            e.Graphics.DrawLine(Pens.Lime, o, new Matrix(0, 10, 0) * proj);
            e.Graphics.DrawLine(Pens.Blue, o, new Matrix(0, 0, 10) * proj);

            fig.Draw(e.Graphics, model * proj);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse)
            {
                float k = 0.02f;
                angleX -= k * (e.Y - my);
                angleY -= k * (e.X - mx);

                mx = e.X;
                my = e.Y;
                Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e) { proj_id = 0; Refresh(); }
        private void button2_Click(object sender, EventArgs e) { proj_id = 1; Refresh(); }
        private void button3_Click(object sender, EventArgs e) { proj_id = 2; Refresh(); }
        private void button4_Click(object sender, EventArgs e) { proj_id = 3; Refresh(); }

        private void Form1_Resize(object sender, EventArgs e) => Refresh();

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = true;
            mx = e.X;
            my = e.Y;
            Refresh();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) => mouse = false;
    }
}
