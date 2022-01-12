

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
        /// <summary>
        /// Delegate that linearly blends left and right using amount as control.
        /// </summary>
        /// <typeparam name="TSrc">The type of the sources to blend.</typeparam>
        /// <typeparam name="TDst">The type of the blended target.</typeparam>
        /// <typeparam name="TAmount"></typeparam>
        /// <param name="left">The 1st source.</param>
        /// <param name="right">The 2nd source.</param>
        /// <param name="amount">The controller. Must be either <see cref="int"/> or <see cref="float"/>.</param>
        /// <param name="dst">The destination.</param>
        public delegate void SpanLerpCallback<TSrc,TDst,TAmount>(ReadOnlySpan<TSrc> left ,ReadOnlySpan<TSrc> right, TAmount amount, Span<TDst> dst);


        public static SpanLerpCallback<TSrc,TDst,TAmount> GetSpanLerpCallback<TSrc,TDst,TAmount>()
            where TSrc : unmanaged
            where TDst : unmanaged
            where TAmount : unmanaged, IConvertible
        {
            var instance = default(TSrc) as ISpanLerpDelegateProvider<TSrc,TDst,TAmount>;
            if (instance != null) return instance.GetSpanLerpDelegate();

            instance = default(TDst) as ISpanLerpDelegateProvider<TSrc, TDst, TAmount>;
            if (instance != null) return instance.GetSpanLerpDelegate();

            throw new NotImplementedException();
        }


        interface ISpanLerpDelegateProvider<TSrc,TDst,TAmount>
            where TSrc : unmanaged
            where TDst : unmanaged
            where TAmount : unmanaged, IConvertible
        {
            SpanLerpCallback<TSrc,TDst,TAmount> GetSpanLerpDelegate();
        }

        public interface ILerpToBGRP32<TSrc>
        {
            BGRP32 LerpToBGRP32(TSrc other, int amount);
        }

        interface ILerpToBGRP128F<TSrc>
        {
            BGRP128F LerpToBGRP32(TSrc other, float amount);
        }        

        partial struct Alpha8
            : ISpanLerpDelegateProvider<Alpha8,Alpha8,int>
            , ISpanLerpDelegateProvider<Alpha8,Alpha8,float>
            , ILerpToBGRP32<Alpha8>
        {
            SpanLerpCallback<Alpha8,Alpha8,int> ISpanLerpDelegateProvider<Alpha8,Alpha8,int>.GetSpanLerpDelegate() { return Lerp; }
            SpanLerpCallback<Alpha8,Alpha8,float> ISpanLerpDelegateProvider<Alpha8,Alpha8,float>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<Alpha8> left, ReadOnlySpan<Alpha8> right, float amount, Span<Alpha8> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<Alpha8> left, ReadOnlySpan<Alpha8> right, int amount, Span<Alpha8> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static Alpha8 Lerp(Alpha8 left, Alpha8 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var A = (left.A * lx + right.A * rx) / 16384;
                return new Alpha8(A);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(Alpha8 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate rgb
                var a = (this.A * lx + other.A * rx) / 16384;
                return new BGRP32(0, 0, 0, a);
            }
        }
        partial struct Luminance8
            : ISpanLerpDelegateProvider<Luminance8,Luminance8,int>
        {
            SpanLerpCallback<Luminance8,Luminance8,int> ISpanLerpDelegateProvider<Luminance8,Luminance8,int>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<Luminance8> left, ReadOnlySpan<Luminance8> right, float amount, Span<Luminance8> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<Luminance8> left, ReadOnlySpan<Luminance8> right, int amount, Span<Luminance8> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static Luminance8 Lerp(Luminance8 left, Luminance8 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var L = (left.L * lx + right.L * rx) / 16384;
                return new Luminance8(L);
            }
        }
        partial struct BGR24
            : ISpanLerpDelegateProvider<BGR24,BGR24,int>
            , ILerpToBGRP32<BGR24>
        {
            SpanLerpCallback<BGR24,BGR24,int> ISpanLerpDelegateProvider<BGR24,BGR24,int>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGR24> left, ReadOnlySpan<BGR24> right, float amount, Span<BGR24> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGR24> left, ReadOnlySpan<BGR24> right, int amount, Span<BGR24> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(BGR24 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate rgb
                var r = (this.R * lx + other.R * rx) / 16384;
                var g = (this.G * lx + other.G * rx) / 16384;
                var b = (this.B * lx + other.B * rx) / 16384;
                return new BGRP32(r, g, b, 255);
            }
        }
        partial struct RGB24
            : ISpanLerpDelegateProvider<RGB24,RGB24,int>
            , ILerpToBGRP32<RGB24>
        {
            SpanLerpCallback<RGB24,RGB24,int> ISpanLerpDelegateProvider<RGB24,RGB24,int>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGB24> left, ReadOnlySpan<RGB24> right, float amount, Span<RGB24> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGB24> left, ReadOnlySpan<RGB24> right, int amount, Span<RGB24> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(RGB24 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate rgb
                var r = (this.R * lx + other.R * rx) / 16384;
                var g = (this.G * lx + other.G * rx) / 16384;
                var b = (this.B * lx + other.B * rx) / 16384;
                return new BGRP32(r, g, b, 255);
            }
        }
        partial struct BGR565
            : ISpanLerpDelegateProvider<BGR565,BGR565,int>
            , ILerpToBGRP32<BGR565>
        {
            SpanLerpCallback<BGR565,BGR565,int> ISpanLerpDelegateProvider<BGR565,BGR565,int>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGR565> left, ReadOnlySpan<BGR565> right, float amount, Span<BGR565> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGR565> left, ReadOnlySpan<BGR565> right, int amount, Span<BGR565> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGR565 Lerp(BGR565 left, BGR565 right, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);
                var R = (left.R * lx + right.R * rx) / 16384;
                var G = (left.G * lx + right.G * rx) / 16384;
                var B = (left.B * lx + right.B * rx) / 16384;
                return new BGR565(R, G, B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(BGR565 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate rgb
                var r = (this.R * lx + other.R * rx) / 16384;
                var g = (this.G * lx + other.G * rx) / 16384;
                var b = (this.B * lx + other.B * rx) / 16384;
                return new BGRP32(r, g, b, 255);
            }
        }
        partial struct BGRA5551
        {

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGRA5551> left, ReadOnlySpan<BGRA5551> right, float amount, Span<BGRA5551> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(BGRA5551 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate final alpha
                int a = (this.A * lx + other.A * rx) / 16384;
                if (a == 0) return default;

                // calculate rgb
                lx *= this.A;
                rx *= other.A;
                var r = (this.R * lx + other.R * rx) / (16384 * 255);
                var g = (this.G * lx + other.G * rx) / (16384 * 255);
                var b = (this.B * lx + other.B * rx) / (16384 * 255);
                return new BGRP32(r, g, b, a);
            }
        }
        partial struct BGRA4444
        {

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGRA4444> left, ReadOnlySpan<BGRA4444> right, float amount, Span<BGRA4444> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(BGRA4444 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate final alpha
                int a = (this.A * lx + other.A * rx) / 16384;
                if (a == 0) return default;

                // calculate rgb
                lx *= this.A;
                rx *= other.A;
                var r = (this.R * lx + other.R * rx) / (16384 * 255);
                var g = (this.G * lx + other.G * rx) / (16384 * 255);
                var b = (this.B * lx + other.B * rx) / (16384 * 255);
                return new BGRP32(r, g, b, a);
            }
        }
        partial struct RGBA32
            : ISpanLerpDelegateProvider<RGBA32,RGBA32,int>
            , ILerpToBGRP32<RGBA32>
        {
            SpanLerpCallback<RGBA32,RGBA32,int> ISpanLerpDelegateProvider<RGBA32,RGBA32,int>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGBA32> left, ReadOnlySpan<RGBA32> right, float amount, Span<RGBA32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(RGBA32 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate final alpha
                int a = (this.A * lx + other.A * rx) / 16384;
                if (a == 0) return default;

                // calculate rgb
                lx *= this.A;
                rx *= other.A;
                var r = (this.R * lx + other.R * rx) / (16384 * 255);
                var g = (this.G * lx + other.G * rx) / (16384 * 255);
                var b = (this.B * lx + other.B * rx) / (16384 * 255);
                return new BGRP32(r, g, b, a);
            }
        }
        partial struct BGRA32
            : ISpanLerpDelegateProvider<BGRA32,BGRA32,int>
            , ILerpToBGRP32<BGRA32>
        {
            SpanLerpCallback<BGRA32,BGRA32,int> ISpanLerpDelegateProvider<BGRA32,BGRA32,int>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGRA32> left, ReadOnlySpan<BGRA32> right, float amount, Span<BGRA32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(BGRA32 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate final alpha
                int a = (this.A * lx + other.A * rx) / 16384;
                if (a == 0) return default;

                // calculate rgb
                lx *= this.A;
                rx *= other.A;
                var r = (this.R * lx + other.R * rx) / (16384 * 255);
                var g = (this.G * lx + other.G * rx) / (16384 * 255);
                var b = (this.B * lx + other.B * rx) / (16384 * 255);
                return new BGRP32(r, g, b, a);
            }
        }
        partial struct ARGB32
            : ISpanLerpDelegateProvider<ARGB32,ARGB32,int>
            , ILerpToBGRP32<ARGB32>
        {
            SpanLerpCallback<ARGB32,ARGB32,int> ISpanLerpDelegateProvider<ARGB32,ARGB32,int>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<ARGB32> left, ReadOnlySpan<ARGB32> right, float amount, Span<ARGB32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(ARGB32 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate final alpha
                int a = (this.A * lx + other.A * rx) / 16384;
                if (a == 0) return default;

                // calculate rgb
                lx *= this.A;
                rx *= other.A;
                var r = (this.R * lx + other.R * rx) / (16384 * 255);
                var g = (this.G * lx + other.G * rx) / (16384 * 255);
                var b = (this.B * lx + other.B * rx) / (16384 * 255);
                return new BGRP32(r, g, b, a);
            }
        }
        partial struct RGBP32
            : ILerpToBGRP32<RGBP32>
        {

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGBP32> left, ReadOnlySpan<RGBP32> right, float amount, Span<RGBP32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGBP32> left, ReadOnlySpan<RGBP32> right, int amount, Span<RGBP32> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(RGBP32 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate rgb
                var r = (this.PreR * lx + other.PreR * rx) / 16384;
                var g = (this.PreG * lx + other.PreG * rx) / 16384;
                var b = (this.PreB * lx + other.PreB * rx) / 16384;
                var a = (this.A * lx + other.A * rx) / 16384;
                return new BGRP32(r, g, b, a);
            }
        }
        partial struct BGRP32
            : ILerpToBGRP32<BGRP32>
        {

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGRP32> left, ReadOnlySpan<BGRP32> right, float amount, Span<BGRP32> dst)
            {
                Lerp(left,right,(int)(amount * 16384f),dst);
            }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGRP32> left, ReadOnlySpan<BGRP32> right, int amount, Span<BGRP32> dst)
            {
                Vector4Streaming.Lerp(left.AsBytes(), right.AsBytes(), amount, dst.AsBytes());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
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

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 LerpToBGRP32(BGRP32 other, int rx)
            {

                // calculate quantized weights
                var lx = 16384 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 16384);

                // calculate rgb
                var r = (this.PreR * lx + other.PreR * rx) / 16384;
                var g = (this.PreG * lx + other.PreG * rx) / 16384;
                var b = (this.PreB * lx + other.PreB * rx) / 16384;
                var a = (this.A * lx + other.A * rx) / 16384;
                return new BGRP32(r, g, b, a);
            }
        }
        partial struct RGB96F
            : ISpanLerpDelegateProvider<RGB96F,RGB96F,float>
        {
            SpanLerpCallback<RGB96F,RGB96F,float> ISpanLerpDelegateProvider<RGB96F,RGB96F,float>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGB96F> left, ReadOnlySpan<RGB96F> right, float amount, Span<RGB96F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGB96F Lerp(RGB96F left, RGB96F right, float rx)
            {

                // calculate quantized weights
                var lx = 1f - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 1f);
                var R = left.R * lx + right.R * rx;
                var G = left.G * lx + right.G * rx;
                var B = left.B * lx + right.B * rx;
                return new RGB96F(R, G, B);
            }
        }
        partial struct BGR96F
            : ISpanLerpDelegateProvider<BGR96F,BGR96F,float>
        {
            SpanLerpCallback<BGR96F,BGR96F,float> ISpanLerpDelegateProvider<BGR96F,BGR96F,float>.GetSpanLerpDelegate() { return Lerp; }

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGR96F> left, ReadOnlySpan<BGR96F> right, float amount, Span<BGR96F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGR96F Lerp(BGR96F left, BGR96F right, float rx)
            {

                // calculate quantized weights
                var lx = 1f - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 1f);
                var B = left.B * lx + right.B * rx;
                var G = left.G * lx + right.G * rx;
                var R = left.R * lx + right.R * rx;
                return new BGR96F(B, G, R);
            }
        }
        partial struct RGBA128F
        {

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGBA128F> left, ReadOnlySpan<RGBA128F> right, float amount, Span<RGBA128F> dst)
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

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBA128F Lerp(in RGBA128F p00, in RGBA128F p01, float rx)
            {

                // calculate quantized weights
                var lx = 1f - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 1f);

                // calculate final alpha
                var a = p00.A * lx + p01.A * rx;
                if (a == 0) return default;

                // calculate premultiplied RGB
                lx *= p00.A;
                rx *= p01.A;
                var v = (p00.RGBA * lx + p01.RGBA * rx) / a;
                v.W = a;

                // unpremultiply RGB
                return new RGBA128F(v.X, v.Y, v.Z, a);
            }
        }
        partial struct BGRA128F
        {

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<BGRA128F> left, ReadOnlySpan<BGRA128F> right, float amount, Span<BGRA128F> dst)
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

            /// <summary>
            /// Lerps two values
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRA128F Lerp(in BGRA128F p00, in BGRA128F p01, float rx)
            {

                // calculate quantized weights
                var lx = 1f - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == 1f);

                // calculate final alpha
                var a = p00.A * lx + p01.A * rx;
                if (a == 0) return default;

                // calculate premultiplied RGB
                lx *= p00.A;
                rx *= p01.A;
                var v = (p00.BGRA * lx + p01.BGRA * rx) / a;
                v.W = a;

                // unpremultiply RGB
                return new BGRA128F(v.X, v.Y, v.Z, a);
            }
        }
        partial struct RGBP128F
        {

            /// <summary>
            /// Lerps left and right into dst.
            /// </summary>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(ReadOnlySpan<RGBP128F> left, ReadOnlySpan<RGBP128F> right, float amount, Span<RGBP128F> dst)
            {
                Vector4Streaming.Lerp(left.AsSingles(), right.AsSingles(), amount, dst.AsSingles());
            }
        }

    }
}
