using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using NUnit.Framework;

using SixLabors.ImageSharp.Processing;

using POINT = SixLabors.Primitives.PointF;

namespace InteropBitmaps.Backends
{
    [Category("ZXing Backend")]
    public class ZXingToolkitTests
    {
        [TestCase("Resources\\QRCode.png", "http://tecnohotelnews.com/")]
        [TestCase("Resources\\ios-11-camera-qr-code-scan.jpg", "WIFI:T:WPA2;S:iMore;P:iMore12345678;;")]
        public void ZXingFindQRCode(string filePath, string expected)
        {
            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            using (var image = SixLabors.ImageSharp.Image.Load(filePath))
            {
                var code = image.AsSpanBitmap().ScanAndDecodeQRCode();

                if (string.IsNullOrWhiteSpace(expected)) { Assert.Null(code); return; }

                Assert.AreEqual(expected, code.Text);

                TestContext.WriteLine($"Code found: {code?.Text}");

                foreach(var p in code.ResultPoints)
                {
                    var p1 = new POINT(p.X, p.Y - 5);
                    var p2 = new POINT(p.X + 5, p.Y + 5);
                    var p3 = new POINT(p.X - 5, p.Y + 5);
                    image.Mutate(dc => dc.FillPolygon(SixLabors.ImageSharp.Color.Red, p1, p2, p3));
                }

                var font = SixLabors.Fonts.SystemFonts.CreateFont("Arial", 20);

                image.Mutate(dc => dc.DrawText(code.Text, font, SixLabors.ImageSharp.Color.Red, new POINT(5, 5)));

                image.AttachToCurrentTest("Result.png");
            }
        }
    }
}
