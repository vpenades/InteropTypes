using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Fonts
{
    /// <summary>
    /// Represents the style of a Font. <see cref="FontStyle"/>.
    /// </summary>
    [Flags]
    public enum FontAlignStyle
    {
        None,

        /// <summary>
        /// Flips the text based on the underlaying <see cref="ICanvas2D"/>'s <see cref="Quadrant"/>.
        /// </summary>
        /// <remarks>
        /// We can set the canvas origin at any of the 4 corners, but based on that, we may want the text to always be rendered "straight"
        /// </remarks>
        FlipAuto = 1,
        FlipHorizontal = 2,
        FlipVertical = 4,
        FlipMask = FlipAuto | FlipHorizontal | FlipVertical,

        // to determine the text origin, calculate the points of the 4 corners, and
        // based on the values below, apply weights to the points.

        CenterHorizontal = 8,
        DockRight = 16,
        CenterVertical = 32,
        DockBottom = 64,        
        Center = CenterHorizontal | CenterVertical,
        AlignmentMask = CenterHorizontal | CenterVertical | DockRight | DockBottom
    }
}
