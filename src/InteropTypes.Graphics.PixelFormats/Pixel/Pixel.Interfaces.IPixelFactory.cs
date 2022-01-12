
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
        public interface IPixelFactory<in TSrcPixel, out TDstPixel>
            where TSrcPixel: unmanaged
            where TDstPixel: unmanaged
        {
            /// <remarks>
            /// Creates a new value of <typeparamref name="TDstPixel"/> type, which is converted from a <typeparamref name="TSrcPixel"/> value.
            /// </remarks>            
            TDstPixel From(TSrcPixel color);            
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
        }

    }
}
