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
    [Flags]
    public enum Quadrant
    {        
        Origin = 0,

        Top = 1,
        Left = 2,
        Right = 4,
        Bottom = 8,

        /// <summary>
        /// Quadrant I
        /// </summary>
        TopRight = Top|Right,

        /// <summary>
        /// Quadrant II
        /// </summary>
        TopLeft = Top|Left,

        /// <summary>
        /// Quadrant III
        /// </summary>
        BottomLeft = Bottom|Left,

        /// <summary>
        /// Quadrant IV
        /// </summary>
        BottomRight = Bottom|Right
    }
}
