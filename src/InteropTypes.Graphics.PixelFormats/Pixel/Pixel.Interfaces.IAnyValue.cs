
// GENERATED CODE: using CodeGenUtils.t4
// GENERATED CODE: using Pixel.Constants.t4

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
        public interface IAnyValueGetter<T> where T : unmanaged
        {
            TTo GetValue<TTo>() where TTo : unmanaged, IValueSetter<T>;
        }

        public interface ICopyValueToAny<T> where T : unmanaged
        {
            void CopyTo<TTo>(ref TTo value) where TTo : unmanaged, IValueSetter<T>;
        }

        internal interface _IGetSetCopy<TOther> :
            IValueGetter<TOther>,            
            ICopyValueTo<TOther>
            where TOther : unmanaged
        {
        
        }

        internal interface _IAllGetters
            : _IGetSetCopy<Alpha8>
            , _IGetSetCopy<Luminance8>
            , _IGetSetCopy<Luminance16>
            , _IGetSetCopy<Luminance32F>
            , _IGetSetCopy<BGR565>
            , _IGetSetCopy<BGRA5551>
            , _IGetSetCopy<BGRA4444>
            , _IGetSetCopy<BGR24>
            , _IGetSetCopy<RGB24>
            , _IGetSetCopy<BGRA32>
            , _IGetSetCopy<RGBA32>
            , _IGetSetCopy<ARGB32>
            , _IGetSetCopy<RGBP32>
            , _IGetSetCopy<BGRP32>
            , _IGetSetCopy<RGB96F>
            , _IGetSetCopy<BGR96F>
            , _IGetSetCopy<BGRA128F>
            , _IGetSetCopy<RGBA128F>
            , _IGetSetCopy<RGBP128F>
        {
        }



        partial struct Alpha8
            : _IAllGetters
            , IAnyValueGetter<Alpha8>
            , ICopyValueToAny<Alpha8>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<Alpha8>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<Alpha8> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct Luminance8
            : _IAllGetters
            , IAnyValueGetter<Luminance8>
            , ICopyValueToAny<Luminance8>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<Luminance8>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<Luminance8> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct Luminance16
            : _IAllGetters
            , IAnyValueGetter<Luminance16>
            , ICopyValueToAny<Luminance16>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<Luminance16>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<Luminance16> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct Luminance32F
            : _IAllGetters
            , IAnyValueGetter<Luminance32F>
            , ICopyValueToAny<Luminance32F>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<Luminance32F>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<Luminance32F> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGR565
            : _IAllGetters
            , IAnyValueGetter<BGR565>
            , ICopyValueToAny<BGR565>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGR565>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGR565> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGRA5551
            : _IAllGetters
            , IAnyValueGetter<BGRA5551>
            , ICopyValueToAny<BGRA5551>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGRA5551>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGRA5551> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGRA4444
            : _IAllGetters
            , IAnyValueGetter<BGRA4444>
            , ICopyValueToAny<BGRA4444>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGRA4444>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGRA4444> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGR24
            : _IAllGetters
            , IAnyValueGetter<BGR24>
            , ICopyValueToAny<BGR24>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGR24>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGR24> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct RGB24
            : _IAllGetters
            , IAnyValueGetter<RGB24>
            , ICopyValueToAny<RGB24>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<RGB24>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<RGB24> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGRA32
            : _IAllGetters
            , IAnyValueGetter<BGRA32>
            , ICopyValueToAny<BGRA32>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGRA32>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGRA32> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct RGBA32
            : _IAllGetters
            , IAnyValueGetter<RGBA32>
            , ICopyValueToAny<RGBA32>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<RGBA32>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<RGBA32> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct ARGB32
            : _IAllGetters
            , IAnyValueGetter<ARGB32>
            , ICopyValueToAny<ARGB32>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<ARGB32>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<ARGB32> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct RGBP32
            : _IAllGetters
            , IAnyValueGetter<RGBP32>
            , ICopyValueToAny<RGBP32>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<RGBP32>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<RGBP32> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGRP32
            : _IAllGetters
            , IAnyValueGetter<BGRP32>
            , ICopyValueToAny<BGRP32>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGRP32>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGRP32> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct RGB96F
            : _IAllGetters
            , IAnyValueGetter<RGB96F>
            , ICopyValueToAny<RGB96F>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<RGB96F>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<RGB96F> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGR96F
            : _IAllGetters
            , IAnyValueGetter<BGR96F>
            , ICopyValueToAny<BGR96F>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGR96F>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGR96F> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct BGRA128F
            : _IAllGetters
            , IAnyValueGetter<BGRA128F>
            , ICopyValueToAny<BGRA128F>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<BGRA128F>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<BGRA128F> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct RGBA128F
            : _IAllGetters
            , IAnyValueGetter<RGBA128F>
            , ICopyValueToAny<RGBA128F>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<RGBA128F>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<RGBA128F> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }
        partial struct RGBP128F
            : _IAllGetters
            , IAnyValueGetter<RGBP128F>
            , ICopyValueToAny<RGBP128F>
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TTo GetValue<TTo>() where TTo:unmanaged,IValueSetter<RGBP128F>
            {
                var tmp = default(TTo); tmp.SetValue(this); return tmp;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<T>(ref T value) where T:unmanaged,IValueSetter<RGBP128F> { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Alpha8 IValueGetter<Alpha8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Alpha8>(out var t);
                #else
                var t = default(Alpha8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Alpha8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance8 IValueGetter<Luminance8>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance8>(out var t);
                #else
                var t = default(Luminance8);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance8 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance16 IValueGetter<Luminance16>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance16>(out var t);
                #else
                var t = default(Luminance16);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance16 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            Luminance32F IValueGetter<Luminance32F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<Luminance32F>(out var t);
                #else
                var t = default(Luminance32F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref Luminance32F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR565 IValueGetter<BGR565>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR565>(out var t);
                #else
                var t = default(BGR565);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR565 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA5551 IValueGetter<BGRA5551>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA5551>(out var t);
                #else
                var t = default(BGRA5551);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA5551 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA4444 IValueGetter<BGRA4444>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA4444>(out var t);
                #else
                var t = default(BGRA4444);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA4444 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR24 IValueGetter<BGR24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var t);
                #else
                var t = default(BGR24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB24 IValueGetter<RGB24>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var t);
                #else
                var t = default(RGB24);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB24 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA32 IValueGetter<BGRA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var t);
                #else
                var t = default(BGRA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA32 IValueGetter<RGBA32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var t);
                #else
                var t = default(RGBA32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            ARGB32 IValueGetter<ARGB32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var t);
                #else
                var t = default(ARGB32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref ARGB32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP32 IValueGetter<RGBP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var t);
                #else
                var t = default(RGBP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRP32 IValueGetter<BGRP32>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var t);
                #else
                var t = default(BGRP32);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRP32 value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGB96F IValueGetter<RGB96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB96F>(out var t);
                #else
                var t = default(RGB96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGB96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGR96F IValueGetter<BGR96F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR96F>(out var t);
                #else
                var t = default(BGR96F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGR96F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            BGRA128F IValueGetter<BGRA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA128F>(out var t);
                #else
                var t = default(BGRA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref BGRA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBA128F IValueGetter<RGBA128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA128F>(out var t);
                #else
                var t = default(RGBA128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBA128F value) { value.SetValue(this); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            RGBP128F IValueGetter<RGBP128F>.GetValue()
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP128F>(out var t);
                #else
                var t = default(RGBP128F);
                #endif
                t.SetValue(this); return t;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref RGBP128F value) { value.SetValue(this); }
        }

    }
}
