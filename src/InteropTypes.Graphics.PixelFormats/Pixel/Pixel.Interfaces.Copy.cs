
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InteropBitmaps
{
    partial class Pixel    
    {
        interface ICopyConverterDelegateProvider<TSrc,TDst>
        {
            CopyConverterCallback<TSrc,TDst> GetCopyConverterDelegate();
        }

        partial struct Alpha8
            : ICopyConverterDelegateProvider<Alpha8, Alpha8>
            , ICopyConverterDelegateProvider<Alpha8, Luminance8>
            , ICopyConverterDelegateProvider<Alpha8, BGR565>
            , ICopyConverterDelegateProvider<Alpha8, BGRA5551>
            , ICopyConverterDelegateProvider<Alpha8, BGRA4444>
            , ICopyConverterDelegateProvider<Alpha8, RGB24>
            , ICopyConverterDelegateProvider<Alpha8, BGR24>
            , ICopyConverterDelegateProvider<Alpha8, RGBA32>
            , ICopyConverterDelegateProvider<Alpha8, BGRA32>
            , ICopyConverterDelegateProvider<Alpha8, ARGB32>
            , ICopyConverterDelegateProvider<Alpha8, BGRP32>
            , ICopyConverterDelegateProvider<Alpha8, RGBP32>
            , ICopyConverterDelegateProvider<Alpha8, Luminance32F>
            , ICopyConverterDelegateProvider<Alpha8, RGB96F>
            , ICopyConverterDelegateProvider<Alpha8, BGR96F>
            , ICopyConverterDelegateProvider<Alpha8, RGBA128F>
            , ICopyConverterDelegateProvider<Alpha8, BGRA128F>
            , ICopyConverterDelegateProvider<Alpha8, RGBP128F>
        {

            CopyConverterCallback<Alpha8,Alpha8> ICopyConverterDelegateProvider<Alpha8,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<Alpha8,Luminance8> ICopyConverterDelegateProvider<Alpha8,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.AsBytes().TryCopyTo(dst.AsBytes());
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<Alpha8,BGR565> ICopyConverterDelegateProvider<Alpha8,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,BGRA5551> ICopyConverterDelegateProvider<Alpha8,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,BGRA4444> ICopyConverterDelegateProvider<Alpha8,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,RGB24> ICopyConverterDelegateProvider<Alpha8,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,BGR24> ICopyConverterDelegateProvider<Alpha8,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,RGBA32> ICopyConverterDelegateProvider<Alpha8,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,BGRA32> ICopyConverterDelegateProvider<Alpha8,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,ARGB32> ICopyConverterDelegateProvider<Alpha8,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 0;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,BGRP32> ICopyConverterDelegateProvider<Alpha8,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,RGBP32> ICopyConverterDelegateProvider<Alpha8,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,Luminance32F> ICopyConverterDelegateProvider<Alpha8,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,RGB96F> ICopyConverterDelegateProvider<Alpha8,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,BGR96F> ICopyConverterDelegateProvider<Alpha8,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,RGBA128F> ICopyConverterDelegateProvider<Alpha8,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,BGRA128F> ICopyConverterDelegateProvider<Alpha8,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Alpha8,RGBP128F> ICopyConverterDelegateProvider<Alpha8,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Alpha8> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct Luminance8
            : ICopyConverterDelegateProvider<Luminance8, Alpha8>
            , ICopyConverterDelegateProvider<Luminance8, Luminance8>
            , ICopyConverterDelegateProvider<Luminance8, BGR565>
            , ICopyConverterDelegateProvider<Luminance8, BGRA5551>
            , ICopyConverterDelegateProvider<Luminance8, BGRA4444>
            , ICopyConverterDelegateProvider<Luminance8, RGB24>
            , ICopyConverterDelegateProvider<Luminance8, BGR24>
            , ICopyConverterDelegateProvider<Luminance8, RGBA32>
            , ICopyConverterDelegateProvider<Luminance8, BGRA32>
            , ICopyConverterDelegateProvider<Luminance8, ARGB32>
            , ICopyConverterDelegateProvider<Luminance8, BGRP32>
            , ICopyConverterDelegateProvider<Luminance8, RGBP32>
            , ICopyConverterDelegateProvider<Luminance8, Luminance32F>
            , ICopyConverterDelegateProvider<Luminance8, RGB96F>
            , ICopyConverterDelegateProvider<Luminance8, BGR96F>
            , ICopyConverterDelegateProvider<Luminance8, RGBA128F>
            , ICopyConverterDelegateProvider<Luminance8, BGRA128F>
            , ICopyConverterDelegateProvider<Luminance8, RGBP128F>
        {

            CopyConverterCallback<Luminance8,Alpha8> ICopyConverterDelegateProvider<Luminance8,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.AsBytes().TryCopyTo(dst.AsBytes());
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<Luminance8,Luminance8> ICopyConverterDelegateProvider<Luminance8,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<Luminance8,BGR565> ICopyConverterDelegateProvider<Luminance8,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,BGRA5551> ICopyConverterDelegateProvider<Luminance8,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,BGRA4444> ICopyConverterDelegateProvider<Luminance8,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,RGB24> ICopyConverterDelegateProvider<Luminance8,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,BGR24> ICopyConverterDelegateProvider<Luminance8,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,RGBA32> ICopyConverterDelegateProvider<Luminance8,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,BGRA32> ICopyConverterDelegateProvider<Luminance8,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,ARGB32> ICopyConverterDelegateProvider<Luminance8,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,BGRP32> ICopyConverterDelegateProvider<Luminance8,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,RGBP32> ICopyConverterDelegateProvider<Luminance8,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,Luminance32F> ICopyConverterDelegateProvider<Luminance8,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }

            CopyConverterCallback<Luminance8,RGB96F> ICopyConverterDelegateProvider<Luminance8,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,BGR96F> ICopyConverterDelegateProvider<Luminance8,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,RGBA128F> ICopyConverterDelegateProvider<Luminance8,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,BGRA128F> ICopyConverterDelegateProvider<Luminance8,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance8,RGBP128F> ICopyConverterDelegateProvider<Luminance8,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance8> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct Luminance16
            : ICopyConverterDelegateProvider<Luminance16, Alpha8>
            , ICopyConverterDelegateProvider<Luminance16, Luminance8>
            , ICopyConverterDelegateProvider<Luminance16, BGR565>
            , ICopyConverterDelegateProvider<Luminance16, BGRA5551>
            , ICopyConverterDelegateProvider<Luminance16, BGRA4444>
            , ICopyConverterDelegateProvider<Luminance16, RGB24>
            , ICopyConverterDelegateProvider<Luminance16, BGR24>
            , ICopyConverterDelegateProvider<Luminance16, RGBA32>
            , ICopyConverterDelegateProvider<Luminance16, BGRA32>
            , ICopyConverterDelegateProvider<Luminance16, ARGB32>
            , ICopyConverterDelegateProvider<Luminance16, BGRP32>
            , ICopyConverterDelegateProvider<Luminance16, RGBP32>
            , ICopyConverterDelegateProvider<Luminance16, Luminance32F>
            , ICopyConverterDelegateProvider<Luminance16, RGB96F>
            , ICopyConverterDelegateProvider<Luminance16, BGR96F>
            , ICopyConverterDelegateProvider<Luminance16, RGBA128F>
            , ICopyConverterDelegateProvider<Luminance16, BGRA128F>
            , ICopyConverterDelegateProvider<Luminance16, RGBP128F>
        {

            CopyConverterCallback<Luminance16,Alpha8> ICopyConverterDelegateProvider<Luminance16,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,Luminance8> ICopyConverterDelegateProvider<Luminance16,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGR565> ICopyConverterDelegateProvider<Luminance16,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGRA5551> ICopyConverterDelegateProvider<Luminance16,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGRA4444> ICopyConverterDelegateProvider<Luminance16,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,RGB24> ICopyConverterDelegateProvider<Luminance16,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGR24> ICopyConverterDelegateProvider<Luminance16,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,RGBA32> ICopyConverterDelegateProvider<Luminance16,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGRA32> ICopyConverterDelegateProvider<Luminance16,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,ARGB32> ICopyConverterDelegateProvider<Luminance16,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGRP32> ICopyConverterDelegateProvider<Luminance16,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,RGBP32> ICopyConverterDelegateProvider<Luminance16,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,Luminance32F> ICopyConverterDelegateProvider<Luminance16,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,RGB96F> ICopyConverterDelegateProvider<Luminance16,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGR96F> ICopyConverterDelegateProvider<Luminance16,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,RGBA128F> ICopyConverterDelegateProvider<Luminance16,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,BGRA128F> ICopyConverterDelegateProvider<Luminance16,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance16,RGBP128F> ICopyConverterDelegateProvider<Luminance16,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance16> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct Luminance32F
            : ICopyConverterDelegateProvider<Luminance32F, Alpha8>
            , ICopyConverterDelegateProvider<Luminance32F, Luminance8>
            , ICopyConverterDelegateProvider<Luminance32F, BGR565>
            , ICopyConverterDelegateProvider<Luminance32F, BGRA5551>
            , ICopyConverterDelegateProvider<Luminance32F, BGRA4444>
            , ICopyConverterDelegateProvider<Luminance32F, RGB24>
            , ICopyConverterDelegateProvider<Luminance32F, BGR24>
            , ICopyConverterDelegateProvider<Luminance32F, RGBA32>
            , ICopyConverterDelegateProvider<Luminance32F, BGRA32>
            , ICopyConverterDelegateProvider<Luminance32F, ARGB32>
            , ICopyConverterDelegateProvider<Luminance32F, BGRP32>
            , ICopyConverterDelegateProvider<Luminance32F, RGBP32>
            , ICopyConverterDelegateProvider<Luminance32F, Luminance32F>
            , ICopyConverterDelegateProvider<Luminance32F, RGB96F>
            , ICopyConverterDelegateProvider<Luminance32F, BGR96F>
            , ICopyConverterDelegateProvider<Luminance32F, RGBA128F>
            , ICopyConverterDelegateProvider<Luminance32F, BGRA128F>
            , ICopyConverterDelegateProvider<Luminance32F, RGBP128F>
        {

            CopyConverterCallback<Luminance32F,Alpha8> ICopyConverterDelegateProvider<Luminance32F,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,Luminance8> ICopyConverterDelegateProvider<Luminance32F,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGR565> ICopyConverterDelegateProvider<Luminance32F,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGRA5551> ICopyConverterDelegateProvider<Luminance32F,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGRA4444> ICopyConverterDelegateProvider<Luminance32F,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,RGB24> ICopyConverterDelegateProvider<Luminance32F,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGR24> ICopyConverterDelegateProvider<Luminance32F,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,RGBA32> ICopyConverterDelegateProvider<Luminance32F,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGRA32> ICopyConverterDelegateProvider<Luminance32F,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,ARGB32> ICopyConverterDelegateProvider<Luminance32F,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGRP32> ICopyConverterDelegateProvider<Luminance32F,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,RGBP32> ICopyConverterDelegateProvider<Luminance32F,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,Luminance32F> ICopyConverterDelegateProvider<Luminance32F,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<Luminance32F,RGB96F> ICopyConverterDelegateProvider<Luminance32F,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGR96F> ICopyConverterDelegateProvider<Luminance32F,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,RGBA128F> ICopyConverterDelegateProvider<Luminance32F,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,BGRA128F> ICopyConverterDelegateProvider<Luminance32F,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<Luminance32F,RGBP128F> ICopyConverterDelegateProvider<Luminance32F,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<Luminance32F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGR565
            : ICopyConverterDelegateProvider<BGR565, Alpha8>
            , ICopyConverterDelegateProvider<BGR565, Luminance8>
            , ICopyConverterDelegateProvider<BGR565, BGR565>
            , ICopyConverterDelegateProvider<BGR565, BGRA5551>
            , ICopyConverterDelegateProvider<BGR565, BGRA4444>
            , ICopyConverterDelegateProvider<BGR565, RGB24>
            , ICopyConverterDelegateProvider<BGR565, BGR24>
            , ICopyConverterDelegateProvider<BGR565, RGBA32>
            , ICopyConverterDelegateProvider<BGR565, BGRA32>
            , ICopyConverterDelegateProvider<BGR565, ARGB32>
            , ICopyConverterDelegateProvider<BGR565, BGRP32>
            , ICopyConverterDelegateProvider<BGR565, RGBP32>
            , ICopyConverterDelegateProvider<BGR565, Luminance32F>
            , ICopyConverterDelegateProvider<BGR565, RGB96F>
            , ICopyConverterDelegateProvider<BGR565, BGR96F>
            , ICopyConverterDelegateProvider<BGR565, RGBA128F>
            , ICopyConverterDelegateProvider<BGR565, BGRA128F>
            , ICopyConverterDelegateProvider<BGR565, RGBP128F>
        {

            CopyConverterCallback<BGR565,Alpha8> ICopyConverterDelegateProvider<BGR565,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,Luminance8> ICopyConverterDelegateProvider<BGR565,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,BGR565> ICopyConverterDelegateProvider<BGR565,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGR565,BGRA5551> ICopyConverterDelegateProvider<BGR565,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,BGRA4444> ICopyConverterDelegateProvider<BGR565,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,RGB24> ICopyConverterDelegateProvider<BGR565,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,BGR24> ICopyConverterDelegateProvider<BGR565,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,RGBA32> ICopyConverterDelegateProvider<BGR565,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,BGRA32> ICopyConverterDelegateProvider<BGR565,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,ARGB32> ICopyConverterDelegateProvider<BGR565,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,BGRP32> ICopyConverterDelegateProvider<BGR565,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,RGBP32> ICopyConverterDelegateProvider<BGR565,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,Luminance32F> ICopyConverterDelegateProvider<BGR565,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,RGB96F> ICopyConverterDelegateProvider<BGR565,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,BGR96F> ICopyConverterDelegateProvider<BGR565,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,RGBA128F> ICopyConverterDelegateProvider<BGR565,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,BGRA128F> ICopyConverterDelegateProvider<BGR565,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR565,RGBP128F> ICopyConverterDelegateProvider<BGR565,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR565> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGR24
            : ICopyConverterDelegateProvider<BGR24, Alpha8>
            , ICopyConverterDelegateProvider<BGR24, Luminance8>
            , ICopyConverterDelegateProvider<BGR24, BGR565>
            , ICopyConverterDelegateProvider<BGR24, BGRA5551>
            , ICopyConverterDelegateProvider<BGR24, BGRA4444>
            , ICopyConverterDelegateProvider<BGR24, RGB24>
            , ICopyConverterDelegateProvider<BGR24, BGR24>
            , ICopyConverterDelegateProvider<BGR24, RGBA32>
            , ICopyConverterDelegateProvider<BGR24, BGRA32>
            , ICopyConverterDelegateProvider<BGR24, ARGB32>
            , ICopyConverterDelegateProvider<BGR24, BGRP32>
            , ICopyConverterDelegateProvider<BGR24, RGBP32>
            , ICopyConverterDelegateProvider<BGR24, Luminance32F>
            , ICopyConverterDelegateProvider<BGR24, RGB96F>
            , ICopyConverterDelegateProvider<BGR24, BGR96F>
            , ICopyConverterDelegateProvider<BGR24, RGBA128F>
            , ICopyConverterDelegateProvider<BGR24, BGRA128F>
            , ICopyConverterDelegateProvider<BGR24, RGBP128F>
        {

            CopyConverterCallback<BGR24,Alpha8> ICopyConverterDelegateProvider<BGR24,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,Luminance8> ICopyConverterDelegateProvider<BGR24,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGR565> ICopyConverterDelegateProvider<BGR24,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGRA5551> ICopyConverterDelegateProvider<BGR24,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGRA4444> ICopyConverterDelegateProvider<BGR24,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,RGB24> ICopyConverterDelegateProvider<BGR24,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGR24> ICopyConverterDelegateProvider<BGR24,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGR24,RGBA32> ICopyConverterDelegateProvider<BGR24,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGRA32> ICopyConverterDelegateProvider<BGR24,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,ARGB32> ICopyConverterDelegateProvider<BGR24,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGRP32> ICopyConverterDelegateProvider<BGR24,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,RGBP32> ICopyConverterDelegateProvider<BGR24,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,Luminance32F> ICopyConverterDelegateProvider<BGR24,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,RGB96F> ICopyConverterDelegateProvider<BGR24,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGR96F> ICopyConverterDelegateProvider<BGR24,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }

            CopyConverterCallback<BGR24,RGBA128F> ICopyConverterDelegateProvider<BGR24,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,BGRA128F> ICopyConverterDelegateProvider<BGR24,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR24,RGBP128F> ICopyConverterDelegateProvider<BGR24,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR24> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGB24
            : ICopyConverterDelegateProvider<RGB24, Alpha8>
            , ICopyConverterDelegateProvider<RGB24, Luminance8>
            , ICopyConverterDelegateProvider<RGB24, BGR565>
            , ICopyConverterDelegateProvider<RGB24, BGRA5551>
            , ICopyConverterDelegateProvider<RGB24, BGRA4444>
            , ICopyConverterDelegateProvider<RGB24, RGB24>
            , ICopyConverterDelegateProvider<RGB24, BGR24>
            , ICopyConverterDelegateProvider<RGB24, RGBA32>
            , ICopyConverterDelegateProvider<RGB24, BGRA32>
            , ICopyConverterDelegateProvider<RGB24, ARGB32>
            , ICopyConverterDelegateProvider<RGB24, BGRP32>
            , ICopyConverterDelegateProvider<RGB24, RGBP32>
            , ICopyConverterDelegateProvider<RGB24, Luminance32F>
            , ICopyConverterDelegateProvider<RGB24, RGB96F>
            , ICopyConverterDelegateProvider<RGB24, BGR96F>
            , ICopyConverterDelegateProvider<RGB24, RGBA128F>
            , ICopyConverterDelegateProvider<RGB24, BGRA128F>
            , ICopyConverterDelegateProvider<RGB24, RGBP128F>
        {

            CopyConverterCallback<RGB24,Alpha8> ICopyConverterDelegateProvider<RGB24,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,Luminance8> ICopyConverterDelegateProvider<RGB24,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,BGR565> ICopyConverterDelegateProvider<RGB24,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,BGRA5551> ICopyConverterDelegateProvider<RGB24,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,BGRA4444> ICopyConverterDelegateProvider<RGB24,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,RGB24> ICopyConverterDelegateProvider<RGB24,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<RGB24,BGR24> ICopyConverterDelegateProvider<RGB24,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,RGBA32> ICopyConverterDelegateProvider<RGB24,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,BGRA32> ICopyConverterDelegateProvider<RGB24,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,ARGB32> ICopyConverterDelegateProvider<RGB24,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = 255;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,BGRP32> ICopyConverterDelegateProvider<RGB24,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,RGBP32> ICopyConverterDelegateProvider<RGB24,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,Luminance32F> ICopyConverterDelegateProvider<RGB24,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,RGB96F> ICopyConverterDelegateProvider<RGB24,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }

            CopyConverterCallback<RGB24,BGR96F> ICopyConverterDelegateProvider<RGB24,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,RGBA128F> ICopyConverterDelegateProvider<RGB24,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,BGRA128F> ICopyConverterDelegateProvider<RGB24,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB24,RGBP128F> ICopyConverterDelegateProvider<RGB24,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB24> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA5551
            : ICopyConverterDelegateProvider<BGRA5551, Alpha8>
            , ICopyConverterDelegateProvider<BGRA5551, Luminance8>
            , ICopyConverterDelegateProvider<BGRA5551, BGR565>
            , ICopyConverterDelegateProvider<BGRA5551, BGRA5551>
            , ICopyConverterDelegateProvider<BGRA5551, BGRA4444>
            , ICopyConverterDelegateProvider<BGRA5551, RGB24>
            , ICopyConverterDelegateProvider<BGRA5551, BGR24>
            , ICopyConverterDelegateProvider<BGRA5551, RGBA32>
            , ICopyConverterDelegateProvider<BGRA5551, BGRA32>
            , ICopyConverterDelegateProvider<BGRA5551, ARGB32>
            , ICopyConverterDelegateProvider<BGRA5551, BGRP32>
            , ICopyConverterDelegateProvider<BGRA5551, RGBP32>
            , ICopyConverterDelegateProvider<BGRA5551, Luminance32F>
            , ICopyConverterDelegateProvider<BGRA5551, RGB96F>
            , ICopyConverterDelegateProvider<BGRA5551, BGR96F>
            , ICopyConverterDelegateProvider<BGRA5551, RGBA128F>
            , ICopyConverterDelegateProvider<BGRA5551, BGRA128F>
            , ICopyConverterDelegateProvider<BGRA5551, RGBP128F>
        {

            CopyConverterCallback<BGRA5551,Alpha8> ICopyConverterDelegateProvider<BGRA5551,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,Luminance8> ICopyConverterDelegateProvider<BGRA5551,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,BGR565> ICopyConverterDelegateProvider<BGRA5551,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,BGRA5551> ICopyConverterDelegateProvider<BGRA5551,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGRA5551,BGRA4444> ICopyConverterDelegateProvider<BGRA5551,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,RGB24> ICopyConverterDelegateProvider<BGRA5551,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,BGR24> ICopyConverterDelegateProvider<BGRA5551,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,RGBA32> ICopyConverterDelegateProvider<BGRA5551,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,BGRA32> ICopyConverterDelegateProvider<BGRA5551,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,ARGB32> ICopyConverterDelegateProvider<BGRA5551,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,BGRP32> ICopyConverterDelegateProvider<BGRA5551,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,RGBP32> ICopyConverterDelegateProvider<BGRA5551,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,Luminance32F> ICopyConverterDelegateProvider<BGRA5551,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,RGB96F> ICopyConverterDelegateProvider<BGRA5551,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,BGR96F> ICopyConverterDelegateProvider<BGRA5551,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,RGBA128F> ICopyConverterDelegateProvider<BGRA5551,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,BGRA128F> ICopyConverterDelegateProvider<BGRA5551,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA5551,RGBP128F> ICopyConverterDelegateProvider<BGRA5551,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA5551> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA4444
            : ICopyConverterDelegateProvider<BGRA4444, Alpha8>
            , ICopyConverterDelegateProvider<BGRA4444, Luminance8>
            , ICopyConverterDelegateProvider<BGRA4444, BGR565>
            , ICopyConverterDelegateProvider<BGRA4444, BGRA5551>
            , ICopyConverterDelegateProvider<BGRA4444, BGRA4444>
            , ICopyConverterDelegateProvider<BGRA4444, RGB24>
            , ICopyConverterDelegateProvider<BGRA4444, BGR24>
            , ICopyConverterDelegateProvider<BGRA4444, RGBA32>
            , ICopyConverterDelegateProvider<BGRA4444, BGRA32>
            , ICopyConverterDelegateProvider<BGRA4444, ARGB32>
            , ICopyConverterDelegateProvider<BGRA4444, BGRP32>
            , ICopyConverterDelegateProvider<BGRA4444, RGBP32>
            , ICopyConverterDelegateProvider<BGRA4444, Luminance32F>
            , ICopyConverterDelegateProvider<BGRA4444, RGB96F>
            , ICopyConverterDelegateProvider<BGRA4444, BGR96F>
            , ICopyConverterDelegateProvider<BGRA4444, RGBA128F>
            , ICopyConverterDelegateProvider<BGRA4444, BGRA128F>
            , ICopyConverterDelegateProvider<BGRA4444, RGBP128F>
        {

            CopyConverterCallback<BGRA4444,Alpha8> ICopyConverterDelegateProvider<BGRA4444,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,Luminance8> ICopyConverterDelegateProvider<BGRA4444,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGR565> ICopyConverterDelegateProvider<BGRA4444,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGRA5551> ICopyConverterDelegateProvider<BGRA4444,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGRA4444> ICopyConverterDelegateProvider<BGRA4444,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGRA4444,RGB24> ICopyConverterDelegateProvider<BGRA4444,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGR24> ICopyConverterDelegateProvider<BGRA4444,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,RGBA32> ICopyConverterDelegateProvider<BGRA4444,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGRA32> ICopyConverterDelegateProvider<BGRA4444,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,ARGB32> ICopyConverterDelegateProvider<BGRA4444,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGRP32> ICopyConverterDelegateProvider<BGRA4444,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,RGBP32> ICopyConverterDelegateProvider<BGRA4444,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,Luminance32F> ICopyConverterDelegateProvider<BGRA4444,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,RGB96F> ICopyConverterDelegateProvider<BGRA4444,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGR96F> ICopyConverterDelegateProvider<BGRA4444,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,RGBA128F> ICopyConverterDelegateProvider<BGRA4444,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,BGRA128F> ICopyConverterDelegateProvider<BGRA4444,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA4444,RGBP128F> ICopyConverterDelegateProvider<BGRA4444,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA4444> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA32
            : ICopyConverterDelegateProvider<BGRA32, Alpha8>
            , ICopyConverterDelegateProvider<BGRA32, Luminance8>
            , ICopyConverterDelegateProvider<BGRA32, BGR565>
            , ICopyConverterDelegateProvider<BGRA32, BGRA5551>
            , ICopyConverterDelegateProvider<BGRA32, BGRA4444>
            , ICopyConverterDelegateProvider<BGRA32, RGB24>
            , ICopyConverterDelegateProvider<BGRA32, BGR24>
            , ICopyConverterDelegateProvider<BGRA32, RGBA32>
            , ICopyConverterDelegateProvider<BGRA32, BGRA32>
            , ICopyConverterDelegateProvider<BGRA32, ARGB32>
            , ICopyConverterDelegateProvider<BGRA32, BGRP32>
            , ICopyConverterDelegateProvider<BGRA32, RGBP32>
            , ICopyConverterDelegateProvider<BGRA32, Luminance32F>
            , ICopyConverterDelegateProvider<BGRA32, RGB96F>
            , ICopyConverterDelegateProvider<BGRA32, BGR96F>
            , ICopyConverterDelegateProvider<BGRA32, RGBA128F>
            , ICopyConverterDelegateProvider<BGRA32, BGRA128F>
            , ICopyConverterDelegateProvider<BGRA32, RGBP128F>
        {

            CopyConverterCallback<BGRA32,Alpha8> ICopyConverterDelegateProvider<BGRA32,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,Luminance8> ICopyConverterDelegateProvider<BGRA32,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGR565> ICopyConverterDelegateProvider<BGRA32,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGRA5551> ICopyConverterDelegateProvider<BGRA32,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGRA4444> ICopyConverterDelegateProvider<BGRA32,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,RGB24> ICopyConverterDelegateProvider<BGRA32,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGR24> ICopyConverterDelegateProvider<BGRA32,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,RGBA32> ICopyConverterDelegateProvider<BGRA32,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGRA32> ICopyConverterDelegateProvider<BGRA32,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGRA32,ARGB32> ICopyConverterDelegateProvider<BGRA32,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGRP32> ICopyConverterDelegateProvider<BGRA32,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,RGBP32> ICopyConverterDelegateProvider<BGRA32,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,Luminance32F> ICopyConverterDelegateProvider<BGRA32,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,RGB96F> ICopyConverterDelegateProvider<BGRA32,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGR96F> ICopyConverterDelegateProvider<BGRA32,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,RGBA128F> ICopyConverterDelegateProvider<BGRA32,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA32,BGRA128F> ICopyConverterDelegateProvider<BGRA32,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }

            CopyConverterCallback<BGRA32,RGBP128F> ICopyConverterDelegateProvider<BGRA32,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBA32
            : ICopyConverterDelegateProvider<RGBA32, Alpha8>
            , ICopyConverterDelegateProvider<RGBA32, Luminance8>
            , ICopyConverterDelegateProvider<RGBA32, BGR565>
            , ICopyConverterDelegateProvider<RGBA32, BGRA5551>
            , ICopyConverterDelegateProvider<RGBA32, BGRA4444>
            , ICopyConverterDelegateProvider<RGBA32, RGB24>
            , ICopyConverterDelegateProvider<RGBA32, BGR24>
            , ICopyConverterDelegateProvider<RGBA32, RGBA32>
            , ICopyConverterDelegateProvider<RGBA32, BGRA32>
            , ICopyConverterDelegateProvider<RGBA32, ARGB32>
            , ICopyConverterDelegateProvider<RGBA32, BGRP32>
            , ICopyConverterDelegateProvider<RGBA32, RGBP32>
            , ICopyConverterDelegateProvider<RGBA32, Luminance32F>
            , ICopyConverterDelegateProvider<RGBA32, RGB96F>
            , ICopyConverterDelegateProvider<RGBA32, BGR96F>
            , ICopyConverterDelegateProvider<RGBA32, RGBA128F>
            , ICopyConverterDelegateProvider<RGBA32, BGRA128F>
            , ICopyConverterDelegateProvider<RGBA32, RGBP128F>
        {

            CopyConverterCallback<RGBA32,Alpha8> ICopyConverterDelegateProvider<RGBA32,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,Luminance8> ICopyConverterDelegateProvider<RGBA32,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,BGR565> ICopyConverterDelegateProvider<RGBA32,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,BGRA5551> ICopyConverterDelegateProvider<RGBA32,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,BGRA4444> ICopyConverterDelegateProvider<RGBA32,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,RGB24> ICopyConverterDelegateProvider<RGBA32,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,BGR24> ICopyConverterDelegateProvider<RGBA32,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,RGBA32> ICopyConverterDelegateProvider<RGBA32,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<RGBA32,BGRA32> ICopyConverterDelegateProvider<RGBA32,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,ARGB32> ICopyConverterDelegateProvider<RGBA32,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,BGRP32> ICopyConverterDelegateProvider<RGBA32,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,RGBP32> ICopyConverterDelegateProvider<RGBA32,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,Luminance32F> ICopyConverterDelegateProvider<RGBA32,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,RGB96F> ICopyConverterDelegateProvider<RGBA32,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,BGR96F> ICopyConverterDelegateProvider<RGBA32,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,RGBA128F> ICopyConverterDelegateProvider<RGBA32,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }

            CopyConverterCallback<RGBA32,BGRA128F> ICopyConverterDelegateProvider<RGBA32,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA32,RGBP128F> ICopyConverterDelegateProvider<RGBA32,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct ARGB32
            : ICopyConverterDelegateProvider<ARGB32, Alpha8>
            , ICopyConverterDelegateProvider<ARGB32, Luminance8>
            , ICopyConverterDelegateProvider<ARGB32, BGR565>
            , ICopyConverterDelegateProvider<ARGB32, BGRA5551>
            , ICopyConverterDelegateProvider<ARGB32, BGRA4444>
            , ICopyConverterDelegateProvider<ARGB32, RGB24>
            , ICopyConverterDelegateProvider<ARGB32, BGR24>
            , ICopyConverterDelegateProvider<ARGB32, RGBA32>
            , ICopyConverterDelegateProvider<ARGB32, BGRA32>
            , ICopyConverterDelegateProvider<ARGB32, ARGB32>
            , ICopyConverterDelegateProvider<ARGB32, BGRP32>
            , ICopyConverterDelegateProvider<ARGB32, RGBP32>
            , ICopyConverterDelegateProvider<ARGB32, Luminance32F>
            , ICopyConverterDelegateProvider<ARGB32, RGB96F>
            , ICopyConverterDelegateProvider<ARGB32, BGR96F>
            , ICopyConverterDelegateProvider<ARGB32, RGBA128F>
            , ICopyConverterDelegateProvider<ARGB32, BGRA128F>
            , ICopyConverterDelegateProvider<ARGB32, RGBP128F>
        {

            CopyConverterCallback<ARGB32,Alpha8> ICopyConverterDelegateProvider<ARGB32,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,Luminance8> ICopyConverterDelegateProvider<ARGB32,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,BGR565> ICopyConverterDelegateProvider<ARGB32,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,BGRA5551> ICopyConverterDelegateProvider<ARGB32,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,BGRA4444> ICopyConverterDelegateProvider<ARGB32,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,RGB24> ICopyConverterDelegateProvider<ARGB32,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,BGR24> ICopyConverterDelegateProvider<ARGB32,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,RGBA32> ICopyConverterDelegateProvider<ARGB32,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,BGRA32> ICopyConverterDelegateProvider<ARGB32,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    // per channel copy.
                    dPtr = (Byte)sPtr.B;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.G;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.R;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                    dPtr = (Byte)sPtr.A;
                    dPtr = ref Unsafe.Add(ref dPtr, 1);

                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,ARGB32> ICopyConverterDelegateProvider<ARGB32,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<ARGB32,BGRP32> ICopyConverterDelegateProvider<ARGB32,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,RGBP32> ICopyConverterDelegateProvider<ARGB32,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,Luminance32F> ICopyConverterDelegateProvider<ARGB32,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,RGB96F> ICopyConverterDelegateProvider<ARGB32,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,BGR96F> ICopyConverterDelegateProvider<ARGB32,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,RGBA128F> ICopyConverterDelegateProvider<ARGB32,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,BGRA128F> ICopyConverterDelegateProvider<ARGB32,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<ARGB32,RGBP128F> ICopyConverterDelegateProvider<ARGB32,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<ARGB32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBP32
            : ICopyConverterDelegateProvider<RGBP32, Alpha8>
            , ICopyConverterDelegateProvider<RGBP32, Luminance8>
            , ICopyConverterDelegateProvider<RGBP32, BGR565>
            , ICopyConverterDelegateProvider<RGBP32, BGRA5551>
            , ICopyConverterDelegateProvider<RGBP32, BGRA4444>
            , ICopyConverterDelegateProvider<RGBP32, RGB24>
            , ICopyConverterDelegateProvider<RGBP32, BGR24>
            , ICopyConverterDelegateProvider<RGBP32, RGBA32>
            , ICopyConverterDelegateProvider<RGBP32, BGRA32>
            , ICopyConverterDelegateProvider<RGBP32, ARGB32>
            , ICopyConverterDelegateProvider<RGBP32, BGRP32>
            , ICopyConverterDelegateProvider<RGBP32, RGBP32>
            , ICopyConverterDelegateProvider<RGBP32, Luminance32F>
            , ICopyConverterDelegateProvider<RGBP32, RGB96F>
            , ICopyConverterDelegateProvider<RGBP32, BGR96F>
            , ICopyConverterDelegateProvider<RGBP32, RGBA128F>
            , ICopyConverterDelegateProvider<RGBP32, BGRA128F>
            , ICopyConverterDelegateProvider<RGBP32, RGBP128F>
        {

            CopyConverterCallback<RGBP32,Alpha8> ICopyConverterDelegateProvider<RGBP32,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,Luminance8> ICopyConverterDelegateProvider<RGBP32,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGR565> ICopyConverterDelegateProvider<RGBP32,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGRA5551> ICopyConverterDelegateProvider<RGBP32,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGRA4444> ICopyConverterDelegateProvider<RGBP32,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,RGB24> ICopyConverterDelegateProvider<RGBP32,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGR24> ICopyConverterDelegateProvider<RGBP32,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,RGBA32> ICopyConverterDelegateProvider<RGBP32,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGRA32> ICopyConverterDelegateProvider<RGBP32,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,ARGB32> ICopyConverterDelegateProvider<RGBP32,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGRP32> ICopyConverterDelegateProvider<RGBP32,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,RGBP32> ICopyConverterDelegateProvider<RGBP32,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<RGBP32,Luminance32F> ICopyConverterDelegateProvider<RGBP32,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,RGB96F> ICopyConverterDelegateProvider<RGBP32,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGR96F> ICopyConverterDelegateProvider<RGBP32,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,RGBA128F> ICopyConverterDelegateProvider<RGBP32,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,BGRA128F> ICopyConverterDelegateProvider<RGBP32,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP32,RGBP128F> ICopyConverterDelegateProvider<RGBP32,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }
        }
        partial struct BGRP32
            : ICopyConverterDelegateProvider<BGRP32, Alpha8>
            , ICopyConverterDelegateProvider<BGRP32, Luminance8>
            , ICopyConverterDelegateProvider<BGRP32, BGR565>
            , ICopyConverterDelegateProvider<BGRP32, BGRA5551>
            , ICopyConverterDelegateProvider<BGRP32, BGRA4444>
            , ICopyConverterDelegateProvider<BGRP32, RGB24>
            , ICopyConverterDelegateProvider<BGRP32, BGR24>
            , ICopyConverterDelegateProvider<BGRP32, RGBA32>
            , ICopyConverterDelegateProvider<BGRP32, BGRA32>
            , ICopyConverterDelegateProvider<BGRP32, ARGB32>
            , ICopyConverterDelegateProvider<BGRP32, BGRP32>
            , ICopyConverterDelegateProvider<BGRP32, RGBP32>
            , ICopyConverterDelegateProvider<BGRP32, Luminance32F>
            , ICopyConverterDelegateProvider<BGRP32, RGB96F>
            , ICopyConverterDelegateProvider<BGRP32, BGR96F>
            , ICopyConverterDelegateProvider<BGRP32, RGBA128F>
            , ICopyConverterDelegateProvider<BGRP32, BGRA128F>
            , ICopyConverterDelegateProvider<BGRP32, RGBP128F>
        {

            CopyConverterCallback<BGRP32,Alpha8> ICopyConverterDelegateProvider<BGRP32,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,Luminance8> ICopyConverterDelegateProvider<BGRP32,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGR565> ICopyConverterDelegateProvider<BGRP32,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGRA5551> ICopyConverterDelegateProvider<BGRP32,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGRA4444> ICopyConverterDelegateProvider<BGRP32,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,RGB24> ICopyConverterDelegateProvider<BGRP32,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGR24> ICopyConverterDelegateProvider<BGRP32,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,RGBA32> ICopyConverterDelegateProvider<BGRP32,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGRA32> ICopyConverterDelegateProvider<BGRP32,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,ARGB32> ICopyConverterDelegateProvider<BGRP32,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGRP32> ICopyConverterDelegateProvider<BGRP32,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGRP32,RGBP32> ICopyConverterDelegateProvider<BGRP32,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,Luminance32F> ICopyConverterDelegateProvider<BGRP32,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,RGB96F> ICopyConverterDelegateProvider<BGRP32,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGR96F> ICopyConverterDelegateProvider<BGRP32,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,RGBA128F> ICopyConverterDelegateProvider<BGRP32,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,BGRA128F> ICopyConverterDelegateProvider<BGRP32,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRP32,RGBP128F> ICopyConverterDelegateProvider<BGRP32,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRP32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGB96F
            : ICopyConverterDelegateProvider<RGB96F, Alpha8>
            , ICopyConverterDelegateProvider<RGB96F, Luminance8>
            , ICopyConverterDelegateProvider<RGB96F, BGR565>
            , ICopyConverterDelegateProvider<RGB96F, BGRA5551>
            , ICopyConverterDelegateProvider<RGB96F, BGRA4444>
            , ICopyConverterDelegateProvider<RGB96F, RGB24>
            , ICopyConverterDelegateProvider<RGB96F, BGR24>
            , ICopyConverterDelegateProvider<RGB96F, RGBA32>
            , ICopyConverterDelegateProvider<RGB96F, BGRA32>
            , ICopyConverterDelegateProvider<RGB96F, ARGB32>
            , ICopyConverterDelegateProvider<RGB96F, BGRP32>
            , ICopyConverterDelegateProvider<RGB96F, RGBP32>
            , ICopyConverterDelegateProvider<RGB96F, Luminance32F>
            , ICopyConverterDelegateProvider<RGB96F, RGB96F>
            , ICopyConverterDelegateProvider<RGB96F, BGR96F>
            , ICopyConverterDelegateProvider<RGB96F, RGBA128F>
            , ICopyConverterDelegateProvider<RGB96F, BGRA128F>
            , ICopyConverterDelegateProvider<RGB96F, RGBP128F>
        {

            CopyConverterCallback<RGB96F,Alpha8> ICopyConverterDelegateProvider<RGB96F,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,Luminance8> ICopyConverterDelegateProvider<RGB96F,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,BGR565> ICopyConverterDelegateProvider<RGB96F,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,BGRA5551> ICopyConverterDelegateProvider<RGB96F,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,BGRA4444> ICopyConverterDelegateProvider<RGB96F,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,RGB24> ICopyConverterDelegateProvider<RGB96F,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,BGR24> ICopyConverterDelegateProvider<RGB96F,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,RGBA32> ICopyConverterDelegateProvider<RGB96F,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,BGRA32> ICopyConverterDelegateProvider<RGB96F,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,ARGB32> ICopyConverterDelegateProvider<RGB96F,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,BGRP32> ICopyConverterDelegateProvider<RGB96F,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,RGBP32> ICopyConverterDelegateProvider<RGB96F,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,Luminance32F> ICopyConverterDelegateProvider<RGB96F,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,RGB96F> ICopyConverterDelegateProvider<RGB96F,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<RGB96F,BGR96F> ICopyConverterDelegateProvider<RGB96F,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,RGBA128F> ICopyConverterDelegateProvider<RGB96F,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,BGRA128F> ICopyConverterDelegateProvider<RGB96F,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGB96F,RGBP128F> ICopyConverterDelegateProvider<RGB96F,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGB96F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGR96F
            : ICopyConverterDelegateProvider<BGR96F, Alpha8>
            , ICopyConverterDelegateProvider<BGR96F, Luminance8>
            , ICopyConverterDelegateProvider<BGR96F, BGR565>
            , ICopyConverterDelegateProvider<BGR96F, BGRA5551>
            , ICopyConverterDelegateProvider<BGR96F, BGRA4444>
            , ICopyConverterDelegateProvider<BGR96F, RGB24>
            , ICopyConverterDelegateProvider<BGR96F, BGR24>
            , ICopyConverterDelegateProvider<BGR96F, RGBA32>
            , ICopyConverterDelegateProvider<BGR96F, BGRA32>
            , ICopyConverterDelegateProvider<BGR96F, ARGB32>
            , ICopyConverterDelegateProvider<BGR96F, BGRP32>
            , ICopyConverterDelegateProvider<BGR96F, RGBP32>
            , ICopyConverterDelegateProvider<BGR96F, Luminance32F>
            , ICopyConverterDelegateProvider<BGR96F, RGB96F>
            , ICopyConverterDelegateProvider<BGR96F, BGR96F>
            , ICopyConverterDelegateProvider<BGR96F, RGBA128F>
            , ICopyConverterDelegateProvider<BGR96F, BGRA128F>
            , ICopyConverterDelegateProvider<BGR96F, RGBP128F>
        {

            CopyConverterCallback<BGR96F,Alpha8> ICopyConverterDelegateProvider<BGR96F,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,Luminance8> ICopyConverterDelegateProvider<BGR96F,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGR565> ICopyConverterDelegateProvider<BGR96F,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGRA5551> ICopyConverterDelegateProvider<BGR96F,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGRA4444> ICopyConverterDelegateProvider<BGR96F,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,RGB24> ICopyConverterDelegateProvider<BGR96F,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGR24> ICopyConverterDelegateProvider<BGR96F,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,RGBA32> ICopyConverterDelegateProvider<BGR96F,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGRA32> ICopyConverterDelegateProvider<BGR96F,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,ARGB32> ICopyConverterDelegateProvider<BGR96F,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGRP32> ICopyConverterDelegateProvider<BGR96F,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,RGBP32> ICopyConverterDelegateProvider<BGR96F,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,Luminance32F> ICopyConverterDelegateProvider<BGR96F,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,RGB96F> ICopyConverterDelegateProvider<BGR96F,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGR96F> ICopyConverterDelegateProvider<BGR96F,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGR96F,RGBA128F> ICopyConverterDelegateProvider<BGR96F,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,BGRA128F> ICopyConverterDelegateProvider<BGR96F,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGR96F,RGBP128F> ICopyConverterDelegateProvider<BGR96F,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGR96F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA128F
            : ICopyConverterDelegateProvider<BGRA128F, Alpha8>
            , ICopyConverterDelegateProvider<BGRA128F, Luminance8>
            , ICopyConverterDelegateProvider<BGRA128F, BGR565>
            , ICopyConverterDelegateProvider<BGRA128F, BGRA5551>
            , ICopyConverterDelegateProvider<BGRA128F, BGRA4444>
            , ICopyConverterDelegateProvider<BGRA128F, RGB24>
            , ICopyConverterDelegateProvider<BGRA128F, BGR24>
            , ICopyConverterDelegateProvider<BGRA128F, RGBA32>
            , ICopyConverterDelegateProvider<BGRA128F, BGRA32>
            , ICopyConverterDelegateProvider<BGRA128F, ARGB32>
            , ICopyConverterDelegateProvider<BGRA128F, BGRP32>
            , ICopyConverterDelegateProvider<BGRA128F, RGBP32>
            , ICopyConverterDelegateProvider<BGRA128F, Luminance32F>
            , ICopyConverterDelegateProvider<BGRA128F, RGB96F>
            , ICopyConverterDelegateProvider<BGRA128F, BGR96F>
            , ICopyConverterDelegateProvider<BGRA128F, RGBA128F>
            , ICopyConverterDelegateProvider<BGRA128F, BGRA128F>
            , ICopyConverterDelegateProvider<BGRA128F, RGBP128F>
        {

            CopyConverterCallback<BGRA128F,Alpha8> ICopyConverterDelegateProvider<BGRA128F,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,Luminance8> ICopyConverterDelegateProvider<BGRA128F,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGR565> ICopyConverterDelegateProvider<BGRA128F,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGRA5551> ICopyConverterDelegateProvider<BGRA128F,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGRA4444> ICopyConverterDelegateProvider<BGRA128F,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,RGB24> ICopyConverterDelegateProvider<BGRA128F,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGR24> ICopyConverterDelegateProvider<BGRA128F,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,RGBA32> ICopyConverterDelegateProvider<BGRA128F,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGRA32> ICopyConverterDelegateProvider<BGRA128F,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,ARGB32> ICopyConverterDelegateProvider<BGRA128F,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGRP32> ICopyConverterDelegateProvider<BGRA128F,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,RGBP32> ICopyConverterDelegateProvider<BGRA128F,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,Luminance32F> ICopyConverterDelegateProvider<BGRA128F,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,RGB96F> ICopyConverterDelegateProvider<BGRA128F,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGR96F> ICopyConverterDelegateProvider<BGRA128F,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,RGBA128F> ICopyConverterDelegateProvider<BGRA128F,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<BGRA128F,BGRA128F> ICopyConverterDelegateProvider<BGRA128F,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<BGRA128F,RGBP128F> ICopyConverterDelegateProvider<BGRA128F,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<BGRA128F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBA128F
            : ICopyConverterDelegateProvider<RGBA128F, Alpha8>
            , ICopyConverterDelegateProvider<RGBA128F, Luminance8>
            , ICopyConverterDelegateProvider<RGBA128F, BGR565>
            , ICopyConverterDelegateProvider<RGBA128F, BGRA5551>
            , ICopyConverterDelegateProvider<RGBA128F, BGRA4444>
            , ICopyConverterDelegateProvider<RGBA128F, RGB24>
            , ICopyConverterDelegateProvider<RGBA128F, BGR24>
            , ICopyConverterDelegateProvider<RGBA128F, RGBA32>
            , ICopyConverterDelegateProvider<RGBA128F, BGRA32>
            , ICopyConverterDelegateProvider<RGBA128F, ARGB32>
            , ICopyConverterDelegateProvider<RGBA128F, BGRP32>
            , ICopyConverterDelegateProvider<RGBA128F, RGBP32>
            , ICopyConverterDelegateProvider<RGBA128F, Luminance32F>
            , ICopyConverterDelegateProvider<RGBA128F, RGB96F>
            , ICopyConverterDelegateProvider<RGBA128F, BGR96F>
            , ICopyConverterDelegateProvider<RGBA128F, RGBA128F>
            , ICopyConverterDelegateProvider<RGBA128F, BGRA128F>
            , ICopyConverterDelegateProvider<RGBA128F, RGBP128F>
        {

            CopyConverterCallback<RGBA128F,Alpha8> ICopyConverterDelegateProvider<RGBA128F,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,Luminance8> ICopyConverterDelegateProvider<RGBA128F,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,BGR565> ICopyConverterDelegateProvider<RGBA128F,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,BGRA5551> ICopyConverterDelegateProvider<RGBA128F,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,BGRA4444> ICopyConverterDelegateProvider<RGBA128F,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,RGB24> ICopyConverterDelegateProvider<RGBA128F,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,BGR24> ICopyConverterDelegateProvider<RGBA128F,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,RGBA32> ICopyConverterDelegateProvider<RGBA128F,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,BGRA32> ICopyConverterDelegateProvider<RGBA128F,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,ARGB32> ICopyConverterDelegateProvider<RGBA128F,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,BGRP32> ICopyConverterDelegateProvider<RGBA128F,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,RGBP32> ICopyConverterDelegateProvider<RGBA128F,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,Luminance32F> ICopyConverterDelegateProvider<RGBA128F,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,RGB96F> ICopyConverterDelegateProvider<RGBA128F,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,BGR96F> ICopyConverterDelegateProvider<RGBA128F,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,RGBA128F> ICopyConverterDelegateProvider<RGBA128F,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }

            CopyConverterCallback<RGBA128F,BGRA128F> ICopyConverterDelegateProvider<RGBA128F,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBA128F,RGBP128F> ICopyConverterDelegateProvider<RGBA128F,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBA128F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBP128F
            : ICopyConverterDelegateProvider<RGBP128F, Alpha8>
            , ICopyConverterDelegateProvider<RGBP128F, Luminance8>
            , ICopyConverterDelegateProvider<RGBP128F, BGR565>
            , ICopyConverterDelegateProvider<RGBP128F, BGRA5551>
            , ICopyConverterDelegateProvider<RGBP128F, BGRA4444>
            , ICopyConverterDelegateProvider<RGBP128F, RGB24>
            , ICopyConverterDelegateProvider<RGBP128F, BGR24>
            , ICopyConverterDelegateProvider<RGBP128F, RGBA32>
            , ICopyConverterDelegateProvider<RGBP128F, BGRA32>
            , ICopyConverterDelegateProvider<RGBP128F, ARGB32>
            , ICopyConverterDelegateProvider<RGBP128F, BGRP32>
            , ICopyConverterDelegateProvider<RGBP128F, RGBP32>
            , ICopyConverterDelegateProvider<RGBP128F, Luminance32F>
            , ICopyConverterDelegateProvider<RGBP128F, RGB96F>
            , ICopyConverterDelegateProvider<RGBP128F, BGR96F>
            , ICopyConverterDelegateProvider<RGBP128F, RGBA128F>
            , ICopyConverterDelegateProvider<RGBP128F, BGRA128F>
            , ICopyConverterDelegateProvider<RGBP128F, RGBP128F>
        {

            CopyConverterCallback<RGBP128F,Alpha8> ICopyConverterDelegateProvider<RGBP128F,Alpha8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,Luminance8> ICopyConverterDelegateProvider<RGBP128F,Luminance8>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGR565> ICopyConverterDelegateProvider<RGBP128F,BGR565>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGRA5551> ICopyConverterDelegateProvider<RGBP128F,BGRA5551>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGRA4444> ICopyConverterDelegateProvider<RGBP128F,BGRA4444>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,RGB24> ICopyConverterDelegateProvider<RGBP128F,RGB24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGR24> ICopyConverterDelegateProvider<RGBP128F,BGR24>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,RGBA32> ICopyConverterDelegateProvider<RGBP128F,RGBA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGRA32> ICopyConverterDelegateProvider<RGBP128F,BGRA32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,ARGB32> ICopyConverterDelegateProvider<RGBP128F,ARGB32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGRP32> ICopyConverterDelegateProvider<RGBP128F,BGRP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,RGBP32> ICopyConverterDelegateProvider<RGBP128F,RGBP32>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,Luminance32F> ICopyConverterDelegateProvider<RGBP128F,Luminance32F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,RGB96F> ICopyConverterDelegateProvider<RGBP128F,RGB96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGR96F> ICopyConverterDelegateProvider<RGBP128F,BGR96F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,RGBA128F> ICopyConverterDelegateProvider<RGBP128F,RGBA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,BGRA128F> ICopyConverterDelegateProvider<RGBP128F,BGRA128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref var sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref var dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                var dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }

            CopyConverterCallback<RGBP128F,RGBP128F> ICopyConverterDelegateProvider<RGBP128F,RGBP128F>.GetCopyConverterDelegate() { return Copy; }

            /// <summary>
            /// Copies all the values of <paramref name="src"/> into <paramref name="dst"/>.
            /// </summary>
            /// <param name="src">The source buffer.</param>
            /// <param name="dst">The target buffer.</param>
            public static void Copy(ReadOnlySpan<RGBP128F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
        }

    }
}
