using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropBitmaps
{
    public static partial class ImageSharpToolkit
    {
        #region As adapter

        public static ImageSharpAdapter AsImageSharp(this SpanBitmap bitmap) { return new ImageSharpAdapter(bitmap); }

        public static ImageSharpAdapter AsImageSharp(this MemoryBitmap bitmap) { return new ImageSharpAdapter(bitmap.AsSpanBitmap()); }

        public static ImageSharpAdapter<TPixel> AsImageSharp<TPixel>(this SpanBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return new ImageSharpAdapter<TPixel>(bitmap);
        }

        public static ImageSharpAdapter<TPixel> AsImageSharp<TPixel>(this MemoryBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return new ImageSharpAdapter<TPixel>(bitmap);
        }

        #endregion

        #region As SpanBitmap

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(src.GetPixelSpan());

            return new SpanBitmap<TPixel>(span, src.Width, src.Height, GetPixelFormat<TPixel>());
        }

        #endregion

        #region API

        public static MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var dst = new MemoryBitmap<TPixel>(src.Width, src.Height, GetPixelFormat<TPixel>());

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcLine = src.Frames[0].GetPixelRowSpan(y);
                var dstLine = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,TPixel>(dst.UseBytesScanline(y));
                srcLine.CopyTo(dstLine);
            }

            return dst;
        }

        public static Image CreateImageSharp(this BitmapInfo binfo)
        {
            return binfo.PixelFormat.CreateImageSharp(binfo.Width, binfo.Height);
        }

        

        #endregion
    }
}
