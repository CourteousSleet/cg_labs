﻿using System;
using SharpGL;
using CGLabPlatform;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Collections.Generic;

// Создание и работа с классом приложения аналогична предыдущим примерам, только в 
// в данном случае наследуемся от шаблона OGLApplicationTemplate<T>, в силу чего
// для вывода графики будет использоваться элемент управления OGLDevice работающий
// через OpenGL (далее OGL). Код OGLDevice размещается в Controls\OGLDevice.cs
public abstract class CGLabDemoOGL : OGLApplicationTemplate<CGLabDemoOGL>
{
    [STAThread]
    static void Main() { RunApplication(); }

    #region Свойства

    [DisplayCheckerProperty(false, "Использовать буфер вершин")]
    public virtual bool useVBO { get; set; }

    public double Height = 100; // высота параболоида

    [DisplayNumericProperty(5, 1, "Апроксимация", 1, 15)]
    public abstract int Aprox { get; set; }

    [DisplayNumericProperty(new[] { -90d, 72d, 0d }, 1, 0, "Положение камеры (X/Y/Z)")]
    public virtual DVector3 cameraAngle
    {
        get { return Get<DVector3>(); }
        set { if (Set(value)) UpdateModelViewMatrix(); }
    }

    [DisplayNumericProperty(500d, 0.1, 2, "Удаленность камеры")]
    public virtual double cameraDistance
    {
        get { return Get<double>(); }
        set { if (Set(value)) UpdateModelViewMatrix(); }
    }

    [DisplayNumericProperty(new[] { 0d, 0d, 200d }, 1, "LightPosition")]
    public DVector3 LightPosition { get; set; }

    [DisplayNumericProperty(new[] { 1d, 1d, 1d }, 0.01, "LightColor", 0, 1)]
    public DVector3 LightColor { get; set; }

    [DisplayNumericProperty(new[] { 1d, 1d, 1d }, 0.01, "ObjectColor", 0, 1)]
    public DVector3 ObjectColor { get; set; }

    [DisplayNumericProperty(new[] { .30d, .30d, .30d }, 0.01, "Ka", 0, 1)]
    public DVector3 Ka { get; set; }

    [DisplayNumericProperty(new[] { .30d, .30d, .30d }, 0.01, "Kd", 0, 1)]
    public DVector3 Kd { get; set; }

    [DisplayNumericProperty(new[] { .30d, .30d, .30d }, 0.01, "Ks", 0, 1)]
    public DVector3 Ks { get; set; }

    [DisplayNumericProperty(32, 1, "SpecularPower", 1, 100)]
    public abstract int SpecularPower { get; set; }

    [DisplayCheckerProperty(false, "Сетка")]
    public virtual bool Grid { get; set; }

    [DisplayCheckerProperty(true, "UseShader")]
    public bool UseShader
    {
        get
        {
            return Get<bool>();
        }
        set
        {
            if (Set<bool>(value))
                RenderDevice.AddScheduleTask((gl, s) => gl.UseProgram(value ? program : 0));
        }
    }

    [DisplayNumericProperty(70d, 1d, "Высота", 0d, 200d)]
    public abstract double H { get; set; }

    [DisplayNumericProperty(new[] { 1.5d, 1d }, 0.01d, "Полуоси")]
    public DVector2 AB { get; set; }

    #endregion

    // Само создание объекта типа OpenGL осуществляется при создании устройства вывода (класс OGLDevice)
    // и доступ к нему можно получить при помощи свойства gl данного объекта (RenderDevice) или объекта
    // типа OGLDeviceUpdateArgs передаваемого в качестве параметра методу OnDeviceUpdate(). Данный метод,
    // как и сама работа с устройством OpenGL реализуются в параллельном потоке. Обращение к устройству
    // OpenGL из другого потока не допускается (создание многопоточного рендера возможно, но это достаточно
    // специфическая архитектура, например рендинг частей экрана в текустуры а потом их объединение).
    // Для большинства функций библиотеки OpenGL при отладке DEBUG конфигурации осуществляется проверка
    // ошибок выполнения и их вывод в окно вывода Microsoft Visual Studio. Поэтому при отладке и написании 
    // кода связанного с OpenGL необходимо также контролировать ошибки библиотеки OpenGL в окне вывода. 

    uint program;
    public uint[] attrib_loc = new uint[2];
    public uint[] uniform_loc = new uint[3];
    public uint[] buffers = new uint[2];
    protected override void OnMainWindowLoad(object sender, EventArgs args)
    {
        base.ValueStorage.Font = new Font("Courier New", 12f);
        base.ValueStorage.ForeColor = Color.Black;
        base.ValueStorage.RowHeight = 30;
        base.ValueStorage.BackColor = Color.FromArgb(180, 180, 180);
        base.MainWindow.BackColor = Color.FromArgb(160, 160, 160);
        base.ValueStorage.RightColWidth = 70;
        base.VSPanelWidth = 480;
        base.VSPanelLeft = true;
        base.MainWindow.Size = new Size(960, 640);

        #region Обработчики событий мыши и клавиатуры -------------------------------------------------------
        RenderDevice.MouseMoveWithLeftBtnDown += (s, e) => cameraAngle += new DVector3(-e.MovDeltaY, -e.MovDeltaX, 0);
        RenderDevice.MouseWheel += (s, e) => cameraDistance -= e.Delta / 10.0;
        #endregion


        // Как было отмеченно выше вся работа связанная с OGL должна выполнятся в одном потоке. Тут работа с OGL
        // осуществляется в отдельном потоке, а метод OnMainWindowLoad() является событием возбуждаемым потоком
        // пользовательского интерфейса (UI). Поэтой причине весь код ниже добавляется в диспетчер устройства
        // вывода (метод AddScheduleTask() объекта RenderDevice) и выполняется ассинхронно в контексте потока
        // OGL. Сам диспетчер является очередью типа FIFO (First In First Out - т.е. задания обрабатываются 
        // строго в порядке их поступления) и гарантирует, что все задания добавленные в OnMainWindowLoad()
        // будут выполнены до первого вызова метода OnDeviceUpdate() (aka OnPaint)

        #region  Инициализация OGL и параметров рендера -----------------------------------------------------
        RenderDevice.AddScheduleTask((gl, s) =>
        {
            //gl.Disable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.CullFace(OpenGL.GL_BACK); // отбросить полигоны, повёрнутые изнанкой
            gl.ClearColor(1, 1, 1, 0);
            gl.FrontFace(OpenGL.GL_CW); // обход вершин по часовой стрелки
            //gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL); // закрашиваются полигоны, повёрнутые лицевой стороной
            gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_LINE);
            gl.PolygonMode(OpenGL.GL_BACK, OpenGL.GL_FILL);
        });
        #endregion

        #region Загрузка и компиляция шейдера

        var errorhandler = new Action<string, object, object>((format, arg0, arg1) =>
        {
            string errormessage = String.Format(format, arg0, arg1);
            Trace.WriteLine(errormessage);
            throw new Exception(errormessage);
            MessageBox.Show(errormessage, "SHADER CREATION ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        });



        RenderDevice.AddScheduleTask((gl, s) =>
        {
            var parameters = new int[1];
            uint uit_ver = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
            uint uit_frag = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
            var compile_shader = new Action<uint>(shader =>
            {
                gl.CompileShader(shader);
                // Проверяем была ли компиляция выполнена успешно
                gl.GetShader(shader, OpenGL.GL_COMPILE_STATUS, parameters);
                if (parameters[0] != OpenGL.GL_TRUE)
                {
                    // В случае если компиляция не удалась пытаемся добиться от OGL
                    // что именно ему не понравилось. Для этого вначале получаем длину
                    // сообщения, выделим память под него, а затем уже запрашиваем
                    // скопировать туда само сообщение. В случае C# это выглядит
                    // немного иначе, но суть таже.
                    gl.GetShader(shader, OpenGL.GL_INFO_LOG_LENGTH, parameters);
                    StringBuilder strbuilder = new StringBuilder(parameters[0]);
                    gl.GetShaderInfoLog(shader, parameters[0], IntPtr.Zero, strbuilder);
                    errorhandler("OpenGL Error: ошибка во время компиляции {1}.\n{0}",
                    strbuilder.ToString(), shader == uit_ver ? "VERTEX_SHADER"
                    : shader == uit_frag ? "FRAGMENT_SHADER" : "??????????_SHADER");
                }
            });

            var shader_vert = HelpUtils.GetTextFileFromRes("shader.vert");
            var shader_frag = HelpUtils.GetTextFileFromRes("shader.frag");
            //return;
            gl.ShaderSource(uit_ver, shader_vert);
            gl.ShaderSource(uit_frag, shader_frag);
            compile_shader(uit_ver);
            compile_shader(uit_frag);
            program = gl.CreateProgram();
            gl.AttachShader(program, uit_ver);
            gl.AttachShader(program, uit_frag);
            //gl.LinkProgram(program);

            gl.LinkProgram(program);
            gl.GetProgram(program, OpenGL.GL_LINK_STATUS, parameters);
            if (parameters[0] != OpenGL.GL_TRUE)
                errorhandler("OpenGL Error: ошибка линковки програмы шейдера", null, null);

            attrib_loc[0] = (uint)gl.GetAttribLocation(program, "position");
            attrib_loc[1] = (uint)gl.GetAttribLocation(program, "normal");
            uniform_loc[0] = (uint)gl.GetUniformLocation(program, "projection");
            uniform_loc[1] = (uint)gl.GetUniformLocation(program, "view");
            uniform_loc[2] = (uint)gl.GetUniformLocation(program, "model");
            gl.UseProgram(program);


        });
        #endregion

        #region Инициализация буфера вершин -----------------------------------------------------------------
        RenderDevice.AddScheduleTask((gl, s) =>
        {
            gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.EnableClientState(OpenGL.GL_NORMAL_ARRAY);
            gl.EnableClientState(OpenGL.GL_INDEX_ARRAY);
            gl.EnableClientState(OpenGL.GL_COLOR_ARRAY);

            gl.GenBuffers(buffers.Length, buffers);
        }, this);
        #endregion

        #region Уничтожение буфера вершин по завершению работы OGL ------------------------------------------
        RenderDevice.Closed += (s, e) => // Событие выполняется в контексте потока OGL при завершении работы
        {
            var gl = e.gl;
            gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
            gl.DisableClientState(OpenGL.GL_NORMAL_ARRAY);
            gl.DisableClientState(OpenGL.GL_INDEX_ARRAY);
            gl.DisableClientState(OpenGL.GL_COLOR_ARRAY);
        };
        #endregion

        #region Обновление матрицы проекции при изменении размеров окна и запуске приложения ----------------
        RenderDevice.Resized += (s, e) =>
        {
            var gl = e.gl;
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            pMatrix = Perspective(60, (double)e.Width / e.Height, 0.1, 100);

            gl.LoadMatrix(pMatrix.ToArray(true));
        };
        #endregion
    }
    public DMatrix4 mMatrix;
    public DMatrix4 vMatrix;
    public DMatrix4 pMatrix;
    private void UpdateModelViewMatrix() // метод вызывается при измении свойств cameraAngle и cameraDistance
    {
        #region Обновление объектно-видовой матрицы ---------------------------------------------------------
        RenderDevice.AddScheduleTask((gl, s) =>
        {
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
            var deg2rad = Math.PI / 180; // Вращается камера, а не сам объект
            double phi = deg2rad * cameraAngle.X;
            double teta = deg2rad * cameraAngle.Y;
            double psi = deg2rad * cameraAngle.Z;
            // матрицы поворота вокруг осей
            DMatrix3 RX = new DMatrix3(1, 0, 0,
                                       0, Math.Cos(phi), -Math.Sin(phi),
                                       0, Math.Sin(phi), Math.Cos(phi));

            DMatrix3 RY = new DMatrix3(Math.Cos(teta), 0, Math.Sin(teta),
                                       0, 1, 0,
                                       -Math.Sin(teta), 0, Math.Cos(teta));

            DMatrix3 RZ = new DMatrix3(Math.Cos(psi), -Math.Sin(psi), 0,
                                       Math.Sin(psi), Math.Cos(psi), 0,
                                       0, 0, 1);
            var cameraTransform = (RX * RY) * RZ;
            var cameraPosition = cameraTransform * new DVector3(0, 0, cameraDistance);
            var cameraUpDirection = cameraTransform * new DVector3(0, 1, 0);
            // Мировая матрица (преобразование локальной системы координат в мировую)
            mMatrix = DMatrix4.Identity; // нет никаких преобразований над объекта
            // Видовая матрица (переход из мировой системы координат к системе координат камеры)
            vMatrix = LookAt(DMatrix4.Identity, cameraPosition, DVector3.Zero, cameraUpDirection);
            // матрица ModelView
            var mvMatrix = vMatrix * mMatrix;
            gl.LoadMatrix(mvMatrix.ToArray(true));


            //gl.Rotate(45, 1f, 0f, 0);
            //gl.Rotate(-45, 0f, 1f, 0);
        });
        #endregion
    }

    private static DMatrix4 Perspective(double verticalAngle, double aspectRatio, double nearPlane, double farPlane)
    {
        var radians = (verticalAngle / 2) * Math.PI / 180;
        var sine = Math.Sin(radians);
        if (nearPlane == farPlane || aspectRatio == 0 || sine == 0)
            return DMatrix4.Zero;
        var cotan = Math.Cos(radians) / sine;
        var clip = farPlane - nearPlane;
        return new DMatrix4(
        cotan / aspectRatio, 0, 0, 0,
        0, cotan, 0, 0,
        0, 0, -(nearPlane + farPlane) / clip, -(2.0 * nearPlane * farPlane) / clip,
        0, 0, -1.0, 1.0
        );
    }

    private static DMatrix4 LookAt(DMatrix4 matrix, DVector3 eye, DVector3 center, DVector3 up)
    {
        var forward = (center - eye).Normalized();
        if (forward.ApproxEqual(DVector3.Zero, 0.00001))
            return matrix;
        var side = (forward * up).Normalized();
        var upVector = side * forward;
        var result = matrix * new DMatrix4(
        +side.X, +side.Y, +side.Z, 0,
        +upVector.X, +upVector.Y, +upVector.Z, 0,
        -forward.X, -forward.Y, -forward.Z, 0,
        0, 0, 0, 1
        );
        result.M14 -= result.M11 * eye.X + result.M12 * eye.Y + result.M13 * eye.Z;
        result.M24 -= result.M21 * eye.X + result.M22 * eye.Y + result.M23 * eye.Z;
        result.M34 -= result.M31 * eye.X + result.M32 * eye.Y + result.M33 * eye.Z;
        result.M44 -= result.M41 * eye.X + result.M42 * eye.Y + result.M43 * eye.Z;
        return result;
    }

    #region Count Figure
    public int[] numbers = new int[60];
    public void Count_Aprox()
    {
        for (int i = 1, j = 0; i < 100; i++)
        {
            if (j >= numbers.Length) break;
            if ((360 % i) == 0)
            {
                numbers[j] = i;
                j++;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex
    {
        // Координата
        public readonly float vx, vy, vz;
        // Нормаль
        public readonly float nx, ny, nz;
        // Цвет
        public readonly float r, g, b;
        public Vertex(float vx, float vy, float vz, float nx, float ny, float nz, float r, float g, float b)
        {
            this.vx = vx; this.vy = vy; this.vz = vz;
            this.nx = nx; this.ny = ny; this.nz = nz;
            this.r = r; this.g = g; this.b = b;
        }
    }

    public Vertex[] vertices; // массив вершин

    public int SIZE = 0; // размер массива вершин
    public void Count_Vertex() // получение размера массива (расчёт кол-ва вершин) и его создание
    {
        SIZE = 0;

        ///////////////////////////////////////////////////////верхняя и нижняя крышки
        for (int j = 1; j < Height; j += numbers[Aprox + 3], BottomCircles++)
        {
            CntVertInBottomCircles = 0;
            for (int phi = 0; phi <= 360; phi += numbers[Aprox + 3], CntVertInBottomCircles++)
            {
                SIZE++;
            }
        }
        SIZE *= 2;
        //////////////////////////////////////////////////боковая поверхность
        int cnt_step = (int)Height / numbers[Aprox + 3];
        double step = Height / (double)cnt_step;
        for (int j = 0; j <= cnt_step; j++)
        {
            for (int i = 0, phi = 0; i < CntVertInBottomCircles - 1; i++, phi += numbers[Aprox + 3])
            {
                SIZE++;
            }
        }
        SIZE += 2;
        vertices = new Vertex[SIZE];
    }
    public int FIRST = 0, SECOND = 0, LAST = 0;
    public int CntVertInCircle = 0;
    public int Circles = 0;
    public int CntVertInTopCircles = 0;
    public int TopCircles = 0;
    public int CntVertInBottomCircles = 0;
    public int BottomCircles = 0;
    public void Create_Vertex()
    {
        CntVertInCircle = 0;
        Circles = 0;
        CntVertInTopCircles = 0;
        TopCircles = 0;
        CntVertInBottomCircles = 0;
        BottomCircles = 0;
        double X, Y, Z;
        int iterator = 0;
        /////////////////////////////////////////////////////// верхняя крышка
        double h1 = H, r1 = 1;
        for (int j = 1; j < Height; j += numbers[Aprox + 3], TopCircles++)
        {
            CntVertInTopCircles = 0;
            for (int phi = 0; phi <= 360; phi += numbers[Aprox + 3], CntVertInTopCircles++)
            {
                if ((360 - phi) < numbers[Aprox + 3])
                {
                    X = (Height - j) * r1 * AB[0];
                    Y = 0;
                    Z = h1;
                }
                else
                {
                    X = ((Height - j)) * Math.Cos(phi * Math.PI / 180) * r1 * AB[0];
                    Y = (-1) * (Height - j) * Math.Sin(phi * Math.PI / 180) * r1 * AB[1];
                    Z = h1;
                }
                vertices[iterator] = new Vertex((float)X, (float)Y, (float)Z, 0, 0, 0, 0.0f, 1.0f, 0.0f);
                iterator++;

            }
        }
        /////////////////////////////////////////////////////// нижняя крышка
        double h2 = -H, r2 = 0;
        for (int j = 1; j < Height; j += numbers[Aprox + 3], BottomCircles++)
        {
            CntVertInBottomCircles = 0;
            for (int phi = 0; phi <= 360; phi += numbers[Aprox + 3], CntVertInBottomCircles++)
            {
                if ((360 - phi) < numbers[Aprox + 3])
                {
                    X = (Height - j) * r2 * AB[0];
                    Y = 0;
                    Z = h2;
                }
                else
                {
                    X = (Height - j) * Math.Cos(phi * Math.PI / 180) * r2 * AB[0];
                    Y = (-1) * (Height - j) * Math.Sin(phi * Math.PI / 180) * r2 * AB[1];
                    Z = h2;
                }
                vertices[iterator] = new Vertex((float)X, (float)Y, (float)Z, 0, 0, 0, 0.0f, 0.0f, 1.0f);
                iterator++;

            }
        }

        int cnt_step = (int)Height / numbers[Aprox+3];
        double step = Height / (double)cnt_step;

        //////////////////////////////////////////////////боковая поверхность

        for (int j = 0; j <= cnt_step; j++, Circles++)//счетник окружностей
        {
            CntVertInCircle = 0;
            for (int i = 0, phi = 0; i < CntVertInBottomCircles - 1; i++, phi += numbers[Aprox + 3], CntVertInCircle++)
            {


                Z = h2 + j * step * (h1 - h2) / Height;


                Vertex tmp = vertices[i];

                double tmp_ = Math.Sqrt(j * step) / Math.Sqrt(Height);

                X = tmp.vx*tmp_;
                Y = tmp.vy* tmp_;


                //

                vertices[iterator] = new Vertex((float)X, (float)Y, (float)Z, 0, 0, 0, 1.0f, 0.0f, 0.0f);
                iterator++;
            }

        }
        vertices[iterator] = new Vertex((float)0, (float)0, (float)h1, 0, 0, 0, 0.0f, 1.0f, 0.0f);
        iterator++;
        vertices[iterator] = new Vertex((float)0, (float)0, (float)h2, 0, 0, 0, 0.0f, 0.0f, 1.0f);
        iterator++;

    }

    public int Polygons = 0; // число полигонов
    public void Count_Polygons()
    {
        Polygons = 0;

        // подсчет полигонов крышек

        for (int j = 1; j < BottomCircles; j++)
        {
            for (int i = 0; i < CntVertInBottomCircles - 1; i++)
            {
                Polygons += 3;
            }
        }

        for (int j = 1; j < BottomCircles; j++)
        {
            for (int i = j * CntVertInBottomCircles; i < (j + 1) * CntVertInBottomCircles - 1; i++)
            {
                Polygons += 3;
            }
        }
        for (int i = 0; i < CntVertInBottomCircles - 1; i++)
        {
            Polygons += 3;
        }

        Polygons *= 2;
        //подсчет полигонов бокового основания
        for (int j = 1; j <= Circles; j++)
        {
            for (int i = 0; i < CntVertInCircle - 1; i++)
            {
                Polygons += 3;
            }
        }
        for (int j = 1; j <= Circles; j++)
        {
            for (int i = 0; i < CntVertInCircle - 1; i++)
            {
                Polygons += 3;
            }
        }
        for (int j = 1; j <= Circles; j++)
        {
            for (int i = CntVertInCircle - 1; i < CntVertInCircle; i++)
            {
                Polygons += 3;
            }
        }
        for (int j = 1; j <= Circles; j++)
        {
            for (int i = CntVertInCircle - 1; i < CntVertInCircle; i++)
            {
                Polygons += 3;
            }
        }
        indices = new int[Polygons];
    }

    public int[] indices; // массив индексов вершин
    public void Create_Polygons()
    {

        int iterator = 0;
        int visited = 0;

        int z = 1;

        ////////////////////////////////// разбивка на полигоны верхней крышки
        //разбивка на полигоны верхней крышки
        for (int j = 1; j < TopCircles; j++)
        {
            for (int i = 0; i < CntVertInTopCircles - 1; i++)
            {
                int a = (j - 1) * CntVertInTopCircles + i + visited;
                int b = (j - 1) * CntVertInTopCircles + i + 1 + visited;
                int c = j * CntVertInTopCircles + i + visited;
                indices[iterator] = a; iterator++;
                indices[iterator] = b; iterator++;
                indices[iterator] = c; iterator++;
                Count_Normal(a, b, c);
            }
        }
        z = 1;
        for (int j = 1; j < TopCircles; j++)
        {
            for (int i = j * CntVertInTopCircles; i < (j + 1) * CntVertInTopCircles - 1; i++)
            {
                int a = i + visited;
                int b = z + visited;
                int c = i + 1 + visited;
                indices[iterator] = a; iterator++;
                indices[iterator] = b; iterator++;
                indices[iterator] = c; iterator++;
                z++;
                Count_Normal(a, b, c);
            }
            z++;
        }
        for (int i = 0; i < CntVertInTopCircles - 1; i++)
        {
            int a = (TopCircles - 1) * CntVertInTopCircles + i + visited;
            int b = (TopCircles - 1) * CntVertInTopCircles + i + 1 + visited;
            int c = vertices.Length - 2;
            indices[iterator] = a; iterator++;
            indices[iterator] = b; iterator++;
            indices[iterator] = c; iterator++;
            Count_Normal(a, b, c);
        }


        // разбивка на полигоны нижней крышки
        visited += TopCircles * CntVertInTopCircles;
        for (int j = 1; j < BottomCircles; j++)
        {
            for (int i = 0; i < CntVertInBottomCircles - 1; i++)
            {
                int a = (j - 1) * CntVertInBottomCircles + i + visited;
                int b = j * CntVertInBottomCircles + i + visited;
                int c = (j - 1) * CntVertInBottomCircles + i + 1 + visited;
                indices[iterator] = a; iterator++;
                indices[iterator] = b; iterator++;
                indices[iterator] = c; iterator++;
                Count_Normal(a, b, c);
            }
        }
        z = 1;
        for (int j = 1; j < BottomCircles; j++)
        {
            for (int i = j * CntVertInBottomCircles; i < (j + 1) * CntVertInBottomCircles - 1; i++)
            {
                int a = i + visited;
                int b = i + 1 + visited;
                int c = z + visited;
                indices[iterator] = a; iterator++;
                indices[iterator] = b; iterator++;
                indices[iterator] = c; iterator++;
                z++;
                Count_Normal(a, b, c);
            }
            z++;
        }
        for (int i = 0; i < CntVertInBottomCircles - 1; i++)
        {
            int a = (BottomCircles - 1) * CntVertInBottomCircles + i + visited;
            int b = vertices.Length - 1;
            int c = (BottomCircles - 1) * CntVertInBottomCircles + i + 1 + visited;
            indices[iterator] = a; iterator++;
            indices[iterator] = b; iterator++;
            indices[iterator] = c; iterator++;
            Count_Normal(a, b, c);
        }
        visited += BottomCircles * CntVertInBottomCircles;

        //разбивка на полигоны бокового основания
        for (int j = 1; j < Circles; j++)//первый треугольник
        {
            for (int i = 0; i < CntVertInCircle; i++)
            {
                int a = (j - 1) * CntVertInCircle + i + visited;
                int c = j * CntVertInCircle + i + visited;
                int b = (j - 1) * CntVertInCircle + (i + 1) % CntVertInCircle + visited;
                indices[iterator] = a; iterator++;
                indices[iterator] = b; iterator++;
                indices[iterator] = c; iterator++;
                Count_Normal(a, b, c);

            }
        }
        for (int j = 1; j < Circles; j++)//второй треугольник
        {

            for (int i = 0; i < CntVertInCircle; i++)
            {
                int a = (j - 1) * CntVertInCircle + (i + 1) % CntVertInCircle + visited;
                int c = j * CntVertInCircle + i + visited;
                int b = j * CntVertInCircle + (i + 1) % CntVertInCircle + visited;
                indices[iterator] = a; iterator++;
                indices[iterator] = b; iterator++;
                indices[iterator] = c; iterator++;
                Count_Normal(a, b, c);
            }
        }
    }


    public void Count_Normal(int a, int b, int c)
    {
        DVector3 vec1 = new DVector3(vertices[a].vx, vertices[a].vy, vertices[a].vz);
        DVector3 vec2 = new DVector3(vertices[b].vx, vertices[b].vy, vertices[b].vz);
        DVector3 vec3 = new DVector3(vertices[c].vx, vertices[c].vy, vertices[c].vz);
        DVector3 normal;
        normal = (-1) * DVector3.CrossProduct(vec3 - vec2, vec1 - vec3);
        normal = normal / Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);

        //gl.PopMatrix();
        UpdateModelViewMatrix();
        //normal = (-1) * DVector3.CrossProduct(vec1 - vec3, vec2 - vec1);
        vertices[a] = new Vertex(vertices[a].vx, vertices[a].vy, vertices[a].vz, (float)normal.X, (float)normal.Y, (float)normal.Z, vertices[a].r, vertices[a].g, vertices[a].b);

        //normal = (-1) * DVector3.CrossProduct(vec2 - vec1, vec3 - vec2);
        vertices[b] = new Vertex(vertices[b].vx, vertices[b].vy, vertices[b].vz, (float)normal.X, (float)normal.Y, (float)normal.Z, vertices[b].r, vertices[b].g, vertices[b].b);

        //normal = (-1) * DVector3.CrossProduct(vec3 - vec2, vec1 - vec3);
        vertices[c] = new Vertex(vertices[c].vx, vertices[c].vy, vertices[c].vz, (float)normal.X, (float)normal.Y, (float)normal.Z, vertices[c].r, vertices[c].g, vertices[c].b);
    }

    public void Count_Side_Normal(int a, int b, int c)
    {
        DVector3 vec1 = new DVector3(vertices[a].vx, vertices[a].vy, vertices[a].vz);
        DVector3 vec2 = new DVector3(vertices[b].vx, vertices[b].vy, vertices[b].vz);
        DVector3 vec3 = new DVector3(vertices[c].vx, vertices[c].vy, vertices[c].vz);
        DVector3 normal = new DVector3(1, 0, 0);
        vertices[a] = new Vertex(vertices[a].vx, vertices[a].vy, vertices[a].vz, (float)normal.X, (float)normal.Y, (float)normal.Z, vertices[a].r, vertices[a].g, vertices[a].b);
        vertices[b] = new Vertex(vertices[b].vx, vertices[b].vy, vertices[b].vz, (float)normal.X, (float)normal.Y, (float)normal.Z, vertices[b].r, vertices[b].g, vertices[b].b);
        vertices[c] = new Vertex(vertices[c].vx, vertices[c].vy, vertices[c].vz, (float)normal.X, (float)normal.Y, (float)normal.Z, vertices[c].r, vertices[c].g, vertices[c].b);
    }

    #endregion;


    public void Draw_Axes(OGLDeviceUpdateArgs e)
    {
        var gl = e.gl;
        gl.UseProgram(0);
        #region Axes
        gl.Color(1f, 0f, 0);
        gl.Begin(OpenGL.GL_LINES);
        gl.Vertex(0, 0, 0);
        gl.Vertex(200, 0, 0);
        gl.End();

        gl.Color(0, 1f, 0);
        gl.Begin(OpenGL.GL_LINES);
        gl.Vertex(0, 0, 0);
        gl.Vertex(0, 200, 0);
        gl.End();

        gl.Color(0f, 0f, 1f);
        gl.Begin(OpenGL.GL_LINES);
        gl.Vertex(0, 0, 0);
        gl.Vertex(0, 0, 200);
        gl.End();
        gl.Color(1f, 1f, 1f);
        /*for (int i = 0; i < vertices.Count();i++ )
        {
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(0, 0, 0);
            gl.Vertex(vertices[i].nx, vertices[i].ny, vertices[i].nz);
            gl.End();
        }*/
        


        //gl.PopMatrix();
        UpdateModelViewMatrix();

        #endregion;
        if (UseShader)
            gl.UseProgram(program);

    }

    public float cur_time = 0;

    public void Shader(OGLDeviceUpdateArgs e, int offset_vx, int offset_nx, int mode)
    {
        unsafe
        {
            var gl = e.gl;
            gl.EnableVertexAttribArray(attrib_loc[0]);
            gl.EnableVertexAttribArray(attrib_loc[1]);
            fixed (Vertex* vrt = vertices)
            {
                if (mode == 0)
                {
                    gl.VertexAttribPointer(attrib_loc[0], 3, OpenGL.GL_FLOAT, false, sizeof(Vertex), IntPtr.Add((IntPtr)(vrt), offset_vx));
                    gl.VertexAttribPointer(attrib_loc[1], 3, OpenGL.GL_FLOAT, false, sizeof(Vertex), IntPtr.Add((IntPtr)(vrt), offset_nx));
                }
                else if (mode == 1)
                {
                    gl.VertexAttribPointer(attrib_loc[0], 3, OpenGL.GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset_vx));
                    gl.VertexAttribPointer(attrib_loc[1], 3, OpenGL.GL_FLOAT, false, sizeof(Vertex), new IntPtr(offset_nx));

                }
            }
            int objectColorLoc = gl.GetUniformLocation(program, "objectColor");
            int lightColorLoc = gl.GetUniformLocation(program, "lightColor");
            int lightPosLoc = gl.GetUniformLocation(program, "lightPos");
            int viewPosLoc = gl.GetUniformLocation(program, "viewPos");
            int AmbientPosLoc = gl.GetUniformLocation(program, "ambient");
            int DiffusePosLoc = gl.GetUniformLocation(program, "diffuse");
            int SpectrPosLoc = gl.GetUniformLocation(program, "spectr");
            int PowerLoc = gl.GetUniformLocation(program, "power");
            gl.Uniform3(objectColorLoc, (float)ObjectColor.X, (float)ObjectColor.Y, (float)ObjectColor.Z);
            gl.Uniform3(lightColorLoc, (float)LightColor.X, (float)LightColor.Y, (float)LightColor.Z);
            gl.Uniform3(viewPosLoc, (float)cameraAngle.X, (float)cameraAngle.Y, (float)cameraAngle.Z);
            gl.Uniform3(lightPosLoc, (float)LightPosition.X, (float)LightPosition.Y, (float)LightPosition.Z);
            //gl.Uniform3(viewPosLoc, 0, 0, (float)cameraDistance);
            ///////////
            ///////////
            ////////////
            ////////////
            gl.Uniform3(viewPosLoc, (float)LightPosition.X, (float)LightPosition.Y, (float)LightPosition.Z);
            gl.Uniform3(AmbientPosLoc, (float)Ka.X, (float)Ka.Y, (float)Ka.Z);
            gl.Uniform3(DiffusePosLoc, (float)Kd.X, (float)Kd.Y, (float)Kd.Z);
            gl.Uniform3(SpectrPosLoc, (float)Ks.X, (float)Ks.Y, (float)Ks.Z);
            gl.Uniform1(PowerLoc, SpecularPower);
            gl.UniformMatrix4((int)uniform_loc[0], 1, false, pMatrix.ToFloatArray(true));
            gl.UniformMatrix4((int)uniform_loc[1], 1, false, vMatrix.ToFloatArray(true));
            gl.UniformMatrix4((int)uniform_loc[2], 1, false, mMatrix.ToFloatArray(true));
        }
    }

    protected unsafe override void OnDeviceUpdate(object s, OGLDeviceUpdateArgs e)
    {
        var gl = e.gl;

        UpdateModelViewMatrix();
        // Очищаем буфер экрана и буфер глубины (иначе рисоваться все будет поверх старого)
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
        Count_Aprox();
        Count_Vertex();
        Create_Vertex();
        Count_Polygons();
        Create_Polygons();
        Draw_Axes(e);

        int offset_vx = (int)Marshal.OffsetOf(typeof(Vertex), "vx");
        int offset_nx = (int)Marshal.OffsetOf(typeof(Vertex), "nx");
        int offset_col = (int)Marshal.OffsetOf(typeof(Vertex), "r");
        //gl.Rotate(1, 1, 0, 0);
        gl.Enable(OpenGL.GL_NORMALIZE);
        // Рендинг сцены реализуется одним из двух методов - VB (Vertex Buffer) или VA (Vertex Array), 
        // в зависимости от выбранного пользователем режима.
        if (!useVBO)
        #region Рендинг сцены методом VA (Vertex Array) -----------------------------------------------------
        {
            fixed (Vertex* vrt = vertices)
            {
                gl.VertexPointer(3, OpenGL.GL_FLOAT, sizeof(Vertex), IntPtr.Add((IntPtr)vrt, offset_vx));
                gl.NormalPointer(OpenGL.GL_FLOAT, sizeof(Vertex), IntPtr.Add((IntPtr)vrt, offset_nx));
                gl.ColorPointer(3, OpenGL.GL_FLOAT, sizeof(Vertex), IntPtr.Add((IntPtr)vrt, offset_col));
            }

            fixed (int* ptr = indices)
            {
                gl.IndexPointer(OpenGL.GL_INT, 0, (IntPtr)(&ptr[0]));
            }

            if (UseShader)
            {
                Shader(e, offset_vx, offset_nx, 0);
            }
            fixed (int* ptr = indices)
            {
                gl.DrawElements(OpenGL.GL_TRIANGLES, indices.Length, (IntPtr)(&ptr[0]));
            }
            gl.DisableVertexAttribArray(attrib_loc[0]);
            gl.DisableVertexAttribArray(attrib_loc[1]);

            if (Grid)
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            else
                gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);

        }
        #endregion
        else
        #region Рендинг сцены методом VBO (Vertex Buffer Object) --------------------------------------------
        {
            fixed (uint* ptr = buffers)
            {
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, ptr[0]);
                gl.BindBuffer(OpenGL.GL_ELEMENT_ARRAY_BUFFER, ptr[1]);
            }

            /////////////////////
            fixed (Vertex* vrt = vertices)
            {
                gl.VertexPointer(3, OpenGL.GL_FLOAT, sizeof(Vertex), new IntPtr(offset_vx));
                gl.NormalPointer(OpenGL.GL_FLOAT, sizeof(Vertex), new IntPtr(offset_nx));
                gl.ColorPointer(3, OpenGL.GL_FLOAT, sizeof(Vertex), new IntPtr(offset_col));
            }
            fixed (Vertex* ptr = vertices)
            {
                gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertices.Length * sizeof(Vertex), IntPtr.Add((IntPtr)ptr, offset_vx), OpenGL.GL_STATIC_DRAW);
            }
            fixed (int* ptr = indices)
            {
                gl.BufferData(OpenGL.GL_ELEMENT_ARRAY_BUFFER, indices.Length * sizeof(int), (IntPtr)(&ptr[0]), OpenGL.GL_STATIC_DRAW);
            }

            if (UseShader)
            {
                Shader(e, offset_vx, offset_nx, 1);
            }

            fixed (int* ptr = indices)
            {
                gl.DrawElements(OpenGL.GL_TRIANGLES, indices.Length, OpenGL.GL_UNSIGNED_INT, new IntPtr(0 * sizeof(uint)));
            }

            gl.DeleteBuffers(2, buffers);
            gl.DisableVertexAttribArray(attrib_loc[0]);
            gl.DisableVertexAttribArray(attrib_loc[1]);


            if (Grid)
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
            else
                gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
        }
        #endregion
    }
}