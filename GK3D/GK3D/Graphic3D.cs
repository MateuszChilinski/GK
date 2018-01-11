using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK3D
{
    static public class Graphic3D
    {
        public static Matrix4x4 Mmodel { get; set; } = new Matrix4x4(1, 0, 0, 0,
           0, 1, 0, 0,
           0, 0, 1, 0,
           0, 0, 0, 1);
        public static Matrix4x4 Mview { get; set; } = CreateViewAt(3.0, 0.5, 0.5, 0, 0.5, 0.5, 0, 0, 1);
        public static Matrix4x4 Mproj { get; set; } = new Matrix4x4(2.414213562F, 0, 0, 0,
                        0, 2.414213562F, 0, 0,
                        0, 0, -1.02020202F, -2.02020202F,
                        0, 0, -1, 0);
        public static int width { get; set; } = 0;
        public static int height { get; set; } = 0;
        static public Matrix4x4 CreateViewAt(double CPx, double CPy, double CPz, double CTx, double CTy, double CTz, double UPVx, double UPVy, double UPVz)
        {
            double zAxis_X = CPx - CTx;
            double zAxis_Y = CPy - CTy;
            double zAxis_Z = CPz - CTz;
            double nom = Math.Sqrt(zAxis_X * zAxis_X + zAxis_Y * zAxis_Y + zAxis_Z * zAxis_Z);
            double zAxis_XW = zAxis_X / nom;
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
        static public Vector4 CreatePoint(Vector4 point)
        {
            return CreatePoint(point, Mview, Mmodel, Mproj);
        }
        static public Vector4 CreatePoint(Vector4 point, Matrix4x4 MatrixView, Matrix4x4 MatrixModel, Matrix4x4 MatrixProj)
        {
            Vector4 pp = Normalize4(MultiplyV4(MatrixProj, MultiplyV4(MatrixView, MultiplyV4(MatrixModel, new Vector4(point.X, point.Y, point.Z, point.W)))));
            pp.X = (pp.X + 1) / 2 * width / 3 + 1.2F*width/3;
            pp.Y = (pp.Y + 1) / 2 * height /3 + height/3;
            pp.Z = (pp.Z + 1) / 2 * 100;
            return pp;
        }
        static public Vector4 CreatePoint3D(Vector4 point, Matrix4x4 Mmodel2)
        {
            Vector4 pp = Normalize4(MultiplyV4(Mmodel2, new Vector4(point.X, point.Y, point.Z, point.W)));
            return pp;
        }
        static public Vector4 Normalize4(Vector4 v)
        {
            return new Vector4(v.X / v.W, v.Y / v.W, v.Z / v.W, 1);
        }
        static public Vector4 MultiplyV4(Matrix4x4 m4, Vector4 v4)
        {
            Matrix4x4 nm = new Matrix4x4(v4.X, 0, 0, 0,
                v4.Y, 0, 0, 0,
                v4.Z, 0, 0, 0,
                v4.W, 0, 0, 0);
            Matrix4x4 mm = Matrix4x4.Multiply(m4, nm);
            return new Vector4(mm.M11, mm.M21, mm.M31, mm.M41);
        }

        internal static Vector4 CreatePoint(Vector4 p1, Matrix4x4 mmodel1)
        {
            return CreatePoint(p1, Mview, mmodel1, Mproj);
        }
    }
}
