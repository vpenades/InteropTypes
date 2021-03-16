using System;
using System.Collections.Generic;
using System.Text;

using InteropDrawing;

using POINT = InteropDrawing.Point2;

namespace InteropBitmaps
{
    public static class DrawingExtensions
    {
        public static void DrawConsoleFont(this IDrawing2D dc, POINT origin, string text, System.Drawing.Color color)
        {
            dc.DrawFont(origin, 0.4f, text, FontStyle.Gray.With(color, 1));
        }

        public static IDrawing2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap, Converter<System.Drawing.Color,TPixel> converter)            
            where TPixel:unmanaged
        {
            return new InteropDrawing.Backends._SimpleDrawingContext<TPixel>(bitmap, converter);
        }

        public static IDrawing2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap)
            where TPixel : unmanaged, Pixel.IFactory<TPixel>
        {
            return new InteropDrawing.Backends._SimpleDrawingContext<TPixel>(bitmap, c=> Pixel.GetColor<TPixel>(c));
        }

        public static void DrawPixelLine<TPixel>(this MemoryBitmap<TPixel> bitmap, POINT a, POINT b, TPixel color)
            where TPixel:unmanaged
        {
            var bounds = bitmap.Info;            

            var ab = (b - a).ToNumerics();

            if (Math.Abs(ab.X) <= 1 && Math.Abs(ab.Y) <=1)
                // can't loop; draw a single pixel
            {
                int x = (int)a.X;
                int y = (int)a.Y;
                if (bounds.Contains(x, y)) bitmap.SetPixel(x, y, color);
                return;
            }

            if (Math.Abs(ab.X) > Math.Abs(ab.Y))
                // loop from left to right
            {
                var ptr = a;
                var max = b.X;

                if (ab.X < 0) { ab = -ab; ptr = b; max = a.X; }

                var d = new System.Numerics.Vector2(1, ab.Y / ab.X);

                while(ptr.X <= max)
                {
                    int x = (int)ptr.X;
                    int y = (int)ptr.Y;
                    if (bounds.Contains(x,y)) bitmap.SetPixel(x,y, color);
                    ptr += d;
                }                
            }
            else
                // loop from top to bottom
            {
                var ptr = a;
                var max = b.Y;

                if (ab.Y < 0) { ab = -ab; ptr = b; max = a.Y; }

                var d = new System.Numerics.Vector2(ab.X / ab.Y, 1);

                while (ptr.Y <= max)
                {
                    int x = (int)ptr.X;
                    int y = (int)ptr.Y;
                    if (bounds.Contains(x, y)) bitmap.SetPixel(x, y, color);
                    ptr += d;
                }
            }
        }
    }
}
