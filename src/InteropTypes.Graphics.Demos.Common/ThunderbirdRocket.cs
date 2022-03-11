using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

using COLOR = System.Drawing.Color;

namespace InteropTypes
{
    public class ThunderbirdRocket : IDrawingBrush<IScene3D>
    {
        public ThunderbirdRocket(Matrix4x4 l) { Location = l; }

        public Matrix4x4 Location = Matrix4x4.Identity;

        void IDrawingBrush<IScene3D>.DrawTo(IScene3D context)
        {
            context = context.CreateTransformed3D(Location);
            DrawTo(context);
        }

        public static void DrawTo(IScene3D context)
        {
            var style = new OutlineFillStyle(ColorStyle.GetDefaultFrom(context, COLOR.Red), COLOR.Black, 0.1f);

            context.DrawSphere(Point3.UnitY, 2.5f, style);

            context.DrawSegment((0, 4, 0), (0, 8, 0), 1.5f, (style, LineCapStyle.Round, LineCapStyle.Triangle));

            for (int i = 0; i < 3; ++i)
            {
                var angle = (i * 120) * (float)Math.PI / 180f;
                var h = new Point3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));

                var a = new Point3(0, 4, 0) + h * 0.5f;
                var b = new Point3(0, 2, 0) + h * 2;
                var c = new Point3(0, 0, 0) + h * 2;
                var d = new Point3(0, -2, 0) + h * 2;
                var e = new Point3(0, 0.15f, 0) + h * 0.5f;

                context.DrawSegments(Point3.Array(a, b, c), 0.5f, (style, LineCapStyle.Triangle, LineCapStyle.Flat));
                context.DrawSegment(c, d, 1f, (style, LineCapStyle.Triangle, LineCapStyle.Flat));

                // connection to central sphere
                context.DrawSegment((c + d) * 0.5f, e, 0.25f, (style, LineCapStyle.Triangle, LineCapStyle.Flat));

                // exhaust plume
                context.DrawSegment(d - Vector3.UnitY, d - Vector3.UnitY * 3, 0.5f, ((COLOR.White, COLOR.Blue.WithAlpha(150), 0.5f), LineCapStyle.Round, LineCapStyle.Triangle));
            }
        }        
    }
}
