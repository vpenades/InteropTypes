using System;
using System.Collections.Generic;
using System.Text;

namespace InteropDrawing
{
    /// <summary>
    /// <see href="https://en.wikipedia.org/wiki/Quadrant_(plane_geometry)"/>
    /// </summary>
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
