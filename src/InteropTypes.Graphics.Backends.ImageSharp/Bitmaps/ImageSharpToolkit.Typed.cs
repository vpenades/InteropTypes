
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
        public static MemoryBitmap<Pixel.BGR565> ToMemoryBitmap(this Image<Bgr565> image) { return ToMemoryBitmap<Pixel.BGR565>(image); }
        public static MemoryBitmap<Pixel.BGRA5551> ToMemoryBitmap(this Image<Bgra5551> image) { return ToMemoryBitmap<Pixel.BGRA5551>(image); }
        public static MemoryBitmap<Pixel.BGRA4444> ToMemoryBitmap(this Image<Bgra4444> image) { return ToMemoryBitmap<Pixel.BGRA4444>(image); }
        public static MemoryBitmap<Pixel.BGR24> ToMemoryBitmap(this Image<Bgr24> image) { return ToMemoryBitmap<Pixel.BGR24>(image); }
        public static MemoryBitmap<Pixel.RGB24> ToMemoryBitmap(this Image<Rgb24> image) { return ToMemoryBitmap<Pixel.RGB24>(image); }
        public static MemoryBitmap<Pixel.BGRA32> ToMemoryBitmap(this Image<Bgra32> image) { return ToMemoryBitmap<Pixel.BGRA32>(image); }
        public static MemoryBitmap<Pixel.RGBA32> ToMemoryBitmap(this Image<Rgba32> image) { return ToMemoryBitmap<Pixel.RGBA32>(image); }
        public static MemoryBitmap<Pixel.ARGB32> ToMemoryBitmap(this Image<Argb32> image) { return ToMemoryBitmap<Pixel.ARGB32>(image); }
        public static MemoryBitmap<Pixel.RGBA128F> ToMemoryBitmap(this Image<RgbaVector> image) { return ToMemoryBitmap<Pixel.RGBA128F>(image); }
        public static bool TryWrapAsSpanBitmap(this Image<A8> self, SpanBitmap<Pixel.Alpha8>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<L8> self, SpanBitmap<Pixel.Luminance8>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<L16> self, SpanBitmap<Pixel.Luminance16>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Bgr565> self, SpanBitmap<Pixel.BGR565>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Bgra5551> self, SpanBitmap<Pixel.BGRA5551>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Bgra4444> self, SpanBitmap<Pixel.BGRA4444>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static bool TryWrapAsSpanBitmap(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F>.Action1 action) { return _Implementation.TryWrapAsSpanBitmap(self, action); }
        public static void ReadAsSpanBitmap(this Image<A8> self, SpanBitmap<Pixel.Alpha8>.Action1 action) { _Implementation.ReadAsSpanBitmap<A8, Pixel.Alpha8, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<L8> self, SpanBitmap<Pixel.Luminance8>.Action1 action) { _Implementation.ReadAsSpanBitmap<L8, Pixel.Luminance8, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<L16> self, SpanBitmap<Pixel.Luminance16>.Action1 action) { _Implementation.ReadAsSpanBitmap<L16, Pixel.Luminance16, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Bgr565> self, SpanBitmap<Pixel.BGR565>.Action1 action) { _Implementation.ReadAsSpanBitmap<Bgr565, Pixel.BGR565, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Bgra5551> self, SpanBitmap<Pixel.BGRA5551>.Action1 action) { _Implementation.ReadAsSpanBitmap<Bgra5551, Pixel.BGRA5551, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Bgra4444> self, SpanBitmap<Pixel.BGRA4444>.Action1 action) { _Implementation.ReadAsSpanBitmap<Bgra4444, Pixel.BGRA4444, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24>.Action1 action) { _Implementation.ReadAsSpanBitmap<Bgr24, Pixel.BGR24, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24>.Action1 action) { _Implementation.ReadAsSpanBitmap<Rgb24, Pixel.RGB24, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32>.Action1 action) { _Implementation.ReadAsSpanBitmap<Bgra32, Pixel.BGRA32, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32>.Action1 action) { _Implementation.ReadAsSpanBitmap<Rgba32, Pixel.RGBA32, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32>.Action1 action) { _Implementation.ReadAsSpanBitmap<Argb32, Pixel.ARGB32, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static void ReadAsSpanBitmap(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F>.Action1 action) { _Implementation.ReadAsSpanBitmap<RgbaVector, Pixel.RGBA128F, int>(self,default, (a,b) => { action(a); return 0; } ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<A8> self, SpanBitmap<Pixel.Alpha8>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<A8, Pixel.Alpha8, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L8> self, SpanBitmap<Pixel.Luminance8>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<L8, Pixel.Luminance8, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L16> self, SpanBitmap<Pixel.Luminance16>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<L16, Pixel.Luminance16, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgr565> self, SpanBitmap<Pixel.BGR565>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgr565, Pixel.BGR565, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra5551> self, SpanBitmap<Pixel.BGRA5551>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra5551, Pixel.BGRA5551, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra4444> self, SpanBitmap<Pixel.BGRA4444>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra4444, Pixel.BGRA4444, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgr24, Pixel.BGR24, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgb24, Pixel.RGB24, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra32, Pixel.BGRA32, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgba32, Pixel.RGBA32, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<Argb32, Pixel.ARGB32, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F>.Function1<TResult> function) { return _Implementation.ReadAsSpanBitmap<RgbaVector, Pixel.RGBA128F, TResult>(self,default, (a,b) => function(a) ); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<A8> self, SpanBitmap<Pixel.Alpha8> other, SpanBitmap<Pixel.Alpha8>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<A8, Pixel.Alpha8, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L8> self, SpanBitmap<Pixel.Luminance8> other, SpanBitmap<Pixel.Luminance8>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<L8, Pixel.Luminance8, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<L16> self, SpanBitmap<Pixel.Luminance16> other, SpanBitmap<Pixel.Luminance16>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<L16, Pixel.Luminance16, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgr565> self, SpanBitmap<Pixel.BGR565> other, SpanBitmap<Pixel.BGR565>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgr565, Pixel.BGR565, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra5551> self, SpanBitmap<Pixel.BGRA5551> other, SpanBitmap<Pixel.BGRA5551>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra5551, Pixel.BGRA5551, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra4444> self, SpanBitmap<Pixel.BGRA4444> other, SpanBitmap<Pixel.BGRA4444>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra4444, Pixel.BGRA4444, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24> other, SpanBitmap<Pixel.BGR24>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgr24, Pixel.BGR24, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24> other, SpanBitmap<Pixel.RGB24>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgb24, Pixel.RGB24, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32> other, SpanBitmap<Pixel.BGRA32>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Bgra32, Pixel.BGRA32, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32> other, SpanBitmap<Pixel.RGBA32>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Rgba32, Pixel.RGBA32, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32> other, SpanBitmap<Pixel.ARGB32>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<Argb32, Pixel.ARGB32, TResult>(self, other, function); }
        public static TResult ReadAsSpanBitmap<TResult>(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F> other, SpanBitmap<Pixel.RGBA128F>.Function2<TResult> function) { return _Implementation.ReadAsSpanBitmap<RgbaVector, Pixel.RGBA128F, TResult>(self, other, function); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.Alpha8> self, Action<Image<A8>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.Alpha8, A8, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.Luminance8> self, Action<Image<L8>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.Luminance8, L8, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.Luminance16> self, Action<Image<L16>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.Luminance16, L16, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.BGR565> self, Action<Image<Bgr565>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGR565, Bgr565, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.BGRA5551> self, Action<Image<Bgra5551>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGRA5551, Bgra5551, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.BGRA4444> self, Action<Image<Bgra4444>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGRA4444, Bgra4444, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.BGR24> self, Action<Image<Bgr24>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGR24, Bgr24, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.RGB24> self, Action<Image<Rgb24>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.RGB24, Rgb24, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.BGRA32> self, Action<Image<Bgra32>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGRA32, Bgra32, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.RGBA32> self, Action<Image<Rgba32>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.RGBA32, Rgba32, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.ARGB32> self, Action<Image<Argb32>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.ARGB32, Argb32, int>(self, img => { action(img); return 0; } ); }
        public static void ReadAsImageSharp(this SpanBitmap<Pixel.RGBA128F> self, Action<Image<RgbaVector>> action) { _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.RGBA128F, RgbaVector, int>(self, img => { action(img); return 0; } ); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.Alpha8> self, Func<Image<A8>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.Alpha8, A8, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.Luminance8> self, Func<Image<L8>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.Luminance8, L8, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.Luminance16> self, Func<Image<L16>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.Luminance16, L16, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.BGR565> self, Func<Image<Bgr565>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGR565, Bgr565, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.BGRA5551> self, Func<Image<Bgra5551>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGRA5551, Bgra5551, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.BGRA4444> self, Func<Image<Bgra4444>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGRA4444, Bgra4444, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.BGR24> self, Func<Image<Bgr24>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGR24, Bgr24, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.RGB24> self, Func<Image<Rgb24>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.RGB24, Rgb24, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.BGRA32> self, Func<Image<Bgra32>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.BGRA32, Bgra32, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.RGBA32> self, Func<Image<Rgba32>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.RGBA32, Rgba32, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.ARGB32> self, Func<Image<Argb32>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.ARGB32, Argb32, TResult>(self, function); }
        public static TResult ReadAsImageSharp<TResult>(this SpanBitmap<Pixel.RGBA128F> self, Func<Image<RgbaVector>, TResult> function) { return _ImageSharpChangedMonitor.ReadAsImageSharp<Pixel.RGBA128F, RgbaVector, TResult>(self, function); }
        public static void WriteAsSpanBitmap(this Image<A8> self, SpanBitmap<Pixel.Alpha8>.Action1 action) { _Implementation.WriteAsSpanBitmap<A8, Pixel.Alpha8>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<L8> self, SpanBitmap<Pixel.Luminance8>.Action1 action) { _Implementation.WriteAsSpanBitmap<L8, Pixel.Luminance8>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<L16> self, SpanBitmap<Pixel.Luminance16>.Action1 action) { _Implementation.WriteAsSpanBitmap<L16, Pixel.Luminance16>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Bgr565> self, SpanBitmap<Pixel.BGR565>.Action1 action) { _Implementation.WriteAsSpanBitmap<Bgr565, Pixel.BGR565>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Bgra5551> self, SpanBitmap<Pixel.BGRA5551>.Action1 action) { _Implementation.WriteAsSpanBitmap<Bgra5551, Pixel.BGRA5551>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Bgra4444> self, SpanBitmap<Pixel.BGRA4444>.Action1 action) { _Implementation.WriteAsSpanBitmap<Bgra4444, Pixel.BGRA4444>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24>.Action1 action) { _Implementation.WriteAsSpanBitmap<Bgr24, Pixel.BGR24>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24>.Action1 action) { _Implementation.WriteAsSpanBitmap<Rgb24, Pixel.RGB24>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32>.Action1 action) { _Implementation.WriteAsSpanBitmap<Bgra32, Pixel.BGRA32>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32>.Action1 action) { _Implementation.WriteAsSpanBitmap<Rgba32, Pixel.RGBA32>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32>.Action1 action) { _Implementation.WriteAsSpanBitmap<Argb32, Pixel.ARGB32>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F>.Action1 action) { _Implementation.WriteAsSpanBitmap<RgbaVector, Pixel.RGBA128F>(self, default, (a,b) => action(a) ); }
        public static void WriteAsSpanBitmap(this Image<A8> self, SpanBitmap<Pixel.Alpha8> other, SpanBitmap<Pixel.Alpha8>.Action2 action) { _Implementation.WriteAsSpanBitmap<A8, Pixel.Alpha8>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<L8> self, SpanBitmap<Pixel.Luminance8> other, SpanBitmap<Pixel.Luminance8>.Action2 action) { _Implementation.WriteAsSpanBitmap<L8, Pixel.Luminance8>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<L16> self, SpanBitmap<Pixel.Luminance16> other, SpanBitmap<Pixel.Luminance16>.Action2 action) { _Implementation.WriteAsSpanBitmap<L16, Pixel.Luminance16>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Bgr565> self, SpanBitmap<Pixel.BGR565> other, SpanBitmap<Pixel.BGR565>.Action2 action) { _Implementation.WriteAsSpanBitmap<Bgr565, Pixel.BGR565>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Bgra5551> self, SpanBitmap<Pixel.BGRA5551> other, SpanBitmap<Pixel.BGRA5551>.Action2 action) { _Implementation.WriteAsSpanBitmap<Bgra5551, Pixel.BGRA5551>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Bgra4444> self, SpanBitmap<Pixel.BGRA4444> other, SpanBitmap<Pixel.BGRA4444>.Action2 action) { _Implementation.WriteAsSpanBitmap<Bgra4444, Pixel.BGRA4444>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Bgr24> self, SpanBitmap<Pixel.BGR24> other, SpanBitmap<Pixel.BGR24>.Action2 action) { _Implementation.WriteAsSpanBitmap<Bgr24, Pixel.BGR24>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Rgb24> self, SpanBitmap<Pixel.RGB24> other, SpanBitmap<Pixel.RGB24>.Action2 action) { _Implementation.WriteAsSpanBitmap<Rgb24, Pixel.RGB24>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Bgra32> self, SpanBitmap<Pixel.BGRA32> other, SpanBitmap<Pixel.BGRA32>.Action2 action) { _Implementation.WriteAsSpanBitmap<Bgra32, Pixel.BGRA32>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Rgba32> self, SpanBitmap<Pixel.RGBA32> other, SpanBitmap<Pixel.RGBA32>.Action2 action) { _Implementation.WriteAsSpanBitmap<Rgba32, Pixel.RGBA32>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<Argb32> self, SpanBitmap<Pixel.ARGB32> other, SpanBitmap<Pixel.ARGB32>.Action2 action) { _Implementation.WriteAsSpanBitmap<Argb32, Pixel.ARGB32>(self, other, action); }
        public static void WriteAsSpanBitmap(this Image<RgbaVector> self, SpanBitmap<Pixel.RGBA128F> other, SpanBitmap<Pixel.RGBA128F>.Action2 action) { _Implementation.WriteAsSpanBitmap<RgbaVector, Pixel.RGBA128F>(self, other, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.Alpha8> self, Action<Image<A8>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.Alpha8, A8>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.Luminance8> self, Action<Image<L8>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.Luminance8, L8>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.Luminance16> self, Action<Image<L16>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.Luminance16, L16>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.BGR565> self, Action<Image<Bgr565>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.BGR565, Bgr565>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.BGRA5551> self, Action<Image<Bgra5551>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.BGRA5551, Bgra5551>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.BGRA4444> self, Action<Image<Bgra4444>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.BGRA4444, Bgra4444>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.BGR24> self, Action<Image<Bgr24>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.BGR24, Bgr24>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.RGB24> self, Action<Image<Rgb24>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.RGB24, Rgb24>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.BGRA32> self, Action<Image<Bgra32>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.BGRA32, Bgra32>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.RGBA32> self, Action<Image<Rgba32>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.RGBA32, Rgba32>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.ARGB32> self, Action<Image<Argb32>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.ARGB32, Argb32>(self, action); }
        public static void WriteAsImageSharp(this SpanBitmap<Pixel.RGBA128F> self, Action<Image<RgbaVector>> action) { _ImageSharpChangedMonitor.WriteAsImageSharp<Pixel.RGBA128F, RgbaVector>(self, action); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.Alpha8> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.Alpha8, A8>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.Luminance8> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.Luminance8, L8>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.Luminance16> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.Luminance16, L16>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.BGR565> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.BGR565, Bgr565>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.BGRA5551> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.BGRA5551, Bgra5551>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.BGRA4444> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.BGRA4444, Bgra4444>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.BGR24> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.BGR24, Bgr24>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.RGB24> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.RGB24, Rgb24>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.BGRA32> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.BGRA32, Bgra32>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.RGBA32> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.RGBA32, Rgba32>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.ARGB32> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.ARGB32, Argb32>(self, operation); }
        public static void MutateAsImageSharp(this SpanBitmap<Pixel.RGBA128F> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation) { _Implementation.MutateAsImageSharp<Pixel.RGBA128F, RgbaVector>(self, operation); }

    }
}