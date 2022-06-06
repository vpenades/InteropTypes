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
        FlipAuto = 1,
        FlipHorizontal = 2,
        FlipVertical = 4,

        // to determine the text origin, calculate the points of the 4 corners, and
        // based on the values below, apply weights to the points.

        DockLeft = 8,
        DockRight = 16,
        DockTop = 32,
        DockBottom = 64,
        Center = DockLeft | DockRight | DockTop | DockBottom
    }
}
