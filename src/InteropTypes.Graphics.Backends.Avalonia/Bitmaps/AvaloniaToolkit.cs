using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
{
    public static partial class AvaloniaToolkit
    {
        public static MemoryBitmap ToMemoryBitmap<TImage>(this TImage src)
            where TImage: Avalonia.Media.IImageBrushSource
        {
            if (src is Avalonia.Media.Imaging.Bitmap bmp)
            {
                var dst = new MemoryBitmap(_Implementation.GetBitmapInfo(bmp));

                _Implementation.CopyPixels(bmp, dst);
                return dst;
            }

            throw new NotImplementedException();
        }
    }
}
