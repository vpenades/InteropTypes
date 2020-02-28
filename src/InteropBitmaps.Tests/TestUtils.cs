using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace InteropBitmaps
{
    static class TestUtils
    {
        public static string OutputPath(this NUnit.Framework.TestContext context, params string[] parts)
        {
            // https://github.com/nunit/nunit/issues/1768#issuecomment-242454699

            var path = System.IO.Path.Combine(context.WorkDirectory, "TestsOutput", context.Test.ID);
            foreach (var part in parts) path = System.IO.Path.Combine(path, part);

            return path;
        }

        public static void AttachToCurrentTest(this SixLabors.ImageSharp.Image image, string filePath)
        {
            filePath = NUnit.Framework.TestContext.CurrentContext.OutputPath(filePath);

            var dirPath = System.IO.Path.GetDirectoryName(filePath);
            System.IO.Directory.CreateDirectory(dirPath);

            SixLabors.ImageSharp.ImageExtensions.Save(image, filePath);

            NUnit.Framework.TestContext.AddTestAttachment(filePath);
        }

        public static void AttachToCurrentTest(this MemoryBitmap bmp, string filePath)
        {
            filePath = NUnit.Framework.TestContext.CurrentContext.OutputPath(filePath);

            var dirPath = System.IO.Path.GetDirectoryName(filePath);
            System.IO.Directory.CreateDirectory(dirPath);

            string _injectExt(string fp, string extPrefix)
            {
                var ext = System.IO.Path.GetExtension(fp);
                fp = fp.Substring(0, fp.Length - ext.Length);
                return $"{fp}.{extPrefix}{ext}";
            }

            // try {
                bmp.AsGDI().Save(_injectExt(filePath, "GDI"));
                NUnit.Framework.TestContext.AddTestAttachment(_injectExt(filePath, "GDI"));
            // } catch { }

            // try {
                bmp.AsImageSharp().Save(_injectExt(filePath, "ImageSharp"));
                NUnit.Framework.TestContext.AddTestAttachment(_injectExt(filePath, "ImageSharp"));
            // } catch { }            
        }
    }

    static class ImageSharpUtils
    {
        public static IImageProcessingContext FillPolygon(this IImageProcessingContext source, Color color, params (float,float)[] points)
        {
            var ppp = points.Select(item => new SixLabors.Primitives.PointF(item.Item1, item.Item2)).ToArray();

            return source.FillPolygon(color, ppp);
        }

        public static IImageProcessingContext DrawPolygon(this IImageProcessingContext source, Color color, float thickness, params (float, float)[] points)
        {
            var ppp = points.Select(item => new SixLabors.Primitives.PointF(item.Item1, item.Item2)).ToArray();

            return source.DrawPolygon(color, thickness, ppp);
        }

    }
}
