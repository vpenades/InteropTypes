﻿using System;
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
                if (object.ReferenceEquals(target, this)) throw new ArgumentException($"{nameof(target)} must not reference itself to avod a circular dependency. Derive from {nameof(PassToSelf)} instead.");
                _Target = target;
            }

            private void _Check()
            {
                if (_Target == null) throw new ObjectDisposedException(nameof(_Target));
            }

            #endregion

            #region API - IPrimitiveCanvas2D

            /// <inheritdoc/>
            public void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle fillColor)
            {
                _Check(); _Target.DrawConvexPolygon(points, fillColor);
            }

            /// <inheritdoc/>
            public void DrawImage(in Matrix3x2 transform, ImageStyle style)
            {
                _Check(); _Target.DrawImage(transform, style);
            }

            #endregion

            #region API - ICanvas2D

            /// <inheritdoc/>
            public virtual void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
            {
                _Check(); Decompose2D.DrawAsset(_Target, transform, asset, style);
            }            

            /// <inheritdoc/>
            public void DrawEllipse(POINT2 center, float width, float height, OutlineFillStyle style)
            {
                _Check(); Decompose2D.DrawEllipse(_Target, center, width, height, style);
            }            

            /// <inheritdoc/>
            public void DrawLines(ReadOnlySpan<POINT2> points, float diameter, LineStyle style)
            {
                _Check(); Decompose2D.DrawLines(_Target, points, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawPolygon(ReadOnlySpan<POINT2> points, PolygonStyle style)
            {
                _Check(); Decompose2D.DrawPolygon(_Target, points, style);
            }

            #endregion
        }
    }
}
