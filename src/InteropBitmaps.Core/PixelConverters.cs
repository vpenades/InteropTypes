using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    
    readonly struct _PixelBGRA32
    {
        #region constructors

        public static implicit operator System.Numerics.Vector4(_PixelBGRA32 value)
        {
            return new System.Numerics.Vector4(value.R, value.G, value.B, value.A) / 255f;
        }
        
        public _PixelBGRA32(System.Numerics.Vector4 rgba)
        {
            rgba *= 255f;

            R = (Byte)(rgba.X);
            G = (Byte)(rgba.Y);
            B = (Byte)(rgba.Z);
            A = (Byte)(rgba.W);
        }

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

        const uint RLuminanceWeight = 19562;
        const uint GLuminanceWeight = 38550;
        const uint BLuminanceWeight = 7424;

        #endregion

        #region Gray8

        public static _PixelBGRA32 FromGray8(ReadOnlySpan<Byte> src)
        {
            return new _PixelBGRA32(src[0], src[0], src[0]);
        }

        public void ToGray8(Span<Byte> dst)
        {
            uint gray = 0;

            gray += RLuminanceWeight*(uint)R;
            gray += GLuminanceWeight*(uint)G;
            gray += BLuminanceWeight*(uint)B;

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
            uint gray = 0;

            gray += RLuminanceWeight * (uint)R;
            gray += GLuminanceWeight * (uint)G;
            gray += BLuminanceWeight * (uint)B;

            gray >>= 8;

            dst[0] = (Byte)(gray & 255);
            dst[1] = (Byte)(gray >> 8);
        }

        #endregion

        #region Gray32F

        public static _PixelBGRA32 FromGray32F(ReadOnlySpan<Byte> src)
        {
            var val = System.Runtime.InteropServices.MemoryMarshal.Read<float>(src);

            var r = (int)val * 255;
            if (r > 255) r = 255;
            if (r < 0) r = 0;

            return new _PixelBGRA32((Byte)r, (Byte)r, (Byte)r);
        }

        public void ToGray32F(Span<Byte> dst)
        {
            uint gray = 0;

            gray += RLuminanceWeight * (uint)R;
            gray += GLuminanceWeight * (uint)G;
            gray += BLuminanceWeight * (uint)B;

            gray >>= 8;

            var val = (float)gray / 65536.0f;

            System.Runtime.InteropServices.MemoryMarshal.Write<float>(dst, ref val);
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

        #region PRGB32

        public static _PixelBGRA32 FromPrgb32(ReadOnlySpan<Byte> src)
        {
            var A = (int)src[0];
            if (A == 0) return default;            

            int r = (int)src[1] * 255 / (int)A;
            int g = (int)src[2] * 255 / (int)A;
            int b = (int)src[3] * 255 / (int)A;            

            return new _PixelBGRA32((Byte)b, (Byte)g, (Byte)r, src[0]);
        }
        
        public void ToPrgb32(Span<Byte> dst)
        {            
            int a = A;
            int r = R;
            int g = G;
            int b = B;

            r = r * a / 255;
            g = g * a / 255;
            b = b * a / 255;

            dst[0] = A;
            dst[1] = (Byte)r;
            dst[2] = (Byte)g;
            dst[3] = (Byte)b;         
        }

        #endregion
    }

    
    struct _PixelBGRA64
    {
        #region constructors

        public _PixelBGRA64(UInt16 blue, UInt16 green, UInt16 red, UInt16 alpha)
        {
            B = blue;
            G = green;
            R = red;
            A = alpha;
        }

        public _PixelBGRA64(UInt16 blue, UInt16 green, UInt16 red)
        {
            B = blue;
            G = green;
            R = red;
            A = 65535;
        }

        #endregion

        #region data
        
        public UInt16 B;
        public UInt16 G;
        public UInt16 R;
        public UInt16 A;        

        const uint RLuminanceWeight = 19562;
        const uint GLuminanceWeight = 38550;
        const uint BLuminanceWeight = 7424;
        
        #endregion

        #region Gray8

        public static _PixelBGRA64 FromGray8(ReadOnlySpan<Byte> src)
        {
            uint v = src[0];
            var vv = (ushort)(v * 256 + v);
            return new _PixelBGRA64(vv, vv, vv);
        }

        public void ToGray8(Span<Byte> dst)
        {
            uint gray = 0;

            gray += RLuminanceWeight * (uint)R;
            gray += GLuminanceWeight * (uint)G;
            gray += BLuminanceWeight * (uint)B;

            gray >>= 24;

            dst[0] = (Byte)gray;
        }

        #endregion
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
            private readonly IBGRA32Converter _SrcConverter;
            private readonly IBGRA32Converter _DstConverter;

            public void Convert(Span<byte> dst, Span<byte> tmp, ReadOnlySpan<byte> src, int y)
            {
                var xtmp = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PixelBGRA32>(tmp);
                _SrcConverter.ConvertFrom(xtmp, src, y);
                _SrcConverter.ConvertTo(dst, y, xtmp);
            }
        }

        #endregion

        #region static API

        public interface IBGRA32Converter
        {
            void ConvertFrom(Span<_PixelBGRA32> dst, ReadOnlySpan<Byte> src, int srcY);
            void ConvertTo(Span<Byte> dst,int dstY, ReadOnlySpan<_PixelBGRA32> src);

            void ConvertFrom(Span<System.Numerics.Vector4> dst, ReadOnlySpan<Byte> src, int srcY);
            void ConvertTo(Span<Byte> dst, int dstY, ReadOnlySpan<System.Numerics.Vector4> src);
        }

        public static IBGRA32Converter GetConverter(Pixel.Format fmt)
        {
            if (fmt == Pixel.Standard.Gray8) return new _CvtGray8();

            if (fmt == Pixel.Standard.Gray16) return new _CvtGray16();
            if (fmt == Pixel.Standard.DepthMM16) return new _CvtGray16();

            if (fmt == Pixel.Standard.RGB24) return new _CvtRgb24();
            if (fmt == Pixel.Standard.BGR24) return new _CvtBgr24();

            if (fmt == Pixel.Standard.BGRA32) return new _CvtBgra32();
            if (fmt == Pixel.Standard.ARGB32) return new _CvtArgb32();
            if (fmt == Pixel.Standard.RGBA32) return new _CvtRgba32();

            throw new NotImplementedException();
        }

        #endregion       
    }
}
