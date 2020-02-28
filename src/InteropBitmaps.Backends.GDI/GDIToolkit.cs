using System;
using System.Drawing;

using GDIFMT = System.Drawing.Imaging.PixelFormat;

namespace InteropBitmaps
{
    /// <see href="https://github.com/dotnet/runtime/tree/master/src/libraries/System.Drawing.Common"/>
    public static partial class GDIToolkit
    {
        public static GDIAdapter AsGDI(this SpanBitmap bmp) { return new GDIAdapter(bmp); }

        public static GDIAdapter AsGDI(this MemoryBitmap bmp) { return new GDIAdapter(bmp); }

        public static GDIAdapter AsGDI<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel : unmanaged
        { return new GDIAdapter(bmp.AsSpanBitmap()); }

        public static GDIAdapter AsGDI<TPixel>(this MemoryBitmap<TPixel> bmp)
            where TPixel : unmanaged
        { return new GDIAdapter(bmp.AsSpanBitmap()); }        

        public static SpanBitmap AsSpanBitmap(this System.Drawing.Imaging.BitmapData data)
        {
            var info = new BitmapInfo(data.Width, data.Height, data.PixelFormat.ToInteropPixelFormat(), data.Stride);

            return new SpanBitmap(data.Scan0, info);
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this System.Drawing.Imaging.BitmapData data)
            where TPixel: unmanaged
        {
            return data.AsSpanBitmap().AsSpanBitmap<TPixel>();
        }

        public static void Mutate(this Bitmap bmp, Action<PointerBitmap> action)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            System.Drawing.Imaging.BitmapData bits = null;

            try
            {
                bits = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                var info = new BitmapInfo(bits.Width, bits.Height, bits.PixelFormat.ToInteropPixelFormat(), bits.Stride);

                action((bits.Scan0, info));
            }
            finally
            {
                if (bits != null) bmp.UnlockBits(bits);
            }
        }

        public static void SetPixels(this Bitmap dst, int dstx, int dsty, SpanBitmap src)
        {
            var rect = new Rectangle(0, 0, dst.Width, dst.Height);

            System.Drawing.Imaging.BitmapData dstbits = null;

            try
            {
                dstbits = dst.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, dst.PixelFormat);

                dstbits.AsSpanBitmap().SetPixels(dstx, dsty, src);
            }
            finally
            {
                if (dstbits != null) dst.UnlockBits(dstbits);
            }
        }

        public static MemoryBitmap CloneToMemoryBitmap(this Bitmap bmp)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            System.Drawing.Imaging.BitmapData bits = null;

            try
            {
                bits = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                return bits.AsSpanBitmap().ToMemoryBitmap();
            }
            finally
            {
                if (bits != null) bmp.UnlockBits(bits);
            }
        }        

        public static MemoryBitmap CloneToMemoryBitmap(this Image img)
        {
            using (var bmp = new Bitmap(img))
            {
                return bmp.CloneToMemoryBitmap();
            }
        }

        public static MemoryBitmap CloneToMemoryBitmap(this TextureBrush brush)
        {
            return brush.Image.CloneToMemoryBitmap();
        }

        public static MemoryBitmap<TPixel> CloneToMemoryBitmap<TPixel>(this Bitmap bmp) where TPixel : unmanaged
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            System.Drawing.Imaging.BitmapData bits = null;

            try
            {
                bits = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);

                return bits.AsSpanBitmap<TPixel>().ToMemoryBitmap();
            }
            finally
            {
                if (bits != null) bmp.UnlockBits(bits);
            }
        }

        public static MemoryBitmap CloneToMemoryBitmap(this Icon icon)
        {
            using (var bmp = icon.ToBitmap())
            {
                return bmp.CloneToMemoryBitmap();
            }
        }

        
    }
}
