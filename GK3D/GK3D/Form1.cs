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

namespace GK3D
{
    public partial class Form1 : Form
    {
        int cameraType = 0;
        int lightType = 0;
        int shadingType = 0;
        static Matrix4x4 Mmodel = new Matrix4x4(1, 0, 0, 0,
                   0, 1, 0, 0,
                   0, 0, 1, 0,
                   0, 0, 0, 1);
        Matrix4x4 Mview = CreateViewAt(3.5, 0.5, 0.5, 0, 0.1, 0.5, 0, 0, 1);
        Matrix4x4 Mproj = CreateProjectionMatrix(1, 100, 45, 1);
        static double t = 0;
        Cube c;
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap tmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(tmp);
            foreach (Edge ed in c.edgeList)
            {
                Vector4 fp = CreatePoint(ed.firstPoint);
                Vector4 sp = CreatePoint(ed.secondPoint);
                g.DrawLine(new Pen(Color.Red), new Point((int)fp.X, (int)fp.Y), new Point((int)sp.X, (int)sp.Y));
            }
            pictureBox1.Image = tmp;
            pictureBox1.Invalidate();
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Mmodel = new Matrix4x4((float)Math.Cos(t), (float)(-Math.Sin(t)), 0, (float) (0.5*Math.Sin(t)),
                   (float)Math.Sin(t), (float)Math.Cos(t), 0, (float)(0.5 * Math.Sin(t)),
                   0, 0, 1, (float)(0.5 * Math.Sin(t)),
                   0, 0, 0, 1);
            t += 0.1;
        }
        public Form1()
        {

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 50;
            aTimer.Enabled = true;

            InitializeComponent();
            c = new Cube();
            c.addEdge(new Edge(new Vector4(0, 0, 0, 1), new Vector4(1, 0, 0, 1)));
            c.addEdge(new Edge(new Vector4(1, 0, 0, 1), new Vector4(1, 0, 1, 1)));
            c.addEdge(new Edge(new Vector4(1, 0, 1, 1), new Vector4(0, 0, 1, 1)));
            c.addEdge(new Edge(new Vector4(0, 0, 1, 1), new Vector4(0, 0, 0, 1)));

            c.addEdge(new Edge(new Vector4(0, 1, 0, 1), new Vector4(1, 1, 0, 1)));
            c.addEdge(new Edge(new Vector4(1, 1, 0, 1), new Vector4(1, 1, 1, 1)));
            c.addEdge(new Edge(new Vector4(1, 1, 1, 1), new Vector4(0, 1, 1, 1)));
            c.addEdge(new Edge(new Vector4(0, 1, 1, 1), new Vector4(0, 1, 0, 1)));

            c.addEdge(new Edge(new Vector4(0, 0, 0, 1), new Vector4(0, 1, 0, 1)));
            c.addEdge(new Edge(new Vector4(1, 0, 0, 1), new Vector4(1, 1, 0, 1)));
            c.addEdge(new Edge(new Vector4(1, 0, 1, 1), new Vector4(1, 1, 1, 1)));
            c.addEdge(new Edge(new Vector4(0, 0, 1, 1), new Vector4(0, 1, 1, 1)));
            Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bm;
        }
        static private Matrix4x4 CreateProjectionMatrix(double n, double f, double fov, double a)
        {
            double e = 1 / (Math.Tan(fov * Math.PI / 180 / 2));
            return new Matrix4x4((float)e, 0F, 0F, 0F,
                0F, (float)(e / a), 0F, 0F,
                0F, 0F, (float)((f + n) / (n - f)), (float)((2 * f * n) / (n - f)),
                0, 0, -1, 0);
        }
        static private Matrix4x4 CreateViewAt(double CPx, double CPy, double CPz, double CTx, double CTy, double CTz, double UPVx, double UPVy, double UPVz)
        {
            double zAxis_X = CPx - CTx;
            double zAxis_Y = CPy - CTy;
            double zAxis_Z = CPz - CTz;
            double nom = Math.Sqrt(zAxis_X * zAxis_X + zAxis_Y * zAxis_Y + zAxis_Z * zAxis_Z);
            double zAxis_XW = zAxis_X /nom;
            double zAxis_YW = zAxis_Y / nom;
            double zAxis_ZW = zAxis_Z / nom;

            double xAxis_X = UPVy * zAxis_ZW - UPVz * zAxis_YW;
            double xAxis_Y = UPVz * zAxis_XW - UPVx * zAxis_ZW;
            double xAxis_Z = UPVx * zAxis_YW - UPVy * zAxis_XW;
            nom = Math.Sqrt(xAxis_X * xAxis_X + xAxis_Y * xAxis_Y + xAxis_Z * xAxis_Z);
            double xAxis_XW = xAxis_X / nom;
            double xAxis_YW = xAxis_Y / nom;
            double xAxis_ZW = xAxis_Z / nom;

            double yAxis_X = zAxis_YW * xAxis_ZW - zAxis_ZW * xAxis_YW;
            double yAxis_Y = zAxis_ZW * xAxis_XW - zAxis_XW * xAxis_ZW;
            double yAxis_Z = zAxis_XW * xAxis_YW - zAxis_YW * xAxis_XW;
            nom = Math.Sqrt(yAxis_X * yAxis_X + yAxis_Y * yAxis_Y + yAxis_Z * yAxis_Z);
            double yAxis_XW = yAxis_X / nom;
            double yAxis_YW = yAxis_Y / nom;
            double yAxis_ZW = yAxis_Z / nom;
            Matrix4x4 rtMatrix;
            Matrix4x4.Invert(new Matrix4x4(
                (float)xAxis_XW, (float)yAxis_XW, (float)zAxis_XW, (float)CPx,
                (float)xAxis_YW, (float)yAxis_YW, (float)zAxis_YW, (float)CPy,
                (float)xAxis_ZW, (float)yAxis_ZW, (float)zAxis_ZW, (float)CPz,
                0, 0, 0, 1), out rtMatrix);
            return rtMatrix;
        }
        public Vector4 CreatePoint(Vector4 point)
        {
            Vector4 pp = Normalize4(MultiplyV4(Mproj, MultiplyV4(Mview, MultiplyV4(Mmodel, new Vector4(point.X, point.Y, point.Z, point.W)))));
            pp.X = (pp.X + 1) / 2 * pictureBox1.Width;
            pp.Y = (pp.Y + 1) / 2 * pictureBox1.Height;
            return pp;
        }
        public Vector4 Normalize4(Vector4 v)
        {
            return new Vector4(v.X / v.W, v.Y / v.W, v.Z / v.W, 1);
        }
        public Vector4 MultiplyV4(Matrix4x4 m4, Vector4 v4)
        {
            Matrix4x4 nm = new Matrix4x4(v4.X, 0, 0, 0,
                v4.Y, 0, 0, 0,
                v4.Z, 0, 0, 0,
                v4.W, 0, 0, 0);
            Matrix4x4 mm = Matrix4x4.Multiply(m4, nm);
            return new Vector4(mm.M11, mm.M21, mm.M31, mm.M41);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            shadingType = 0;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            shadingType = 1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            cameraType = 0;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            cameraType = 1;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            cameraType = 2;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            lightType = 0;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            lightType = 1;
        }
    }
    public class Cube
    {
        public List<Edge> edgeList = new List<Edge>();
        public List<Polygon> polygonList = new List<Polygon>();
        public void addEdge(Edge e)
        {
            edgeList.Add(e);
        }
    }
    public class Edge
    {
        public Vector4 firstPoint;
        public Vector4 secondPoint;
        public Edge(Vector4 fp, Vector4 sp)
        {
            firstPoint = fp;
            secondPoint = sp;
        }
        public Vector3 getVector()
        {
            return new Vector3(secondPoint.X - firstPoint.X, secondPoint.Y - firstPoint.Y, secondPoint.Z - firstPoint.Z);
        }
    }
    public class Polygon
    {
        public List<Edge> edges = new List<Edge>();
        public void addEdge(Edge e)
        {
            edges.Add(e);
        }
        public Vector3 calculateNormal()
        {
            if (edges.Count < 3)
                throw new ArithmeticException("Polygon has to have at least 3 edges!");
            return Vector3.Cross(edges[0].getVector(), edges[1].getVector());
        }
    }
}
