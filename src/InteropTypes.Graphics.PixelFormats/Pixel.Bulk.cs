using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public static TPixel GetColor<TPixel>(System.Drawing.Color color)
            where TPixel : IPixelFactory<Pixel.BGRA32, TPixel>
        {
            return default(TPixel).From(new BGRA32(color));
        }

        public static bool TryGetFormatAsRGBX(Format fmt, out Format newFmt)
        {
            newFmt = default;

            switch(fmt.PackedFormat)
            {                
                case RGB24.Code: newFmt = RGB24.Format; break;
                case BGR24.Code: newFmt = RGB24.Format; break;

                case RGBA32.Code: newFmt = RGBA32.Format; break;
                case BGRA32.Code: newFmt = RGBA32.Format; break;
                case ARGB32.Code: newFmt = RGBA32.Format; break;
                case RGBP32.Code: newFmt = RGBP32.Format; break;
                case BGRP32.Code: newFmt = RGBP32.Format; break;

                case RGB96F.Code: newFmt = RGB96F.Format; break;
                case BGR96F.Code: newFmt = RGB96F.Format; break;

                case RGBA128F.Code: newFmt = RGBA128F.Format; break;
                case BGRA128F.Code: newFmt = RGBA128F.Format; break;
                case RGBP128F.Code: newFmt = RGBP128F.Format; break;
                case BGRP128F.Code: newFmt = BGRP128F.Format; break;
            }

            return newFmt != default;            
        }

        public static bool TryGetFormatAsBGRX(Format fmt, out Format newFmt)
        {
            newFmt = default;

            switch (fmt.PackedFormat)
            {
                case RGB24.Code: newFmt = BGR24.Format; break;
                case BGR24.Code: newFmt = BGR24.Format; break;

                case RGBA32.Code: newFmt = BGRA32.Format; break;
                case BGRA32.Code: newFmt = BGRA32.Format; break;
                case ARGB32.Code: newFmt = BGRA32.Format; break;
                case RGBP32.Code: newFmt = BGRA32.Format; break;
                case BGRP32.Code: newFmt = BGRA32.Format; break;

                case RGB96F.Code: newFmt = BGR96F.Format; break;
                case BGR96F.Code: newFmt = BGR96F.Format; break;

                case RGBA128F.Code: newFmt = BGRA128F.Format; break;
                case BGRA128F.Code: newFmt = BGRA128F.Format; break;
                case RGBP128F.Code: newFmt = RGBP128F.Format; break;
                case BGRP128F.Code: newFmt = BGRP128F.Format; break;
            }

            return newFmt != default;
        }        

        /// <summary>
        /// Callback that copies the pixels from src to dst,<br/>
        /// while aplying the appropiate conversion.
        /// </summary>
        /// <param name="src">The source pixels.</param>
        /// <param name="dst">The target pixels.</param>
        public delegate void BulkConverterCallback<TSrc,TDst>(ReadOnlySpan<TSrc> src, Span<TDst> dst);

        

        public static BulkConverterCallback<Byte,Byte> GetByteConverter(Format srcFmt, Format dstFmt)
        {
            // direct converter
            if (srcFmt == dstFmt) return (a, b) => a.CopyTo(b);

            // common converters

            var converter = GetConverterToRGB(srcFmt, dstFmt);
            if (converter != null) return converter;

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

        public static BulkConverterCallback<TSrc, TDst> GetPixelConverter<TSrc, TDst>()
            where TSrc : unmanaged
            where TDst : unmanaged
        {
            var srcFmt = Format.TryIdentifyPixel<TSrc>();
            var dstFmt = Format.TryIdentifyPixel<TDst>();

            var byteConverter = GetByteConverter(srcFmt, dstFmt);

            return (ReadOnlySpan<TSrc> src, Span<TDst> dst) =>
            {
                var srcBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrc, byte>(src);
                var dstBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TDst, byte>(dst);
                byteConverter(srcBytes, dstBytes);
            };
        }

        public static BulkConverterCallback<Byte, Byte> GetByteConverter<TSrcPixel, TDstPixel>()
            where TSrcPixel : unmanaged, IPixelConvertible<RGBA128F>
            where TDstPixel : unmanaged, IPixelFactory<RGBA128F, TDstPixel>
        {
            if (typeof(TSrcPixel) == typeof(TDstPixel)) { return (a, b) => a.CopyTo(b); }

            void _converter(ReadOnlySpan<Byte> srcBytes, Span<Byte> dstBytes)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TSrcPixel>(srcBytes);
                var dst = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dstBytes);

                int blockSize = Math.Min(src.Length, 1024);

                Span<RGBA128F> block = stackalloc RGBA128F[blockSize];

                var factory = default(TDstPixel);

                while (dst.Length > 0)
                {
                    for (int i = 0; i < blockSize; ++i) { block[i] = src[i].ToPixel(); }
                    for (int i = 0; i < blockSize; ++i) { dst[i] = factory.From(block[i]); }

                    src = src.Slice(blockSize);
                    dst = dst.Slice(blockSize);

                    blockSize = Math.Min(src.Length, 1024);
                }
            }

            return _converter;            
        }


        /// <summary>
        /// Converts pixels by using <see cref="RGBA128F"/> as an intermediate format.
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

            Span<RGBA128F> block = stackalloc RGBA128F[blockSize];

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

        private static void _ConvertPixels(Span<RGBA128F> dst, ReadOnlySpan<Byte> src, Format srcFmt)
        {
            switch (srcFmt.PackedFormat)
            {
                case Alpha8.Code: _ConvertPixels(dst, src.OfType<Alpha8>()); break;
                case Luminance8.Code: _ConvertPixels(dst, src.OfType<Luminance8>()); break;
                case Luminance16.Code: _ConvertPixels(dst, src.OfType<Luminance16>()); break;
                case Luminance32F.Code: _ConvertPixels(dst, src.OfType<Luminance32F>()); break;
                case RGB24.Code: _ConvertPixels(dst, src.OfType<RGB24>()); break;
                case BGR24.Code: _ConvertPixels(dst, src.OfType<BGR24>()); break;
                case BGR565.Code: _ConvertPixels(dst, src.OfType<BGR565>()); break;
                case BGRA5551.Code: _ConvertPixels(dst, src.OfType<BGRA5551>()); break;
                case BGRA4444.Code: _ConvertPixels(dst, src.OfType<BGRA4444>()); break;
                case BGRA32.Code: _ConvertPixels(dst, src.OfType<BGRA32>()); break;
                case BGRP32.Code: _ConvertPixels(dst, src.OfType<BGRP32>()); break;
                case RGBA32.Code: _ConvertPixels(dst, src.OfType<RGBA32>()); break;
                case RGBP32.Code: _ConvertPixels(dst, src.OfType<RGBP32>()); break;
                case ARGB32.Code: _ConvertPixels(dst, src.OfType<ARGB32>()); break;
                case RGB96F.Code: _ConvertPixels(dst, src.OfType<RGB96F>()); break;
                case BGR96F.Code: _ConvertPixels(dst, src.OfType<BGR96F>()); break;
                case BGRA128F.Code: _ConvertPixels(dst, src.OfType<BGRA128F>()); break;
                case RGBA128F.Code: _ConvertPixels(dst, src.OfType<RGBA128F>()); break;
                default: throw new NotImplementedException(nameof(srcFmt));
            }
        }

        private static void _ConvertPixels(Span<Byte> dst, Format dstFmt, ReadOnlySpan<RGBA128F> src)
        {
            switch (dstFmt.PackedFormat)
            {
                case Alpha8.Code: _ConvertPixels(dst.OfType<Alpha8>(), src); break;
                case Luminance8.Code: _ConvertPixels(dst.OfType<Luminance8>(), src); break;
                case Luminance16.Code: _ConvertPixels(dst.OfType<Luminance16>(), src); break;
                case Luminance32F.Code: _ConvertPixels(dst.OfType<Luminance32F>(), src); break;
                case RGB24.Code: _ConvertPixels(dst.OfType<RGB24>(), src); break;
                case BGR24.Code: _ConvertPixels(dst.OfType<BGR24>(), src); break;
                case BGR565.Code: _ConvertPixels(dst.OfType<BGR565>(), src); break;
                case BGRA5551.Code: _ConvertPixels(dst.OfType<BGRA5551>(), src); break;
                case BGRA4444.Code: _ConvertPixels(dst.OfType<BGRA4444>(), src); break;
                case BGRA32.Code: _ConvertPixels(dst.OfType<BGRA32>(), src); break;
                case BGRP32.Code: _ConvertPixels(dst.OfType<BGRP32>(), src); break;
                case RGBA32.Code: _ConvertPixels(dst.OfType<RGBA32>(), src); break;
                case RGBP32.Code: _ConvertPixels(dst.OfType<RGBP32>(), src); break;
                case ARGB32.Code: _ConvertPixels(dst.OfType<ARGB32>(), src); break;
                case RGB96F.Code: _ConvertPixels(dst.OfType<RGB96F>(), src); break;
                case BGR96F.Code: _ConvertPixels(dst.OfType<BGR96F>(), src); break;
                case BGRA128F.Code: _ConvertPixels(dst.OfType<BGRA128F>(), src); break;
                case RGBA128F.Code: _ConvertPixels(dst.OfType<RGBA128F>(), src); break;
                
                default: throw new NotImplementedException(nameof(dstFmt));
            }
        }

        private static void _ConvertPixels(Span<RGBA128F> dst, ReadOnlySpan<RGBA128F> src)
        {
            src.CopyTo(dst);
        }

        private static void _ConvertPixels<TSrcPixel>(Span<RGBA128F> dst, ReadOnlySpan<TSrcPixel> src)
            where TSrcPixel : unmanaged, IPixelConvertible<RGBA128F>
        {
            if (typeof(TSrcPixel) == typeof(RGBA128F))
            {
                var srcBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(src);
                var dstBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<RGBA128F, Byte>(dst);
                srcBytes.CopyTo(dstBytes);
                return;
            }

            for (int i = 0; i < dst.Length; ++i) { dst[i] = src[i].ToPixel(); }
        }

        private static void _ConvertPixels<TDstPixel>(Span<TDstPixel> dst, ReadOnlySpan<RGBA128F> src)
            where TDstPixel : unmanaged, IPixelFactory<RGBA128F, TDstPixel>
        {
            if (typeof(RGBA128F) == typeof(TDstPixel))
            {
                var srcBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<RGBA128F, Byte>(src);
                var dstBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, Byte>(dst);
                srcBytes.CopyTo(dstBytes);
                return;
            }

            var factory = default(TDstPixel);

            for (int i = 0; i < dst.Length; ++i) { dst[i] = factory.From(src[i]); }
        }

        public static void LerpArray<TSrcPixel, TDstPixel>(ReadOnlySpan<TSrcPixel> left, ReadOnlySpan<TSrcPixel> right, float amount, Span<TDstPixel> dst)
            where TSrcPixel : unmanaged, IPixelConvertible<RGBA128F>
            where TDstPixel : unmanaged, IPixelFactory<RGBA128F, TDstPixel>
        {
            for(int i=0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector4.Lerp(left[i].ToPixel().RGBA, right[i].ToPixel().RGBA, amount);
                dst[i] = default(TDstPixel).From( new RGBA128F(v));
            }
        }

        public static void LerpArray<TDstPixel>(ReadOnlySpan<System.Numerics.Vector3> left, ReadOnlySpan<System.Numerics.Vector3> right, float amount, Span<TDstPixel> dst)            
            where TDstPixel : unmanaged, IPixelFactory<RGBA128F, TDstPixel>
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector3.Lerp(left[i], right[i], amount);
                dst[i] = default(TDstPixel).From(new RGBA128F(v));
            }
        }

    }
}
