﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using InteropDrawing;

using POINT = InteropDrawing.Point2;

namespace InteropBitmaps
{
    public static class DrawingExtensions
    {
        

        public static IDrawing2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap, POINT virtualSize)
            where TPixel : unmanaged
        {
            if (virtualSize.X == 0) throw new ArgumentException(nameof(virtualSize));
            if (virtualSize.Y == 0) throw new ArgumentException(nameof(virtualSize));

            var dc = bitmap.CreateDrawingContext();
            return _UseVirtualViewport(dc, (bitmap.Width,bitmap.Height), virtualSize);
        }

        public static IDrawing2D CreateDrawingContext(this MemoryBitmap bitmap, POINT virtualSize)
        {
            if (virtualSize.X == 0) throw new ArgumentException(nameof(virtualSize));
            if (virtualSize.Y == 0) throw new ArgumentException(nameof(virtualSize));

            var dc = bitmap.CreateDrawingContext();
            return _UseVirtualViewport(dc, (bitmap.Width, bitmap.Height), virtualSize);
        }

        private static IDrawing2D _UseVirtualViewport(IDrawing2D dc, POINT bitmapSize, POINT virtualSize)
        {
            var sx = bitmapSize.X / virtualSize.X;
            var sy = bitmapSize.Y / virtualSize.Y;
            var tx = sx < 0 ? bitmapSize.X : 0;
            var ty = sy < 0 ? bitmapSize.Y : 0;
            var xform = System.Numerics.Matrix3x2.CreateScale(sx, sy);
            xform.Translation = new System.Numerics.Vector2(tx, ty);

            return InteropDrawing.Transforms.Drawing2DTransform.Create(dc, xform);
        }

        public static IDrawing2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap)
            where TPixel:unmanaged
        {
            return bitmap.AsTypeless().CreateDrawingContext();
        }

        public static IDrawing2D CreateDrawingContext(this MemoryBitmap bitmap)
        {
            IDrawing2D _Create<TPixel>()
                where TPixel
                : unmanaged
                , Pixel.IPixelReflection<TPixel>
            {                
                return new InteropDrawing.Backends._MemoryDrawingContext<TPixel>(bitmap.OfType<TPixel>(), c => Pixel.GetColor<TPixel>(c));
            }

            switch (bitmap.PixelFormat.PackedFormat)
            {
                case Pixel.Alpha8.Code: return _Create<Pixel.Alpha8>();

                case Pixel.Luminance8.Code: return _Create<Pixel.Luminance8>();
                case Pixel.Luminance16.Code: return _Create<Pixel.Luminance16>();

                case Pixel.BGR565.Code: return _Create<Pixel.BGR565>();
                case Pixel.BGRA4444.Code: return _Create<Pixel.BGRA4444>();
                case Pixel.BGRA5551.Code: return _Create<Pixel.BGRA5551>();

                case Pixel.BGR24.Code: return _Create<Pixel.BGR24>();
                case Pixel.RGB24.Code: return _Create<Pixel.RGB24>();

                case Pixel.ARGB32.Code: return _Create<Pixel.ARGB32>();
                case Pixel.RGBA32.Code: return _Create<Pixel.RGBA32>();
                case Pixel.BGRA32.Code: return _Create<Pixel.BGRA32>();

                case Pixel.VectorRGB.Code: return _Create<Pixel.VectorRGB>();
                case Pixel.VectorBGR.Code: return _Create<Pixel.VectorBGR>();
                case Pixel.VectorRGBA.Code: return _Create<Pixel.VectorRGBA>();
                case Pixel.VectorBGRA.Code: return _Create<Pixel.VectorBGRA>();
            }            

            throw new NotImplementedException();
        }

        public static IDrawing2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap, Converter<System.Drawing.Color, TPixel> converter)
            where TPixel : unmanaged
        {
            return new InteropDrawing.Backends._MemoryDrawingContext<TPixel>(bitmap, converter);
        }

        public static void DrawConsoleFont(this IDrawing2D dc, POINT origin, string text, System.Drawing.Color color)
        {
            dc.DrawFont(origin, 0.4f, text, FontStyle.Gray.With(color, 1));
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
