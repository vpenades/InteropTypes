using System;
using System.Drawing;

using GDIICON = System.Drawing.Icon;
using GDIIMAGE = System.Drawing.Image;
using GDIBITMAP = System.Drawing.Bitmap;
using GDIPTR = System.Drawing.Imaging.BitmapData;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
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

        public static PointerBitmap.ISource UsingPointerBitmap(this GDIBITMAP bmp)
        {
            return new Adapters.GDIMemoryManager(bmp);
        }

        public static MemoryBitmap.ISource UsingMemoryBitmap(this GDIBITMAP bmp)
        {
            return new Adapters.GDIMemoryManager(bmp);
        }

        #endregion

        #region As SpanBitmap

        public static BitmapInfo GetBitmapInfo(this GDIPTR data)
        {
            var binfo = _Implementation.GetBitmapInfo(data);
            System.Diagnostics.Debug.Assert(binfo.StepByteSize == data.Stride);
            return binfo;
        }

        public static PointerBitmap AsPointerBitmapDangerous(this GDIPTR data)
        {
            var info = _Implementation.GetBitmapInfo(data);
            System.Diagnostics.Debug.Assert(info.StepByteSize == data.Stride);
            return new PointerBitmap(data.Scan0, info);
        }

        public static SpanBitmap AsSpanBitmapDangerous(this GDIPTR data)
        {
            return data.AsPointerBitmapDangerous();
        }

        public static SpanBitmap<TPixel> AsSpanBitmapDangerous<TPixel>(this GDIPTR data)
            where TPixel: unmanaged
        {
            return data.AsPointerBitmapDangerous().AsSpanBitmapOfType<TPixel>();
        }

        #endregion        

        #region Generic API
        
        public static void WriteAsSpanBitmap(this GDIBITMAP bmp, SpanBitmap.Action1 action)
        {
            _Implementation.WriteAsSpanBitmap(bmp, action);
        }

        public static void MutateAsGDI<TPixel>(this SpanBitmap<TPixel> bmp, Action<System.Drawing.Graphics> mutator) where TPixel : unmanaged
        {
            bmp.WithGDI().Draw(mutator);
        }

        public static void SetPixels(this GDIBITMAP dst, int dstx, int dsty, in SpanBitmap src)
        {
            _Implementation.Transfer(src, dst, (s, d) => d.SetPixels(dstx, dsty, s));
        }

        public static void SetPixels(this SpanBitmap dst, int dstx, int dsty, in GDIBITMAP src)
        {
            _Implementation.Transfer(src, dst, (s, d) => d.SetPixels(dstx, dsty, s));
        }

        public static void SetPixels(this GDIBITMAP dst, System.Numerics.Matrix3x2 dstSRT, in SpanBitmap src)
        {
            _Implementation.Transfer(src, dst, (s, d) => d.SetPixels(dstSRT, s));
        }

        public static void SetPixels(this SpanBitmap dst, System.Numerics.Matrix3x2 dstSRT, in GDIBITMAP src)
        {
            _Implementation.Transfer(src, dst, (s, d) => d.SetPixels(dstSRT, s));
        }

        public static void FitPixels(this GDIBITMAP dst, in SpanBitmap src)
        {
            _Implementation.Transfer(src, dst, (s, d) => d.FitPixels(s));
        }

        public static void FitPixels(this SpanBitmap dst, in GDIBITMAP src)
        {
            _Implementation.Transfer(src, dst, (s, d) => d.FitPixels(s));
        }

        public static bool CopyTo(this GDIBITMAP src, ref MemoryBitmap dst, PixelFormat? fmtOverride = null)
        {
            if (src == null)
            {
                // if both are empty, exit with no changes
                if (dst.IsEmpty) return false;

                // if src is empty, clear dst
                dst = default; return true;
            }

            var refreshed = _Implementation.Reshape(ref dst, src, fmtOverride);
            dst.AsSpanBitmap().SetPixels(0, 0, src);
            return refreshed;
        }

        public static bool CopyTo(this GDIPTR src, ref MemoryBitmap dst, PixelFormat? fmtOverride = null)
        {
            if (src == null)
            {
                // if both are empty, exit with no changes
                if (dst.IsEmpty) return false;

                // if src is empty, clear dst
                dst = default; return true;
            }

            var refreshed = _Implementation.Reshape(ref dst, src, fmtOverride);
            dst.SetPixels(0, 0, src.AsSpanBitmapDangerous());
            return refreshed;
        }

        #endregion

        #region Specific API

        public static MemoryBitmap ToMemoryBitmap(this TextureBrush brush, PixelFormat? fmtOverride = null)
        {
            return brush.Image.ToMemoryBitmap(fmtOverride);
        }

        public static MemoryBitmap ToMemoryBitmap(this GDIIMAGE img, PixelFormat? fmtOverride = null)
        {
            using (var bmp = new GDIBITMAP(img))
            {
                return bmp.ToMemoryBitmap(fmtOverride);
            }
        }

        public static MemoryBitmap ToMemoryBitmap(this GDIICON icon, PixelFormat? fmtOverride = null)
        {
            using (var bmp = icon.ToBitmap())
            {
                return bmp.ToMemoryBitmap(fmtOverride);
            }
        }

        public static MemoryBitmap ToMemoryBitmap(this GDIBITMAP bmp, PixelFormat? fmtOverride = null)
        {
            MemoryBitmap dst = default;
            return CopyTo(bmp, ref dst, fmtOverride) ? dst : throw new ArgumentException(nameof(bmp));
        }

        public static GDIBITMAP UsingAsGDIBitmap(this PointerBitmap bmp)
        {
            return _Implementation.WrapAsGDIBitmap(bmp);
        }

        public static GDIBITMAP ToGDIBitmap(this MemoryBitmap bmp)
        {
            return _Implementation.CloneAsGDIBitmap(bmp.AsSpanBitmap());
        }


        #endregion
    }
}
