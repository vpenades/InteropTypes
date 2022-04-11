using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using MEMUNSAFE = System.Runtime.CompilerServices.Unsafe;

namespace InteropTypes.Graphics.Bitmaps
{
    internal static class _PrivateExtensions
    {
        public static TPixel GetReferenceBGRP32<TPixel>(this Pixel.BGRP32 pixel)
            where TPixel : unmanaged
        {
            if (typeof(TPixel) == typeof(Pixel.BGR24))
            {
                var pix = pixel.GetReferenceBGR24();
                return MEMUNSAFE.As<Pixel.BGR24, TPixel>(ref pix);
            }

            if (typeof(TPixel) == typeof(Pixel.RGB24))
            {
                var pix = pixel.GetReferenceRGB24();
                return MEMUNSAFE.As<Pixel.RGB24, TPixel>(ref pix);
            }

            if (typeof(TPixel) == typeof(Pixel.BGRA32))
            {
                var pix = pixel.GetReferenceBGRA32();
                return MEMUNSAFE.As<Pixel.BGRA32, TPixel>(ref pix);
            }

            if (typeof(TPixel) == typeof(Pixel.RGBA32))
            {
                var pix = pixel.GetReferenceRGBA32();
                return MEMUNSAFE.As<Pixel.RGBA32, TPixel>(ref pix);
            }

            if (typeof(TPixel) == typeof(Pixel.ARGB32))
            {
                var pix = pixel.GetReferenceARGB32();
                return MEMUNSAFE.As<Pixel.ARGB32, TPixel>(ref pix);
            }

            throw new NotImplementedException(typeof(TPixel).Name);
        }

        public static Pixel.BGRP32 GetReferenceBGRP32<TPixel>(this TPixel pixel)
            where TPixel : unmanaged
        {
            if (typeof(TPixel) == typeof(Pixel.BGRP32))
            {
                return MEMUNSAFE.As<TPixel, Pixel.BGRP32>(ref pixel);
            }

            if (typeof(TPixel) == typeof(Pixel.RGBP32))
            {
                ref var pix = ref MEMUNSAFE.As<TPixel, Pixel.RGBP32>(ref pixel);
                return new Pixel.BGRP32(pix);
            }

            if (typeof(TPixel) == typeof(Pixel.BGR565))
            {
                return MEMUNSAFE.As<TPixel, Pixel.BGR565>(ref pixel).GetReferenceBGRP32();
            }

            if (typeof(TPixel) == typeof(Pixel.BGR24))
            {
                return MEMUNSAFE.As<TPixel, Pixel.BGR24>(ref pixel).GetReferenceBGRP32();
            }

            if (typeof(TPixel) == typeof(Pixel.RGB24))
            {
                return MEMUNSAFE.As<TPixel, Pixel.RGB24>(ref pixel).GetReferenceBGRP32();
            }

            if (typeof(TPixel) == typeof(Pixel.BGRA32))
            {
                return MEMUNSAFE.As<TPixel, Pixel.BGRA32>(ref pixel).GetReferenceBGRP32();
            }

            if (typeof(TPixel) == typeof(Pixel.RGBA32))
            {
                return MEMUNSAFE.As<TPixel, Pixel.RGBA32>(ref pixel).GetReferenceBGRP32();
            }

            if (typeof(TPixel) == typeof(Pixel.ARGB32))
            {
                return MEMUNSAFE.As<TPixel, Pixel.ARGB32>(ref pixel).GetReferenceBGRP32();
            }

            throw new NotImplementedException(typeof(TPixel).Name);
        }

        public static Pixel.BGRA32 GetReferenceBGRP32(this Pixel.BGRP32 premul)
        {
            return new Pixel.BGRA32(premul.R, premul.G, premul.B, premul.A);
        }

        public static Pixel.BGRP32 GetReferenceBGRP32(this Pixel.BGR565 pixel)
        {
            return new Pixel.BGRP32(pixel.R, pixel.G, pixel.B, (Byte)255);
        }

        public static Pixel.BGRP32 GetReferenceBGRP32(this Pixel.BGR24 pixel)
        {
            return new Pixel.BGRP32(pixel.R, pixel.G, pixel.B, (Byte)255);
        }

        public static Pixel.BGRP32 GetReferenceBGRP32(this Pixel.RGB24 pixel)
        {
            return new Pixel.BGRP32(pixel.R, pixel.G, pixel.B, (Byte)255);
        }

        public static Pixel.BGRP32 GetReferenceBGRP32(this Pixel.BGRA32 pixel)
        {
            uint preR = ((uint)pixel.R * (uint)pixel.A) / 255u;
            uint preG = ((uint)pixel.G * (uint)pixel.A) / 255u;
            uint preB = ((uint)pixel.B * (uint)pixel.A) / 255u;

            return new Pixel.BGRP32((Byte)preR, (Byte)preG, (Byte)preB, pixel.A);
        }

        public static Pixel.BGRP32 GetReferenceBGRP32(this Pixel.RGBA32 pixel)
        {
            uint preR = ((uint)pixel.R * (uint)pixel.A) / 255u;
            uint preG = ((uint)pixel.G * (uint)pixel.A) / 255u;
            uint preB = ((uint)pixel.B * (uint)pixel.A) / 255u;

            return new Pixel.BGRP32((Byte)preR, (Byte)preG, (Byte)preB, pixel.A);
        }

        public static Pixel.BGRP32 GetReferenceBGRP32(this Pixel.ARGB32 pixel)
        {
            uint preR = ((uint)pixel.R * (uint)pixel.A) / 255u;
            uint preG = ((uint)pixel.G * (uint)pixel.A) / 255u;
            uint preB = ((uint)pixel.B * (uint)pixel.A) / 255u;

            return new Pixel.BGRP32((Byte)preR, (Byte)preG, (Byte)preB, pixel.A);
        }

        public static Pixel.BGRA32 GetReferenceBGRA32(this Pixel.BGRP32 pixel)
        {
            if (pixel.A == 0) return default;
            uint r = ((uint)pixel.PreR * 255u) / (uint)pixel.A;
            uint g = ((uint)pixel.PreG * 255u) / (uint)pixel.A;
            uint b = ((uint)pixel.PreB * 255u) / (uint)pixel.A;

            return new Pixel.BGRA32((Byte)r, (Byte)g, (Byte)b, pixel.A);
        }

        public static Pixel.RGBA32 GetReferenceRGBA32(this Pixel.BGRP32 pixel)
        {
            if (pixel.A == 0) return default;
            uint r = ((uint)pixel.PreR * 255u) / (uint)pixel.A;
            uint g = ((uint)pixel.PreG * 255u) / (uint)pixel.A;
            uint b = ((uint)pixel.PreB * 255u) / (uint)pixel.A;

            return new Pixel.RGBA32((Byte)r, (Byte)g, (Byte)b, pixel.A);
        }

        public static Pixel.ARGB32 GetReferenceARGB32(this Pixel.BGRP32 pixel)
        {
            if (pixel.A == 0) return default;
            uint r = ((uint)pixel.PreR * 255u) / (uint)pixel.A;
            uint g = ((uint)pixel.PreG * 255u) / (uint)pixel.A;
            uint b = ((uint)pixel.PreB * 255u) / (uint)pixel.A;

            return new Pixel.ARGB32((Byte)r, (Byte)g, (Byte)b, pixel.A);
        }

        public static Pixel.BGR24 GetReferenceBGR24(this Pixel.BGRP32 pixel)
        {
            if (pixel.A == 0) return default;
            uint r = ((uint)pixel.PreR * 255u) / (uint)pixel.A;
            uint g = ((uint)pixel.PreG * 255u) / (uint)pixel.A;
            uint b = ((uint)pixel.PreB * 255u) / (uint)pixel.A;

            return new Pixel.BGR24((Byte)r, (Byte)g, (Byte)b);
        }

        public static Pixel.RGB24 GetReferenceRGB24(this Pixel.BGRP32 pixel)
        {
            if (pixel.A == 0) return default;
            uint r = ((uint)pixel.PreR * 255u) / (uint)pixel.A;
            uint g = ((uint)pixel.PreG * 255u) / (uint)pixel.A;
            uint b = ((uint)pixel.PreB * 255u) / (uint)pixel.A;

            return new Pixel.RGB24((Byte)r, (Byte)g, (Byte)b);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void AssertEqualsWithPremul(this Pixel.BGRA32 a, Pixel.BGRP32 b)
        {
            // Comparing Premultiplied values is tricky. Because As Alpha become smaller,
            // the precission loss in the RGB components increases.

            if (b.A == 255)
            {
                var bb = new Pixel.BGRA32(b);
                Assert.AreEqual(a, bb);
                return;
            }

            if (b.PreR > b.A) throw new ArgumentOutOfRangeException(nameof(b));
            if (b.PreG > b.A) throw new ArgumentOutOfRangeException(nameof(b));
            if (b.PreB > b.A) throw new ArgumentOutOfRangeException(nameof(b));

            Assert.AreEqual(a.A, b.A, "Alpha");            
            if (a.A == 0) return;

            // premultiply usually rounds down the RGB components.
            // roundtrip:
            var art = new Pixel.BGRA32(new Pixel.BGRP32(a));
            var brt = new Pixel.BGRA32(b);

            var errorR = a.R - art.R;
            var errorG = a.G - art.G;
            var errorB = a.B - art.B;

            // valid values must be between the rounded down values and the true values.

            Assert.GreaterOrEqual(brt.R, art.R, "Red");
            Assert.LessOrEqual(brt.R, a.R + errorR, "Red");

            Assert.GreaterOrEqual(brt.G, art.G, "Green");
            Assert.LessOrEqual(brt.G, a.G + errorG, "Green");

            Assert.GreaterOrEqual(brt.B, art.B, "Blue");
            Assert.LessOrEqual(brt.B, a.B + errorB, "Blue");
        }

        public static bool EqualsWithPremul(this Pixel.BGRA32 a, Pixel.BGRP32 b)
        {
            if (b.PreR > b.A) throw new ArgumentOutOfRangeException(nameof(b));
            if (b.PreG > b.A) throw new ArgumentOutOfRangeException(nameof(b));
            if (b.PreB > b.A) throw new ArgumentOutOfRangeException(nameof(b));

            if (a.A != b.A) return false;
            if (a.A == 0) return true;

            // premultiply usually rounds down the RGB components.
            // roundtrip:
            var rt = new Pixel.BGRA32(new Pixel.BGRP32(a));

            // valid values must be between the rounded down values and the true values.
            if (b.R < rt.R || b.R > a.R) return false;
            if (b.G < rt.G || b.G > a.G) return false;
            if (b.B < rt.B || b.B > a.B) return false;
            return true;            
        }


        public static bool EqualsWithPremul(this Pixel.BGRA32 a, Pixel.BGRA32 b)
        {
            return EqualsWithPremul(new Pixel.BGRP32(a), new Pixel.BGRP32(b));
        }


        public static bool EqualsWithPremul(this Pixel.BGRP32 a, Pixel.BGRP32 b)
        {
            if (a.A != b.A) return false;
            if (a.A == 0) return true;            

            var error = 256 / a.A;            

            var aa = a.GetReferenceRGBA32();
            var bb = b.GetReferenceRGBA32();

            var xr = Math.Abs((int)aa.R - (int)bb.R);
            var xg = Math.Abs((int)aa.G - (int)bb.G);
            var xb = Math.Abs((int)aa.B - (int)bb.B);

            if (xr > error) return false;
            if (xg > error) return false;
            if (xb > error) return false;

            return true;
        }



        public static Pixel.BGRA32 PremulRoundtrip(this Pixel.BGRA32 pixel)
        {
            var premul = new Pixel.BGRP32(pixel);
            return new Pixel.BGRA32(premul);
        }

        public static TPixel PremulRoundtrip8<TPixel>(this TPixel pixel)
            where TPixel : unmanaged
            , Pixel.IConvertTo
            , Pixel.IValueSetter<Pixel.BGRP32>
        {
            var premul = pixel.To<Pixel.BGRP32>();
            pixel.SetValue(premul);
            return pixel;
        }


        public static bool _ComparePixelsAfterPremul<TSrcPixel,TDstPixel>(this TSrcPixel a, TDstPixel b)
            where TSrcPixel: unmanaged, Pixel.IConvertTo
            where TDstPixel: unmanaged, Pixel.IConvertTo, IEquatable<TDstPixel>
        {
            if (typeof(TSrcPixel) == typeof(TDstPixel))
            {
                return a.To<TDstPixel>().Equals(b);
            }

            var aInfo = a as Pixel.IReflection;
            var bInfo = b as Pixel.IReflection;

            if (aInfo.IsPremultiplied || bInfo.IsPremultiplied)
            {
                

                var aPremul = a.To<Pixel.BGRP32>();
                var bPremul = b.To<Pixel.BGRP32>();
                return aPremul == bPremul;
            }
            else
            {
                return false;
            }
        }

    }
}
