
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InteropBitmaps
{
    partial class Pixel    
    {        

        partial struct Alpha8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Alpha8 Lerp(Alpha8 left, Alpha8 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var A = (left.A * lx + right.A * rx) / 16384;
                return new Alpha8(A);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<Alpha8> left, ReadOnlySpan<Alpha8> right, float amount, Span<Alpha8> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<Alpha8> left, ReadOnlySpan<Alpha8> right, int amount, Span<Alpha8> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct Luminance8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Luminance8 Lerp(Luminance8 left, Luminance8 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var L = (left.L * lx + right.L * rx) / 16384;
                return new Luminance8(L);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<Luminance8> left, ReadOnlySpan<Luminance8> right, float amount, Span<Luminance8> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<Luminance8> left, ReadOnlySpan<Luminance8> right, int amount, Span<Luminance8> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct BGR24
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGR24 Lerp(BGR24 left, BGR24 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var R = (left.R * lx + right.R * rx) / 16384;
                var G = (left.G * lx + right.G * rx) / 16384;
                var B = (left.B * lx + right.B * rx) / 16384;
                return new BGR24(R, G, B);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static TDst Lerp<TDst>(BGR24 left, BGR24 right, int rx)
                where TDst: unmanaged, IPixelFactory<BGRA32, TDst>
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var R = (left.R * lx + right.R * rx) / 16384;
                var G = (left.G * lx + right.G * rx) / 16384;
                var B = (left.B * lx + right.B * rx) / 16384;
                return default(TDst).From( new BGRA32(R, G, B));
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGR24> left, ReadOnlySpan<BGR24> right, float amount, Span<BGR24> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGR24> left, ReadOnlySpan<BGR24> right, int amount, Span<BGR24> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct RGB24
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static RGB24 Lerp(RGB24 left, RGB24 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var R = (left.R * lx + right.R * rx) / 16384;
                var G = (left.G * lx + right.G * rx) / 16384;
                var B = (left.B * lx + right.B * rx) / 16384;
                return new RGB24(R, G, B);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGB24> left, ReadOnlySpan<RGB24> right, float amount, Span<RGB24> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGB24> left, ReadOnlySpan<RGB24> right, int amount, Span<RGB24> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct BGRA5551
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRA5551 Lerp(BGRA5551 p00, BGRA5551 p01, int rx)
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
                int r = (p00.R * lx + p01.R * rx) / 16384;
                int g = (p00.G * lx + p01.G * rx) / 16384;
                int b = (p00.B * lx + p01.B * rx) / 16384;

                // unpremultiply RGB
                return new BGRA5551(r / a, g / a, b / a, a);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRA5551> left, ReadOnlySpan<BGRA5551> right, float amount, Span<BGRA5551> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRA5551> left, ReadOnlySpan<BGRA5551> right, int amount, Span<BGRA5551> dst)
            {
                ref var lPtr = ref MemoryMarshal.GetReference(left);
                ref var rPtr = ref MemoryMarshal.GetReference(right);
                ref var dPtr = ref MemoryMarshal.GetReference(dst);
                var len = dst.Length;

                // old school loop
                while(len-- > 0)
                {
                    dPtr = Lerp(lPtr,rPtr,amount);
                    dPtr = Unsafe.Add(ref dPtr,1);
                    lPtr = Unsafe.Add(ref lPtr,1);
                    rPtr = Unsafe.Add(ref rPtr,1);
                }
            }
        }
        partial struct BGRA4444
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRA4444 Lerp(BGRA4444 p00, BGRA4444 p01, int rx)
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
                int r = (p00.R * lx + p01.R * rx) / 16384;
                int g = (p00.G * lx + p01.G * rx) / 16384;
                int b = (p00.B * lx + p01.B * rx) / 16384;

                // unpremultiply RGB
                return new BGRA4444(r / a, g / a, b / a, a);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRA4444> left, ReadOnlySpan<BGRA4444> right, float amount, Span<BGRA4444> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRA4444> left, ReadOnlySpan<BGRA4444> right, int amount, Span<BGRA4444> dst)
            {
                ref var lPtr = ref MemoryMarshal.GetReference(left);
                ref var rPtr = ref MemoryMarshal.GetReference(right);
                ref var dPtr = ref MemoryMarshal.GetReference(dst);
                var len = dst.Length;

                // old school loop
                while(len-- > 0)
                {
                    dPtr = Lerp(lPtr,rPtr,amount);
                    dPtr = Unsafe.Add(ref dPtr,1);
                    lPtr = Unsafe.Add(ref lPtr,1);
                    rPtr = Unsafe.Add(ref rPtr,1);
                }
            }
        }
        partial struct RGBA32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static RGBA32 Lerp(RGBA32 p00, RGBA32 p01, int rx)
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
                int r = (p00.R * lx + p01.R * rx) / 16384;
                int g = (p00.G * lx + p01.G * rx) / 16384;
                int b = (p00.B * lx + p01.B * rx) / 16384;

                // unpremultiply RGB
                return new RGBA32(r / a, g / a, b / a, a);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGBA32> left, ReadOnlySpan<RGBA32> right, float amount, Span<RGBA32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGBA32> left, ReadOnlySpan<RGBA32> right, int amount, Span<RGBA32> dst)
            {
                ref var lPtr = ref MemoryMarshal.GetReference(left);
                ref var rPtr = ref MemoryMarshal.GetReference(right);
                ref var dPtr = ref MemoryMarshal.GetReference(dst);
                var len = dst.Length;

                // old school loop
                while(len-- > 0)
                {
                    dPtr = Lerp(lPtr,rPtr,amount);
                    dPtr = Unsafe.Add(ref dPtr,1);
                    lPtr = Unsafe.Add(ref lPtr,1);
                    rPtr = Unsafe.Add(ref rPtr,1);
                }
            }
        }
        partial struct BGRA32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRA32 Lerp(BGRA32 p00, BGRA32 p01, int rx)
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
                int r = (p00.R * lx + p01.R * rx) / 16384;
                int g = (p00.G * lx + p01.G * rx) / 16384;
                int b = (p00.B * lx + p01.B * rx) / 16384;

                // unpremultiply RGB
                return new BGRA32(r / a, g / a, b / a, a);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRA32> left, ReadOnlySpan<BGRA32> right, float amount, Span<BGRA32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRA32> left, ReadOnlySpan<BGRA32> right, int amount, Span<BGRA32> dst)
            {
                ref var lPtr = ref MemoryMarshal.GetReference(left);
                ref var rPtr = ref MemoryMarshal.GetReference(right);
                ref var dPtr = ref MemoryMarshal.GetReference(dst);
                var len = dst.Length;

                // old school loop
                while(len-- > 0)
                {
                    dPtr = Lerp(lPtr,rPtr,amount);
                    dPtr = Unsafe.Add(ref dPtr,1);
                    lPtr = Unsafe.Add(ref lPtr,1);
                    rPtr = Unsafe.Add(ref rPtr,1);
                }
            }
        }
        partial struct ARGB32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ARGB32 Lerp(ARGB32 p00, ARGB32 p01, int rx)
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
                int r = (p00.R * lx + p01.R * rx) / 16384;
                int g = (p00.G * lx + p01.G * rx) / 16384;
                int b = (p00.B * lx + p01.B * rx) / 16384;

                // unpremultiply RGB
                return new ARGB32(r / a, g / a, b / a, a);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<ARGB32> left, ReadOnlySpan<ARGB32> right, float amount, Span<ARGB32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<ARGB32> left, ReadOnlySpan<ARGB32> right, int amount, Span<ARGB32> dst)
            {
                ref var lPtr = ref MemoryMarshal.GetReference(left);
                ref var rPtr = ref MemoryMarshal.GetReference(right);
                ref var dPtr = ref MemoryMarshal.GetReference(dst);
                var len = dst.Length;

                // old school loop
                while(len-- > 0)
                {
                    dPtr = Lerp(lPtr,rPtr,amount);
                    dPtr = Unsafe.Add(ref dPtr,1);
                    lPtr = Unsafe.Add(ref lPtr,1);
                    rPtr = Unsafe.Add(ref rPtr,1);
                }
            }
        }
        partial struct RGBP32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static RGBP32 Lerp(RGBP32 left, RGBP32 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var PreR = (left.PreR * lx + right.PreR * rx) / 16384;
                var PreG = (left.PreG * lx + right.PreG * rx) / 16384;
                var PreB = (left.PreB * lx + right.PreB * rx) / 16384;
                var A = (left.A * lx + right.A * rx) / 16384;
                return new RGBP32(PreR, PreG, PreB, A);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGBP32> left, ReadOnlySpan<RGBP32> right, float amount, Span<RGBP32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGBP32> left, ReadOnlySpan<RGBP32> right, int amount, Span<RGBP32> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct BGRP32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BGRP32 Lerp(BGRP32 left, BGRP32 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var PreR = (left.PreR * lx + right.PreR * rx) / 16384;
                var PreG = (left.PreG * lx + right.PreG * rx) / 16384;
                var PreB = (left.PreB * lx + right.PreB * rx) / 16384;
                var A = (left.A * lx + right.A * rx) / 16384;
                return new BGRP32(PreR, PreG, PreB, A);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRP32> left, ReadOnlySpan<BGRP32> right, float amount, Span<BGRP32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRP32> left, ReadOnlySpan<BGRP32> right, int amount, Span<BGRP32> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct RGB96F
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGB96F> left, ReadOnlySpan<RGB96F> right, float amount, Span<RGB96F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }
        }
        partial struct BGR96F
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGR96F> left, ReadOnlySpan<BGR96F> right, float amount, Span<BGR96F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }
        }
        partial struct RGBP128F
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGBP128F> left, ReadOnlySpan<RGBP128F> right, float amount, Span<RGBP128F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }
        }

    }
}
