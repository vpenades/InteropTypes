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

        /// <summary>
        /// returs a rectangle that fits the given text with the given size
        /// </summary>
        /// <param name="text">the text line to measure.</param>
        /// <param name="size">The text size. If negative it will use <see cref="Height"/>.</param>
        /// <returns></returns>
        GDIRECTF MeasureTextLine(string text, float size)
        {
            var rect = MeasureTextLine(text);

            float scale = size < 0
                ? 1
                : size / Math.Max(1, this.Height);

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

        /// <summary>
        /// Draws a text to the given location.
        /// </summary>
        /// <param name="target">render target surface</param>
        /// <param name="transform">text location transform</param>
        /// <param name="text">text to render</param>
        /// <param name="tintColor">text colo</param>
        /// <remarks>
        /// to use text alignment, use <see cref="FontStyle.DrawDecomposedTo(ICoreCanvas2D, in System.Numerics.Matrix3x2, string, float)"/>
        /// </remarks>
        void DrawTextLineTo(ICoreCanvas2D target, XFORM2 transform, string text, ColorStyle tintColor);        
        public FontStyle ToStyle() { return (this, GDICOLOR.White); }        
    }
}
