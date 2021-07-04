using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace kg2_6
{
    //matrix mxn
    public class Matrix
    {
        private float[,] values;
        public int M { get; private set; }
        public int N { get; private set; }

        public Matrix(int heigth, int width)
        {
            M = heigth;
            N = width;

            values = new float[M, N];

            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    values[i, j] = 0;
        }
        public Matrix(float x, float y, float z)
        {
            M = 1;
            N = 3;

            values = new float[M, N];

            values[0, 0] = x;
            values[0, 1] = y;
            values[0, 2] = z;
        }

        //operators
        public float this[int i, int j]
        {
            get { return values[i, j]; }
            set { values[i, j] = value; }
        }
        public float this[int i]
        {
            get { return values[0, i]; }
            set { values[0, i] = value; }
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.M != b.M || a.N != b.N) return null;

            Matrix res = new Matrix(a.M, a.N);

            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    res[i, j] = a[i, j] + b[i, j];

            return res;
        }
        public static Matrix operator -(Matrix a, Matrix b) => a + (-1) * b;

        public static Matrix operator *(float k, Matrix a)
        {
            Matrix res = new Matrix(a.M, a.N);

            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    res[i, j] = k * a[i, j];

            return res;
        }
        public static Matrix operator *(Matrix a, float k) => k * a;
        public static Matrix operator /(Matrix a, float k) => (1 / k) * a;

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.N != b.M)
            {
                //special case, add 1 to the end
                if (a.N + 1 != b.M) return null;

                Matrix a1 = new Matrix(a.M, a.N + 1);
                for (int i = 0; i < a.M; i++)
                {
                    for (int j = 0; j < a.N; j++) a1[i, j] = a[i, j];
                    a1[i, a.N] = 1;
                }

                Matrix temp = a1 * b;

                Matrix res1 = new Matrix(temp.M, temp.N - 1);
                for (int i = 0; i < res1.M; i++)
                    for (int j = 0; j < res1.N; j++)
                        res1[i, j] = temp[i, j] / temp[i, temp.N - 1];

                return res1;
            }

            //regular multiplication
            Matrix res = new Matrix(a.M, b.N);

            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < b.N; j++)
                    for (int k = 0; k < a.N; k++)
                        res[i, j] += a[i, k] * b[k, j];

            return res;
        }

        //additional functions
        public static float Dot(Matrix a, Matrix b)
        {
            float dot = 0;

            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    dot += a[i, j] * b[i, j];

            return dot;
        }
        public static Matrix Cross(Matrix a, Matrix b) =>
            new Matrix(a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0]);

        public static Matrix ElementMult(Matrix a, Matrix b)
        {
            if (a.M != b.M || a.N != b.N) return null;

            Matrix res = new Matrix(a.M, a.N);

            for (int i = 0; i < a.M; i++)
                for (int j = 0; j < a.N; j++)
                    res[i, j] = a[i, j] * b[i, j];

            return res;
        }

        //convertions
        public static implicit operator Point(Matrix m) =>
            new Point((int)Math.Floor(m[0]), (int)Math.Floor(m[1]));

        private static int toclr(float v) =>
            Math.Min(Math.Max(0, (int)Math.Floor(255 * v)), 255);

        public static implicit operator Color(Matrix m) =>
            Color.FromArgb(toclr(m[0]), toclr(m[1]), toclr(m[2]));
    }
}
