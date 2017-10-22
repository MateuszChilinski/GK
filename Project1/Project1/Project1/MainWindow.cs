using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{

    public partial class MainWindow : Form
    {
        List<MyPoint> points = new List<MyPoint>();
        HashSet<Tuple<Tuple<MyPoint, MyPoint>, Restriction>> restrictions = new HashSet<Tuple<Tuple<MyPoint, MyPoint>, Restriction>>(); // todo types
        static int radius = 5;
        static bool closed = false;
        static int pointInUse;
        static MyPoint middlePoint = new MyPoint(0,0);
        static MyPoint mainPoint = new MyPoint(0,0);
        static bool moved = false;
        public MainWindow()
        {
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
        private Restriction FindRestriction(MyPoint p1, MyPoint p2) 
        {
            foreach(var t in restrictions)
            {
                if((t.Item1.Item1 == p1 && t.Item1.Item2 == p2) || (t.Item1.Item1 == p2 && t.Item1.Item2 == p1))
                {
                    return t.Item2;
                }
            }
            return null;
        }
        private void ValidatePoints(int firstPoint, int p1, int p2, int dX, int dY, bool add)
        {
            Restriction restriction;
            if (p2 == points.Count)
            {
                p2 = 0;
            }
            restriction = FindRestriction(points[p1], points[p2]);
            if (restriction == null)
                return;
            restriction.movePoint(points[p1], points[p2], ref dX, ref dY);
            if (p2 != firstPoint)
            {
                if (add)
                    ValidatePoints(firstPoint, p2, p2 + 1, dX, dY, add);
                else
                {
                    if(p2 - 1 != -1)
                    {
                        ValidatePoints(firstPoint, p2, p2 - 1, dX, dY, add);
                    }
                    else
                        ValidatePoints(firstPoint, p2, points.Count-1, dX, dY, add);
                }
            }
            moved = true;
            mainPictureBox.Refresh();
        }
        private void MainPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int dX = e.X - points[pointInUse].X;
            int dY = e.Y - points[pointInUse].Y;
            points[pointInUse].X = e.X;
            points[pointInUse].Y = e.Y;
            int secondPoint = pointInUse + 1;
            if (pointInUse == points.Count - 1)
                secondPoint = 0;
            ValidatePoints(pointInUse, pointInUse, secondPoint, dX, dY, true);
            secondPoint = pointInUse - 1;
            if (pointInUse == 0)
                secondPoint = points.Count-1;
            ValidatePoints(pointInUse, pointInUse, secondPoint, dX, dY, false);
        }
        private void MainPictureBox_MouseMovePoly(object sender, MouseEventArgs e)
        {
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
            foreach (MyPoint p in points)
            {
                e.Graphics.FillEllipse(new SolidBrush(Color.Red), p.X- radius, p.Y- radius, 2*radius, 2*radius);
            }
            for(int i = 0; i < points.Count - 1; i++)
            {
                if (points.Count <= 1)
                    break;
                DrawLine(points[i], points[i + 1], e);
            }
            if (closed)
            {
                DrawLine(points[points.Count - 1], points[0], e);
                int x = 0, y = 0;
                foreach (MyPoint p in points)
                {
                    x += p.X;
                    y += p.Y;
                }
                x = x / points.Count;
                y = y / points.Count;
                mainPoint.setCoords(x, y);
                e.Graphics.FillEllipse(new SolidBrush(Color.Green), mainPoint.X - radius, mainPoint.Y - radius, 2 * radius, 2 * radius);
            }
            mainPictureBox.Invalidate();
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
        private void DrawLine(MyPoint p1, MyPoint p2, PaintEventArgs e)
        {
            PointF drawPoint = new PointF(Math.Abs(p1.X - p2.X) / 2 + 5 + Math.Min(p1.X, p2.X), Math.Abs(p2.Y - p1.Y) / 2 + 5 + Math.Min(p1.Y, p2.Y));
            FindRestriction(p1, p2)?.drawIcon(e.Graphics, drawPoint);
            e.Graphics.DrawLine(new Pen(Color.White, 2F), p1.X, p1.Y, p2.X, p2.Y);
        }

        private void deletePointStripMenuItem_Click(object sender, EventArgs e)
        {
            removeNeighbourRelations(pointInUse);
            points.RemoveAt(pointInUse);
            if (points.Count <= 2)
                closed = false;
        }
        private void removeNeighbourRelations(int p)
        {
            if (points.Count <= 1)
                return;
            if (p == 0)
            {
                removeRelation(points.Count - 1, 0);
                removeRelation(0, 1);
            }
            else if (p == points.Count - 1)
            {
                removeRelation(points.Count - 1, 0);
                removeRelation(points.Count - 1, points.Count - 2);
            }
            else
            {
                removeRelation(p, p + 1);
                removeRelation(p - 1, p);
            }
        }
        private bool checkRestriction(Tuple<Tuple<MyPoint, MyPoint>, Restriction> tp, int p1, int p2)
        {
            if ((tp.Item1.Item1 == points[p1] && tp.Item1.Item2 == points[p2]) || (tp.Item1.Item1 == points[p2] && tp.Item1.Item2 == points[p1]))
                return true;
            return false;
        }
        private void removeRelation(int p1, int p2)
        {
            restrictions.RemoveWhere((v) => { return checkRestriction(v, p1, p2); });
        }
        private void addPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(pointInUse != points.Count-1)
                removeRelation(pointInUse, pointInUse + 1);
            else
                removeRelation(pointInUse, 0);
            points.Insert(pointInUse+1, middlePoint);
            middlePoint = new MyPoint(0,0);
        }

        private void removeLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secondPoint = (points.Count - 1 == pointInUse) ? 0 : pointInUse + 1;
            for(int i = 0; i < restrictions.Count; i++)
            {
                var t = restrictions.ElementAt(i);
                if ((t.Item1.Item1 == points[pointInUse] && t.Item1.Item2 == points[secondPoint]) || (t.Item1.Item1 == points[secondPoint] && t.Item1.Item2 == points[pointInUse]))
                {
                    restrictions.Remove(t);
                }
            }
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
        private void lengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double maxLength = Prompt.ShowDialog(50, "Input the text");
            int secondPoint = (pointInUse == points.Count - 1) ? 0 : pointInUse + 1;
            restrictions.Add(new Tuple<Tuple<MyPoint, MyPoint>, Restriction>(new Tuple<MyPoint, MyPoint>(points[pointInUse], points[secondPoint]), new LengthRestriction(maxLength)));
            if(calculateLength(points[pointInUse], points[secondPoint]) > maxLength)
            {
                if(points[secondPoint].Y < points[pointInUse].Y)
                    points[secondPoint].setCoords(points[pointInUse].X, points[pointInUse].Y-(int)(maxLength));
                else
                    points[secondPoint].setCoords(points[pointInUse].X, points[pointInUse].Y + (int)(maxLength));
            }
        }
        private bool checkNeighbours(int p1, int p2, Type restriction)
        {
            int tmp;
            if(p1 > p2)
            {
                tmp = p2;
                p2 = p1;
                p1 = tmp;
            }
            if (FindRestriction(points[p1], points[(p1 - 1 >= 0) ? p1 -1 : points.Count-1])?.GetType() == restriction)
                return false;
            if (FindRestriction(points[p2], points[(p2 + 1 <= points.Count - 1) ? p2 + 1 : 0])?.GetType() == restriction)
                return false;

            return true;
        }
        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secondPoint = (pointInUse == points.Count - 1) ? 0 : pointInUse + 1;
            if (checkNeighbours(pointInUse, secondPoint, typeof(VerticalRestriction)))
            {
                points[secondPoint].setCoords(points[pointInUse].X, points[secondPoint].Y);
                restrictions.Add(new Tuple<Tuple<MyPoint, MyPoint>, Restriction>(new Tuple<MyPoint, MyPoint>(points[pointInUse], points[secondPoint]), new VerticalRestriction()));
            }
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int secondPoint = (pointInUse == points.Count - 1) ? 0 : pointInUse + 1;
            if (checkNeighbours(pointInUse, secondPoint, typeof(HorizontalRestriction)))
            {
                points[secondPoint].setCoords(points[secondPoint].X, points[pointInUse].Y);
                restrictions.Add(new Tuple<Tuple<MyPoint, MyPoint>, Restriction>(new Tuple<MyPoint, MyPoint>(points[pointInUse], points[secondPoint]), new HorizontalRestriction()));
            }
        }
    }
    class MyPoint
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

}
