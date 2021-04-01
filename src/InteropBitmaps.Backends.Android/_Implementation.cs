using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ANDROIDGFX = Android.Graphics;
using ANDROIDBITMAP = Android.Graphics.Bitmap;
using ANDROIDIMAGE = Android.Media.Image;

// interesting: ANDROIDMEDIA.FaceDetector

namespace InteropBitmaps
{
    static class _Implementation
    {
        #region format conversions

        public static Pixel.Format ToInterop(ANDROIDGFX.Format fmt, Pixel.Format? defFmt = null)
        {
            switch (fmt)
            {
                case ANDROIDGFX.Format.A8: return Pixel.Alpha8.Format;
                case ANDROIDGFX.Format.L8: return Pixel.Luminance8.Format;
                case ANDROIDGFX.Format.Rgb565: return Pixel.BGR565.Format;
                case ANDROIDGFX.Format.Rgba4444: return Pixel.BGRA4444.Format;
                case ANDROIDGFX.Format.Rgb888: return Pixel.RGB24.Format;
                case ANDROIDGFX.Format.Rgba8888: return Pixel.RGBA32.Format;                
                default: if (defFmt.HasValue) return defFmt.Value; break;
            }

            throw new NotImplementedException($"{fmt}");
        }

        public static Pixel.Format ToInterop(ANDROIDBITMAP.Config fmt,bool isPremultiplied, Pixel.Format? defFmt = null)
        {
            if (isPremultiplied) throw new NotSupportedException();

            if (fmt == ANDROIDBITMAP.Config.Alpha8) return Pixel.Alpha8.Format;
            if (fmt == ANDROIDBITMAP.Config.Rgb565) return Pixel.BGR565.Format;
            if (fmt == ANDROIDBITMAP.Config.Argb4444) return Pixel.BGRA4444.Format;
            if (fmt == ANDROIDBITMAP.Config.Argb8888) return Pixel.RGBA32.Format;
            if (defFmt.HasValue) return defFmt.Value;

            throw new NotImplementedException($"{fmt}");
        }

        public static ANDROIDBITMAP.Config ToAndroidBitmapConfig(Pixel.Format fmt, ANDROIDBITMAP.Config defval = null)
        {
            switch (fmt.PackedFormat)
            {
                case Pixel.Alpha8.Code: return ANDROIDBITMAP.Config.Alpha8;
                case Pixel.BGR565.Code: return ANDROIDBITMAP.Config.Rgb565;
                case Pixel.BGRA4444.Code: return ANDROIDBITMAP.Config.Argb4444;
                case Pixel.RGBA32.Code: return ANDROIDBITMAP.Config.Argb8888;
                default: return defval;
            }
        }

        public static BitmapInfo ToInterop(ANDROIDGFX.AndroidBitmapInfo info, Pixel.Format? defFmt = null)
        {
            var fmt = ToInterop(info.Format, defFmt);
            return new BitmapInfo((int)info.Width, (int)info.Height, fmt, (int)info.Stride);
        }

        #endregion

        #region clone & copy

        public static void Mutate(ANDROIDBITMAP bmp, Action<PointerBitmap> pinContext)
        {
            System.Diagnostics.Debug.Assert(bmp != null);

            var info = ToInterop(bmp.GetBitmapInfo());

            if (info.BitmapByteSize != bmp.ByteCount) throw new InvalidOperationException("Byte Size mismatch");

            var ptr = bmp.LockPixels();
            try { pinContext(new PointerBitmap(ptr, info, !bmp.IsMutable)); }
            finally { bmp.UnlockPixels(); }
        }

        public static bool Reconfigure(ref ANDROIDBITMAP bmp, ANDROIDBITMAP.Config fmt, int w, int h)
        {
            if (w <= 0 || w <= 0)
            {
                if (bmp == null) return false;                
                bmp.Dispose();
                bmp = null;
                return true;
            }
            
            if (bmp == null)
            {
                bmp = ANDROIDBITMAP.CreateBitmap(w, h, fmt);
                return true;
            }

            return Reconfigure(bmp, fmt, w, h);
        }

        public static bool Reconfigure(ANDROIDBITMAP bmp, ANDROIDBITMAP.Config fmt, int w, int h)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            if (fmt == null) throw new ArgumentNullException(nameof(fmt));
            if (w <= 0) throw new ArgumentOutOfRangeException(nameof(w));
            if (h <= 0) throw new ArgumentOutOfRangeException(nameof(h));

            if (bmp.Width == w && bmp.Height == h && bmp.GetConfig() == fmt) return false;

            if (!bmp.IsMutable) throw new ArgumentException(nameof(bmp));

            bmp.Reconfigure(w, h, fmt);
            return true;
        }

        public static bool Reconfigure(ref ANDROIDBITMAP bmp, BitmapInfo info, ANDROIDBITMAP.Config defFmt)
        {
            var bmpCfg = ToAndroidBitmapConfig(info.PixelFormat, bmp != null ? bmp.GetConfig() : defFmt);

            return Reconfigure(ref bmp, bmpCfg, info.Width, info.Height);
        }

        public static bool Reconfigure(ANDROIDBITMAP bmp, BitmapInfo info, ANDROIDBITMAP.Config defFmt)
        {
            var bmpCfg = ToAndroidBitmapConfig(info.PixelFormat, bmp != null ? bmp.GetConfig() : defFmt);

            return Reconfigure(bmp, bmpCfg, info.Width, info.Height);
        }

        public static bool CopyTo(SpanBitmap src, ANDROIDBITMAP dst)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            if (!dst.IsMutable) throw new ArgumentException(nameof(dst));

            var refreshed = Reconfigure(dst, src.Info, null);
            _PerformCopy(src, dst);
            return refreshed;
        }        

        public static bool CopyTo(SpanBitmap src, ref ANDROIDBITMAP dst)
        {
            var refreshed = Reconfigure(ref dst, src.Info, null);
            _PerformCopy(src, dst);
            return refreshed;
        }

        private static void _PerformCopy(SpanBitmap src, ANDROIDBITMAP dst)
        {
            if (src.Info.IsEmpty) return;
            if (dst == null) return;

            if (dst.IsRecycled || !dst.IsMutable) throw new ArgumentNullException(nameof(dst));

            var nfo = ToInterop(dst.GetBitmapInfo());
            var ptr = dst.LockPixels();

            try { new PointerBitmap(ptr, nfo).AsSpanBitmap().SetPixels(0, 0, src); }
            finally { dst.UnlockPixels(); }
        }



        public static bool CopyTo(ANDROIDBITMAP src, ref MemoryBitmap dst)
        {
            var info = src.GetBitmapInfo().ToInterop();

            var ptr = src.LockPixels();
            try { return new PointerBitmap(ptr, info).CopyTo(ref dst); }
            finally { src.UnlockPixels(); }
        }

        public static bool CopyTo<TPixel>(ANDROIDBITMAP src, ref MemoryBitmap<TPixel> dst)
            where TPixel : unmanaged
        {
            var info = src.GetBitmapInfo().ToInterop();

            var ptr = src.LockPixels();
            try { return new PointerBitmap(ptr, info).CopyTo(ref dst); }
            finally { src.UnlockPixels(); }
        }

        public static void CopyTo(this ANDROIDIMAGE src, ref MemoryBitmap dst)
        {
            if (dst.Width != src.Width || dst.Height != src.Height) dst = default;
            if (dst.IsEmpty) dst = new MemoryBitmap(src.Width, src.Height, Pixel.BGR24.Format);

            src._CopyYuv445345To(dst.AsSpanBitmap());
        }

        public static void CopyTo(this ANDROIDIMAGE src, ref MemoryBitmap<System.Numerics.Vector3> dst)
        {
            if (dst.Width != src.Width || dst.Height != src.Height) dst = default;
            if (dst.IsEmpty) dst = new MemoryBitmap<System.Numerics.Vector3>(src.Width, src.Height, Pixel.VectorBGR.Format);

            src._CopyYuv445345To(dst.AsSpanBitmap());
        }        

        private static void _CopyYuv445345To(this ANDROIDIMAGE src, SpanBitmap<System.Numerics.Vector3> dst)
        {
            // http://werner-dittmann.blogspot.com/2016/03/solving-some-mysteries-about-androids.html
            // http://werner-dittmann.blogspot.com/2016/03/using-android-renderscript-to-convert.html

            if (src == null) throw new ArgumentNullException(nameof(src));
            if (src.Format != ANDROIDGFX.ImageFormatType.Yuv420888) throw new ArgumentNullException(nameof(src.Format));

            var planes = src.GetPlanes();

            var yPlane = planes[0].Buffer;
            var uPlane = planes[1].Buffer;
            var vPlane = planes[2].Buffer;

            int total = yPlane.Capacity();
            int uvCapacity = uPlane.Capacity();
            int width = planes[0].RowStride;

            int yPos = 0;
            for (int y = 0; y < src.Height; y++)
            {
                int uvPos = (y >> 1) * width;

                var dstRow = dst.UseScanlinePixels(y);

                for (int x = 0; x < width; x++)
                {
                    if (uvPos >= uvCapacity - 1) break;
                    if (yPos >= total) break;

                    int y1 = yPlane.Get(yPos++) & 0xff;

                    /*
                      The ordering of the u (Cb) and v (Cr) bytes inside the planes is a
                      bit strange. The _first_ byte of the u-plane and the _second_ byte
                      of the v-plane build the u/v pair and belong to the first two pixels
                      (y-bytes), thus usual YUV 420 behavior. What the Android devs did 
                      here (IMHO): just copy the interleaved NV21 U/V data to two planes
                      but keep the offset of the interleaving.
                     */

                    int u = (uPlane.Get(uvPos) & 0xff) - 128;
                    int v = (vPlane.Get(uvPos + 1) & 0xff) - 128;
                    if ((x & 1) == 1) { uvPos += 2; }

                    // This is the integer variant to convert YCbCr to RGB, NTSC values.
                    // formulae found at
                    // https://software.intel.com/en-us/android/articles/trusted-tools-in-the-new-android-world-optimization-techniques-from-intel-sse-intrinsics-to
                    // and on StackOverflow etc.


                    int y1192 = 1192 * y1;
                    int r = (y1192 + 1634 * v);
                    int g = (y1192 - 833 * v - 400 * u);
                    int b = (y1192 + 2066 * u);

                    r = (r < 0) ? 0 : ((r > 262143) ? 262143 : r);
                    g = (g < 0) ? 0 : ((g > 262143) ? 262143 : g);
                    b = (b < 0) ? 0 : ((b > 262143) ? 262143 : b);

                    /*
                    mRgbBuffer[bufferIndex++] = ((r << 6) & 0xff0000) |
                                                ((g >> 2) & 0xff00) |
                                                ((b >> 10) & 0xff);*/

                    dstRow[x] = new System.Numerics.Vector3(r, g, b) / 262143.0f;
                }
            }
        }

        private static void _CopyYuv445345To(this ANDROIDIMAGE src, SpanBitmap dst)
        {
            // http://werner-dittmann.blogspot.com/2016/03/solving-some-mysteries-about-androids.html

            if (src == null) throw new ArgumentNullException(nameof(src));
            if (src.Format != ANDROIDGFX.ImageFormatType.Yuv420888) throw new ArgumentNullException(nameof(src.Format));
            if (dst.PixelFormat != Pixel.BGR24.Format) throw new ArgumentNullException(nameof(dst.PixelFormat));

            var planes = src.GetPlanes();

            var yPlane = planes[0].Buffer;
            var uPlane = planes[1].Buffer;
            var vPlane = planes[2].Buffer;

            int total = yPlane.Capacity();
            int uvCapacity = uPlane.Capacity();
            int width = planes[0].RowStride;

            int yPos = 0;
            for (int y = 0; y < src.Height; y++)
            {
                int uvPos = (y >> 1) * width;

                var dstRow = dst.UseScanlineBytes(y);

                for (int x = 0; x < width; x++)
                {
                    if (uvPos >= uvCapacity - 1) break;
                    if (yPos >= total) break;

                    int y1 = yPlane.Get(yPos++) & 0xff;

                    /*
                      The ordering of the u (Cb) and v (Cr) bytes inside the planes is a
                      bit strange. The _first_ byte of the u-plane and the _second_ byte
                      of the v-plane build the u/v pair and belong to the first two pixels
                      (y-bytes), thus usual YUV 420 behavior. What the Android devs did 
                      here (IMHO): just copy the interleaved NV21 U/V data to two planes
                      but keep the offset of the interleaving.
                     */

                    int u = (uPlane.Get(uvPos) & 0xff) - 128;
                    int v = (vPlane.Get(uvPos + 1) & 0xff) - 128;
                    if ((x & 1) == 1) { uvPos += 2; }

                    // This is the integer variant to convert YCbCr to RGB, NTSC values.
                    // formulae found at
                    // https://software.intel.com/en-us/android/articles/trusted-tools-in-the-new-android-world-optimization-techniques-from-intel-sse-intrinsics-to
                    // and on StackOverflow etc.


                    int y1192 = 1192 * y1;
                    int r = (y1192 + 1634 * v);
                    int g = (y1192 - 833 * v - 400 * u);
                    int b = (y1192 + 2066 * u);

                    r = (r < 0) ? 0 : ((r > 262143) ? 262143 : r);
                    g = (g < 0) ? 0 : ((g > 262143) ? 262143 : g);
                    b = (b < 0) ? 0 : ((b > 262143) ? 262143 : b);

                    dstRow[x * 3 + 0] = (Byte)(b >> 10);
                    dstRow[x * 3 + 1] = (Byte)(g >> 10);
                    dstRow[x * 3 + 2] = (Byte)(r >> 10);
                }
            }
        }        

        #endregion
    }
}
