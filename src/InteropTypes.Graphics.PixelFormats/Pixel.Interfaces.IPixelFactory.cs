using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InteropBitmaps
{
    partial class Pixel    
    {
        public interface IPixelFactory<TSrcPixel, TDstPixel>
        {
            TDstPixel From(TSrcPixel color);
        }

        partial struct Alpha8
            : IPixelFactory<BGRA32,Alpha8>
            , IPixelFactory<RGBA128F,Alpha8>
        {
            Alpha8 IPixelFactory<BGRA32,Alpha8>.From(BGRA32 color) { return new Alpha8(color); }
            Alpha8 IPixelFactory<RGBA128F,Alpha8>.From(RGBA128F color) { return new Alpha8(color); }
        }
        partial struct Luminance8
            : IPixelFactory<BGRA32,Luminance8>
            , IPixelFactory<RGBA128F,Luminance8>
        {
            Luminance8 IPixelFactory<BGRA32,Luminance8>.From(BGRA32 color) { return new Luminance8(color); }
            Luminance8 IPixelFactory<RGBA128F,Luminance8>.From(RGBA128F color) { return new Luminance8(color); }
        }
        partial struct Luminance16
            : IPixelFactory<BGRA32,Luminance16>
            , IPixelFactory<RGBA128F,Luminance16>
        {
            Luminance16 IPixelFactory<BGRA32,Luminance16>.From(BGRA32 color) { return new Luminance16(color); }
            Luminance16 IPixelFactory<RGBA128F,Luminance16>.From(RGBA128F color) { return new Luminance16(color); }
        }
        partial struct Luminance32F
            : IPixelFactory<BGRA32,Luminance32F>
            , IPixelFactory<RGBA128F,Luminance32F>
        {
            Luminance32F IPixelFactory<BGRA32,Luminance32F>.From(BGRA32 color) { return new Luminance32F(color); }
            Luminance32F IPixelFactory<RGBA128F,Luminance32F>.From(RGBA128F color) { return new Luminance32F(color); }
        }
        partial struct BGR565
            : IPixelFactory<BGRA32,BGR565>
            , IPixelFactory<RGBA128F,BGR565>
        {
            BGR565 IPixelFactory<BGRA32,BGR565>.From(BGRA32 color) { return new BGR565(color); }
            BGR565 IPixelFactory<RGBA128F,BGR565>.From(RGBA128F color) { return new BGR565(color); }
        }
        partial struct BGR24
            : IPixelFactory<BGRA32,BGR24>
            , IPixelFactory<RGBA128F,BGR24>
        {
            BGR24 IPixelFactory<BGRA32,BGR24>.From(BGRA32 color) { return new BGR24(color); }
            BGR24 IPixelFactory<RGBA128F,BGR24>.From(RGBA128F color) { return new BGR24(color); }
        }
        partial struct RGB24
            : IPixelFactory<BGRA32,RGB24>
            , IPixelFactory<RGBA128F,RGB24>
        {
            RGB24 IPixelFactory<BGRA32,RGB24>.From(BGRA32 color) { return new RGB24(color); }
            RGB24 IPixelFactory<RGBA128F,RGB24>.From(RGBA128F color) { return new RGB24(color); }
        }
        partial struct BGRA5551
            : IPixelFactory<BGRA32,BGRA5551>
            , IPixelFactory<RGBA128F,BGRA5551>
        {
            BGRA5551 IPixelFactory<BGRA32,BGRA5551>.From(BGRA32 color) { return new BGRA5551(color); }
            BGRA5551 IPixelFactory<RGBA128F,BGRA5551>.From(RGBA128F color) { return new BGRA5551(color); }
        }
        partial struct BGRA4444
            : IPixelFactory<BGRA32,BGRA4444>
            , IPixelFactory<RGBA128F,BGRA4444>
        {
            BGRA4444 IPixelFactory<BGRA32,BGRA4444>.From(BGRA32 color) { return new BGRA4444(color); }
            BGRA4444 IPixelFactory<RGBA128F,BGRA4444>.From(RGBA128F color) { return new BGRA4444(color); }
        }
        partial struct BGRA32
            : IPixelFactory<BGRA32,BGRA32>
            , IPixelFactory<RGBA128F,BGRA32>
        {
            BGRA32 IPixelFactory<BGRA32,BGRA32>.From(BGRA32 color) { return color; }
            BGRA32 IPixelFactory<RGBA128F,BGRA32>.From(RGBA128F color) { return new BGRA32(color); }
        }
        partial struct RGBA32
            : IPixelFactory<BGRA32,RGBA32>
            , IPixelFactory<RGBA128F,RGBA32>
        {
            RGBA32 IPixelFactory<BGRA32,RGBA32>.From(BGRA32 color) { return new RGBA32(color); }
            RGBA32 IPixelFactory<RGBA128F,RGBA32>.From(RGBA128F color) { return new RGBA32(color); }
        }
        partial struct ARGB32
            : IPixelFactory<BGRA32,ARGB32>
            , IPixelFactory<RGBA128F,ARGB32>
        {
            ARGB32 IPixelFactory<BGRA32,ARGB32>.From(BGRA32 color) { return new ARGB32(color); }
            ARGB32 IPixelFactory<RGBA128F,ARGB32>.From(RGBA128F color) { return new ARGB32(color); }
        }
        partial struct RGBP32
            : IPixelFactory<BGRA32,RGBP32>
            , IPixelFactory<RGBA128F,RGBP32>
        {
            RGBP32 IPixelFactory<BGRA32,RGBP32>.From(BGRA32 color) { return new RGBP32(color); }
            RGBP32 IPixelFactory<RGBA128F,RGBP32>.From(RGBA128F color) { return new RGBP32(color); }
        }
        partial struct BGRP32
            : IPixelFactory<BGRA32,BGRP32>
            , IPixelFactory<RGBA128F,BGRP32>
        {
            BGRP32 IPixelFactory<BGRA32,BGRP32>.From(BGRA32 color) { return new BGRP32(color); }
            BGRP32 IPixelFactory<RGBA128F,BGRP32>.From(RGBA128F color) { return new BGRP32(color); }
        }
        partial struct RGB96F
            : IPixelFactory<BGRA32,RGB96F>
            , IPixelFactory<RGBA128F,RGB96F>
        {
            RGB96F IPixelFactory<BGRA32,RGB96F>.From(BGRA32 color) { return new RGB96F(color); }
            RGB96F IPixelFactory<RGBA128F,RGB96F>.From(RGBA128F color) { return new RGB96F(color); }
        }
        partial struct BGR96F
            : IPixelFactory<BGRA32,BGR96F>
            , IPixelFactory<RGBA128F,BGR96F>
        {
            BGR96F IPixelFactory<BGRA32,BGR96F>.From(BGRA32 color) { return new BGR96F(color); }
            BGR96F IPixelFactory<RGBA128F,BGR96F>.From(RGBA128F color) { return new BGR96F(color); }
        }
        partial struct BGRA128F
            : IPixelFactory<BGRA32,BGRA128F>
            , IPixelFactory<RGBA128F,BGRA128F>
        {
            BGRA128F IPixelFactory<BGRA32,BGRA128F>.From(BGRA32 color) { return new BGRA128F(color); }
            BGRA128F IPixelFactory<RGBA128F,BGRA128F>.From(RGBA128F color) { return new BGRA128F(color); }
        }
        partial struct RGBA128F
            : IPixelFactory<BGRA32,RGBA128F>
            , IPixelFactory<RGBA128F,RGBA128F>
        {
            RGBA128F IPixelFactory<BGRA32,RGBA128F>.From(BGRA32 color) { return new RGBA128F(color); }
            RGBA128F IPixelFactory<RGBA128F,RGBA128F>.From(RGBA128F color) { return color; }
        }
        partial struct RGBP128F
            : IPixelFactory<BGRA32,RGBP128F>
            , IPixelFactory<RGBA128F,RGBP128F>
        {
            RGBP128F IPixelFactory<BGRA32,RGBP128F>.From(BGRA32 color) { return new RGBP128F(color); }
            RGBP128F IPixelFactory<RGBA128F,RGBP128F>.From(RGBA128F color) { return new RGBP128F(color); }
        }
    }
}
