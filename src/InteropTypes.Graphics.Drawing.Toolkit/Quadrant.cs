using System;
using System.Collections.Generic;
using System.Text;

using VECTOR2 = System.Numerics.Vector2;
using POINT2 = InteropTypes.Graphics.Drawing.Point2;
using XFORM2 = System.Numerics.Matrix3x2;

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

    public static partial class Toolkit
    {
        public static bool TryGetQuadrant(this PRIMITIVE2D dc, out Quadrant quadrant)
        {

            if (dc is ITransformer2D xform)
            {
                Span<POINT2> points = stackalloc POINT2[1];
                points[0] = VECTOR2.One;
                xform.TransformNormalsForward(points);
                quadrant = GetQuadrant(points[0].ToNumerics());
                return true;
            }
            else
            {
                quadrant = default;
                return false;
            }
        }

        /// <summary>
        /// Determines the predominant quadrant from a given transform matrix.
        /// </summary>
        /// <param name="transform">The viewport transform matrix</param>
        /// <returns>The positive quadrant</returns>
        public static Quadrant GetQuadrant(in XFORM2 transform)
        {
            var p0 = VECTOR2.Transform(VECTOR2.Zero, transform);
            var p1 = VECTOR2.Transform(VECTOR2.One, transform);

            return GetQuadrant(p1 - p0);
        }

        public static Quadrant GetQuadrant(in VECTOR2 direction)
        {
            var q = Quadrant.Origin;
            if (direction.X < 0) q |= Quadrant.Left;
            else if (direction.X > 0) q |= Quadrant.Right;

            if (direction.Y < 0) q |= Quadrant.Top;
            else if (direction.Y > 0) q |= Quadrant.Bottom;

            return q;
        }
    }
}
