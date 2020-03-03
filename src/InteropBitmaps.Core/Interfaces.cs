using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents an interface to a bitmap that allows accessing pixels individually.
    /// </summary>    
    /// <remarks>
    /// This interface is suitable for image types that store pixel data in non contiguous memory,
    /// or if the underlaying memory data is not accessible.
    /// </remarks>
    public interface IBitmap<TPixel>
        where TPixel : unmanaged
    {
        int Width { get; }
        int Height { get; }

        PixelFormat PixelFormat { get; }

        TPixel GetPixel(int x, int y);
        void SetPixel(int x, int y, TPixel value);
    }


    public interface ISpanLock : IDisposable
    {
        BitmapInfo Info { get; }
        SpanBitmap Span { get; }
    }
}
