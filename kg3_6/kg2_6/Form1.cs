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
        Mesh mesh;
        Light light;

        bool mouse = false;
        int mx = 0, my = 0;

        float wheel = 1.0f, wheelChange = 1.1f;
        float angleX = 0.1f, angleY = 0.0f;

        Matrix projection;

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
            mesh = new Mesh(1.0f, 1.5f, 2, 20, 15);
            light = new Light(1, 1, 3, new Matrix(1, 1, 1));

            projection = new Matrix(4, 4);
            projection[0, 0] = projection[1, 1] = projection[3, 3] = 1;

            //fill textboxes
            tpos.Text = MatrixRow(light.origin, 0);

            tka.Text = MatrixRow(mesh.material.k, 0);
            tkd.Text = MatrixRow(mesh.material.k, 1);
            tks.Text = MatrixRow(mesh.material.k, 2);

            tia.Text = MatrixRow(light.props, 0);
            tid.Text = MatrixRow(light.props, 1);
            tis.Text = MatrixRow(light.props, 2);

            tpower.Text = mesh.material.power.ToString();
        }
        private string MatrixRow(Matrix m, int i) =>
            m[i, 0].ToString() + " " + m[i, 1].ToString() + " " + m[i, 2].ToString();

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            float w = e.ClipRectangle.Width, h = e.ClipRectangle.Height;
            float scale = 0.25f * wheel * Math.Min(w, h);

            //rotations
            Matrix model = Transforms.RotY(angleY) * Transforms.RotX(angleX);
            light.Colorize(mesh, model, cbguro.Checked);

            Matrix proj = projection *
                Transforms.Scale(scale, -scale, scale) * Transforms.Move(w / 2, h / 2, 0); //to screen

            mesh.Draw(e.Graphics, model * proj, cbguro.Checked);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse)
            {
                float k = 0.02f;
                angleX += k * (e.Y - my);
                angleY += k * (e.X - mx);

                mx = e.X;
                my = e.Y;
                Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //check inputs
            Matrix pos = new Matrix(0, 0, 0), k = new Matrix(3, 3), i = new Matrix(3, 3);
            float pow = 0;

            if (ParseRow(tpos.Text, pos, 0) &&

                ParseRow(tka.Text, k, 0) &&
                ParseRow(tkd.Text, k, 1) &&
                ParseRow(tks.Text, k, 2) &&

                ParseRow(tia.Text, i, 0) &&
                ParseRow(tid.Text, i, 1) &&
                ParseRow(tis.Text, i, 2) &&

                ParseFloat(ref pow, tpower.Text))
            {
                //set values
                light.origin = pos;
                mesh.material.k = k;
                light.props = i;
                mesh.material.power = pow;

                Refresh();
            }
            else
                MessageBox.Show("Некорректный ввод");
        }
        private bool ParseRow(string s, Matrix result, int row)
        {
            string[] split = s.Split(' ');
            if (split.Length != 3) return false;

            for (int i = 0; i < 3; i++)
                try
                {
                    result[row, i] = (float)Convert.ToDouble(split[i]);
                }
                catch
                {
                    return false;
                }

            return true;
        }
        private bool ParseFloat(ref float res, string s)
        {
            try
            {
                res = (float)Convert.ToDouble(s);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void cbguro_CheckedChanged(object sender, EventArgs e) => Refresh();

        private void RebuildMesh(object sender, EventArgs e)
        {
            mesh.Build((float)numericA.Value, (float)numericB.Value, (float)numericH.Value, (int)nr.Value, (int)nh.Value);
            Refresh();
        }

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
