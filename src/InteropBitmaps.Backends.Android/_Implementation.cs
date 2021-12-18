using System;
using System.Collections.Generic;
using System.Linq;

using ANDROIDGFX = Android.Graphics;
using ANDROIDBITMAP = Android.Graphics.Bitmap;
using ANDROIDIMAGE = Android.Media.Image;

// interesting: ANDROIDMEDIA.FaceDetector

namespace InteropBitmaps
{
    static partial class _Implementation
    {
        #region clone & copy

        public static void Mutate(ANDROIDBITMAP bmp, Action<PointerBitmap> pinContext)
        {
            System.Diagnostics.Debug.Assert(bmp != null);

            if (!TryGetBitmapInfo(bmp.GetBitmapInfo(),bmp.IsPremultiplied, out var info)) throw new Diagnostics.PixelFormatNotSupportedException(bmp.GetBitmapInfo(), nameof(bmp));
            
            if (info.BitmapByteSize != bmp.ByteCount) throw new InvalidOperationException("Byte Size mismatch");

            var ptr = bmp.LockPixels();
            try { pinContext(new PointerBitmap(ptr, info, !bmp.IsMutable)); }
            finally { bmp.UnlockPixels(); }
        }

        public static bool Reshape(ref ANDROIDBITMAP bmp, ANDROIDBITMAP.Config fmt, int w, int h)
        {
            if (w <= 0 || w <= 0)
            {
                if (bmp == null) return false;
                System.Threading.Interlocked.Exchange(ref bmp, null)?.Dispose();
                return true;
            }
            
            if (bmp == null)
            {
                bmp = ANDROIDBITMAP.CreateBitmap(w, h, fmt);
                return true;
            }

            return Reshape(bmp, fmt, w, h);
        }

        public static bool Reshape(ANDROIDBITMAP bmp, ANDROIDBITMAP.Config fmt, int w, int h)
        {
            if (bmp == null) throw new ArgumentNullException(nameof(bmp));
            if (fmt == null) throw new Diagnostics.PixelFormatNotSupportedException(fmt, nameof(fmt));
            if (w <= 0) throw new ArgumentOutOfRangeException(nameof(w));
            if (h <= 0) throw new ArgumentOutOfRangeException(nameof(h));

            if (bmp.Width == w && bmp.Height == h && bmp.GetConfig() == fmt) return false;

            if (!bmp.IsMutable) throw new ArgumentException(nameof(bmp));

            bmp.Reconfigure(w, h, fmt);
            return true;
        }

        public static bool Reshape(ref ANDROIDBITMAP bmp, BitmapInfo info, ANDROIDBITMAP.Config defFmt)
        {
            var bmpCfg = ToAndroidBitmapConfig(info.PixelFormat, bmp != null ? bmp.GetConfig() : defFmt);

            return Reshape(ref bmp, bmpCfg, info.Width, info.Height);
        }

        public static bool Reshape(ANDROIDBITMAP bmp, BitmapInfo info, ANDROIDBITMAP.Config defFmt)
        {
            var bmpCfg = ToAndroidBitmapConfig(info.PixelFormat, bmp != null ? bmp.GetConfig() : defFmt);

            return Reshape(bmp, bmpCfg, info.Width, info.Height);
        }

        public static bool CopyTo(SpanBitmap src, ANDROIDBITMAP dst)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            if (!dst.IsMutable) throw new ArgumentException(nameof(dst));

            var refreshed = Reshape(dst, src.Info, null);
            _PerformCopy(src, dst);
            return refreshed;
        }        

        public static bool CopyTo(SpanBitmap src, ref ANDROIDBITMAP dst)
        {
            var refreshed = Reshape(ref dst, src.Info, null);
            _PerformCopy(src, dst);
            return refreshed;
        }

        private static void _PerformCopy(SpanBitmap src, ANDROIDBITMAP dst)
        {
            if (src.Info.IsEmpty) return;
            if (dst == null) return;

            if (dst.IsRecycled || !dst.IsMutable) throw new ArgumentNullException(nameof(dst));

            if (!TryGetBitmapInfo(dst.GetBitmapInfo(), dst.IsPremultiplied, out var nfo)) throw new Diagnostics.PixelFormatNotSupportedException((dst.GetBitmapInfo().Format, dst.Premultiplied), nameof(dst));            

            var ptr = dst.LockPixels();
            if (ptr == IntPtr.Zero) throw new ArgumentNullException("lock",nameof(dst));

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
            if (dst.IsEmpty) dst = new MemoryBitmap<System.Numerics.Vector3>(src.Width, src.Height, Pixel.BGR96F.Format);

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
