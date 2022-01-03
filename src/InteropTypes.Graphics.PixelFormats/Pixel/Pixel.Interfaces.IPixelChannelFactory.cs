using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        interface _IPixelChannelFactory<TPixel, TChannel>
        {
            void Fill(Span<TPixel> dst, int srcStep, ReadOnlySpan<TChannel> red, ReadOnlySpan<TChannel> green, ReadOnlySpan<TChannel> blue);
        }

        partial struct BGR565
            : _IPixelChannelFactory<BGR565, byte>
            , _IPixelChannelFactory<BGR565, float>
        {
            void _IPixelChannelFactory<BGR565, byte>.Fill(Span<BGR565> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelChannelFactory<BGR565, float>.Fill(Span<BGR565> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
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
            : _IPixelChannelFactory<BGR24, byte>
            , _IPixelChannelFactory<BGR24, float>
        {
            void _IPixelChannelFactory<BGR24, byte>.Fill(Span<BGR24> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelChannelFactory<BGR24, float>.Fill(Span<BGR24> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
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
            : _IPixelChannelFactory<RGB24, byte>
            , _IPixelChannelFactory<RGB24, float>
        {
            void _IPixelChannelFactory<RGB24, byte>.Fill(Span<RGB24> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelChannelFactory<RGB24, float>.Fill(Span<RGB24> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
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
            : _IPixelChannelFactory<RGB96F, byte>
            , _IPixelChannelFactory<RGB96F, float>
        {
            void _IPixelChannelFactory<RGB96F, byte>.Fill(Span<RGB96F> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelChannelFactory<RGB96F, float>.Fill(Span<RGB96F> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
            {
                for (int i = 0; i < dst.Length; ++i)
                {
                    var ii = i * srcStep;
                    dst[i] = new RGB96F(red[ii], green[ii], blue[ii]);
                }
            }
        }

        partial struct BGR96F
            : _IPixelChannelFactory<BGR96F, byte>
            , _IPixelChannelFactory<BGR96F, float>
        {
            void _IPixelChannelFactory<BGR96F, byte>.Fill(Span<BGR96F> dst, int srcStep, ReadOnlySpan<byte> red, ReadOnlySpan<byte> green, ReadOnlySpan<byte> blue)
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

            void _IPixelChannelFactory<BGR96F, float>.Fill(Span<BGR96F> dst, int srcStep, ReadOnlySpan<float> red, ReadOnlySpan<float> green, ReadOnlySpan<float> blue)
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
