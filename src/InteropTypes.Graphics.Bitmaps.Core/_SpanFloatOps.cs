using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    using Diagnostics;

    // TODO: implement https://devblogs.microsoft.com/dotnet/hardware-intrinsics-in-net-core/

    static class _SpanSingleExtensions
    {
              

        public static void CopyPixels(ReadOnlySpan<Single> src, Span<Byte> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);
            
            var srcVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(src.Slice(0, dst.Length & ~3));

            var minVec = new Vector4(range.min);
            var maxVec = new Vector4(range.max);
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < srcVec.Length; ++x)
            {
                var v = (offVec + srcVec[x]) * transform.scale;

                v = Vector4.Max(v, minVec);
                v = Vector4.Min(v, maxVec);

                var i = x * 4;

                dst[i + 0] = (Byte)v.X;
                dst[i + 1] = (Byte)v.Y;
                dst[i + 2] = (Byte)v.Z;
                dst[i + 3] = (Byte)v.W;
            }

            for (int x = srcVec.Length * 4; x < dst.Length; ++x)
            {
                var v = (transform.offset + src[x]) * transform.scale;

                if (v < range.min) v = range.min;
                if (v > range.max) v = range.max;

                dst[x] = (Byte)v;
            }
        }

        public static void CopyPixels(ReadOnlySpan<Single> src, Span<Byte> dst, (Single offset, Single scale) transform)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);

            var srcVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(src.Slice(0, dst.Length & ~3));
            
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < srcVec.Length; ++x)
            {
                var v = (offVec + srcVec[x]) * transform.scale;
                
                var i = x * 4;

                dst[i + 0] = (Byte)v.X;
                dst[i + 1] = (Byte)v.Y;
                dst[i + 2] = (Byte)v.Z;
                dst[i + 3] = (Byte)v.W;
            }

            for (int x = srcVec.Length * 4; x < dst.Length; ++x)
            {
                var v = (transform.offset + src[x]) * transform.scale;                

                dst[x] = (Byte)v;
            }
        }

        public static void CopyPixels(ReadOnlySpan<Byte> src, Span<Single> dst, (Single offset, Single scale) transform, (Single min, Single max) range)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);

            var dstVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(dst.Slice(0, dst.Length & ~3));

            var minVec = new Vector4(range.min);
            var maxVec = new Vector4(range.max);
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < dstVec.Length; ++x)
            {
                var i = x * 4;

                var v = (offVec + new Vector4(src[i + 0], src[i + 1], src[i + 2], src[i + 3])) * transform.scale;

                v = Vector4.Max(v, minVec);
                v = Vector4.Min(v, maxVec);

                dstVec[x] = v;
            }

            for (int x = dstVec.Length * 4; x < dst.Length; ++x)
            {
                var v = (transform.offset + src[x]) * transform.scale;

                if (v < range.min) v = range.min;
                if (v > range.max) v = range.max;

                dst[x] = v;
            }
        }

        public static void CopyPixels(ReadOnlySpan<Byte> src, Span<Single> dst, (Single offset, Single scale) transform)
        {
            Guard.AreEqual(nameof(dst), dst.Length, src.Length);

            var dstVec = System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(dst.Slice(0, dst.Length & ~3));            
            var offVec = new Vector4(transform.offset);

            for (int x = 0; x < dstVec.Length; ++x)
            {
                var i = x * 4;

                dstVec[x] = (offVec + new Vector4(src[i + 0], src[i + 1], src[i + 2], src[i + 3])) * transform.scale;
            }

            for (int x = dstVec.Length * 4; x < dst.Length; ++x)
            {
                dst[x] = (transform.offset + src[x]) * transform.scale;
            }
        }

        public static void LerpArray<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, float amount, Span<T> dst)
            where T:unmanaged
        {
            if (typeof(T) == typeof(float))
            {
                Vector4Streaming.Lerp
                    (System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(left)
                    , System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(right)
                    , amount
                    , System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(dst)
                    );

                return;
            }

            throw new NotImplementedException();
        }        
    }
}
