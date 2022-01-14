using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

namespace InteropBitmaps
{
    public interface IBitmapInfo
    {
        /// <summary>
        /// Gets the layout information of the bitmap; Width, Height, PixelFormat, etc.
        /// </summary>
        BitmapInfo Info { get; }

        /// <summary>
        /// Gets the pixel format of the bitmap.
        /// </summary>
        PixelFormat PixelFormat { get; }

        /// <summary>
        /// Gets the size of the bitmap, in pixels.
        /// </summary>
        SIZE Size { get; }

        /// <summary>
        /// Gets the width of the bitmap, in pixels.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the bitmap, in pixels.
        /// </summary>
        int Height { get; }
    }

    /// <summary>
    /// Represents an interface to a memory bitmap.
    /// </summary>
    /// <remarks>
    /// Implemented by <see cref="MemoryBitmap"/> and <see cref="MemoryBitmap{TPixel}"/>
    /// </remarks>
    public interface IMemoryBitmap : IBitmapInfo
    {
        /// <summary>
        /// Casts this bitmap to a <see cref="SpanBitmap"/>
        /// </summary>
        /// <returns>A <see cref="SpanBitmap"/> that shares the pixels with the source bitmap.</returns>
        SpanBitmap AsSpanBitmap();
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
}
