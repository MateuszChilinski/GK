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

namespace Project3
{
    public partial class Form1 : Form
    {
        static Bitmap currentPicture, c1, c2, c3;
        static int currentConv = 0;
        double[,] M, Mi;
        private bool notConstant = false;
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
        private double[,] generateM(bool inv = false)
        {
            double xr=0.64, xg=0.3, xb=0.15, yr=0.33, yg=0.6, yb=0.06,Wx=0.31273,Wy=0.32902,Wz=1-Wx-Wy;
            if (notConstant)
            {
                xr = (double) RPx.Value;
                xg = (double)GPx.Value;
                xb = (double)BPx.Value;
                yr = (double)RPy.Value;
                yg = (double)GPy.Value;
                yb = (double)BPy.Value;
                Wx = (double)WPx.Value;
                Wy = (double)WPy.Value;
                Wz = 1 - Wx - Wy;
            }
            double[,] Mm = { {xr, xg, xb },
            { yr, yg, yb },
            { 1-xr-yr, 1-xg-yg, 1-xb-yb }
            };
            Matrix<double> z = Matrix<double>.Build.DenseOfArray(Mm);
            double[,] V2 = { { Wx/Wy }, {Wy/Wy }, { Wz/Wy } };
            Matrix<double> v = Matrix<double>.Build.DenseOfArray(V2);
            z = z.Inverse().Multiply(v);
            double[,] M = z.ToArray();

            double[,] rM = new double[3,3];
            rM[0,0] = M[0, 0] * Mm[0,0];
            rM[1,0] = M[0, 0] * Mm[1,0];
            rM[2,0] = M[0, 0] * Mm[2, 0];
            rM[0,1] = M[1, 0] * Mm[0,1];
            rM[1,1] = M[1, 0] * Mm[1,1];
            rM[2,1] = M[1, 0] * Mm[2,1];
            rM[0,2] = M[2, 0] * Mm[0,2];
            rM[1,2] = M[2, 0] * Mm[1,2];
            rM[2,2] = M[2, 0] * Mm[2,2];
            z = Matrix<double>.Build.DenseOfArray(rM);
            if (inv)
                rM = z.Inverse().ToArray();
            return rM;
        }
        private double[,] generateMinv()
        {
            return generateM(true);
        }
        double[] RGB2YCBCR(byte R, byte G, byte B)
        {
            double[] ycbcr = new double[3];

            ycbcr[0] = 0.299 * R + 0.587 * G + 0.114 * B;
            ycbcr[1] = 128 - 0.168736 * R - 0.331264 * G + 0.5 * B;
            ycbcr[2] = 128 + 0.5 * R - 0.418688 * G - 0.081312 * B;
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
            else if (R >= G && R >= B)
            {
                hsv[0] = 60 * ((G - B) / D % 6);
            }
            else if (G >= R && G >= B)
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
            M = generateM();
            Mi = generateMinv();
            for (int x = 0; x < currentPicture.Width; x++)
            {
                for (int y = 0; y < currentPicture.Height; y++)
                {
                    currentColor = currentPicture.GetPixel(x, y);
                    lab = sRGB2Lab(currentColor.R, currentColor.G, currentColor.B);
                    rgb = Lab2sRGB(lab[0], 0, 0);
                    c1.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = Lab2sRGB(0, lab[1], 0);
                    c2.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                    rgb = Lab2sRGB(0, 0, lab[2]);
                    c3.SetPixel(x, y, Color.FromArgb(rgb[0], rgb[1], rgb[2]));
                }
            }
            c1.Save("xd.bmp");
            pictureBox2.Image = c1;
            pictureBox3.Image = c2;
            pictureBox4.Image = c3;
        }
        private double[] sRGB2Lab(double r, double g, double b)
        {
            double[] xyz = sRGB2XYZ(r, g, b);
            return XYZ2Lab(xyz[0], xyz[1], xyz[2]);
        }
        private byte[] Lab2sRGB(double l, double a, double b)
        {
            double[] xyz = Lab2XYZ(l, a, b);
            return XYZ2sRGB(xyz[0], xyz[1], xyz[2]);
        }
        private byte[] XYZ2sRGB(double x, double y, double z)
        {
            
            // linearize
            double R, G, B;
            double gamma = notConstant ? 1 / 2.2 : 1.0/(double) Gamma.Value;
            R = Math.Pow(Mi[0,0] * x + Mi[0,1] * y + Mi[0,2] * z, gamma);
            G = Math.Pow(Mi[1,0] * x + Mi[1,1] * y + Mi[1,2] * z, gamma);
            B = Math.Pow(Mi[2,0] * x + Mi[2,1] * y + Mi[2,2] * z, gamma);
            byte[] sRGB = new byte[3];
            sRGB[0] = (byte)Math.Round(R * 255.0);
            sRGB[1] = (byte)Math.Round(G * 255.0);
            sRGB[2] = (byte)Math.Round(B * 255.0);
            return sRGB;
        }
        private double[] sRGB2XYZ(double r, double g, double b)
        {
            // linearize
            double R = r / 255.0, B = b / 255.0, G = g / 255.0;
            double[] xyz = new double[3];
            double[] sRGB = new double[3];
            double gamma = notConstant ? 2.2 : (double) Gamma.Value;
            sRGB[0] = R;
            sRGB[1] = G;
            sRGB[2] = B;
            xyz[0] = Math.Pow(M[0, 0] * sRGB[0] + M[0,1] * sRGB[1] + M[0,2] * sRGB[2], gamma);
            xyz[1] = Math.Pow(M[1, 0] * sRGB[0] + M[1,1] * sRGB[1] + M[1,2] * sRGB[2], gamma);
            xyz[2] = Math.Pow(M[2, 0] * sRGB[0] + M[2, 1] * sRGB[1] + M[2, 2] * sRGB[2], gamma);
            return xyz;
        }
        private double[] XYZ2Lab(double x, double y, double z)
        {
            double[] lab = new double[3];
            double Xr = 94.81, Yr = 100, Zr = 107.3;
            double e = 0.008856, k = 903.3;
            double zr = z / Zr, yr = y / Yr, xr = x / Xr;
            double fx, fy, fz;
            if (xr > e)
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
            lab[0] = (116 * fy - 16);
            lab[1] = 500 * (fx - fy);
            lab[2] = 200 * (fy - fz);
            return lab;
        }
        private double[] Lab2XYZ(double L, double a, double b)
        {
            double[] xyz = new double[3];
            double Xr = 94.81, Yr = 100, Zr = 107.3;
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
                xr = (116 * fx - 16) / k;
            }
            if (L > e * k)
            {
                yr = Math.Pow((L + 16) / 116.0, 3.0);
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

        private void predefinedRadio_CheckedChanged(object sender, EventArgs e)
        {
            notConstant = false;
            comboBox3.Enabled = true;
            comboBox2.Enabled = true;
            RPx.Enabled = false;
            WPy.Enabled = false;
            GPy.Enabled = false;
            GPx.Enabled = false;
            WPx.Enabled = false;
            Gamma.Enabled = false;
            RPy.Enabled = false;
            BPy.Enabled = false;
            BPx.Enabled = false;
        }

        private void calculateRadio_CheckedChanged(object sender, EventArgs e)
        {
            notConstant = true;
            RPx.Enabled = true;
            WPy.Enabled = true;
            GPy.Enabled = true;
            WPx.Enabled = true;
            GPx.Enabled = true;
            Gamma.Enabled = true;
            RPy.Enabled = true;
            BPy.Enabled = true;
            BPx.Enabled = true;
            comboBox3.Enabled = false;
            comboBox2.Enabled = false;
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

            rgb[0] = (byte)(y + 1.402 * (cr - 128));
            rgb[1] = (byte)(y - 0.344136 * (cb - 128) - 0.714136 * (cr - 128));
            rgb[2] = (byte)(y + 1.772 * (cb - 128));
            return rgb;
        }
        private byte[] HSV2RGB(double h, double s, double v)
        {
            byte[] rgb = new byte[3];
            double C = v * s;
            double X = C * (1 - Math.Abs(((h / 60) % 2) - 1));
            double m = v - C;
            double R = 0, G = 0, B = 0;
            if (0 <= h && h < 60)
            {
                R = C;
                G = X;
                B = 0;
            }
            else if (60 <= h && h < 120)
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
            rgb[0] = (byte)((R + m) * 255);
            rgb[1] = (byte)((G + m) * 255);
            rgb[2] = (byte)((B + m) * 255);
            return rgb;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentConv = ((ComboBox)sender).SelectedIndex;
        }
    }
}
