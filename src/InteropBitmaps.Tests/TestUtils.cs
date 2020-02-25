using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (bmp.PixelSize == 1) bmp.AsImageSharp<SixLabors.ImageSharp.PixelFormats.Gray8>().Save(filePath);
            if (bmp.PixelSize == 3) bmp.AsImageSharp<SixLabors.ImageSharp.PixelFormats.Rgb24>().Save(filePath);
            if (bmp.PixelSize == 4) bmp.AsImageSharp<SixLabors.ImageSharp.PixelFormats.Rgba32>().Save(filePath);

            NUnit.Framework.TestContext.AddTestAttachment(filePath);
        }
    }
}
