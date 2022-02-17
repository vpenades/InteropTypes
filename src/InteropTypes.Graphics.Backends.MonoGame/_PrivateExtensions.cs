using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

using DRWCOLOR = System.Drawing.Color;
using XNACOLOR = Microsoft.Xna.Framework.Color;
using POINT3 = InteropTypes.Graphics.Drawing.Point3;
using Microsoft.Xna.Framework.Graphics;

namespace InteropTypes.Graphics.Backends
{
    static class _PrivateExtensions
    {
        public static XNACOLOR ToXna(this DRWCOLOR c) { return new XNACOLOR(c.R, c.G, c.B, c.A); }

        public static XNACOLOR ToXnaPremul(this DRWCOLOR c) { return XNACOLOR.FromNonPremultiplied(c.R, c.G, c.B, c.A); }

        public static XNACOLOR ToXnaPremul(this XNACOLOR c) { return XNACOLOR.FromNonPremultiplied(c.R, c.G, c.B, c.A); }

        public static Microsoft.Xna.Framework.Matrix ToXna(this in Matrix3x2 m)
        {
            return new Microsoft.Xna.Framework.Matrix
                (
                m.M11, m.M12, 0, 0,
                m.M21, m.M22, 0, 0,
                0, 0, 1, 0,
                m.M31, m.M32, 0, 1);
        }

        public static Microsoft.Xna.Framework.Matrix ToXNA(this in Matrix4x4 m)
        {
            return new Microsoft.Xna.Framework.Matrix
                (
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
                );
        }

        public static Microsoft.Xna.Framework.Vector2 ToXna(this in Vector2 vector)
        {
            return new Microsoft.Xna.Framework.Vector2(vector.X, vector.Y);
        }

        public static Microsoft.Xna.Framework.Vector3 ToXna(this in Vector3 vector)
        {
            return new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Microsoft.Xna.Framework.Vector3 ToXna(this in POINT3 vector)
        {
            return new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Matrix3x2 CreateVirtualToPhysical(this Microsoft.Xna.Framework.Graphics.GraphicsDevice device, (float width, float height) virtualSize, bool keepAspect)
        {
            var physicalSize = new Vector2(device.Viewport.Width, device.Viewport.Height);

            return physicalSize.CreateVirtualToPhysical(new Vector2(virtualSize.width, virtualSize.height), keepAspect);
        }

        public static Matrix3x2 CreateVirtualToPhysical(this Vector2 physicalSize, Vector2 virtualSize, bool keepAspect)
        {
            var ws = physicalSize.X / Math.Abs(virtualSize.X);
            var hs = physicalSize.Y / Math.Abs(virtualSize.Y);

            if (keepAspect) ws = hs;
            var xform = Matrix3x2.CreateScale(ws, hs);

            if (virtualSize.X < 0) xform.M11 *= -1;
            if (virtualSize.Y < 0) xform.M22 *= -1;

            var offsx = (physicalSize.X - virtualSize.X * hs) * 0.5f;
            var offsy = (physicalSize.Y - virtualSize.Y * hs) * 0.5f;

            xform *= Matrix3x2.CreateTranslation(offsx, offsy);
            return xform;
        }

        public static float DecomposeScale(this in Matrix3x2 xform)
        {
            var det = xform.GetDeterminant();
            var area = Math.Abs(det);
            return (float)Math.Sqrt(area);
        }

        public static Vector2 GetScale(this in Matrix3x2 matrix)
        {
            var sx = matrix.M12 == 0 ? Math.Abs(matrix.M11) : new Vector2(matrix.M11, matrix.M12).Length();
            var sy = matrix.M21 == 0 ? Math.Abs(matrix.M22) : new Vector2(matrix.M21, matrix.M22).Length();
            if (matrix.GetDeterminant() < 0) sy = -sy;
            return new Vector2(sx, sy);
        }

        public static float GetRotation(this in Matrix3x2 matrix)
        {
            return (float)Math.Atan2(matrix.M12, matrix.M11);
        }

        public static void Decompose(this Matrix3x2 matrix, out Vector2 scale, out float rotation, out Vector2 translation)
        {
            scale = matrix.GetScale();
            rotation = matrix.GetRotation();
            translation = matrix.Translation;
        }

        public static Matrix3x2 WithScale(this Matrix3x2 matrix, Vector2 scale)
        {
            return (scale, matrix.GetRotation(), matrix.Translation).CreateMatrix3x2();
        }

        public static Matrix3x2 WithRotation(this Matrix3x2 matrix, float radians)
        {
            return (matrix.GetScale(), radians, matrix.Translation).CreateMatrix3x2();
        }

        public static Matrix3x2 CreateMatrix3x2(this (Vector2 Scale, float Radians, Vector2 Translation) terms)
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
