using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        #region static methods                

        public static void ApplyPremul(ref XYZA xxxa)
        {
            if (xxxa.W == 0) { xxxa = XYZA.Zero; }
            else
            {
                var a = xxxa.W;
                xxxa *= a;
                xxxa.W = a;                
            }
        }

        public static void ApplyUnpremul(ref XYZA xxxa)
        {
            if (xxxa.W == 0) { xxxa = XYZA.Zero; }
            else
            {
                var a = xxxa.W;
                xxxa /= a;
                xxxa.W = a;
            }
        }        

        #endregion

        #region normal type converters

        partial struct BGRA32
        {
            public BGRA32(RGBP32 value)
            {
                uint rcpA = value.A == 0 ? 0u : (65536u * 255u) / (uint)value.A;
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                A = value.A;
            }

            public BGRA32(BGRP32 value)
            {
                uint rcpA = value.A == 0 ? 0u : (65536u * 255u) / (uint)value.A;
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                A = value.A;
            }

            public BGRA32(RGBP128F color)
            {
                var c = color.A == 0
                    ? XYZA.Zero
                    : new XYZA(color.PreRGB / color.A, color.A) * 255f;

                R = (Byte)c.X;
                G = (Byte)c.Y;
                B = (Byte)c.Z;
                A = (Byte)c.W;
            }
        }

        partial struct RGBA128F
        {
            public RGBA128F(RGBP32 color)
            {
                this = default;
                this.RGBA = new XYZA(color.PreR, color.PreG, color.PreB, color.A) / 255f;
                ApplyUnpremul(ref this.RGBA);
            }

            public RGBA128F(BGRP32 color)
            {
                this = default;
                this.RGBA = new XYZA(color.PreR, color.PreG, color.PreB, color.A) / 255f;
                ApplyUnpremul(ref this.RGBA);
            }

            public RGBA128F(RGBP128F color)
            {
                this = default;
                this.RGBA = color.RGBP;
                ApplyUnpremul(ref this.RGBA);
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public readonly XYZA ToNativePremul()
            {
                return new XYZA(RGB * A, A);
            }

            public static RGBA128F FromNativePremul(in XYZA nativePremul)
            {
                if (nativePremul.W == 0) return default;
                return new RGBA128F(new XYZ(nativePremul.X, nativePremul.Y, nativePremul.Z) / nativePremul.W, nativePremul.W);
            }            
        }

        #endregion

        #region premultiplied types

        /// <summary>
        /// Standard RGBA Quantized to 8 bits. Alpha Premultiplied
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{PreR} {PreG} {PreB} {A}")]
        public partial struct RGBP32
        {
            #region constructors            

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGBP32(in RGBA128F value)
            {
                var v = value.ToNativePremul() * 255f;
                PreR = (Byte)v.X;
                PreG = (Byte)v.Y;
                PreB = (Byte)v.Z;
                A = (Byte)v.W;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGBP32(BGRA32 value)
            {
                uint fwdA = 257u * (uint)value.A;
                PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                A = value.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGBP32(Byte premulRed, Byte premulGreen, Byte premulBlue, Byte alpha)
            {
                PreB = premulBlue;
                PreG = premulGreen;
                PreR = premulRed;
                A = alpha;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGBP32(int premulRed, int premulGreen, int premulBlue, int alpha = 255)
            {
                PreR = (byte)premulRed;
                PreG = (byte)premulGreen;
                PreB = (byte)premulBlue;
                A = (byte)alpha;
            }

            #endregion

            #region data

            public readonly uint RGBP
            {
                get
                {
                    var tmp = this;
                    return Unsafe.As<RGBP32, uint>(ref tmp);
                }
            }

            public Byte PreR;
            public Byte PreG;
            public Byte PreB;
            public Byte A;

            public readonly Byte R => A == 0 ? (Byte)0 : (Byte)(PreR * 255 / A);
            public readonly Byte G => A == 0 ? (Byte)0 : (Byte)(PreG * 255 / A);
            public readonly Byte B => A == 0 ? (Byte)0 : (Byte)(PreB * 255 / A);            

            #endregion
        }

        /// <summary>
        /// Standard BGRA Quantized to 8 bits. Alpha Premultiplied
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{PreR} {PreG} {PreB} {A}")]
        public partial struct BGRP32
        {
            #region constructors            

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(Alpha8 value)
            {
                PreR = PreG = PreB = A = value.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(Luminance8 value)
            {
                PreR = PreG = PreB = value.L;
                A = 255;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(BGR565 value)
            {
                PreR = value.R;
                PreG = value.G;
                PreB = value.B;
                A = 255;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(BGR24 value)
            {
                PreR = value.R;
                PreG = value.G;
                PreB = value.B;
                A = 255;                
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(RGB24 value)
            {
                PreR = value.R;
                PreG = value.G;
                PreB = value.B;
                A = 255;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(BGRA5551 value)
            {
                uint alpha = (uint)(value.BGRA >> 15);
                PreR = (Byte)(value.Ru * alpha);
                PreG = (Byte)(value.Gu * alpha);
                PreB = (Byte)(value.Bu * alpha);
                A = value.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(BGRA4444 value)
            {
                uint fwdA = 257u * (uint)value.A;
                PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                A = (Byte)value.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(BGRA32 value)
            {
                uint fwdA = 257u * (uint)value.A;
                PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                A = value.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(RGBA32 value)
            {
                uint fwdA = 257u * (uint)value.A;
                PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                A = value.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(ARGB32 value)
            {
                uint fwdA = 257u * (uint)value.A;
                PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                A = value.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(RGBP32 value)
            {
                PreR = value.PreR;
                PreG = value.PreG;
                PreB = value.PreB;
                A = value.A;                
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(in RGBA128F value)
            {
                var v = value.ToNativePremul() * 255f;
                PreB = (Byte)v.Z;
                PreG = (Byte)v.Y;
                PreR = (Byte)v.X;
                A = (Byte)v.W;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(Byte premulRed, Byte premulGreen, Byte premulBlue, Byte alpha)
            {
                PreB = premulBlue;
                PreG = premulGreen;
                PreR = premulRed;
                A = alpha;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32(int premulRed, int premulGreen, int premulBlue, int alpha = 255)
            {
                PreR = (byte)premulRed;
                PreG = (byte)premulGreen;
                PreB = (byte)premulBlue;
                A = (byte)alpha;
            }

            #endregion

            #region data

            public readonly uint BGRP
            {
                [MethodImpl(_PrivateConstants.Fastest)]
                get
                {
                    var tmp = this;
                    return Unsafe.As<BGRP32, uint>(ref tmp);
                }
            }

            public Byte PreB;
            public Byte PreG;
            public Byte PreR;
            public Byte A;

            public readonly Byte R => A == 0 ? (Byte)0 : (Byte)(((uint)PreR * 255u) / (uint)A);
            public readonly Byte G => A == 0 ? (Byte)0 : (Byte)(((uint)PreG * 255u) / (uint)A);
            public readonly Byte B => A == 0 ? (Byte)0 : (Byte)(((uint)PreB * 255u) / (uint)A);

            #endregion

            #region API from-to

            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 From<TPixel>(in TPixel value) // according to benchmarks, this method is about as fast as a direct constructor
                where TPixel : unmanaged
            {
                switch(value)
                {
                    case Alpha8 pixel: return new BGRP32(pixel);
                    case Luminance8 pixel: return new BGRP32(pixel);

                    case BGR565 pixel: return new BGRP32(pixel);
                    case BGR24 pixel: return new BGRP32(pixel);
                    case RGB24 pixel: return new BGRP32(pixel);

                    case BGRA5551 pixel: return new BGRP32(pixel);
                    case BGRA4444 pixel: return new BGRP32(pixel);
                    case BGRA32 pixel: return new BGRP32(pixel);
                    case RGBA32 pixel: return new BGRP32(pixel);
                    case ARGB32 pixel: return new BGRP32(pixel);

                    case BGRP32 pixel: return pixel;
                    case RGBP32 pixel: return new BGRP32(pixel);

                    default: throw new NotImplementedException();
                }                
            }

            #endregion
        }

        /// <summary>
        /// Standard RGBP in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct RGBP128F
        {
            #region RGB constructors            

            public RGBP128F(Alpha8 alpha) : this(255, 255, 255, alpha.A) { }
            public RGBP128F(Luminance8 luma) : this(luma.L, luma.L, luma.L, 255) { }
            public RGBP128F(Luminance16 luma) : this(new XYZ(luma.L, luma.L, luma.L) / 65535f) { }
            public RGBP128F(Luminance32F luma) : this(new XYZ(luma.L, luma.L, luma.L)) { }
            public RGBP128F(BGR565 color) : this(color.R, color.G, color.B, 255) { }
            public RGBP128F(BGR24 color) : this(color.R, color.G, color.B, 255) { }
            public RGBP128F(RGB24 color) : this(color.R, color.G, color.B, 255) { }
            public RGBP128F(BGRA5551 color) : this(color.R, color.G, color.B, color.A) { }
            public RGBP128F(BGRA4444 color) : this(color.R, color.G, color.B, color.A) { }
            public RGBP128F(RGBA32 color) : this(color.R, color.G, color.B, color.A) { }
            public RGBP128F(BGRA32 color) : this(color.R, color.G, color.B, color.A) { }
            public RGBP128F(ARGB32 color) : this(color.R, color.G, color.B, color.A) { }

            public RGBP128F(byte r, byte g, byte b, byte a) : this()
            {                
                RGBP = new XYZA(r, g, b, a) / 255f;
                PreRGB *= a;
            }

            public RGBP128F(Single r, Single g, Single b, Single a) : this()
            {             
                RGBP = new XYZA(r, g, b, a);
                PreRGB *= a;
            }

            #endregion

            #region premultiplied constructors

            public RGBP128F(in BGR96F color) : this()
            {
                RGBP = new XYZA(color.R,color.G,color.B, 1);
            }

            public RGBP128F(in RGB96F color) : this()
            {
                RGBP = new XYZA(color.RGB, 1);
            }

            public RGBP128F(in RGBA128F color) : this()
            {
                RGBP = new XYZA(color.RGB * color.A, color.A);
            }

            public RGBP128F(in BGRA128F color) : this()
            {
                RGBP = new XYZA(color.R, color.G, color.B, color.A);
                PreRGB *= color.A;
            }

            public RGBP128F(RGBP32 color) : this()
            {
                RGBP = new XYZA(color.PreR, color.PreG, color.PreB, color.A) / 255f;
            }

            public RGBP128F(BGRP32 color) : this()
            {
                RGBP = new XYZA(color.PreR, color.PreG, color.PreB, color.A) / 255f;
            }

            public RGBP128F(in XYZ v) : this()
            {                
                RGBP = new XYZA(v, 1);
            }

            public RGBP128F(in XYZ prergb, float a) : this()
            {             
                RGBP = new XYZA(prergb, a);
            }

            public RGBP128F(in XYZA prergba) : this()
            {             
                RGBP = prergba;
            }

            #endregion

            #region data

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZA RGBP;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZ PreRGB;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public Single PreR;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public Single PreG;

            [System.Runtime.InteropServices.FieldOffset(8)]
            public Single PreB;

            [System.Runtime.InteropServices.FieldOffset(12)]
            public Single A;

            public readonly XYZ RGB => A == 0 ? XYZ.Zero : PreRGB / A;

            public readonly Single R => A == 0 ? 0 : Math.Min(1, PreR / A);
            public readonly Single G => A == 0 ? 0 : Math.Min(1, PreG / A);
            public readonly Single B => A == 0 ? 0 : Math.Min(1, PreB / A);

            #endregion

            #region API

            public static RGBP128F Lerp(in RGBP128F p00, in RGBP128F p01, in RGBP128F p10, in RGBP128F p11, float rx, float by)
            {                
                var lx = 1f - rx;
                var ty = 1f - by;               

                var r = p00.RGBP * (lx * ty);
                r += p01.RGBP * (rx * ty);
                r += p10.RGBP * (lx * by);
                r += p11.RGBP * (rx * by);

                return new RGBP128F(r);
            }

            #endregion
        }

        #endregion
    }
}
