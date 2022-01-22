using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDrawing
{
    /// <summary>
    /// The axes of a two-dimensional Cartesian system divide the plane into four infinite regions,
    /// called quadrants, each bounded by two half-axes. 
    /// </summary>
    /// <remarks>
    /// <see href="https://en.wikipedia.org/wiki/Quadrant_(plane_geometry)">Quadrant (plane geometry)</see>
    /// </remarks>
    public enum Quadrant
    {
        /// <summary>
        /// quadrant is not known
        /// </summary>
        Undefined,

        /// <summary>
        /// Quadrant I
        /// </summary>
        TopRight,

        /// <summary>
        /// Quadrant II
        /// </summary>
        TopLeft,

        /// <summary>
        /// Quadrant III
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Quadrant IV
        /// </summary>
        BottomRight
    }
}
