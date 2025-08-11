using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    using PRIMITIVE2D = ICoreCanvas2D;    

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
        TopRight = Top | Right,

        /// <summary>
        /// Quadrant II
        /// </summary>
        TopLeft = Top | Left,

        /// <summary>
        /// Quadrant III
        /// </summary>
        BottomLeft = Bottom | Left,

        /// <summary>
        /// Quadrant IV
        /// </summary>
        BottomRight = Bottom | Right
    }

    public static partial class DrawingToolkit
    {
        /// <summary>
        /// Tries to get the quadrant used by the render target.
        /// </summary>
        /// <param name="dc">The rendering target.</param>
        /// <param name="quadrant">The quadrant used by the render target</param>
        /// <returns>true if it succeeded to retrieve the quadrant.</returns>
        public static bool TryGetQuadrant(this PRIMITIVE2D dc, out Quadrant quadrant)
        {
            if (dc is ITransformer2D xform)
            {
                Span<POINT2> points = stackalloc POINT2[1];
                points[0] = XY.One;
                xform.TransformNormalsForward(points);

                var dir = points[0].XY;

                dir *= new XY(1, -1); // We reverse the vertical axis so it makes sense from screen POV.

                quadrant = _GetQuadrant(dir);

                return true;
            }
            else
            {
                quadrant = default;
                return false;
            }
        }        

        private static Quadrant _GetQuadrant(in XY direction)
        {
            var q = Quadrant.Origin;
            if (direction.X < 0) q |= Quadrant.Left;
            else if (direction.X > 0) q |= Quadrant.Right;

            if (direction.Y < 0) q |= Quadrant.Bottom;
            else if (direction.Y > 0) q |= Quadrant.Top;

            return q;
        }
    }
}
