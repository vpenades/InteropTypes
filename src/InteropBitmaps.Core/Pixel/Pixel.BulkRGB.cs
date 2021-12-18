using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PEF = Pixel.Format.ElementID;

    partial class Pixel
    {
        private static BulkConverterCallback<Byte, Byte> GetConverterToRGB(Format srcFmt, Format dstFmt)
        {
            switch(dstFmt.PackedFormat)
            {
                case BGR565.Code: return GetConverterToRGB<BGR565>(srcFmt);
                case BGR24.Code: return GetConverterToRGB<BGR24>(srcFmt);
                case RGB24.Code: return GetConverterToRGB<RGB24>(srcFmt);
                case RGB96F.Code: return GetConverterToRGB<RGB96F>(srcFmt);
                case BGR96F.Code: return GetConverterToRGB<BGR96F>(srcFmt);
            }

            return null;
        }

        private static BulkConverterCallback<Byte, Byte> GetConverterToRGB<TDstPixel>(Format srcFmt)
            where TDstPixel : unmanaged, _IPixelBulkRGB<TDstPixel>
        {
            if (srcFmt.HasPremul) return null;

            if (srcFmt.All(PEF.Red8, PEF.Green8, PEF.Blue8))
            {
                int srcStep = srcFmt.ByteCount;
                int srcIdxR = srcFmt.GetByteOffset(PEF.Red8);
                int srcIdxG = srcFmt.GetByteOffset(PEF.Green8);
                int srcIdxB = srcFmt.GetByteOffset(PEF.Blue8);

                return (src, dst) =>
                {
                    var srcR = src.Slice(srcIdxR);
                    var srcG = src.Slice(srcIdxG);
                    var srcB = src.Slice(srcIdxB);

                    var dstRGB = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dst);
                    default(TDstPixel).Fill(dstRGB, srcStep, srcR, srcG, srcB);
                };
            }

            if (srcFmt.All(PEF.Red32F, PEF.Green32F, PEF.Blue32F))
            {
                int srcStep = srcFmt.ByteCount / 4;
                int srcIdxR = srcFmt.GetByteOffset(PEF.Red32F);
                int srcIdxG = srcFmt.GetByteOffset(PEF.Green32F);
                int srcIdxB = srcFmt.GetByteOffset(PEF.Blue32F);

                return (src, dst) =>
                {
                    var srcR = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, float>(src.Slice(srcIdxR));
                    var srcG = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, float>(src.Slice(srcIdxG));
                    var srcB = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, float>(src.Slice(srcIdxB));

                    var dstRGB = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, TDstPixel>(dst);
                    default(TDstPixel).Fill(dstRGB, srcStep, srcR, srcG, srcB);
                };
            }

            return null;
        }

        interface _IPixelBulkRGB<TPixel>
        {
            void Fill(Span<TPixel> dst, int srcStep, ReadOnlySpan<Byte> red, ReadOnlySpan<Byte> green, ReadOnlySpan<Byte> blue);
            void Fill(Span<TPixel> dst, int srcStep, ReadOnlySpan<Single> red, ReadOnlySpan<Single> green, ReadOnlySpan<Single> blue);
        }

        partial struct BGR565 : _IPixelBulkRGB<BGR565>
        {
            void _IPixelBulkRGB<BGR565>.Fill(Span<BGR565> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelBulkRGB<BGR565>.Fill(Span<BGR565> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var rgb = new System.Numerics.Vector3(red[ii], green[ii], blue[ii]) * 255;
                    dst[i] = new BGR565((Byte)rgb.X, (Byte)rgb.Y, (Byte)rgb.Z);
                }
            }
        }

        partial struct BGR24 : _IPixelBulkRGB<BGR24>
        {
            void _IPixelBulkRGB<BGR24>.Fill(Span<BGR24> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelBulkRGB<BGR24>.Fill(Span<BGR24> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var rgb = new System.Numerics.Vector3(red[ii], green[ii], blue[ii]) * 255;
                    dst[i] = new BGR24((Byte)rgb.X, (Byte)rgb.Y, (Byte)rgb.Z);
                }
            }
        }

        partial struct RGB24 : _IPixelBulkRGB<RGB24>
        {
            void _IPixelBulkRGB<RGB24>.Fill(Span<RGB24> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelBulkRGB<RGB24>.Fill(Span<RGB24> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    var rgb = new System.Numerics.Vector3(red[ii], green[ii], blue[ii]) * 255;
                    dst[i] = new RGB24((Byte)rgb.X, (Byte)rgb.Y, (Byte)rgb.Z);
                }
            }
        }        

        partial struct RGB96F : _IPixelBulkRGB<RGB96F>
        {
            void _IPixelBulkRGB<RGB96F>.Fill(Span<RGB96F> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelBulkRGB<RGB96F>.Fill(Span<RGB96F> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    dst[i] = new RGB96F(red[ii], green[ii], blue[ii]);
                }
            }
        }

        partial struct BGR96F : _IPixelBulkRGB<BGR96F>
        {
            void _IPixelBulkRGB<BGR96F>.Fill(Span<BGR96F> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelBulkRGB<BGR96F>.Fill(Span<BGR96F> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
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
