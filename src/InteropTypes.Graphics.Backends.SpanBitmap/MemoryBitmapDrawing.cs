using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

using POINT = InteropTypes.Graphics.Drawing.Point2;

namespace InteropTypes.Graphics.Backends
{
    public static class SpanBitmapDrawing
    {
        public static void UseDrawingContext<TPixel>(this SpanBitmap<TPixel> bitmap, Action<ICanvas2D> canvas)
            where TPixel : unmanaged
            , Pixel.IValueSetter<Pixel.BGRA32>
            , Pixel.IValueSetter<Pixel.BGRP32>
        {
            ICanvas2D onPin(PointerBitmap ptr)
            {
                return new _PointerDrawingContext<TPixel>(ptr, c => Pixel.GetColor<TPixel>(c));
            }

            bitmap.PinWritablePointer(ptr => canvas(onPin(ptr)));
        }

        public static void UseDrawingContext<TPixel>(this SpanBitmap<TPixel> bitmap, POINT virtualSize, Action<ICanvas2D> canvas)
            where TPixel : unmanaged
            , Pixel.IValueSetter<Pixel.BGRA32>
            , Pixel.IValueSetter<Pixel.BGRP32>
        {
            if (virtualSize.X == 0) throw new ArgumentException(nameof(virtualSize));
            if (virtualSize.Y == 0) throw new ArgumentException(nameof(virtualSize));

            ICanvas2D onPin(PointerBitmap ptr)
            {
                var dc = new _PointerDrawingContext<TPixel>(ptr, c => Pixel.GetColor<TPixel>(c));
                return _UseVirtualViewport(dc, (ptr.Width, ptr.Height), virtualSize);
            }

            bitmap.PinWritablePointer(ptr => canvas(onPin(ptr)));            
        }

        public static ICanvas2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap, POINT virtualSize)
            where TPixel : unmanaged
        {
            if (virtualSize.X == 0) throw new ArgumentException(nameof(virtualSize));
            if (virtualSize.Y == 0) throw new ArgumentException(nameof(virtualSize));

            var dc = bitmap.CreateDrawingContext();
            return _UseVirtualViewport(dc, (bitmap.Width,bitmap.Height), virtualSize);
        }

        public static ICanvas2D CreateDrawingContext(this MemoryBitmap bitmap, POINT virtualSize)
        {
            if (virtualSize.X == 0) throw new ArgumentException(nameof(virtualSize));
            if (virtualSize.Y == 0) throw new ArgumentException(nameof(virtualSize));

            var dc = bitmap.CreateDrawingContext();
            return _UseVirtualViewport(dc, (bitmap.Width, bitmap.Height), virtualSize);
        }

        private static ICanvas2D _UseVirtualViewport(ICanvas2D dc, POINT bitmapSize, POINT virtualSize)
        {
            var sx = bitmapSize.X / virtualSize.X;
            var sy = bitmapSize.Y / virtualSize.Y;
            var tx = sx < 0 ? bitmapSize.X : 0;
            var ty = sy < 0 ? bitmapSize.Y : 0;
            var xform = System.Numerics.Matrix3x2.CreateScale(sx, sy);
            xform.Translation = new System.Numerics.Vector2(tx, ty);

            return Drawing.Transforms.Canvas2DTransform.Create(dc, xform);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static ICanvas2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap)
            where TPixel:unmanaged
        {
            return bitmap.AsTypeless().CreateDrawingContext();
        }

        public static ICanvas2D CreateDrawingContext(this MemoryBitmap bitmap)
        {
            ICanvas2D _Create<TPixel>()
                where TPixel : unmanaged
                , Pixel.IValueSetter<Pixel.BGRA32>
                , Pixel.IValueSetter<Pixel.BGRP32>
            {                
                return new _MemoryDrawingContext<TPixel>(bitmap.OfType<TPixel>(), c => Pixel.GetColor<TPixel>(c));
            }

            switch (bitmap.PixelFormat.Code)
            {
                case Pixel.Alpha8.Code: return _Create<Pixel.Alpha8>();

                // case Pixel.Luminance8.Code: return _Create<Pixel.Luminance8>();
                // case Pixel.Luminance16.Code: return _Create<Pixel.Luminance16>();

                // case Pixel.BGR565.Code: return _Create<Pixel.BGR565>();
                // case Pixel.BGRA4444.Code: return _Create<Pixel.BGRA4444>();
                // case Pixel.BGRA5551.Code: return _Create<Pixel.BGRA5551>();

                case Pixel.BGR24.Code: return _Create<Pixel.BGR24>();
                case Pixel.RGB24.Code: return _Create<Pixel.RGB24>();

                case Pixel.ARGB32.Code: return _Create<Pixel.ARGB32>();
                case Pixel.RGBA32.Code: return _Create<Pixel.RGBA32>();
                case Pixel.BGRA32.Code: return _Create<Pixel.BGRA32>();

                // case Pixel.RGB96F.Code: return _Create<Pixel.RGB96F>();
                // case Pixel.BGR96F.Code: return _Create<Pixel.BGR96F>();
                // case Pixel.RGBA128F.Code: return _Create<Pixel.RGBA128F>();
                // case Pixel.BGRA128F.Code: return _Create<Pixel.BGRA128F>();
            }            

            throw new NotImplementedException($"{bitmap.PixelFormat}");
        }

        public static ICanvas2D CreateDrawingContext<TPixel>(this MemoryBitmap<TPixel> bitmap, Converter<System.Drawing.Color, TPixel> converter)
            where TPixel : unmanaged
            , Pixel.IValueSetter<Pixel.BGRP32>
        {
            return new _MemoryDrawingContext<TPixel>(bitmap, converter);
        }

        public static void DrawConsoleFont(this ICanvas2D dc, POINT origin, string text, System.Drawing.Color color)
        {
            dc.DrawFont(origin, 0.4f, text, FontStyle.Gray.With(color, 1));
        }

        public static void DrawPixelLine<TPixel>(this MemoryBitmap<TPixel> bitmap, POINT a, POINT b, TPixel color)
            where TPixel : unmanaged
        {
            bitmap.AsSpanBitmap().DrawPixelLine(a, b, color);
        }

        public static void DrawPixelLine<TPixel>(this SpanBitmap<TPixel> bitmap, POINT a, POINT b, TPixel color)
            where TPixel:unmanaged
        {
            var bounds = bitmap.Info;            

            var ab = (b - a);

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
