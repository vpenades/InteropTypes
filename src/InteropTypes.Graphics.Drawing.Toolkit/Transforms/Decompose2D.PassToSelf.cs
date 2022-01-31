using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using POINT2 = InteropTypes.Graphics.Drawing.Point2;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    partial struct Decompose2D
    {
        /// <summary>
        /// Base class for <see cref="IPrimitiveScene3D"/> pass through classes.
        /// </summary>
        public abstract class PassToSelf : ICanvas2D
        {
            /// <inheritdoc/>
            public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
            {
                Decompose2D.DrawAsset(this, transform, asset, style);
            }

            /// <inheritdoc/>
            public abstract void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle fillColor);

            /// <inheritdoc/>
            public abstract void DrawImage(in Matrix3x2 transform, in ImageStyle style);

            /// <inheritdoc/>
            public void DrawEllipse(POINT2 center, float width, float height, in OutlineFillStyle style)
            {
                Decompose2D.DrawEllipse(this, center, width, height, style);
            }            

            /// <inheritdoc/>
            public void DrawLines(ReadOnlySpan<POINT2> points, float diameter, in LineStyle style)
            {

/* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
Before:
                if (this is Backends.IDrawingBackend2D backend) Decompose2D.DrawLines(backend, points, diameter, style);
After:
                if (this is IDrawingBackend2D backend) Decompose2D.DrawLines(backend, points, diameter, style);
*/
                if (this is InteropTypes.Graphics.Drawing.Backends.IDrawingBackend2D backend) Decompose2D.DrawLines(backend, points, diameter, style);
                else Decompose2D.DrawLines(this, points, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawPolygon(ReadOnlySpan<POINT2> points, in PolygonStyle style)
            {
                Decompose2D.DrawPolygon(this, points, style);
            }
        }
    }
}
