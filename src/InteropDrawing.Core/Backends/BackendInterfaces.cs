using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;
using POINT2 = InteropDrawing.Point2;

using COLOR = System.Drawing.Color;

namespace InteropDrawing.Backends
{
    /// <summary>
    /// Represents a drawing canvas where we can draw 2D thin lines.
    /// </summary>
    /// <remarks>
    /// When a backend implements this interface, <see cref="IVectorsDrawing2D.DrawLines(ReadOnlySpan{POINT2}, SCALAR, in LineStyle)"/>
    /// should call <see cref="DrawThinLines(ReadOnlySpan{POINT2}, SCALAR, COLOR)"/> when it detects the line thickness is equal or less than 1.
    /// </remarks>
    public interface IDrawingBackend2D : IPolygonDrawing2D
    {
        float GetThinLinesPixelSize();

        /// <summary>
        /// Draws a sequence of lines between points.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="thickness">The thickness of the line, usually 1 or less.</param>
        /// <param name="color">The color of the line.</param>
        void DrawThinLines(ReadOnlySpan<POINT2> points, float thickness, COLOR color);
    }

    public interface IDrawingBackend3D : ISurfaceDrawing3D
    {
        float GetThinSegmentsSize();

        /// <summary>
        /// Draws a sequence of lines between points.
        /// </summary>
        /// <param name="a">The point 1</param>
        /// <param name="b">The point 1</param>
        /// <param name="diameter">The thickness of the line, usually 1 or less.</param>
        /// <param name="color">The color of the line.</param>
        void DrawSegment(Point3 a, Point3 b, float diameter, COLOR color);
    }

    /// <summary>
    /// provides additional information about the rendering backend
    /// </summary>
    /// <remarks>
    /// This interface must be implemented by the final rendering backend, and
    /// queried through any exposed <see cref="IDrawing2D"/> casted to a
    /// <see cref="IServiceProvider"/>
    /// </remarks>
    public interface IViewportInfo
    {
        /// <summary>
        /// The viewport width i pixels
        /// </summary>
        int PixelsWidth { get; }

        /// <summary>
        /// The viewport height in pixels
        /// </summary>
        int PixelsHeight { get; }
    }
}
