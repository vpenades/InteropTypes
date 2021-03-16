using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using DRWCOLOR = System.Drawing.Color;
using XNACOLOR = Microsoft.Xna.Framework.Color;

namespace InteropDrawing.Backends
{
    static class _PrivateExtensions
    {
        public static XNACOLOR ToXna(this DRWCOLOR c) { return new XNACOLOR(c.R, c.G, c.B, c.A); }

        public static XNACOLOR ToXnaPremul(this DRWCOLOR c) { return XNACOLOR.FromNonPremultiplied(c.R, c.G, c.B, c.A); }

        private static XNACOLOR ToXnaPremul(this XNACOLOR c) { return XNACOLOR.FromNonPremultiplied(c.R, c.G, c.B, c.A); }

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

        public static Microsoft.Xna.Framework.Vector3 ToXna(this in Vector3 vector)
        {
            return new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Microsoft.Xna.Framework.Vector3 ToXna(this in Point3 vector)
        {
            return new Microsoft.Xna.Framework.Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Matrix3x2 CreateVirtualToPhysical(this Microsoft.Xna.Framework.Graphics.GraphicsDevice device, (float width, float height) virtualSize, bool keepAspect)
        {
            var physicalSize = new Vector2(device.Viewport.Width, device.Viewport.Height);

            return CreateVirtualToPhysical(physicalSize, new Vector2(virtualSize.width, virtualSize.height), keepAspect);
        }

        public static Matrix3x2 CreateVirtualToPhysical(this Vector2 physicalSize, Vector2 virtualSize, bool keepAspect)
        {
            var ws = (float)physicalSize.X / Math.Abs(virtualSize.X);
            var hs = (float)physicalSize.Y / Math.Abs(virtualSize.Y);

            if (keepAspect) ws = hs;
            var xform = Matrix3x2.CreateScale(ws, hs);

            if (virtualSize.X < 0) xform.M11 *= -1;
            if (virtualSize.Y < 0) xform.M22 *= -1;

            var offsx = (physicalSize.X - virtualSize.X * hs) * 0.5f;
            var offsy = (physicalSize.Y - virtualSize.Y * hs) * 0.5f;

            xform *= Matrix3x2.CreateTranslation(offsx, offsy);
            return xform;
        }

        public static void Decompose(this Matrix3x2 matrix, out Vector2 scale, out float rotation, out Vector2 translation)
        {
            // https://stackoverflow.com/questions/45159314/decompose-2d-transformation-matrix            

            // this scale decomposition seems more correct
            var sx = new Vector2(matrix.M11, matrix.M12).Length();
            var sy = new Vector2(matrix.M21, matrix.M22).Length();

            scale = new Vector2(sx, sy);

            // if scale has negative sign, extracting rotation is a bit more complicated ^_^;

            rotation = (float)Math.Atan2(matrix.M12, matrix.M11);
            translation = matrix.Translation;
        }
    }
}
