using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Codecs
{
    [Category("Codecs")]
    public class LoadAndSaveTests
    {
        [TestCase("Resources\\shannon.jpg")]
        [TestCase("Resources\\diagram.jpg")]
        [TestCase("Resources\\white.png")]
        public void LoadImage(string filePath)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var codecs = new IBitmapDecoding[] { new GDICodec(), new WPFCodec(), new ImageSharpCodec(), new STBCodec(), new SkiaCodec() };

            foreach (var codec in codecs)
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var bitmap = MemoryBitmap.Load(filePath, codec);
                sw.Stop();                

                TestContext.WriteLine($"Loading {System.IO.Path.GetFileName(filePath)} with {codec} tool {sw.ElapsedTicks}");

                var fname = $"{codec.GetType().Name}-To-.png";
                bitmap.AttachToCurrentTest(fname);

                fname = $"{codec.GetType().Name}-To-.jpg";
                bitmap.AttachToCurrentTest(fname);
            }
        }

        
    }
}
