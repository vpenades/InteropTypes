using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

using InteropBitmaps;

namespace InteropTensors
{
    static class _Implementation
    {
        public static (T min, T max) GetMinMax<T>(this Span<T> span)
        {
            var comparer = Comparer<T>.Default;

            if (span.Length == 0) return (default, default);

            T min = span[0];
            T max = span[0];

            for (int i = 1; i < span.Length; ++i)
            {
                var v = span[i];

                if (comparer.Compare(min, v) < 0) min = v;
                if (comparer.Compare(max, v) > 0) max = v;
            }

            return (min, max);
        }


        public static SpanBitmap GetSpanBitmap<T>(IDenseTensor<T> tensor) where T : unmanaged
        {
            var dims = tensor.Dimensions;
            var width = dims[2];
            var height = dims[1];
            var fmt = Pixel.Format.GetFromDepthAndChannels(typeof(T), dims[3]);

            var info = new BitmapInfo(width, height, fmt);
            var data = System.Runtime.InteropServices.MemoryMarshal.Cast<T, Byte>(tensor.Span);

            return new SpanBitmap(data, info);
        }        

        public static bool TryGetBitmapBGR(IDenseTensor<float> src, out SpanBitmap<Vector3> bmp)
        {
            bmp = default;            

            var dims = src.Dimensions;

            if (dims.Length < 3 || dims.Length > 4) return false;
            if (dims.Length == 4)
            {
                if (dims[0] != 1) return false;
                dims = dims.Slice(1);
            }

            if (dims[2] != 3) return false;

            var data = System.Runtime.InteropServices.MemoryMarshal.Cast<float, byte>(src.Span);
            bmp = new SpanBitmap<Vector3>(data, dims[1], dims[0], Pixel.Standard.BGR96F);
            return true;
        }
        
        public static MemoryBitmap<Vector3> CreateBGRBitmap(SpanTensor4<Single> tensor, Func<Single[], Vector3> pixelFunc)
        {
            return CreateBGRBitmap(tensor[0], pixelFunc);
        }

        public static MemoryBitmap<Vector3> CreateBGRBitmap(SpanTensor3<Single> tensor, Func<Single[], Vector3> pixelFunc)
        {
            var midPixel = new Single[tensor.Dimensions[2]];

            var memory = new MemoryBitmap<Vector3>(tensor.Dimensions[1], tensor.Dimensions[0], Pixel.Standard.BGR96F);

            for (int y = 0; y < tensor.Dimensions[0]; ++y)
            {
                var row = tensor[y];

                for (int x = 0; x < row.Dimensions[0]; ++x)
                {
                    row[x].CopyTo(midPixel);
                    var dstPixel = pixelFunc(midPixel);
                    memory.SetPixel(x, y, dstPixel);
                }
            }

            return memory;
        }

        public static void FitPixels(SpanBitmap bgrSrc, SpanTensor2<Vector3> dst, float offset, float scale)
        {
            if (bgrSrc.PixelSize != 3) throw new ArgumentException(nameof(bgrSrc));

            Span<Vector3> vSrc = stackalloc Vector3[bgrSrc.Width];
            Span<Vector3> vDst0 = stackalloc Vector3[dst.BitmapWidth];
            Span<Vector3> vDst1 = stackalloc Vector3[dst.BitmapWidth];

            // expand bgrSrc bytes to vSrc (use V4s)
            // bilinear interpolate vSrc over vDst1 along X
            // bilinear interpolate vDst0 and vDst1 to dst (use V4s)
        }

        static void FitPixels(SpanBitmap<_bgr> src, SpanTensor2<Vector3> dst, float offset, float scale)
        {
            Span<Vector3> vSrc = stackalloc Vector3[src.Width];
            Span<Vector3> vDst0 = stackalloc Vector3[dst.BitmapWidth];
            Span<Vector3> vDst1 = stackalloc Vector3[dst.BitmapWidth];

            for(int dsty=0; dsty < dst.BitmapHeight; ++dsty)
            {
                var srcRow = src.GetPixelsScanline(dsty);
                for (int i = 0; i < vSrc.Length; ++i) vSrc[i] = srcRow[i].ToVector3();

                Lerp(vSrc, vDst1);
                



            }

            // expand bgrSrc bytes to vSrc (use V4s)
            // bilinear interpolate vSrc over vDst1 along X
            // bilinear interpolate vDst0 and vDst1 to dst (use V4s)
        }

        struct _bgr
        {
            public Byte B;
            public Byte G;
            public Byte R;
            public Vector3 ToVector3() => new Vector3(B, G, R);            
        }

        static void Lerp(ReadOnlySpan<Vector3> src, Span<Vector3> dst)
        {
            for(int dst0=0; dst0 < src.Length; ++dst0)
            {
                var src0 = (dst0+0) * src.Length / dst.Length;
                var src1 = (dst0+1) * src.Length / dst.Length;
                src1 = Math.Max(src1, src.Length - 1); // notice that if we're rescaling from a slice (as in larger rowstep) we could go full throttle

                var dst1 = (float)(src1 * dst.Length) / (float)src.Length;                

                dst[dst0] = Vector3.Lerp(src[src0], src[src1], dst1 - dst0);                
            }
        }

        public static void SetGrayPixels(SpanTensor2<Single> dst, PointerBitmap src, Single offset, Single scale)
        {
            if (src.Width != dst.BitmapWidth) throw new ArgumentException(nameof(dst.BitmapWidth));
            if (src.Height != dst.BitmapHeight) throw new ArgumentException(nameof(dst.BitmapHeight));

            var dstSpan = dst.AsSpanBitmap();
            var srcSpan = src.AsSpanBitmap();

            if (srcSpan.PixelFormat.ByteCount == 1)
            {
                SpanBitmap.CopyPixels(srcSpan.OfType<Byte>(), dstSpan, (offset, scale), (0, 1));
            }

            

        }
    }
}
