using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using NUnit.Framework;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Backends;
using InteropTypes.Codecs;

namespace InteropTypes.Vision.Backends
{
    public class ZXingTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.That(IntPtr.Size, Is.EqualTo(8), "x64 test environment required");
        }

        [TestCase("Resources\\QRCode.png", "http://tecnohotelnews.com/")]
        // [TestCase("Resources\\glyphs_printed.jpg", null)]
        [TestCase("Resources\\ios-11-camera-qr-code-scan.jpg", "WIFI:T:WPA2;S:iMore;P:iMore12345678;;")]
        public void ZXingFindQRCode(string filePath, string expected)
        {
            // http://www.cvsandbox.com/

            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            var image = MemoryBitmap.Load(filePath, STBCodec.Default);

            // detect code:            

            var result = new ZXingCode();

            using var detector = new ZXingCode.Detector();
                
            detector.Inference(result, image, new System.Drawing.Rectangle(20,20,1000,1000) );

            var code = result.Results[0];

            if (string.IsNullOrWhiteSpace(expected)) { Assert.That(code, Is.Null); return; }

            Assert.That(code.Text, Is.EqualTo(expected));

            // report result:

            TestContext.Out.WriteLine($"Code found: {code?.Text}");

            image
                .CreateDrawingContext()
                .DrawAsset(System.Numerics.Matrix3x2.Identity, result);

            image.Save(new AttachmentInfo("result.png"));            
        }
    }
}
