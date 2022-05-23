using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Backends
{
    // bitmap fonts format https://en.wikipedia.org/wiki/Computer_font

    // https://fontforge.org/docs/techref/pcf-format.html
    // C lang https://github.com/notxx/pcf
    // Go https://github.com/nareix/pcf
    // C# (kaitai generated) http://formats.kaitai.io/pcf_font/csharp.html
    // angelcode font https://www.codeproject.com/Articles/317694/AngelCode-bitmap-Font-Parsing-Using-Csharp
    //   https://github.com/AuroraBertaOldham/SharpFNT


    public static class MemoryBitmapFontFactory
    {
        public static Drawing.Fonts.IBitmapFont LoadBitmapFont(string filePath)
        {
            return new _AlignedBitmapFont(filePath);
        }

        internal static IEnumerable<MemoryBitmap<Pixel.BGRP32>> FindGlyphsFromMonogameFont(MemoryBitmap<Pixel.BGRP32> bmp)
        {
            foreach(var row in GetGlyphRows(bmp))
            {
                var framePix = row.GetPixel(0, 0);

                int x = 0;

                int start = -1;

                while (x < row.Width)
                {
                    var isFull = true;

                    for(int y=0; y < row.Height; ++y)
                    {
                        if (row.GetPixel(x,y) != framePix) { isFull = false; break; }
                    }

                    if (!isFull) { ++x; continue; }

                    else
                    {
                        if (start < 0) { start = x; ++x; continue; }

                        var w = x - start - 1;                        

                        if (w > 0)
                        {
                            var glyph = row.Slice(new BitmapBounds(start + 1, 0, w, row.Height));

                            // trim from the bottom
                            while(glyph.Height > 0)
                            {
                                if (!glyph.GetScanlinePixels(glyph.Height - 1).All(p => p == framePix)) break;

                                glyph = glyph.Slice((0, 0, glyph.Width, glyph.Height - 1));
                            }

                            if (glyph.Width > 4 && glyph.Height > 4)                            
                            {
                                glyph = glyph.Slice((2, 2, glyph.Width - 4, glyph.Height - 4));

                                yield return glyph;
                            }
                            else
                            {
                                yield return default;
                            }
                        }

                        start = x;
                        ++x;
                    }
                }
            }

        }

        private static IEnumerable<MemoryBitmap<Pixel.BGRP32>> GetGlyphRows(MemoryBitmap<Pixel.BGRP32> bmp)
        {
            var framePix = bmp.GetPixel(0, 0);

            int y = 0;

            int start = -1;            

            while(y < bmp.Height)
            {
                var isFull = bmp.GetScanlinePixels(y).All(item => item == framePix);

                if (!isFull) { ++y; continue; }

                else
                {
                    if (start < 0) { start = y; ++y; continue; }

                    var h = y - start - 1;

                    if (h >= 1) yield return bmp.Slice(new BitmapBounds(0, start + 1, bmp.Width, h));

                    start = y;
                    ++y;
                }
            }            
        }
    }

    class _AlignedBitmapFont : Drawing.Fonts.IBitmapFont
    {
        #region lifecycle

        // private http://faculty.salina.k-state.edu/tmertz/Java/072graphicscolorfont/05fontmetrics.pdf

        public _AlignedBitmapFont(string filePath, int leading = 2)
        {
            _FirstChar = 32;

            var bmp = MemoryBitmap<Pixel.BGRP32>.Load(filePath);

            var glyphs = MemoryBitmapFontFactory
                .FindGlyphsFromMonogameFont(bmp)
                .ToArray();

            _MaxAscent = glyphs[65 - _FirstChar].Height;
            _MaxDescent = glyphs.Max(item => item.Height) - _MaxAscent;

            _Glyphs = glyphs
                .Select(glyph => new ImageSource(glyph, (0,0), (glyph.Width,glyph.Height), (0,0)))
                .ToArray();            

            Height = _MaxAscent + _MaxDescent + leading;
        }

        #endregion

        #region data

        private int _FirstChar;

        private ImageSource[] _Glyphs;

        private int _MaxAscent;
        private int _MaxDescent;

        #endregion

        #region API

        public int Height { get; }

        public Size Measure(string text)
        {
            int width = 0;

            foreach (var c in text)
            {
                var idx = (int)c;
                idx -= _FirstChar;

                var glyph = _Glyphs[idx];

                width += (int)glyph.GetSourceRectangle().Width;                
            }

            return new Size(width, Height);
        }

        public void DrawFont(ICoreCanvas2D target, Matrix3x2 transform, string text, ColorStyle tintColor)
        {
            foreach(var c in text)
            {
                var idx = (int)c;
                idx -= _FirstChar;

                var glyph = _Glyphs[idx];

                target.DrawImage(transform, (glyph, tintColor));

                var next = new Vector2(glyph.GetSourceRectangle().Width, 0);

                transform = Matrix3x2.CreateTranslation(next) * transform;
            }
        }

        #endregion
    }
}
