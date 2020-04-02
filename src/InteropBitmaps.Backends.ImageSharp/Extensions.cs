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

        public static Adapters.ImageSharpSpanAdapter WithImageSharp(this SpanBitmap bitmap) { return new Adapters.ImageSharpSpanAdapter(bitmap); }        

        public static Adapters.ImageSharpSpanAdapter<TPixel> WithImageSharp<TPixel>(this SpanBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        { return new Adapters.ImageSharpSpanAdapter<TPixel>(bitmap); }        

        public static Adapters.ImageSharpMemoryAdapter UsingImageSharp(this MemoryBitmap bmp) { return new Adapters.ImageSharpMemoryAdapter(bmp); }

        public static Adapters.ImageSharpMemoryAdapter<TPixel> UsingImageSharp<TPixel>(this MemoryBitmap<TPixel> bmp)
            where TPixel : unmanaged, IPixel<TPixel>
        { return new Adapters.ImageSharpMemoryAdapter<TPixel>(bmp); }

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

        #region extras

        public static Image<TPixel> TryWrapAsImageSharp<TPixel>(this MemoryBitmap<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return _Implementation.TryWrapImageSharp<TPixel>(src);
        }
        
        public static Image TryUsingAsImageSharp(this MemoryBitmap src)            
        {
            return _Implementation.TryWrapImageSharp(src);
        }
        
        public static Image<TPixel> TryUsingImageSharp<TPixel>(this PointerBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (!src.Info.IsContinuous) return null;

            var m = src.UsingMemory<TPixel>();

            return Image.WrapMemory(m, src.Width, src.Height); // We assume here that Image<Tpixel> will dispose IMemoryOwner<T> when disposed.
        }

        #endregion
    }
}
