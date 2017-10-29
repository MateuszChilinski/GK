using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    abstract class Restriction
    {
        abstract public void drawIcon(Graphics graphics, PointF drawPoint);
        abstract public void movePoint(MyPoint p1, MyPoint p2, ref int dX, ref int dY);
    }
    class VerticalRestriction : Restriction
    {
        public override void drawIcon(Graphics graphics, PointF drawPoint)
        {
            Font drawFont = new Font("Arial", 6);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            graphics.DrawString("V", drawFont, drawBrush, drawPoint);
        }

        public override void movePoint(MyPoint p1, MyPoint p2, ref int dX, ref int dY)
        {
            p2.X = p1.X;
        }
    }
    class HorizontalRestriction : Restriction
    {
        public override void drawIcon(Graphics graphics, PointF drawPoint)
        {
            Font drawFont = new Font("Arial", 6);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            graphics.DrawString("H", drawFont, drawBrush, drawPoint);
        }
        public override void movePoint(MyPoint p1, MyPoint p2, ref int dX, ref int dY)
        {
            p2.Y = p1.Y;
        }
    }
    class LengthRestriction : Restriction
    {
        double maxLength=0;
        public LengthRestriction(double length)
        {
            maxLength = length;
        }
        public override void drawIcon(Graphics graphics, PointF drawPoint)
        {
            Font drawFont = new Font("Arial", 6);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            graphics.DrawString("L="+maxLength, drawFont, drawBrush, drawPoint);
        }
        private double calculateLength(MyPoint p1, MyPoint p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        public override void movePoint(MyPoint p1, MyPoint p2, ref int dX, ref int dY)
        {
            if(Math.Abs(calculateLength(p1, p2) - maxLength) > 1)
            {
                p2.setCoords(p2.X + dX, p2.Y + dY);
                if (Math.Abs(calculateLength(p1, p2) - maxLength) > 1)
                {
                    if (p2.Y < p1.Y)
                        p2.setCoords(p1.X, p1.Y - (int)(maxLength));
                    else
                        p2.setCoords(p1.X, p1.Y + (int)(maxLength));
                }
            }
            else
            {
                dX = 0;
                dY = 0;
            }
        }
    }
}
