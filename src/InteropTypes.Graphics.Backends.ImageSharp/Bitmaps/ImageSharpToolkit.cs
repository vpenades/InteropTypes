using System;

using InteropTypes.Graphics.Bitmaps;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropTypes.Graphics.Backends
{
    public static partial class ImageSharpToolkit
    {
        #region Imagesharp facade

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

        #region Image => MemoryBitmap

        [Obsolete("Use other alternatives")]
        public static MemoryBitmap.IDisposableSource UsingMemoryBitmap<TPixel>(this Image<TPixel> src, bool owned = false)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return new Adapters.ImageSharpMemoryManager<TPixel>(src, owned);
        }
        
        public static MemoryBitmap ToMemoryBitmap(this Image src)            
        {
            var dst = new MemoryBitmap(_Implementation.GetBitmapInfo(src));
            _Implementation.CopyPixels(src, dst);
            return dst;
        }

        public static MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>(this Image src)
            where TPixel:unmanaged
        {
            var dst = new MemoryBitmap<TPixel>(_Implementation.GetBitmapInfo(src));
            _Implementation.CopyPixels(src, dst);
            return dst;
        }

        #endregion                      
    }
}
