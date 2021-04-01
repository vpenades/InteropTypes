using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ANDROIDGFX = Android.Graphics;
using ANDROIDMEDIA = Android.Media;
using ANDROIDBITMAP = Android.Graphics.Bitmap;

namespace InteropBitmaps
{
    public static class AndroidToolkit
    {
        public static Pixel.Format ToInterop(this ANDROIDGFX.Format fmt, Pixel.Format? defFmt = null)
        {
            return _Implementation.ToInterop(fmt, defFmt);
        }

        public static Pixel.Format ToInterop(this ANDROIDBITMAP.Config fmt, bool isAlphaPremul = false, Pixel.Format? defFmt = null)
        {
            if (fmt == null) throw new ArgumentNullException(nameof(fmt));

            return _Implementation.ToInterop(fmt, isAlphaPremul, defFmt);
        }

        public static BitmapInfo ToInterop(this ANDROIDGFX.AndroidBitmapInfo info, Pixel.Format? defFmt = null)
        {
            return _Implementation.ToInterop(info, defFmt);
        }

        public static Adapters.AndroidFactory ToAndroidFactory(this BitmapInfo binfo, ANDROIDBITMAP.Config defCfg = null)
        {
            return new Adapters.AndroidFactory(binfo, defCfg);
        }

        public static void Mutate(this ANDROIDBITMAP bmp, Action<PointerBitmap> pinContext)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));            
            _Implementation.Mutate(bmp, pinContext);
        }

        public static IPointerBitmapOwner UsingPointerBitmap(this ANDROIDBITMAP bmp)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            return new Adapters.AndroidBitmapBits(bmp);
        }

        public static IMemoryBitmapOwner UsingMemoryBitmap(this ANDROIDBITMAP bmp)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));            
            return new Adapters.AndroidBitmapBits(bmp);
        }

        public static void SetPixels(this ANDROIDBITMAP dst, int x, int y, SpanBitmap src)
        {
            using var pinned = dst.UsingPointerBitmap();
            pinned.Bitmap.AsSpanBitmap().SetPixels(x, y, src);
        }

        public static void SetPixels(this SpanBitmap dst, int x, int y, ANDROIDBITMAP src)
        {
            using var pinned = src.UsingPointerBitmap();
            dst.SetPixels(x, y, pinned.Bitmap.AsSpanBitmap());
        }
        
        public static bool CopyTo(this SpanBitmap src, ANDROIDBITMAP dst)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            return _Implementation.CopyTo(src, dst);
        }

        public static bool CopyTo(this SpanBitmap src, ref ANDROIDBITMAP dst)
        {
            return _Implementation.CopyTo(src, ref dst);
        }

        public static bool CopyTo(this MemoryBitmap src, ANDROIDBITMAP dst)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            return _Implementation.CopyTo(src, dst);
        }

        public static bool CopyTo(this MemoryBitmap src, ref ANDROIDBITMAP dst)
        {
            return _Implementation.CopyTo(src, ref dst);
        }

        public static bool CopyTo(this ANDROIDBITMAP src, ref MemoryBitmap dst)
        {
            if (src == null) throw new NotImplementedException(nameof(src));
            return _Implementation.CopyTo(src, ref dst);
        }

        public static bool CopyTo<TPixel>(this ANDROIDBITMAP src, ref MemoryBitmap<TPixel> dst)
            where TPixel:unmanaged
        {
            if (src == null) throw new NotImplementedException(nameof(src));
            return _Implementation.CopyTo(src, ref dst);
        }

    }
}
