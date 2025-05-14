using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Vision;

using NUnit.Framework;

using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

using POINT = SixLabors.ImageSharp.PointF;

namespace InteropTypes.Graphics.Backends
{
    [Category("Computer Vision")]
    public class ZXingTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.That(IntPtr.Size, Is.EqualTo(8), "x64 test environment required");
        }

        [TestCase("QRCode.png", "http://tecnohotelnews.com/")]
        // [TestCase("Resources\\glyphs_printed.jpg", null)]
        [TestCase("ios-11-camera-qr-code-scan.jpg", "WIFI:T:WPA2;S:iMore;P:iMore12345678;;")]
        public void ZXingFindQRCode(string filePath, string expected)
        {
            // http://www.cvsandbox.com/

            filePath = ResourceInfo.From(filePath);

            using var image = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(filePath);

            // detect code:

            var code = image.ReadAsSpanBitmap(self => self.ScanAndDecodeQRCode());            

            if (string.IsNullOrWhiteSpace(expected)) { Assert.That(code, Is.Null); return; }

            Assert.That(code.Text, Is.EqualTo(expected));

            // report result:

            TestContext.Out.WriteLine($"Code found: {code?.Text}");

            var points = code.ResultPoints.Select(item => (item.X, item.Y)).ToArray();
            var font = SixLabors.Fonts.SystemFonts.CreateFont("Arial", 20);

            image.Mutate(dc => dc.DrawPolygon(SixLabors.ImageSharp.Color.Red, 3, points));
            image.Mutate(dc => dc.DrawText(code.Text.Replace("/",":"), font, SixLabors.ImageSharp.Color.Red, new POINT(5, 5)));

            AttachmentInfo.From("result.png").WriteImage(image);
        }
    }
}
