using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;
using POINT2 = InteropDrawing.Point2;
using VECTOR2 = System.Numerics.Vector2;
using System.Drawing;

using COLOR = System.Drawing.Color;

namespace InteropDrawing.Transforms
{
    partial struct Decompose2D
    {
        /// <summary>
        /// Base class for <see cref="IPrimitiveScene3D"/> pass through classes.
        /// </summary>
        public abstract class PassThrough : ICanvas2D
        {
            #region data

            private IPrimitiveCanvas2D _Target;

            #endregion

            #region API

            /// <summary>
            /// Sets the target <see cref="IPrimitiveCanvas2D"/> to where the drawing calls will be redirected.
            /// </summary>
            /// <param name="target">The drawing target.</param>
            protected void SetPassThroughTarget(IPrimitiveCanvas2D target)
            {
                if (object.ReferenceEquals(target, this)) throw new ArgumentException($"{nameof(target)} must not reference itself to avod a circular dependency.");
                _Target = target;
            }

            private void _Check()
            {
                if (_Target == null) throw new ObjectDisposedException(nameof(_Target));
            }

            #endregion

            #region API - ICanvas

            /// <inheritdoc/>
            public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
            {
                _Check(); Decompose2D.DrawAsset(_Target, transform, asset, style);
            }

            /// <inheritdoc/>
            public void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle fillColor)
            {
                _Check(); _Target.DrawConvexPolygon(points, fillColor);
            }

            /// <inheritdoc/>
            public void DrawEllipse(POINT2 center, float width, float height, in OutlineFillStyle style)
            {
                _Check(); Decompose2D.DrawEllipse(_Target, center, width, height, style);
            }

            /// <inheritdoc/>
            public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
            {
                _Check(); _Target.DrawImage(transform, style);
            }

            /// <inheritdoc/>
            public void DrawLines(ReadOnlySpan<POINT2> points, float diameter, in LineStyle style)
            {
                _Check(); Decompose2D.DrawLines(_Target, points, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawPolygon(ReadOnlySpan<POINT2> points, in PolygonStyle style)
            {
                _Check(); Decompose2D.DrawPolygon(_Target, points, style);
            }

            #endregion
        }
    }
}
