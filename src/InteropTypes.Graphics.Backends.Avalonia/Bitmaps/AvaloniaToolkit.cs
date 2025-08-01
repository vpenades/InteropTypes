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
            if (src is Avalonia.Media.Imaging.WriteableBitmap srcWrt)
            {               

                return _Implementation.ToMemoryBitmap(srcWrt);
            }

            if (src is Avalonia.Media.Imaging.Bitmap srcBmp)
            {
                var nfo = _Implementation.GetBitmapInfo(srcBmp);
                var dst = new MemoryBitmap(nfo);

                _Implementation.CopyPixels(srcBmp, dst);
                return dst;
            }

            

            throw new NotImplementedException();
        }

        public static Avalonia.Media.Imaging.WriteableBitmap ToAvaloniaBitmap(this MemoryBitmap src) // seems it works
        {
            return _Implementation.ToAvaloniaBitmap(src);
        }
    }
}
