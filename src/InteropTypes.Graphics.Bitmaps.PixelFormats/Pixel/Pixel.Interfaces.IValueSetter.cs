
// GENERATED CODE: using CodeGenUtils.t4
// GENERATED CODE: using Pixel.Constants.t4

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Numerics;

namespace InteropTypes.Graphics.Bitmaps
{

    partial class Pixel    
    {        

        internal interface _IAllSetters
            : IValueSetter<Alpha8>
            , IValueSetter<Luminance8>
            , IValueSetter<Luminance16>
            , IValueSetter<Luminance32F>
            , IValueSetter<BGR565>
            , IValueSetter<BGRA5551>
            , IValueSetter<BGRA4444>
            , IValueSetter<BGR24>
            , IValueSetter<RGB24>
            , IValueSetter<BGRA32>
            , IValueSetter<RGBA32>
            , IValueSetter<ARGB32>
            , IValueSetter<RGBP32>
            , IValueSetter<BGRP32>
            , IValueSetter<RGB96F>
            , IValueSetter<BGR96F>
            , IValueSetter<BGRA128F>
            , IValueSetter<RGBA128F>
            , IValueSetter<RGBP128F>
        {
        }



        partial struct Alpha8 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.A = (Byte)(value.A*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.A = (Byte)(value.A*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this.A = (Byte)(value.A*255f);
            }
        }
        partial struct Luminance8 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.L = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                this.L = (Byte)(value.L >> 8);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                this.L = (Byte)(value.L*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.L = _FromRGB((uint)value.R,(uint)value.G,(uint)value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.L = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.L = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.L = _FromRGB((uint)value.R,(uint)value.G,(uint)value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.L = _FromRGB((uint)value.R,(uint)value.G,(uint)value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }
        }
        partial struct Luminance32F : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.L = value.A * Reciprocal255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.L = value.L * Reciprocal255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                this.L = value.L * Reciprocal65535;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.L = _FromRGB(value.R, value.G, value.B) * Reciprocal255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.L = _FromRGBA(value.R, value.G, value.B, value.A) * Reciprocal65025;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.L = _FromRGBA(value.R, value.G, value.B, value.A) * Reciprocal65025;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.L = _FromRGB(value.R, value.G, value.B) * Reciprocal255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.L = _FromRGB(value.R, value.G, value.B) * Reciprocal255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.L = _FromRGBA(value.R, value.G, value.B, value.A) * Reciprocal65025;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.L = _FromRGBA(value.R, value.G, value.B, value.A) * Reciprocal65025;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.L = _FromRGBA(value.R, value.G, value.B, value.A) * Reciprocal65025;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                this.L = _FromRGB(value.PreR, value.PreG, value.PreB) * Reciprocal255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                this.L = _FromRGB(value.PreR, value.PreG, value.PreB) * Reciprocal255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.L = _FromRGB(value.R,value.G,value.B);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.L = _FromRGBA(value.R,value.G,value.B,value.A);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.L = _FromRGBA(value.R,value.G,value.B,value.A);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this.L = _FromRGB(value.PreR,value.PreG,value.PreB);
            }
        }
        partial struct BGR24 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 255;
                this.G = 255;
                this.R = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                value.L >>= 8; // shift down
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                value.L *= 255f; // shift up
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }
        }
        partial struct RGB24 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 255;
                this.G = 255;
                this.R = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                value.L >>= 8; // shift down
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                value.L *= 255f; // shift up
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this.B = (Byte)(value.B*255f);
                this.G = (Byte)(value.G*255f);
                this.R = (Byte)(value.R*255f);
            }
        }
        partial struct BGRA32 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 255;
                this.G = 255;
                this.R = 255;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                value.L >>= 8; // shift down
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                value.L *= 255f; // shift up
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                value.BGR *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                value.RGB *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                value.BGRA *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                value.RGBA *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                throw new NotImplementedException("Setting RGBP128F not implemented.");
            }
        }
        partial struct RGBA32 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 255;
                this.G = 255;
                this.R = 255;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                value.L >>= 8; // shift down
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                value.L *= 255f; // shift up
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                value.BGR *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                value.RGB *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                value.BGRA *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                value.RGBA *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                throw new NotImplementedException("Setting RGBP128F not implemented.");
            }
        }
        partial struct ARGB32 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 255;
                this.G = 255;
                this.R = 255;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                value.L >>= 8; // shift down
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                value.L *= 255f; // shift up
                this.B = (Byte)value.L;
                this.G = (Byte)value.L;
                this.R = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                value.BGR *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                value.RGB *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                value.BGRA *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                value.RGBA *= 255f; // shift up
                this.B = (Byte)value.B;
                this.G = (Byte)value.G;
                this.R = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                uint rcpA = value.A == 0 ? 0 : (65536u * 255u) / (uint)value.A;
                B = (Byte)((value.PreB * rcpA + 255u) >> 16);
                G = (Byte)((value.PreG * rcpA + 255u) >> 16);
                R = (Byte)((value.PreR * rcpA + 255u) >> 16);
                A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                throw new NotImplementedException("Setting RGBP128F not implemented.");
            }
        }
        partial struct BGRP32 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.PreB = value.A;
                this.PreG = value.A;
                this.PreR = value.A;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.PreB = value.L;
                this.PreG = value.L;
                this.PreR = value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                value.L >>= 8; // shift down
                this.PreB = (Byte)value.L;
                this.PreG = (Byte)value.L;
                this.PreR = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                value.L *= 255f; // shift up
                this.PreB = (Byte)value.L;
                this.PreG = (Byte)value.L;
                this.PreR = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.PreB = value.B;
                this.PreG = value.G;
                this.PreR = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.PreB = value.B;
                this.PreG = value.G;
                this.PreR = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.PreB = (Byte)(value.B*255f);
                this.PreG = (Byte)(value.G*255f);
                this.PreR = (Byte)(value.R*255f);
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.PreB = (Byte)(value.B*255f);
                this.PreG = (Byte)(value.G*255f);
                this.PreR = (Byte)(value.R*255f);
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                this.PreB = value.PreB;
                this.PreG = value.PreG;
                this.PreR = value.PreR;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                value.BGR *= value.A; // premul
                value.BGRA *= 255f; // shift up
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                value.RGB *= value.A; // premul
                value.RGBA *= 255f; // shift up
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                value.RGBP *= 255f; // shift up
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = (Byte)value.A;
            }
        }
        partial struct RGBP32 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.PreB = value.A;
                this.PreG = value.A;
                this.PreR = value.A;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.PreB = value.L;
                this.PreG = value.L;
                this.PreR = value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                value.L >>= 8; // shift down
                this.PreB = (Byte)value.L;
                this.PreG = (Byte)value.L;
                this.PreR = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                value.L *= 255f; // shift up
                this.PreB = (Byte)value.L;
                this.PreG = (Byte)value.L;
                this.PreR = (Byte)value.L;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.PreB = value.B;
                this.PreG = value.G;
                this.PreR = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.PreB = value.B;
                this.PreG = value.G;
                this.PreR = value.R;
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.PreB = (Byte)(value.B*255f);
                this.PreG = (Byte)(value.G*255f);
                this.PreR = (Byte)(value.R*255f);
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.PreB = (Byte)(value.B*255f);
                this.PreG = (Byte)(value.G*255f);
                this.PreR = (Byte)(value.R*255f);
                this.A = 255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                uint fwdA = (uint)value.A * 257u;
                this.PreB = (Byte)(((uint)value.B * fwdA + 255u) >> 16);
                this.PreG = (Byte)(((uint)value.G * fwdA + 255u) >> 16);
                this.PreR = (Byte)(((uint)value.R * fwdA + 255u) >> 16);
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                this.PreB = value.PreB;
                this.PreG = value.PreG;
                this.PreR = value.PreR;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                value.BGR *= value.A; // premul
                value.BGRA *= 255f; // shift up
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                value.RGB *= value.A; // premul
                value.RGBA *= 255f; // shift up
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = (Byte)value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                value.RGBP *= 255f; // shift up
                this.PreB = (Byte)value.B;
                this.PreG = (Byte)value.G;
                this.PreR = (Byte)value.R;
                this.A = (Byte)value.A;
            }
        }
        partial struct BGR96F : _IAllSetters
        {
            const float __RCP255 = 1f / 255f;

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 1;
                this.G = 1;
                this.R = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                throw new NotImplementedException("Setting Luminance16 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                BGR *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
            }
        }
        partial struct RGB96F : _IAllSetters
        {
            const float __RCP255 = 1f / 255f;

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 1;
                this.G = 1;
                this.R = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                throw new NotImplementedException("Setting Luminance16 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                RGB *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
            }
        }
        partial struct BGRA128F : _IAllSetters
        {
            const float __RCP255 = 1f / 255f;

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 255;
                this.G = 255;
                this.R = 255;
                this.A = value.A;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 255;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 65535;
                this.BGRA /= 65535;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.BGRA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                if (value.A == 0) { this = default; return; }
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                this.A = value.A;
                this.BGRA *= __RCP255;
                BGR = Vector3.Min(Vector3.One, BGR / this.A);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                if (value.A == 0) { this = default; return; }
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                this.A = value.A;
                this.BGRA *= __RCP255;
                BGR = Vector3.Min(Vector3.One, BGR / this.A);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                if (value.A == 0) { this = default; return; }
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                this.A = value.A;
                BGR = Vector3.Min(Vector3.One, BGR / this.A);
            }
        }
        partial struct RGBA128F : _IAllSetters
        {
            const float __RCP255 = 1f / 255f;

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                this.B = 255;
                this.G = 255;
                this.R = 255;
                this.A = value.A;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 255;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 65535;
                this.RGBA /= 65535;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                this.B = value.L;
                this.G = value.L;
                this.R = value.L;
                this.A = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 255;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
                this.RGBA *= __RCP255;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = 1;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                this.B = value.B;
                this.G = value.G;
                this.R = value.R;
                this.A = value.A;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                if (value.A == 0) { this = default; return; }
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                this.A = value.A;
                this.RGBA *= __RCP255;
                RGB = Vector3.Min(Vector3.One, RGB / this.A);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                if (value.A == 0) { this = default; return; }
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                this.A = value.A;
                this.RGBA *= __RCP255;
                RGB = Vector3.Min(Vector3.One, RGB / this.A);
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                if (value.A == 0) { this = default; return; }
                this.B = value.PreB;
                this.G = value.PreG;
                this.R = value.PreR;
                this.A = value.A;
                RGB = Vector3.Min(Vector3.One, RGB / this.A);
            }
        }


        //---------------------------------------------------------- Not implemented


        partial struct Luminance16 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                throw new NotImplementedException("Setting Alpha8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                throw new NotImplementedException("Setting Luminance8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                throw new NotImplementedException("Setting Luminance32F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                throw new NotImplementedException("Setting BGR565 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                throw new NotImplementedException("Setting BGRA5551 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                throw new NotImplementedException("Setting BGRA4444 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                throw new NotImplementedException("Setting BGR24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                throw new NotImplementedException("Setting RGB24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                throw new NotImplementedException("Setting BGRA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                throw new NotImplementedException("Setting RGBA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                throw new NotImplementedException("Setting ARGB32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                throw new NotImplementedException("Setting RGBP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                throw new NotImplementedException("Setting BGRP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                throw new NotImplementedException("Setting RGB96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                throw new NotImplementedException("Setting BGR96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                throw new NotImplementedException("Setting BGRA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                throw new NotImplementedException("Setting RGBA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                throw new NotImplementedException("Setting RGBP128F not implemented.");
            }
        }
        partial struct BGR565 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                throw new NotImplementedException("Setting Alpha8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                throw new NotImplementedException("Setting Luminance8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                throw new NotImplementedException("Setting Luminance16 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                throw new NotImplementedException("Setting Luminance32F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                throw new NotImplementedException("Setting BGRA5551 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                throw new NotImplementedException("Setting BGRA4444 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                throw new NotImplementedException("Setting BGR24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                throw new NotImplementedException("Setting RGB24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                throw new NotImplementedException("Setting BGRA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                throw new NotImplementedException("Setting RGBA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                throw new NotImplementedException("Setting ARGB32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                throw new NotImplementedException("Setting RGBP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                throw new NotImplementedException("Setting BGRP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                throw new NotImplementedException("Setting RGB96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                throw new NotImplementedException("Setting BGR96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                throw new NotImplementedException("Setting BGRA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                throw new NotImplementedException("Setting RGBA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                throw new NotImplementedException("Setting RGBP128F not implemented.");
            }
        }
        partial struct BGRA5551 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                throw new NotImplementedException("Setting Alpha8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                throw new NotImplementedException("Setting Luminance8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                throw new NotImplementedException("Setting Luminance16 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                throw new NotImplementedException("Setting Luminance32F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                throw new NotImplementedException("Setting BGR565 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                throw new NotImplementedException("Setting BGRA4444 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                throw new NotImplementedException("Setting BGR24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                throw new NotImplementedException("Setting RGB24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                throw new NotImplementedException("Setting BGRA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                throw new NotImplementedException("Setting RGBA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                throw new NotImplementedException("Setting ARGB32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                throw new NotImplementedException("Setting RGBP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                throw new NotImplementedException("Setting BGRP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                throw new NotImplementedException("Setting RGB96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                throw new NotImplementedException("Setting BGR96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                throw new NotImplementedException("Setting BGRA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                throw new NotImplementedException("Setting RGBA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                throw new NotImplementedException("Setting RGBP128F not implemented.");
            }
        }
        partial struct BGRA4444 : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                throw new NotImplementedException("Setting Alpha8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                throw new NotImplementedException("Setting Luminance8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                throw new NotImplementedException("Setting Luminance16 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                throw new NotImplementedException("Setting Luminance32F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                throw new NotImplementedException("Setting BGR565 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                throw new NotImplementedException("Setting BGRA5551 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                throw new NotImplementedException("Setting BGR24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                throw new NotImplementedException("Setting RGB24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                throw new NotImplementedException("Setting BGRA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                throw new NotImplementedException("Setting RGBA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                throw new NotImplementedException("Setting ARGB32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                throw new NotImplementedException("Setting RGBP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                throw new NotImplementedException("Setting BGRP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                throw new NotImplementedException("Setting RGB96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                throw new NotImplementedException("Setting BGR96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                throw new NotImplementedException("Setting BGRA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                throw new NotImplementedException("Setting RGBA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                throw new NotImplementedException("Setting RGBP128F not implemented.");
            }
        }
        partial struct RGBP128F : _IAllSetters
        {

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP128F value)
            {
                this = value;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Alpha8 value)
            {
                throw new NotImplementedException("Setting Alpha8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance8 value)
            {
                throw new NotImplementedException("Setting Luminance8 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance16 value)
            {
                throw new NotImplementedException("Setting Luminance16 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(Luminance32F value)
            {
                throw new NotImplementedException("Setting Luminance32F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR565 value)
            {
                throw new NotImplementedException("Setting BGR565 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA5551 value)
            {
                throw new NotImplementedException("Setting BGRA5551 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA4444 value)
            {
                throw new NotImplementedException("Setting BGRA4444 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR24 value)
            {
                throw new NotImplementedException("Setting BGR24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB24 value)
            {
                throw new NotImplementedException("Setting RGB24 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA32 value)
            {
                throw new NotImplementedException("Setting BGRA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA32 value)
            {
                throw new NotImplementedException("Setting RGBA32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(ARGB32 value)
            {
                throw new NotImplementedException("Setting ARGB32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBP32 value)
            {
                throw new NotImplementedException("Setting RGBP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRP32 value)
            {
                throw new NotImplementedException("Setting BGRP32 not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGB96F value)
            {
                throw new NotImplementedException("Setting RGB96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGR96F value)
            {
                throw new NotImplementedException("Setting BGR96F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(BGRA128F value)
            {
                throw new NotImplementedException("Setting BGRA128F not implemented.");
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValue(RGBA128F value)
            {
                throw new NotImplementedException("Setting RGBA128F not implemented.");
            }
        }

    }
}

