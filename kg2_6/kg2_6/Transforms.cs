using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kg2_6
{
    public static class Transforms
    {
        public static Matrix Nothing()
        {
            Matrix m = new Matrix(4, 4);
            for (int i = 0; i < 4; i++) m[i, i] = 1;
            return m;
        }
        public static Matrix Move(float x, float y, float z)
        {
            Matrix m = new Matrix(4, 4);
            for (int i = 0; i < 4; i++) m[i, i] = 1;
            m[3, 0] = x;
            m[3, 1] = y;
            m[3, 2] = z;
            return m;
        }
        public static Matrix Scale(float x, float y, float z)
        {
            Matrix m = new Matrix(4, 4);
            m[0, 0] = x;
            m[1, 1] = y;
            m[2, 2] = z;
            m[3, 3] = 1;
            return m;
        }
        public static Matrix RotX(float angle)
        {
            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);

            Matrix m = new Matrix(4, 4);
            m[1, 1] = c;
            m[1, 2] = s;
            m[2, 1] = -s;
            m[2, 2] = c;
            m[0, 0] = m[3, 3] = 1;
            return m;
        }
        public static Matrix RotY(float angle)
        {
            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);

            Matrix m = new Matrix(4, 4);
            m[0, 0] = c;
            m[0, 2] = -s;
            m[2, 0] = s;
            m[2, 2] = c;
            m[1, 1] = m[3, 3] = 1;
            return m;
        }
        public static Matrix RotZ(float angle)
        {
            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);

            Matrix m = new Matrix(4, 4);
            m[0, 0] = c;
            m[0, 1] = s;
            m[1, 0] = -s;
            m[1, 1] = c;
            m[2, 2] = m[3, 3] = 1;
            return m;
        }
    }
}
