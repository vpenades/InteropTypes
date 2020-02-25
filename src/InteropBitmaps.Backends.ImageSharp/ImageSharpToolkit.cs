using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Processing;

using PEF = InteropBitmaps.PixelElementFormat;

namespace InteropBitmaps
{
    public static partial class ImageSharpToolkit
    {
        public static ImageSharpAdapter<TPixel> AsImageSharp<TPixel>(this MemoryBitmap bitmap)
            where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            return new ImageSharpAdapter<TPixel>(bitmap.AsSpanBitmapOfType<TPixel>());
        }

        public static ImageSharpAdapter<TPixel> AsImageSharp<TPixel>(this MemoryBitmap<TPixel> bitmap)
            where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            return new ImageSharpAdapter<TPixel>(bitmap);
        }

        public static ImageSharpAdapter<TPixel> AsImageSharp<TPixel>(this SpanBitmap<TPixel> bitmap)
            where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            return new ImageSharpAdapter<TPixel>(bitmap);
        }

        public static MemoryBitmap<TPixel> CopyToMemoryBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            var dst = new MemoryBitmap<TPixel>(src.Width, src.Height);

            for (int y = 0; y < dst.Height; ++y)
            {
                var srcLine = src.Frames[0].GetPixelRowSpan(y);
                var dstLine = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte,TPixel>(dst.UseBytesScanline(y));
                srcLine.CopyTo(dstLine);
            }

            return dst;
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            var span = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(src.GetPixelSpan());

            return new SpanBitmap<TPixel>(span, src.Width, src.Height);
        }                 

          
    }
}
