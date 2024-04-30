using System;
using System.Collections.Generic;
using System.Text;

using XY = System.Numerics.Vector2;

namespace InteropTypes.Tensors.Imaging
{
    static class _Drawing
    {
        public static void DrawPixelLine<TPixel>(this SpanTensor2<TPixel> bitmap, XY a, XY b, TPixel color)
            where TPixel : unmanaged
        {
            var bounds = bitmap.Dimensions;

            var ab = b - a;

            if (Math.Abs(ab.X) <= 1 && Math.Abs(ab.Y) <= 1)
            // can't loop; draw a single pixel
            {
                int x = (int)a.X;
                int y = (int)a.Y;

                if (bounds.ContainsIndices(y, x)) bitmap[y].Span[x] = color;

                return;
            }

            if (Math.Abs(ab.X) > Math.Abs(ab.Y))
            // loop from left to right
            {
                var ptr = a;
                var max = b.X;

                if (ab.X < 0) { ab = -ab; ptr = b; max = a.X; }

                var d = new XY(1, ab.Y / ab.X);

                while (ptr.X <= max)
                {
                    int x = (int)ptr.X;
                    int y = (int)ptr.Y;

                    if (bounds.ContainsIndices(y, x)) bitmap[y].Span[x] = color;

                    ptr += d;
                }
            }
            else
            // loop from top to bottom
            {
                var ptr = a;
                var max = b.Y;

                if (ab.Y < 0) { ab = -ab; ptr = b; max = a.Y; }

                var d = new XY(ab.X / ab.Y, 1);

                while (ptr.Y <= max)
                {
                    int x = (int)ptr.X;
                    int y = (int)ptr.Y;

                    if (bounds.ContainsIndices(y, x)) bitmap[y].Span[x] = color;

                    ptr += d;
                }
            }
        }

        public static void DrawPixelRectangle<TPixel>(this SpanTensor2<TPixel> bitmap, XY a, XY b, TPixel color)
            where TPixel : unmanaged
        {
            DrawPixelLine(bitmap, a, new XY(b.X, a.Y), color);
            DrawPixelLine(bitmap, new XY(b.X, a.Y), b, color);
            DrawPixelLine(bitmap, b, new XY(a.X, b.Y), color);
            DrawPixelLine(bitmap, new XY(a.X, b.Y), a, color);
        }
    }
}
