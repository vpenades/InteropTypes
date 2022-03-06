
using System;

using InteropTypes.Graphics.Bitmaps;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropTypes.Graphics.Backends
{
    partial class ImageSharpToolkit
    {
    
        public static MemoryBitmap<Pixel.Alpha8> ToMemoryBitmap(this Image<A8> image) { return ToMemoryBitmap<Pixel.Alpha8>(image); }
        public static MemoryBitmap<Pixel.Luminance8> ToMemoryBitmap(this Image<L8> image) { return ToMemoryBitmap<Pixel.Luminance8>(image); }
        public static MemoryBitmap<Pixel.Luminance16> ToMemoryBitmap(this Image<L16> image) { return ToMemoryBitmap<Pixel.Luminance16>(image); }
        public static MemoryBitmap<Pixel.BGR24> ToMemoryBitmap(this Image<Bgr24> image) { return ToMemoryBitmap<Pixel.BGR24>(image); }
        public static MemoryBitmap<Pixel.RGB24> ToMemoryBitmap(this Image<Rgb24> image) { return ToMemoryBitmap<Pixel.RGB24>(image); }
        public static MemoryBitmap<Pixel.BGRA32> ToMemoryBitmap(this Image<Bgra32> image) { return ToMemoryBitmap<Pixel.BGRA32>(image); }
        public static MemoryBitmap<Pixel.RGBA32> ToMemoryBitmap(this Image<Rgba32> image) { return ToMemoryBitmap<Pixel.RGBA32>(image); }
        public static MemoryBitmap<Pixel.ARGB32> ToMemoryBitmap(this Image<Argb32> image) { return ToMemoryBitmap<Pixel.ARGB32>(image); }
        public static MemoryBitmap<Pixel.RGBA128F> ToMemoryBitmap(this Image<RgbaVector> image) { return ToMemoryBitmap<Pixel.RGBA128F>(image); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<A8> self, SpanBitmap<Pixel.Alpha8>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<A8, Pixel.Alpha8, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L8> self, SpanBitmap<Pixel.Luminance8>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<L8, Pixel.Luminance8, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L16> self, SpanBitmap<Pixel.Luminance16>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<L16, Pixel.Luminance16, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgr24, Pixel.BGR24, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgb24, Pixel.RGB24, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra32, Pixel.BGRA32, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgba32, Pixel.RGBA32, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Argb32, Pixel.ARGB32, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<RgbaVector, Pixel.RGBA128F, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<A8> self, SpanBitmap<Pixel.Alpha8> other, SpanBitmap<Pixel.Alpha8>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<A8, Pixel.Alpha8, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L8> self, SpanBitmap<Pixel.Luminance8> other, SpanBitmap<Pixel.Luminance8>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<L8, Pixel.Luminance8, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L16> self, SpanBitmap<Pixel.Luminance16> other, SpanBitmap<Pixel.Luminance16>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<L16, Pixel.Luminance16, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24> other, SpanBitmap<Pixel.BGR24>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgr24, Pixel.BGR24, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24> other, SpanBitmap<Pixel.RGB24>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgb24, Pixel.RGB24, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32> other, SpanBitmap<Pixel.BGRA32>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra32, Pixel.BGRA32, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32> other, SpanBitmap<Pixel.RGBA32>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgba32, Pixel.RGBA32, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32> other, SpanBitmap<Pixel.ARGB32>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Argb32, Pixel.ARGB32, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F> other, SpanBitmap<Pixel.RGBA128F>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<RgbaVector, Pixel.RGBA128F, TResult>(self, other, function); }
        public static void WriteAsSpanBitmap(this Image<A8> self, SpanBitmap<Pixel.Alpha8>.Action1 action) { _Implementation.WriteAsSpanBitmap<A8, Pixel.Alpha8>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<L8> self, SpanBitmap<Pixel.Luminance8>.Action1 action) { _Implementation.WriteAsSpanBitmap<L8, Pixel.Luminance8>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<L16> self, SpanBitmap<Pixel.Luminance16>.Action1 action) { _Implementation.WriteAsSpanBitmap<L16, Pixel.Luminance16>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24>.Action1 action) { _Implementation.WriteAsSpanBitmap<Bgr24, Pixel.BGR24>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24>.Action1 action) { _Implementation.WriteAsSpanBitmap<Rgb24, Pixel.RGB24>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32>.Action1 action) { _Implementation.WriteAsSpanBitmap<Bgra32, Pixel.BGRA32>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32>.Action1 action) { _Implementation.WriteAsSpanBitmap<Rgba32, Pixel.RGBA32>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32>.Action1 action) { _Implementation.WriteAsSpanBitmap<Argb32, Pixel.ARGB32>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F>.Action1 action) { _Implementation.WriteAsSpanBitmap<RgbaVector, Pixel.RGBA128F>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<A8> self, SpanBitmap<Pixel.Alpha8> other, SpanBitmap<Pixel.Alpha8>.Action2 action) { _Implementation.WriteAsSpanBitmap<A8, Pixel.Alpha8>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<L8> self, SpanBitmap<Pixel.Luminance8> other, SpanBitmap<Pixel.Luminance8>.Action2 action) { _Implementation.WriteAsSpanBitmap<L8, Pixel.Luminance8>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<L16> self, SpanBitmap<Pixel.Luminance16> other, SpanBitmap<Pixel.Luminance16>.Action2 action) { _Implementation.WriteAsSpanBitmap<L16, Pixel.Luminance16>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24> other, SpanBitmap<Pixel.BGR24>.Action2 action) { _Implementation.WriteAsSpanBitmap<Bgr24, Pixel.BGR24>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24> other, SpanBitmap<Pixel.RGB24>.Action2 action) { _Implementation.WriteAsSpanBitmap<Rgb24, Pixel.RGB24>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32> other, SpanBitmap<Pixel.BGRA32>.Action2 action) { _Implementation.WriteAsSpanBitmap<Bgra32, Pixel.BGRA32>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32> other, SpanBitmap<Pixel.RGBA32>.Action2 action) { _Implementation.WriteAsSpanBitmap<Rgba32, Pixel.RGBA32>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32> other, SpanBitmap<Pixel.ARGB32>.Action2 action) { _Implementation.WriteAsSpanBitmap<Argb32, Pixel.ARGB32>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F> other, SpanBitmap<Pixel.RGBA128F>.Action2 action) { _Implementation.WriteAsSpanBitmap<RgbaVector, Pixel.RGBA128F>(self, other, action); }
        public static bool TryWrapAsSpanBitmap(this Image<A8> self, SpanBitmap<Pixel.Alpha8>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<L8> self, SpanBitmap<Pixel.Luminance8>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<L16> self, SpanBitmap<Pixel.Luminance16>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }

    }
}