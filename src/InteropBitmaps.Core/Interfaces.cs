using System;
using System.Collections.Generic;
using System.Text;

using SIZE = System.Drawing.Size;
using POINT = System.Drawing.Point;

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
        Pixel.Format PixelFormat { get; }

        SIZE Size { get; }        

        TPixel GetPixel(POINT point);
        void SetPixel(POINT point, TPixel value);
    }    

    

    
}
