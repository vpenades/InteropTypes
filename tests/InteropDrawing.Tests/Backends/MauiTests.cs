using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Backends;

using NUnit.Framework;

namespace InteropTypes.Graphics.Drawing
{
    public class MauiTests
    {
        [Test]
        public void TestMaui()
        {
            using (var skBmp = new SkiaSharp.SKBitmap(256, 256, false))
            {
                using (var skCanvas = new SkiaSharp.SKCanvas(skBmp))
                {
                    using (var mauiCanvas = new Microsoft.Maui.Graphics.Skia.SkiaCanvas())
                    {
                        mauiCanvas.Canvas = skCanvas;

                        var myCanvas = CanvasWrapper.Create(mauiCanvas);

                        myCanvas.DrawCircle((50, 50), 20, System.Drawing.Color.Red);

                        myCanvas.DrawRectangle((100, 10), (50, 50), System.Drawing.Color.White, 5, 3);
                    }
                }

                using (var memBmp = skBmp.UsingMemoryBitmap())
                {
                    memBmp.Bitmap
                        .ToMemoryBitmap(Pixel.RGBA32.Format)
                        .AttachToCurrentTest("maui.png");
                }
            }
        }

    }
}
