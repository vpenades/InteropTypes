using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Codecs;

using NUnit.Framework;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

using POINTF = SixLabors.ImageSharp.PointF;

namespace InteropBitmaps
{
    static class TestUtils
    {
        public static string GetTestResultPath(this TestContext context, params string[] parts)
        {
            // https://github.com/nunit/nunit/issues/1768#issuecomment-242454699

            var path = System.IO.Path.Combine(context.WorkDirectory, "TestsOutput", context.Test.ID);

            System.IO.Directory.CreateDirectory(path);

            foreach (var part in parts) path = System.IO.Path.Combine(path, part);

            

            return path;
        }

        public static void AttachToCurrentTest(this Image image, string filePath)
        {
            filePath = TestContext.CurrentContext.GetTestResultPath(filePath);

            var dirPath = System.IO.Path.GetDirectoryName(filePath);
            System.IO.Directory.CreateDirectory(dirPath);

            ImageExtensions.Save(image, filePath);

            TestContext.AddTestAttachment(filePath);
        }

        public static void AttachToCurrentTest(this System.Drawing.Bitmap image, string filePath)
        {
            using var owner = image.UsingMemoryBitmap();

            owner.Bitmap.AttachToCurrentTest(filePath);
        }

        

        public static void AttachToCurrentTest(this MemoryBitmap bmp, string filePath)
        {
            bmp
                .AsSpanBitmap()
                .AsReadOnly()
                .AttachToCurrentTestAll(filePath);
        }

        public static void AttachToCurrentTest<TPixel>(this MemoryBitmap<TPixel> bmp, string filePath)
            where TPixel:unmanaged
        {
            bmp
                .AsSpanBitmap()
                .AsTypeless()
                .AsReadOnly()
                .AttachToCurrentTestAll(filePath);
        }

        public static void AttachToCurrentTestAll(this SpanBitmap bmp, string filePath)
        {
            bmp = bmp.AsReadOnly();

            TestContext.WriteLine($"{filePath} {bmp.Info.ToDebuggerDisplayString()}");

            filePath = TestContext.CurrentContext.GetTestResultPath(filePath);

            var dirPath = System.IO.Path.GetDirectoryName(filePath);
            System.IO.Directory.CreateDirectory(dirPath);

            if (bmp.PixelFormat == Pixel.BGR96F.Format || bmp.PixelFormat == Pixel.RGB96F.Format)
            {
                var tmp = new MemoryBitmap(bmp.Width, bmp.Height, Pixel.BGR24.Format);
                SpanBitmap.CopyPixels(bmp.OfType<System.Numerics.Vector3>(), tmp, (0, 1), (0,255));
                bmp = tmp;
            }            

            string _injectExt(string fp, string extPrefix)
            {
                var ext = System.IO.Path.GetExtension(fp);
                fp = fp.Substring(0, fp.Length - ext.Length);
                return $"{fp}.{extPrefix}{ext}";
            }

            var f1 = _injectExt(filePath, "WPF");
            bmp.Save(f1, InteropTypes.Graphics.Codecs.WPFCodec.Default); TestContext.AddTestAttachment(f1);

            var f2 = _injectExt(filePath, "GDI");
            bmp.Save(f2, Codecs.GDICodec.Default); TestContext.AddTestAttachment(f2);

            var f3 = _injectExt(filePath, "ImageSharp");
            bmp.Save(f3, Codecs.ImageSharpCodec.Default); TestContext.AddTestAttachment(f3);

            var f4 = _injectExt(filePath, "SkiaSharp");
            bmp.Save(f4, Codecs.SkiaCodec.Default); TestContext.AddTestAttachment(f4);

            var f5 = _injectExt(filePath, "OpenCvSharp");
            bmp.Save(f5, Codecs.OpenCvCodec.Default); TestContext.AddTestAttachment(f5);

            var f6 = _injectExt(filePath, "STB");
            bmp.Save(f6, Codecs.STBCodec.WithQuality(80)); TestContext.AddTestAttachment(f6);

            // TODO: it should compare saved files against bmp
        }

        public static void AttachToCurrentTest(this SpanBitmap bmp, string filePath)
        {
            filePath = TestContext.CurrentContext.GetTestResultPath(filePath);

            var dirPath = System.IO.Path.GetDirectoryName(filePath);
            System.IO.Directory.CreateDirectory(dirPath);            
            
            bmp.Save(filePath, WPFCodec.Default);
            TestContext.AddTestAttachment(filePath);
        }

        public static string AttachToCurrentTest(this IEnumerable<PointerBitmap> frames, string videoPath)
        {
            videoPath = TestContext.CurrentContext.GetTestResultPath(videoPath);
            Codecs.FFmpegAutoGenCodec.EncodeFrames(videoPath, frames);
            TestContext.AddTestAttachment(videoPath);

            return videoPath;

        }

        public static void AttachToCurrentTest(this IEnumerable<(PointerBitmap bmp, long pts)> frames, string videoPath)
        {
            videoPath = TestContext.CurrentContext.GetTestResultPath(videoPath);
            Codecs.FFmpegAutoGenCodec.EncodeFrames(videoPath, frames);
            TestContext.AddTestAttachment(videoPath);
        }

        public static string AttachAviToCurrentTest(this IEnumerable<MemoryBitmap> frames, string videoPath, float frameRate = 25)
        {
            videoPath = TestContext.CurrentContext.GetTestResultPath(videoPath);

            InteropTypes.Graphics.Video.VideoEncoderFactory.SaveToAVI(frames, videoPath, (decimal)frameRate, new Codecs.GDICodec(50));

            TestContext.AddTestAttachment(videoPath);

            return videoPath;
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

    public static class ShortcutUtils
    {
        public static void AttachShowDirLink(this TestContext context)
        {
            context.AttachLink("📂 Show Directory", context.GetTestResultPath(string.Empty));
        }

        public static void AttachLink(this TestContext context, string linkPath, string targetPath)
        {
            linkPath = context.GetTestResultPath(linkPath);

            linkPath = ShortcutUtils.CreateLink(linkPath, targetPath);

            TestContext.AddTestAttachment(linkPath);
        }

        public static string CreateLink(string localLinkPath, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(localLinkPath)) throw new ArgumentNullException(nameof(localLinkPath));
            if (string.IsNullOrWhiteSpace(targetPath)) throw new ArgumentNullException(nameof(targetPath));

            if (!Uri.TryCreate(targetPath, UriKind.Absolute, out Uri uri)) throw new UriFormatException(nameof(targetPath));

            var sb = new StringBuilder();

            sb.AppendLine("[{000214A0-0000-0000-C000-000000000046}]");
            sb.AppendLine("Prop3=19,11");
            sb.AppendLine("[InternetShortcut]");
            sb.AppendLine("IDList=");
            sb.AppendLine($"URL={uri.AbsoluteUri}");

            if (uri.IsFile)
            {
                sb.AppendLine("IconIndex=1");
                string icon = targetPath.Replace('\\', '/');
                sb.AppendLine("IconFile=" + icon);
            }
            else
            {
                sb.AppendLine("IconIndex=0");
            }

            localLinkPath = System.IO.Path.ChangeExtension(localLinkPath, ".url");

            System.IO.File.WriteAllText(localLinkPath, sb.ToString());

            return localLinkPath;
        }
    }

    public static class TestResources
    {
        public static string ShannonJpg => System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources\\shannon.jpg");
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
