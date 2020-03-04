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

        #region API

        

        public static Image CreateImageSharp(this BitmapInfo binfo)
        {
            return binfo.PixelFormat.CreateImageSharp(binfo.Width, binfo.Height);
        }        

        #endregion
    }
}
