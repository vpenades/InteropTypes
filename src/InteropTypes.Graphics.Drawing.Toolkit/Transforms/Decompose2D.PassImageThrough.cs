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
        public abstract class PassImageThrough : ICanvas2D
        {
            #region data

            private IPrimitiveCanvas2D _Target;

/* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
Before:
            private Backends.IDrawingBackend2D _Backend;
After:
            private IDrawingBackend2D _Backend;
*/
            private InteropTypes.Graphics.Drawing.Backends.IDrawingBackend2D _Backend;

            #endregion

            #region API

            /// <summary>
            /// Sets the target <see cref="IPrimitiveCanvas2D"/> to where the drawing calls will be redirected.
            /// </summary>
            /// <param name="target">The drawing target.</param>
            protected void SetPassImageThroughTarget(IPrimitiveCanvas2D target)
            {
                if (object.ReferenceEquals(target, this)) throw new ArgumentException($"{nameof(target)} must not reference itself to avod a circular dependency.");
                _Target = target;

/* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
Before:
                _Backend = target as Backends.IDrawingBackend2D;
After:
                _Backend = target as IDrawingBackend2D;
*/
                _Backend = target as InteropTypes.Graphics.Drawing.Backends.IDrawingBackend2D;
            }

            /// <summary>
            /// This method is called to set or clear the current image drawing
            /// </summary>
            /// <param name="image">the image being set, or null</param>
            protected abstract void SetImage(ImageAsset image);

            private void _Check()
            {
                if (_Target == null) throw new ObjectDisposedException(nameof(_Target));
            }

            #endregion

            #region API - ICanvas2D

            /// <inheritdoc/>
            public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
            {
                _Check();
                
                // we need to redirect the call through ourselves so the SetImage method is properly called.
                Decompose2D.DrawAsset(this, transform, asset, style);
            }

            /// <inheritdoc/>
            public void DrawImage(in Matrix3x2 transform, in ImageStyle style)
            {
                _Check(); SetImage(style.Bitmap); _Target.DrawImage(transform, style);
            }

            /// <inheritdoc/>
            public void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle fillColor)
            {
                _Check(); SetImage(null); _Target.DrawConvexPolygon(points, fillColor);
            }            

            /// <inheritdoc/>
            public void DrawEllipse(POINT2 center, float width, float height, in OutlineFillStyle style)
            {
                _Check(); SetImage(null);

                if (_Backend != null) Decompose2D.DrawEllipse(_Backend, center, width, height, style);
                else Decompose2D.DrawEllipse(_Target, center, width, height, style);
            }

            /// <inheritdoc/>
            public void DrawLines(ReadOnlySpan<POINT2> points, float diameter, in LineStyle style)
            {
                _Check(); SetImage(null);

                if (_Backend != null) Decompose2D.DrawLines(_Backend, points, diameter, style);
                else Decompose2D.DrawLines(_Target, points, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawPolygon(ReadOnlySpan<POINT2> points, in PolygonStyle style)
            {
                _Check(); SetImage(null);

                if (_Backend != null) Decompose2D.DrawPolygon(_Backend, points, style);
                else Decompose2D.DrawPolygon(_Target, points, style);
            }

            #endregion
        }
    }
}
