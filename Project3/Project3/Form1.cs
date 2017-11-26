using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project3
{
    public partial class Form1 : Form
    {
        static Bitmap currentPicture,c1,c2,c3;
        static int currentConv = 0;
        public Form1()
        {
            currentPicture = new Bitmap("test.jpg");
            InitializeComponent();
            pictureBox1.Image = currentPicture;
            c1 = new Bitmap(currentPicture.Width, currentPicture.Height);
            c2 = new Bitmap(currentPicture.Width, currentPicture.Height);
            c3 = new Bitmap(currentPicture.Width, currentPicture.Height);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentPicture = new Bitmap(openFileDialog1.FileName);
                c1 = new Bitmap(currentPicture.Width, currentPicture.Height);
                c2 = new Bitmap(currentPicture.Width, currentPicture.Height);
                c3 = new Bitmap(currentPicture.Width, currentPicture.Height);
                pictureBox1.Image = currentPicture;
                pictureBox2.Image = c1;
                pictureBox3.Image = c2;
                pictureBox4.Image = c3;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        double[] RGB2YCBCR(byte R, byte G, byte B)
        {
            double[] ycbcr = new double[3];

            ycbcr[0] =  0.299 * R + 0.587 * G + 0.114 * B;
            ycbcr[1] =  128 - 0.168736 * R - 0.331264 * G + 0.5 * B;
            ycbcr[2] =  128 + 0.5 * R - 0.418688 * G - 0.081312 * B;
            return ycbcr;
        }
        double[] RGB2HSV(double Rr, double Gr, double Br)
        {
            double[] hsv = new double[3];
            double R = Rr / 255, G = Gr / 255, B = Br / 255;
            double Cmax = Math.Max(Math.Max(R, G), B);
            double Cmin = Math.Min(Math.Min(R, G), B);
            double D = Cmax - Cmin;
            if (D == 0)
            {
                hsv[0] = 0;
            }
            else if(R >= G && R >= B)
            {
                hsv[0] = 60 * ((G - B) / D % 6);
            }
            else if(G >= R && G >= B)
            {
                hsv[0] = 60 * ((B - R) / D + 2);
            }
            else if (B >= R && B >= G)
            {
                hsv[0] = 60 * ((R - G) / D + 4);
            }
            hsv[1] = (Cmax == 0) ? 0 : D / Cmax;
            hsv[2] = Cmax;
            return hsv;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (currentConv == 0)
                ycbcr();
            else if (currentConv == 1)
                hvs();
            else if (currentConv == 2)
                lab();
        }
        private void lab()
        {
            double[] lab;
            byte[] rgb;
            Color currentColor;
            for (int x = 0; x < currentPicture.Width; x++)
            {
                for (int y = 0; y < currentPicture.Height; y++)
                {
                    currentColor = currentPicture.GetPixel(x, y);
                    lab = sRGB2Lab(currentColor.R, currentColor.G, currentColor.B);
                    rgb = Lab2sRGB(lab[0], lab[1], lab[2]);
                    c1.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = Lab2sRGB(65, lab[1], 0);
                    c2.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = Lab2sRGB(65, 0, lab[2]);
                    c3.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                }
            }
            c1.Save("xd.bmp");
            pictureBox2.Image = c1;
            pictureBox3.Image = c2;
            pictureBox4.Image = c3;
        }
        private double[] sRGB2Lab(double r, double b, double g)
        {
            double[] xyz = sRGB2XYZ(r, g, b);
            return XYZ2Lab(xyz[0], xyz[1], xyz[2]);
        }
        private byte[] Lab2sRGB(double l, double a, double b)
        {
            double[] xyz = Lab2XYZ(l, a, b);
            return XYZ2sRGB(xyz[0], xyz[1], xyz[2]);
        }
        private double[] sRGB2XYZ(double r, double b, double g)
        {
            // linearize
            double R = r / 255.0, B = b / 255.0, G = g / 255.0;
            double[] sRGB = new double[3];
            if (R <= 0.04045)
                sRGB[0] = R / 12.92;
            else
                sRGB[0] = Math.Pow(((R + 0.055) / 1.055), 2.4);
            if (G <= 0.04045)
                sRGB[1] = G / 12.92;
            else
                sRGB[1] = Math.Pow(((G + 0.055) / 1.055), 2.4);
            if (B <= 0.04045)
                sRGB[2] = B / 12.92;
            else
                sRGB[2] = Math.Pow(((B + 0.055) / 1.055), 2.4);
            double[] xyz = new double[3];
            xyz[0] = 0.4124564 * sRGB[0] + 0.3575761 * sRGB[1] + 0.1804375 * sRGB[2];
            xyz[1] = 0.2126729 * sRGB[0] + 0.7151522 * sRGB[1] + 0.0721750 * sRGB[2];
            xyz[2] = 0.0193339 * sRGB[0] + 0.1191920 * sRGB[1] + 0.9503041 * sRGB[2];
            return xyz;
        }
        private double[] XYZ2Lab(double x, double y, double z)
        {
            double[] lab = new double[3];
            double Xr= 0.3127, Yr= 0.3290, Zr = 0.3582152;
            double e = 0.008856, k = 903.3;
            double zr = z / Zr, yr = y / Yr, xr = x / Xr;
            double fx, fy, fz;
            if(xr > e)
            {
                fx = Math.Pow(xr, 1.0 / 3.0);
            }
            else
            {
                fx = (k * xr + 16) / 116.0;
            }
            if (yr > e)
            {
                fy = Math.Pow(yr, 1.0 / 3.0);
            }
            else
            {
                fy = (k * yr + 16) / 116.0;
            }
            if (zr > e)
            {
                fz = Math.Pow(zr, 1.0 / 3.0);
            }
            else
            {
                fz = (k * zr + 16) / 116.0;
            }
            lab[0] = 116 * fy - 16;
            lab[1] = 500 * (fx-fy);
            lab[2] = 200 * (fy - fz);
            return lab;
        }
        private double[] Lab2XYZ(double L, double a, double b)
        {
            double[] xyz = new double[3];
            double Xr = 0.3127, Yr = 0.3290, Zr = 0.3582152;
            double e = 0.008856, k = 903.3;
            double zr, yr, xr;
            double fx, fy, fz;
            fy = (L + 16) / 116.0;
            fz = fy - b / 200.0;
            fx = a / 500.0 + fy;

            if (Math.Pow(fx, 3) > e)
            {
                xr = Math.Pow(fx, 3.0);
            }
            else
            {
                xr = (116*fx-16)/k;
            }
            if (L > e*k)
            {
                yr = Math.Pow((L+16)/116.0, 3.0);
            }
            else
            {
                yr = L / k;
            }
            if (Math.Pow(fz, 3) > e)
            {
                zr = Math.Pow(fz, 3.0);
            }
            else
            {
                zr = (116 * fz - 16) / k;
            }
            xyz[0] = xr * Xr;
            xyz[1] = yr * Yr;
            xyz[2] = zr * Zr;
            return xyz;
        }
        private byte[] XYZ2sRGB(double x, double y, double z)
        {
            // linearize
            double R,G,B;
            R = 3.2494542 * x - 1.5371385 * y - 0.4985314 * z;
            G = -0.9692660 * x + 1.8760108 * y + 0.0415560 * z;
            B = 0.0556434 * x - 0.2040259 * y + 1.0572252 * z;

            byte[] sRGB = new byte[3];
            if (R <= 0.0031308)
                sRGB[0] = (byte)(255 * (R * 12.92));
            else
                sRGB[0] = (byte)(255 * (Math.Pow(((R + 1.055) / 1.055), 1/2.4) - 0.055));
            if (G <= 0.0031308)
                sRGB[1] = (byte)(255 * (G * 12.92));
            else
                sRGB[1] = (byte)(255 * (Math.Pow(((G + 1.055) / 1.055), 1/2.4) - 0.055));
            if (B <= 0.0031308)
                sRGB[2] = (byte)(255 * (B * 12.92));
            else
                sRGB[2] = (byte)(255*(Math.Pow(((B + 1.055) / 1.055), 1/2.4) - 0.055));
            return sRGB;
        }
        private void hvs()
        {
            for (int x = 0; x < currentPicture.Width; x++)
            {
                for (int y = 0; y < currentPicture.Height; y++)
                {
                    Color currentColor = currentPicture.GetPixel(x, y);
                    double[] hsv = RGB2HSV(currentColor.R, currentColor.G, currentColor.B);
                    byte[] rgb = HSV2RGB(hsv[0], 1, 1);
                    c1.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = HSV2RGB(0, hsv[1], 1);
                    c2.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = HSV2RGB(0, 0, hsv[2]);
                    c3.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                }
            }
            pictureBox2.Image = c1;
            pictureBox3.Image = c2;
            pictureBox4.Image = c3;
        }
        private void ycbcr()
        {
            for (int x = 0; x < currentPicture.Width; x++)
            {
                for (int y = 0; y < currentPicture.Height; y++)
                {
                    Color currentColor = currentPicture.GetPixel(x, y);
                    double[] ycbcr = RGB2YCBCR(currentColor.R, currentColor.G, currentColor.B);
                    byte[] rgb = YCBCR2RGB(ycbcr[0], 128, 128);
                    c1.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = YCBCR2RGB(128, ycbcr[1], 128);
                    c2.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = YCBCR2RGB(128, 128, ycbcr[2]);
                    c3.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                }
            }
            pictureBox2.Image = c1;
            pictureBox3.Image = c2;
            pictureBox4.Image = c3;
        }
        private byte[] YCBCR2RGB(double y, double cb, double cr)
        {
            byte[] rgb = new byte[3];

            rgb[0] = (byte) (y + 1.402 * (cr-128));
            rgb[1] = (byte) (y - 0.344136 * (cb - 128) - 0.714136 * (cr-128));
            rgb[2] = (byte) (y + 1.772 * (cb - 128));
            return rgb;
        }
        private byte[] HSV2RGB(double h, double s, double v)
        {
            byte[] rgb = new byte[3];
            double C = v * s;
            double X = C * (1 - Math.Abs(((h / 60) % 2) - 1));
            double m = v - C;
            double R=0, G=0, B=0;
            if(0 <= h && h < 60)
            {
                R = C;
                G = X;
                B = 0;
            }
            else if(60 <= h && h < 120)
            {
                R = X;
                G = C;
                B = 0;
            }
            else if (120 <= h && h < 180)
            {
                R = 0;
                G = C;
                B = X;
            }
            else if (180 <= h && h < 240)
            {
                R = 0;
                G = X;
                B = C;
            }
            else if (240 <= h && h < 300)
            {
                R = X;
                G = 0;
                B = C;
            }
            else if (300 <= h && h < 360)
            {
                R = C;
                G = 0;
                B = X;
            }
            rgb[0] = (byte)((R+m)*255);
            rgb[1] = (byte)((G+m)*255);
            rgb[2] = (byte)((B+m)*255);
            return rgb;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentConv = ((ComboBox)sender).SelectedIndex;
        }
    }
}
