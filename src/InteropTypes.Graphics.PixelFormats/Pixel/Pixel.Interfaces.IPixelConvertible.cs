
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InteropBitmaps
{
    partial class Pixel    
    {
        public interface IConvertible<TPixel>
        {
            TPixel ToPixel();
        }

        partial struct Alpha8
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return this; }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(this.A); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct Luminance8
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(this.L); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return this; }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct Luminance16
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct Luminance32F
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return this; }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGR565
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGR24
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGB24
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return this; }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA5551
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA4444
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return this; }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA32
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(this); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(this); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(this); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(this); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(this); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(this); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(this); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(this); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return this; }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(this); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(this); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(this); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBA32
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct ARGB32
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBP32
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return this; }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRP32
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGB96F
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return this; }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGR96F
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct BGRA128F
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return this; }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBA128F
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(this); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(this); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(this); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return this; }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(this); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return new RGBP128F(this); }
        }
        partial struct RGBP128F
            : IConvertible<Alpha8>
            , IConvertible<Luminance8>
            , IConvertible<BGR565>
            , IConvertible<BGRA5551>
            , IConvertible<BGRA4444>
            , IConvertible<RGB24>
            , IConvertible<BGR24>
            , IConvertible<RGBA32>
            , IConvertible<BGRA32>
            , IConvertible<ARGB32>
            , IConvertible<BGRP32>
            , IConvertible<RGBP32>
            , IConvertible<Luminance32F>
            , IConvertible<RGB96F>
            , IConvertible<BGR96F>
            , IConvertible<RGBA128F>
            , IConvertible<BGRA128F>
            , IConvertible<RGBP128F>
        {
            /// <inheritdoc />
            Alpha8 IConvertible<Alpha8>.ToPixel() { return new Alpha8(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance8 IConvertible<Luminance8>.ToPixel() { return new Luminance8(new BGRA32(this)); }
            /// <inheritdoc />
            BGR565 IConvertible<BGR565>.ToPixel() { return new BGR565(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA5551 IConvertible<BGRA5551>.ToPixel() { return new BGRA5551(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA4444 IConvertible<BGRA4444>.ToPixel() { return new BGRA4444(new BGRA32(this)); }
            /// <inheritdoc />
            RGB24 IConvertible<RGB24>.ToPixel() { return new RGB24(new BGRA32(this)); }
            /// <inheritdoc />
            BGR24 IConvertible<BGR24>.ToPixel() { return new BGR24(new BGRA32(this)); }
            /// <inheritdoc />
            RGBA32 IConvertible<RGBA32>.ToPixel() { return new RGBA32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRA32 IConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            /// <inheritdoc />
            ARGB32 IConvertible<ARGB32>.ToPixel() { return new ARGB32(new BGRA32(this)); }
            /// <inheritdoc />
            BGRP32 IConvertible<BGRP32>.ToPixel() { return new BGRP32(new BGRA32(this)); }
            /// <inheritdoc />
            RGBP32 IConvertible<RGBP32>.ToPixel() { return new RGBP32(new BGRA32(this)); }
            /// <inheritdoc />
            Luminance32F IConvertible<Luminance32F>.ToPixel() { return new Luminance32F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGB96F IConvertible<RGB96F>.ToPixel() { return new RGB96F(new RGBA128F(this)); }
            /// <inheritdoc />
            BGR96F IConvertible<BGR96F>.ToPixel() { return new BGR96F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBA128F IConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
            /// <inheritdoc />
            BGRA128F IConvertible<BGRA128F>.ToPixel() { return new BGRA128F(new RGBA128F(this)); }
            /// <inheritdoc />
            RGBP128F IConvertible<RGBP128F>.ToPixel() { return this; }
        }
    }
}
