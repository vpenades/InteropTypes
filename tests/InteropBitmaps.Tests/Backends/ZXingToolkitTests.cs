﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

using POINT = SixLabors.ImageSharp.PointF;

namespace InteropBitmaps.Backends
{
    [Category("Computer Vision")]
    public class ZXingTests
    {
        [SetUp]
        public void SetUp()
        {
            Assert.AreEqual(8, IntPtr.Size, "x64 test environment required");
        }

        [TestCase("Resources\\QRCode.png", "http://tecnohotelnews.com/")]
        // [TestCase("Resources\\glyphs_printed.jpg", null)]
        [TestCase("Resources\\ios-11-camera-qr-code-scan.jpg", "WIFI:T:WPA2;S:iMore;P:iMore12345678;;")]
        public void ZXingFindQRCode(string filePath, string expected)
        {
            // http://www.cvsandbox.com/

            filePath = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, filePath);

            using var image = SixLabors.ImageSharp.Image.Load(filePath);

            // detect code:

            var code = image.AsSpanBitmap().ScanAndDecodeQRCode();

            if (string.IsNullOrWhiteSpace(expected)) { Assert.Null(code); return; }

            Assert.AreEqual(expected, code.Text);

            // report result:

            TestContext.WriteLine($"Code found: {code?.Text}");

            var points = code.ResultPoints.Select(item => (item.X, item.Y)).ToArray();
            var font = SixLabors.Fonts.SystemFonts.CreateFont("Arial", 20);

            image.Mutate(dc => dc.DrawPolygon(SixLabors.ImageSharp.Color.Red, 3, points));
            image.Mutate(dc => dc.DrawText(code.Text, font, SixLabors.ImageSharp.Color.Red, new POINT(5, 5)));

            image.AttachToCurrentTest("result.png");
        }
    }
}
