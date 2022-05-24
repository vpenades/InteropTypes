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


    

    class _AlignedBitmapFont : Drawing.Fonts.IBitmapFont
    {
        #region lifecycle

        // private http://faculty.salina.k-state.edu/tmertz/Java/072graphicscolorfont/05fontmetrics.pdf

        public _AlignedBitmapFont(int leading, params MemoryBitmap<Pixel.BGRP32>[] bmps)
        {
            _Font = Bitmaps.Fonts.XnaSpriteFont.CreateFrom(bmps);            

            _Glyphs = _Font.Glyphs
                .Select(glyph => new ImageSource(glyph, (0,0), (glyph.Width,glyph.Height), (0,0)))
                .ToArray();
        }

        #endregion

        #region data

        private Bitmaps.Fonts.XnaSpriteFont _Font;
        private ImageSource[] _Glyphs;

        #endregion

        #region API

        public int Height { get; }

        public Size Measure(string text) { return _Font.Measure(text); }

        public void DrawFont(ICoreCanvas2D target, Matrix3x2 transform, string text, ColorStyle tintColor)
        {
            foreach(var (idx,xform) in _Font.GetGlyphLocations(transform,text))
            {
                var glyph = _Glyphs[idx];
                target.DrawImage(xform, (glyph, tintColor));
            }
        }

        #endregion
    }
}
