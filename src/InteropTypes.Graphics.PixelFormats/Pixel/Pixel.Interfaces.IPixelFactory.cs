using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace InteropBitmaps
{
    partial class Pixel    
    {
        public interface IPixelFactory<TSrcPixel, TDstPixel>
            where TSrcPixel: unmanaged
            where TDstPixel: unmanaged
        {
            TDstPixel From(TSrcPixel color);

            void Copy(ReadOnlySpan<TSrcPixel> src, Span<TDstPixel> dst);
        }

        partial struct Alpha8
            : IPixelFactory<BGRA32,Alpha8>
            , IPixelFactory<RGBA128F,Alpha8>
            , IPixelFactory<BGR565,Alpha8>
            , IPixelFactory<BGRA5551,Alpha8>
            , IPixelFactory<BGRA4444,Alpha8>
            , IPixelFactory<RGB24,Alpha8>
            , IPixelFactory<BGR24,Alpha8>
            , IPixelFactory<RGBA32,Alpha8>
            , IPixelFactory<ARGB32,Alpha8>
        {
            Alpha8 IPixelFactory<BGRA32,Alpha8>.From(BGRA32 color) { return new Alpha8(color); }
            Alpha8 IPixelFactory<RGBA128F,Alpha8>.From(RGBA128F color) { return new Alpha8(color); }
            Alpha8 IPixelFactory<BGR565,Alpha8>.From(BGR565 color) { return new Alpha8(new BGRA32(color)); }
            Alpha8 IPixelFactory<BGRA5551,Alpha8>.From(BGRA5551 color) { return new Alpha8(new BGRA32(color)); }
            Alpha8 IPixelFactory<BGRA4444,Alpha8>.From(BGRA4444 color) { return new Alpha8(new BGRA32(color)); }
            Alpha8 IPixelFactory<RGB24,Alpha8>.From(RGB24 color) { return new Alpha8(new BGRA32(color)); }
            Alpha8 IPixelFactory<BGR24,Alpha8>.From(BGR24 color) { return new Alpha8(new BGRA32(color)); }
            Alpha8 IPixelFactory<RGBA32,Alpha8>.From(RGBA32 color) { return new Alpha8(new BGRA32(color)); }
            Alpha8 IPixelFactory<ARGB32,Alpha8>.From(ARGB32 color) { return new Alpha8(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,Alpha8>.Copy(ReadOnlySpan<BGRA32> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<RGBA128F,Alpha8>.Copy(ReadOnlySpan<RGBA128F> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<BGR565,Alpha8>.Copy(ReadOnlySpan<BGR565> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<BGRA5551,Alpha8>.Copy(ReadOnlySpan<BGRA5551> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<BGRA4444,Alpha8>.Copy(ReadOnlySpan<BGRA4444> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<RGB24,Alpha8>.Copy(ReadOnlySpan<RGB24> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<BGR24,Alpha8>.Copy(ReadOnlySpan<BGR24> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<RGBA32,Alpha8>.Copy(ReadOnlySpan<RGBA32> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
            unsafe void IPixelFactory<ARGB32,Alpha8>.Copy(ReadOnlySpan<ARGB32> src, Span<Alpha8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Alpha8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(Alpha8));
                }
            }
        }
        partial struct Luminance8
            : IPixelFactory<BGRA32,Luminance8>
            , IPixelFactory<RGBA128F,Luminance8>
            , IPixelFactory<BGR565,Luminance8>
            , IPixelFactory<BGRA5551,Luminance8>
            , IPixelFactory<BGRA4444,Luminance8>
            , IPixelFactory<RGB24,Luminance8>
            , IPixelFactory<BGR24,Luminance8>
            , IPixelFactory<RGBA32,Luminance8>
            , IPixelFactory<ARGB32,Luminance8>
        {
            Luminance8 IPixelFactory<BGRA32,Luminance8>.From(BGRA32 color) { return new Luminance8(color); }
            Luminance8 IPixelFactory<RGBA128F,Luminance8>.From(RGBA128F color) { return new Luminance8(color); }
            Luminance8 IPixelFactory<BGR565,Luminance8>.From(BGR565 color) { return new Luminance8(new BGRA32(color)); }
            Luminance8 IPixelFactory<BGRA5551,Luminance8>.From(BGRA5551 color) { return new Luminance8(new BGRA32(color)); }
            Luminance8 IPixelFactory<BGRA4444,Luminance8>.From(BGRA4444 color) { return new Luminance8(new BGRA32(color)); }
            Luminance8 IPixelFactory<RGB24,Luminance8>.From(RGB24 color) { return new Luminance8(new BGRA32(color)); }
            Luminance8 IPixelFactory<BGR24,Luminance8>.From(BGR24 color) { return new Luminance8(new BGRA32(color)); }
            Luminance8 IPixelFactory<RGBA32,Luminance8>.From(RGBA32 color) { return new Luminance8(new BGRA32(color)); }
            Luminance8 IPixelFactory<ARGB32,Luminance8>.From(ARGB32 color) { return new Luminance8(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,Luminance8>.Copy(ReadOnlySpan<BGRA32> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<RGBA128F,Luminance8>.Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<BGR565,Luminance8>.Copy(ReadOnlySpan<BGR565> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<BGRA5551,Luminance8>.Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<BGRA4444,Luminance8>.Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<RGB24,Luminance8>.Copy(ReadOnlySpan<RGB24> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<BGR24,Luminance8>.Copy(ReadOnlySpan<BGR24> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<RGBA32,Luminance8>.Copy(ReadOnlySpan<RGBA32> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
            unsafe void IPixelFactory<ARGB32,Luminance8>.Copy(ReadOnlySpan<ARGB32> src, Span<Luminance8> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance8(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(Luminance8));
                }
            }
        }
        partial struct Luminance16
            : IPixelFactory<BGRA32,Luminance16>
            , IPixelFactory<RGBA128F,Luminance16>
            , IPixelFactory<BGR565,Luminance16>
            , IPixelFactory<BGRA5551,Luminance16>
            , IPixelFactory<BGRA4444,Luminance16>
            , IPixelFactory<RGB24,Luminance16>
            , IPixelFactory<BGR24,Luminance16>
            , IPixelFactory<RGBA32,Luminance16>
            , IPixelFactory<ARGB32,Luminance16>
        {
            Luminance16 IPixelFactory<BGRA32,Luminance16>.From(BGRA32 color) { return new Luminance16(color); }
            Luminance16 IPixelFactory<RGBA128F,Luminance16>.From(RGBA128F color) { return new Luminance16(color); }
            Luminance16 IPixelFactory<BGR565,Luminance16>.From(BGR565 color) { return new Luminance16(new BGRA32(color)); }
            Luminance16 IPixelFactory<BGRA5551,Luminance16>.From(BGRA5551 color) { return new Luminance16(new BGRA32(color)); }
            Luminance16 IPixelFactory<BGRA4444,Luminance16>.From(BGRA4444 color) { return new Luminance16(new BGRA32(color)); }
            Luminance16 IPixelFactory<RGB24,Luminance16>.From(RGB24 color) { return new Luminance16(new BGRA32(color)); }
            Luminance16 IPixelFactory<BGR24,Luminance16>.From(BGR24 color) { return new Luminance16(new BGRA32(color)); }
            Luminance16 IPixelFactory<RGBA32,Luminance16>.From(RGBA32 color) { return new Luminance16(new BGRA32(color)); }
            Luminance16 IPixelFactory<ARGB32,Luminance16>.From(ARGB32 color) { return new Luminance16(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,Luminance16>.Copy(ReadOnlySpan<BGRA32> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<RGBA128F,Luminance16>.Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<BGR565,Luminance16>.Copy(ReadOnlySpan<BGR565> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<BGRA5551,Luminance16>.Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<BGRA4444,Luminance16>.Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<RGB24,Luminance16>.Copy(ReadOnlySpan<RGB24> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<BGR24,Luminance16>.Copy(ReadOnlySpan<BGR24> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<RGBA32,Luminance16>.Copy(ReadOnlySpan<RGBA32> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
            unsafe void IPixelFactory<ARGB32,Luminance16>.Copy(ReadOnlySpan<ARGB32> src, Span<Luminance16> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance16(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(Luminance16));
                }
            }
        }
        partial struct Luminance32F
            : IPixelFactory<BGRA32,Luminance32F>
            , IPixelFactory<RGBA128F,Luminance32F>
            , IPixelFactory<BGR565,Luminance32F>
            , IPixelFactory<BGRA5551,Luminance32F>
            , IPixelFactory<BGRA4444,Luminance32F>
            , IPixelFactory<RGB24,Luminance32F>
            , IPixelFactory<BGR24,Luminance32F>
            , IPixelFactory<RGBA32,Luminance32F>
            , IPixelFactory<ARGB32,Luminance32F>
        {
            Luminance32F IPixelFactory<BGRA32,Luminance32F>.From(BGRA32 color) { return new Luminance32F(color); }
            Luminance32F IPixelFactory<RGBA128F,Luminance32F>.From(RGBA128F color) { return new Luminance32F(color); }
            Luminance32F IPixelFactory<BGR565,Luminance32F>.From(BGR565 color) { return new Luminance32F(new BGRA32(color)); }
            Luminance32F IPixelFactory<BGRA5551,Luminance32F>.From(BGRA5551 color) { return new Luminance32F(new BGRA32(color)); }
            Luminance32F IPixelFactory<BGRA4444,Luminance32F>.From(BGRA4444 color) { return new Luminance32F(new BGRA32(color)); }
            Luminance32F IPixelFactory<RGB24,Luminance32F>.From(RGB24 color) { return new Luminance32F(new BGRA32(color)); }
            Luminance32F IPixelFactory<BGR24,Luminance32F>.From(BGR24 color) { return new Luminance32F(new BGRA32(color)); }
            Luminance32F IPixelFactory<RGBA32,Luminance32F>.From(RGBA32 color) { return new Luminance32F(new BGRA32(color)); }
            Luminance32F IPixelFactory<ARGB32,Luminance32F>.From(ARGB32 color) { return new Luminance32F(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,Luminance32F>.Copy(ReadOnlySpan<BGRA32> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<RGBA128F,Luminance32F>.Copy(ReadOnlySpan<RGBA128F> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<BGR565,Luminance32F>.Copy(ReadOnlySpan<BGR565> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<BGRA5551,Luminance32F>.Copy(ReadOnlySpan<BGRA5551> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<BGRA4444,Luminance32F>.Copy(ReadOnlySpan<BGRA4444> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<RGB24,Luminance32F>.Copy(ReadOnlySpan<RGB24> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<BGR24,Luminance32F>.Copy(ReadOnlySpan<BGR24> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<RGBA32,Luminance32F>.Copy(ReadOnlySpan<RGBA32> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
            unsafe void IPixelFactory<ARGB32,Luminance32F>.Copy(ReadOnlySpan<ARGB32> src, Span<Luminance32F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new Luminance32F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(Luminance32F));
                }
            }
        }
        partial struct BGR565
            : IPixelFactory<BGRA32,BGR565>
            , IPixelFactory<RGBA128F,BGR565>
            , IPixelFactory<BGR565,BGR565>
            , IPixelFactory<BGRA5551,BGR565>
            , IPixelFactory<BGRA4444,BGR565>
            , IPixelFactory<RGB24,BGR565>
            , IPixelFactory<BGR24,BGR565>
            , IPixelFactory<RGBA32,BGR565>
            , IPixelFactory<ARGB32,BGR565>
        {
            BGR565 IPixelFactory<BGRA32,BGR565>.From(BGRA32 color) { return new BGR565(color); }
            BGR565 IPixelFactory<RGBA128F,BGR565>.From(RGBA128F color) { return new BGR565(color); }
            BGR565 IPixelFactory<BGR565,BGR565>.From(BGR565 color) { return color; }
            BGR565 IPixelFactory<BGRA5551,BGR565>.From(BGRA5551 color) { return new BGR565(new BGRA32(color)); }
            BGR565 IPixelFactory<BGRA4444,BGR565>.From(BGRA4444 color) { return new BGR565(new BGRA32(color)); }
            BGR565 IPixelFactory<RGB24,BGR565>.From(RGB24 color) { return new BGR565(new BGRA32(color)); }
            BGR565 IPixelFactory<BGR24,BGR565>.From(BGR24 color) { return new BGR565(new BGRA32(color)); }
            BGR565 IPixelFactory<RGBA32,BGR565>.From(RGBA32 color) { return new BGR565(new BGRA32(color)); }
            BGR565 IPixelFactory<ARGB32,BGR565>.From(ARGB32 color) { return new BGR565(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,BGR565>.Copy(ReadOnlySpan<BGRA32> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
            unsafe void IPixelFactory<RGBA128F,BGR565>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
            unsafe void IPixelFactory<BGR565,BGR565>.Copy(ReadOnlySpan<BGR565> src, Span<BGR565> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<BGRA5551,BGR565>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
            unsafe void IPixelFactory<BGRA4444,BGR565>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
            unsafe void IPixelFactory<RGB24,BGR565>.Copy(ReadOnlySpan<RGB24> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
            unsafe void IPixelFactory<BGR24,BGR565>.Copy(ReadOnlySpan<BGR24> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
            unsafe void IPixelFactory<RGBA32,BGR565>.Copy(ReadOnlySpan<RGBA32> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGR565>.Copy(ReadOnlySpan<ARGB32> src, Span<BGR565> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR565(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGR565));
                }
            }
        }
        partial struct BGR24
            : IPixelFactory<BGRA32,BGR24>
            , IPixelFactory<RGBA128F,BGR24>
            , IPixelFactory<BGR565,BGR24>
            , IPixelFactory<BGRA5551,BGR24>
            , IPixelFactory<BGRA4444,BGR24>
            , IPixelFactory<RGB24,BGR24>
            , IPixelFactory<BGR24,BGR24>
            , IPixelFactory<RGBA32,BGR24>
            , IPixelFactory<ARGB32,BGR24>
        {
            BGR24 IPixelFactory<BGRA32,BGR24>.From(BGRA32 color) { return new BGR24(color); }
            BGR24 IPixelFactory<RGBA128F,BGR24>.From(RGBA128F color) { return new BGR24(color); }
            BGR24 IPixelFactory<BGR565,BGR24>.From(BGR565 color) { return new BGR24(new BGRA32(color)); }
            BGR24 IPixelFactory<BGRA5551,BGR24>.From(BGRA5551 color) { return new BGR24(new BGRA32(color)); }
            BGR24 IPixelFactory<BGRA4444,BGR24>.From(BGRA4444 color) { return new BGR24(new BGRA32(color)); }
            BGR24 IPixelFactory<RGB24,BGR24>.From(RGB24 color) { return new BGR24(new BGRA32(color)); }
            BGR24 IPixelFactory<BGR24,BGR24>.From(BGR24 color) { return color; }
            BGR24 IPixelFactory<RGBA32,BGR24>.From(RGBA32 color) { return new BGR24(new BGRA32(color)); }
            BGR24 IPixelFactory<ARGB32,BGR24>.From(ARGB32 color) { return new BGR24(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,BGR24>.Copy(ReadOnlySpan<BGRA32> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
            unsafe void IPixelFactory<RGBA128F,BGR24>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
            unsafe void IPixelFactory<BGR565,BGR24>.Copy(ReadOnlySpan<BGR565> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
            unsafe void IPixelFactory<BGRA5551,BGR24>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
            unsafe void IPixelFactory<BGRA4444,BGR24>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
            unsafe void IPixelFactory<RGB24,BGR24>.Copy(ReadOnlySpan<RGB24> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
            unsafe void IPixelFactory<BGR24,BGR24>.Copy(ReadOnlySpan<BGR24> src, Span<BGR24> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<RGBA32,BGR24>.Copy(ReadOnlySpan<RGBA32> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGR24>.Copy(ReadOnlySpan<ARGB32> src, Span<BGR24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGR24));
                }
            }
        }
        partial struct RGB24
            : IPixelFactory<BGRA32,RGB24>
            , IPixelFactory<RGBA128F,RGB24>
            , IPixelFactory<BGR565,RGB24>
            , IPixelFactory<BGRA5551,RGB24>
            , IPixelFactory<BGRA4444,RGB24>
            , IPixelFactory<RGB24,RGB24>
            , IPixelFactory<BGR24,RGB24>
            , IPixelFactory<RGBA32,RGB24>
            , IPixelFactory<ARGB32,RGB24>
        {
            RGB24 IPixelFactory<BGRA32,RGB24>.From(BGRA32 color) { return new RGB24(color); }
            RGB24 IPixelFactory<RGBA128F,RGB24>.From(RGBA128F color) { return new RGB24(color); }
            RGB24 IPixelFactory<BGR565,RGB24>.From(BGR565 color) { return new RGB24(new BGRA32(color)); }
            RGB24 IPixelFactory<BGRA5551,RGB24>.From(BGRA5551 color) { return new RGB24(new BGRA32(color)); }
            RGB24 IPixelFactory<BGRA4444,RGB24>.From(BGRA4444 color) { return new RGB24(new BGRA32(color)); }
            RGB24 IPixelFactory<RGB24,RGB24>.From(RGB24 color) { return color; }
            RGB24 IPixelFactory<BGR24,RGB24>.From(BGR24 color) { return new RGB24(new BGRA32(color)); }
            RGB24 IPixelFactory<RGBA32,RGB24>.From(RGBA32 color) { return new RGB24(new BGRA32(color)); }
            RGB24 IPixelFactory<ARGB32,RGB24>.From(ARGB32 color) { return new RGB24(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,RGB24>.Copy(ReadOnlySpan<BGRA32> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
            unsafe void IPixelFactory<RGBA128F,RGB24>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
            unsafe void IPixelFactory<BGR565,RGB24>.Copy(ReadOnlySpan<BGR565> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
            unsafe void IPixelFactory<BGRA5551,RGB24>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
            unsafe void IPixelFactory<BGRA4444,RGB24>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
            unsafe void IPixelFactory<RGB24,RGB24>.Copy(ReadOnlySpan<RGB24> src, Span<RGB24> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<BGR24,RGB24>.Copy(ReadOnlySpan<BGR24> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
            unsafe void IPixelFactory<RGBA32,RGB24>.Copy(ReadOnlySpan<RGBA32> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
            unsafe void IPixelFactory<ARGB32,RGB24>.Copy(ReadOnlySpan<ARGB32> src, Span<RGB24> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB24(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(RGB24));
                }
            }
        }
        partial struct BGRA5551
            : IPixelFactory<BGRA32,BGRA5551>
            , IPixelFactory<RGBA128F,BGRA5551>
            , IPixelFactory<BGR565,BGRA5551>
            , IPixelFactory<BGRA5551,BGRA5551>
            , IPixelFactory<BGRA4444,BGRA5551>
            , IPixelFactory<RGB24,BGRA5551>
            , IPixelFactory<BGR24,BGRA5551>
            , IPixelFactory<RGBA32,BGRA5551>
            , IPixelFactory<ARGB32,BGRA5551>
        {
            BGRA5551 IPixelFactory<BGRA32,BGRA5551>.From(BGRA32 color) { return new BGRA5551(color); }
            BGRA5551 IPixelFactory<RGBA128F,BGRA5551>.From(RGBA128F color) { return new BGRA5551(color); }
            BGRA5551 IPixelFactory<BGR565,BGRA5551>.From(BGR565 color) { return new BGRA5551(new BGRA32(color)); }
            BGRA5551 IPixelFactory<BGRA5551,BGRA5551>.From(BGRA5551 color) { return color; }
            BGRA5551 IPixelFactory<BGRA4444,BGRA5551>.From(BGRA4444 color) { return new BGRA5551(new BGRA32(color)); }
            BGRA5551 IPixelFactory<RGB24,BGRA5551>.From(RGB24 color) { return new BGRA5551(new BGRA32(color)); }
            BGRA5551 IPixelFactory<BGR24,BGRA5551>.From(BGR24 color) { return new BGRA5551(new BGRA32(color)); }
            BGRA5551 IPixelFactory<RGBA32,BGRA5551>.From(RGBA32 color) { return new BGRA5551(new BGRA32(color)); }
            BGRA5551 IPixelFactory<ARGB32,BGRA5551>.From(ARGB32 color) { return new BGRA5551(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,BGRA5551>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
            unsafe void IPixelFactory<RGBA128F,BGRA5551>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
            unsafe void IPixelFactory<BGR565,BGRA5551>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
            unsafe void IPixelFactory<BGRA5551,BGRA5551>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA5551> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<BGRA4444,BGRA5551>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
            unsafe void IPixelFactory<RGB24,BGRA5551>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
            unsafe void IPixelFactory<BGR24,BGRA5551>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
            unsafe void IPixelFactory<RGBA32,BGRA5551>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGRA5551>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA5551> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA5551(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGRA5551));
                }
            }
        }
        partial struct BGRA4444
            : IPixelFactory<BGRA32,BGRA4444>
            , IPixelFactory<RGBA128F,BGRA4444>
            , IPixelFactory<BGR565,BGRA4444>
            , IPixelFactory<BGRA5551,BGRA4444>
            , IPixelFactory<BGRA4444,BGRA4444>
            , IPixelFactory<RGB24,BGRA4444>
            , IPixelFactory<BGR24,BGRA4444>
            , IPixelFactory<RGBA32,BGRA4444>
            , IPixelFactory<ARGB32,BGRA4444>
        {
            BGRA4444 IPixelFactory<BGRA32,BGRA4444>.From(BGRA32 color) { return new BGRA4444(color); }
            BGRA4444 IPixelFactory<RGBA128F,BGRA4444>.From(RGBA128F color) { return new BGRA4444(color); }
            BGRA4444 IPixelFactory<BGR565,BGRA4444>.From(BGR565 color) { return new BGRA4444(new BGRA32(color)); }
            BGRA4444 IPixelFactory<BGRA5551,BGRA4444>.From(BGRA5551 color) { return new BGRA4444(new BGRA32(color)); }
            BGRA4444 IPixelFactory<BGRA4444,BGRA4444>.From(BGRA4444 color) { return color; }
            BGRA4444 IPixelFactory<RGB24,BGRA4444>.From(RGB24 color) { return new BGRA4444(new BGRA32(color)); }
            BGRA4444 IPixelFactory<BGR24,BGRA4444>.From(BGR24 color) { return new BGRA4444(new BGRA32(color)); }
            BGRA4444 IPixelFactory<RGBA32,BGRA4444>.From(RGBA32 color) { return new BGRA4444(new BGRA32(color)); }
            BGRA4444 IPixelFactory<ARGB32,BGRA4444>.From(ARGB32 color) { return new BGRA4444(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,BGRA4444>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
            unsafe void IPixelFactory<RGBA128F,BGRA4444>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
            unsafe void IPixelFactory<BGR565,BGRA4444>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
            unsafe void IPixelFactory<BGRA5551,BGRA4444>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
            unsafe void IPixelFactory<BGRA4444,BGRA4444>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA4444> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<RGB24,BGRA4444>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
            unsafe void IPixelFactory<BGR24,BGRA4444>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
            unsafe void IPixelFactory<RGBA32,BGRA4444>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGRA4444>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA4444> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA4444(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGRA4444));
                }
            }
        }
        partial struct BGRA32
            : IPixelFactory<BGRA32,BGRA32>
            , IPixelFactory<RGBA128F,BGRA32>
            , IPixelFactory<BGR565,BGRA32>
            , IPixelFactory<BGRA5551,BGRA32>
            , IPixelFactory<BGRA4444,BGRA32>
            , IPixelFactory<RGB24,BGRA32>
            , IPixelFactory<BGR24,BGRA32>
            , IPixelFactory<RGBA32,BGRA32>
            , IPixelFactory<ARGB32,BGRA32>
        {
            BGRA32 IPixelFactory<BGRA32,BGRA32>.From(BGRA32 color) { return color; }
            BGRA32 IPixelFactory<RGBA128F,BGRA32>.From(RGBA128F color) { return new BGRA32(color); }
            BGRA32 IPixelFactory<BGR565,BGRA32>.From(BGR565 color) { return new BGRA32(color); }
            BGRA32 IPixelFactory<BGRA5551,BGRA32>.From(BGRA5551 color) { return new BGRA32(color); }
            BGRA32 IPixelFactory<BGRA4444,BGRA32>.From(BGRA4444 color) { return new BGRA32(color); }
            BGRA32 IPixelFactory<RGB24,BGRA32>.From(RGB24 color) { return new BGRA32(color); }
            BGRA32 IPixelFactory<BGR24,BGRA32>.From(BGR24 color) { return new BGRA32(color); }
            BGRA32 IPixelFactory<RGBA32,BGRA32>.From(RGBA32 color) { return new BGRA32(color); }
            BGRA32 IPixelFactory<ARGB32,BGRA32>.From(ARGB32 color) { return new BGRA32(color); }
            unsafe void IPixelFactory<BGRA32,BGRA32>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA32> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<RGBA128F,BGRA32>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
            unsafe void IPixelFactory<BGR565,BGRA32>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
            unsafe void IPixelFactory<BGRA5551,BGRA32>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
            unsafe void IPixelFactory<BGRA4444,BGRA32>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
            unsafe void IPixelFactory<RGB24,BGRA32>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
            unsafe void IPixelFactory<BGR24,BGRA32>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
            unsafe void IPixelFactory<RGBA32,BGRA32>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGRA32>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA32(src[0]);
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGRA32));
                }
            }
        }
        partial struct RGBA32
            : IPixelFactory<BGRA32,RGBA32>
            , IPixelFactory<RGBA128F,RGBA32>
            , IPixelFactory<BGR565,RGBA32>
            , IPixelFactory<BGRA5551,RGBA32>
            , IPixelFactory<BGRA4444,RGBA32>
            , IPixelFactory<RGB24,RGBA32>
            , IPixelFactory<BGR24,RGBA32>
            , IPixelFactory<RGBA32,RGBA32>
            , IPixelFactory<ARGB32,RGBA32>
        {
            RGBA32 IPixelFactory<BGRA32,RGBA32>.From(BGRA32 color) { return new RGBA32(color); }
            RGBA32 IPixelFactory<RGBA128F,RGBA32>.From(RGBA128F color) { return new RGBA32(color); }
            RGBA32 IPixelFactory<BGR565,RGBA32>.From(BGR565 color) { return new RGBA32(new BGRA32(color)); }
            RGBA32 IPixelFactory<BGRA5551,RGBA32>.From(BGRA5551 color) { return new RGBA32(new BGRA32(color)); }
            RGBA32 IPixelFactory<BGRA4444,RGBA32>.From(BGRA4444 color) { return new RGBA32(new BGRA32(color)); }
            RGBA32 IPixelFactory<RGB24,RGBA32>.From(RGB24 color) { return new RGBA32(new BGRA32(color)); }
            RGBA32 IPixelFactory<BGR24,RGBA32>.From(BGR24 color) { return new RGBA32(new BGRA32(color)); }
            RGBA32 IPixelFactory<RGBA32,RGBA32>.From(RGBA32 color) { return color; }
            RGBA32 IPixelFactory<ARGB32,RGBA32>.From(ARGB32 color) { return new RGBA32(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,RGBA32>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
            unsafe void IPixelFactory<RGBA128F,RGBA32>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
            unsafe void IPixelFactory<BGR565,RGBA32>.Copy(ReadOnlySpan<BGR565> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
            unsafe void IPixelFactory<BGRA5551,RGBA32>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
            unsafe void IPixelFactory<BGRA4444,RGBA32>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
            unsafe void IPixelFactory<RGB24,RGBA32>.Copy(ReadOnlySpan<RGB24> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
            unsafe void IPixelFactory<BGR24,RGBA32>.Copy(ReadOnlySpan<BGR24> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
            unsafe void IPixelFactory<RGBA32,RGBA32>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBA32> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<ARGB32,RGBA32>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBA32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(RGBA32));
                }
            }
        }
        partial struct ARGB32
            : IPixelFactory<BGRA32,ARGB32>
            , IPixelFactory<RGBA128F,ARGB32>
            , IPixelFactory<BGR565,ARGB32>
            , IPixelFactory<BGRA5551,ARGB32>
            , IPixelFactory<BGRA4444,ARGB32>
            , IPixelFactory<RGB24,ARGB32>
            , IPixelFactory<BGR24,ARGB32>
            , IPixelFactory<RGBA32,ARGB32>
            , IPixelFactory<ARGB32,ARGB32>
        {
            ARGB32 IPixelFactory<BGRA32,ARGB32>.From(BGRA32 color) { return new ARGB32(color); }
            ARGB32 IPixelFactory<RGBA128F,ARGB32>.From(RGBA128F color) { return new ARGB32(color); }
            ARGB32 IPixelFactory<BGR565,ARGB32>.From(BGR565 color) { return new ARGB32(new BGRA32(color)); }
            ARGB32 IPixelFactory<BGRA5551,ARGB32>.From(BGRA5551 color) { return new ARGB32(new BGRA32(color)); }
            ARGB32 IPixelFactory<BGRA4444,ARGB32>.From(BGRA4444 color) { return new ARGB32(new BGRA32(color)); }
            ARGB32 IPixelFactory<RGB24,ARGB32>.From(RGB24 color) { return new ARGB32(new BGRA32(color)); }
            ARGB32 IPixelFactory<BGR24,ARGB32>.From(BGR24 color) { return new ARGB32(new BGRA32(color)); }
            ARGB32 IPixelFactory<RGBA32,ARGB32>.From(RGBA32 color) { return new ARGB32(new BGRA32(color)); }
            ARGB32 IPixelFactory<ARGB32,ARGB32>.From(ARGB32 color) { return color; }
            unsafe void IPixelFactory<BGRA32,ARGB32>.Copy(ReadOnlySpan<BGRA32> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<RGBA128F,ARGB32>.Copy(ReadOnlySpan<RGBA128F> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<BGR565,ARGB32>.Copy(ReadOnlySpan<BGR565> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<BGRA5551,ARGB32>.Copy(ReadOnlySpan<BGRA5551> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<BGRA4444,ARGB32>.Copy(ReadOnlySpan<BGRA4444> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<RGB24,ARGB32>.Copy(ReadOnlySpan<RGB24> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<BGR24,ARGB32>.Copy(ReadOnlySpan<BGR24> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<RGBA32,ARGB32>.Copy(ReadOnlySpan<RGBA32> src, Span<ARGB32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new ARGB32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(ARGB32));
                }
            }
            unsafe void IPixelFactory<ARGB32,ARGB32>.Copy(ReadOnlySpan<ARGB32> src, Span<ARGB32> dst)
            {
                src.CopyTo(dst);
            }
        }
        partial struct RGBP32
            : IPixelFactory<BGRA32,RGBP32>
            , IPixelFactory<RGBA128F,RGBP32>
            , IPixelFactory<BGR565,RGBP32>
            , IPixelFactory<BGRA5551,RGBP32>
            , IPixelFactory<BGRA4444,RGBP32>
            , IPixelFactory<RGB24,RGBP32>
            , IPixelFactory<BGR24,RGBP32>
            , IPixelFactory<RGBA32,RGBP32>
            , IPixelFactory<ARGB32,RGBP32>
        {
            RGBP32 IPixelFactory<BGRA32,RGBP32>.From(BGRA32 color) { return new RGBP32(color); }
            RGBP32 IPixelFactory<RGBA128F,RGBP32>.From(RGBA128F color) { return new RGBP32(color); }
            RGBP32 IPixelFactory<BGR565,RGBP32>.From(BGR565 color) { return new RGBP32(new BGRA32(color)); }
            RGBP32 IPixelFactory<BGRA5551,RGBP32>.From(BGRA5551 color) { return new RGBP32(new BGRA32(color)); }
            RGBP32 IPixelFactory<BGRA4444,RGBP32>.From(BGRA4444 color) { return new RGBP32(new BGRA32(color)); }
            RGBP32 IPixelFactory<RGB24,RGBP32>.From(RGB24 color) { return new RGBP32(new BGRA32(color)); }
            RGBP32 IPixelFactory<BGR24,RGBP32>.From(BGR24 color) { return new RGBP32(new BGRA32(color)); }
            RGBP32 IPixelFactory<RGBA32,RGBP32>.From(RGBA32 color) { return new RGBP32(new BGRA32(color)); }
            RGBP32 IPixelFactory<ARGB32,RGBP32>.From(ARGB32 color) { return new RGBP32(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,RGBP32>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<RGBA128F,RGBP32>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<BGR565,RGBP32>.Copy(ReadOnlySpan<BGR565> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<BGRA5551,RGBP32>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<BGRA4444,RGBP32>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<RGB24,RGBP32>.Copy(ReadOnlySpan<RGB24> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<BGR24,RGBP32>.Copy(ReadOnlySpan<BGR24> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<RGBA32,RGBP32>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
            unsafe void IPixelFactory<ARGB32,RGBP32>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(RGBP32));
                }
            }
        }
        partial struct BGRP32
            : IPixelFactory<BGRA32,BGRP32>
            , IPixelFactory<RGBA128F,BGRP32>
            , IPixelFactory<BGR565,BGRP32>
            , IPixelFactory<BGRA5551,BGRP32>
            , IPixelFactory<BGRA4444,BGRP32>
            , IPixelFactory<RGB24,BGRP32>
            , IPixelFactory<BGR24,BGRP32>
            , IPixelFactory<RGBA32,BGRP32>
            , IPixelFactory<ARGB32,BGRP32>
        {
            BGRP32 IPixelFactory<BGRA32,BGRP32>.From(BGRA32 color) { return new BGRP32(color); }
            BGRP32 IPixelFactory<RGBA128F,BGRP32>.From(RGBA128F color) { return new BGRP32(color); }
            BGRP32 IPixelFactory<BGR565,BGRP32>.From(BGR565 color) { return new BGRP32(new BGRA32(color)); }
            BGRP32 IPixelFactory<BGRA5551,BGRP32>.From(BGRA5551 color) { return new BGRP32(new BGRA32(color)); }
            BGRP32 IPixelFactory<BGRA4444,BGRP32>.From(BGRA4444 color) { return new BGRP32(new BGRA32(color)); }
            BGRP32 IPixelFactory<RGB24,BGRP32>.From(RGB24 color) { return new BGRP32(new BGRA32(color)); }
            BGRP32 IPixelFactory<BGR24,BGRP32>.From(BGR24 color) { return new BGRP32(new BGRA32(color)); }
            BGRP32 IPixelFactory<RGBA32,BGRP32>.From(RGBA32 color) { return new BGRP32(new BGRA32(color)); }
            BGRP32 IPixelFactory<ARGB32,BGRP32>.From(ARGB32 color) { return new BGRP32(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,BGRP32>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<RGBA128F,BGRP32>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<BGR565,BGRP32>.Copy(ReadOnlySpan<BGR565> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<BGRA5551,BGRP32>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<BGRA4444,BGRP32>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<RGB24,BGRP32>.Copy(ReadOnlySpan<RGB24> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<BGR24,BGRP32>.Copy(ReadOnlySpan<BGR24> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<RGBA32,BGRP32>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGRP32>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRP32> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRP32(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGRP32));
                }
            }
        }
        partial struct RGB96F
            : IPixelFactory<BGRA32,RGB96F>
            , IPixelFactory<RGBA128F,RGB96F>
            , IPixelFactory<BGR565,RGB96F>
            , IPixelFactory<BGRA5551,RGB96F>
            , IPixelFactory<BGRA4444,RGB96F>
            , IPixelFactory<RGB24,RGB96F>
            , IPixelFactory<BGR24,RGB96F>
            , IPixelFactory<RGBA32,RGB96F>
            , IPixelFactory<ARGB32,RGB96F>
        {
            RGB96F IPixelFactory<BGRA32,RGB96F>.From(BGRA32 color) { return new RGB96F(color); }
            RGB96F IPixelFactory<RGBA128F,RGB96F>.From(RGBA128F color) { return new RGB96F(color); }
            RGB96F IPixelFactory<BGR565,RGB96F>.From(BGR565 color) { return new RGB96F(new BGRA32(color)); }
            RGB96F IPixelFactory<BGRA5551,RGB96F>.From(BGRA5551 color) { return new RGB96F(new BGRA32(color)); }
            RGB96F IPixelFactory<BGRA4444,RGB96F>.From(BGRA4444 color) { return new RGB96F(new BGRA32(color)); }
            RGB96F IPixelFactory<RGB24,RGB96F>.From(RGB24 color) { return new RGB96F(new BGRA32(color)); }
            RGB96F IPixelFactory<BGR24,RGB96F>.From(BGR24 color) { return new RGB96F(new BGRA32(color)); }
            RGB96F IPixelFactory<RGBA32,RGB96F>.From(RGBA32 color) { return new RGB96F(new BGRA32(color)); }
            RGB96F IPixelFactory<ARGB32,RGB96F>.From(ARGB32 color) { return new RGB96F(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,RGB96F>.Copy(ReadOnlySpan<BGRA32> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
            unsafe void IPixelFactory<RGBA128F,RGB96F>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
            unsafe void IPixelFactory<BGR565,RGB96F>.Copy(ReadOnlySpan<BGR565> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
            unsafe void IPixelFactory<BGRA5551,RGB96F>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
            unsafe void IPixelFactory<BGRA4444,RGB96F>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
            unsafe void IPixelFactory<RGB24,RGB96F>.Copy(ReadOnlySpan<RGB24> src, Span<RGB96F> dst)
            {
                Vector4Streaming.CopyByteToUnit(src.AsBytes(), dst.AsSingles());
            }
            unsafe void IPixelFactory<BGR24,RGB96F>.Copy(ReadOnlySpan<BGR24> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
            unsafe void IPixelFactory<RGBA32,RGB96F>.Copy(ReadOnlySpan<RGBA32> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
            unsafe void IPixelFactory<ARGB32,RGB96F>.Copy(ReadOnlySpan<ARGB32> src, Span<RGB96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGB96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(RGB96F));
                }
            }
        }
        partial struct BGR96F
            : IPixelFactory<BGRA32,BGR96F>
            , IPixelFactory<RGBA128F,BGR96F>
            , IPixelFactory<BGR565,BGR96F>
            , IPixelFactory<BGRA5551,BGR96F>
            , IPixelFactory<BGRA4444,BGR96F>
            , IPixelFactory<RGB24,BGR96F>
            , IPixelFactory<BGR24,BGR96F>
            , IPixelFactory<RGBA32,BGR96F>
            , IPixelFactory<ARGB32,BGR96F>
        {
            BGR96F IPixelFactory<BGRA32,BGR96F>.From(BGRA32 color) { return new BGR96F(color); }
            BGR96F IPixelFactory<RGBA128F,BGR96F>.From(RGBA128F color) { return new BGR96F(color); }
            BGR96F IPixelFactory<BGR565,BGR96F>.From(BGR565 color) { return new BGR96F(new BGRA32(color)); }
            BGR96F IPixelFactory<BGRA5551,BGR96F>.From(BGRA5551 color) { return new BGR96F(new BGRA32(color)); }
            BGR96F IPixelFactory<BGRA4444,BGR96F>.From(BGRA4444 color) { return new BGR96F(new BGRA32(color)); }
            BGR96F IPixelFactory<RGB24,BGR96F>.From(RGB24 color) { return new BGR96F(new BGRA32(color)); }
            BGR96F IPixelFactory<BGR24,BGR96F>.From(BGR24 color) { return new BGR96F(new BGRA32(color)); }
            BGR96F IPixelFactory<RGBA32,BGR96F>.From(RGBA32 color) { return new BGR96F(new BGRA32(color)); }
            BGR96F IPixelFactory<ARGB32,BGR96F>.From(ARGB32 color) { return new BGR96F(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,BGR96F>.Copy(ReadOnlySpan<BGRA32> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
            unsafe void IPixelFactory<RGBA128F,BGR96F>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
            unsafe void IPixelFactory<BGR565,BGR96F>.Copy(ReadOnlySpan<BGR565> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
            unsafe void IPixelFactory<BGRA5551,BGR96F>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
            unsafe void IPixelFactory<BGRA4444,BGR96F>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
            unsafe void IPixelFactory<RGB24,BGR96F>.Copy(ReadOnlySpan<RGB24> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
            unsafe void IPixelFactory<BGR24,BGR96F>.Copy(ReadOnlySpan<BGR24> src, Span<BGR96F> dst)
            {
                Vector4Streaming.CopyByteToUnit(src.AsBytes(), dst.AsSingles());
            }
            unsafe void IPixelFactory<RGBA32,BGR96F>.Copy(ReadOnlySpan<RGBA32> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGR96F>.Copy(ReadOnlySpan<ARGB32> src, Span<BGR96F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGR96F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGR96F));
                }
            }
        }
        partial struct BGRA128F
            : IPixelFactory<BGRA32,BGRA128F>
            , IPixelFactory<RGBA128F,BGRA128F>
            , IPixelFactory<BGR565,BGRA128F>
            , IPixelFactory<BGRA5551,BGRA128F>
            , IPixelFactory<BGRA4444,BGRA128F>
            , IPixelFactory<RGB24,BGRA128F>
            , IPixelFactory<BGR24,BGRA128F>
            , IPixelFactory<RGBA32,BGRA128F>
            , IPixelFactory<ARGB32,BGRA128F>
        {
            BGRA128F IPixelFactory<BGRA32,BGRA128F>.From(BGRA32 color) { return new BGRA128F(color); }
            BGRA128F IPixelFactory<RGBA128F,BGRA128F>.From(RGBA128F color) { return new BGRA128F(color); }
            BGRA128F IPixelFactory<BGR565,BGRA128F>.From(BGR565 color) { return new BGRA128F(new BGRA32(color)); }
            BGRA128F IPixelFactory<BGRA5551,BGRA128F>.From(BGRA5551 color) { return new BGRA128F(new BGRA32(color)); }
            BGRA128F IPixelFactory<BGRA4444,BGRA128F>.From(BGRA4444 color) { return new BGRA128F(new BGRA32(color)); }
            BGRA128F IPixelFactory<RGB24,BGRA128F>.From(RGB24 color) { return new BGRA128F(new BGRA32(color)); }
            BGRA128F IPixelFactory<BGR24,BGRA128F>.From(BGR24 color) { return new BGRA128F(new BGRA32(color)); }
            BGRA128F IPixelFactory<RGBA32,BGRA128F>.From(RGBA32 color) { return new BGRA128F(new BGRA32(color)); }
            BGRA128F IPixelFactory<ARGB32,BGRA128F>.From(ARGB32 color) { return new BGRA128F(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,BGRA128F>.Copy(ReadOnlySpan<BGRA32> src, Span<BGRA128F> dst)
            {
                Vector4Streaming.CopyByteToUnit(src.AsBytes(), dst.AsSingles());
            }
            unsafe void IPixelFactory<RGBA128F,BGRA128F>.Copy(ReadOnlySpan<RGBA128F> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
            unsafe void IPixelFactory<BGR565,BGRA128F>.Copy(ReadOnlySpan<BGR565> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
            unsafe void IPixelFactory<BGRA5551,BGRA128F>.Copy(ReadOnlySpan<BGRA5551> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
            unsafe void IPixelFactory<BGRA4444,BGRA128F>.Copy(ReadOnlySpan<BGRA4444> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
            unsafe void IPixelFactory<RGB24,BGRA128F>.Copy(ReadOnlySpan<RGB24> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
            unsafe void IPixelFactory<BGR24,BGRA128F>.Copy(ReadOnlySpan<BGR24> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
            unsafe void IPixelFactory<RGBA32,BGRA128F>.Copy(ReadOnlySpan<RGBA32> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
            unsafe void IPixelFactory<ARGB32,BGRA128F>.Copy(ReadOnlySpan<ARGB32> src, Span<BGRA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new BGRA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(BGRA128F));
                }
            }
        }
        partial struct RGBA128F
            : IPixelFactory<BGRA32,RGBA128F>
            , IPixelFactory<RGBA128F,RGBA128F>
            , IPixelFactory<BGR565,RGBA128F>
            , IPixelFactory<BGRA5551,RGBA128F>
            , IPixelFactory<BGRA4444,RGBA128F>
            , IPixelFactory<RGB24,RGBA128F>
            , IPixelFactory<BGR24,RGBA128F>
            , IPixelFactory<RGBA32,RGBA128F>
            , IPixelFactory<ARGB32,RGBA128F>
        {
            RGBA128F IPixelFactory<BGRA32,RGBA128F>.From(BGRA32 color) { return new RGBA128F(color); }
            RGBA128F IPixelFactory<RGBA128F,RGBA128F>.From(RGBA128F color) { return color; }
            RGBA128F IPixelFactory<BGR565,RGBA128F>.From(BGR565 color) { return new RGBA128F(new BGRA32(color)); }
            RGBA128F IPixelFactory<BGRA5551,RGBA128F>.From(BGRA5551 color) { return new RGBA128F(new BGRA32(color)); }
            RGBA128F IPixelFactory<BGRA4444,RGBA128F>.From(BGRA4444 color) { return new RGBA128F(new BGRA32(color)); }
            RGBA128F IPixelFactory<RGB24,RGBA128F>.From(RGB24 color) { return new RGBA128F(new BGRA32(color)); }
            RGBA128F IPixelFactory<BGR24,RGBA128F>.From(BGR24 color) { return new RGBA128F(new BGRA32(color)); }
            RGBA128F IPixelFactory<RGBA32,RGBA128F>.From(RGBA32 color) { return new RGBA128F(new BGRA32(color)); }
            RGBA128F IPixelFactory<ARGB32,RGBA128F>.From(ARGB32 color) { return new RGBA128F(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,RGBA128F>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA128F(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(RGBA128F));
                }
            }
            unsafe void IPixelFactory<RGBA128F,RGBA128F>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBA128F> dst)
            {
                src.CopyTo(dst);
            }
            unsafe void IPixelFactory<BGR565,RGBA128F>.Copy(ReadOnlySpan<BGR565> src, Span<RGBA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(RGBA128F));
                }
            }
            unsafe void IPixelFactory<BGRA5551,RGBA128F>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(RGBA128F));
                }
            }
            unsafe void IPixelFactory<BGRA4444,RGBA128F>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(RGBA128F));
                }
            }
            unsafe void IPixelFactory<RGB24,RGBA128F>.Copy(ReadOnlySpan<RGB24> src, Span<RGBA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(RGBA128F));
                }
            }
            unsafe void IPixelFactory<BGR24,RGBA128F>.Copy(ReadOnlySpan<BGR24> src, Span<RGBA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(RGBA128F));
                }
            }
            unsafe void IPixelFactory<RGBA32,RGBA128F>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBA128F> dst)
            {
                Vector4Streaming.CopyByteToUnit(src.AsBytes(), dst.AsSingles());
            }
            unsafe void IPixelFactory<ARGB32,RGBA128F>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBA128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBA128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(RGBA128F));
                }
            }
        }
        partial struct RGBP128F
            : IPixelFactory<BGRA32,RGBP128F>
            , IPixelFactory<RGBA128F,RGBP128F>
            , IPixelFactory<BGR565,RGBP128F>
            , IPixelFactory<BGRA5551,RGBP128F>
            , IPixelFactory<BGRA4444,RGBP128F>
            , IPixelFactory<RGB24,RGBP128F>
            , IPixelFactory<BGR24,RGBP128F>
            , IPixelFactory<RGBA32,RGBP128F>
            , IPixelFactory<ARGB32,RGBP128F>
        {
            RGBP128F IPixelFactory<BGRA32,RGBP128F>.From(BGRA32 color) { return new RGBP128F(color); }
            RGBP128F IPixelFactory<RGBA128F,RGBP128F>.From(RGBA128F color) { return new RGBP128F(color); }
            RGBP128F IPixelFactory<BGR565,RGBP128F>.From(BGR565 color) { return new RGBP128F(new BGRA32(color)); }
            RGBP128F IPixelFactory<BGRA5551,RGBP128F>.From(BGRA5551 color) { return new RGBP128F(new BGRA32(color)); }
            RGBP128F IPixelFactory<BGRA4444,RGBP128F>.From(BGRA4444 color) { return new RGBP128F(new BGRA32(color)); }
            RGBP128F IPixelFactory<RGB24,RGBP128F>.From(RGB24 color) { return new RGBP128F(new BGRA32(color)); }
            RGBP128F IPixelFactory<BGR24,RGBP128F>.From(BGR24 color) { return new RGBP128F(new BGRA32(color)); }
            RGBP128F IPixelFactory<RGBA32,RGBP128F>.From(RGBA32 color) { return new RGBP128F(new BGRA32(color)); }
            RGBP128F IPixelFactory<ARGB32,RGBP128F>.From(ARGB32 color) { return new RGBP128F(new BGRA32(color)); }
            unsafe void IPixelFactory<BGRA32,RGBP128F>.Copy(ReadOnlySpan<BGRA32> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(src[0]);
                    src = src.Slice(sizeof(BGRA32));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<RGBA128F,RGBP128F>.Copy(ReadOnlySpan<RGBA128F> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(src[0]);
                    src = src.Slice(sizeof(RGBA128F));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<BGR565,RGBP128F>.Copy(ReadOnlySpan<BGR565> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR565));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<BGRA5551,RGBP128F>.Copy(ReadOnlySpan<BGRA5551> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA5551));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<BGRA4444,RGBP128F>.Copy(ReadOnlySpan<BGRA4444> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGRA4444));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<RGB24,RGBP128F>.Copy(ReadOnlySpan<RGB24> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGB24));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<BGR24,RGBP128F>.Copy(ReadOnlySpan<BGR24> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(BGR24));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<RGBA32,RGBP128F>.Copy(ReadOnlySpan<RGBA32> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(RGBA32));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
            unsafe void IPixelFactory<ARGB32,RGBP128F>.Copy(ReadOnlySpan<ARGB32> src, Span<RGBP128F> dst)
            {
                while(dst.Length > 0)
                {
                    dst[0] = new RGBP128F(new BGRA32(src[0]));
                    src = src.Slice(sizeof(ARGB32));
                    dst = dst.Slice(sizeof(RGBP128F));
                }
            }
        }
    }
}
