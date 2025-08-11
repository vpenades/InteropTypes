using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    public interface IFont
    {
        /// <summary>
        /// Must return true if it renders the font using vectorial primitives, and false if it renders the fonts using bitmaps.
        /// </summary>
        bool IsVectorial { get; }

        GDIRECTF MeasureTextLine(string text, float size)
        {
            var rect = MeasureTextLine(text);

            var h = Math.Max(1, this.Height);

            if (size <= 0) size = h;

            var scale = size / h;

            rect.X *= scale;
            rect.Y *= scale;
            rect.Width *= scale;
            rect.Height *= scale;

            return rect;
        }

        /// <summary>
        /// calculates the pixel size of the given text line when rendered.
        /// </summary>
        /// <param name="text">the text to measure</param>
        /// <returns>the size of the rendered area.</returns>
        GDIRECTF MeasureTextLine(string text);

        /// <summary>
        /// Interline height separation
        /// </summary>
        /// <remarks>
        /// <see href="http://faculty.salina.k-state.edu/tmertz/Java/072graphicscolorfont/05fontmetrics.pdf"/>
        /// This value must always be equal, or larger than the Height returned by <see cref="MeasureTextLine(string)"/>
        /// </remarks>
        int Height { get; }

        void DrawTextLineTo(ICoreCanvas2D target, XFORM2 transform, string text, ColorStyle tintColor);        
        public FontStyle ToStyle() { return (this, GDICOLOR.White); }        
    }
}
