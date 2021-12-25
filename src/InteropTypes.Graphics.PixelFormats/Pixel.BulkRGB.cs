using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PEF = PixelFormat.ElementID;

    partial class Pixel
    {
        private static CopyConverterCallback<Byte, Byte> GetConverterToRGB(PixelFormat srcFmt, PixelFormat dstFmt)
        {
            if (srcFmt.All(PEF.Red8, PEF.Green8, PEF.Blue8))
            {
                switch (dstFmt.PackedFormat)
                {
                    case BGR565.Code: return GetConverterByteToRGB<BGR565>(srcFmt);
                    case BGR24.Code: return GetConverterByteToRGB<BGR24>(srcFmt);
                    case RGB24.Code: return GetConverterByteToRGB<RGB24>(srcFmt);
                    case RGB96F.Code: return GetConverterByteToRGB<RGB96F>(srcFmt);
                    case BGR96F.Code: return GetConverterByteToRGB<BGR96F>(srcFmt);
                }
            }

            if (srcFmt.All(PEF.Red32F, PEF.Green32F, PEF.Blue32F))
            {
                switch (dstFmt.PackedFormat)
                {
                    case BGR565.Code: return GetConverterFloatToRGB<BGR565>(srcFmt);
                    case BGR24.Code: return GetConverterFloatToRGB<BGR24>(srcFmt);
                    case RGB24.Code: return GetConverterFloatToRGB<RGB24>(srcFmt);
                    case RGB96F.Code: return GetConverterFloatToRGB<RGB96F>(srcFmt);
                    case BGR96F.Code: return GetConverterFloatToRGB<BGR96F>(srcFmt);
                }
            }

            return null;
        }

        private static CopyConverterCallback<Byte, Byte> GetConverterByteToRGB<TDstPixel>(PixelFormat srcFmt)
            where TDstPixel : unmanaged, _IPixelBulkRGB<TDstPixel, Byte>
        {
            if (srcFmt.HasPremul) return null;
            
            int srcStep = srcFmt.ByteCount;
            int srcIdxR = srcFmt.GetByteOffset(PEF.Red8);
            int srcIdxG = srcFmt.GetByteOffset(PEF.Green8);
            int srcIdxB = srcFmt.GetByteOffset(PEF.Blue8);

            return (src, dst) =>
            {
                src.AssertNoOverlapWith(dst);

                var srcR = src.Slice(srcIdxR);
                var srcG = src.Slice(srcIdxG);
                var srcB = src.Slice(srcIdxB);

                var dstRGB = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dst);
                default(TDstPixel).Fill(dstRGB, srcStep, srcR, srcG, srcB);
            };
        }

        private static CopyConverterCallback<Byte, Byte> GetConverterFloatToRGB<TDstPixel>(PixelFormat srcFmt)
            where TDstPixel : unmanaged, _IPixelBulkRGB<TDstPixel,float>
        {
            if (srcFmt.HasPremul) return null;
            
            int srcStep = srcFmt.ByteCount / 4;
            int srcIdxR = srcFmt.GetByteOffset(PEF.Red32F);
            int srcIdxG = srcFmt.GetByteOffset(PEF.Green32F);
            int srcIdxB = srcFmt.GetByteOffset(PEF.Blue32F);

            return (src, dst) =>
            {
                src.AssertNoOverlapWith(dst);

                var srcR = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, float>(src.Slice(srcIdxR));
                var srcG = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, float>(src.Slice(srcIdxG));
                var srcB = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, float>(src.Slice(srcIdxB));

                var dstRGB = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dst);
                default(TDstPixel).Fill(dstRGB, srcStep, srcR, srcG, srcB);
            };
        }

        interface _IPixelBulkRGB<TPixel, TChannel>
        {
            void Fill(Span<TPixel> dst, int srcStep, ReadOnlySpan<TChannel> red, ReadOnlySpan<TChannel> green, ReadOnlySpan<TChannel> blue);
        }

        partial struct BGR565
            : _IPixelBulkRGB<BGR565, byte>
            , _IPixelBulkRGB<BGR565, float>
        {
            void _IPixelBulkRGB<BGR565, byte>.Fill(Span<BGR565> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var r = red[ii];
                    var g = green[ii];
                    var b = blue[ii];
                    dst[i] = new BGR565(r, g, b);
                }
            }

            void _IPixelBulkRGB<BGR565, float>.Fill(Span<BGR565> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var rgb = new System.Numerics.Vector3(red[ii], green[ii], blue[ii]) * 255;
                    dst[i] = new BGR565((Byte)rgb.X, (Byte)rgb.Y, (Byte)rgb.Z);
                }
            }
        }

        partial struct BGR24
            : _IPixelBulkRGB<BGR24, byte>
            , _IPixelBulkRGB<BGR24, float>
        {
            void _IPixelBulkRGB<BGR24, byte>.Fill(Span<BGR24> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var r = red[ii];
                    var g = green[ii];
                    var b = blue[ii];
                    dst[i] = new BGR24(r, g, b);
                }
            }

            void _IPixelBulkRGB<BGR24, float>.Fill(Span<BGR24> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var rgb = new System.Numerics.Vector3(red[ii], green[ii], blue[ii]) * 255;
                    dst[i] = new BGR24((Byte)rgb.X, (Byte)rgb.Y, (Byte)rgb.Z);
                }
            }
        }

        partial struct RGB24
            : _IPixelBulkRGB<RGB24, byte>
            , _IPixelBulkRGB<RGB24, float>
        {
            void _IPixelBulkRGB<RGB24, byte>.Fill(Span<RGB24> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var r = red[ii];
                    var g = green[ii];
                    var b = blue[ii];
                    dst[i] = new RGB24(r, g, b);
                }
            }

            void _IPixelBulkRGB<RGB24, float>.Fill(Span<RGB24> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var rgb = new System.Numerics.Vector3(red[ii], green[ii], blue[ii]) * 255;
                    dst[i] = new RGB24((Byte)rgb.X, (Byte)rgb.Y, (Byte)rgb.Z);
                }
            }
        }

        partial struct RGB96F
            : _IPixelBulkRGB<RGB96F, byte>
            , _IPixelBulkRGB<RGB96F, float>
        {
            void _IPixelBulkRGB<RGB96F, byte>.Fill(Span<RGB96F> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var r = red[ii];
                    var g = green[ii];
                    var b = blue[ii];
                    dst[i] = new RGB96F(r, g, b);
                }
            }

            void _IPixelBulkRGB<RGB96F, float>.Fill(Span<RGB96F> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    dst[i] = new RGB96F(red[ii], green[ii], blue[ii]);
                }
            }
        }

        partial struct BGR96F
            : _IPixelBulkRGB<BGR96F, byte>
            , _IPixelBulkRGB<BGR96F, float>
        {
            void _IPixelBulkRGB<BGR96F, byte>.Fill(Span<BGR96F> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var r = red[ii];
                    var g = green[ii];
                    var b = blue[ii];
                    dst[i] = new BGR96F(r, g, b);
                }
            }

            void _IPixelBulkRGB<BGR96F, float>.Fill(Span<BGR96F> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    dst[i] = new BGR96F(red[ii], green[ii], blue[ii]);
                }
            }
        }
    }
}
