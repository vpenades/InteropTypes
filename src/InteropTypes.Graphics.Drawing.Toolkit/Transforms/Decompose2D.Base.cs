using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using COLOR = System.Drawing.Color;

namespace InteropDrawing.Transforms
{
    partial struct Decompose2D
    {
        /// <summary>
        /// Represents a drawing canvas that translates complex vector drawing calls into basic convex polygons.
        /// </summary>
        /// <remarks>
        /// <see cref="IVectorsDrawing2D"/> 🡆 <see cref="IPolygonDrawing2D"/>
        /// </remarks>
        public abstract class Base : Backends.IDrawingBackend2D
        {
            #region API

            /// <inheritdoc/>
            public abstract void FillConvexPolygon(ReadOnlySpan<Point2> points, COLOR color);

            /// <inheritdoc/>
            public void DrawEllipse(Point2 center, float width, float height, in ColorStyle style)
            {
                Decompose2D.DrawEllipse(this, center, width, height, style);
            }

            /// <inheritdoc/>
            public void DrawLines(ReadOnlySpan<Point2> points, float diameter, in LineStyle style)
            {
                Decompose2D.DrawLines(this, points, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawPolygon(ReadOnlySpan<Point2> points, in PolygonStyle style)
            {
                Decompose2D.DrawPolygon(this, points, style);
            }

            /// <inheritdoc/>
            public abstract float GetThinLinesPixelSize();

            /// <inheritdoc/>
            public abstract void DrawThinLines(ReadOnlySpan<Point2> points, float thickness, COLOR color);

            #endregion
        }

    }
}
