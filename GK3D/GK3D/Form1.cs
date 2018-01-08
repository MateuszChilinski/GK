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
        int[,] zindex;
        static double t = 0;
        Object3D c;
        int R = 255, G = 0, B = 0;

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            int i = 0;
            zindex = new int[pictureBox1.Width, pictureBox1.Height];
            for(int j = 0; j < pictureBox1.Width; j++)
            {
                for(int k = 0; k < pictureBox1.Height; k++)
                {
                    zindex[j, k] = Int32.MaxValue;
                }
            }
            foreach (Polygon poly in c.polygonList)
            {
                switch(i)
                {
                    case 0:
                        R = 255; G = 0; B = 0;
                        break;
                    case 1:
                        R = 0; G = 255; B = 0;
                        break;
                    case 2:
                        R = 0; G = 0; B = 255;
                        break;
                    case 3:
                        R = 255; G = 255; B = 0;
                        break;
                    case 4:
                        R = 255; G = 0; B = 255;
                        break;
                    case 5:
                        R = 0; G = 0; B = 0;
                        break;
                    case 6:
                        R = 255; G = 111; B = 102;
                        break;
                    default:
                        R = 0; G = 255; B = 255;
                        break;
                }
                i++;
                FillPolygon(e, Polygon.convertTo2D(poly).edgeList);
                pictureBox1.Invalidate();
            }
        }
        private void FillPolygon(PaintEventArgs e, List<Edge> edges)
        {
            List<Vector4> points = new List<Vector4>();

            List<Edge>[] EA = new List<Edge>[pictureBox1.Height];
            for (int i = 0; i < pictureBox1.Height; i++)
            {
                EA[i] = new List<Edge>();
            }
            foreach(Edge ed in edges)
            {
                EA[(int) Math.Max(ed.firstPoint.Y, ed.secondPoint.Y)].Add(ed);
            }
            List<Edge> AET = new List<Edge>();
            for (int y = pictureBox1.Height - 1; y >= 0; y--)
            {
                foreach (var z in EA[y])
                    AET.Add(z);
                AET = AET.OrderBy(edge => edge.x).ToList();
                List<int> toRemove = new List<int>();
                for (int i = 0; i < AET.Count - 1; i += 2)
                {
                    DrawLine((int)AET[i].x, (int)AET[i + 1].x, y, Math.Max((int)AET[i].z, (int)AET[i+1].z), e, false);
                }

                for (int i = 0; i < AET.Count; i++)
                {
                    if (AET[i].yMax == y)
                        toRemove.Add(i);
                }

                for (int i = 0; i < AET.Count; i++)
                {
                    AET[i].x -= AET[i].m;
                    if (AET[i].x < 0)
                        toRemove.Add(i);
                }
                toRemove = toRemove.Distinct().ToList();
                toRemove = toRemove.OrderByDescending(i => i).ToList();
                foreach (int z in toRemove)
                {
                    AET.RemoveAt(z);
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
        private void DrawLine(int x1, int x2, int y, int z, PaintEventArgs e, bool second)
        {
            if (x1 > Width || x2 > Width || y > Height)
                return;
            DoLine(x1, y, z, x2, y, z);
            //e.Graphics.DrawLine(myPen, x1, y, x2, y);
            //DrawLine(new MyPoint(x1, y), new MyPoint(x2, y), e, second);
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Graphic3D.Mmodel = new Matrix4x4((float)Math.Cos(t), (float)(-Math.Sin(t)), 0, (float) (0.5*Math.Sin(t)),
                   (float)Math.Sin(t), (float)Math.Cos(t), 0, (float)(0.5 * Math.Sin(t)),
                   0, 0, 1, (float)(0.5 * Math.Sin(t)),
                   0, 0, 0, 1);
            t += 0.05;
        }
        public static void Swap<T>(ref T x, ref T y)
        {
            T tmp = y;
            y = x;
            x = tmp;
        }
        public void DoLine(int startX, int startY, int startZ, int endX, int endY, int endZ)
        {
            Action<int, int, int> callback = (x, y, z) => {
                
                if (x >= pictureBox1.Width || y >= pictureBox1.Height || zindex[x,y] < z)
                    return;
                zindex[x, y] = z;
                ((Bitmap)pictureBox1.Image).SetPixel(x, y, Color.FromArgb(R,G,B));
        };
            int dx, dy, dz;
            int sx, sy, sz;
            int accum, accum2;//accumilator

            dx = endX - startX;//Start X subtracted from End X
            dy = endY - startY;
            dz = endZ - startZ;

            sx = ((dx) < 0 ? -1 : ((dx) > 0 ? 1 : 0));//if dx is less than 0, sx = -1; otherwise if dx is greater than 0, sx = 1; otherwise sx = 0
            sy = ((dy) < 0 ? -1 : ((dy) > 0 ? 1 : 0));
            sz = ((dz) < 0 ? -1 : ((dz) > 0 ? 1 : 0));

            //dx = (dx < 0 ? -dx : dx);//if dx is less than 0, dx = -dx (becomes positive), otherwise nothing changes
            dx = Math.Abs(dx);//Absolute value
            //dy = (dy < 0 ? -dy : dy);
            dy = Math.Abs(dy);

            dz = Math.Abs(dz);

            endX += sx;//Add sx to End X
            endY += sy;
            endZ += sz;

            if (dx > dy)//if dx is greater than dy
            {
                if (dx > dz)
                {
                    accum = dx >> 1;
                    accum2 = accum;
                    do
                    {

                        callback(startX, startY, startZ);

                        accum -= dy;
                        accum2 -= dz;
                        if (accum < 0)
                        {
                            accum += dx;
                            startY += sy;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dx;
                            startZ += sz;
                        }
                        startX += sx;
                    }
                    while (startX != endX);
                }
                else
                {
                    accum = dz >> 1;
                    accum2 = accum;
                    do
                    {
                        callback(startX, startY, startZ);

                        accum -= dy;
                        accum2 -= dx;
                        if (accum < 0)
                        {
                            accum += dz;
                            startY += sy;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dz;
                            startX += sx;
                        }
                        startZ += sz;
                    }
                    while (startZ != endZ);
                }
            }
            else
            {
                if (dy > dz)
                {
                    accum = dy >> 1;
                    accum2 = accum;
                    do
                    {
                        callback(startX, startY, startZ);

                        accum -= dx;
                        accum2 -= dz;
                        if (accum < 0)
                        {
                            accum += dx;
                            startX += sx;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dx;
                            startZ += sz;
                        }
                        startY += sy;
                    }
                    while (startY != endY);
                }
                else
                {
                    accum = dz >> 1;
                    accum2 = accum;
                    do
                    {
                        callback(startX, startY, startZ);

                        accum -= dx;
                        accum2 -= dy;
                        if (accum < 0)
                        {
                            accum += dx;
                            startX += sx;
                        }
                        if (accum2 < 0)
                        {
                            accum2 += dx;
                            startY += sy;
                        }
                        startZ += sz;
                    }
                    while (startZ != endZ);
                }
            }
        }
        public Form1()
        {

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 50;
            aTimer.Enabled = true;
            InitializeComponent();

            Graphic3D.width = pictureBox1.Width;
            Graphic3D.height = pictureBox1.Height;

            c = new Object3D();
            Polygon poly= new Polygon();

            poly.addEdge(new Edge(new Vector4(0, 0, 0, 1), new Vector4(1, 0, 0, 1)));
            poly.addEdge(new Edge(new Vector4(1, 0, 0, 1), new Vector4(1, 0, 1, 1)));
            poly.addEdge(new Edge(new Vector4(1, 0, 1, 1), new Vector4(0, 0, 1, 1)));
            poly.addEdge(new Edge(new Vector4(0, 0, 1, 1), new Vector4(0, 0, 0, 1)));
            c.addPolygon(poly);
            poly = new Polygon();
            poly.addEdge(new Edge(new Vector4(0, 1, 0, 1), new Vector4(1, 1, 0, 1)));
            poly.addEdge(new Edge(new Vector4(1, 1, 0, 1), new Vector4(1, 1, 1, 1)));
            poly.addEdge(new Edge(new Vector4(1, 1, 1, 1), new Vector4(0, 1, 1, 1)));
            poly.addEdge(new Edge(new Vector4(0, 1, 1, 1), new Vector4(0, 1, 0, 1)));
            c.addPolygon(poly);
            poly = new Polygon();

            poly.addEdge(new Edge(new Vector4(0, 0, 0, 1), new Vector4(1, 0, 0, 1)));
            poly.addEdge(new Edge(new Vector4(1, 0, 0, 1), new Vector4(1, 1, 0, 1)));
            poly.addEdge(new Edge(new Vector4(1, 1, 0, 1), new Vector4(0, 1, 0, 1)));
            poly.addEdge(new Edge(new Vector4(0, 1, 0, 1), new Vector4(0, 0, 0, 1)));
            c.addPolygon(poly);
            poly = new Polygon();
            poly.addEdge(new Edge(new Vector4(0, 0, 1, 1), new Vector4(1, 0, 1, 1)));
            poly.addEdge(new Edge(new Vector4(1, 0, 1, 1), new Vector4(1, 1, 1, 1)));
            poly.addEdge(new Edge(new Vector4(1, 1, 1, 1), new Vector4(0, 1, 1, 1)));
            poly.addEdge(new Edge(new Vector4(0, 1, 1, 1), new Vector4(0, 0, 1, 1)));
            c.addPolygon(poly);
            poly = new Polygon();
            poly.addEdge(new Edge(new Vector4(0, 0, 0, 1), new Vector4(0, 0, 1, 1)));
            poly.addEdge(new Edge(new Vector4(0, 0, 1, 1), new Vector4(0, 1, 1, 1)));
            poly.addEdge(new Edge(new Vector4(0, 1, 1, 1), new Vector4(0, 1, 0, 1)));
            poly.addEdge(new Edge(new Vector4(0, 1, 0, 1), new Vector4(0, 0, 0, 1)));
            c.addPolygon(poly);
            poly = new Polygon();
            poly.addEdge(new Edge(new Vector4(1, 0, 0, 1), new Vector4(1, 0, 1, 1)));
            poly.addEdge(new Edge(new Vector4(1, 0, 1, 1), new Vector4(1, 1, 1, 1)));
            poly.addEdge(new Edge(new Vector4(1, 1, 1, 1), new Vector4(1, 1, 0, 1)));
            poly.addEdge(new Edge(new Vector4(1, 1, 0, 1), new Vector4(1, 0, 0, 1)));
            c.addPolygon(poly);
            poly = new Polygon();

            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bm;
        }
        
    }
    public class Polygon
    {
        public List<Edge> edgeList = new List<Edge>();
        public void addEdge(Edge e)
        {
            edgeList.Add(e);
        }
        static public Polygon convertTo2D(Polygon polygon)
        {
            Polygon newPoly = new Polygon();
            foreach(Edge e in polygon.edgeList)
            {
                newPoly.addEdge(new Edge(Graphic3D.CreatePoint(e.firstPoint), Graphic3D.CreatePoint(e.secondPoint)));
            }
            return newPoly;
        }
        public Vector3 getNormalVector()
        {
            Vector3 v1 = new Vector3(edgeList[0].firstPoint.X, edgeList[0].firstPoint.Y, edgeList[0].firstPoint.Z);
            Vector3 v2 = new Vector3(edgeList[1].firstPoint.X, edgeList[1].firstPoint.Y, edgeList[1].firstPoint.Z);
            Vector3 cross = Vector3.Cross(v1, v2);
            return cross;
        }
    }
    public class Object3D
    {
        public List<Polygon> polygonList = new List<Polygon>();
        public void addPolygon(Polygon e)
        {
            polygonList.Add(e);
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
}
