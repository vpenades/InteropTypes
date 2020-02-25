using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    /// <summary>
    /// Represents an interface to a bitmap, where pixels can be accessed individually.
    /// </summary>
    /// <typeparam name="TPixel"></typeparam>
    public interface IBitmap<TPixel>
        where TPixel : unmanaged
    {
        int Width { get; }
        int Height { get; }

        TPixel GetPixel(int x, int y);
        void SetPixel(int x, int y, TPixel value);
    }    
}
