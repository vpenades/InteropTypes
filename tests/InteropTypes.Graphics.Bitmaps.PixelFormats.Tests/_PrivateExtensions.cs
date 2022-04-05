using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
