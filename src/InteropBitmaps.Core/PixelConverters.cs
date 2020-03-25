using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    
    readonly struct _PixelBGRA32
    {
        #region constructors

        public _PixelBGRA32(Byte blue, Byte green, Byte red, Byte alpha)
        {
            B = blue;
            G = green;
            R = red;
            A = alpha;
        }

        public _PixelBGRA32(Byte blue, Byte green, Byte red)
        {
            B = blue;
            G = green;
            R = red;
            A = 255;
        }

        #endregion

        #region DATA

        public readonly Byte B;
        public readonly Byte G;
        public readonly Byte R;
        public readonly Byte A;

        const int RLuminanceWeight = 19562;
        const int GLuminanceWeight = 38550;
        const int BLuminanceWeight = 7424;

        #endregion

        #region Gray8

        public static _PixelBGRA32 FromGray8(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[0], src[0], src[0]);
        }

        public void ToGray8(Span<Byte> dst)
        {
            int gray = 0;

            gray += RLuminanceWeight*(int)R;
            gray += GLuminanceWeight*(int)G;
            gray += BLuminanceWeight*(int)B;

            gray >>= 16;

            dst[0] = (Byte)gray;
        }

        #endregion

        #region Gray16

        public static _PixelBGRA32 FromGray16(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[1], src[1], src[1]);
        }

        public void ToGray16(Span<Byte> dst)
        {
            int gray = 0;

            gray += RLuminanceWeight * (int)R;
            gray += GLuminanceWeight * (int)G;
            gray += BLuminanceWeight * (int)B;

            gray >>= 8;

            dst[0] = (Byte)(gray & 255);
            dst[1] = (Byte)(gray >> 8);
        }

        #endregion

        #region RGB24

        public static _PixelBGRA32 FromRgb24(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[2], src[1], src[0]);
        }

        public void ToRgb24(Span<Byte> dst)
        {
            dst[0] = R;
            dst[1] = G;
            dst[2] = B;
        }

        #endregion

        #region BGR24

        public static _PixelBGRA32 FromBgr24(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[0], src[1], src[2]);
        }

        public void ToBgr24(Span<Byte> dst)
        {
            dst[0] = B;
            dst[1] = G;
            dst[2] = R;
        }

        #endregion

        #region RGBA32

        public static _PixelBGRA32 FromRgba32(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[2], src[1], src[0], src[3]);
        }

        public void ToRgba32(Span<Byte> dst)
        {
            dst[0] = R;
            dst[1] = G;
            dst[2] = B;
            dst[3] = A;
        }

        #endregion

        #region BGRA32

        public static _PixelBGRA32 FromBgra32(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[0], src[1], src[2], src[3]);
        }

        public void ToBgra32(Span<Byte> dst)
        {
            dst[0] = B;
            dst[1] = G;
            dst[2] = R;
            dst[3] = A;
        }

        #endregion

        #region ARGB32

        public static _PixelBGRA32 FromArgb32(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[3], src[2], src[1], src[0]);
        }

        public void ToArgb32(Span<Byte> dst)
        {
            dst[0] = A;
            dst[1] = R;
            dst[2] = G;
            dst[3] = B;
        }

        #endregion
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct _PixelBGRA64
    {
        [System.Runtime.InteropServices.FieldOffset(0)]
        public Byte BL;
        [System.Runtime.InteropServices.FieldOffset(1)]
        public Byte BH;

        [System.Runtime.InteropServices.FieldOffset(2)]
        public Byte GL;
        [System.Runtime.InteropServices.FieldOffset(3)]
        public Byte GH;

        [System.Runtime.InteropServices.FieldOffset(4)]
        public Byte RL;
        [System.Runtime.InteropServices.FieldOffset(5)]
        public Byte RH;

        [System.Runtime.InteropServices.FieldOffset(6)]
        public Byte AL;
        [System.Runtime.InteropServices.FieldOffset(7)]
        public Byte AH;
    }

    static partial class _PixelConverters
    {
        #region API

        /*
        public ICopyConverter CreateConverter(PixelFormat dst, PixelFormat src)
        {

        }*/

        public interface ICopyConverter
        {
            void Convert(Span<Byte> dst, Span<Byte> tmp, ReadOnlySpan<Byte> src, int y);
        }

        readonly struct _RGBConverter : ICopyConverter
        {
            private readonly IRGBConverter _SrcConverter;
            private readonly IRGBConverter _DstConverter;

            public void Convert(Span<byte> dst, Span<byte> tmp, ReadOnlySpan<byte> src, int y)
            {
                var xtmp = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PixelBGRA32>(tmp);
                _SrcConverter.ConvertFrom(xtmp, src, y);
                _SrcConverter.ConvertTo(dst, y, xtmp);
            }
        }

        #endregion

        #region static API

        public interface IRGBConverter
        {
            void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<Byte> src, int srcY);
            void ConvertTo(Span<Byte> dst,int dstY, ReadOnlySpan<_PixelBGRA32> src);
        }

        public static IRGBConverter GetConverter(PixelFormat fmt)
        {
            if (fmt == PixelFormat.Standard.Gray8) return new _CvtGray8();

            if (fmt == PixelFormat.Standard.Gray16) return new _CvtGray16();
            if (fmt == PixelFormat.Standard.DepthMM16) return new _CvtGray16();

            if (fmt == PixelFormat.Standard.RGB24) return new _CvtRgb24();
            if (fmt == PixelFormat.Standard.BGR24) return new _CvtBgr24();

            if (fmt == PixelFormat.Standard.BGRA32) return new _CvtBgra32();
            if (fmt == PixelFormat.Standard.ARGB32) return new _CvtArgb32();
            if (fmt == PixelFormat.Standard.RGBA32) return new _CvtRgba32();

            throw new NotImplementedException();
        }

        #endregion       
    }
}
