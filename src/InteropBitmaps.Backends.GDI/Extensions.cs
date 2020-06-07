using System;
using System.Drawing;

using GDIPTR = System.Drawing.Imaging.BitmapData;

namespace InteropBitmaps
{
    /// <see href="https://github.com/dotnet/runtime/tree/master/src/libraries/System.Drawing.Common"/>
    public static partial class GDIToolkit
    {
        #region GDI facade

        public static Adapters.GDIFactory WithGDI(this BitmapInfo binfo) { return new Adapters.GDIFactory(binfo); }

        public static Adapters.GDISpanAdapter WithGDI(this SpanBitmap bmp) { return new Adapters.GDISpanAdapter(bmp); }        

        public static Adapters.GDISpanAdapter WithGDI<TPixel>(this SpanBitmap<TPixel> bmp) where TPixel : unmanaged
        { return new Adapters.GDISpanAdapter(bmp.AsTypeless()); }
        
        public static Adapters.GDIMemoryAdapter UsingGDI(this MemoryBitmap bmp) { return new Adapters.GDIMemoryAdapter(bmp); }

        public static Adapters.GDIMemoryAdapter WithGDI<TPixel>(this MemoryBitmap<TPixel> bmp) where TPixel : unmanaged
        { return new Adapters.GDIMemoryAdapter(bmp); }        

        #endregion

        #region As MemoryBitmap

        public static IPointerBitmapOwner UsingPointerBitmap(this Bitmap bmp)
        {
            return new Adapters.GDIMemoryManager(bmp);
        }

        public static IMemoryBitmapOwner UsingMemoryBitmap(this Bitmap bmp)
        {
            return new Adapters.GDIMemoryManager(bmp);
        }

        #endregion

        #region As SpanBitmap

        public static BitmapInfo GetBitmapInfo(this GDIPTR data)
        {
            return _Implementation.GetBitmapInfo(data);
        }

        public static PointerBitmap AsPointerBitmap(this GDIPTR data)
        {
            var info = _Implementation.GetBitmapInfo(data);
            return new PointerBitmap(data.Scan0, info);
        }

        public static SpanBitmap AsSpanBitmap(this GDIPTR data)
        {
            return data.AsPointerBitmap();
        }

        public static SpanBitmap<TPixel> AsSpanBitmap<TPixel>(this GDIPTR data)
            where TPixel: unmanaged
        {
            return data.AsPointerBitmap().AsSPanBitmapOfType<TPixel>();
        }

        #endregion        

        #region API
        
        public static void Mutate(this Bitmap bmp, Action<PointerBitmap> action)
        {
            _Implementation.Mutate(bmp, action);
        }

        public static void SetPixels(this Bitmap dst, int dstx, int dsty, in SpanBitmap src)
        {
            _Implementation.SetPixels(dst, dstx, dsty, src);
        }        

        public static MemoryBitmap ToMemoryBitmap(this Bitmap bmp, PixelFormat? fmtOverride = null)
        {
            return _Implementation.CloneToMemoryBitmap(bmp, fmtOverride);
        }        

        public static MemoryBitmap ToMemoryBitmap(this Image img, PixelFormat? fmtOverride = null)
        {
            using (var bmp = new Bitmap(img))
            {
                return _Implementation.CloneToMemoryBitmap(bmp, fmtOverride);
            }
        }

        public static MemoryBitmap ToMemoryBitmap(this TextureBrush brush, PixelFormat? fmtOverride = null)
        {
            return ToMemoryBitmap(brush.Image, fmtOverride);
        }        

        public static MemoryBitmap ToMemoryBitmap(this Icon icon, PixelFormat? fmtOverride = null)
        {
            using (var bmp = icon.ToBitmap())
            {
                return _Implementation.CloneToMemoryBitmap(bmp, fmtOverride);
            }
        }

        #endregion
    }
}
