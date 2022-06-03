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
    /// <summary>
    /// Adapter for <see cref="Bitmaps.Fonts.XnaSpriteFont"/>
    /// </summary>
    class _XnaSpriteFontAdapter : Drawing.Fonts.IFont
    {
        #region lifecycle

        // private http://faculty.salina.k-state.edu/tmertz/Java/072graphicscolorfont/05fontmetrics.pdf

        public _XnaSpriteFontAdapter(int leading, params MemoryBitmap<Pixel.BGRP32>[] bmps)
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

        public int Height => _Font.Height;

        public Size MeasureTextLine(string text) { return _Font.Measure(text); }

        public void DrawTextLineTo(ICoreCanvas2D target, Matrix3x2 origin, string text, ColorStyle tintColor)
        {
            foreach(var (idx,xform) in _Font.GetGlyphLocations(origin,text))
            {
                var glyph = _Glyphs[idx];
                target.DrawImage(xform, (glyph, tintColor));
            }
        }

        #endregion
    }
}
