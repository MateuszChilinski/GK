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

        float ka = 0.25f;
        float kd = 0.25f;
        float ks = 0.3f;
        float alpha = 40f;

        float[,] zindex;
        double t = 0;
        Vector4 cameraPos = new Vector4(3.0f, 0.5f, 0.5f, 1);
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
                
                for (var index = 0; index < verticesCount; index++)
                {
                    var x = (float)verticesArray[index * verticesStep].Value;
                    var y = (float)verticesArray[index * verticesStep + 1].Value;
                    var z = (float)verticesArray[index * verticesStep + 2].Value;
                    // Loading the vertex normal exported by Blender
                    var nx = (float)verticesArray[index * verticesStep + 3].Value;
                    var ny = (float)verticesArray[index * verticesStep + 4].Value;
                    var nz = (float)verticesArray[index * verticesStep + 5].Value;
                    mesh.addPoint(new MyPoint(new Vector4(x,y,z,1), new Vector4(nx, ny, nz, 1)));
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
            zindex = new float[pictureBox1.Width, pictureBox1.Height];
           
            tmpBtmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics gfx = Graphics.FromImage(tmpBtmp))
            using (SolidBrush brush = new SolidBrush(Color.Black))
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
                    cameraPos = new Vector4(3.0f, 0.5f, 0.5f, 1);
                    Graphic3D.Mview = Graphic3D.CreateViewAt(3.0, 0.5, 0.5, objects[1].pointList[5]._currentCoords.X, objects[1].pointList[5]._currentCoords.Y, objects[1].pointList[5]._currentCoords.Z, 0, 0, 1);
                }
                if (radioButton3.Checked == true)
                {
                    cameraPos = new Vector4(objects[1].pointList[5]._currentCoords.X + 2, objects[1].pointList[5]._currentCoords.Y + 2, objects[1].pointList[5]._currentCoords.Z + 2, 1);
                    Graphic3D.Mview = Graphic3D.CreateViewAt(objects[1].pointList[5]._currentCoords.X + 2, objects[1].pointList[5]._currentCoords.Y + 2, objects[1].pointList[5]._currentCoords.Z + 2, objects[1].pointList[5]._currentCoords.X, objects[1].pointList[5]._currentCoords.Y,
                        objects[1].pointList[5]._currentCoords.Z, 0, 0, 1);
                }
                int k = 0;
                foreach (Object3D c in objects)
                {
                    //if (k++ == 0) continue;
                    Parallel.ForEach(c.triangleList, poly =>
                    {
                        {
                            float R = 0.0f, G = 0.0f, B = 0.0f;
                            if (k == 0)
                            {
                                R = 1.0f;
                                G = 0.549019608f;
                                B = 0.0f;
                            }
                            else if (k == 1)
                            {
                                R = 0.0f;
                                B = 1.0f;
                                G = 0.0f;
                            }
                            var p1 = c.pointList[poly.p1];
                            var p2 = c.pointList[poly.p2];
                            var p3 = c.pointList[poly.p3];


                            DrawTriangle(p1, p2, p3, new Vector4(R, G, B, 1.0f));

                        }
                    });
                    k++;
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

        Vector4 Interpolate(Vector4 max, Vector4 min, float gradient)
        {
            return Vector4.Lerp(max, min, gradient);
        }
        void ProcessScanLine(ScanLineData data, MyPoint va, MyPoint vb, MyPoint vc, MyPoint vd, Vector4 color)
        {
            Vector4 pa = va.coords2d;
            Vector4 pb = vb.coords2d;
            Vector4 pc = vc.coords2d;
            Vector4 pd = vd.coords2d;

            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = pa.Y != pb.Y ? (data.currentY - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = pc.Y != pd.Y ? (data.currentY - pc.Y) / (pd.Y - pc.Y) : 1;

            int sx = (int)Interpolate(pa.X, pb.X, gradient1);
            int ex = (int)Interpolate(pc.X, pd.X, gradient2);

            // starting Z & ending Z
            float z1 = Interpolate(pa.Z, pb.Z, gradient1);
            float z2 = Interpolate(pc.Z, pd.Z, gradient2);

            float real_z1 = Interpolate(va._currentCoords.Z, vb._currentCoords.Z, gradient1);
            float real_z2 = Interpolate(vc._currentCoords.Z, vd._currentCoords.Z, gradient1);
            Vector4 Vsnl=new Vector4(), Venl = new Vector4();
            float snl=0.0f, enl=0.0f;
            if (radioButton4.Checked == true)
            {
                snl = Interpolate(data.ndotla, data.ndotlb, gradient1);
                enl = Interpolate(data.ndotlc, data.ndotld, gradient2);
            }
            else if(radioButton5.Checked == true)
            {
                Vsnl = Interpolate(data.normalA, data.normalB, gradient1);
                Venl = Interpolate(data.normalC, data.normalD, gradient2);
            }


            Vector4 lightPos2 = new Vector4(0, 10, 10, 1);
            // Light position 
            Vector4 lightPos = new Vector4(objects[1].pointList[5]._currentCoords.X - 0.01f, objects[1].pointList[5]._currentCoords.Y - 0.01f, objects[1].pointList[5]._currentCoords.Z - 0.011f, 1);
            List<Vector4> lights = new List<Vector4>();
            lights.Add(lightPos);
            lights.Add(lightPos2);
            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Interpolate(z1, z2, gradient);

                var real_z = Interpolate(real_z1, real_z2, gradient);
                Vector4 Vndotl;
                float ndotl=0.0f;
                if (radioButton4.Checked == true)
                {
                    ndotl = Interpolate(snl, enl, gradient);
                }
                else if(radioButton5.Checked == true)
                {
                    Vndotl = Interpolate(Vsnl, Venl, gradient);
                    ndotl = CalculateLight(lights, new Vector4(x / pictureBox1.Width, data.currentY / pictureBox1.Height, real_z, 1), Vndotl);
                }
                float ndotl1 = Interpolate(snl, enl, gradient);
                // changing the color value using the cosine of the angle
                // between the light vector and the normal vector
                DrawPoint(new Vector4(x, data.currentY, z, 1), color* ndotl);
            }
        }
        public float CalculateLight(List<Vector4> lightSources, MyPoint point)
        {
            return CalculateLight(lightSources, point._currentCoords, point.normal);
        }
        public float CalculateLight(List<Vector4> lightSources, Vector4 point, Vector4 normal)
        {
            if (radioButton6.Checked)
                return CalculateLightPhong(lightSources, point, normal);
            else
                return CalculateLightBlinn(lightSources, point, normal);
        }
        public float CalculateLightBlinn(List<Vector4> lightSources, Vector4 point, Vector4 normal)
        {
            float i = 0.0f;
            foreach (Vector4 light in lightSources)
            {
                i += kd * ComputeNDotL(point, normal, light) + ks * (float)Math.Pow(ComputeNDotH(point, normal, light), alpha);
            }
            return Clamp(ka + i);
        }
        public float CalculateLightPhong(List<Vector4> lightSources, Vector4 point, Vector4 normal)
        {
            float i = 0.0f;
            foreach (Vector4 light in lightSources)
            {
                float dp = 2 * ComputeNDotL(point, normal, light);
                Vector4 R = Vector4.Subtract(Vector4.Multiply(dp, Vector4.Normalize(normal)), Vector4.Normalize(light));
                if(dp > 0)
                    i += kd * ComputeNDotL(point, normal, light) + ks * (float)Math.Pow(ComputeRDotV(point, cameraPos, R), alpha);
                else
                    i += kd * ComputeNDotL(point, normal, light);
            }
            return Clamp(ka + i);
        }
        public void DrawTriangle(MyPoint v1, MyPoint v2, MyPoint v3, Vector4 color)
        {
            if (v1.coords2d.Y > v2.coords2d.Y)
            {
                Swap<MyPoint>(ref v1, ref v2);
            }

            if (v2.coords2d.Y > v3.coords2d.Y)
            {
                Swap<MyPoint>(ref v2, ref v3);
            }
            if (v1.coords2d.Y > v2.coords2d.Y)
            {
                Swap<MyPoint>(ref v1, ref v2);
            }

            Vector4 p1 = v1.coords2d;
            Vector4 p2 = v2.coords2d;
            Vector4 p3 = v3.coords2d;

            Vector4 lightPos2 = new Vector4(0, 10, 10, 1);
            // Light position 
            Vector4 lightPos = new Vector4(objects[1].pointList[5]._currentCoords.X - 0.1f, objects[1].pointList[5]._currentCoords.Y - 0.1f, objects[1].pointList[5]._currentCoords.Z - 0.1f, 1);
            List<Vector4> lights = new List<Vector4>();

            lights.Add(lightPos);
            lights.Add(lightPos2);
            // computing the cos of the angle between the light vector and the normal vector
            // it will return a value between 0 and 1 that will be used as the intensity of the color
            float nl1 = 0.0f, nl2 = 0.0f, nl3=0.0f;
            if (radioButton4.Checked == true)
            {
                nl1 = CalculateLight(lights, v1);
                nl2 = CalculateLight(lights, v2);
                nl3 = CalculateLight(lights, v3);
            }
            var data = new ScanLineData { };

            float dP1P2, dP1P3;

            dP1P2 = (p2.Y - p1.Y > 0) ? (p2.X - p1.X) / (p2.Y - p1.Y) : 0;

            dP1P3 = (p3.Y - p1.Y > 0) ? (p3.X - p1.X) / (p3.Y - p1.Y) : 0;
            // Light position 

            // first triangle
            if (dP1P2 > dP1P3)
            {
                for (int y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    data.currentY = y;
                    if (y < p2.Y)
                    {
                            data.ndotla = nl1;
                            data.ndotlb = nl3;
                            data.ndotlc = nl1;
                            data.ndotld = nl2;
                            data.normalA = v1.normal;
                            data.normalB = v3.normal;
                            data.normalC = v1.normal;
                            data.normalD = v2.normal;
                        ProcessScanLine(data, v1, v3, v1, v2, color);
                    }
                    else
                    {
                            data.ndotla = nl1;
                            data.ndotlb = nl3;
                            data.ndotlc = nl2;
                            data.ndotld = nl3;
                            data.normalA = v1.normal;
                            data.normalB = v3.normal;
                            data.normalC = v2.normal;
                            data.normalD = v3.normal;
                        ProcessScanLine(data, v1, v3, v2, v3, color);
                    }
                }
            }
            // second triangle
            else
            {
                for (int  y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    data.currentY = y;
                    if (y < p2.Y)
                        {
                            data.ndotla = nl1;
                            data.ndotlb = nl2;
                            data.ndotlc = nl1;
                            data.ndotld = nl3;
                            data.normalA = v1.normal;
                            data.normalB = v2.normal;
                            data.normalC = v1.normal;
                            data.normalD = v3.normal;
                        ProcessScanLine(data, v1, v2, v1, v3, color);
                    }
                    else
                    {
                            data.ndotla = nl2;
                            data.ndotlb = nl3;
                            data.ndotlc = nl1;
                            data.ndotld = nl3;
                            data.normalA = v2.normal;
                            data.normalB = v3.normal;
                            data.normalC = v1.normal;
                            data.normalD = v3.normal;
                        ProcessScanLine(data, v2, v3, v1, v3, color);
                    }
                }
            }
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
            int R =  (int)(color.X * 255);
            int G =  (int)(color.Y * 255);
            int B = (int)(color.Z * 255);
            lbm.SetPixel(x, y, Color.FromArgb(255,R, G, B));
        }
        public static void Swap<T>(ref T x, ref T y)
        {
            T tmp = y;
            y = x;
            x = tmp;
        }
        float ComputeNDotH(Vector4 point, Vector4 normal, Vector4 lightPosition)
        {

            Vector3 point3d = new Vector3(point.X, point.Y, point.Z);
            Vector3 light3d = new Vector3(lightPosition.X, lightPosition.Y, lightPosition.Z);
            Vector3 normal3d = new Vector3(normal.X, normal.Y, normal.Z);

            Vector3 cameraPos3D = new Vector3(cameraPos.X, cameraPos.Y, cameraPos.Z);

            var cameraDirection = cameraPos3D - point3d;
            var lightDirection = light3d - point3d;

            normal3d = Vector3.Normalize(normal3d);
            lightDirection = Vector3.Normalize(lightDirection);
            cameraDirection = Vector3.Normalize(cameraDirection);

            Vector3 H = Vector3.Normalize(lightDirection + cameraDirection);

            return Math.Max(0, Vector3.Dot(normal3d, H));
        }
        float ComputeNDotL(Vector4 point, Vector4 normal, Vector4 lightPosition)
        {
            Vector3 point3d = new Vector3(point.X, point.Y, point.Z);
            Vector3 light3d = new Vector3(lightPosition.X, lightPosition.Y, lightPosition.Z);
            Vector3 normal3d = new Vector3(normal.X, normal.Y, normal.Z);
            var lightDirection = light3d - point3d;

            normal3d = Vector3.Normalize(normal3d);
            lightDirection = Vector3.Normalize(lightDirection);

            return Math.Max(0, Vector3.Dot(lightDirection, normal3d));
        }
        float ComputeRDotV(Vector4 point, Vector4 cameraPos, Vector4 R)
        {
            Vector3 point3d = new Vector3(point.X, point.Y, point.Z);
            Vector3 cameraPos3D = new Vector3(cameraPos.X, cameraPos.Y, cameraPos.Z);
            Vector3 R3D = new Vector3(R.X, R.Y, R.Z);
            var cameraDirection = cameraPos3D - point3d;

            R3D = Vector3.Normalize(R3D);
            cameraDirection = Vector3.Normalize(cameraDirection);

            return Math.Max(0, Vector3.Dot(R3D, cameraDirection));
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
                objects[1].pointList[i].realCoords = new Vector4(scale * objects[0].pointList[i].realCoords.X, scale * objects[0].pointList[i].realCoords.Y, scale * objects[0].pointList[i].realCoords.Z, 1);
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
            cameraPos = new Vector4(3.0f, 0.5f, 0.5f, 1);
            Graphic3D.Mview = Graphic3D.CreateViewAt(3.0, 0.5, 0.5, 0, 0.5, 0.5, 0, 0, 1);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
    public struct ScanLineData
    {
        public int currentY;
        public float ndotla;
        public float ndotlb;
        public float ndotlc;
        public float ndotld;
        public Vector4 normalA;
        public Vector4 normalB;
        public Vector4 normalC;
        public Vector4 normalD;
    }
    public struct Triangle
    {
        public int p1, p2, p3;
    }
    public class Object3D
    {
        public bool rotate = true;
        static double t=0.0;
        private Matrix4x4 MModel = new Matrix4x4(1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);
        public List<Triangle> triangleList = new List<Triangle>();
        public List<MyPoint> pointList = new List<MyPoint>();
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
                    for(int i = 0; i < pointList.Count; i++)
                    {
                        pointList[i]._currentCoords = Graphic3D.MultiplyV4(MModel, pointList[i].realCoords);
                        pointList[i].normal = Graphic3D.MultiplyV4(MModel, pointList[i]._normal);
                    }
                    MModel = new Matrix4x4((float)Math.Cos(t), (float)(-Math.Sin(t)), 0, (float)(2 * Math.Sin(t)),
                    (float)Math.Sin(t), (float)Math.Cos(t), 0, (float)(2 * Math.Cos(t)),
                    0, 0, 1, 0,
                    0, 0, 0, 1);
                    //MModel = new Matrix4x4((float)Math.Cos(t), (float)(-Math.Sin(t)), 0, (float)(2 * Math.Sin(t)),
                    //(float)Math.Sin(t), (float)Math.Cos(t), 0, (float)(2 * Math.Cos(t)),
                    //0, 0, 1, 0,
                    //0, 0, 0, 1);
                    t += 0.05;
                }
            }
        }
        public void addTriangle(Triangle e)
        {
            triangleList.Add(e);
        }
        public void addPoint(MyPoint e)
        {
            pointList.Add(e);
        }
    }
    public class MyPoint
    {
        public Vector4 realCoords;
        public Vector4 _currentCoords;
        public Vector4 _normal;
        public Vector4 currentCoords { get { return new Vector4(_currentCoords.X, _currentCoords.Y, coords2d.Z, 1); } }
        public Vector4 normal;
        public Vector4 coords2d { get { return Graphic3D.CreatePoint(_currentCoords); } }
        public MyPoint(Vector4 v)
        {
            realCoords = v;
            _currentCoords = v;
        }

        public MyPoint(Vector4 v, Vector4 vector4)
        {
            realCoords = v;
            _currentCoords = v;
            _normal = vector4;
            normal = vector4;
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
