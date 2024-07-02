using System;
using System.Collections.Generic;
using System.Text;

using MMARSHAL = System.Runtime.InteropServices.MemoryMarshal;

namespace InteropTypes.Tensors.Imaging
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    readonly struct _PixelRGB<TElement>
        where TElement : unmanaged
    {
        public _PixelRGB(TElement r, TElement g, TElement b)
        {
            R = r;
            G = g;
            B = b;
        }

        public readonly TElement R;
        public readonly TElement G;
        public readonly TElement B;

        public static void CopyTo<TOtherPixel>(ReadOnlySpan<_PixelRGB<TElement>> srcRGB, Span<TOtherPixel> dst)
            where TOtherPixel : unmanaged
        {
            if (typeof(TOtherPixel) == typeof(_PixelRGB<TElement>))
            {
                var dstRGB = MMARSHAL.Cast<TOtherPixel, _PixelRGB<TElement>>(dst);
                srcRGB.CopyTo(dstRGB);
                return;
            }

            if (typeof(TOtherPixel) == typeof(_PixelBGR<TElement>))
            {
                var dstBGR = MMARSHAL.Cast<TOtherPixel, _PixelBGR<TElement>>(dst);
                for (int i = 0; i < dst.Length; ++i) { dstBGR[i] = new _PixelBGR<TElement>(srcRGB[i]); }
                return;
            }

            if (typeof(TOtherPixel) == typeof(_PixelRGBA<TElement>))
            {
                var dstBGR = MMARSHAL.Cast<TOtherPixel, _PixelRGBA<TElement>>(dst);
                for (int i = 0; i < dst.Length; ++i) { dstBGR[i] = new _PixelRGBA<TElement>(srcRGB[i]); }
                return;
            }

            if (typeof(TOtherPixel) == typeof(_PixelBGRA<TElement>))
            {
                var dstBGR = MMARSHAL.Cast<TOtherPixel, _PixelBGRA<TElement>>(dst);
                for (int i = 0; i < dst.Length; ++i) { dstBGR[i] = new _PixelBGRA<TElement>(srcRGB[i]); }
                return;
            }

            if (typeof(TOtherPixel) == typeof(_PixelARGB<TElement>))
            {
                var dstBGR = MMARSHAL.Cast<TOtherPixel, _PixelARGB<TElement>>(dst);
                for (int i = 0; i < dst.Length; ++i) { dstBGR[i] = new _PixelARGB<TElement>(srcRGB[i]); }
                return;
            }            
        }        
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    readonly struct _PixelBGR<TElement>
        where TElement : unmanaged
    {
        public _PixelBGR(in _PixelRGB<TElement> src)
        {
            R = src.R;
            G = src.G;
            B = src.B;
        }

        public _PixelBGR(TElement r, TElement g, TElement b)
        {
            R = r;
            G = g;
            B = b;
        }

        public readonly TElement B;
        public readonly TElement G;
        public readonly TElement R;

        public static void CopyTo<TOther>(ReadOnlySpan<_PixelBGR<TElement>> src, Span<_PixelRGB<TElement>> dst)
        {
            for (int i = 0; i < dst.Length; ++i) { dst[i] = src[i].ToRGB(); }
        }

        public _PixelRGB<TElement> ToRGB() { return new _PixelRGB<TElement>(R, G, B); }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    readonly struct _PixelRGBA<TElement>
        where TElement : unmanaged
    {
        public _PixelRGBA(in _PixelRGB<TElement> src)
        {
            R = src.R;
            G = src.G;
            B = src.B;
            A = default;
        }

        public readonly TElement R;
        public readonly TElement G;
        public readonly TElement B;
        public readonly TElement A;

        public static void CopyTo(ReadOnlySpan<_PixelRGBA<TElement>> src, Span<_PixelBGRA<TElement>> dst)
        {
            var srcXYZW = System.Runtime.InteropServices.MemoryMarshal.Cast<_PixelRGBA<TElement>, TElement>(src);
            var dstXYZW = System.Runtime.InteropServices.MemoryMarshal.Cast<_PixelBGRA<TElement>, TElement>(dst);
            srcXYZW.ShuffleTo(dstXYZW, 2, 1, 0, 3);
        }

        public static void CopyTo(ReadOnlySpan<_PixelRGBA<TElement>> src, Span<_PixelARGB<TElement>> dst)
        {
            var srcXYZW = System.Runtime.InteropServices.MemoryMarshal.Cast<_PixelRGBA<TElement>, TElement>(src);
            var dstXYZW = System.Runtime.InteropServices.MemoryMarshal.Cast<_PixelARGB<TElement>, TElement>(dst);
            srcXYZW.ShuffleTo(dstXYZW, 3, 0, 1, 2);
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    readonly struct _PixelBGRA<TElement>
        where TElement : unmanaged
    {
        public _PixelBGRA(in _PixelRGB<TElement> src)
        {
            R = src.R;
            G = src.G;
            B = src.B;
            A = default;
        }

        public readonly TElement B;
        public readonly TElement G;
        public readonly TElement R;
        public readonly TElement A;
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    readonly struct _PixelARGB<TElement>
        where TElement : unmanaged
    {
        public _PixelARGB(in _PixelRGB<TElement> src)
        {
            R = src.R;
            G = src.G;
            B = src.B;
            A = default;
        }

        public readonly TElement A;
        public readonly TElement R;
        public readonly TElement G;
        public readonly TElement B;
    }
}
