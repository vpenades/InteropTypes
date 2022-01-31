using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;
using POINT2 = InteropTypes.Graphics.Drawing.Point2;

using COLOR = System.Drawing.Color;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Drawing.Backends
{
    /// <summary>
    /// Represents a drawing canvas where we can draw 2D thin lines.
    /// </summary>
    /// <remarks>
    /// When a backend implements this interface, <see cref="IVectorsCanvas2D.DrawLines(ReadOnlySpan{POINT2}, SCALAR, in LineStyle)"/>
    /// should call <see cref="DrawThinLines(ReadOnlySpan{POINT2}, SCALAR, COLOR)"/> when it detects the line thickness is equal or less than 1.
    /// </remarks>
    public interface IDrawingBackend2D : IPrimitiveCanvas2D
    {
        /// <summary>
        /// Gets the number of pixels covered by a unit of length.
        /// </summary>
        /// <returns>The number of pixels of a unit.</returns>
        float GetPixelsPerUnit();

        /// <summary>
        /// Draws a sequence of lines between points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="thickness">The thickness of the line, usually 1 or less.</param>
        /// <param name="color">The color of the line.</param>
        void DrawThinLines(ReadOnlySpan<POINT2> points, float thickness, ColorStyle color);
    }

    public interface IDrawingBackend3D : IPrimitiveScene3D
    {
        float GetThinSegmentsSize();

        /// <summary>
        /// Draws a sequence of lines between points.
        /// </summary>
        /// <param name="a">The point 1</param>
        /// <param name="b">The point 1</param>
        /// <param name="diameter">The thickness of the line, usually 1 or less.</param>
        /// <param name="color">The color of the line.</param>
        void DrawSegment(Point3 a, Point3 b, float diameter, ColorStyle color);
    }


}
