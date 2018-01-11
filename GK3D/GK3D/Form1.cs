using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using System.Timers;
using System.Reflection;

namespace GK3D
{
    public partial class Form1 : Form
    {
        float[,] zindex;
        static double t = 0;
        List<Object3D> objects = new List<Object3D>();
        public static readonly object syncLock = new object();

        public Object3D[] LoadJSONFile(string fileName)
        {
            var meshes = new List<Object3D>();
            var file = System.IO.File.ReadAllText(fileName);
            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(file);

            for (var meshIndex = 0; meshIndex < jsonObject.meshes.Count; meshIndex++)
            {
                var verticesArray = jsonObject.meshes[meshIndex].vertices;
                // Faces
                var indicesArray = jsonObject.meshes[meshIndex].indices;

                var uvCount = jsonObject.meshes[meshIndex].uvCount.Value;
                var verticesStep = 1;

                // Depending of the number of texture's coordinates per vertex
                // we're jumping in the vertices array  by 6, 8 & 10 windows frame
                switch ((int)uvCount)
                {
                    case 0:
                        verticesStep = 6;
                        break;
                    case 1:
                        verticesStep = 8;
                        break;
                    case 2:
                        verticesStep = 10;
                        break;
                }

                // the number of interesting vertices information for us
                var verticesCount = verticesArray.Count / verticesStep;
                // number of faces is logically the size of the array divided by 3 (A, B, C)
                var facesCount = indicesArray.Count / 3;
                var mesh = new Object3D();

                // Filling the Vertices array of our mesh first
                for (var index = 0; index < verticesCount; index++)
                {
                    var x = (float)verticesArray[index * verticesStep].Value;
                    var y = (float)verticesArray[index * verticesStep + 1].Value;
                    var z = (float)verticesArray[index * verticesStep + 2].Value;
                    mesh.addPoint(new Vector4(x, y, z, 1));
                }

                // Then filling the Faces array
                for (var index = 0; index < facesCount; index++)
                {
                    var a = (int)indicesArray[index * 3].Value;
                    var b = (int)indicesArray[index * 3 + 1].Value;
                    var c = (int)indicesArray[index * 3 + 2].Value;
                    mesh.addTriangle(new Triangle{ p1 = a, p2 = b, p3 = c });
                }

                // Getting the position you've set in Blender
                var position = jsonObject.meshes[meshIndex].position;
                //mesh.Position = new Vector3((float)position[0].Value, (float)position[1].Value, (float)position[2].Value);
                meshes.Add(mesh);
            }
            return meshes.ToArray();
        }
        Bitmap tmpBtmp;
        LockBitmap lbm;
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            int i = 0;
            zindex = new float[pictureBox1.Width, pictureBox1.Height];
           
            tmpBtmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics gfx = Graphics.FromImage(tmpBtmp))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                gfx.FillRectangle(brush, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }
            lbm = new LockBitmap(tmpBtmp);
            for (int k = 0; k < pictureBox1.Width; k++)
            {
                for (int j = 0; j < pictureBox1.Height; j++)
                    zindex[k, j] = float.MaxValue;
            }
            lbm.LockBits();
            lock (syncLock)
            {
                if (radioButton2.Checked == true)
                {
                    var followed = Graphic3D.CreatePoint3D(objects[1].pointList[5], objects[1].MModel);
                    Graphic3D.Mview = Graphic3D.CreateViewAt(3.0, 0.5, 0.5, followed.X, followed.Y, followed.Z, 0, 0, 1);
                }
                if (radioButton3.Checked == true)
                {
                    var followed1 = Graphic3D.CreatePoint3D(objects[1].pointList[5], objects[1].MModel);
                    var followed2 = Graphic3D.CreatePoint3D(objects[0].pointList[5], objects[1].MModel);
                    Graphic3D.Mview = Graphic3D.CreateViewAt(followed1.X+2, followed1.Y + 2, followed1.Z + 2, followed1.X, followed1.Y, followed1.Z, 0, 0, 1);
                }
                int k = 0;
                foreach (Object3D c in objects)
                {
                    //if (k++ == 0) continue;
                    foreach (Triangle poly in c.triangleList)
                    {
                        var color = 0.25f + (i % c.triangleList.Count) * 0.75f / c.triangleList.Count;

                        var p1 = c.pointList[poly.p1];
                        var p2 = c.pointList[poly.p2];
                        var p3 = c.pointList[poly.p3];

                        var pixelA = Graphic3D.CreatePoint(p1, c.MModel);
                        var pixelB = Graphic3D.CreatePoint(p2, c.MModel);
                        var pixelC = Graphic3D.CreatePoint(p3, c.MModel);

                        DrawTriangle(pixelA, pixelB, pixelC, new Vector4(color * 255, color * 255, color * 255, 1));
                        i++;
                    }
                }
            }
            lbm.UnlockBits();
            pictureBox1.Image = tmpBtmp;
        }
        float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }
        
        float Interpolate(float min, float max, float gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }
        
        void ProcessScanLine(int y, Vector4 pa, Vector4 pb, Vector4 pc, Vector4 pd, Vector4 color)
        {
            var gradient1 = pa.Y != pb.Y ? (y - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (y - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);
            
            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);
            
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Interpolate(z1, z2, gradient);
                DrawPoint(new Vector4(x, y, z, 1), Vector4.Multiply(0.5F, color));
            }
        }

        public void DrawTriangle(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 color)
        {
            if (p1.Y > p2.Y)
            {
                Swap<Vector4>(ref p1, ref p2);
            }

            if (p2.Y > p3.Y)
            {
                Swap<Vector4>(ref p2, ref p3);
            }
            if (p1.Y > p2.Y)
            {
                Swap<Vector4>(ref p1, ref p2);
            }
            float dP1P2, dP1P3;

            dP1P2 = (p2.Y - p1.Y > 0) ? (p2.X - p1.X) / (p2.Y - p1.Y) : 0;

            dP1P3 = (p3.Y - p1.Y > 0) ? (p3.X - p1.X) / (p3.Y - p1.Y) : 0;
            
            // first triangle
            if (dP1P2 > dP1P3)
            {
                for (int y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                        ProcessScanLine(y, p1, p3, p1, p2, color);
                    else
                        ProcessScanLine(y, p1, p3, p2, p3, color);
                }
            }
            // second triangle
            else
            {
                for (int  y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    if (y < p2.Y)
                        ProcessScanLine(y, p1, p2, p1, p3, color);
                    else
                        ProcessScanLine(y, p2, p3, p1, p3, color);
                }
            }
        }

        private Vector3 CalculateLight(Vector4 point, Vector3 lightPoint, Vector3 normal, Vector3 cameraPoint, Vector3 rgb)
        {
            // trzeba zamienic lightpointa na prawdziwy wektor, a nie wektor wspolrzednych
            float ka = 0.5F;
            float kd = 0.5F;
            float ks = 0.5F;
            float a = 0.5F;
            Vector3 lightSource = new Vector3(0.2F, 0.2F, 0.2F);
            Vector3 r = 2 * Vector3.Subtract(Vector3.Multiply(Vector3.Cross(lightPoint, normal), normal), lightPoint);
            return Vector3.Add(
                Vector3.Multiply(rgb, new Vector3(ka, ka, ka)), 
                Vector3.Multiply(lightSource, Vector3.Add(
                    (Vector3.Multiply(
                        new Vector3(kd, kd, kd), Vector3.Cross(normal, lightPoint))),
                        Vector3.Multiply(new Vector3(ks,ks,ks), Vector3.Cross(cameraPoint, r))
                    )));

        }
        public void DrawPoint(Vector4 point, Vector4 color)
        {
            if (point.X >= 0 && point.Y >= 0 && point.X < pictureBox1.Width && point.Y < pictureBox1.Height)
            {
                PutPixel((int)point.X, (int)point.Y, point.Z, color);
            }
        }
        public void PutPixel(int x, int y, float z, Vector4 color)
        {
            if (zindex[x,y] < z)
            {
                return;
            }

            zindex[x,y] = z;

            lbm.SetPixel(x, y, Color.FromArgb((int)color.X, (int)color.Y, (int)color.Z));
        }
        public static void Swap<T>(ref T x, ref T y)
        {
            T tmp = y;
            y = x;
            x = tmp;
        }
        
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            Graphic3D.width = pictureBox1.Width;
            Graphic3D.height = pictureBox1.Height;
            objects.Add(LoadJSONFile("monkey.babylon")[0]);
            float scale = 0.2F;
            objects.Add(LoadJSONFile("monkey.babylon")[0]);
            for (int i = 0; i < objects[0].pointList.Count; i++)
            {
                objects[1].pointList[i] = new Vector4(scale * objects[0].pointList[i].X, scale * objects[0].pointList[i].Y, scale * objects[0].pointList[i].Z, 1);
            }
            objects[0].rotate = false;
            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bm;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Graphic3D.Mview = Graphic3D.CreateViewAt(3.0, 0.5, 0.5, 0, 0.5, 0.5, 0, 0, 1);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
    public struct Triangle
    {
        public int p1, p2, p3;
    }
    public class Object3D
    {
        public bool rotate = true;
        static double t=0.0;
        public Matrix4x4 MModel = new Matrix4x4(1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);
        public List<Triangle> triangleList = new List<Triangle>();
        public List<Vector4> pointList = new List<Vector4>();
        public Object3D()
        {
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            myTimer.Interval = 50; // 1000 ms is one second
            myTimer.Start();

        }
        public void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            if (rotate == true)
            {
                lock (Form1.syncLock)
                {
                    MModel = new Matrix4x4((float)Math.Cos(t), (float)(-Math.Sin(t)), 0, (float)(2 * Math.Sin(t)),
                    (float)Math.Sin(t), (float)Math.Cos(t), 0, (float)(2 * Math.Cos(t)),
                    0, 0, 1, 0,
                    0, 0, 0, 1);
                    t += 0.05;
                }
            }
        }
        public void addTriangle(Triangle e)
        {
            triangleList.Add(e);
        }
        public void addPoint(Vector4 e)
        {
            pointList.Add(e);
        }
    }
    public class Edge
    {
        public Vector4 firstPoint;
        public Vector4 secondPoint;
        public Edge(Vector4 myPoint1, Vector4 myPoint2)
        {
            firstPoint = myPoint1;
            secondPoint = myPoint2;
            if (m < 0)
                x = Math.Min(myPoint1.X, myPoint2.X);
            else
                x = Math.Max(myPoint1.X, myPoint2.X);
        }

        public int yMax { get { return (int) Math.Min(firstPoint.Y, secondPoint.Y) + 1; } }
        public double x { get; set; }
        public double z { get { return (firstPoint.Z - secondPoint.Z / 2.0); } }
        public double m { get { if (firstPoint.Y == secondPoint.Y) return 0;
                return (double)(firstPoint.X - secondPoint.X) / (double)(firstPoint.Y - secondPoint.Y); } }
    }
    public class MyPB : PictureBox
    {
        public MyPB()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
        }
    }
}
