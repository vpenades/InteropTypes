using System;
using System.Collections.Generic;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        /// <summary>
        /// Alpha (Transparency) Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{A}")]
        public partial struct Alpha8 
        {
            #region constructors

            public static implicit operator Alpha8(Byte a) { return new Alpha8(a); }

            public static implicit operator Alpha8(int a) { return new Alpha8(a); }
            public Alpha8(BGRA32 color) { A = color.A; }
            public Alpha8(RGBA128F color) { A = (Byte)(color.A / 255f); }
            public Alpha8(Byte alpha) { A = alpha; }
            public Alpha8(int alpha) { A = (Byte)alpha; }

            #endregion

            #region data

            public Byte A;

            #endregion
        }

        /// <summary>
        /// Luminance (Gray) Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]        
        public partial struct Luminance8
        {
            #region constants

            const uint RLuminanceWeight16 = 19562;
            const uint GLuminanceWeight16 = 38550;
            const uint BLuminanceWeight16 = 7424;

            const float RLuminanceWeightF = 0.2989f;
            const float GLuminanceWeightF = 0.5870f;
            const float BLuminanceWeightF = 0.1140f;

            #endregion

            #region constructors

            public Luminance8(Byte luminance) { L = luminance; }

            public Luminance8(int luminance) { L = (Byte)luminance; }
            public Luminance8(BGRA32 color)
            {
                uint accum = 0;

                accum += RLuminanceWeight16 * (uint)color.R;
                accum += GLuminanceWeight16 * (uint)color.G;
                accum += BLuminanceWeight16 * (uint)color.B;

                accum >>= 16;

                L  =(Byte)accum;
            }

            public Luminance8(RGBA128F color)
            {
                float accum = color.R * RLuminanceWeightF + color.G * GLuminanceWeightF + color.B * BLuminanceWeightF;
                accum *= 255f;

                L = (Byte)accum;
            }

            internal static Byte _FromRGB(uint r, uint g, uint b)
            {
                uint accum = 0;

                accum += RLuminanceWeight16 * r;
                accum += GLuminanceWeight16 * g;
                accum += BLuminanceWeight16 * b;

                accum >>= 16;

                return (Byte)accum;
            }

            internal static Byte _FromRGB(float r, float g, float b)
            {
                return (Byte)(Luminance32F._FromRGB(r, g, b) * 255f);
            }

            #endregion

            #region data

            public Byte L;

            #endregion
        }

        /// <summary>
        /// Luminance (Gray) Quantized to 16 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public partial struct Luminance16
        {
            #region constants            

            const uint RLuminanceWeight16 = 19562;
            const uint GLuminanceWeight16 = 38550;
            const uint BLuminanceWeight16 = 7424;

            const float RLuminanceWeightF = 0.2989f;
            const float GLuminanceWeightF = 0.5870f;
            const float BLuminanceWeightF = 0.1140f;

            #endregion

            #region constructors
            public Luminance16(UInt16 luminance) { L = luminance; }

            public Luminance16(BGRA32 color)
            {
                uint accum = 0;

                accum += RLuminanceWeight16 * (uint)color.R;
                accum += GLuminanceWeight16 * (uint)color.G;
                accum += BLuminanceWeight16 * (uint)color.B;

                accum >>= 8;

                L = (UInt16)accum;
            }

            public Luminance16(RGBA128F color)
            {
                float accum = color.R * RLuminanceWeightF + color.G * GLuminanceWeightF + color.B * BLuminanceWeightF;
                accum *= 65535f;

                L = (UInt16)accum;
            }

            #endregion

            #region data

            public UInt16 L;

            #endregion            
        }

        /// <summary>
        /// Luminance (Gray) in floating point range 0-1.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{L}")]
        public partial struct Luminance32F
        {
            #region constants                        

            const float RLuminanceWeightF = 0.2989f;
            const float GLuminanceWeightF = 0.5870f;
            const float BLuminanceWeightF = 0.1140f;

            #endregion

            #region constructors
            public Luminance32F(UInt16 luminance) { L = luminance; }

            public Luminance32F(BGRA32 color)
            {
                float accum = color.R * RLuminanceWeightF + color.G * GLuminanceWeightF + color.B * BLuminanceWeightF;                

                L = accum / 255f;
            }

            public Luminance32F(RGBA128F color)
            {
                L = color.R * RLuminanceWeightF + color.G * GLuminanceWeightF + color.B * BLuminanceWeightF;
            }

            internal static float _FromRGB(float r, float g, float b)
            {
                float accum = 0;

                accum += RLuminanceWeightF * r;
                accum += GLuminanceWeightF * g;
                accum += BLuminanceWeightF * b;                

                return accum;
            }

            #endregion

            #region data

            public float L;

            #endregion

            #region API

            public void SetFromRGB(float r, float g, float b)
            {
                L = 0;

                L += RLuminanceWeightF * r;
                L += GLuminanceWeightF * g;
                L += BLuminanceWeightF * b;                
            }

            #endregion
        }

        /// <summary>
        /// Standard BGR Quantized to 5,6,5 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public partial struct BGR565
        {
            #region constructors

            public BGR565(BGRA32 color) : this(_PackRGB(color.R, color.G, color.B)) { }

            public BGR565(in RGBA128F color) : this(_PackRGB(color.RGBA)) { }

            public BGR565(int red, int green, int blue) : this(_PackRGB((uint)red, (uint)green, (uint)blue)) { }

            private static UInt16 _PackRGB(XYZA rgba)
            {
                rgba *= 255f;
                return _PackRGB((uint)rgba.X, (uint)rgba.Y, (uint)rgba.Z);
            }

            private static UInt16 _PackRGB(uint red, uint green, uint blue)
            {
                uint bgr = red << 8;
                bgr &= 0b1111100000000000;
                bgr |= green << 3;
                bgr &= 0b1111111111100000;
                bgr |= blue >> 3;

                return (UInt16)bgr;
            }

            private BGR565(UInt16 packed) { BGR = packed; }

            #endregion

            #region data

            public UInt16 BGR;

            public int R { get { var p = (BGR >> 11) & 0x1f; return (p * 8) | (p >> 2); } }
            public int G { get { var p = (BGR >> 5) & 0x3f; return (p * 4) | (p >> 4); } }
            public int B { get { var p = BGR & 0x1f; return (p * 8) | (p >> 2); } }

            #endregion

            #region API

            public void SetFromRGB8(uint red, uint green, uint blue)
            {
                System.Diagnostics.Debug.Assert(red < 256);
                System.Diagnostics.Debug.Assert(green < 256);
                System.Diagnostics.Debug.Assert(blue < 256);

                uint bgr = red << 8;
                bgr &= 0b1111100000000000;
                bgr |= green << 3;
                bgr &= 0b1111111111100000;
                bgr |= blue >> 3;

                BGR = (UInt16)bgr;
            }

            #endregion
        }

        /// <summary>
        /// Standard BGRA Quantized to 5,5,5,1 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct BGRA5551
        {
            #region constructors

            const uint _ALPHA_THRESHOLD = 96;

            public BGRA5551(BGRA32 color) : this(_PackRGBA(color.R,color.G,color.B,color.A >= _ALPHA_THRESHOLD)) { }

            public BGRA5551(in RGBA128F color) : this(_PackRGBA(color.RGBA)) { }

            public BGRA5551(int red, int green, int blue, int alpha) : this(red,green,blue, alpha >=128) { }

            public BGRA5551(int red, int green, int blue, Boolean alpha) : this(_PackRGBA(red, green, blue, alpha)) { }

            private static UInt16 _PackRGBA(XYZA rgba)
            {
                rgba *= 255f;
                return _PackRGBA((int)rgba.X, (int)rgba.Y, (int)rgba.Z, rgba.W >= _ALPHA_THRESHOLD);
            }

            private static UInt16 _PackRGBA(int red, int green, int blue, Boolean alpha)
            {
                System.Diagnostics.Debug.Assert(red >= 0 && red < 256);
                System.Diagnostics.Debug.Assert(green >= 0 && green < 256);
                System.Diagnostics.Debug.Assert(blue >= 0 && blue < 256);

                var x = blue >> 3;
                x |= (green >> 3) << 5;
                x |= (red >> 3) << 10;
                x |= alpha ? 0x10000000 : 0;
                return (UInt16)x;
            }

            private BGRA5551(UInt16 packed) { BGRA = packed; }

            #endregion

            #region data

            public UInt16 BGRA;

            public int R { get { var p = (BGRA >> 10) & 0x1f; return p * 8 + (p >> 2); } }
            public int G { get { var p = (BGRA >> 5) & 0x1f; return p * 8 + (p >> 2); } }
            public int B { get { var p = BGRA & 0x1f; return p * 8 + (p >> 2); } }
            public int A => (BGRA >> 15) * 255;            

            #endregion

            #region properties

            public bool IsTransparent => (BGRA & 0x8000) == 0;

            #endregion

            #region API

            public void SetFromRGBA8(uint red, uint green, uint blue, uint alpha)
            {
                System.Diagnostics.Debug.Assert(red < 256);
                System.Diagnostics.Debug.Assert(green < 256);
                System.Diagnostics.Debug.Assert(blue < 256);
                System.Diagnostics.Debug.Assert(alpha < 256);

                var x = blue >> 3;
                x |= (green >> 3) << 5;
                x |= (red >> 3) << 10;
                x |= alpha >= _ALPHA_THRESHOLD ? (uint)0x10000000 : (uint)0;

                BGRA = (UInt16)x;
            }

            #endregion
        }

        /// <summary>
        /// Standard BGRA Quantized to 4,4,4,4 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct BGRA4444
        {
            #region constructors

            public BGRA4444(BGRA32 color) : this(_PackRGBA(color.R,color.G,color.B,color.A)) { }
            public BGRA4444(RGBA128F color) : this(_PackRGBA(color.RGBA)) { }
            public BGRA4444(int red, int green, int blue, int alpha) : this(_PackRGBA(red,green,blue,alpha)) { }

            private static UInt16 _PackRGBA(XYZA rgba)
            {
                rgba *= 255f;
                return _PackRGBA((int)rgba.X, (int)rgba.Y, (int)rgba.Z, (int)rgba.W);
            }

            private static UInt16 _PackRGBA(int red, int green, int blue, int alpha)
            {
                System.Diagnostics.Debug.Assert(red >= 0 && red < 256);
                System.Diagnostics.Debug.Assert(green >= 0 && green < 256);
                System.Diagnostics.Debug.Assert(blue >= 0 && blue < 256);
                System.Diagnostics.Debug.Assert(alpha >= 0 && alpha < 256);

                var x = blue >> 4;
                x |= (green >> 4) << 4;
                x |= (red >> 4) << 8;
                x |= (alpha >> 4) << 12;
                return (UInt16)x;
            }

            private BGRA4444(UInt16 packed) { BGRA = packed; }

            #endregion

            #region data

            public UInt16 BGRA;

            public int A => ((BGRA >> 12) & 0xf) * 17;
            public int R => ((BGRA >> 8) & 0xf) * 17;
            public int G => ((BGRA >> 4) & 0xf) * 17;
            public int B => (BGRA & 0xf) * 17;

            #endregion

            #region properties

            public bool IsTransparent => (BGRA & 0xF000) == 0;

            #endregion
        }

        /// <summary>
        /// Standard BGRA Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct BGRA32
        {
            #region constructors

            public static implicit operator BGRA32(in (int R, int G, int B) color) { return new BGRA32(color.R, color.G, color.B); }
            public static implicit operator BGRA32(in (int R, int G, int B, int A) color) { return new BGRA32(color.R, color.G, color.B, color.A); }

            public BGRA32(in System.Drawing.Color color) { R = color.R; G = color.G;B = color.B;A = color.A; }
            public BGRA32(Alpha8 color) { B = 255; G = 255; R = 255; A = color.A; }
            public BGRA32(Luminance8 color) { B = color.L; G = color.L; R = color.L; A = 255; }
            public BGRA32(Luminance16 color)
            {
                var l = (Byte)(color.L >> 8);
                B = l;
                G = l;
                R = l;
                A = 255;
            }

            public BGRA32(Luminance32F color)
            {
                var l = (Byte)(color.L * 255f);
                B = l;
                G = l;
                R = l;
                A = 255;
            }

            public BGRA32(BGR565 color) { B = (Byte)color.B; G = (Byte)color.G; R = (Byte)color.R; A = 255; }
            public BGRA32(RGB24 color) { B = color.B; G = color.G; R = color.R; A = 255; }
            public BGRA32(BGR24 color) { B = color.B; G = color.G; R = color.R; A = 255; }
            public BGRA32(BGRA5551 color) { B = (Byte)color.B; G = (Byte)color.G; R = (Byte)color.R; A = (Byte)color.A; }
            public BGRA32(BGRA4444 color) { B = (Byte)color.B; G = (Byte)color.G; R = (Byte)color.R; A = (Byte)color.A; }
            public BGRA32(RGBA32 color) { B = color.B; G = color.G; R = color.R; A = color.A; }
            public BGRA32(ARGB32 color) { B = color.B; G = color.G; R = color.R; A = color.A; }

            public BGRA32(in RGBA128F rgba)
            {
                var v = rgba.RGBA * 255f;

                B = (Byte)v.Z;                
                G = (Byte)v.Y;
                R = (Byte)v.X;
                A = (Byte)v.W;
            }

            public BGRA32(in BGRA128F bgra)
            {
                var v = bgra.BGRA * 255f;

                B = (Byte)v.X;
                G = (Byte)v.Y;
                R = (Byte)v.Z;
                A = (Byte)v.W;
            }

            public BGRA32(in RGB96F bgra)
            {
                var v = bgra.RGB * 255f;

                R = (Byte)v.X;
                G = (Byte)v.Y;
                B = (Byte)v.Z;
                A = 255;
            }

            public BGRA32(in BGR96F bgra)
            {
                var v = bgra.BGR * 255f;

                B = (Byte)v.X;
                G = (Byte)v.Y;
                R = (Byte)v.Z;
                A = 255;
            }            

            public BGRA32(int red, int green, int blue, int alpha = 255)
            {
                System.Diagnostics.Debug.Assert(red >= 0 && red < 256);
                System.Diagnostics.Debug.Assert(green >= 0 && green < 256);
                System.Diagnostics.Debug.Assert(blue >= 0 && blue < 256);
                System.Diagnostics.Debug.Assert(alpha >= 0 && alpha < 256);

                B = (byte)blue;
                G = (byte)green;
                R = (byte)red;
                A = (byte)alpha;
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

            public Byte B;
            public Byte G;
            public Byte R;
            public Byte A;            

            #endregion
        }

        /// <summary>
        /// Standard RGBA Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct RGBA32
        {
            #region constructors

            public static implicit operator RGBA32(in (int R, int G, int B) color) { return new RGBA32(color.R, color.G, color.B); }
            public static implicit operator RGBA32(in (int R, int G, int B, int A) color) { return new RGBA32(color.R, color.G, color.B, color.A); }

            public RGBA32(BGRA32 color)
            {
                R = color.R;
                G = color.G;
                B = color.B;
                A = color.A;
            }

            public RGBA32(in RGBA128F rgba)
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

            public Byte R;
            public Byte G;
            public Byte B;
            public Byte A;            

            #endregion
        }

        /// <summary>
        /// Standard ARGB Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct ARGB32
        {
            #region constructors

            public ARGB32(BGRA32 color)
            {
                A = color.A;
                R = color.R;
                G = color.G;
                B = color.B;
            }

            public ARGB32(in RGBA128F rgba)
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

            public Byte A;
            public Byte R;
            public Byte G;
            public Byte B;            

            #endregion
        }

        /// <summary>
        /// Standard BGR Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public partial struct BGR24
        {
            #region constructors

            public static implicit operator BGR24(in (int R, int G, int B) color) { return new BGR24(color.R, color.G, color.B); }

            public BGR24(in RGBA128F rgba)
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

            public Byte B;
            public Byte G;
            public Byte R;            

            #endregion
        }

        /// <summary>
        /// Standard RGB Quantized to 8 bits.
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public partial struct RGB24
        {
            #region constructors

            public static implicit operator RGB24(in (int R, int G, int B) color) { return new RGB24(color.R, color.G, color.B); }

            public RGB24(BGRA32 color)
            {
                R = color.R;
                G = color.G;
                B = color.B;
            }

            public RGB24(in RGBA128F rgba)
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

            public Byte R;
            public Byte G;
            public Byte B;            

            #endregion
        }

        /// <summary>
        /// Standard RGB in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public partial struct RGB96F
        {
            #region constructors
            public RGB96F(in RGBA128F color) : this() { RGB = color.RGB; }
            public RGB96F(Single red, Single green, Single blue) : this() { RGB = new XYZ(red, green, blue); }
            public RGB96F(Byte red, Byte green, Byte blue) : this() { RGB = new XYZ(red, green, blue) / 255f; }
            public RGB96F(BGRA32 color) : this() { RGB = new XYZ(color.R, color.G, color.B) / 255f; }

            public RGB96F(in XYZ rgb) : this() { RGB = rgb; }

            #endregion

            #region data

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZ RGB;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public Single R;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public Single G;

            [System.Runtime.InteropServices.FieldOffset(8)]
            public Single B;

            #endregion
        }

        /// <summary>
        /// Standard RGB in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B}")]
        public partial struct BGR96F
        {
            #region constructors

            public BGR96F(RGBA128F color) : this() { BGR = new XYZ(color.B, color.G, color.R); }
            public BGR96F(BGRA32 color) : this() { BGR = new XYZ(color.B, color.G, color.R) / 255f; }
            public BGR96F(Single red, Single green, Single blue) : this() { BGR = new XYZ(blue, green, red); }
            public BGR96F(Byte red, Byte green, Byte blue) : this() { BGR = new XYZ(blue, green, red) / 255f; }

            #endregion

            #region data

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZ BGR;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public Single B;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public Single G;

            [System.Runtime.InteropServices.FieldOffset(8)]
            public Single R;

            #endregion
        }

        /// <summary>
        /// Standard BGRA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct BGRA128F
        {
            #region constructors
            public BGRA128F(BGRA32 color) : this() { BGRA = new XYZA(color.B, color.G, color.R, color.A) / 255f; }
            public BGRA128F(RGBA128F color) : this() { BGRA = new XYZA(color.B, color.G, color.R, color.A); }
            public BGRA128F(Single red, Single green, Single blue, Single alpha) : this() { BGRA = new XYZA(blue, green, red, alpha); }

            #endregion

            #region data

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZA BGRA;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZ BGR;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public Single B;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public Single G;

            [System.Runtime.InteropServices.FieldOffset(8)]
            public Single R;

            [System.Runtime.InteropServices.FieldOffset(12)]
            public Single A;

            #endregion
        }

        /// <summary>
        /// Standard RGBA in values between 0-1
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        [System.Diagnostics.DebuggerDisplay("{R} {G} {B} {A}")]
        public partial struct RGBA128F
        {
            #region constructors
            public RGBA128F(Alpha8 alpha) : this() { RGBA = new XYZA(255, 255, 255, alpha.A) / 255f; }
            public RGBA128F(Luminance8 luma) : this() { RGBA = new XYZA(luma.L, luma.L, luma.L, 255) / 255f; }
            public RGBA128F(Luminance16 luma) : this() { RGBA = new XYZA(luma.L, luma.L, luma.L, 65565) / 65535f; }
            public RGBA128F(Luminance32F luma) : this() { RGBA = new XYZA(luma.L, luma.L, luma.L, 1); }
            public RGBA128F(BGR565 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, 255) / 255f; }
            public RGBA128F(BGRA5551 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public RGBA128F(BGRA4444 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public RGBA128F(BGR24 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, 255) / 255f; }
            public RGBA128F(RGB24 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, 255) / 255f; }
            public RGBA128F(RGBA32 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public RGBA128F(BGRA32 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }
            public RGBA128F(ARGB32 color) : this() { RGBA = new XYZA(color.R, color.G, color.B, color.A) / 255f; }

            public RGBA128F(Single red, Single green, Single blue, Single alpha) : this() { RGBA = new XYZA(red, green, blue, alpha); }

            public RGBA128F(in RGB96F color) : this() { RGBA = new XYZA(color.RGB, 1); }
            public RGBA128F(in BGR96F color) : this() { RGBA = new XYZA(color.R, color.G, color.B, 1); }
            public RGBA128F(in BGRA128F color) : this() { RGBA = new XYZA(color.R, color.G, color.B, color.A); }
            public RGBA128F(in XYZ v) : this() { RGBA = new XYZA(v, 1); }
            public RGBA128F(in XYZ rgb, float a) : this() { RGBA = new XYZA(rgb, a); }
            public RGBA128F(in XYZA rgba) : this() { RGBA = rgba; }

            #endregion

            #region data

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZA RGBA;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public XYZ RGB;

            [System.Runtime.InteropServices.FieldOffset(0)]
            public Single R;
            
            [System.Runtime.InteropServices.FieldOffset(4)]
            public Single G;

            [System.Runtime.InteropServices.FieldOffset(8)]
            public Single B;

            [System.Runtime.InteropServices.FieldOffset(12)]
            public Single A;            

            #endregion

            #region API
            
            public RGBA128F FromPremul(XYZA premultiplied)
            {
                if (premultiplied.W == 0) return new RGBA128F(0, 0, 0, 0);
                
                var a = premultiplied.W;
                premultiplied /= a;
                premultiplied.W = a;

                return new RGBA128F(premultiplied);                
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
    }
}