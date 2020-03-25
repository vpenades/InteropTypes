using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropBitmaps
{
    public static partial class ImageSharpToolkit
    {
        #region WithImageSharp

        public static Adapters.ImageSharpFactory WithImageSharp(this BitmapInfo binfo) { return new Adapters.ImageSharpFactory(binfo); }

        public static Adapters.ImageSharpAdapter WithImageSharp(this SpanBitmap bitmap) { return new Adapters.ImageSharpAdapter(bitmap); }

        public static Adapters.ImageSharpAdapter WithImageSharp(this MemoryBitmap bitmap) { return new Adapters.ImageSharpAdapter(bitmap.AsSpanBitmap()); }

        public static Adapters.ImageSharpAdapter<TPixel> WithImageSharp<TPixel>(this SpanBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        { return new Adapters.ImageSharpAdapter<TPixel>(bitmap); }

        public static Adapters.ImageSharpAdapter<TPixel> WithImageSharp<TPixel>(this MemoryBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        { return new Adapters.ImageSharpAdapter<TPixel>(bitmap); }

        #endregion

        #region As Span

        public static SpanBitmap AsSpanBitmap(this Image src)
        {
            return _Implementation.WrapAsSpanBitmap(src);
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return _Implementation.WrapAsSpanBitmap(src);
        }

        public static MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return AsSpanBitmap(src).ToMemoryBitmap();
        }

        #endregion        
    }
}
