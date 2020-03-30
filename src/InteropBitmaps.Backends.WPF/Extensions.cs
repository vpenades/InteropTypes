using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// BitmapSource -> WriteableBitmap, BitmapFrame, BitmapImage, CroppedBitmap, FormatConvertedBitmap,RenderTargetBitmap,TransformedBitmap        
using WIC_READABLE = System.Windows.Media.Imaging.BitmapSource;
using WIC_WRITABLE = System.Windows.Media.Imaging.WriteableBitmap;

namespace InteropBitmaps
{
    /// <summary>
    ///  Windows Imaging Component (WIC)
    /// </summary>
    public static partial class WPFToolkit
    {
        public static BitmapInfo GetBitmapInfo(this WIC_READABLE src) { return _Implementation.GetBitmapInfo(src); }

        public static Adapters.WPFFactory WithWPF(this BitmapInfo binfo) { return new Adapters.WPFFactory(binfo); }

        public static Adapters.WPFAdapter WithWPF(this SpanBitmap bmp) { return new Adapters.WPFAdapter(bmp); }

        public static Adapters.WPFAdapter WithWPF<TPixel>(this SpanBitmap<TPixel> bmp)
            where TPixel:unmanaged
        { return new Adapters.WPFAdapter(bmp); }

        public static IMemoryBitmapOwner UsingMemoryBitmap(this WIC_WRITABLE src) { return new Adapters.WPFMemoryManager(src); }

        public static void SetPixels(this WIC_WRITABLE bmp, int dstX, int dstY, SpanBitmap spanSrc)
        {
            _Implementation.SetPixels(bmp, dstX, dstY, spanSrc);
        }

        public static MemoryBitmap ToMemoryBitmap(this WIC_READABLE src) { return _Implementation.ToMemoryBitmap(src); }

        
    }    
}
