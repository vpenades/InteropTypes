using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Codecs;

using NUnit.Framework;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

using POINTF = SixLabors.ImageSharp.PointF;

namespace InteropTypes.Graphics.Bitmaps
{
    static class TestUtils
    {
        public static void AttachToCurrentTestAll(this SpanBitmap bmp, string filePath)
        {
            var mem = bmp.ToMemoryBitmap();

            TestContext.WriteLine($"{filePath} {bmp.Info.ToDebuggerDisplayString()}");


            if (bmp.PixelFormat == Pixel.BGR96F.Format || bmp.PixelFormat == Pixel.RGB96F.Format)
            {
                var tmp = new MemoryBitmap(bmp.Width, bmp.Height, Pixel.BGR24.Format);
                SpanBitmap.CopyPixels(bmp.OfType<System.Numerics.Vector3>(), tmp, (0, 1), (0, 255));
                bmp = tmp;
            }

            AttachmentInfo _injectExt(string fp, string extPrefix)
            {
                var ext = System.IO.Path.GetExtension(fp);
                fp = fp.Substring(0, fp.Length - ext.Length);
                return new AttachmentInfo($"{fp}.{extPrefix}{ext}");
            }

            var f1 = _injectExt(filePath, "WPF");
            mem.Save(f1, WPFCodec.Default);

            var f2 = _injectExt(filePath, "GDI");
            mem.Save(f2, GDICodec.Default);

            var f3 = _injectExt(filePath, "ImageSharp");
            mem.Save(f3, ImageSharpCodec.Default);

            var f4 = _injectExt(filePath, "SkiaSharp");
            mem.Save(f4, SkiaCodec.Default);

            var f5 = _injectExt(filePath, "OpenCvSharp");
            mem.Save(f5, OpenCvCodec.Default);

            var f6 = _injectExt(filePath, "STB");
            mem.Save(f6, STBCodec.WithQuality(80));

            // TODO: it should compare saved files against bmp
        }

        public static System.IO.FileInfo WriteImage(this AttachmentInfo ainfo, MemoryBitmap image)
        {
            return ainfo.WriteObject(f => image.Save(f.FullName));
        }

        public static System.IO.FileInfo WriteImage(this AttachmentInfo ainfo, Image image)
        {
            return ainfo.WriteObject(f => image.Save(f.FullName));
        }

        public static System.IO.FileInfo WriteVideo(this AttachmentInfo ainfo, IEnumerable<PointerBitmap> frames)
        {
            return ainfo.WriteObject(finfo => FFmpegAutoGen.EncodeFrames(finfo.FullName, frames));
        }

        public static System.IO.FileInfo WriteAVI(this AttachmentInfo ainfo, IEnumerable<MemoryBitmap> frames, float frameRate = 25)
        {
            void _saveVideo(System.IO.FileInfo finfo)
            {
                MJpegAviFrameWriter.SaveToAVI(finfo.FullName, frames, (decimal)frameRate, new GDICodec(50));
            }

            return ainfo.WriteObject(_saveVideo);
        }
    }

    static class ImageSharpUtils
    {
        public static IImageProcessingContext FillPolygon(this IImageProcessingContext source, Color color, params (float,float)[] points)
        {
            var ppp = points
                .Select(item => new POINTF(item.Item1, item.Item2))
                .ToArray();            

            return source.FillPolygon(color, ppp);
        }

        public static IImageProcessingContext DrawPolygon(this IImageProcessingContext source, Color color, float thickness, params (float, float)[] points)
        {
            var ppp = points
                .Select(item => new POINTF(item.Item1, item.Item2))
                .ToArray();

            return source.DrawPolygon(color, thickness, ppp);
        }        
    }    

    

    readonly struct PerformanceBenchmark : IDisposable
    {
        public static PerformanceBenchmark Run(Action<TimeSpan> onCompleted)
        {
            return new PerformanceBenchmark(onCompleted);
        }

        private PerformanceBenchmark(Action<TimeSpan> onCompleted)
        {
            _OnCompleted = onCompleted;
            _Timer = System.Diagnostics.Stopwatch.StartNew();
        }

        public void Dispose()
        {
            var elapsed = _Timer.Elapsed;

            if (_OnCompleted != null) _OnCompleted(elapsed);
        }

        private readonly Action<TimeSpan> _OnCompleted;

        private readonly System.Diagnostics.Stopwatch _Timer;        
    }
}
