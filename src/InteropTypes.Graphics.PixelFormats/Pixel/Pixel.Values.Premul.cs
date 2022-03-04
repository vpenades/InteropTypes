using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropTypes.Graphics
{
    partial class Pixel
    {
        #region static methods

        public static void ApplyPremul(ref Byte r, ref Byte g, ref Byte b, Byte a)
        {
            if (a == 0) { r = g = b = 0; }
            else
            {
                r = (byte)((r * a) / 255);                
                g = (byte)((g * a) / 255);                
                b = (byte)((b * a) / 255);
            }
        }

        public static void ApplyUnpremul(ref Byte r, ref Byte g, ref Byte b, Byte a)
        {
            if (a == 0) { r = g = b = 0; }
            else
            {
                var x = (255 * 256) / a;
                r = (Byte)((r * x) / 256);
                g = (Byte)((g * x) / 256);
                b = (Byte)((b * x) / 256);                
            }
        }

        public static void ApplyPremul(ref float r, ref float g, ref float b, float a)
        {
            if (a == 0) { r = g = b = 0; }
            else
            {
                r *= a;
                g *= a;
                b *= a;
            }
        }

        public static void ApplyUnpremul(ref float r, ref float g, ref float b, float a)
        {
            if (a == 0) { r = g = b = 0; }
            else
            {
                a = 1f / a;
                r *= a;
                g *= a;
                b *= a;
            }
        }

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

        public static XYZA GetPremul(XYZA xxxa)
        {
            if (xxxa.W == 0) return XYZA.Zero;            
            var a = xxxa.W;
            xxxa *= a;
            xxxa.W = a;
            return xxxa;            
        }

        public static XYZA GetUnpremul(XYZA xxxa)
        {
            if (xxxa.W == 0) return XYZA.Zero;
            var a = xxxa.W;
            xxxa /= a;
            xxxa.W = a;
            return xxxa;
        }

        #endregion

        #region normal type converters

        partial struct BGRA32
        {
            public BGRA32(RGBP32 color)
            {
                if (color.A == 0) this = default;
                else
                {
                    var rcpA = (255 * 256) / color.A;
                    R = (Byte)((color.PreR * rcpA) / 256);
                    G = (Byte)((color.PreG * rcpA) / 256);
                    B = (Byte)((color.PreB * rcpA) / 256);
                    A = color.A;
                }
            }

            public BGRA32(BGRP32 color)
            {
                if (color.A == 0) this = default;
                else
                {
                    var rcpA = (255 * 256) / color.A;
                    R = (Byte)((color.PreR * rcpA) / 256);
                    G = (Byte)((color.PreG * rcpA) / 256);
                    B = (Byte)((color.PreB * rcpA) / 256);
                    A = color.A;
                }
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

            public XYZA ToNativePremul()
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

            public RGBP32(in RGBA128F rgba)
            {
                var v = rgba.ToNativePremul() * 255f;
                PreR = (Byte)v.X;
                PreG = (Byte)v.Y;
                PreB = (Byte)v.Z;
                A = (Byte)v.W;
            }

            public RGBP32(in BGRA32 bgra)
            {
                PreR = bgra.R;
                PreG = bgra.G;
                PreB = bgra.B;
                A = bgra.A;
                ApplyPremul(ref PreR, ref PreG, ref PreB, A);
            }

            public RGBP32(Byte premulRed, Byte premulGreen, Byte premulBlue, Byte alpha)
            {
                PreB = premulBlue;
                PreG = premulGreen;
                PreR = premulRed;
                A = alpha;
            }

            public RGBP32(int premulRed, int premulGreen, int premulBlue, int alpha = 255)
            {
                PreR = (byte)premulRed;
                PreG = (byte)premulGreen;
                PreB = (byte)premulBlue;
                A = (byte)alpha;
            }

            #endregion

            #region data

            public uint RGBP
            {
                get
                {
                    var tmp = this;
                    return System.Runtime.CompilerServices.Unsafe.As<RGBP32, uint>(ref tmp);
                }
            }

            public Byte PreR;
            public Byte PreG;
            public Byte PreB;
            public Byte A;

            public Byte R => A == 0 ? (Byte)0 : (Byte)(PreR * 255 / A);
            public Byte G => A == 0 ? (Byte)0 : (Byte)(PreG * 255 / A);
            public Byte B => A == 0 ? (Byte)0 : (Byte)(PreB * 255 / A);            

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

            public BGRP32(in RGBA128F rgba)
            {
                var v = rgba.ToNativePremul() * 255f;
                PreB = (Byte)v.Z;
                PreG = (Byte)v.Y;
                PreR = (Byte)v.X;
                A = (Byte)v.W;
            }

            public BGRP32(in BGRA32 bgra)
            {
                PreR = bgra.R;
                PreG = bgra.G;
                PreB = bgra.B;
                A = bgra.A;
                ApplyPremul(ref PreR, ref PreG, ref PreB, A);
            }

            public BGRP32(Byte premulRed, Byte premulGreen, Byte premulBlue, Byte alpha)
            {
                PreB = premulBlue;
                PreG = premulGreen;
                PreR = premulRed;
                A = alpha;
            }

            public BGRP32(int premulRed, int premulGreen, int premulBlue, int alpha = 255)
            {
                PreR = (byte)premulRed;
                PreG = (byte)premulGreen;
                PreB = (byte)premulBlue;
                A = (byte)alpha;
            }

            #endregion

            #region data

            public uint BGRP
            {
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

            public Byte R => A == 0 ? (Byte)0 : (Byte)(PreR * 255 / A);
            public Byte G => A == 0 ? (Byte)0 : (Byte)(PreG * 255 / A);
            public Byte B => A == 0 ? (Byte)0 : (Byte)(PreB * 255 / A);

            #endregion

            #region API - Lerps

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRP32 Lerp(RGB24 left, RGB24 right, int rx)
            {
                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var R = (left.R * lx + right.R * rx) / 16384;
                var G = (left.G * lx + right.G * rx) / 16384;
                var B = (left.B * lx + right.B * rx) / 16384;
                return new BGRP32(R, G, B, 255);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRP32 Lerp(BGR24 left, BGR24 right, int rx)
            {
                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var B = (left.B * lx + right.B * rx) / 16384;
                var G = (left.G * lx + right.G * rx) / 16384;
                var R = (left.R * lx + right.R * rx) / 16384;                
                return new BGRP32(R, G, B, 255);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRP32 Lerp(RGBA32 p00, RGBA32 p01, int rx)
            {
                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate final alpha
                int a = (p00.A * lx + p01.A * rx) / 16384;
                if (a == 0) return default;

                // calculate premultiplied RGB
                lx *= p00.A;
                rx *= p01.A;
                int r = (p00.R * lx + p01.R * rx) / (16384 * 255);
                int g = (p00.G * lx + p01.G * rx) / (16384 * 255);
                int b = (p00.B * lx + p01.B * rx) / (16384 * 255);

                // unpremultiply RGB
                return new BGRP32(r , g , b , a);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRP32 Lerp(in RGBA32 p00, in RGBA32 p01, in RGBA32 p10, in RGBA32 p11, int rx, int by)
            {
                // calculate quantized weights
                var lx = 16384 - rx;
                var ty = 16384 - by;
                var w00 = lx * ty / 16384;
                var w01 = rx * ty / 16384;
                var w10 = lx * by / 16384;
                var w11 = rx * by / 16384;

                System.Diagnostics.Debug.Assert((w00 + w01 + w10 + w11) == 16384);

                // calculate final alpha

                int a = (p00.A * w00 + p01.A * w01 + p10.A * w10 + p11.A * w11) / 16384;

                if (a == 0) return default;

                // calculate premultiplied RGB

                w00 *= p00.A;
                w01 *= p01.A;
                w10 *= p10.A;
                w11 *= p11.A;

                int r = (p00.R * w00 + p01.R * w01 + p10.R * w10 + p11.R * w11) / (16384 * 255);
                int g = (p00.G * w00 + p01.G * w01 + p10.G * w10 + p11.G * w11) / (16384 * 255);
                int b = (p00.B * w00 + p01.B * w01 + p10.B * w10 + p11.B * w11) / (16384 * 255);                

                return new BGRP32(r, g, b, a);
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

            public XYZ RGB => A == 0 ? XYZ.Zero : PreRGB / A;

            public Single R => A == 0 ? 0 : Math.Min(1, PreR / A);
            public Single G => A == 0 ? 0 : Math.Min(1, PreG / A);
            public Single B => A == 0 ? 0 : Math.Min(1, PreB / A);            

            #endregion      
        }

        #endregion
    }
}
