using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropBitmaps;

// BitmapSource -> WriteableBitmap, BitmapFrame, BitmapImage, CroppedBitmap, FormatConvertedBitmap,RenderTargetBitmap,TransformedBitmap        
using WIC_READABLE = System.Windows.Media.Imaging.BitmapSource;
using WIC_WRITABLE = System.Windows.Media.Imaging.WriteableBitmap;

namespace InteropTypes.Graphics.Backends
{
    /// <summary>
    ///  Windows Imaging Component (WIC)
    /// </summary>
    public static partial class WPFToolkit
    {
        public static BitmapInfo GetBitmapInfo(this WIC_READABLE src) { return _Implementation.GetBitmapInfo(src); }

        public static WPFFactory WithWPF(this BitmapInfo binfo) { return new WPFFactory(binfo); }

        public static WPFAdapter WithWPF(this SpanBitmap bmp) { return new WPFAdapter(bmp); }

        public static WPFAdapter WithWPF<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel:unmanaged
        { return new WPFAdapter(bmp); }

        public static MemoryBitmap.ISource UsingMemoryBitmap(this WIC_WRITABLE src) { return new WPFMemoryManager(src); }

        public static void SetPixels(this WIC_WRITABLE bmp, int dstX, int dstY, SpanBitmap spanSrc)
        {
            _Implementation.SetPixels(bmp, dstX, dstY, spanSrc);
        }

        public static MemoryBitmap ToMemoryBitmap(this WIC_READABLE src) { return _Implementation.ToMemoryBitmap(src); }

        
    }    
}
