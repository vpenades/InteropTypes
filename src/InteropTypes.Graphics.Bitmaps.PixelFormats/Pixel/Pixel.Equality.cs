
// GENERATED CODE: using CodeGenUtils.t4
// GENERATED CODE: using Pixel.Constants.t4

using System;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel    
    {
        partial struct Alpha8 : IEquatable<Alpha8>
        {

            /// <inheritdoc/>
            public static bool operator ==(in Alpha8 a,in Alpha8 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in Alpha8 a,in Alpha8 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is Alpha8 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(Alpha8 other)
            {
                return this.A == other.A;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.A.GetHashCode();
            }
        }
        partial struct Luminance8 : IEquatable<Luminance8>
        {

            /// <inheritdoc/>
            public static bool operator ==(in Luminance8 a,in Luminance8 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in Luminance8 a,in Luminance8 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is Luminance8 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(Luminance8 other)
            {
                return this.L == other.L;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.L.GetHashCode();
            }
        }
        partial struct Luminance16 : IEquatable<Luminance16>
        {

            /// <inheritdoc/>
            public static bool operator ==(in Luminance16 a,in Luminance16 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in Luminance16 a,in Luminance16 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is Luminance16 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(Luminance16 other)
            {
                return this.L == other.L;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.L.GetHashCode();
            }
        }
        partial struct Luminance32F : IEquatable<Luminance32F>
        {

            /// <inheritdoc/>
            public static bool operator ==(in Luminance32F a,in Luminance32F b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in Luminance32F a,in Luminance32F b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is Luminance32F other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(Luminance32F other)
            {
                return this.L == other.L;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.L.GetHashCode();
            }
        }
        partial struct BGR565 : IEquatable<BGR565>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGR565 a,in BGR565 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGR565 a,in BGR565 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGR565 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGR565 other)
            {
                return this.BGR == other.BGR;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.BGR.GetHashCode();
            }
        }
        partial struct BGRA5551 : IEquatable<BGRA5551>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGRA5551 a,in BGRA5551 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGRA5551 a,in BGRA5551 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGRA5551 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGRA5551 other)
            {
                if (this.A == 0 && other.A == 0) return true;
                return this.BGRA == other.BGRA;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                if (this.A == 0) return 0;
                return this.BGRA.GetHashCode();
            }
        }
        partial struct BGRA4444 : IEquatable<BGRA4444>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGRA4444 a,in BGRA4444 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGRA4444 a,in BGRA4444 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGRA4444 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGRA4444 other)
            {
                if (this.A == 0 && other.A == 0) return true;
                return this.BGRA == other.BGRA;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                if (this.A == 0) return 0;
                return this.BGRA.GetHashCode();
            }
        }
        partial struct BGR24 : IEquatable<BGR24>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGR24 a,in BGR24 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGR24 a,in BGR24 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGR24 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGR24 other)
            {
                if (this.B != other.B) return false;
                if (this.G != other.G) return false;
                if (this.R != other.R) return false;
                return true;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                int h = 0;
                h ^= this.B; h <<= 8;
                h ^= this.G; h <<= 8;
                h ^= this.R; h <<= 8;
                return h;
            }
        }
        partial struct RGB24 : IEquatable<RGB24>
        {

            /// <inheritdoc/>
            public static bool operator ==(in RGB24 a,in RGB24 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in RGB24 a,in RGB24 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is RGB24 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(RGB24 other)
            {
                if (this.R != other.R) return false;
                if (this.G != other.G) return false;
                if (this.B != other.B) return false;
                return true;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                int h = 0;
                h ^= this.R; h <<= 8;
                h ^= this.G; h <<= 8;
                h ^= this.B; h <<= 8;
                return h;
            }
        }
        partial struct BGRA32 : IEquatable<BGRA32>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGRA32 a,in BGRA32 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGRA32 a,in BGRA32 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGRA32 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGRA32 other)
            {
                if (this.A == 0 && other.A == 0) return true;
                return this.BGRA == other.BGRA;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                if (this.A == 0) return 0;
                return this.BGRA.GetHashCode();
            }
        }
        partial struct RGBA32 : IEquatable<RGBA32>
        {

            /// <inheritdoc/>
            public static bool operator ==(in RGBA32 a,in RGBA32 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in RGBA32 a,in RGBA32 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is RGBA32 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(RGBA32 other)
            {
                if (this.A == 0 && other.A == 0) return true;
                return this.RGBA == other.RGBA;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                if (this.A == 0) return 0;
                return this.RGBA.GetHashCode();
            }
        }
        partial struct ARGB32 : IEquatable<ARGB32>
        {

            /// <inheritdoc/>
            public static bool operator ==(in ARGB32 a,in ARGB32 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in ARGB32 a,in ARGB32 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is ARGB32 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(ARGB32 other)
            {
                if (this.A == 0 && other.A == 0) return true;
                return this.ARGB == other.ARGB;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                if (this.A == 0) return 0;
                return this.ARGB.GetHashCode();
            }
        }
        partial struct RGBP32 : IEquatable<RGBP32>
        {

            /// <inheritdoc/>
            public static bool operator ==(in RGBP32 a,in RGBP32 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in RGBP32 a,in RGBP32 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is RGBP32 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(RGBP32 other)
            {
                return this.RGBP == other.RGBP;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.RGBP.GetHashCode();
            }
        }
        partial struct BGRP32 : IEquatable<BGRP32>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGRP32 a,in BGRP32 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGRP32 a,in BGRP32 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGRP32 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGRP32 other)
            {
                return this.BGRP == other.BGRP;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.BGRP.GetHashCode();
            }
        }
        partial struct RGB96F : IEquatable<RGB96F>
        {

            /// <inheritdoc/>
            public static bool operator ==(in RGB96F a,in RGB96F b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in RGB96F a,in RGB96F b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is RGB96F other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(RGB96F other)
            {
                return this.RGB == other.RGB;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.RGB.GetHashCode();
            }
        }
        partial struct BGR96F : IEquatable<BGR96F>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGR96F a,in BGR96F b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGR96F a,in BGR96F b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGR96F other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGR96F other)
            {
                return this.BGR == other.BGR;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.BGR.GetHashCode();
            }
        }
        partial struct BGRA128F : IEquatable<BGRA128F>
        {

            /// <inheritdoc/>
            public static bool operator ==(in BGRA128F a,in BGRA128F b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in BGRA128F a,in BGRA128F b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is BGRA128F other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(BGRA128F other)
            {
                if (this.A == 0 && other.A == 0) return true;
                return this.BGRA == other.BGRA;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                if (this.A == 0) return 0;
                return this.BGRA.GetHashCode();
            }
        }
        partial struct RGBA128F : IEquatable<RGBA128F>
        {

            /// <inheritdoc/>
            public static bool operator ==(in RGBA128F a,in RGBA128F b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in RGBA128F a,in RGBA128F b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is RGBA128F other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(RGBA128F other)
            {
                if (this.A == 0 && other.A == 0) return true;
                return this.RGBA == other.RGBA;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                if (this.A == 0) return 0;
                return this.RGBA.GetHashCode();
            }
        }
        partial struct RGBP128F : IEquatable<RGBP128F>
        {

            /// <inheritdoc/>
            public static bool operator ==(in RGBP128F a,in RGBP128F b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in RGBP128F a,in RGBP128F b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is RGBP128F other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(RGBP128F other)
            {
                return this.RGBP == other.RGBP;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return this.RGBP.GetHashCode();
            }
        }
    }
}

