using System;
using System.Drawing;

namespace InteropBitmaps
{
    /// <see href="https://github.com/dotnet/runtime/tree/master/src/libraries/System.Drawing.Common"/>
    public static partial class GDIToolkit
    {
        #region As Adapter

        public static GDIAdapter AsGDI(this SpanBitmap bmp) { return new GDIAdapter(bmp); }

        public static GDIAdapter AsGDI(this MemoryBitmap bmp) { return new GDIAdapter(bmp); }

        public static GDIAdapter AsGDI<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel : unmanaged
        { return new GDIAdapter(bmp.AsSpanBitmap()); }

        public static GDIAdapter AsGDI<TPixel>(this MemoryBitmap<TPixel> bmp)
            where TPixel : unmanaged
        { return new GDIAdapter(bmp.AsSpanBitmap()); }

        #endregion

        #region As SpanBitmap

        public static PointerBitmap AsPointerBitmap(this System.Drawing.Imaging.BitmapData data)
        {
            var info = new BitmapInfo(data.Width, data.Height, data.PixelFormat.ToInteropPixelFormat(), data.Stride);

            return new PointerBitmap(data.Scan0, info);
        }

        public static SpanBitmap AsSpanBitmap(this System.Drawing.Imaging.BitmapData data)
        {
            return data.AsPointerBitmap().Span;
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this System.Drawing.Imaging.BitmapData data)
            where TPixel: unmanaged
        {
            return data.AsPointerBitmap().AsSpanBitmap<TPixel>();
        }

        #endregion

        public static Bitmap CreateGDIBitmap(this BitmapInfo binfo)
        {
            return new Bitmap(binfo.Width, binfo.Height, binfo.PixelFormat.ToGDIPixelFormat());
        }

        public static void Mutate(this Bitmap bmp, Action<PointerBitmap> action)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            System.Drawing.Imaging.BitmapData bits = null;

            try
            {
                bits = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);

                action(bits.AsPointerBitmap());
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
                dstbits = dst.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, dst.PixelFormat);

                dstbits.AsSpanBitmap().SetPixels(dstx, dsty, src);
            }
            finally
            {
                if (dstbits != null) dst.UnlockBits(dstbits);
            }
        }

        public static MemoryBitmap ToMemoryBitmap(this Bitmap bmp)
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

        public static MemoryBitmap ToMemoryBitmap(this Image img)
        {
            using (var bmp = new Bitmap(img))
            {
                return bmp.ToMemoryBitmap();
            }
        }

        public static MemoryBitmap ToMemoryBitmap(this TextureBrush brush)
        {
            return brush.Image.ToMemoryBitmap();
        }        

        public static MemoryBitmap ToMemoryBitmap(this Icon icon)
        {
            using (var bmp = icon.ToBitmap())
            {
                return bmp.ToMemoryBitmap();
            }
        }

        
    }
}
