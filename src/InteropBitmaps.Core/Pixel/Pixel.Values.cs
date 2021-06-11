using System;
using System.Collections.Generic;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropBitmaps
{
    partial class Pixel
    {
        public interface IConvertible
        {
            // int QuantizedBits { get; }

            BGRA32 ToBGRA32();
            VectorRGBA ToVectorRGBA();

            // XYZA ToPremul();
        }

        public interface IPixelReflection<TPixel> : IConvertible
        {
            TPixel From(BGRA32 color);
            TPixel From(VectorRGBA color);

            // TPixel FromPremul(XYZA premultiplied);

            // TPixel AverageWith(TPixel other);
        }

        public interface IPixelBlendOps<TDstPixel, TSrcPixel> : IConvertible
        {
            TDstPixel AverageWith(TSrcPixel other);

            // TODO: TDstPixel LerpWith(TSrcPixel other);
        }

        /// <summary>
        /// Alpha (Transparency) Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{A}")]
        public readonly partial struct Alpha8 : IPixelReflection<Alpha8>, IPixelBlendOps<Alpha8, Alpha8>
        {
            #region constructors
            public Alpha8(Byte alpha) { A = alpha; }
            #endregion

            #region data

            public readonly Byte A;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32((Byte)0, (Byte)0, (Byte)0, A); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            Alpha8 IPixelReflection<Alpha8>.From(BGRA32 color) { return new Alpha8(color.A); }
            Alpha8 IPixelReflection<Alpha8>.From(VectorRGBA color) { return new Alpha8((Byte)(color.A * 255f)); }

            public Alpha8 AverageWith(Alpha8 other)
            {
                return new Alpha8((Byte)((this.A + other.A) / 2));
            }

            #endregion
        }

        /// <summary>
        /// Luminance (Gray) Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public readonly partial struct Luminance8 : IPixelReflection<Luminance8>
        {
            #region constants

            const uint RLuminanceWeight = 19562;
            const uint GLuminanceWeight = 38550;
            const uint BLuminanceWeight = 7424;

            #endregion

            #region constructors

            public Luminance8(Byte luminance) { L = luminance; }
            public Luminance8(BGRA32 color)
            {
                uint accum = 0;

                accum += RLuminanceWeight * (uint)color.R;
                accum += GLuminanceWeight * (uint)color.G;
                accum += BLuminanceWeight * (uint)color.B;

                accum >>= 16;

                L  =(Byte)accum;
            }

            public Luminance8(VectorRGBA color)
            {
                float accum = color.R * 0.2989f + color.G * 0.5870f + color.B * 0.1140f;
                accum *= 255f;

                L = (Byte)accum;
            }

            #endregion

            #region data

            public readonly Byte L;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(L, L, L); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            Luminance8 IPixelReflection<Luminance8>.From(BGRA32 color) { return new Luminance8(color); }
            Luminance8 IPixelReflection<Luminance8>.From(VectorRGBA color) { return new Luminance8(color); }

            #endregion
        }

        /// <summary>
        /// Luminance (Gray) Quantized to 16 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public readonly partial struct Luminance16 : IPixelReflection<Luminance16>
        {
            #region constants            

            const uint RLuminanceWeight = 19562;
            const uint GLuminanceWeight = 38550;
            const uint BLuminanceWeight = 7424;

            #endregion

            #region constructors
            public Luminance16(UInt16 luminance) { L = luminance; }

            public Luminance16(BGRA32 color)
            {
                uint accum = 0;

                accum += RLuminanceWeight * (uint)color.R;
                accum += GLuminanceWeight * (uint)color.G;
                accum += BLuminanceWeight * (uint)color.B;

                accum >>= 8;

                L = (UInt16)accum;
            }

            public Luminance16(VectorRGBA color)
            {
                float accum = color.R * 0.2989f + color.G * 0.5870f + color.B * 0.1140f;
                accum *= 65535f;

                L = (UInt16)accum;
            }

            #endregion

            #region data

            public readonly UInt16 L;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { var v = (Byte)(L >> 8); return new BGRA32(v, v, v); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            Luminance16 IPixelReflection<Luminance16>.From(BGRA32 color) { return new Luminance16(color); }
            Luminance16 IPixelReflection<Luminance16>.From(VectorRGBA color) { return new Luminance16(color); }             

            #endregion
        }

        /// <summary>
        /// Luminance
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public readonly partial struct LuminanceScalar : IPixelReflection<LuminanceScalar>
        {
            #region constructors
            public LuminanceScalar(UInt16 luminance) { L = luminance; }

            public LuminanceScalar(BGRA32 color)
            {
                float accum = 0;

                accum += 0.2989f * color.R;
                accum += 0.5870f * color.G;
                accum += 0.1140f * color.B;

                accum /= 255;

                L = accum;
            }

            public LuminanceScalar(VectorRGBA color)
            {
                float accum = color.R * 0.2989f + color.G * 0.5870f + color.B * 0.1140f;
                accum *= 65535f;

                L = accum;
            }

            #endregion

            #region data

            public readonly float L;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { var v = (Byte)(L * 255); return new BGRA32(v, v, v); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            LuminanceScalar IPixelReflection<LuminanceScalar>.From(BGRA32 color) { return new LuminanceScalar(color); }
            LuminanceScalar IPixelReflection<LuminanceScalar>.From(VectorRGBA color) { return new LuminanceScalar(color); }

            #endregion
        }

        /// <summary>
        /// BGR Quantized to 5,6,5 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct BGR565 : IPixelReflection<BGR565>
        {
            #region constructors

            public BGR565(Byte red, Byte green, Byte blue)
            {
                var x = blue >> 3;
                x |= (green >> 2) << 5;
                x |= (red >> 3) << 11;
                BGR = (UInt16)x;
            }
            
            #endregion

            #region data

            public readonly UInt16 BGR;

            public int R { get { var p = ((BGR >> 11) & 0x1f); return p * 8 + (p >> 2); } }
            public int G { get { var p = ((BGR >> 5) & 0x3f); return p * 4 + (p >> 4); } }
            public int B { get { var p = BGR & 0x1f; return p * 8 + (p >> 2); } }

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32((Byte)R, (Byte)G, (Byte)B, (Byte)255); }            
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }

            BGR565 IPixelReflection<BGR565>.From(BGRA32 color) { return new BGR565(color.R, color.G, color.B); }
            BGR565 IPixelReflection<BGR565>.From(VectorRGBA color)
            {
                var v = color.RGBA * 255f;
                return new BGR565((Byte)v.X, (Byte)v.Y, (Byte)v.Z);
            }

            #endregion
        }

        /// <summary>
        /// BGRA Quantized to 5,5,5,1 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct BGRA5551 : IPixelReflection<BGRA5551>
        {
            #region constructors

            public BGRA5551(Byte red, Byte green, Byte blue, Boolean alpha)
            {
                var x = blue >> 3;
                x |= (green >> 3) << 5;
                x |= (red >> 3) << 10;
                x |= alpha ? 0x10000000 : 0;
                BGRA = (UInt16)x;
            }

            #endregion

            #region data

            public readonly UInt16 BGRA;

            public int R { get { var p = ((BGRA >> 10) & 0x1f); return p * 8 + (p >> 2); } }
            public int G { get { var p = ((BGRA >> 5) & 0x1f); return p * 8 + (p >> 2); } }
            public int B { get { var p = BGRA & 0x1f; return p * 8 + (p >> 2); } }
            public int A => (BGRA >> 15) * 255;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32((Byte)R, (Byte)G, (Byte)B, (Byte)A); }            
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }

            BGRA5551 IPixelReflection<BGRA5551>.From(BGRA32 color) { return new BGRA5551(color.R, color.G, color.B, color.A >= 128); }
            BGRA5551 IPixelReflection<BGRA5551>.From(VectorRGBA color)
            {
                var v = color.RGBA * 255f;
                return new BGRA5551((Byte)v.X, (Byte)v.Y, (Byte)v.Z, v.W >= 128);
            }

            #endregion
        }

        /// <summary>
        /// BGRA Quantized to 4,4,4,4 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct BGRA4444 : IPixelReflection<BGRA4444>
        {
            #region constructors

            public BGRA4444(Byte red, Byte green, Byte blue, Byte alpha)
            {
                var x = blue >> 4;
                x |= (green >> 4) << 4;
                x |= (red >> 4) << 8;
                x |= (alpha >> 4) << 12;
                BGRA = (UInt16)x;
            }

            #endregion

            #region data

            public readonly UInt16 BGRA;

            public int A { get { var p = ((BGRA >> 12) & 0xf); return p * 17; } }
            public int R { get { var p = ((BGRA >> 8) & 0xf); return p * 17; } }
            public int G { get { var p = ((BGRA >> 4) & 0xf); return p * 17; } }
            public int B { get { var p = BGRA & 0xf; return p * 17; } }

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32((Byte)R, (Byte)G, (Byte)B, (Byte)A); }            
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }

            BGRA4444 IPixelReflection<BGRA4444>.From(BGRA32 color) { return new BGRA4444(color.R, color.G, color.B, color.A); }
            BGRA4444 IPixelReflection<BGRA4444>.From(VectorRGBA color)
            {
                var v = color.RGBA * 255f;
                return new BGRA4444((Byte)v.X, (Byte)v.Y, (Byte)v.Z, (Byte)v.W);
            }

            #endregion
        }

        /// <summary>
        /// BGRA Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct BGRA32 : IPixelReflection<BGRA32>, IPixelBlendOps<BGRA32, BGRA32>
        {
            #region constructors

            public BGRA32(in System.Drawing.Color color) { R = color.R; G = color.G;B = color.B;A = color.A; }

            public BGRA32(in VectorRGBA rgba)
            {
                var v = rgba.RGBA * 255f;

                B = (Byte)v.Z;                
                G = (Byte)v.Y;
                R = (Byte)v.X;
                A = (Byte)v.W;
            }

            public BGRA32(in VectorBGRA bgra)
            {
                var v = bgra.BGRA * 255f;

                B = (Byte)v.X;
                G = (Byte)v.Y;
                R = (Byte)v.Z;
                A = (Byte)v.W;
            }

            public BGRA32(in VectorBGR bgra)
            {
                var v = bgra.BGR * 255f;

                B = (Byte)v.X;
                G = (Byte)v.Y;
                R = (Byte)v.Z;
                A = 255;
            }

            public BGRA32(Byte red, Byte green, Byte blue, Byte alpha = 255)
            {
                B = blue;
                G = green;
                R = red;
                A = alpha;
            }

            public BGRA32(int red, int green, int blue, int alpha = 255)
            {
                B = (byte)blue;
                G = (byte)green;
                R = (byte)red;
                A = (byte)alpha;
            }

            public BGRA32(BGR24 bgr)
            {
                B = bgr.B;
                G = bgr.G;
                R = bgr.R;
                A = 255;
            }

            #endregion

            #region data

            public uint BGRA
            {
                get
                {
                    var tmp = this;
                    return System.Runtime.CompilerServices.Unsafe.As<BGRA32, uint>(ref tmp);
                }
            }

            public readonly Byte B;
            public readonly Byte G;
            public readonly Byte R;
            public readonly Byte A;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return this; }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            BGRA32 IPixelReflection<BGRA32>.From(BGRA32 color) { return color; }
            BGRA32 IPixelReflection<BGRA32>.From(VectorRGBA color) { return new BGRA32(color); }

            public BGRA32 AverageWith(BGRA32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new BGRA32(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2 );
            }

            #endregion
        }

        /// <summary>
        /// RGBA Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct RGBA32 : IPixelReflection<RGBA32>, IPixelBlendOps<RGBA32, RGBA32>
        {
            #region constructors

            public RGBA32(in VectorRGBA rgba)
            {
                var v = rgba.RGBA * 255f;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;
                A = (Byte)v.W;
            }
            public RGBA32(Byte red, Byte green, Byte blue, Byte alpha) { B = blue; G = green; R = red; A = alpha; }

            public RGBA32(int red, int green, int blue, int alpha = 255)
            {
                R = (byte)red;
                G = (byte)green;
                B = (byte)blue;                
                A = (byte)alpha;
            }

            #endregion

            #region data

            public uint RGBA
            {
                get
                {
                    var tmp = this;
                    return System.Runtime.CompilerServices.Unsafe.As<RGBA32, uint>(ref tmp);
                }
            }

            public readonly Byte R;
            public readonly Byte G;
            public readonly Byte B;
            public readonly Byte A;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(R, G, B, A); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            RGBA32 IPixelReflection<RGBA32>.From(BGRA32 color) { return new RGBA32(color.R, color.G, color.B, color.A); }
            RGBA32 IPixelReflection<RGBA32>.From(VectorRGBA color) { return new RGBA32(color); }

            public RGBA32 AverageWith(RGBA32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new RGBA32(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2);
            }

            #endregion
        }

        /// <summary>
        /// ARGB Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct ARGB32 : IPixelReflection<ARGB32>, IPixelBlendOps<ARGB32, ARGB32>
        {
            #region constructors

            public ARGB32(in VectorRGBA rgba)
            {
                var v = rgba.RGBA * 255f;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;
                A = (Byte)v.W;
            }
            public ARGB32(Byte red, Byte green, Byte blue, Byte alpha) { B = blue; G = green; R = red; A = alpha; }

            public ARGB32(int red, int green, int blue, int alpha = 255)
            {
                A = (byte)alpha;
                R = (byte)red;
                G = (byte)green;
                B = (byte)blue;                
            }

            #endregion

            #region data

            public uint ARGB
            {
                get
                {
                    var tmp = this;
                    return System.Runtime.CompilerServices.Unsafe.As<ARGB32, uint>(ref tmp);
                }
            }

            public readonly Byte A;
            public readonly Byte R;
            public readonly Byte G;
            public readonly Byte B;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(R, G, B, A); }            
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            ARGB32 IPixelReflection<ARGB32>.From(VectorRGBA color) { return new ARGB32(color); }
            ARGB32 IPixelReflection<ARGB32>.From(BGRA32 color) { return new ARGB32(color.R, color.G, color.B, color.A); }

            public ARGB32 AverageWith(ARGB32 other)
            {
                if (this.A == 0) return other;
                if (other.A == 0) return this;

                return new ARGB32(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2,
                    (this.A + other.A) / 2);
            }

            #endregion
        }

        /// <summary>
        /// BGR Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct BGR24 : IPixelReflection<BGR24>, IPixelBlendOps<BGR24, BGR24>
        {
            #region constructors

            public BGR24(in VectorRGBA rgba)
            {
                var v = rgba.RGBA * 255f;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;
            }

            public BGR24(RGBA32 rgba)
            {
                R = rgba.R;
                G = rgba.G;
                B = rgba.B;
            }

            public BGR24(BGRA32 bgra)
            {
                R = bgra.R;
                G = bgra.G;
                B = bgra.B;
            }

            public BGR24(Byte red, Byte green, Byte blue) { B = blue; G = green; R = red; }

            public BGR24(int red, int green, int blue)
            {
                B = (byte)blue;
                G = (byte)green;
                R = (byte)red;                
            }

            #endregion

            #region data

            public readonly Byte B;
            public readonly Byte G;
            public readonly Byte R;

            #endregion

            #region API
            
            public BGRA32 ToBGRA32() { return new BGRA32(R, G, B); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            BGR24 IPixelReflection<BGR24>.From(BGRA32 color) { return new BGR24(color.R, color.G, color.B); }
            BGR24 IPixelReflection<BGR24>.From(VectorRGBA color) { return new BGR24(color); }

            public BGR24 AverageWith(BGR24 other)
            {
                return new BGR24(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2);
            }

            #endregion
        }

        /// <summary>
        /// RGB Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct RGB24 : IPixelReflection<RGB24>, IPixelBlendOps<RGB24, RGB24>
        {
            #region constructors

            public RGB24(in VectorRGBA rgba)
            {
                var v = rgba.RGBA * 255f;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;
            }

            public RGB24(Byte red, Byte green, Byte blue) { B = blue; G = green; R = red; }

            public RGB24(int red, int green, int blue)
            {
                R = (byte)red;
                G = (byte)green;
                B = (byte)blue;
            }

            #endregion

            #region data

            public readonly Byte R;
            public readonly Byte G;
            public readonly Byte B;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(R, G, B); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            RGB24 IPixelReflection<RGB24>.From(BGRA32 color) { return new RGB24(color.R, color.G, color.B); }
            RGB24 IPixelReflection<RGB24>.From(VectorRGBA color) { return new RGB24(color); }

            public RGB24 AverageWith(RGB24 other)
            {
                return new RGB24(
                    (this.R + other.R) / 2,
                    (this.G + other.G) / 2,
                    (this.B + other.B) / 2);
            }

            #endregion
        }

        /// <summary>
        /// BGRA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct VectorBGRA : IPixelReflection<VectorBGRA>
        {
            #region constructors
            public VectorBGRA(Single red, Single green, Single blue, Single alpha) { BGRA = new XYZA(blue, green, red, alpha); }
            public VectorBGRA(BGRA32 color) { BGRA = new XYZA(color.B, color.G, color.R, color.A) * 255f; }
            #endregion

            #region data

            public readonly XYZA BGRA;
            public Single B => BGRA.X;            
            public Single G => BGRA.Y;
            public Single R => BGRA.Z;
            public Single A => BGRA.W;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(this); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(R, G, B, A); }
            VectorBGRA IPixelReflection<VectorBGRA>.From(BGRA32 color) { return new VectorBGRA(color); }
            VectorBGRA IPixelReflection<VectorBGRA>.From(VectorRGBA color) { return new VectorBGRA(color.R, color.G, color.B, color.A); }
            #endregion
        }

        /// <summary>
        /// RGBA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct VectorRGBA : IPixelReflection<VectorRGBA>
        {
            #region constructors
            public VectorRGBA(Alpha8 alpha) { RGBA = new XYZA(0, 0, 0, alpha.A) / 255f; }
            public VectorRGBA(Luminance8 luma) { RGBA = new XYZA(luma.L, luma.L, luma.L, 255) / 255f; }
            public VectorRGBA(Luminance16 luma) { RGBA = new XYZA(luma.L, luma.L, luma.L, 65565) / 65535f; }
            public VectorRGBA(LuminanceScalar luma) { RGBA = new XYZA(luma.L, luma.L, luma.L, 1); }
            public VectorRGBA(BGR565 color) { RGBA = new XYZA(color.R, color.G, color.B, 255) / 255f; }
            public VectorRGBA(BGRA5551 color) { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public VectorRGBA(BGRA4444 color) { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public VectorRGBA(BGR24 color) { RGBA = new XYZA(color.R, color.G, color.B, 255) / 255f; }
            public VectorRGBA(RGB24 color) { RGBA = new XYZA(color.R, color.G, color.B, 255) / 255f; }
            public VectorRGBA(RGBA32 color) { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public VectorRGBA(BGRA32 color) { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public VectorRGBA(ARGB32 color) { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }

            public VectorRGBA(XYZA v) { RGBA = v; }
            public VectorRGBA(XYZ v) { RGBA = new XYZA(v, 1); }

            public VectorRGBA(Single red, Single green, Single blue, Single alpha) { RGBA = new XYZA(red, green, blue, alpha); }

            #endregion

            #region data

            public readonly XYZA RGBA;

            public Single R => RGBA.X;
            public Single G => RGBA.Y;
            public Single B => RGBA.Z;
            public Single A => RGBA.W;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(this); }
            public VectorRGBA ToVectorRGBA() { return this; }
            VectorRGBA IPixelReflection<VectorRGBA>.From(BGRA32 color) { return new VectorRGBA(color); }
            VectorRGBA IPixelReflection<VectorRGBA>.From(VectorRGBA color) { return color; }

            public VectorRGBA FromPremul(XYZA premultiplied)
            {
                if (premultiplied.W == 0) return new VectorRGBA(0, 0, 0, 0);
                
                var a = premultiplied.W;
                premultiplied /= a;
                premultiplied.W = a;

                return new VectorRGBA(premultiplied);                
            }

            public XYZA ToPremul()
            {
                if (RGBA.W == 0) return XYZA.Zero;
                var tmp = this.RGBA * this.RGBA.W;
                tmp.W = this.RGBA.W;
                return tmp;
            }

            #endregion
        }

        /// <summary>
        /// RGBA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct VectorBGR : IPixelReflection<VectorBGR>
        {
            #region constructors
            public VectorBGR(Single red, Single green, Single blue) { BGR = new XYZ(blue, green, red); }
            public VectorBGR(BGRA32 color) { BGR = new XYZ(color.B, color.G, color.R) * 255f; }

            #endregion

            #region data

            public readonly XYZ BGR;

            public Single B => BGR.X;
            public Single G => BGR.Y;
            public Single R => BGR.Z;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(this); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(R, G, B, 1); }
            VectorBGR IPixelReflection<VectorBGR>.From(BGRA32 color) { return new VectorBGR(color); }
            VectorBGR IPixelReflection<VectorBGR>.From(VectorRGBA color) { return new VectorBGR(color.R, color.G, color.B); }

            #endregion
        }
    }
}