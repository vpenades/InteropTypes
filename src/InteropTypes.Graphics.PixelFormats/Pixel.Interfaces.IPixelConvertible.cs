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
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct Luminance8
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct Luminance16
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct Luminance32F
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGR565
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGR24
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct RGB24
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGRA5551
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGRA4444
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGRA32
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return this; }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct RGBA32
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct ARGB32
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct RGB96F
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGR96F
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGRA128F
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct RGBA128F
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return this; }
        }
        partial struct RGBP32
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct BGRP32
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
        partial struct RGBP128F
            : IPixelConvertible<BGRA32>
            , IPixelConvertible<RGBA128F>
        {
            BGRA32 IPixelConvertible<BGRA32>.ToPixel() { return new BGRA32(this); }
            RGBA128F IPixelConvertible<RGBA128F>.ToPixel() { return new RGBA128F(this); }
        }
    }
}
