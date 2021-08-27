using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public static TPixel GetColor<TPixel>(System.Drawing.Color color)
            where TPixel : IPixelReflection<TPixel>
        {
            return default(TPixel).From(new BGRA32(color));
        }

        /// <summary>
        /// Callback that copies the pixels from src to dst, while aplying<br/>
        /// the appropiate conversion.
        /// </summary>
        /// <param name="src">The source pixels.</param>
        /// <param name="dst">The target pixels.</param>
        public delegate void ConverterCallback<TSrc,TDst>(ReadOnlySpan<TSrc> src, Span<TDst> dst);        

        public static ConverterCallback<Byte,Byte> GetConverter(Format srcFmt, Format dstFmt)
        {
            // direct converter
            if (srcFmt == dstFmt) return (a, b) => a.CopyTo(b);

            
            // common converters
            if (dstFmt == BGR24.Format)
            {
                if (srcFmt == RGBA32.Format)
                {
                    return (src, dst) =>
                    {
                        var srcxxx = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, RGBA32>(src);
                        var dstxxx = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, BGR24>(dst);

                        for(int i=0; i < srcxxx.Length; ++i)
                        {
                            dstxxx[i] = new BGR24(srcxxx[i]);
                        }
                    };
                }

                if (srcFmt == BGRA32.Format)
                {
                    return (src, dst) =>
                    {
                        var srcxxx = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, BGRA32>(src);
                        var dstxxx = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, BGR24>(dst);

                        for (int i = 0; i < srcxxx.Length; ++i)
                        {
                            dstxxx[i] = new BGR24(srcxxx[i]);
                        }
                    };
                }
            }

            if (dstFmt == BGRA32.Format)
            {
                if (srcFmt == BGR24.Format)
                {
                    return (src, dst) =>
                    {
                        var srcxxx = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, BGR24>(src);
                        var dstxxx = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, BGRA32>(dst);

                        for (int i = 0; i < srcxxx.Length; ++i)
                        {
                            dstxxx[i] = new BGRA32(srcxxx[i]);
                        }
                    };
                }
            }

            void _converter(ReadOnlySpan<Byte> src, Span<Byte> dst)
            {
                _ConvertPixels(dst, dstFmt, src, srcFmt);
            }

            return _converter;
        }


        public static ConverterCallback<Byte, Byte> GetConverter<TSrcPixel, TDstPixel>()
            where TSrcPixel : unmanaged, IConvertible
            where TDstPixel : unmanaged, IPixelReflection<TDstPixel>
        {
            if (typeof(TSrcPixel) == typeof(TDstPixel)) { return (a, b) => a.CopyTo(b); }

            void _converter(ReadOnlySpan<Byte> srcBytes, Span<Byte> dstBytes)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TSrcPixel>(srcBytes);
                var dst = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dstBytes);

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

            return _converter;            
        }


        /// <summary>
        /// Converts pixels by using <see cref="VectorRGBA"/> as an intermediate format.
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="dstFmt"></param>
        /// <param name="src"></param>
        /// <param name="srcFmt"></param>
        private static void _ConvertPixels(Span<Byte> dst, Format dstFmt, ReadOnlySpan<Byte> src, Format srcFmt)
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

                _ConvertPixels(block.Slice(0,blockSize), xsrc, srcFmt);
                _ConvertPixels(xdst, dstFmt, block);

                src = src.Slice(xsrc.Length);
                dst = dst.Slice(xdst.Length);

                blockSize = Math.Min(src.Length / srcLen, 1024);
            }
        }

        private static void _ConvertPixels(Span<VectorRGBA> dst, ReadOnlySpan<Byte> src, Format srcFmt)
        {
            switch (srcFmt.PackedFormat)
            {
                case Alpha8.Code: _ConvertPixels(dst, src.OfType<Alpha8>()); break;
                case Luminance8.Code: _ConvertPixels(dst, src.OfType<Luminance8>()); break;
                case Luminance16.Code: _ConvertPixels(dst, src.OfType<Luminance16>()); break;
                case LuminanceScalar.Code: _ConvertPixels(dst, src.OfType<LuminanceScalar>()); break;
                case RGB24.Code: _ConvertPixels(dst, src.OfType<RGB24>()); break;
                case BGR24.Code: _ConvertPixels(dst, src.OfType<BGR24>()); break;
                case BGR565.Code: _ConvertPixels(dst, src.OfType<BGR565>()); break;
                case BGRA5551.Code: _ConvertPixels(dst, src.OfType<BGRA5551>()); break;
                case BGRA4444.Code: _ConvertPixels(dst, src.OfType<BGRA4444>()); break;
                case BGRA32.Code: _ConvertPixels(dst, src.OfType<BGRA32>()); break;
                case BGRA32P.Code: _ConvertPixels(dst, src.OfType<BGRA32P>()); break;
                case RGBA32.Code: _ConvertPixels(dst, src.OfType<RGBA32>()); break;
                case RGBA32P.Code: _ConvertPixels(dst, src.OfType<RGBA32P>()); break;
                case ARGB32.Code: _ConvertPixels(dst, src.OfType<ARGB32>()); break;
                case VectorBGR.Code: _ConvertPixels(dst, src.OfType<VectorBGR>()); break;
                case VectorBGRA.Code: _ConvertPixels(dst, src.OfType<VectorBGRA>()); break;
                case VectorRGBA.Code: _ConvertPixels(dst, src.OfType<VectorRGBA>()); break;
                default: throw new NotImplementedException(nameof(srcFmt));
            }
        }

        private static void _ConvertPixels(Span<Byte> dst, Format dstFmt, ReadOnlySpan<VectorRGBA> src)
        {
            switch (dstFmt.PackedFormat)
            {
                case Alpha8.Code: _ConvertPixels(dst.OfType<Alpha8>(), src); break;
                case Luminance8.Code: _ConvertPixels(dst.OfType<Luminance8>(), src); break;
                case Luminance16.Code: _ConvertPixels(dst.OfType<Luminance16>(), src); break;
                case LuminanceScalar.Code: _ConvertPixels(dst.OfType<LuminanceScalar>(), src); break;
                case RGB24.Code: _ConvertPixels(dst.OfType<RGB24>(), src); break;
                case BGR24.Code: _ConvertPixels(dst.OfType<BGR24>(), src); break;
                case BGR565.Code: _ConvertPixels(dst.OfType<BGR565>(), src); break;
                case BGRA5551.Code: _ConvertPixels(dst.OfType<BGRA5551>(), src); break;
                case BGRA4444.Code: _ConvertPixels(dst.OfType<BGRA4444>(), src); break;
                case BGRA32.Code: _ConvertPixels(dst.OfType<BGRA32>(), src); break;
                case BGRA32P.Code: _ConvertPixels(dst.OfType<BGRA32P>(), src); break;
                case RGBA32.Code: _ConvertPixels(dst.OfType<RGBA32>(), src); break;
                case RGBA32P.Code: _ConvertPixels(dst.OfType<RGBA32P>(), src); break;
                case ARGB32.Code: _ConvertPixels(dst.OfType<ARGB32>(), src); break;
                case VectorBGR.Code: _ConvertPixels(dst.OfType<VectorBGR>(), src); break;
                case VectorBGRA.Code: _ConvertPixels(dst.OfType<VectorBGRA>(), src); break;
                case VectorRGBA.Code: _ConvertPixels(dst.OfType<VectorRGBA>(), src); break;
                
                default: throw new NotImplementedException(nameof(dstFmt));
            }
        }

        private static void _ConvertPixels(Span<VectorRGBA> dst, ReadOnlySpan<VectorRGBA> src)
        {
            src.CopyTo(dst);
        }

        private static void _ConvertPixels<TSrcPixel>(Span<VectorRGBA> dst, ReadOnlySpan<TSrcPixel> src)
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

        private static void _ConvertPixels<TDstPixel>(Span<TDstPixel> dst, ReadOnlySpan<VectorRGBA> src)
            where TDstPixel : unmanaged, IPixelReflection<TDstPixel>
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

        public static void LerpArray<TSrcPixel, TDstPixel>(ReadOnlySpan<TSrcPixel> left, ReadOnlySpan<TSrcPixel> right, float amount, Span<TDstPixel> dst)
            where TSrcPixel : unmanaged, IConvertible
            where TDstPixel : unmanaged, IPixelReflection<TDstPixel>
        {
            for(int i=0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector4.Lerp(left[i].ToVectorRGBA().RGBA, right[i].ToVectorRGBA().RGBA, amount);
                dst[i] = default(TDstPixel).From( new VectorRGBA(v));
            }
        }

        public static void LerpArray<TDstPixel>(ReadOnlySpan<System.Numerics.Vector3> left, ReadOnlySpan<System.Numerics.Vector3> right, float amount, Span<TDstPixel> dst)            
            where TDstPixel : unmanaged, IPixelReflection<TDstPixel>
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector3.Lerp(left[i], right[i], amount);
                dst[i] = default(TDstPixel).From(new VectorRGBA(v));
            }
        }

    }
}
