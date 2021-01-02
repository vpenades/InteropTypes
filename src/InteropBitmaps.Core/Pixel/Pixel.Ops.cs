using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public static TPixel GetColor<TPixel>(System.Drawing.Color color)
            where TPixel : IFactory<TPixel>
        {
            return default(TPixel).From(new BGRA32(color));
        }


        public static void ConvertPixels(Span<Byte> dst, Format dstFmt, ReadOnlySpan<Byte> src, Format srcFmt)
        {
            if (dstFmt == srcFmt) { src.CopyTo(dst); return; }

            var srcLen = srcFmt.ByteCount;
            var dstLen = dstFmt.ByteCount;

            int blockSize = Math.Min(src.Length / srcLen, 1024);

            Span<VectorRGBA> block = stackalloc VectorRGBA[blockSize];

            while (dst.Length > 0)
            {
                var xsrc = src.Slice(0, blockSize * srcLen);
                var xdst = dst.Slice(0, blockSize * dstLen);

                ConvertPixels(block.Slice(0,blockSize), xsrc, srcFmt);
                ConvertPixels(xdst, dstFmt, block);

                src = src.Slice(xsrc.Length);
                dst = dst.Slice(xdst.Length);

                blockSize = Math.Min(src.Length / srcLen, 1024);
            }

        }

        private static void ConvertPixels(Span<VectorRGBA> dst, ReadOnlySpan<Byte> src, Format srcFmt)
        {
            switch (srcFmt.PackedFormat)
            {
                case Alpha8.Code: ConvertPixels(dst, src.OfType<Alpha8>()); break;
                case Luminance8.Code: ConvertPixels(dst, src.OfType<Luminance8>()); break;
                case Luminance16.Code: ConvertPixels(dst, src.OfType<Luminance16>()); break;
                case StdLuminance.Code: ConvertPixels(dst, src.OfType<StdLuminance>()); break;
                case RGB24.Code: ConvertPixels(dst, src.OfType<RGB24>()); break;
                case BGR24.Code: ConvertPixels(dst, src.OfType<BGR24>()); break;
                case BGR565.Code: ConvertPixels(dst, src.OfType<BGR565>()); break;
                case BGRA5551.Code: ConvertPixels(dst, src.OfType<BGRA5551>()); break;
                case BGRA4444.Code: ConvertPixels(dst, src.OfType<BGRA4444>()); break;
                case BGRA32.Code: ConvertPixels(dst, src.OfType<BGRA32>()); break;
                case RGBA32.Code: ConvertPixels(dst, src.OfType<RGBA32>()); break;
                case ARGB32.Code: ConvertPixels(dst, src.OfType<ARGB32>()); break;
                case VectorBGR.Code: ConvertPixels(dst, src.OfType<VectorBGR>()); break;
                case VectorBGRA.Code: ConvertPixels(dst, src.OfType<VectorBGRA>()); break;
                case VectorRGBA.Code: ConvertPixels(dst, src.OfType<VectorRGBA>()); break;
                default: throw new NotImplementedException(nameof(srcFmt));
            }
        }

        private static void ConvertPixels(Span<Byte> dst, Format dstFmt, ReadOnlySpan<VectorRGBA> src)
        {
            switch (dstFmt.PackedFormat)
            {
                case Alpha8.Code: ConvertPixels(dst.OfType<Alpha8>(), src); break;
                case Luminance8.Code: ConvertPixels(dst.OfType<Luminance8>(), src); break;
                case Luminance16.Code: ConvertPixels(dst.OfType<Luminance16>(), src); break;
                case StdLuminance.Code: ConvertPixels(dst.OfType<StdLuminance>(), src); break;
                case RGB24.Code: ConvertPixels(dst.OfType<RGB24>(), src); break;
                case BGR24.Code: ConvertPixels(dst.OfType<BGR24>(), src); break;
                case BGR565.Code: ConvertPixels(dst.OfType<BGR565>(), src); break;
                case BGRA5551.Code: ConvertPixels(dst.OfType<BGRA5551>(), src); break;
                case BGRA4444.Code: ConvertPixels(dst.OfType<BGRA4444>(), src); break;
                case BGRA32.Code: ConvertPixels(dst.OfType<BGRA32>(), src); break;
                case RGBA32.Code: ConvertPixels(dst.OfType<RGBA32>(), src); break;
                case ARGB32.Code: ConvertPixels(dst.OfType<ARGB32>(), src); break;
                case VectorBGR.Code: ConvertPixels(dst.OfType<VectorBGR>(), src); break;
                case VectorBGRA.Code: ConvertPixels(dst.OfType<VectorBGRA>(), src); break;
                case VectorRGBA.Code: ConvertPixels(dst.OfType<VectorRGBA>(), src); break;
                default: throw new NotImplementedException(nameof(dstFmt));
            }
        }

        private static void ConvertPixels(Span<VectorRGBA> dst, ReadOnlySpan<VectorRGBA> src)
        {
            src.CopyTo(dst);
        }

        private static void ConvertPixels<TSrcPixel>(Span<VectorRGBA> dst, ReadOnlySpan<TSrcPixel> src)
            where TSrcPixel : unmanaged, IConvertible
        {
            if (typeof(TSrcPixel) == typeof(VectorRGBA))
            {
                var srcBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(src);
                var dstBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<VectorRGBA, Byte>(dst);
                srcBytes.CopyTo(dstBytes);
                return;
            }

            for (int i = 0; i < dst.Length; ++i) { dst[i] = src[i].ToVectorRGBA(); }
        }

        private static void ConvertPixels<TDstPixel>(Span<TDstPixel> dst, ReadOnlySpan<VectorRGBA> src)
            where TDstPixel : unmanaged, IFactory<TDstPixel>
        {
            if (typeof(VectorRGBA) == typeof(TDstPixel))
            {
                var srcBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<VectorRGBA, Byte>(src);
                var dstBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, Byte>(dst);
                srcBytes.CopyTo(dstBytes);
                return;
            }

            var factory = default(TDstPixel);

            for (int i = 0; i < dst.Length; ++i) { dst[i] = factory.From(src[i]); }
        }


        public static void ConvertPixels<TSrcPixel, TDstPixel>(Span<TDstPixel> dst, ReadOnlySpan<TSrcPixel> src)
            where TSrcPixel : unmanaged, IConvertible
            where TDstPixel : unmanaged, IFactory<TDstPixel>
        {
            if (typeof(TSrcPixel) == typeof(TDstPixel))
            {
                var srcBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(src);
                var dstBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, Byte>(dst);
                srcBytes.CopyTo(dstBytes);
                return;
            }

            int blockSize = Math.Min(src.Length, 1024);

            Span<VectorRGBA> block = stackalloc VectorRGBA[blockSize];

            var factory = default(TDstPixel);

            while (dst.Length > 0)
            {
                for (int i = 0; i < blockSize; ++i) { block[i] = src[i].ToVectorRGBA(); }
                for (int i = 0; i < blockSize; ++i) { dst[i] = factory.From(block[i]); }

                src = src.Slice(blockSize);
                dst = dst.Slice(blockSize);

                blockSize = Math.Min(src.Length, 1024);
            }
        }


        public static void LerpArray<TSrcPixel, TDstPixel>(ReadOnlySpan<TSrcPixel> left, ReadOnlySpan<TSrcPixel> right, float amount, Span<TDstPixel> dst)
            where TSrcPixel : unmanaged, IConvertible
            where TDstPixel : unmanaged, IFactory<TDstPixel>
        {
            for(int i=0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector4.Lerp(left[i].ToVectorRGBA().RGBA, right[i].ToVectorRGBA().RGBA, amount);
                dst[i] = default(TDstPixel).From( new VectorRGBA(v));
            }
        }

        public static void LerpArray<TDstPixel>(ReadOnlySpan<System.Numerics.Vector3> left, ReadOnlySpan<System.Numerics.Vector3> right, float amount, Span<TDstPixel> dst)            
            where TDstPixel : unmanaged, IFactory<TDstPixel>
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector3.Lerp(left[i], right[i], amount);
                dst[i] = default(TDstPixel).From(new VectorRGBA(v));
            }
        }

    }
}
