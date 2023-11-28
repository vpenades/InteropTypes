using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using NUnit.Framework;

using Plotly;

namespace InteropTypes.Graphics.Drawing
{
    internal class FontsTests
    {
        [Test]
        public void TestBitmapGlyphs()
        {
            // test glyph splitter            

            var fontPath = ResourceInfo.From("SegoeUiMono16.png");

            var spriteFont = Bitmaps.Fonts.XnaSpriteFont.Load(fontPath);

            Assert.That(spriteFont.Glyphs, Has.Count.EqualTo(224));

            for (int i = 0; i < spriteFont.Glyphs.Count; ++i)
            {
                var g = spriteFont.Glyphs[i];
                if (g.IsEmpty) continue;
                g.Save(new AttachmentInfo($"glyph {i}.png"));
            }
        }

        [Test]
        public void TestMeasureRegression()
        {
            // apparently text measuring doesn't work well until after we render a text for the first time.

            var a = Fonts.HersheyFont.Default.MeasureTextLine("Hello world!");

            var dc = new Bitmaps.MemoryBitmap<Pixel.RGB24>(256, 128).CreateDrawingContext();
            _DrawText(dc, System.Numerics.Matrix3x2.Identity, System.Drawing.Color.Red, 20);

            var b = Fonts.HersheyFont.Default.MeasureTextLine("Hello world!");

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void DrawFontBasic()
        {
            var renderTarget = new Bitmaps.MemoryBitmap<Pixel.RGB24>(256,128);

            var dc = renderTarget.CreateDrawingContext();
            
            var style = FontStyle
                .TryGetDefaultFontFrom(dc)
                .With(System.Drawing.Color.White);            

            _DrawText(dc, Matrix3x2.CreateTranslation(10, 10), style, style.Font.Height);
            _DrawText(dc, Matrix3x2.CreateTranslation(10, 40), style, 20);
            _DrawText(dc, Matrix3x2.CreateScale(1.5f) * Matrix3x2.CreateRotation(0.1f) * Matrix3x2.CreateTranslation(30, 80), style, 20);

            renderTarget.Save(AttachmentInfo.From("result.png"));
        }

        [Test]
        public void DrawThickHersheyFont()
        {
            var renderTarget = new Bitmaps.MemoryBitmap<Pixel.RGB24>(256, 128);

            var dc = renderTarget.CreateDrawingContext();

            var font = Fonts.HersheyFont.CreateWithThickness(3);
            var style = new FontStyle(font, System.Drawing.Color.White);

            _DrawText(dc, Matrix3x2.CreateTranslation(10, 10), style, style.Font.Height);
            _DrawText(dc, Matrix3x2.CreateTranslation(10, 40), style, 20);
            _DrawText(dc, Matrix3x2.CreateScale(1.5f) * Matrix3x2.CreateRotation(0.1f) * Matrix3x2.CreateTranslation(30, 80), style, 20);

            renderTarget.Save(AttachmentInfo.From("result.png"));
        }

        [Test]
        public void DrawBitmapFont()
        {
            var fontPath = ResourceInfo.From("SegoeUiMono16.png");            

            var dst = new MemoryBitmap<Pixel.BGR24>(512, 512);
            var dc = dst.CreateDrawingContext();           

            var font1 = FontStyle
                .TryGetDefaultFontFrom(dc)
                .With(System.Drawing.Color.White);

            var font2 = MemoryBitmap<Pixel.BGRA32>
                .Load(fontPath)
                .ToBitmapFont();

            var xform = Matrix3x2.CreateScale(2) * Matrix3x2.CreateRotation(0.2f) * Matrix3x2.CreateTranslation(35, 5);            
            _DrawText(dc, xform, (font2, System.Drawing.Color.White), -1);

            xform *= Matrix3x2.CreateTranslation(0, 80);            
            _DrawText(dc, xform, (font2, System.Drawing.Color.White), 20);

            xform *= Matrix3x2.CreateTranslation(0, 80);
            _DrawText(dc, xform, font1, 20);            

            xform *= Matrix3x2.CreateTranslation(0, 80);
            _DrawText(dc, xform, font1, 20);

            xform *= Matrix3x2.CreateTranslation(0, 80);            
            _DrawText(dc, xform, System.Drawing.Color.Red, 20);

            dst.Save(new AttachmentInfo("text.png"));
        }

        private static void _DrawText(ICanvas2D dc, Matrix3x2 xform, FontStyle style, float size)
        {
            var text = "Hello world!";
            var rect = style.MeasureTextLine(text, size);

            dc.DrawRectangle(xform, rect, (System.Drawing.Color.Red,1.5f));

            dc.DrawCircle(xform.Translation, 3, System.Drawing.Color.Green);

            dc.DrawTextLine(xform, text, size, style);
        }
    }
}
