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
        /// Base class for <see cref="ICoreScene3D"/> pass through classes.
        /// </summary>
        public abstract class PassToSelf : ICanvas2D
        {
            #region primitives

            /// <inheritdoc/>
            public abstract void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle fillColor);

            /// <inheritdoc/>
            public abstract void DrawImage(in Matrix3x2 transform, ImageStyle style);

            #endregion

            #region extended

            /// <inheritdoc/>
            public virtual void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
            {
                Decompose2D.DrawAsset(this, transform, asset, style);
            }            

            /// <inheritdoc/>
            public void DrawEllipse(POINT2 center, float width, float height, OutlineFillStyle style)
            {
                if (this is Backends.IBackendCanvas2D backend) Decompose2D.DrawEllipse(backend, center, width, height, style);
                else Decompose2D.DrawEllipse(this, center, width, height, style);
            }            

            /// <inheritdoc/>
            public void DrawLines(ReadOnlySpan<POINT2> points, float diameter, LineStyle style)
            {
                if (this is Backends.IBackendCanvas2D backend) Decompose2D.DrawLines(backend, points, diameter, style);
                else Decompose2D.DrawLines(this, points, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawPolygon(ReadOnlySpan<POINT2> points, PolygonStyle style)
            {
                if (this is Backends.IBackendCanvas2D backend) Decompose2D.DrawPolygon(backend, points, style);
                else Decompose2D.DrawPolygon(this, points, style);
            }

            #endregion
        }
    }
}
