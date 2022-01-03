
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InteropBitmaps
{
    partial class Pixel    
    {
        public interface IPixelConvertible<TPixel>
        {
            TPixel ToPixel();
        }

        partial struct Alpha8
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return this; }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(this.A); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct Luminance8
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(this.L); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return this; }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct Luminance16
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct Luminance32F
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return this; }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGR565
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGR24
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGB24
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return this; }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA5551
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA4444
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return this; }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA32
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(this); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(this); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(this); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(this); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(this); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(this); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(this); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(this); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return this; }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(this); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(this); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(this); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBA32
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct ARGB32
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBP32
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return this; }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRP32
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGB96F
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return this; }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGR96F
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA128F
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBA128F
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(this); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(this); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(this); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(this); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBP128F
            : IPixelConvertible<Alpha8>
            , IPixelConvertible<Luminance8>
            , IPixelConvertible<BGR565>
            , IPixelConvertible<BGRA5551>
            , IPixelConvertible<BGRA4444>
            , IPixelConvertible<RGB24>
            , IPixelConvertible<BGR24>
            , IPixelConvertible<RGBA32>
            , IPixelConvertible<BGRA32>
            , IPixelConvertible<ARGB32>
            , IPixelConvertible<BGRP32>
            , IPixelConvertible<RGBP32>
            , IPixelConvertible<Luminance32F>
            , IPixelConvertible<RGB96F>
            , IPixelConvertible<BGR96F>
            , IPixelConvertible<RGBA128F>
            , IPixelConvertible<BGRA128F>
            , IPixelConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IPixelConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IPixelConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IPixelConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IPixelConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IPixelConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IPixelConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IPixelConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IPixelConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IPixelConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IPixelConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IPixelConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IPixelConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IPixelConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IPixelConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IPixelConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IPixelConvertible<RGBP128F>.ToPixel() { return this; }
        }
    }
}
