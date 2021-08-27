using System;
using System.Collections.Generic;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropBitmaps
{
    partial class Pixel
    {
        partial struct BGRA32
        {
            public BGRA32(RGBA32P color)
            {
                if (color.A == 0) { this = default; return; }
                R = (Byte)(color.R * color.A);
                G = (Byte)(color.G * color.A);
                B = (Byte)(color.B * color.A);
                A = color.A;
            }

            public BGRA32(BGRA32P color)
            {
                if (color.A == 0) { this = default; return; }
                R = (Byte)(color.R * color.A);
                G = (Byte)(color.G * color.A);
                B = (Byte)(color.B * color.A);
                A = color.A;
            }
        }

        partial struct VectorRGBA
        {
            public VectorRGBA(RGBA32P color)
            {
                if (color.A == 0) { this = default; return; }

                this.RGBA = new XYZA(color.R, color.G, color.B, color.A);
                this.RGBA *= this.RGBA.W;
                this.RGBA.W = color.A;
                this.RGBA /= 255f;
            }

            public VectorRGBA(BGRA32P color)
            {
                if (color.A == 0) { this = default; return; }

                this.RGBA = new XYZA(color.R, color.G, color.B, color.A);
                this.RGBA *= this.RGBA.W;
                this.RGBA.W = color.A;
                this.RGBA /= 255f;
            }
        }


        /// <summary>
        /// RGBA Quantized to 8 bits. Alpha Premultiplied
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct RGBA32P : IPixelReflection<RGBA32P>, IPixelBlendOps<RGBA32P, RGBA32P>
        {
            #region constructors

            public RGBA32P(in VectorRGBA rgba)
            {
                if (rgba.A == 0) { this = default; return; }

                var v = rgba.RGBA;
                v *= 255f;

                A = (Byte)v.W;
                
                v /= v.W;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;                
            }

            public RGBA32P(in BGRA32 bgra)
            {
                if (bgra.A == 0) { this = default; return; }

                R = (Byte)(bgra.R / bgra.A);
                G = (Byte)(bgra.G / bgra.A);
                B = (Byte)(bgra.B / bgra.A);
                A = bgra.A;
            }

            public RGBA32P(Byte premulRed, Byte premulGreen, Byte premulBlue, Byte alpha)
            {
                B = premulBlue;
                G = premulGreen;
                R = premulRed;
                A = alpha;
            }

            public RGBA32P(int premulRed, int premulGreen, int premulBlue, int alpha = 255)
            {
                R = (byte)premulRed;
                G = (byte)premulGreen;
                B = (byte)premulBlue;
                A = (byte)alpha;
            }

            #endregion

            #region data

            public uint RGBA
            {
                get
                {
                    var tmp = this;
                    return System.Runtime.CompilerServices.Unsafe.As<RGBA32P, uint>(ref tmp);
                }
            }

            public readonly Byte R;
            public readonly Byte G;
            public readonly Byte B;
            public readonly Byte A;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(this); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            RGBA32P IPixelReflection<RGBA32P>.From(BGRA32 color) { return new RGBA32P(color); }
            RGBA32P IPixelReflection<RGBA32P>.From(VectorRGBA color) { return new RGBA32P(color); }

            public RGBA32P AverageWith(RGBA32P other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new RGBA32P(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2);
            }

            #endregion
        }

        /// <summary>
        /// BGRA Quantized to 8 bits. Alpha Premultiplied
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct BGRA32P : IPixelReflection<BGRA32P>, IPixelBlendOps<BGRA32P, BGRA32P>
        {
            #region constructors

            public BGRA32P(in VectorRGBA rgba)
            {
                if (rgba.A == 0) { this = default; return; }

                var v = rgba.RGBA;
                v *= 255f;

                A = (Byte)v.W;

                v /= v.W;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;
            }

            public BGRA32P(in BGRA32 bgra)
            {
                if (bgra.A == 0) { this = default; return; }

                R = (Byte)(bgra.R / bgra.A);
                G = (Byte)(bgra.G / bgra.A);
                B = (Byte)(bgra.B / bgra.A);
                A = bgra.A;
            }

            public BGRA32P(Byte premulRed, Byte premulGreen, Byte premulBlue, Byte alpha)
            {
                B = premulBlue;
                G = premulGreen;
                R = premulRed;
                A = alpha;
            }

            public BGRA32P(int premulRed, int premulGreen, int premulBlue, int alpha = 255)
            {
                R = (byte)premulRed;
                G = (byte)premulGreen;
                B = (byte)premulBlue;
                A = (byte)alpha;
            }

            #endregion

            #region data

            public uint BGRA
            {
                get
                {
                    var tmp = this;
                    return System.Runtime.CompilerServices.Unsafe.As<BGRA32P, uint>(ref tmp);
                }
            }

            public readonly Byte B;
            public readonly Byte G;
            public readonly Byte R;
            public readonly Byte A;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(this); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            BGRA32P IPixelReflection<BGRA32P>.From(BGRA32 color) { return new BGRA32P(color); }
            BGRA32P IPixelReflection<BGRA32P>.From(VectorRGBA color) { return new BGRA32P(color); }

            public BGRA32P AverageWith(BGRA32P other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new BGRA32P(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2);
            }

            #endregion
        }
    }
}
