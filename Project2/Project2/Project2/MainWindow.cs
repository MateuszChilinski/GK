using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project2
{

    public partial class MainWindow : Form
    {
        List<MyPoint> points = new List<MyPoint>();
        List<MyPoint> points2 = new List<MyPoint>();
        static int radius = 5;
        static bool closed = false;
        static int pointInUse;
        static MyPoint middlePoint = new MyPoint(0,0);
        static MyPoint mainPoint = new MyPoint(0,0);
        static bool moved = false;
        static bool usedTexture = false;
        static bool normalTextureUsed = false;
        static bool dTextureUsed = false;
        static Color chosenColor = Color.Orange;
        static Bitmap chosenTexture;
        static Bitmap nChosenTexture;
        static Bitmap dChosenTexture;
        public MainWindow()
        {
            points2.Add(new MyPoint(30, 30));
            points2.Add(new MyPoint(300, 70));
            points2.Add(new MyPoint(250, 95));
            points2.Add(new MyPoint(100, 100));
            InitializeComponent();
            mainPictureBox.Paint += MainPictureBox_Paint;
            mainPictureBox.MouseClick += MainPictureBox_Click;
            mainPictureBox.MouseDown += MainPictureBox_MouseDown;
            mainPictureBox.MouseUp += MainPictureBox_MouseUp;
        }

        private void MainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            mainPictureBox.MouseMove -= MainPictureBox_MouseMove;
            mainPictureBox.MouseMove -= MainPictureBox_MouseMovePoly;
        }

        private void MainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                {
                    return;
                }
                case MouseButtons.Left:
                {
                    moved = false;
                    if(CheckPoint(mainPoint, e.X, e.Y))
                    {
                            mainPictureBox.MouseMove += MainPictureBox_MouseMovePoly;
                            return;
                    }
                    MyPoint pointUsed = points.Find((MyPoint p) => { return CheckPoint(p, e.X, e.Y); });
                    if (pointUsed != default(MyPoint))
                    {
                        mainPictureBox.MouseMove += MainPictureBox_MouseMove;
                        pointInUse = points.IndexOf(pointUsed);
                    }
                }
                break;
            }
        }
        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.X < mainPictureBox.Width && e.X > 0)
                points[pointInUse].X = e.X;
            if(e.Y < mainPictureBox.Height && e.Y > 0)
                points[pointInUse].Y = e.Y;
        }
        private void MainPictureBox_MouseMovePoly(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < points.Count; i++)
            {
                MyPoint p = points[i];
                if (p.X + (e.X - mainPoint.X) < 0 || p.X + (e.X - mainPoint.X) > mainPictureBox.Width-1 || p.Y + (e.Y - mainPoint.Y) < 0 || p.Y + (e.Y - mainPoint.Y) > mainPictureBox.Height-1)
                    return;
            }
            for (int i = 0; i < points.Count; i++)
            {
                MyPoint p = points[i];
                points[points.IndexOf(p)].setCoords(p.X + (e.X - mainPoint.X), p.Y + (e.Y - mainPoint.Y));
            }
            mainPoint.setCoords(e.X, e.Y);
            moved = true;
            mainPictureBox.Refresh();
        }
        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            Bitmap Bmp = new Bitmap(mainPictureBox.Width, mainPictureBox.Height);
            using (Graphics gfx = Graphics.FromImage(Bmp))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255)))
            {
                gfx.FillRectangle(brush, 0, 0, mainPictureBox.Width, mainPictureBox.Height);
            }
            mainPictureBox.Image = new Bitmap(Bmp);
            foreach (MyPoint p in points)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), p.X- radius, p.Y- radius, 2*radius, 2*radius);
            }
            foreach (MyPoint p in points2)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Blue), p.X - radius, p.Y - radius, 2 * radius, 2 * radius);
            }
            for (int i = 0; i < points2.Count - 1; i++)
            {
                DrawLine(points2[i], points2[i + 1], e, true);
            }
            DrawLine(points2[points2.Count - 1], points2[0], e, true);
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (points.Count <= 1)
                    break;
                DrawLine(points[i], points[i + 1], e, false);
            }
            if (closed)
            {
                DrawLine(points[points.Count - 1], points[0], e, false);
                int x = 0, y = 0;
                foreach (MyPoint p in points)
                {
                    x += p.X;
                    y += p.Y;
                }
                x = x / points.Count;
                y = y / points.Count;
                FillPolygon(e);
                mainPoint.setCoords(x, y);
                e.Graphics.FillEllipse(new SolidBrush(Color.Green), mainPoint.X - radius, mainPoint.Y - radius, 2 * radius, 2 * radius);

            }
            mainPictureBox.Invalidate();
        }
        private void FillPolygon(PaintEventArgs e)
        {
            List<Edge>[] EA = new List<Edge>[mainPictureBox.Height];
            for(int i = 0; i < mainPictureBox.Height; i++)
            {
                EA[i] = new List<Edge>();
            }
            Edge tmpEdge;
            for (int i = 0; i < points.Count - 1; i++)
            {
                tmpEdge = new Edge(points[i], points[i + 1]);
                EA[Math.Max(points[i].Y, points[i + 1].Y)].Add(tmpEdge);
            }
            tmpEdge = new Edge(points[0], points[points.Count-1]);
            EA[Math.Max(points[0].Y, points[points.Count-1].Y)].Add(tmpEdge);
            List<Edge> AET = new List<Edge>();
            for(int y = mainPictureBox.Height - 1; y >= 0; y--)
            {
                foreach (var z in EA[y])
                    AET.Add(z);
                AET = AET.OrderBy(edge => edge.x).ToList();
                List<int> toRemove = new List<int>();
                for(int i = 0; i < AET.Count - 1; i+=2)
                {
                    DrawLine((int) AET[i].x, (int) AET[i + 1].x, y, e, false);
                }
                for(int i = 0; i < AET.Count; i++)
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
                toRemove = toRemove.OrderByDescending(i => i).ToList();
                foreach (int z in toRemove)
                    AET.RemoveAt(z);

            }
        }
        public static bool IsSameSide(MyPoint p1, MyPoint p2, Segment s)
        {
            double result1 = (s.pe.X - s.ps.X) * (p1.Y - s.ps.Y) - (p1.X - s.ps.X) * (s.pe.Y - s.ps.Y);
            double result2 = (s.pe.X - s.ps.X) * (p2.Y - s.ps.Y) - (p2.X - s.ps.X) * (s.pe.Y - s.ps.Y);
            if (result1 * result2 >= 0)
                return true;
            return false;
        }
        public static MyPoint[] GetIntersectedPolygon(MyPoint[] subjectPolygon, MyPoint[] clipPolygon)
        {
            List<MyPoint> input = new List<MyPoint>();
            List<MyPoint> output = new List<MyPoint>(subjectPolygon);
            MyPoint inP = new MyPoint((int) ((clipPolygon[0].X + clipPolygon[1].X + clipPolygon[2].X) / 3.0), (int) ((clipPolygon[0].Y + clipPolygon[1].Y + clipPolygon[2].Y) / 3.0));
            for (int i = 0; i < clipPolygon.Length; i++)
            {
                input = new List<MyPoint>(output);
                output.Clear();
                MyPoint pp = input[input.Count - 1];
                foreach (MyPoint p in input)
                {
                    Segment e = new Segment(clipPolygon[i], clipPolygon[(i + 1) % clipPolygon.Length]);
                    if (IsSameSide(p, inP, e))
                    {
                        if (!IsSameSide(pp, inP, e))
                            output.Add(GetIntersectionPoint(new Segment(pp, p), e));
                        output.Add(p);
                    }
                    else
                    {
                        if (IsSameSide(pp, inP, e))
                            output.Add(GetIntersectionPoint(new Segment(pp, p), e));
                    }
                    pp = p;
                }
            }
            List<int> toRemove = new List<int>();
            for(int i = 0; i < output.Count; i++)
            {
                for(int j = i+1; j < output.Count; j++)
                {
                    if (i == j)
                        continue;
                    if(Math.Abs(output[i].X - output[j].X) < 2 && Math.Abs(output[i].Y - output[j].Y) < 2)
                    {
                        toRemove.Add(j);
                    }
                }
            }
            toRemove = toRemove.OrderByDescending(i => i).ToList();
            foreach (int i in toRemove)
                output.RemoveAt(i);
            return output.Distinct().ToArray();
        }
        public static MyPoint GetIntersectionPoint(Segment seg1, Segment seg2)
        {
            MyPoint direction1 = new MyPoint(seg1.pe.X - seg1.ps.X, seg1.pe.Y - seg1.ps.Y);
            MyPoint direction2 = new MyPoint(seg2.pe.X - seg2.ps.X, seg2.pe.Y - seg2.ps.Y);
            double dotPerp = (direction1.X * direction2.Y) - (direction1.Y * direction2.X);

            MyPoint c = new MyPoint(seg2.ps.X - seg1.ps.X, seg2.ps.Y - seg1.ps.Y);
            double t = (c.X * direction2.Y - c.Y * direction2.X) / dotPerp;

            return new MyPoint((int) (seg1.ps.X + (t * direction1.X)), (int) (seg1.ps.Y + (t * direction1.Y)));
        }
        private bool CheckPoint(MyPoint p, double X, double Y)
        {
            if (p == null) return false;
            if ((p.X - radius * 2 > X || p.X + radius * 2 < X) || (p.Y - radius * 2 > Y || p.Y + radius * 2 < Y))
            {
                return false;
            }
            return true;
        }
        private bool CheckLine(MyPoint p1, MyPoint p2, double X, double Y)
        {
            if (p1.X > p2.X)
            {
                MyPoint tmp = p2;
                p2 = p1;
                p1 = tmp;
            }
            int y;
            for(int x = p1.X; x < p2.X; x++)
            {
                y = (int) ((p1.Y - p2.Y)*1.0 / ((p1.X - p2.X)*1.0) * x + p1.Y - ((p1.Y - p2.Y)*1.0 / ((p1.X - p2.X)*1.0)) * p1.X);
                if ((x - radius * 2 < X && x + radius * 2 > X) && (y - radius * 2 < Y && y + radius * 2 > Y))
                {
                    int y1, y2;
                    if(p2.Y > p1.Y)
                    {
                        y2 = p2.Y;
                        y1 = p1.Y;
                    }
                    else
                    {
                        y2 = p1.Y;
                        y1 = p2.Y;
                    }
                    middlePoint.setCoords(p1.X + (p2.X - p1.X) / 2, y1 + (y2 - y1) / 2);
                    
                    return true;
                }
            }
            if (p1.Y > p2.Y)
            {
                MyPoint tmp = p2;
                p2 = p1;
                p1 = tmp;
            }
            for (y = p1.Y; y < p2.Y; y++)
            {
                int x = p1.X;
                if ((x - radius * 2 < X && x + radius * 2 > X) && (y - radius * 2 < Y && y + radius * 2 > Y))
                {
                    int y1, y2;
                    if (p2.Y > p1.Y)
                    {
                        y2 = p2.Y;
                        y1 = p1.Y;
                    }
                    else
                    {
                        y2 = p1.Y;
                        y1 = p2.Y;
                    }
                    middlePoint.setCoords(p1.X + (p2.X - p1.X) / 2, y1 + (y2 - y1) / 2);

                    return true;
                }
            }
            return false;
        }

        private void MainPictureBox_Click(object sender, MouseEventArgs e)
        {
            MyPoint pointFound = points.FindLast((MyPoint p) => { return CheckPoint(p, e.X, e.Y); });
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        if (points.Count == 0)
                            return;
                        if(pointFound != default(MyPoint))
                        {
                            pointInUse = points.IndexOf(pointFound);
                            pointMenuStrip.Show(Cursor.Position);
                        }
                        else
                        {
                            bool found = false;
                            for (int i = 0; i < points.Count - 1; i++)
                            {
                                if(CheckLine(points[i], points[i + 1], e.X, e.Y))
                                {
                                    pointInUse = i;
                                    found = true;
                                }
                            }
                            if (CheckLine(points[points.Count - 1], points[0], e.X, e.Y))
                            {
                                pointInUse = points.Count - 1;
                                lineMenuStrip.Show(Cursor.Position);
                            }
                            if(found)
                                lineMenuStrip.Show(Cursor.Position);
                        }
                    }
                    break;
                case MouseButtons.Left:
                    {
                        if (closed)
                            return;
                        if (pointFound == default(MyPoint))
                            points.Add(new MyPoint(e.X, e.Y));
                        else if (pointFound == points[0] && !moved)
                        {
                            closed = true;

                        }
                    }
                    break;
            }
        }
        private void DrawLine(int x1, int x2, int y, PaintEventArgs e, bool second)
        {
            DrawLine(new MyPoint(x1, y), new MyPoint(x2, y), e, second);
        }
        private void DrawLine(MyPoint p1, MyPoint p2, PaintEventArgs e, bool second)
        {
            PointF drawPoint = new PointF(Math.Abs(p1.X - p2.X) / 2 + 5 + Math.Min(p1.X, p2.X), Math.Abs(p2.Y - p1.Y) / 2 + 5 + Math.Min(p1.Y, p2.Y));

            int x = p1.X;
            int y = p1.Y;
            int x2 = p2.X;
            int y2 = p2.Y;
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                double[] lightVector = new double[3];
                lightVector[0] = 0;
                lightVector[1] = 0;
                lightVector[2] = 1;

                double[] lightColor = new double[3];
                lightColor[0] = 1;
                lightColor[1] = 1;
                lightColor[2] = 1;

                Color pixelcolor;
                if(!usedTexture || second)
                    pixelcolor = chosenColor;
                else
                    pixelcolor = chosenTexture.GetPixel(x % (chosenTexture.Width-1), y % (chosenTexture.Height-1));
                byte R = pixelcolor.R;
                byte G = pixelcolor.G;
                byte B = pixelcolor.B;

                double NR;
                double NG;
                double NB;

                if (!normalTextureUsed)
                {
                    NR = 0;
                    NG = 0;
                    NB = 1;
                }
                else
                {
                    double normal = Math.Sqrt(nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).B*nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).B+
                        nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).G * nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).G+
                        nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).R * nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).R);
                    NR = (nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).R)/normal;
                    NG = (nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).G) / normal;
                    NB = (nChosenTexture.GetPixel(x % nChosenTexture.Width, y % nChosenTexture.Height).B) / normal;
                }
                double DR=0, DG=0, DB=0;
                if(dTextureUsed)
                {
                    double f = 0.02;
                    DR = dChosenTexture.GetPixel((x + 1) % dChosenTexture.Width, y % dChosenTexture.Height).R - dChosenTexture.GetPixel(x % dChosenTexture.Width, y % dChosenTexture.Height).R;
                    DG = dChosenTexture.GetPixel(x % dChosenTexture.Width, (y + 1) % dChosenTexture.Height).R - dChosenTexture.GetPixel(x % dChosenTexture.Width, y % dChosenTexture.Height).R;
                    DB = -NR*(dChosenTexture.GetPixel((x + 1) % dChosenTexture.Width, y % dChosenTexture.Height).R - dChosenTexture.GetPixel(x % dChosenTexture.Width, y % dChosenTexture.Height).R) 
                        + (-NG)*(dChosenTexture.GetPixel(x % dChosenTexture.Width, (y + 1) % dChosenTexture.Height).R - dChosenTexture.GetPixel(x % dChosenTexture.Width, y % dChosenTexture.Height).R);
                    DR = f * DR;
                    DG = f * DG;
                    DB = f * DB;
                }
                double NprR = NR + DR;
                double NprG = NG + DG;
                double NprB = NB + DB;

                double normal2 = Math.Sqrt(NprR* NprR+ NprG* NprG+ NprB* NprB);
                NprR = NprR / normal2;
                NprG = NprG / normal2;
                NprB = NprB / normal2;

                double cos = Math.Abs(NprR * lightVector[0] + NprG * lightVector[1] + NprB * lightVector[2]);

                R = (byte) (R * (lightColor[0] * cos));
                G = (byte) (G * (lightColor[1] * cos));
                B = (byte) (B * (lightColor[2] * cos));

                ((Bitmap)mainPictureBox.Image).SetPixel(x, y, Color.FromArgb(R,G,B));
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        private void deletePointStripMenuItem_Click(object sender, EventArgs e)
        {
            points.RemoveAt(pointInUse);
            if (points.Count <= 2)
                closed = false;
        }
        private void addPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            points.Insert(pointInUse+1, middlePoint);
            middlePoint = new MyPoint(0,0);
        }

        private void removeLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secondPoint = (points.Count - 1 == pointInUse) ? 0 : pointInUse + 1;
            if (pointInUse == points.Count - 1)
            {
                points.RemoveAt(pointInUse);
                points.RemoveAt(0);
            }
            else
            {
                points.RemoveAt(pointInUse);
                points.RemoveAt(pointInUse);
            }
            if (points.Count <= 2)
                closed = false;
        }
        private double calculateLength(MyPoint p1, MyPoint p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainPictureBox.Paint -= MainPictureBox_Paint;
            DialogResult result = colorDialog1.ShowDialog();
            
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                chosenColor = colorDialog1.Color;
            }
            mainPictureBox.Paint += MainPictureBox_Paint;
            mainPictureBox.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            points = GetIntersectedPolygon(points.ToArray(), points2.ToArray()).ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mainPictureBox.Paint -= MainPictureBox_Paint;
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                chosenTexture = new Bitmap(openFileDialog1.FileName);
            }
            mainPictureBox.Paint += MainPictureBox_Paint;
            mainPictureBox.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mainPictureBox.Paint -= MainPictureBox_Paint;
            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                nChosenTexture = new Bitmap(openFileDialog2.FileName);
            }
            mainPictureBox.Paint += MainPictureBox_Paint;
            mainPictureBox.Invalidate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mainPictureBox.Paint -= MainPictureBox_Paint;
            if (openFileDialog3.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dChosenTexture = new Bitmap(openFileDialog3.FileName);
            }
            mainPictureBox.Paint += MainPictureBox_Paint;
            mainPictureBox.Invalidate();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(chosenTexture != null)
                usedTexture = true;
            else
                radioButton1.Checked = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            usedTexture = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (nChosenTexture != null)
                normalTextureUsed = true;
            else
                radioButton3.Checked = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            normalTextureUsed = false;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (dChosenTexture != null)
                dTextureUsed = true;
            else
                radioButton7.Checked = true;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            dTextureUsed = false;
        }
    }
    public class MyPoint
    {
        Point realPoint;
        public MyPoint(int x, int y)
        {
            realPoint = new Point(x, y);
        }
        public MyPoint(Point p)
        {
            realPoint = new Point(p.X, p.Y);
        }
        public void setCoords(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get { return realPoint.X; } set { realPoint = new Point(value, realPoint.Y); } }
        public int Y { get { return realPoint.Y; } set { realPoint = new Point(realPoint.X, value); } }
    }
    class Edge
    {
        MyPoint p1;
        MyPoint p2;

        public Edge(MyPoint myPoint1, MyPoint myPoint2)
        {
            p1 = myPoint1;
            p2 = myPoint2;
            if (m < 0)
                x = Math.Min(p1.X, p2.X);
            else
                x = Math.Max(p1.X, p2.X);
        }

        public int yMax { get { return Math.Min(p1.Y, p2.Y)+1; } }
        public double x { get; set; }
        public double m { get { if (p1.Y == p2.Y) return 1000; return  (double)(p1.X - p2.X)/(double)(p1.Y - p2.Y); } }
    }
    public struct Segment
    {

        public MyPoint ps;  // poczatek odcinka
        public MyPoint pe;  // koniec odcinka

        public Segment(MyPoint pps, MyPoint ppe) { ps = pps; pe = ppe; }
        

    }
}
