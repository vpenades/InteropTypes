using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    public interface IBitmapFont
    {
        System.Drawing.Size Measure(string text);

        /// <summary>
        /// Interline height separation
        /// </summary>
        /// <remarks>
        /// <see href="http://faculty.salina.k-state.edu/tmertz/Java/072graphicscolorfont/05fontmetrics.pdf"/>
        /// </remarks>
        int Height { get; }
        
        void DrawFont(ICoreCanvas2D target, System.Numerics.Matrix3x2 transform, string text, ColorStyle tintColor);
    }
}
