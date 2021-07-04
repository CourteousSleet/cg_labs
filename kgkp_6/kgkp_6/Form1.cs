using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kgkp_6
{
    public partial class Form1 : Form
    {
        Surface surface;
        float angleX = 0.0f, angleY = 0.0f;

        bool mouse = false;
        int mx, my;

        float wheel = 0.25f, wheelChange = 1.1f;

        Font font = new Font("Arial", 12);

        public Form1()
        {
            InitializeComponent();

            pictureBox1.MouseWheel += (o, e) =>
            {
                if (e.Delta > 0) wheel *= wheelChange;
                else wheel /= wheelChange;
                pictureBox1.Refresh();
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            surface = new Surface();
            SetTexts();
        }

        //set texts for textboxes
        private void SetTexts()
        {
            tp1.Text = surface.bezier[0][0].ToString() + " " +
                surface.bezier[0][1].ToString() + " " + surface.bezier[0][2].ToString();

            tp2.Text = surface.bezier[1][0].ToString() + " " +
                surface.bezier[1][1].ToString() + " " + surface.bezier[1][2].ToString();

            tp3.Text = surface.bezier[2][0].ToString() + " " +
                surface.bezier[2][1].ToString() + " " + surface.bezier[2][2].ToString();

            tp4.Text = surface.bezier[3][0].ToString() + " " +
                surface.bezier[3][1].ToString() + " " + surface.bezier[3][2].ToString();

            tscale.Text = surface.cardSz.ToString();
        }

        private float[] ParseArray(int len, string input, string error)
        {
            error = "Ошибка в поле: " + error;

            string[] str = input.Split(' ');
            if (str.Length != len)
            {
                MessageBox.Show(error);
                return null;
            }

            float[] array = new float[len];
            for (int i = 0; i < array.Length; i++)
                try
                {
                    array[i] = (float)Convert.ToDouble(str[i]);
                }
                catch
                {
                    MessageBox.Show(error);
                    return null;
                }
            return array;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            float[] fp1 = ParseArray(3, tp1.Text, "P1");
            float[] fp2 = ParseArray(3, tp2.Text, "P2");
            float[] fp3 = ParseArray(3, tp3.Text, "P3");
            float[] fp4 = ParseArray(3, tp4.Text, "P4");

            float[] fsz = ParseArray(1, tscale.Text, "Размер кардиоиды");

            if (fp1 == null || fp2 == null || fp3 == null || fp4 == null || fsz == null)
                SetTexts();
            else
            {
                for (int i = 0; i < 3; i++) surface.bezier[0][i] = fp1[i];
                for (int i = 0; i < 3; i++) surface.bezier[1][i] = fp2[i];
                for (int i = 0; i < 3; i++) surface.bezier[2][i] = fp3[i];
                for (int i = 0; i < 3; i++) surface.bezier[3][i] = fp4[i];

                surface.cardSz = fsz[0];

                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            float w = e.ClipRectangle.Width, h = e.ClipRectangle.Height;
            float scale = wheel * Math.Min(w, h);

            Matrix transform = Transforms.RotY(angleX) * Transforms.RotX(angleY) *
                Transforms.Scale(scale, -scale, 1.0f) * Transforms.Move(w / 2, h / 2, 0);

            //axis
            Matrix zero = new Matrix(0, 0, 0) * transform;
            float axisSz = 5.0f;

            e.Graphics.DrawLine(Pens.Red, zero, new Matrix(axisSz, 0, 0) * transform);
            e.Graphics.DrawLine(Pens.Lime, zero, new Matrix(0, axisSz, 0) * transform);
            e.Graphics.DrawLine(Pens.Blue, zero, new Matrix(0, 0, axisSz) * transform);
            
            //surface
            int umax = 4 * (int)numu.Value, vmax = (int)numv.Value;

            //horisontal lines
            for (int j = 0; j <= vmax; j++)
            {
                float v = (float)j / vmax;
                for (int i = 0; i < umax; i++)
                {
                    float u = (float)i / umax;
                    float u1 = (float)(i + 1) / umax;

                    e.Graphics.DrawLine(Pens.Black, surface.Get(u, v) * transform,
                        surface.Get(u1, v) * transform);
                }
            }
            //vertical lines
            for (int i = 0; i <= umax; i++)
            {
                float u = (float)i / umax;
                for (int j = 0; j < vmax; j++)
                {
                    float v = (float)j / vmax;
                    float v1 = (float)(j + 1) / vmax;

                    e.Graphics.DrawLine(Pens.Black, surface.Get(u, v) * transform,
                        surface.Get(u, v1) * transform);
                }
            }

            //carcas
            for (int i = 0; i < surface.bezier.Length - 1; i++)
                e.Graphics.DrawLine(Pens.Purple, surface.bezier[i] * transform,
                    surface.bezier[i + 1] * transform);

            //points
            for (int i = 0; i < surface.bezier.Length; i++)
                e.Graphics.DrawString("P" + i.ToString(), font, Brushes.Purple, surface.bezier[i] * transform);
        }

        private void Form1_Resize(object sender, EventArgs e) => pictureBox1.Refresh();
        private void numericUpDown2_ValueChanged(object sender, EventArgs e) => pictureBox1.Refresh();
        private void checkBox1_CheckedChanged(object sender, EventArgs e) => pictureBox1.Refresh();

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse = true;
            mx = e.X;
            my = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse)
            {
                float k = 0.01f;
                angleX += k * (e.X - mx);
                angleY += k * (e.Y - my);

                mx = e.X;
                my = e.Y;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse = false;
        }
    }
}
