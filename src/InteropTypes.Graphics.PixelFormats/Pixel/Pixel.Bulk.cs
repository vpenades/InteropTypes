using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics
{
    partial class Pixel
    {
        /// <summary>
        /// Callback that copies the pixels from src to dst,<br/>
        /// while aplying the appropiate conversion.
        /// </summary>
        /// <remarks>
        /// src and dst memory blocks should not overlap.
        /// </remarks>
        /// <param name="src">The source pixels.</param>
        /// <param name="dst">The target pixels.</param>
        public delegate void CopyConverterCallback<TSrc, TDst>(ReadOnlySpan<TSrc> src, Span<TDst> dst);

        [System.Diagnostics.DebuggerStepThrough]
        public static CopyConverterCallback<Byte, Byte> GetByteCopyConverter(PixelFormat srcFmt, PixelFormat dstFmt)
        {
            // direct converter
            if (srcFmt == dstFmt) return (a, b) => { a.AssertNoOverlapWith(b); a.CopyTo(b); };

            switch (srcFmt.Code)
            {
                case Luminance8.Code: return _GetByteCopyConverter<Luminance8>(dstFmt);
                case Luminance16.Code: return _GetByteCopyConverter<Luminance16>(dstFmt);
                case Luminance32F.Code: return _GetByteCopyConverter<Luminance32F>(dstFmt);

                case Alpha8.Code: return _GetByteCopyConverter<Alpha8>(dstFmt);

                case BGR565.Code: return _GetByteCopyConverter<BGR565>(dstFmt);
                case BGRA5551.Code: return _GetByteCopyConverter<BGRA5551>(dstFmt);
                case BGRA4444.Code: return _GetByteCopyConverter<BGRA4444>(dstFmt);

                case BGR24.Code: return _GetByteCopyConverter<BGR24>(dstFmt);
                case RGB24.Code: return _GetByteCopyConverter<RGB24>(dstFmt);

                case BGRA32.Code: return _GetByteCopyConverter<BGRA32>(dstFmt);
                case RGBA32.Code: return _GetByteCopyConverter<RGBA32>(dstFmt);
                case ARGB32.Code: return _GetByteCopyConverter<ARGB32>(dstFmt);

                case BGRP32.Code: return _GetByteCopyConverter<BGRP32>(dstFmt);
                case RGBP32.Code: return _GetByteCopyConverter<RGBP32>(dstFmt);
                case PRGB32.Code: return _GetByteCopyConverter<PRGB32>(dstFmt);

                case BGR96F.Code: return _GetByteCopyConverter<BGR96F>(dstFmt);
                case RGB96F.Code: return _GetByteCopyConverter<RGB96F>(dstFmt);

                case BGRA128F.Code: return _GetByteCopyConverter<BGRA128F>(dstFmt);
                case RGBA128F.Code: return _GetByteCopyConverter<RGBA128F>(dstFmt);

                case RGBP128F.Code: return _GetByteCopyConverter<RGBP128F>(dstFmt);
                case BGRP128F.Code: return _GetByteCopyConverter<BGRP128F>(dstFmt);

                default: throw new NotImplementedException();
            }            
        }

        [System.Diagnostics.DebuggerStepThrough]
        private static CopyConverterCallback<Byte, Byte> _GetByteCopyConverter<TSrc>(PixelFormat dstFmt)
            where TSrc : unmanaged
        {
            switch (dstFmt.Code)
            {
                case Luminance8.Code: return GetByteCopyConverter<TSrc, Luminance8>();
                case Luminance16.Code: return GetByteCopyConverter<TSrc, Luminance16>();
                case Luminance32F.Code: return GetByteCopyConverter<TSrc, Luminance32F>();

                case Alpha8.Code: return GetByteCopyConverter<TSrc, Alpha8>();

                case BGR565.Code: return GetByteCopyConverter<TSrc, BGR565>();
                case BGRA5551.Code: return GetByteCopyConverter<TSrc, BGRA5551>();
                case BGRA4444.Code: return GetByteCopyConverter<TSrc, BGRA4444>();

                case BGR24.Code: return GetByteCopyConverter<TSrc, BGR24>();
                case RGB24.Code: return GetByteCopyConverter<TSrc, RGB24>();

                case BGRA32.Code: return GetByteCopyConverter<TSrc, BGRA32>();
                case RGBA32.Code: return GetByteCopyConverter<TSrc, RGBA32>();
                case ARGB32.Code: return GetByteCopyConverter<TSrc, ARGB32>();

                case BGRP32.Code: return GetByteCopyConverter<TSrc, BGRP32>();
                case RGBP32.Code: return GetByteCopyConverter<TSrc, RGBP32>();
                case PRGB32.Code: return GetByteCopyConverter<TSrc, PRGB32>();

                case BGR96F.Code: return GetByteCopyConverter<TSrc, BGR96F>();
                case RGB96F.Code: return GetByteCopyConverter<TSrc, RGB96F>();

                case BGRA128F.Code: return GetByteCopyConverter<TSrc, BGRA128F>();
                case RGBA128F.Code: return GetByteCopyConverter<TSrc, RGBA128F>();

                case RGBP128F.Code: return GetByteCopyConverter<TSrc, RGBP128F>();
                case BGRP128F.Code: return GetByteCopyConverter<TSrc, BGRP128F>();

                default: throw new NotImplementedException();
            }            
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static CopyConverterCallback<Byte, Byte> GetByteCopyConverter<TSrc, TDst>()
            where TSrc : unmanaged
            where TDst : unmanaged
        {
            var pixelConverter = GetPixelCopyConverter<TSrc, TDst>();

            return (s, d) => pixelConverter(s.OfType<TSrc>(), d.OfType<TDst>());
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static CopyConverterCallback<TSrc, TDst> GetPixelCopyConverter<TSrc, TDst>()
            where TSrc : unmanaged
            where TDst : unmanaged
        {
            // direct converter
            if (typeof(TSrc) == typeof(TDst)) return (src, dst) =>
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.AsBytes().TryCopyTo(dst.AsBytes());
                System.Diagnostics.Debug.Assert(r);
            };

            if (default(TSrc) is ICopyConverterDelegateProvider<TSrc, TDst> srcProvider) return srcProvider.GetCopyConverterDelegate();

            throw new NotImplementedException();
        }
    }
}
