using System;
using System.Collections.Generic;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropBitmaps
{
    partial class Pixel
    {
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

        partial struct BGRA32
        {
            public BGRA32(RGBP32 color)
            {                
                R = color.PreR;
                G = color.PreG;
                B = color.PreB;
                A = color.A;
                ApplyUnpremul(ref R, ref G, ref B, A);
            }

            public BGRA32(BGRP32 color)
            {
                R = color.PreR;
                G = color.PreG;
                B = color.PreB;
                A = color.A;
                ApplyUnpremul(ref R, ref G, ref B, A);
            }
        }

        partial struct VectorRGBA
        {
            public VectorRGBA(RGBP32 color)
            {
                this.RGBA = new XYZA(color.PreR, color.PreG, color.PreB, color.A) / 255f;
                ApplyUnpremul(ref this.RGBA);
            }

            public VectorRGBA(BGRP32 color)
            {
                this.RGBA = new XYZA(color.PreR, color.PreG, color.PreB, color.A) / 255f;
                ApplyUnpremul(ref this.RGBA);
            }
        }


        /// <summary>
        /// RGBA Quantized to 8 bits. Alpha Premultiplied
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{PreR} {PreG} {PreB} {A}")]
        public readonly partial struct RGBP32 : IPixelReflection<RGBP32>, IPixelBlendOps<RGBP32, RGBP32>
        {
            #region constructors

            public RGBP32(in VectorRGBA rgba)
            {
                var v = GetPremul(rgba.RGBA) * 255f;
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

            public uint BGRP
            {
                get
                {
                    var tmp = this;
                    return System.Runtime.CompilerServices.Unsafe.As<RGBP32, uint>(ref tmp);
                }
            }

            public readonly Byte PreR;
            public readonly Byte PreG;
            public readonly Byte PreB;
            public readonly Byte A;

            public Byte R => (Byte)(PreR * 255 / A);
            public Byte G => (Byte)(PreR * 255 / A);
            public Byte B => (Byte)(PreR * 255 / A);

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(this); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            RGBP32 IPixelReflection<RGBP32>.From(BGRA32 color) { return new RGBP32(color); }
            RGBP32 IPixelReflection<RGBP32>.From(VectorRGBA color) { return new RGBP32(color); }

            public RGBP32 AverageWith(RGBP32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new RGBP32(
                    (this.PreR + other.PreR) / 2,
                    (this.PreG + other.PreG) / 2,
                    (this.PreB + other.PreB) / 2,
                    (this.A + other.A) / 2);
            }

            #endregion
        }

        /// <summary>
        /// BGRA Quantized to 8 bits. Alpha Premultiplied
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{PreR} {PreG} {PreB} {A}")]
        public readonly partial struct BGRP32 : IPixelReflection<BGRP32>, IPixelBlendOps<BGRP32, BGRP32>
        {
            #region constructors

            public BGRP32(in VectorRGBA rgba)
            {
                var v = GetPremul(rgba.RGBA) * 255f;
                PreR = (Byte)v.X;
                PreG = (Byte)v.Y;
                PreB = (Byte)v.Z;
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
                    return System.Runtime.CompilerServices.Unsafe.As<BGRP32, uint>(ref tmp);
                }
            }

            public readonly Byte PreB;
            public readonly Byte PreG;
            public readonly Byte PreR;
            public readonly Byte A;

            public Byte R => (Byte)(PreR * 255 / A);
            public Byte G => (Byte)(PreR * 255 / A);
            public Byte B => (Byte)(PreR * 255 / A);

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(this); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            BGRP32 IPixelReflection<BGRP32>.From(BGRA32 color) { return new BGRP32(color); }
            BGRP32 IPixelReflection<BGRP32>.From(VectorRGBA color) { return new BGRP32(color); }

            public BGRP32 AverageWith(BGRP32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new BGRP32(
                    (this.PreR + other.PreR) / 2,
                    (this.PreG + other.PreG) / 2,
                    (this.PreB + other.PreB) / 2,
                    (this.A + other.A) / 2);
            }

            #endregion
        }
    }
}
