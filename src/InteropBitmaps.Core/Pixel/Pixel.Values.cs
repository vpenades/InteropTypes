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
        }

        public interface IFactory<TPixel> : IConvertible
        {
            TPixel From(BGRA32 color);
            TPixel From(VectorRGBA color);
        }

        /// <summary>
        /// Alpha (Transparency) Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{A}")]
        public readonly partial struct Alpha8 : IFactory<Alpha8>
        {
            #region constructors
            public Alpha8(Byte alpha) { A = alpha; }
            #endregion

            #region data

            public readonly Byte A;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(0, 0, 0, A); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            public Alpha8 From(BGRA32 color) { return new Alpha8(color.A); }
            public Alpha8 From(VectorRGBA color) { return new Alpha8((Byte)(color.A * 255f)); }

            #endregion
        }

        /// <summary>
        /// Luminance (Gray) Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public readonly partial struct Luminance8 : IFactory<Luminance8>
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
            public Luminance8 From(BGRA32 color) { return new Luminance8(color); }
            public Luminance8 From(VectorRGBA color) { return new Luminance8(color); }

            #endregion
        }

        /// <summary>
        /// Luminance (Gray) Quantized to 16 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public readonly partial struct Luminance16 : IFactory<Luminance16>
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
            public Luminance16 From(BGRA32 color) { return new Luminance16(color); }
            public Luminance16 From(VectorRGBA color) { return new Luminance16(color); }             

            #endregion
        }

        /// <summary>
        /// Luminance
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public readonly partial struct StdLuminance : IFactory<StdLuminance>
        {
            #region constructors
            public StdLuminance(UInt16 luminance) { L = luminance; }

            public StdLuminance(BGRA32 color)
            {
                float accum = 0;

                accum += 0.2989f * color.R;
                accum += 0.5870f * color.G;
                accum += 0.1140f * color.B;

                accum /= 255;

                L = accum;
            }

            public StdLuminance(VectorRGBA color)
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
            public StdLuminance From(BGRA32 color) { return new StdLuminance(color); }
            public StdLuminance From(VectorRGBA color) { return new StdLuminance(color); }

            #endregion
        }

        /// <summary>
        /// BGR Quantized to 5,6,5 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct BGR565 : IFactory<BGR565>
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

            public BGRA32 ToBGRA32() { return new BGRA32((Byte)R, (Byte)G, (Byte)B, 255); }
            public BGR565 From(BGRA32 color) { return new BGR565(color.R, color.G, color.B); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            public BGR565 From(VectorRGBA color)
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
        public readonly partial struct BGRA5551 : IFactory<BGRA5551>
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
            public BGRA5551 From(BGRA32 color) { return new BGRA5551(color.R, color.G, color.B, color.A >= 128); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            public BGRA5551 From(VectorRGBA color)
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
        public readonly partial struct BGRA4444 : IFactory<BGRA4444>
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
            public BGRA4444 From(BGRA32 color) { return new BGRA4444(color.R, color.G, color.B, color.A); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            public BGRA4444 From(VectorRGBA color)
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
        public readonly partial struct BGRA32 : IFactory<BGRA32>
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
            public BGRA32 From(BGRA32 color) { return color; }
            public BGRA32 From(VectorRGBA color) { return new BGRA32(color); }

            #endregion
        }

        /// <summary>
        /// RGBA Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct RGBA32 : IFactory<RGBA32>
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
            public RGBA32 From(BGRA32 color) { return new RGBA32(color.R, color.G, color.B, color.A); }
            public RGBA32 From(VectorRGBA color) { return new RGBA32(color); }

            #endregion
        }

        /// <summary>
        /// ARGB Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct ARGB32 : IFactory<ARGB32>
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
            public ARGB32 From(VectorRGBA color) { return new ARGB32(color); }
            public ARGB32 From(BGRA32 color) { return new ARGB32(color.R, color.G, color.B, color.A); }

            #endregion
        }

        /// <summary>
        /// BGR Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct BGR24 : IFactory<BGR24>
        {
            #region constructors

            public BGR24(in VectorRGBA rgba)
            {
                var v = rgba.RGBA * 255f;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;
            }

            public BGR24(Byte red, Byte green, Byte blue) { B = blue; G = green; R = red; }

            #endregion

            #region data

            public readonly Byte B;
            public readonly Byte G;
            public readonly Byte R;

            #endregion

            #region API

            public BGR24 From(BGRA32 color) { return new BGR24(color.R, color.G, color.B); }
            public BGRA32 ToBGRA32() { return new BGRA32(R, G, B); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            public BGR24 From(VectorRGBA color) { return new BGR24(color); }

            #endregion
        }

        /// <summary>
        /// RGB Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct RGB24 : IFactory<RGB24>
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

            #endregion

            #region data

            public readonly Byte R;
            public readonly Byte G;
            public readonly Byte B;

            #endregion

            #region API

            public BGRA32 ToBGRA32() { return new BGRA32(R, G, B); }
            public VectorRGBA ToVectorRGBA() { return new VectorRGBA(this); }
            public RGB24 From(BGRA32 color) { return new RGB24(color.R, color.G, color.B); }
            public RGB24 From(VectorRGBA color) { return new RGB24(color); }

            #endregion
        }

        /// <summary>
        /// BGRA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct VectorBGRA : IFactory<VectorBGRA>
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
            public VectorBGRA From(BGRA32 color) { return new VectorBGRA(color); }
            public VectorBGRA From(VectorRGBA color) { return new VectorBGRA(color.R, color.G, color.B, color.A); }
            #endregion
        }

        /// <summary>
        /// RGBA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public readonly partial struct VectorRGBA : IFactory<VectorRGBA>
        {
            #region constructors
            public VectorRGBA(Alpha8 alpha) { RGBA = new XYZA(0, 0, 0, alpha.A) / 255f; }
            public VectorRGBA(Luminance8 luma) { RGBA = new XYZA(luma.L, luma.L, luma.L, 255) / 255f; }
            public VectorRGBA(Luminance16 luma) { RGBA = new XYZA(luma.L, luma.L, luma.L, 65565) / 65535f; }
            public VectorRGBA(StdLuminance luma) { RGBA = new XYZA(luma.L, luma.L, luma.L, 1); }
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
            public VectorRGBA From(BGRA32 color) { return new VectorRGBA(color); }
            public VectorRGBA From(VectorRGBA color) { return color; }

            #endregion
        }

        /// <summary>
        /// RGBA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public readonly partial struct VectorBGR : IFactory<VectorBGR>
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
            public VectorBGR From(BGRA32 color) { return new VectorBGR(color); }
            public VectorBGR From(VectorRGBA color) { return new VectorBGR(color.R, color.G, color.B); }

            #endregion
        }
    }
}