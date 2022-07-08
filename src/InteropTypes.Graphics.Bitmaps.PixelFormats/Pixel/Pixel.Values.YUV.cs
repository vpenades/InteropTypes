using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {

        partial struct YUV24
        {
            private void _FromRGB(int r, int g, int b, ref YUV24 yuv)
            {
                var y = ((66 * r + 129 * g + 25 * b + 128) >> 8) + 16;
                var u = ((-38 * r - 74 * g + 112 * b + 128) >> 8) + 128;
                var v = ((112 * r - 94 * g - 18 * b + 128) >> 8) + 128;

                yuv.Y = (Byte)y;
                yuv.U = (Byte)u;
                yuv.V = (Byte)v;
            }

            internal static BGR24 _ToBGR(Byte y, Byte u, Byte v)
            {
                int yy = y - 16;
                int uu = u - 128;
                int vv = v - 128;

                yy *= 298;

                var b = (yy + 516 * uu + 128) >> 8; // blue
                var g = (yy - 100 * uu - 208 * vv + 128) >> 8; // green
                var r = (yy + 409 * vv + 128) >> 8; // red

                #if NETSTANDARD2_0
                if (r < 0) r = 0; else if (r > 255) r = 255;
                if (g < 0) g = 0; else if (g > 255) g = 255;
                if (b < 0) b = 0; else if (b > 255) b = 255;
                #else
                r = Math.Clamp(r, 0, 255);
                g = Math.Clamp(g, 0, 255);
                b = Math.Clamp(b, 0, 255);
                #endif

                return new BGR24(r, g, b);
            }

            internal static BGRA32 _ToBGRA(Byte y, Byte u, Byte v)
            {
                int yy = y - 16;
                int uu = u - 128;
                int vv = v - 128;

                yy *= 298;

                var b = (yy + 516 * uu + 128) >> 8; // blue
                var g = (yy - 100 * uu - 208 * vv + 128) >> 8; // green
                var r = (yy + 409 * vv + 128) >> 8; // red

                #if NETSTANDARD2_0
                if (r < 0) r = 0; else if (r > 255) r = 255;
                if (g < 0) g = 0; else if (g > 255) g = 255;
                if (b < 0) b = 0; else if (b > 255) b = 255;
                #else
                r = Math.Clamp(r, 0, 255);
                g = Math.Clamp(g, 0, 255);
                b = Math.Clamp(b, 0, 255);
                #endif

                return new BGRA32(r, g, b, 255);
            }

            public struct KernelRGB
            {
                #region data

                private int _PartialR;
                private int _PartialG;
                private int _PartialB;

                private static readonly System.Numerics.Vector3 _Rcp255 = new System.Numerics.Vector3(1f / 255f);

                #endregion

                #region API

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public Byte GetRed(int y)
                {
                    y += _PartialR;
                    if (y < 0) y = 0;
                    else if (y > 255) y = 255;
                    return (Byte)y;
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public Byte GetGreen(int y)
                {
                    y += _PartialG;
                    if (y < 0) y = 0;
                    else if (y > 255) y = 255;
                    return (Byte)y;
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public Byte GetBlue(int y)
                {
                    y += _PartialB;
                    if (y < 0) y = 0;
                    else if (y > 255) y = 255;
                    return (Byte)y;
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public void SetUV(int u, int v)
                {
                    _PartialR = ((v * 1436) >> 10) - 179;
                    _PartialG = -((u * 46549) >> 17) + 44 - ((v * 93604) >> 17) + 91;
                    _PartialB = ((u * 1814) >> 10) - 227;
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public BGR24 ToBGR24(int y)
                {
                    #if NETSTANDARD2_0
                    return new BGR24(GetRed(y), GetGreen(y), GetBlue(y));
                    #else
                    return new BGR24
                        (
                        Math.Clamp(y + _PartialR, 0, 255),
                        Math.Clamp(y + _PartialG, 0, 255),
                        Math.Clamp(y + _PartialB, 0, 255)
                        );
                    #endif
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public readonly void CopyTo(ref BGR24 pixel, int y)
                {
                    #if NETSTANDARD2_0
                    pixel.R = GetRed(y);
                    pixel.G = GetGreen(y);
                    pixel.B = GetBlue(y);
                    #else
                    pixel.R = (Byte)Math.Clamp(y + _PartialR, 0, 255);
                    pixel.G = (Byte)Math.Clamp(y + _PartialG, 0, 255);
                    pixel.B = (Byte)Math.Clamp(y + _PartialB, 0, 255);
                    #endif
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public readonly void CopyTo(ref RGB24 pixel, int y)
                {
                    #if NETSTANDARD2_0
                    pixel.R = GetRed(y);
                    pixel.G = GetGreen(y);
                    pixel.B = GetBlue(y);
                    #else
                    pixel.R = (Byte)Math.Clamp(y + _PartialR, 0, 255);
                    pixel.G = (Byte)Math.Clamp(y + _PartialG, 0, 255);
                    pixel.B = (Byte)Math.Clamp(y + _PartialB, 0, 255);
                    #endif
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public readonly void CopyTo(ref RGBA32 pixel, int y)
                {
                    #if NETSTANDARD2_0
                    pixel.R = GetRed(y);
                    pixel.G = GetGreen(y);
                    pixel.B = GetBlue(y);
                    #else
                    pixel.R = (Byte)Math.Clamp(y + _PartialR, 0, 255);
                    pixel.G = (Byte)Math.Clamp(y + _PartialG, 0, 255);
                    pixel.B = (Byte)Math.Clamp(y + _PartialB, 0, 255);
                    #endif

                    pixel.A = 255;
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public readonly void CopyTo(ref BGRA32 pixel, int y)
                {
                    #if NETSTANDARD2_0
                    pixel.R = GetRed(y);
                    pixel.G = GetGreen(y);
                    pixel.B = GetBlue(y);
                    #else
                    pixel.R = (Byte)Math.Clamp(y + _PartialR, 0, 255);
                    pixel.G = (Byte)Math.Clamp(y + _PartialG, 0, 255);
                    pixel.B = (Byte)Math.Clamp(y + _PartialB, 0, 255);
                    #endif

                    pixel.A = 255;
                }

                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public readonly void CopyTo(ref BGR96F pixel, int y)
                {
                    pixel.R = _PartialR + y;
                    pixel.G = _PartialG + y;
                    pixel.B = _PartialB + y;

                    pixel.BGR *= _Rcp255;
                    pixel.BGR = System.Numerics.Vector3.Clamp(pixel.BGR, System.Numerics.Vector3.Zero, System.Numerics.Vector3.One);                    
                }


                public static void TransferYUV420<TDstPixel>(Span<TDstPixel> dst0, Span<TDstPixel> dst1, ReadOnlySpan<Byte> srcY0, ReadOnlySpan<Byte> srcY1, ReadOnlySpan<Byte> srcU, ReadOnlySpan<Byte> srcV)
                    where TDstPixel:unmanaged
                {
                    KernelRGB yuv = default;

                    if (typeof(TDstPixel) == typeof(BGR24))
                    {
                        var dst00 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, BGR24>(dst0);
                        var dst11 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, BGR24>(dst1);

                        for (int x = 0; x < dst0.Length - 1; x += 2)
                        {
                            yuv.SetUV(srcU[x / 2], srcV[x / 2]);

                            yuv.CopyTo(ref dst00[x + 0], srcY0[x + 0]);
                            yuv.CopyTo(ref dst00[x + 1], srcY0[x + 1]);
                            yuv.CopyTo(ref dst11[x + 0], srcY1[x + 0]);
                            yuv.CopyTo(ref dst11[x + 1], srcY1[x + 1]);
                        }

                        return;
                    }

                    if (typeof(TDstPixel) == typeof(RGB24))
                    {
                        var dst00 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, RGB24>(dst0);
                        var dst11 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, RGB24>(dst1);

                        for (int x = 0; x < dst0.Length - 1; x += 2)
                        {
                            yuv.SetUV(srcU[x / 2], srcV[x / 2]);

                            yuv.CopyTo(ref dst00[x + 0], srcY0[x + 0]);
                            yuv.CopyTo(ref dst00[x + 1], srcY0[x + 1]);
                            yuv.CopyTo(ref dst11[x + 0], srcY1[x + 0]);
                            yuv.CopyTo(ref dst11[x + 1], srcY1[x + 1]);
                        }

                        return;
                    }

                    throw new NotImplementedException();
                }

                public static void TransferYUV420<TDstPixel>(Span<TDstPixel> dst0, Span<TDstPixel> dst1, ReadOnlySpan<Byte> srcY0, ReadOnlySpan<Byte> srcY1, ReadOnlySpan<ushort> srcU, ReadOnlySpan<ushort> srcV)
                    where TDstPixel : unmanaged
                {
                    KernelRGB yuv = default;

                    if (typeof(TDstPixel) == typeof(BGR24))
                    {
                        var dst00 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, BGR24>(dst0);
                        var dst11 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, BGR24>(dst1);

                        for (int x = 0; x < dst0.Length - 1; x += 2)
                        {
                            yuv.SetUV(srcU[x / 2] >> 8, srcV[x / 2] >> 8);

                            yuv.CopyTo(ref dst00[x + 0], srcY0[x + 0]);
                            yuv.CopyTo(ref dst00[x + 1], srcY0[x + 1]);
                            yuv.CopyTo(ref dst11[x + 0], srcY1[x + 0]);
                            yuv.CopyTo(ref dst11[x + 1], srcY1[x + 1]);
                        }

                        return;
                    }

                    if (typeof(TDstPixel) == typeof(RGB24))
                    {
                        var dst00 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, RGB24>(dst0);
                        var dst11 = System.Runtime.InteropServices.MemoryMarshal.Cast<TDstPixel, RGB24>(dst1);

                        for (int x = 0; x < dst0.Length - 1; x += 2)
                        {
                            yuv.SetUV(srcU[x / 2] >> 8, srcV[x / 2] >> 8);

                            yuv.CopyTo(ref dst00[x + 0], srcY0[x + 0]);
                            yuv.CopyTo(ref dst00[x + 1], srcY0[x + 1]);
                            yuv.CopyTo(ref dst11[x + 0], srcY1[x + 0]);
                            yuv.CopyTo(ref dst11[x + 1], srcY1[x + 1]);
                        }

                        return;
                    }

                    throw new NotImplementedException();
                }

                #endregion
            }
        }
    }
}
