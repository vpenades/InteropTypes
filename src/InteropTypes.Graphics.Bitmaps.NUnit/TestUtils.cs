using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;

using NUnit.Framework;

namespace InteropTypes.Graphics.Bitmaps
{
    static class NUnitExtensions
    {
        private static string UseTestOutputFilePath(params string[] parts)
        {
            var context = TestContext.CurrentContext;

            var path = System.IO.Path.Combine(context.WorkDirectory, "TestsOutput", context.Test.ID);            

            foreach (var part in parts) path = System.IO.Path.Combine(path, part);

            var dir = System.IO.Path.GetDirectoryName(path);

            System.IO.Directory.CreateDirectory(dir);

            return path;
        }        

        public static void AttachToCurrentTest(this System.Drawing.Bitmap image, string filePath)
        {
            using (var owner = image.UsingMemoryBitmap())
            {
                owner.AsMemoryBitmap().AttachToCurrentTest(filePath);
            }                
        }

        public static void AttachToCurrentTest(this MemoryBitmap bmp, string filePath)
        {
            bmp
                .AsSpanBitmap()
                .AttachToCurrentTest(filePath);
        }

        public static void AttachToCurrentTest<TPixel>(this MemoryBitmap<TPixel> bmp, string filePath)
            where TPixel:unmanaged
        {
            bmp
                .AsSpanBitmap()
                .AsTypeless()
                .AttachToCurrentTest(filePath);
        }

        public static void AttachToCurrentTest<TPixel>(this SpanBitmap<TPixel> bmp, string filePath, params Codecs.IBitmapEncoder[] encoders)
            where TPixel:unmanaged
        {
            bmp
                .AsTypeless()
                .AttachToCurrentTest(filePath, encoders);
        }

        public static void AttachToCurrentTest(this SpanBitmap bmp, string filePath, params Codecs.IBitmapEncoder[] encoders)
        {
            filePath = UseTestOutputFilePath(filePath);            

            if (encoders.Length == 0) encoders = new[] { Codecs.GDICodec.Default };           

            bmp.Save(filePath, encoders);
            
            TestContext.AddTestAttachment(filePath);
        }
    }    
}
