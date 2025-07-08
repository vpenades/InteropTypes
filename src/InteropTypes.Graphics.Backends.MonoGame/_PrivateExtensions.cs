using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    static class _PrivateExtensions
    {
        public static XNACOLOR ToXna(this ColorStyle c) { return new XNACOLOR(c.R, c.G, c.B, c.A); }

        public static XNACOLOR ToXna(this COLOR c) { return new XNACOLOR(c.R, c.G, c.B, c.A); }

        public static XNACOLOR ToXnaPremul(this COLOR c) { return XNACOLOR.FromNonPremultiplied(c.R, c.G, c.B, c.A); }

        public static XNACOLOR ToXnaPremul(this XNACOLOR c) { return XNACOLOR.FromNonPremultiplied(c.R, c.G, c.B, c.A); }

        public static XNAMATRIX ToXna(this in Matrix3x2 m)
        {
            return new XNAMATRIX
                (
                m.M11, m.M12, 0, 0,
                m.M21, m.M22, 0, 0,
                0, 0, 1, 0,
                m.M31, m.M32, 0, 1);
        }

        public static XNAMATRIX ToXNA(this in Matrix4x4 m)
        {
            return new XNAMATRIX
                (
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
                );
        }

        public static XNAV2 ToXna(this in XY value)
        {
            return new XNAV2(value.X, value.Y);
        }

        public static XNAV3 ToXna(this in XYZ value)
        {
            return new XNAV3(value.X, value.Y, value.Z);
        }

        public static XNAV3 ToXna(this in POINT3 value)
        {
            return new XNAV3(value.X, value.Y, value.Z);
        }

        public static Matrix3x2 CreateVirtualToPhysical(this GraphicsDevice device, (float width, float height) virtualSize, bool keepAspect)
        {
            var camera = CameraTransform2D.Create(Matrix3x2.Identity, virtualSize, keepAspect);

            var physicalSize = new XY(device.Viewport.Width, device.Viewport.Height);

            return camera.CreateViewportMatrix(physicalSize);            
        }        

        public static float DecomposeScale(this in Matrix3x2 xform)
        {
            var det = xform.GetDeterminant();
            var area = Math.Abs(det);
            return (float)Math.Sqrt(area);
        }

        public static XY GetScale(this in Matrix3x2 matrix)
        {
            var sx = matrix.M12 == 0 ? Math.Abs(matrix.M11) : new XY(matrix.M11, matrix.M12).Length();
            var sy = matrix.M21 == 0 ? Math.Abs(matrix.M22) : new XY(matrix.M21, matrix.M22).Length();
            if (matrix.GetDeterminant() < 0) sy = -sy;
            return new XY(sx, sy);
        }

        public static float GetRotation(this in Matrix3x2 matrix)
        {
            return (float)Math.Atan2(matrix.M12, matrix.M11);
        }

        public static void Decompose(this Matrix3x2 matrix, out XY scale, out float rotation, out XY translation)
        {
            scale = matrix.GetScale();
            rotation = matrix.GetRotation();
            translation = matrix.Translation;
        }

        public static Matrix3x2 WithScale(this Matrix3x2 matrix, XY scale)
        {
            return (scale, matrix.GetRotation(), matrix.Translation).CreateMatrix3x2();
        }

        public static Matrix3x2 WithRotation(this Matrix3x2 matrix, float radians)
        {
            return (matrix.GetScale(), radians, matrix.Translation).CreateMatrix3x2();
        }

        public static Matrix3x2 CreateMatrix3x2(this (XY Scale, float Radians, XY Translation) terms)
        {
            var m = Matrix3x2.CreateRotation(terms.Radians);
            m.M11 *= terms.Scale.X;
            m.M12 *= terms.Scale.X;
            m.M21 *= terms.Scale.Y;
            m.M22 *= terms.Scale.Y;
            m.M31 = terms.Translation.X;
            m.M32 = terms.Translation.Y;
            return m;
        }

        public static void PremultiplyAlpha(this Texture2D texture)
        {
            var data = new XNACOLOR[texture.Width * texture.Height];
            texture.GetData(data);

            // TODO: we could do with a parallels

            for (int i = 0; i != data.Length; ++i) data[i] = XNACOLOR.FromNonPremultiplied(data[i].ToVector4());

            texture.SetData(data);
        }
    }
}
