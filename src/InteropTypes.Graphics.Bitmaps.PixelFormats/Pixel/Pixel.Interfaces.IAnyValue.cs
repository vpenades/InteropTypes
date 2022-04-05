
// GENERATED CODE: using CodeGenUtils.t4
// GENERATED CODE: using Pixel.Constants.t4

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel    
    {           

        partial struct Alpha8 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct Luminance8 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct Luminance16 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct Luminance32F : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGR565 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGRA5551 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGRA4444 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGR24 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct RGB24 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGRA32 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct RGBA32 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct ARGB32 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct RGBP32 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGRP32 : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct RGB96F : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGR96F : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct BGRA128F : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct RGBA128F : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }
        partial struct RGBP128F : IConvertTo
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public TPixel To<TPixel>() where TPixel: unmanaged
            {
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<TPixel>(out var result);
                #else
                var result = default(TPixel);
                #endif
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref result).SetValue(this); return result; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref result).SetValue(this); return result; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo<TPixel>(ref TPixel target) where TPixel: unmanaged
            {
                if (typeof(TPixel) == typeof(Alpha8)) { Unsafe.As<TPixel, Alpha8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance8)) { Unsafe.As<TPixel, Luminance8>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance16)) { Unsafe.As<TPixel, Luminance16>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(Luminance32F)) { Unsafe.As<TPixel, Luminance32F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR565)) { Unsafe.As<TPixel, BGR565>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA5551)) { Unsafe.As<TPixel, BGRA5551>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA4444)) { Unsafe.As<TPixel, BGRA4444>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR24)) { Unsafe.As<TPixel, BGR24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB24)) { Unsafe.As<TPixel, RGB24>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA32)) { Unsafe.As<TPixel, BGRA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA32)) { Unsafe.As<TPixel, RGBA32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(ARGB32)) { Unsafe.As<TPixel, ARGB32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP32)) { Unsafe.As<TPixel, RGBP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRP32)) { Unsafe.As<TPixel, BGRP32>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGB96F)) { Unsafe.As<TPixel, RGB96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGR96F)) { Unsafe.As<TPixel, BGR96F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(BGRA128F)) { Unsafe.As<TPixel, BGRA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBA128F)) { Unsafe.As<TPixel, RGBA128F>(ref target).SetValue(this); return; }
                if (typeof(TPixel) == typeof(RGBP128F)) { Unsafe.As<TPixel, RGBP128F>(ref target).SetValue(this); return; }
                throw new NotImplementedException($"Cannot convert to {typeof(TPixel).Name}");
            }
        }

    }
}
