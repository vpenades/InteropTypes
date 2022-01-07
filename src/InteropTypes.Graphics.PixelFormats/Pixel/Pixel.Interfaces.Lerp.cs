
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InteropBitmaps
{
    partial class Pixel    
    {
        

        partial struct Alpha8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<Alpha8> left, ReadOnlySpan<Alpha8> right, int amount, Span<Alpha8> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct Luminance8
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<Luminance8> left, ReadOnlySpan<Luminance8> right, int amount, Span<Luminance8> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct BGR24
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGR24> left, ReadOnlySpan<BGR24> right, int amount, Span<BGR24> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct RGB24
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGB24> left, ReadOnlySpan<RGB24> right, int amount, Span<RGB24> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct RGBP32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGBP32> left, ReadOnlySpan<RGBP32> right, int amount, Span<RGBP32> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct BGRP32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGRP32> left, ReadOnlySpan<BGRP32> right, int amount, Span<BGRP32> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }
        }
        partial struct RGB96F
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGB96F> left, ReadOnlySpan<RGB96F> right, int amount, Span<RGB96F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }
        }
        partial struct BGR96F
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<BGR96F> left, ReadOnlySpan<BGR96F> right, int amount, Span<BGR96F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }
        }
        partial struct RGBP128F
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Lerp(ReadOnlySpan<RGBP128F> left, ReadOnlySpan<RGBP128F> right, int amount, Span<RGBP128F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }
        }

    }
}
