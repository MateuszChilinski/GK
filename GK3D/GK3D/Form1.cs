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
        static Matrix4x4 Mmodel = new Matrix4x4(1, 0, 0, 0,
                   0, 1, 0, 0,
                   0, 0, 1, 0,
                   0, 0, 0, 1);
        Matrix4x4 Mview = new Matrix4x4(-0.113546591F, 0.993532673F, 0, -0.099353267F,
            0, 0, 1, -0.5F,
            0.993532673F, 0.113546591F, 0F, -3.53413765F,
            0, 0, 0, 1);
        Matrix4x4 Mproj = new Matrix4x4(2.414213562F, 0, 0, 0,
                        0, 2.414213562F, 0, 0,
                        0, 0, -1.02020202F, -2.02020202F,
                        0, 0, -1, 0);
        static double t = 0;
        Cube c;
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            foreach (Edge ed in c.edgeList)
            {
                Vector4 fp = CreatePoint(ed.firstPoint);
                Vector4 sp = CreatePoint(ed.secondPoint);
                g.DrawLine(new Pen(Color.Red), new Point((int)fp.X, (int)fp.Y), new Point((int)sp.X, (int)sp.Y));
            }
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
    }
    public class Cube
    {
        public List<Edge> edgeList = new List<Edge>();

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
    }
}
