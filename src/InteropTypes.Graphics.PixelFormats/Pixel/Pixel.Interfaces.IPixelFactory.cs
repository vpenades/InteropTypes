
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InteropBitmaps
{
    partial class Pixel    
    {
        internal static IPixelFactory<TSrcPixel, TDstPixel> TryGetPixelFactory<TSrcPixel,TDstPixel>()
            where TSrcPixel: unmanaged
            where TDstPixel: unmanaged
        {
            return default(TDstPixel) as IPixelFactory<TSrcPixel,TDstPixel>;
        }

        
        /// <summary>
        /// Represents an interface to a factory that is able to
        /// convert pixel values from one pixel type to another.
        /// </summary>
        /// <typeparam name="TSrcPixel">The source pixel type.</typeparam>
        /// <typeparam name="TDstPixel">The target pixel type.</typeparam>
        public interface IPixelFactory<TSrcPixel, TDstPixel>
            where TSrcPixel: unmanaged
            where TDstPixel: unmanaged
        {
            /// <remarks>
            /// Creates a new value of <typeparamref name="TDstPixel"/> type, which is converted from a <typeparamref name="TSrcPixel"/> value.
            /// </remarks>            
            TDstPixel From(TSrcPixel color);

            /// <summary>
            /// Copies all the values from <paramref name="src"/> into <paramref name="dst"/> applying the appropiate conversion.
            /// </summary>
            /// <param name="src">The pixels to read.</param>
            /// <param name="dst">The pixels to be written.</param>
            void Copy(ReadOnlySpan<TSrcPixel> src, Span<TDstPixel> dst);
        }
        
        

        partial struct Alpha8
            : IPixelFactory<Alpha8,Alpha8>
            , IPixelFactory<Luminance8,Alpha8>
            , IPixelFactory<BGR565,Alpha8>
            , IPixelFactory<BGRA5551,Alpha8>
            , IPixelFactory<BGRA4444,Alpha8>
            , IPixelFactory<RGB24,Alpha8>
            , IPixelFactory<BGR24,Alpha8>
            , IPixelFactory<RGBA32,Alpha8>
            , IPixelFactory<BGRA32,Alpha8>
            , IPixelFactory<ARGB32,Alpha8>
            , IPixelFactory<BGRP32,Alpha8>
            , IPixelFactory<RGBP32,Alpha8>
            , IPixelFactory<Luminance32F,Alpha8>
            , IPixelFactory<RGB96F,Alpha8>
            , IPixelFactory<BGR96F,Alpha8>
            , IPixelFactory<RGBA128F,Alpha8>
            , IPixelFactory<BGRA128F,Alpha8>
            , IPixelFactory<RGBP128F,Alpha8>
        {
            /// <inheritdoc />
            Alpha8 IPixelFactory<Alpha8,Alpha8>.From(Alpha8 color) { return color; }
            /// <inheritdoc />
            Alpha8 IPixelFactory<Luminance8,Alpha8>.From(Luminance8 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGR565,Alpha8>.From(BGR565 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGRA5551,Alpha8>.From(BGRA5551 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGRA4444,Alpha8>.From(BGRA4444 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<RGB24,Alpha8>.From(RGB24 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGR24,Alpha8>.From(BGR24 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<RGBA32,Alpha8>.From(RGBA32 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGRA32,Alpha8>.From(BGRA32 color) { return new Alpha8(color); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<ARGB32,Alpha8>.From(ARGB32 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGRP32,Alpha8>.From(BGRP32 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<RGBP32,Alpha8>.From(RGBP32 color) { return new Alpha8(new BGRA32(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<Luminance32F,Alpha8>.From(Luminance32F color) { return new Alpha8(new RGBA128F(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<RGB96F,Alpha8>.From(RGB96F color) { return new Alpha8(new RGBA128F(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGR96F,Alpha8>.From(BGR96F color) { return new Alpha8(new RGBA128F(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<RGBA128F,Alpha8>.From(RGBA128F color) { return new Alpha8(color); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<BGRA128F,Alpha8>.From(BGRA128F color) { return new Alpha8(new RGBA128F(color)); }
            /// <inheritdoc />
            Alpha8 IPixelFactory<RGBP128F,Alpha8>.From(RGBP128F color) { return new Alpha8(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,Alpha8>.Copy(ReadOnlySpan<Alpha8> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,Alpha8>.Copy(ReadOnlySpan<Luminance8> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.AsBytes().TryCopyTo(dst.AsBytes());
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,Alpha8>.Copy(ReadOnlySpan<BGR565> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,Alpha8>.Copy(ReadOnlySpan<BGRA5551> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,Alpha8>.Copy(ReadOnlySpan<BGRA4444> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,Alpha8>.Copy(ReadOnlySpan<RGB24> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,Alpha8>.Copy(ReadOnlySpan<BGR24> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,Alpha8>.Copy(ReadOnlySpan<RGBA32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,Alpha8>.Copy(ReadOnlySpan<BGRA32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,Alpha8>.Copy(ReadOnlySpan<ARGB32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,Alpha8>.Copy(ReadOnlySpan<BGRP32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,Alpha8>.Copy(ReadOnlySpan<RGBP32> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,Alpha8>.Copy(ReadOnlySpan<Luminance32F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,Alpha8>.Copy(ReadOnlySpan<RGB96F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,Alpha8>.Copy(ReadOnlySpan<BGR96F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,Alpha8>.Copy(ReadOnlySpan<RGBA128F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,Alpha8>.Copy(ReadOnlySpan<BGRA128F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,Alpha8>.Copy(ReadOnlySpan<RGBP128F> src, Span<Alpha8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Alpha8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Alpha8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct Luminance8
            : IPixelFactory<Alpha8,Luminance8>
            , IPixelFactory<Luminance8,Luminance8>
            , IPixelFactory<BGR565,Luminance8>
            , IPixelFactory<BGRA5551,Luminance8>
            , IPixelFactory<BGRA4444,Luminance8>
            , IPixelFactory<RGB24,Luminance8>
            , IPixelFactory<BGR24,Luminance8>
            , IPixelFactory<RGBA32,Luminance8>
            , IPixelFactory<BGRA32,Luminance8>
            , IPixelFactory<ARGB32,Luminance8>
            , IPixelFactory<BGRP32,Luminance8>
            , IPixelFactory<RGBP32,Luminance8>
            , IPixelFactory<Luminance32F,Luminance8>
            , IPixelFactory<RGB96F,Luminance8>
            , IPixelFactory<BGR96F,Luminance8>
            , IPixelFactory<RGBA128F,Luminance8>
            , IPixelFactory<BGRA128F,Luminance8>
            , IPixelFactory<RGBP128F,Luminance8>
        {
            /// <inheritdoc />
            Luminance8 IPixelFactory<Alpha8,Luminance8>.From(Alpha8 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<Luminance8,Luminance8>.From(Luminance8 color) { return color; }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGR565,Luminance8>.From(BGR565 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGRA5551,Luminance8>.From(BGRA5551 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGRA4444,Luminance8>.From(BGRA4444 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<RGB24,Luminance8>.From(RGB24 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGR24,Luminance8>.From(BGR24 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<RGBA32,Luminance8>.From(RGBA32 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGRA32,Luminance8>.From(BGRA32 color) { return new Luminance8(color); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<ARGB32,Luminance8>.From(ARGB32 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGRP32,Luminance8>.From(BGRP32 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<RGBP32,Luminance8>.From(RGBP32 color) { return new Luminance8(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<Luminance32F,Luminance8>.From(Luminance32F color) { return new Luminance8(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<RGB96F,Luminance8>.From(RGB96F color) { return new Luminance8(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGR96F,Luminance8>.From(BGR96F color) { return new Luminance8(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<RGBA128F,Luminance8>.From(RGBA128F color) { return new Luminance8(color); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<BGRA128F,Luminance8>.From(BGRA128F color) { return new Luminance8(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance8 IPixelFactory<RGBP128F,Luminance8>.From(RGBP128F color) { return new Luminance8(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,Luminance8>.Copy(ReadOnlySpan<Alpha8> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.AsBytes().TryCopyTo(dst.AsBytes());
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,Luminance8>.Copy(ReadOnlySpan<Luminance8> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,Luminance8>.Copy(ReadOnlySpan<BGR565> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,Luminance8>.Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,Luminance8>.Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,Luminance8>.Copy(ReadOnlySpan<RGB24> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,Luminance8>.Copy(ReadOnlySpan<BGR24> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,Luminance8>.Copy(ReadOnlySpan<RGBA32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,Luminance8>.Copy(ReadOnlySpan<BGRA32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,Luminance8>.Copy(ReadOnlySpan<ARGB32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,Luminance8>.Copy(ReadOnlySpan<BGRP32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,Luminance8>.Copy(ReadOnlySpan<RGBP32> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,Luminance8>.Copy(ReadOnlySpan<Luminance32F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,Luminance8>.Copy(ReadOnlySpan<RGB96F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,Luminance8>.Copy(ReadOnlySpan<BGR96F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,Luminance8>.Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,Luminance8>.Copy(ReadOnlySpan<BGRA128F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,Luminance8>.Copy(ReadOnlySpan<RGBP128F> src, Span<Luminance8> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance8 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance8(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct Luminance16
            : IPixelFactory<Alpha8,Luminance16>
            , IPixelFactory<Luminance8,Luminance16>
            , IPixelFactory<BGR565,Luminance16>
            , IPixelFactory<BGRA5551,Luminance16>
            , IPixelFactory<BGRA4444,Luminance16>
            , IPixelFactory<RGB24,Luminance16>
            , IPixelFactory<BGR24,Luminance16>
            , IPixelFactory<RGBA32,Luminance16>
            , IPixelFactory<BGRA32,Luminance16>
            , IPixelFactory<ARGB32,Luminance16>
            , IPixelFactory<BGRP32,Luminance16>
            , IPixelFactory<RGBP32,Luminance16>
            , IPixelFactory<Luminance32F,Luminance16>
            , IPixelFactory<RGB96F,Luminance16>
            , IPixelFactory<BGR96F,Luminance16>
            , IPixelFactory<RGBA128F,Luminance16>
            , IPixelFactory<BGRA128F,Luminance16>
            , IPixelFactory<RGBP128F,Luminance16>
        {
            /// <inheritdoc />
            Luminance16 IPixelFactory<Alpha8,Luminance16>.From(Alpha8 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<Luminance8,Luminance16>.From(Luminance8 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGR565,Luminance16>.From(BGR565 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGRA5551,Luminance16>.From(BGRA5551 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGRA4444,Luminance16>.From(BGRA4444 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<RGB24,Luminance16>.From(RGB24 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGR24,Luminance16>.From(BGR24 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<RGBA32,Luminance16>.From(RGBA32 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGRA32,Luminance16>.From(BGRA32 color) { return new Luminance16(color); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<ARGB32,Luminance16>.From(ARGB32 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGRP32,Luminance16>.From(BGRP32 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<RGBP32,Luminance16>.From(RGBP32 color) { return new Luminance16(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<Luminance32F,Luminance16>.From(Luminance32F color) { return new Luminance16(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<RGB96F,Luminance16>.From(RGB96F color) { return new Luminance16(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGR96F,Luminance16>.From(BGR96F color) { return new Luminance16(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<RGBA128F,Luminance16>.From(RGBA128F color) { return new Luminance16(color); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<BGRA128F,Luminance16>.From(BGRA128F color) { return new Luminance16(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance16 IPixelFactory<RGBP128F,Luminance16>.From(RGBP128F color) { return new Luminance16(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,Luminance16>.Copy(ReadOnlySpan<Alpha8> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,Luminance16>.Copy(ReadOnlySpan<Luminance8> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,Luminance16>.Copy(ReadOnlySpan<BGR565> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,Luminance16>.Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,Luminance16>.Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,Luminance16>.Copy(ReadOnlySpan<RGB24> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,Luminance16>.Copy(ReadOnlySpan<BGR24> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,Luminance16>.Copy(ReadOnlySpan<RGBA32> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,Luminance16>.Copy(ReadOnlySpan<BGRA32> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,Luminance16>.Copy(ReadOnlySpan<ARGB32> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,Luminance16>.Copy(ReadOnlySpan<BGRP32> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,Luminance16>.Copy(ReadOnlySpan<RGBP32> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,Luminance16>.Copy(ReadOnlySpan<Luminance32F> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,Luminance16>.Copy(ReadOnlySpan<RGB96F> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,Luminance16>.Copy(ReadOnlySpan<BGR96F> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,Luminance16>.Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,Luminance16>.Copy(ReadOnlySpan<BGRA128F> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,Luminance16>.Copy(ReadOnlySpan<RGBP128F> src, Span<Luminance16> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance16 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance16(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct Luminance32F
            : IPixelFactory<Alpha8,Luminance32F>
            , IPixelFactory<Luminance8,Luminance32F>
            , IPixelFactory<BGR565,Luminance32F>
            , IPixelFactory<BGRA5551,Luminance32F>
            , IPixelFactory<BGRA4444,Luminance32F>
            , IPixelFactory<RGB24,Luminance32F>
            , IPixelFactory<BGR24,Luminance32F>
            , IPixelFactory<RGBA32,Luminance32F>
            , IPixelFactory<BGRA32,Luminance32F>
            , IPixelFactory<ARGB32,Luminance32F>
            , IPixelFactory<BGRP32,Luminance32F>
            , IPixelFactory<RGBP32,Luminance32F>
            , IPixelFactory<Luminance32F,Luminance32F>
            , IPixelFactory<RGB96F,Luminance32F>
            , IPixelFactory<BGR96F,Luminance32F>
            , IPixelFactory<RGBA128F,Luminance32F>
            , IPixelFactory<BGRA128F,Luminance32F>
            , IPixelFactory<RGBP128F,Luminance32F>
        {
            /// <inheritdoc />
            Luminance32F IPixelFactory<Alpha8,Luminance32F>.From(Alpha8 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<Luminance8,Luminance32F>.From(Luminance8 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGR565,Luminance32F>.From(BGR565 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGRA5551,Luminance32F>.From(BGRA5551 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGRA4444,Luminance32F>.From(BGRA4444 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<RGB24,Luminance32F>.From(RGB24 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGR24,Luminance32F>.From(BGR24 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<RGBA32,Luminance32F>.From(RGBA32 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGRA32,Luminance32F>.From(BGRA32 color) { return new Luminance32F(color); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<ARGB32,Luminance32F>.From(ARGB32 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGRP32,Luminance32F>.From(BGRP32 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<RGBP32,Luminance32F>.From(RGBP32 color) { return new Luminance32F(new BGRA32(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<Luminance32F,Luminance32F>.From(Luminance32F color) { return color; }
            /// <inheritdoc />
            Luminance32F IPixelFactory<RGB96F,Luminance32F>.From(RGB96F color) { return new Luminance32F(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGR96F,Luminance32F>.From(BGR96F color) { return new Luminance32F(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<RGBA128F,Luminance32F>.From(RGBA128F color) { return new Luminance32F(color); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<BGRA128F,Luminance32F>.From(BGRA128F color) { return new Luminance32F(new RGBA128F(color)); }
            /// <inheritdoc />
            Luminance32F IPixelFactory<RGBP128F,Luminance32F>.From(RGBP128F color) { return new Luminance32F(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,Luminance32F>.Copy(ReadOnlySpan<Alpha8> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,Luminance32F>.Copy(ReadOnlySpan<Luminance8> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,Luminance32F>.Copy(ReadOnlySpan<BGR565> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,Luminance32F>.Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,Luminance32F>.Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,Luminance32F>.Copy(ReadOnlySpan<RGB24> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,Luminance32F>.Copy(ReadOnlySpan<BGR24> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,Luminance32F>.Copy(ReadOnlySpan<RGBA32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,Luminance32F>.Copy(ReadOnlySpan<BGRA32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,Luminance32F>.Copy(ReadOnlySpan<ARGB32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,Luminance32F>.Copy(ReadOnlySpan<BGRP32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,Luminance32F>.Copy(ReadOnlySpan<RGBP32> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,Luminance32F>.Copy(ReadOnlySpan<Luminance32F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,Luminance32F>.Copy(ReadOnlySpan<RGB96F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,Luminance32F>.Copy(ReadOnlySpan<BGR96F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,Luminance32F>.Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,Luminance32F>.Copy(ReadOnlySpan<BGRA128F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,Luminance32F>.Copy(ReadOnlySpan<RGBP128F> src, Span<Luminance32F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Luminance32F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new Luminance32F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGR565
            : IPixelFactory<Alpha8,BGR565>
            , IPixelFactory<Luminance8,BGR565>
            , IPixelFactory<BGR565,BGR565>
            , IPixelFactory<BGRA5551,BGR565>
            , IPixelFactory<BGRA4444,BGR565>
            , IPixelFactory<RGB24,BGR565>
            , IPixelFactory<BGR24,BGR565>
            , IPixelFactory<RGBA32,BGR565>
            , IPixelFactory<BGRA32,BGR565>
            , IPixelFactory<ARGB32,BGR565>
            , IPixelFactory<BGRP32,BGR565>
            , IPixelFactory<RGBP32,BGR565>
            , IPixelFactory<Luminance32F,BGR565>
            , IPixelFactory<RGB96F,BGR565>
            , IPixelFactory<BGR96F,BGR565>
            , IPixelFactory<RGBA128F,BGR565>
            , IPixelFactory<BGRA128F,BGR565>
            , IPixelFactory<RGBP128F,BGR565>
        {
            /// <inheritdoc />
            BGR565 IPixelFactory<Alpha8,BGR565>.From(Alpha8 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<Luminance8,BGR565>.From(Luminance8 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGR565,BGR565>.From(BGR565 color) { return color; }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGRA5551,BGR565>.From(BGRA5551 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGRA4444,BGR565>.From(BGRA4444 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<RGB24,BGR565>.From(RGB24 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGR24,BGR565>.From(BGR24 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<RGBA32,BGR565>.From(RGBA32 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGRA32,BGR565>.From(BGRA32 color) { return new BGR565(color); }
            /// <inheritdoc />
            BGR565 IPixelFactory<ARGB32,BGR565>.From(ARGB32 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGRP32,BGR565>.From(BGRP32 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<RGBP32,BGR565>.From(RGBP32 color) { return new BGR565(new BGRA32(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<Luminance32F,BGR565>.From(Luminance32F color) { return new BGR565(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<RGB96F,BGR565>.From(RGB96F color) { return new BGR565(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGR96F,BGR565>.From(BGR96F color) { return new BGR565(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<RGBA128F,BGR565>.From(RGBA128F color) { return new BGR565(color); }
            /// <inheritdoc />
            BGR565 IPixelFactory<BGRA128F,BGR565>.From(BGRA128F color) { return new BGR565(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR565 IPixelFactory<RGBP128F,BGR565>.From(RGBP128F color) { return new BGR565(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGR565>.Copy(ReadOnlySpan<Alpha8> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGR565>.Copy(ReadOnlySpan<Luminance8> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGR565>.Copy(ReadOnlySpan<BGR565> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGR565>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGR565>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGR565>.Copy(ReadOnlySpan<RGB24> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGR565>.Copy(ReadOnlySpan<BGR24> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGR565>.Copy(ReadOnlySpan<RGBA32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGR565>.Copy(ReadOnlySpan<BGRA32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGR565>.Copy(ReadOnlySpan<ARGB32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGR565>.Copy(ReadOnlySpan<BGRP32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGR565>.Copy(ReadOnlySpan<RGBP32> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGR565>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGR565>.Copy(ReadOnlySpan<RGB96F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGR565>.Copy(ReadOnlySpan<BGR96F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGR565>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGR565>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGR565>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGR565> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR565 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR565(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGR24
            : IPixelFactory<Alpha8,BGR24>
            , IPixelFactory<Luminance8,BGR24>
            , IPixelFactory<BGR565,BGR24>
            , IPixelFactory<BGRA5551,BGR24>
            , IPixelFactory<BGRA4444,BGR24>
            , IPixelFactory<RGB24,BGR24>
            , IPixelFactory<BGR24,BGR24>
            , IPixelFactory<RGBA32,BGR24>
            , IPixelFactory<BGRA32,BGR24>
            , IPixelFactory<ARGB32,BGR24>
            , IPixelFactory<BGRP32,BGR24>
            , IPixelFactory<RGBP32,BGR24>
            , IPixelFactory<Luminance32F,BGR24>
            , IPixelFactory<RGB96F,BGR24>
            , IPixelFactory<BGR96F,BGR24>
            , IPixelFactory<RGBA128F,BGR24>
            , IPixelFactory<BGRA128F,BGR24>
            , IPixelFactory<RGBP128F,BGR24>
        {
            /// <inheritdoc />
            BGR24 IPixelFactory<Alpha8,BGR24>.From(Alpha8 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<Luminance8,BGR24>.From(Luminance8 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGR565,BGR24>.From(BGR565 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGRA5551,BGR24>.From(BGRA5551 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGRA4444,BGR24>.From(BGRA4444 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<RGB24,BGR24>.From(RGB24 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGR24,BGR24>.From(BGR24 color) { return color; }
            /// <inheritdoc />
            BGR24 IPixelFactory<RGBA32,BGR24>.From(RGBA32 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGRA32,BGR24>.From(BGRA32 color) { return new BGR24(color); }
            /// <inheritdoc />
            BGR24 IPixelFactory<ARGB32,BGR24>.From(ARGB32 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGRP32,BGR24>.From(BGRP32 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<RGBP32,BGR24>.From(RGBP32 color) { return new BGR24(new BGRA32(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<Luminance32F,BGR24>.From(Luminance32F color) { return new BGR24(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<RGB96F,BGR24>.From(RGB96F color) { return new BGR24(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGR96F,BGR24>.From(BGR96F color) { return new BGR24(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<RGBA128F,BGR24>.From(RGBA128F color) { return new BGR24(color); }
            /// <inheritdoc />
            BGR24 IPixelFactory<BGRA128F,BGR24>.From(BGRA128F color) { return new BGR24(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR24 IPixelFactory<RGBP128F,BGR24>.From(RGBP128F color) { return new BGR24(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGR24>.Copy(ReadOnlySpan<Alpha8> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGR24>.Copy(ReadOnlySpan<Luminance8> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGR24>.Copy(ReadOnlySpan<BGR565> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGR24>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGR24>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGR24>.Copy(ReadOnlySpan<RGB24> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGR24>.Copy(ReadOnlySpan<BGR24> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGR24>.Copy(ReadOnlySpan<RGBA32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGR24>.Copy(ReadOnlySpan<BGRA32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGR24>.Copy(ReadOnlySpan<ARGB32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGR24>.Copy(ReadOnlySpan<BGRP32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGR24>.Copy(ReadOnlySpan<RGBP32> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGR24>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGR24>.Copy(ReadOnlySpan<RGB96F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGR24>.Copy(ReadOnlySpan<BGR96F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGR24>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGR24>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGR24>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGR24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGB24
            : IPixelFactory<Alpha8,RGB24>
            , IPixelFactory<Luminance8,RGB24>
            , IPixelFactory<BGR565,RGB24>
            , IPixelFactory<BGRA5551,RGB24>
            , IPixelFactory<BGRA4444,RGB24>
            , IPixelFactory<RGB24,RGB24>
            , IPixelFactory<BGR24,RGB24>
            , IPixelFactory<RGBA32,RGB24>
            , IPixelFactory<BGRA32,RGB24>
            , IPixelFactory<ARGB32,RGB24>
            , IPixelFactory<BGRP32,RGB24>
            , IPixelFactory<RGBP32,RGB24>
            , IPixelFactory<Luminance32F,RGB24>
            , IPixelFactory<RGB96F,RGB24>
            , IPixelFactory<BGR96F,RGB24>
            , IPixelFactory<RGBA128F,RGB24>
            , IPixelFactory<BGRA128F,RGB24>
            , IPixelFactory<RGBP128F,RGB24>
        {
            /// <inheritdoc />
            RGB24 IPixelFactory<Alpha8,RGB24>.From(Alpha8 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<Luminance8,RGB24>.From(Luminance8 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGR565,RGB24>.From(BGR565 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGRA5551,RGB24>.From(BGRA5551 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGRA4444,RGB24>.From(BGRA4444 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<RGB24,RGB24>.From(RGB24 color) { return color; }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGR24,RGB24>.From(BGR24 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<RGBA32,RGB24>.From(RGBA32 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGRA32,RGB24>.From(BGRA32 color) { return new RGB24(color); }
            /// <inheritdoc />
            RGB24 IPixelFactory<ARGB32,RGB24>.From(ARGB32 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGRP32,RGB24>.From(BGRP32 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<RGBP32,RGB24>.From(RGBP32 color) { return new RGB24(new BGRA32(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<Luminance32F,RGB24>.From(Luminance32F color) { return new RGB24(new RGBA128F(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<RGB96F,RGB24>.From(RGB96F color) { return new RGB24(new RGBA128F(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGR96F,RGB24>.From(BGR96F color) { return new RGB24(new RGBA128F(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<RGBA128F,RGB24>.From(RGBA128F color) { return new RGB24(color); }
            /// <inheritdoc />
            RGB24 IPixelFactory<BGRA128F,RGB24>.From(BGRA128F color) { return new RGB24(new RGBA128F(color)); }
            /// <inheritdoc />
            RGB24 IPixelFactory<RGBP128F,RGB24>.From(RGBP128F color) { return new RGB24(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,RGB24>.Copy(ReadOnlySpan<Alpha8> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,RGB24>.Copy(ReadOnlySpan<Luminance8> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,RGB24>.Copy(ReadOnlySpan<BGR565> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,RGB24>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,RGB24>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,RGB24>.Copy(ReadOnlySpan<RGB24> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,RGB24>.Copy(ReadOnlySpan<BGR24> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,RGB24>.Copy(ReadOnlySpan<RGBA32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,RGB24>.Copy(ReadOnlySpan<BGRA32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,RGB24>.Copy(ReadOnlySpan<ARGB32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,RGB24>.Copy(ReadOnlySpan<BGRP32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,RGB24>.Copy(ReadOnlySpan<RGBP32> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,RGB24>.Copy(ReadOnlySpan<Luminance32F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,RGB24>.Copy(ReadOnlySpan<RGB96F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,RGB24>.Copy(ReadOnlySpan<BGR96F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,RGB24>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,RGB24>.Copy(ReadOnlySpan<BGRA128F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,RGB24>.Copy(ReadOnlySpan<RGBP128F> src, Span<RGB24> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB24 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB24(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA5551
            : IPixelFactory<Alpha8,BGRA5551>
            , IPixelFactory<Luminance8,BGRA5551>
            , IPixelFactory<BGR565,BGRA5551>
            , IPixelFactory<BGRA5551,BGRA5551>
            , IPixelFactory<BGRA4444,BGRA5551>
            , IPixelFactory<RGB24,BGRA5551>
            , IPixelFactory<BGR24,BGRA5551>
            , IPixelFactory<RGBA32,BGRA5551>
            , IPixelFactory<BGRA32,BGRA5551>
            , IPixelFactory<ARGB32,BGRA5551>
            , IPixelFactory<BGRP32,BGRA5551>
            , IPixelFactory<RGBP32,BGRA5551>
            , IPixelFactory<Luminance32F,BGRA5551>
            , IPixelFactory<RGB96F,BGRA5551>
            , IPixelFactory<BGR96F,BGRA5551>
            , IPixelFactory<RGBA128F,BGRA5551>
            , IPixelFactory<BGRA128F,BGRA5551>
            , IPixelFactory<RGBP128F,BGRA5551>
        {
            /// <inheritdoc />
            BGRA5551 IPixelFactory<Alpha8,BGRA5551>.From(Alpha8 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<Luminance8,BGRA5551>.From(Luminance8 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGR565,BGRA5551>.From(BGR565 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGRA5551,BGRA5551>.From(BGRA5551 color) { return color; }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGRA4444,BGRA5551>.From(BGRA4444 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<RGB24,BGRA5551>.From(RGB24 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGR24,BGRA5551>.From(BGR24 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<RGBA32,BGRA5551>.From(RGBA32 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGRA32,BGRA5551>.From(BGRA32 color) { return new BGRA5551(color); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<ARGB32,BGRA5551>.From(ARGB32 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGRP32,BGRA5551>.From(BGRP32 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<RGBP32,BGRA5551>.From(RGBP32 color) { return new BGRA5551(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<Luminance32F,BGRA5551>.From(Luminance32F color) { return new BGRA5551(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<RGB96F,BGRA5551>.From(RGB96F color) { return new BGRA5551(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGR96F,BGRA5551>.From(BGR96F color) { return new BGRA5551(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<RGBA128F,BGRA5551>.From(RGBA128F color) { return new BGRA5551(color); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<BGRA128F,BGRA5551>.From(BGRA128F color) { return new BGRA5551(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA5551 IPixelFactory<RGBP128F,BGRA5551>.From(RGBP128F color) { return new BGRA5551(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGRA5551>.Copy(ReadOnlySpan<Alpha8> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGRA5551>.Copy(ReadOnlySpan<Luminance8> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGRA5551>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGRA5551>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGRA5551>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGRA5551>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGRA5551>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGRA5551>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGRA5551>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGRA5551>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGRA5551>.Copy(ReadOnlySpan<BGRP32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGRA5551>.Copy(ReadOnlySpan<RGBP32> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGRA5551>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGRA5551>.Copy(ReadOnlySpan<RGB96F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGRA5551>.Copy(ReadOnlySpan<BGR96F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGRA5551>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGRA5551>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGRA5551>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA5551> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA5551 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA5551(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA4444
            : IPixelFactory<Alpha8,BGRA4444>
            , IPixelFactory<Luminance8,BGRA4444>
            , IPixelFactory<BGR565,BGRA4444>
            , IPixelFactory<BGRA5551,BGRA4444>
            , IPixelFactory<BGRA4444,BGRA4444>
            , IPixelFactory<RGB24,BGRA4444>
            , IPixelFactory<BGR24,BGRA4444>
            , IPixelFactory<RGBA32,BGRA4444>
            , IPixelFactory<BGRA32,BGRA4444>
            , IPixelFactory<ARGB32,BGRA4444>
            , IPixelFactory<BGRP32,BGRA4444>
            , IPixelFactory<RGBP32,BGRA4444>
            , IPixelFactory<Luminance32F,BGRA4444>
            , IPixelFactory<RGB96F,BGRA4444>
            , IPixelFactory<BGR96F,BGRA4444>
            , IPixelFactory<RGBA128F,BGRA4444>
            , IPixelFactory<BGRA128F,BGRA4444>
            , IPixelFactory<RGBP128F,BGRA4444>
        {
            /// <inheritdoc />
            BGRA4444 IPixelFactory<Alpha8,BGRA4444>.From(Alpha8 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<Luminance8,BGRA4444>.From(Luminance8 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGR565,BGRA4444>.From(BGR565 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGRA5551,BGRA4444>.From(BGRA5551 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGRA4444,BGRA4444>.From(BGRA4444 color) { return color; }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<RGB24,BGRA4444>.From(RGB24 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGR24,BGRA4444>.From(BGR24 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<RGBA32,BGRA4444>.From(RGBA32 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGRA32,BGRA4444>.From(BGRA32 color) { return new BGRA4444(color); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<ARGB32,BGRA4444>.From(ARGB32 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGRP32,BGRA4444>.From(BGRP32 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<RGBP32,BGRA4444>.From(RGBP32 color) { return new BGRA4444(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<Luminance32F,BGRA4444>.From(Luminance32F color) { return new BGRA4444(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<RGB96F,BGRA4444>.From(RGB96F color) { return new BGRA4444(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGR96F,BGRA4444>.From(BGR96F color) { return new BGRA4444(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<RGBA128F,BGRA4444>.From(RGBA128F color) { return new BGRA4444(color); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<BGRA128F,BGRA4444>.From(BGRA128F color) { return new BGRA4444(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA4444 IPixelFactory<RGBP128F,BGRA4444>.From(RGBP128F color) { return new BGRA4444(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGRA4444>.Copy(ReadOnlySpan<Alpha8> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGRA4444>.Copy(ReadOnlySpan<Luminance8> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGRA4444>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGRA4444>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGRA4444>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGRA4444>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGRA4444>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGRA4444>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGRA4444>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGRA4444>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGRA4444>.Copy(ReadOnlySpan<BGRP32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGRA4444>.Copy(ReadOnlySpan<RGBP32> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGRA4444>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGRA4444>.Copy(ReadOnlySpan<RGB96F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGRA4444>.Copy(ReadOnlySpan<BGR96F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGRA4444>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGRA4444>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGRA4444>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA4444> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA4444 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA4444(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA32
            : IPixelFactory<Alpha8,BGRA32>
            , IPixelFactory<Luminance8,BGRA32>
            , IPixelFactory<BGR565,BGRA32>
            , IPixelFactory<BGRA5551,BGRA32>
            , IPixelFactory<BGRA4444,BGRA32>
            , IPixelFactory<RGB24,BGRA32>
            , IPixelFactory<BGR24,BGRA32>
            , IPixelFactory<RGBA32,BGRA32>
            , IPixelFactory<BGRA32,BGRA32>
            , IPixelFactory<ARGB32,BGRA32>
            , IPixelFactory<BGRP32,BGRA32>
            , IPixelFactory<RGBP32,BGRA32>
            , IPixelFactory<Luminance32F,BGRA32>
            , IPixelFactory<RGB96F,BGRA32>
            , IPixelFactory<BGR96F,BGRA32>
            , IPixelFactory<RGBA128F,BGRA32>
            , IPixelFactory<BGRA128F,BGRA32>
            , IPixelFactory<RGBP128F,BGRA32>
        {
            /// <inheritdoc />
            BGRA32 IPixelFactory<Alpha8,BGRA32>.From(Alpha8 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<Luminance8,BGRA32>.From(Luminance8 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGR565,BGRA32>.From(BGR565 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGRA5551,BGRA32>.From(BGRA5551 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGRA4444,BGRA32>.From(BGRA4444 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<RGB24,BGRA32>.From(RGB24 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGR24,BGRA32>.From(BGR24 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<RGBA32,BGRA32>.From(RGBA32 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGRA32,BGRA32>.From(BGRA32 color) { return color; }
            /// <inheritdoc />
            BGRA32 IPixelFactory<ARGB32,BGRA32>.From(ARGB32 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGRP32,BGRA32>.From(BGRP32 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<RGBP32,BGRA32>.From(RGBP32 color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<Luminance32F,BGRA32>.From(Luminance32F color) { return new BGRA32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<RGB96F,BGRA32>.From(RGB96F color) { return new BGRA32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGR96F,BGRA32>.From(BGR96F color) { return new BGRA32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<RGBA128F,BGRA32>.From(RGBA128F color) { return new BGRA32(color); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<BGRA128F,BGRA32>.From(BGRA128F color) { return new BGRA32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA32 IPixelFactory<RGBP128F,BGRA32>.From(RGBP128F color) { return new BGRA32(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGRA32>.Copy(ReadOnlySpan<Alpha8> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGRA32>.Copy(ReadOnlySpan<Luminance8> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGRA32>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGRA32>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGRA32>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGRA32>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGRA32>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGRA32>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGRA32>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGRA32>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGRA32>.Copy(ReadOnlySpan<BGRP32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGRA32>.Copy(ReadOnlySpan<RGBP32> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGRA32>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGRA32>.Copy(ReadOnlySpan<RGB96F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGRA32>.Copy(ReadOnlySpan<BGR96F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGRA32>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGRA32>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGRA32>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBA32
            : IPixelFactory<Alpha8,RGBA32>
            , IPixelFactory<Luminance8,RGBA32>
            , IPixelFactory<BGR565,RGBA32>
            , IPixelFactory<BGRA5551,RGBA32>
            , IPixelFactory<BGRA4444,RGBA32>
            , IPixelFactory<RGB24,RGBA32>
            , IPixelFactory<BGR24,RGBA32>
            , IPixelFactory<RGBA32,RGBA32>
            , IPixelFactory<BGRA32,RGBA32>
            , IPixelFactory<ARGB32,RGBA32>
            , IPixelFactory<BGRP32,RGBA32>
            , IPixelFactory<RGBP32,RGBA32>
            , IPixelFactory<Luminance32F,RGBA32>
            , IPixelFactory<RGB96F,RGBA32>
            , IPixelFactory<BGR96F,RGBA32>
            , IPixelFactory<RGBA128F,RGBA32>
            , IPixelFactory<BGRA128F,RGBA32>
            , IPixelFactory<RGBP128F,RGBA32>
        {
            /// <inheritdoc />
            RGBA32 IPixelFactory<Alpha8,RGBA32>.From(Alpha8 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<Luminance8,RGBA32>.From(Luminance8 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGR565,RGBA32>.From(BGR565 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGRA5551,RGBA32>.From(BGRA5551 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGRA4444,RGBA32>.From(BGRA4444 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<RGB24,RGBA32>.From(RGB24 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGR24,RGBA32>.From(BGR24 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<RGBA32,RGBA32>.From(RGBA32 color) { return color; }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGRA32,RGBA32>.From(BGRA32 color) { return new RGBA32(color); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<ARGB32,RGBA32>.From(ARGB32 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGRP32,RGBA32>.From(BGRP32 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<RGBP32,RGBA32>.From(RGBP32 color) { return new RGBA32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<Luminance32F,RGBA32>.From(Luminance32F color) { return new RGBA32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<RGB96F,RGBA32>.From(RGB96F color) { return new RGBA32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGR96F,RGBA32>.From(BGR96F color) { return new RGBA32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<RGBA128F,RGBA32>.From(RGBA128F color) { return new RGBA32(color); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<BGRA128F,RGBA32>.From(BGRA128F color) { return new RGBA32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBA32 IPixelFactory<RGBP128F,RGBA32>.From(RGBP128F color) { return new RGBA32(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,RGBA32>.Copy(ReadOnlySpan<Alpha8> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,RGBA32>.Copy(ReadOnlySpan<Luminance8> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,RGBA32>.Copy(ReadOnlySpan<BGR565> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,RGBA32>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,RGBA32>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,RGBA32>.Copy(ReadOnlySpan<RGB24> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,RGBA32>.Copy(ReadOnlySpan<BGR24> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,RGBA32>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,RGBA32>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,RGBA32>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,RGBA32>.Copy(ReadOnlySpan<BGRP32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,RGBA32>.Copy(ReadOnlySpan<RGBP32> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,RGBA32>.Copy(ReadOnlySpan<Luminance32F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,RGBA32>.Copy(ReadOnlySpan<RGB96F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,RGBA32>.Copy(ReadOnlySpan<BGR96F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,RGBA32>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,RGBA32>.Copy(ReadOnlySpan<BGRA128F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,RGBA32>.Copy(ReadOnlySpan<RGBP128F> src, Span<RGBA32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct ARGB32
            : IPixelFactory<Alpha8,ARGB32>
            , IPixelFactory<Luminance8,ARGB32>
            , IPixelFactory<BGR565,ARGB32>
            , IPixelFactory<BGRA5551,ARGB32>
            , IPixelFactory<BGRA4444,ARGB32>
            , IPixelFactory<RGB24,ARGB32>
            , IPixelFactory<BGR24,ARGB32>
            , IPixelFactory<RGBA32,ARGB32>
            , IPixelFactory<BGRA32,ARGB32>
            , IPixelFactory<ARGB32,ARGB32>
            , IPixelFactory<BGRP32,ARGB32>
            , IPixelFactory<RGBP32,ARGB32>
            , IPixelFactory<Luminance32F,ARGB32>
            , IPixelFactory<RGB96F,ARGB32>
            , IPixelFactory<BGR96F,ARGB32>
            , IPixelFactory<RGBA128F,ARGB32>
            , IPixelFactory<BGRA128F,ARGB32>
            , IPixelFactory<RGBP128F,ARGB32>
        {
            /// <inheritdoc />
            ARGB32 IPixelFactory<Alpha8,ARGB32>.From(Alpha8 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<Luminance8,ARGB32>.From(Luminance8 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGR565,ARGB32>.From(BGR565 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGRA5551,ARGB32>.From(BGRA5551 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGRA4444,ARGB32>.From(BGRA4444 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<RGB24,ARGB32>.From(RGB24 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGR24,ARGB32>.From(BGR24 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<RGBA32,ARGB32>.From(RGBA32 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGRA32,ARGB32>.From(BGRA32 color) { return new ARGB32(color); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<ARGB32,ARGB32>.From(ARGB32 color) { return color; }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGRP32,ARGB32>.From(BGRP32 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<RGBP32,ARGB32>.From(RGBP32 color) { return new ARGB32(new BGRA32(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<Luminance32F,ARGB32>.From(Luminance32F color) { return new ARGB32(new RGBA128F(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<RGB96F,ARGB32>.From(RGB96F color) { return new ARGB32(new RGBA128F(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGR96F,ARGB32>.From(BGR96F color) { return new ARGB32(new RGBA128F(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<RGBA128F,ARGB32>.From(RGBA128F color) { return new ARGB32(color); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<BGRA128F,ARGB32>.From(BGRA128F color) { return new ARGB32(new RGBA128F(color)); }
            /// <inheritdoc />
            ARGB32 IPixelFactory<RGBP128F,ARGB32>.From(RGBP128F color) { return new ARGB32(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,ARGB32>.Copy(ReadOnlySpan<Alpha8> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,ARGB32>.Copy(ReadOnlySpan<Luminance8> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,ARGB32>.Copy(ReadOnlySpan<BGR565> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,ARGB32>.Copy(ReadOnlySpan<BGRA5551> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,ARGB32>.Copy(ReadOnlySpan<BGRA4444> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,ARGB32>.Copy(ReadOnlySpan<RGB24> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,ARGB32>.Copy(ReadOnlySpan<BGR24> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,ARGB32>.Copy(ReadOnlySpan<RGBA32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,ARGB32>.Copy(ReadOnlySpan<BGRA32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref Byte dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst.AsBytes());
                int dLen = dst.Length;

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
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,ARGB32>.Copy(ReadOnlySpan<ARGB32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,ARGB32>.Copy(ReadOnlySpan<BGRP32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,ARGB32>.Copy(ReadOnlySpan<RGBP32> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,ARGB32>.Copy(ReadOnlySpan<Luminance32F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,ARGB32>.Copy(ReadOnlySpan<RGB96F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,ARGB32>.Copy(ReadOnlySpan<BGR96F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,ARGB32>.Copy(ReadOnlySpan<RGBA128F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,ARGB32>.Copy(ReadOnlySpan<BGRA128F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,ARGB32>.Copy(ReadOnlySpan<RGBP128F> src, Span<ARGB32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref ARGB32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new ARGB32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBP32
            : IPixelFactory<Alpha8,RGBP32>
            , IPixelFactory<Luminance8,RGBP32>
            , IPixelFactory<BGR565,RGBP32>
            , IPixelFactory<BGRA5551,RGBP32>
            , IPixelFactory<BGRA4444,RGBP32>
            , IPixelFactory<RGB24,RGBP32>
            , IPixelFactory<BGR24,RGBP32>
            , IPixelFactory<RGBA32,RGBP32>
            , IPixelFactory<BGRA32,RGBP32>
            , IPixelFactory<ARGB32,RGBP32>
            , IPixelFactory<BGRP32,RGBP32>
            , IPixelFactory<RGBP32,RGBP32>
            , IPixelFactory<Luminance32F,RGBP32>
            , IPixelFactory<RGB96F,RGBP32>
            , IPixelFactory<BGR96F,RGBP32>
            , IPixelFactory<RGBA128F,RGBP32>
            , IPixelFactory<BGRA128F,RGBP32>
            , IPixelFactory<RGBP128F,RGBP32>
        {
            /// <inheritdoc />
            RGBP32 IPixelFactory<Alpha8,RGBP32>.From(Alpha8 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<Luminance8,RGBP32>.From(Luminance8 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGR565,RGBP32>.From(BGR565 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGRA5551,RGBP32>.From(BGRA5551 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGRA4444,RGBP32>.From(BGRA4444 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<RGB24,RGBP32>.From(RGB24 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGR24,RGBP32>.From(BGR24 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<RGBA32,RGBP32>.From(RGBA32 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGRA32,RGBP32>.From(BGRA32 color) { return new RGBP32(color); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<ARGB32,RGBP32>.From(ARGB32 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGRP32,RGBP32>.From(BGRP32 color) { return new RGBP32(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<RGBP32,RGBP32>.From(RGBP32 color) { return color; }
            /// <inheritdoc />
            RGBP32 IPixelFactory<Luminance32F,RGBP32>.From(Luminance32F color) { return new RGBP32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<RGB96F,RGBP32>.From(RGB96F color) { return new RGBP32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGR96F,RGBP32>.From(BGR96F color) { return new RGBP32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<RGBA128F,RGBP32>.From(RGBA128F color) { return new RGBP32(color); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<BGRA128F,RGBP32>.From(BGRA128F color) { return new RGBP32(new RGBA128F(color)); }
            /// <inheritdoc />
            RGBP32 IPixelFactory<RGBP128F,RGBP32>.From(RGBP128F color) { return new RGBP32(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,RGBP32>.Copy(ReadOnlySpan<Alpha8> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,RGBP32>.Copy(ReadOnlySpan<Luminance8> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,RGBP32>.Copy(ReadOnlySpan<BGR565> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,RGBP32>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,RGBP32>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,RGBP32>.Copy(ReadOnlySpan<RGB24> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,RGBP32>.Copy(ReadOnlySpan<BGR24> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,RGBP32>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,RGBP32>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,RGBP32>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,RGBP32>.Copy(ReadOnlySpan<BGRP32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,RGBP32>.Copy(ReadOnlySpan<RGBP32> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,RGBP32>.Copy(ReadOnlySpan<Luminance32F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,RGBP32>.Copy(ReadOnlySpan<RGB96F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,RGBP32>.Copy(ReadOnlySpan<BGR96F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,RGBP32>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,RGBP32>.Copy(ReadOnlySpan<BGRA128F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,RGBP32>.Copy(ReadOnlySpan<RGBP128F> src, Span<RGBP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRP32
            : IPixelFactory<Alpha8,BGRP32>
            , IPixelFactory<Luminance8,BGRP32>
            , IPixelFactory<BGR565,BGRP32>
            , IPixelFactory<BGRA5551,BGRP32>
            , IPixelFactory<BGRA4444,BGRP32>
            , IPixelFactory<RGB24,BGRP32>
            , IPixelFactory<BGR24,BGRP32>
            , IPixelFactory<RGBA32,BGRP32>
            , IPixelFactory<BGRA32,BGRP32>
            , IPixelFactory<ARGB32,BGRP32>
            , IPixelFactory<BGRP32,BGRP32>
            , IPixelFactory<RGBP32,BGRP32>
            , IPixelFactory<Luminance32F,BGRP32>
            , IPixelFactory<RGB96F,BGRP32>
            , IPixelFactory<BGR96F,BGRP32>
            , IPixelFactory<RGBA128F,BGRP32>
            , IPixelFactory<BGRA128F,BGRP32>
            , IPixelFactory<RGBP128F,BGRP32>
        {
            /// <inheritdoc />
            BGRP32 IPixelFactory<Alpha8,BGRP32>.From(Alpha8 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<Luminance8,BGRP32>.From(Luminance8 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGR565,BGRP32>.From(BGR565 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGRA5551,BGRP32>.From(BGRA5551 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGRA4444,BGRP32>.From(BGRA4444 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<RGB24,BGRP32>.From(RGB24 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGR24,BGRP32>.From(BGR24 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<RGBA32,BGRP32>.From(RGBA32 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGRA32,BGRP32>.From(BGRA32 color) { return new BGRP32(color); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<ARGB32,BGRP32>.From(ARGB32 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGRP32,BGRP32>.From(BGRP32 color) { return color; }
            /// <inheritdoc />
            BGRP32 IPixelFactory<RGBP32,BGRP32>.From(RGBP32 color) { return new BGRP32(new BGRA32(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<Luminance32F,BGRP32>.From(Luminance32F color) { return new BGRP32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<RGB96F,BGRP32>.From(RGB96F color) { return new BGRP32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGR96F,BGRP32>.From(BGR96F color) { return new BGRP32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<RGBA128F,BGRP32>.From(RGBA128F color) { return new BGRP32(color); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<BGRA128F,BGRP32>.From(BGRA128F color) { return new BGRP32(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRP32 IPixelFactory<RGBP128F,BGRP32>.From(RGBP128F color) { return new BGRP32(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGRP32>.Copy(ReadOnlySpan<Alpha8> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGRP32>.Copy(ReadOnlySpan<Luminance8> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGRP32>.Copy(ReadOnlySpan<BGR565> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGRP32>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGRP32>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGRP32>.Copy(ReadOnlySpan<RGB24> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGRP32>.Copy(ReadOnlySpan<BGR24> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGRP32>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGRP32>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGRP32>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGRP32>.Copy(ReadOnlySpan<BGRP32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGRP32>.Copy(ReadOnlySpan<RGBP32> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGRP32>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGRP32>.Copy(ReadOnlySpan<RGB96F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGRP32>.Copy(ReadOnlySpan<BGR96F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGRP32>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGRP32>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGRP32>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGRP32> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRP32 dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRP32(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGB96F
            : IPixelFactory<Alpha8,RGB96F>
            , IPixelFactory<Luminance8,RGB96F>
            , IPixelFactory<BGR565,RGB96F>
            , IPixelFactory<BGRA5551,RGB96F>
            , IPixelFactory<BGRA4444,RGB96F>
            , IPixelFactory<RGB24,RGB96F>
            , IPixelFactory<BGR24,RGB96F>
            , IPixelFactory<RGBA32,RGB96F>
            , IPixelFactory<BGRA32,RGB96F>
            , IPixelFactory<ARGB32,RGB96F>
            , IPixelFactory<BGRP32,RGB96F>
            , IPixelFactory<RGBP32,RGB96F>
            , IPixelFactory<Luminance32F,RGB96F>
            , IPixelFactory<RGB96F,RGB96F>
            , IPixelFactory<BGR96F,RGB96F>
            , IPixelFactory<RGBA128F,RGB96F>
            , IPixelFactory<BGRA128F,RGB96F>
            , IPixelFactory<RGBP128F,RGB96F>
        {
            /// <inheritdoc />
            RGB96F IPixelFactory<Alpha8,RGB96F>.From(Alpha8 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<Luminance8,RGB96F>.From(Luminance8 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGR565,RGB96F>.From(BGR565 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGRA5551,RGB96F>.From(BGRA5551 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGRA4444,RGB96F>.From(BGRA4444 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<RGB24,RGB96F>.From(RGB24 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGR24,RGB96F>.From(BGR24 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<RGBA32,RGB96F>.From(RGBA32 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGRA32,RGB96F>.From(BGRA32 color) { return new RGB96F(color); }
            /// <inheritdoc />
            RGB96F IPixelFactory<ARGB32,RGB96F>.From(ARGB32 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGRP32,RGB96F>.From(BGRP32 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<RGBP32,RGB96F>.From(RGBP32 color) { return new RGB96F(new BGRA32(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<Luminance32F,RGB96F>.From(Luminance32F color) { return new RGB96F(new RGBA128F(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<RGB96F,RGB96F>.From(RGB96F color) { return color; }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGR96F,RGB96F>.From(BGR96F color) { return new RGB96F(new RGBA128F(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<RGBA128F,RGB96F>.From(RGBA128F color) { return new RGB96F(color); }
            /// <inheritdoc />
            RGB96F IPixelFactory<BGRA128F,RGB96F>.From(BGRA128F color) { return new RGB96F(new RGBA128F(color)); }
            /// <inheritdoc />
            RGB96F IPixelFactory<RGBP128F,RGB96F>.From(RGBP128F color) { return new RGB96F(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,RGB96F>.Copy(ReadOnlySpan<Alpha8> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,RGB96F>.Copy(ReadOnlySpan<Luminance8> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,RGB96F>.Copy(ReadOnlySpan<BGR565> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,RGB96F>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,RGB96F>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,RGB96F>.Copy(ReadOnlySpan<RGB24> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,RGB96F>.Copy(ReadOnlySpan<BGR24> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,RGB96F>.Copy(ReadOnlySpan<RGBA32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,RGB96F>.Copy(ReadOnlySpan<BGRA32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,RGB96F>.Copy(ReadOnlySpan<ARGB32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,RGB96F>.Copy(ReadOnlySpan<BGRP32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,RGB96F>.Copy(ReadOnlySpan<RGBP32> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,RGB96F>.Copy(ReadOnlySpan<Luminance32F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,RGB96F>.Copy(ReadOnlySpan<RGB96F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,RGB96F>.Copy(ReadOnlySpan<BGR96F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,RGB96F>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,RGB96F>.Copy(ReadOnlySpan<BGRA128F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,RGB96F>.Copy(ReadOnlySpan<RGBP128F> src, Span<RGB96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGB96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGB96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGR96F
            : IPixelFactory<Alpha8,BGR96F>
            , IPixelFactory<Luminance8,BGR96F>
            , IPixelFactory<BGR565,BGR96F>
            , IPixelFactory<BGRA5551,BGR96F>
            , IPixelFactory<BGRA4444,BGR96F>
            , IPixelFactory<RGB24,BGR96F>
            , IPixelFactory<BGR24,BGR96F>
            , IPixelFactory<RGBA32,BGR96F>
            , IPixelFactory<BGRA32,BGR96F>
            , IPixelFactory<ARGB32,BGR96F>
            , IPixelFactory<BGRP32,BGR96F>
            , IPixelFactory<RGBP32,BGR96F>
            , IPixelFactory<Luminance32F,BGR96F>
            , IPixelFactory<RGB96F,BGR96F>
            , IPixelFactory<BGR96F,BGR96F>
            , IPixelFactory<RGBA128F,BGR96F>
            , IPixelFactory<BGRA128F,BGR96F>
            , IPixelFactory<RGBP128F,BGR96F>
        {
            /// <inheritdoc />
            BGR96F IPixelFactory<Alpha8,BGR96F>.From(Alpha8 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<Luminance8,BGR96F>.From(Luminance8 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGR565,BGR96F>.From(BGR565 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGRA5551,BGR96F>.From(BGRA5551 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGRA4444,BGR96F>.From(BGRA4444 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<RGB24,BGR96F>.From(RGB24 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGR24,BGR96F>.From(BGR24 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<RGBA32,BGR96F>.From(RGBA32 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGRA32,BGR96F>.From(BGRA32 color) { return new BGR96F(color); }
            /// <inheritdoc />
            BGR96F IPixelFactory<ARGB32,BGR96F>.From(ARGB32 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGRP32,BGR96F>.From(BGRP32 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<RGBP32,BGR96F>.From(RGBP32 color) { return new BGR96F(new BGRA32(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<Luminance32F,BGR96F>.From(Luminance32F color) { return new BGR96F(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<RGB96F,BGR96F>.From(RGB96F color) { return new BGR96F(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGR96F,BGR96F>.From(BGR96F color) { return color; }
            /// <inheritdoc />
            BGR96F IPixelFactory<RGBA128F,BGR96F>.From(RGBA128F color) { return new BGR96F(color); }
            /// <inheritdoc />
            BGR96F IPixelFactory<BGRA128F,BGR96F>.From(BGRA128F color) { return new BGR96F(new RGBA128F(color)); }
            /// <inheritdoc />
            BGR96F IPixelFactory<RGBP128F,BGR96F>.From(RGBP128F color) { return new BGR96F(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGR96F>.Copy(ReadOnlySpan<Alpha8> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGR96F>.Copy(ReadOnlySpan<Luminance8> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGR96F>.Copy(ReadOnlySpan<BGR565> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGR96F>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGR96F>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGR96F>.Copy(ReadOnlySpan<RGB24> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGR96F>.Copy(ReadOnlySpan<BGR24> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGR96F>.Copy(ReadOnlySpan<RGBA32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGR96F>.Copy(ReadOnlySpan<BGRA32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGR96F>.Copy(ReadOnlySpan<ARGB32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGR96F>.Copy(ReadOnlySpan<BGRP32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGR96F>.Copy(ReadOnlySpan<RGBP32> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGR96F>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGR96F>.Copy(ReadOnlySpan<RGB96F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGR96F>.Copy(ReadOnlySpan<BGR96F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGR96F>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGR96F>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGR96F>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGR96F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGR96F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGR96F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct BGRA128F
            : IPixelFactory<Alpha8,BGRA128F>
            , IPixelFactory<Luminance8,BGRA128F>
            , IPixelFactory<BGR565,BGRA128F>
            , IPixelFactory<BGRA5551,BGRA128F>
            , IPixelFactory<BGRA4444,BGRA128F>
            , IPixelFactory<RGB24,BGRA128F>
            , IPixelFactory<BGR24,BGRA128F>
            , IPixelFactory<RGBA32,BGRA128F>
            , IPixelFactory<BGRA32,BGRA128F>
            , IPixelFactory<ARGB32,BGRA128F>
            , IPixelFactory<BGRP32,BGRA128F>
            , IPixelFactory<RGBP32,BGRA128F>
            , IPixelFactory<Luminance32F,BGRA128F>
            , IPixelFactory<RGB96F,BGRA128F>
            , IPixelFactory<BGR96F,BGRA128F>
            , IPixelFactory<RGBA128F,BGRA128F>
            , IPixelFactory<BGRA128F,BGRA128F>
            , IPixelFactory<RGBP128F,BGRA128F>
        {
            /// <inheritdoc />
            BGRA128F IPixelFactory<Alpha8,BGRA128F>.From(Alpha8 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<Luminance8,BGRA128F>.From(Luminance8 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGR565,BGRA128F>.From(BGR565 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGRA5551,BGRA128F>.From(BGRA5551 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGRA4444,BGRA128F>.From(BGRA4444 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<RGB24,BGRA128F>.From(RGB24 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGR24,BGRA128F>.From(BGR24 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<RGBA32,BGRA128F>.From(RGBA32 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGRA32,BGRA128F>.From(BGRA32 color) { return new BGRA128F(color); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<ARGB32,BGRA128F>.From(ARGB32 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGRP32,BGRA128F>.From(BGRP32 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<RGBP32,BGRA128F>.From(RGBP32 color) { return new BGRA128F(new BGRA32(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<Luminance32F,BGRA128F>.From(Luminance32F color) { return new BGRA128F(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<RGB96F,BGRA128F>.From(RGB96F color) { return new BGRA128F(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGR96F,BGRA128F>.From(BGR96F color) { return new BGRA128F(new RGBA128F(color)); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<RGBA128F,BGRA128F>.From(RGBA128F color) { return new BGRA128F(color); }
            /// <inheritdoc />
            BGRA128F IPixelFactory<BGRA128F,BGRA128F>.From(BGRA128F color) { return color; }
            /// <inheritdoc />
            BGRA128F IPixelFactory<RGBP128F,BGRA128F>.From(RGBP128F color) { return new BGRA128F(new RGBA128F(color)); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,BGRA128F>.Copy(ReadOnlySpan<Alpha8> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,BGRA128F>.Copy(ReadOnlySpan<Luminance8> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,BGRA128F>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,BGRA128F>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,BGRA128F>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,BGRA128F>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,BGRA128F>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,BGRA128F>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,BGRA128F>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,BGRA128F>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,BGRA128F>.Copy(ReadOnlySpan<BGRP32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,BGRA128F>.Copy(ReadOnlySpan<RGBP32> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,BGRA128F>.Copy(ReadOnlySpan<Luminance32F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,BGRA128F>.Copy(ReadOnlySpan<RGB96F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,BGRA128F>.Copy(ReadOnlySpan<BGR96F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,BGRA128F>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,BGRA128F>.Copy(ReadOnlySpan<BGRA128F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,BGRA128F>.Copy(ReadOnlySpan<RGBP128F> src, Span<BGRA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref BGRA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new BGRA128F(new RGBA128F(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBA128F
            : IPixelFactory<Alpha8,RGBA128F>
            , IPixelFactory<Luminance8,RGBA128F>
            , IPixelFactory<BGR565,RGBA128F>
            , IPixelFactory<BGRA5551,RGBA128F>
            , IPixelFactory<BGRA4444,RGBA128F>
            , IPixelFactory<RGB24,RGBA128F>
            , IPixelFactory<BGR24,RGBA128F>
            , IPixelFactory<RGBA32,RGBA128F>
            , IPixelFactory<BGRA32,RGBA128F>
            , IPixelFactory<ARGB32,RGBA128F>
            , IPixelFactory<BGRP32,RGBA128F>
            , IPixelFactory<RGBP32,RGBA128F>
            , IPixelFactory<Luminance32F,RGBA128F>
            , IPixelFactory<RGB96F,RGBA128F>
            , IPixelFactory<BGR96F,RGBA128F>
            , IPixelFactory<RGBA128F,RGBA128F>
            , IPixelFactory<BGRA128F,RGBA128F>
            , IPixelFactory<RGBP128F,RGBA128F>
        {
            /// <inheritdoc />
            RGBA128F IPixelFactory<Alpha8,RGBA128F>.From(Alpha8 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<Luminance8,RGBA128F>.From(Luminance8 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGR565,RGBA128F>.From(BGR565 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGRA5551,RGBA128F>.From(BGRA5551 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGRA4444,RGBA128F>.From(BGRA4444 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<RGB24,RGBA128F>.From(RGB24 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGR24,RGBA128F>.From(BGR24 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<RGBA32,RGBA128F>.From(RGBA32 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGRA32,RGBA128F>.From(BGRA32 color) { return new RGBA128F(color); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<ARGB32,RGBA128F>.From(ARGB32 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGRP32,RGBA128F>.From(BGRP32 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<RGBP32,RGBA128F>.From(RGBP32 color) { return new RGBA128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<Luminance32F,RGBA128F>.From(Luminance32F color) { return new RGBA128F(color); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<RGB96F,RGBA128F>.From(RGB96F color) { return new RGBA128F(color); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGR96F,RGBA128F>.From(BGR96F color) { return new RGBA128F(color); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<RGBA128F,RGBA128F>.From(RGBA128F color) { return color; }
            /// <inheritdoc />
            RGBA128F IPixelFactory<BGRA128F,RGBA128F>.From(BGRA128F color) { return new RGBA128F(color); }
            /// <inheritdoc />
            RGBA128F IPixelFactory<RGBP128F,RGBA128F>.From(RGBP128F color) { return new RGBA128F(color); }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,RGBA128F>.Copy(ReadOnlySpan<Alpha8> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,RGBA128F>.Copy(ReadOnlySpan<Luminance8> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,RGBA128F>.Copy(ReadOnlySpan<BGR565> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,RGBA128F>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,RGBA128F>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,RGBA128F>.Copy(ReadOnlySpan<RGB24> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,RGBA128F>.Copy(ReadOnlySpan<BGR24> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,RGBA128F>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,RGBA128F>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,RGBA128F>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,RGBA128F>.Copy(ReadOnlySpan<BGRP32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,RGBA128F>.Copy(ReadOnlySpan<RGBP32> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,RGBA128F>.Copy(ReadOnlySpan<Luminance32F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,RGBA128F>.Copy(ReadOnlySpan<RGB96F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,RGBA128F>.Copy(ReadOnlySpan<BGR96F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,RGBA128F>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,RGBA128F>.Copy(ReadOnlySpan<BGRA128F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,RGBA128F>.Copy(ReadOnlySpan<RGBP128F> src, Span<RGBA128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBP128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBA128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBA128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
        }
        partial struct RGBP128F
            : IPixelFactory<Alpha8,RGBP128F>
            , IPixelFactory<Luminance8,RGBP128F>
            , IPixelFactory<BGR565,RGBP128F>
            , IPixelFactory<BGRA5551,RGBP128F>
            , IPixelFactory<BGRA4444,RGBP128F>
            , IPixelFactory<RGB24,RGBP128F>
            , IPixelFactory<BGR24,RGBP128F>
            , IPixelFactory<RGBA32,RGBP128F>
            , IPixelFactory<BGRA32,RGBP128F>
            , IPixelFactory<ARGB32,RGBP128F>
            , IPixelFactory<BGRP32,RGBP128F>
            , IPixelFactory<RGBP32,RGBP128F>
            , IPixelFactory<Luminance32F,RGBP128F>
            , IPixelFactory<RGB96F,RGBP128F>
            , IPixelFactory<BGR96F,RGBP128F>
            , IPixelFactory<RGBA128F,RGBP128F>
            , IPixelFactory<BGRA128F,RGBP128F>
            , IPixelFactory<RGBP128F,RGBP128F>
        {
            /// <inheritdoc />
            RGBP128F IPixelFactory<Alpha8,RGBP128F>.From(Alpha8 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<Luminance8,RGBP128F>.From(Luminance8 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGR565,RGBP128F>.From(BGR565 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGRA5551,RGBP128F>.From(BGRA5551 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGRA4444,RGBP128F>.From(BGRA4444 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<RGB24,RGBP128F>.From(RGB24 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGR24,RGBP128F>.From(BGR24 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<RGBA32,RGBP128F>.From(RGBA32 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGRA32,RGBP128F>.From(BGRA32 color) { return new RGBP128F(color); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<ARGB32,RGBP128F>.From(ARGB32 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGRP32,RGBP128F>.From(BGRP32 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<RGBP32,RGBP128F>.From(RGBP32 color) { return new RGBP128F(new BGRA32(color)); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<Luminance32F,RGBP128F>.From(Luminance32F color) { return new RGBP128F(color); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<RGB96F,RGBP128F>.From(RGB96F color) { return new RGBP128F(color); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGR96F,RGBP128F>.From(BGR96F color) { return new RGBP128F(color); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<RGBA128F,RGBP128F>.From(RGBA128F color) { return new RGBP128F(color); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<BGRA128F,RGBP128F>.From(BGRA128F color) { return new RGBP128F(color); }
            /// <inheritdoc />
            RGBP128F IPixelFactory<RGBP128F,RGBP128F>.From(RGBP128F color) { return color; }
            /// <inheritdoc />
            unsafe void IPixelFactory<Alpha8,RGBP128F>.Copy(ReadOnlySpan<Alpha8> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Alpha8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance8,RGBP128F>.Copy(ReadOnlySpan<Luminance8> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance8 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR565,RGBP128F>.Copy(ReadOnlySpan<BGR565> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR565 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA5551,RGBP128F>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA5551 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA4444,RGBP128F>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA4444 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB24,RGBP128F>.Copy(ReadOnlySpan<RGB24> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR24,RGBP128F>.Copy(ReadOnlySpan<BGR24> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR24 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA32,RGBP128F>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA32,RGBP128F>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<ARGB32,RGBP128F>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref ARGB32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRP32,RGBP128F>.Copy(ReadOnlySpan<BGRP32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRP32 sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(new BGRA32(sPtr));
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP32,RGBP128F>.Copy(ReadOnlySpan<RGBP32> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                Vector4Streaming.BytesToUnits(src.AsBytes(), dst.AsSingles());
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<Luminance32F,RGBP128F>.Copy(ReadOnlySpan<Luminance32F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref Luminance32F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGB96F,RGBP128F>.Copy(ReadOnlySpan<RGB96F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGB96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGR96F,RGBP128F>.Copy(ReadOnlySpan<BGR96F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGR96F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBA128F,RGBP128F>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref RGBA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<BGRA128F,RGBP128F>.Copy(ReadOnlySpan<BGRA128F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));

                // Get pointer references.
                ref BGRA128F sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
                ref RGBP128F dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
                int dLen = dst.Length;

                // Old school pointer copy loop.
                while(dLen > 0)
                {
                    --dLen;
                    dPtr = new RGBP128F(sPtr);
                    sPtr = ref Unsafe.Add(ref sPtr, 1);
                    dPtr = ref Unsafe.Add(ref dPtr, 1);
                }
            }
            /// <inheritdoc />
            unsafe void IPixelFactory<RGBP128F,RGBP128F>.Copy(ReadOnlySpan<RGBP128F> src, Span<RGBP128F> dst)
            {
                System.Diagnostics.Debug.Assert(src.Length == dst.Length);
                System.Diagnostics.Debug.Assert(!src.AsBytes().Overlaps(dst.AsBytes()));
                var r = src.TryCopyTo(dst);
                System.Diagnostics.Debug.Assert(r);
            }
        }

    }
}
