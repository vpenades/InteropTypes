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

        public static PixelFormat GetPixelFormat<TPixel>()
            where TPixel : unmanaged, SixLabors.ImageSharp.PixelFormats.IPixel<TPixel>
        {
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Alpha8)) return new PixelFormat(PEF.Alpha8);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Argb32)) return new PixelFormat(PEF.Alpha8, PEF.Red8, PEF.Green8,PEF.Blue8);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Bgr24)) return new PixelFormat(PEF.Blue8, PEF.Green8, PEF.Red8);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Bgr565)) return new PixelFormat(PEF.Blue5, PEF.Green6, PEF.Red5);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Bgra32)) return new PixelFormat(PEF.Blue8, PEF.Green8, PEF.Red8,PEF.Alpha8);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Bgra4444)) return new PixelFormat(PEF.Blue4, PEF.Green4, PEF.Red4, PEF.Alpha4);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Bgra5551)) return new PixelFormat(PEF.Blue5, PEF.Green5, PEF.Red5, PEF.Alpha1);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Gray16)) return new PixelFormat(PEF.Gray16);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Gray8)) return new PixelFormat(PEF.Gray8);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Rgb24)) return new PixelFormat(PEF.Red8, PEF.Green8,PEF.Blue8);
            if (typeof(TPixel) == typeof(SixLabors.ImageSharp.PixelFormats.Rgba32)) return new PixelFormat(PEF.Red8, PEF.Green8, PEF.Blue8,PEF.Alpha8);            

            throw new NotImplementedException();
        }        
    }
}
