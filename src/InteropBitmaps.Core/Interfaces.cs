using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

namespace InteropBitmaps
{
    public interface IBitmapInfo
    {
        BitmapInfo Info { get; }
        PixelFormat PixelFormat { get; }
        SIZE Size { get; }
        int Width { get; }
        int Height { get; }
    }

    /// <summary>
    /// Represents an interface to a bitmap that allows accessing pixels individually.
    /// </summary>    
    /// <remarks>
    /// This interface is suitable for image types that store pixel data in non contiguous memory,
    /// or if the underlaying memory data is not accessible.
    /// </remarks>
    public interface IBitmap<TPixel> : IBitmapInfo
        where TPixel : unmanaged
    {
        TPixel GetPixel(POINT point);
        void SetPixel(POINT point, TPixel value);
    }

    public interface ISpanBitmap
    {
        SpanBitmap AsSpanBitmap();
    }

    

    
}
