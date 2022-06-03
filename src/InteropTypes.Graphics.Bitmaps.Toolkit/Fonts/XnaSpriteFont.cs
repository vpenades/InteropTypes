﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace InteropTypes.Graphics.Bitmaps.Fonts
{
    using GLYPHBITMAP = MemoryBitmap<Pixel.BGRP32>;
    using FONTBITMAP = MemoryBitmap<Pixel.BGRP32>;

    /// <summary>
    /// Represents a Bitmap font using XNA SpriteFont glyph pattern arrangement
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class consumes fonts generated by <see href="https://github.com/nkast/XNAGameStudio/wiki/Bitmap-Font-Maker">XNA's Bitmap Font Maker</see>
    /// </para>
    /// <para>
    /// This class is used in <see cref="InteropTypes.Graphics.Backends.SpanBitmap.Font._XnaSpriteFontAdapter"/>
    /// </para>
    /// </remarks>
    public class XnaSpriteFont
    {
        #region lifecycle

        public static XnaSpriteFont Load(string spriteFontPath)
        {
            var bmp = FONTBITMAP.Load(spriteFontPath);
            return CreateFrom(bmp);
        }

        public static XnaSpriteFont CreateFrom(params MemoryBitmap[] bmps)
        {
            var xbmps = bmps.Select(item => item.CloneAs<Pixel.BGRP32>()).ToArray();

            return CreateFrom(xbmps);
        }

        public static XnaSpriteFont CreateFrom(params FONTBITMAP[] bmps)
        {            
            var glyphs = _SplitGlyphsFromSpriteFont(bmps).ToArray();

            return new XnaSpriteFont(glyphs);
        }

        private XnaSpriteFont(GLYPHBITMAP[] glyphs, int leading = 2)
        {
            _Glyphs = glyphs;

            _FirstChar = 32;

            _MaxAscent = _Glyphs[65 - _FirstChar].Height; // take 'A' for max ascent
            _MaxDescent = _Glyphs.Max(item => item.Height) - _MaxAscent;

            Height = _MaxAscent + _MaxDescent + leading;            
        }

        #endregion

        #region  data

        private GLYPHBITMAP[] _Glyphs;

        private int _FirstChar;
        private int _MaxAscent;
        private int _MaxDescent;

        #endregion

        #region properties

        public int Height { get; }

        public IReadOnlyList<GLYPHBITMAP> Glyphs => _Glyphs;

        #endregion

        #region API

        public System.Drawing.Size Measure(string text)
        {
            int width = 0;

            foreach (var c in text)
            {
                var idx = (int)c;
                idx -= _FirstChar;

                var glyph = _Glyphs[idx];

                width += glyph.Width;
            }

            return new System.Drawing.Size(width, Height);
        }

        public IEnumerable<(int GlyphIndex, Matrix3x2 Transform)> GetGlyphLocations(Matrix3x2 origin, string text)
        {
            var transform = origin;

            foreach (var c in text)
            {
                var idx = (int)c;
                idx -= _FirstChar;

                var glyph = _Glyphs[idx];

                yield return (idx, transform);                

                var next = new Vector2(glyph.Width, 0);

                transform = Matrix3x2.CreateTranslation(next) * transform;
            }
        }


        public void DrawTextTo<TPixel>(SpanBitmap<TPixel> target, Matrix3x2 origin, string text, float opacity)
            where TPixel: unmanaged
        {
            foreach (var (idx, xform) in GetGlyphLocations(origin, text))
            {
                var glyph = _Glyphs[idx];

                target.SetPixels(xform, glyph.AsSpanBitmap(), true, opacity);                
            }
        }

        #endregion

        #region font parsing

        private static IEnumerable<GLYPHBITMAP> _SplitGlyphsFromSpriteFont(params FONTBITMAP[] bmps)
        {
            foreach (var bmp in bmps)
            {

                foreach (var row in _SplitGlyphsInRow(bmp))
                {
                    var framePix = row.GetPixel(0, 0);

                    int x = 0;

                    int start = -1;

                    while (x < row.Width)
                    {
                        var isFull = true;

                        for (int y = 0; y < row.Height; ++y)
                        {
                            if (row.GetPixel(x, y) != framePix) { isFull = false; break; }
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
                                while (glyph.Height > 0)
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
        }

        private static IEnumerable<GLYPHBITMAP> _SplitGlyphsInRow(FONTBITMAP bmp)
        {
            var framePix = bmp.GetPixel(0, 0);

            int y = 0;

            int start = -1;

            while (y < bmp.Height)
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

        #endregion
    }
}