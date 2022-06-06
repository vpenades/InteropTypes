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
        public abstract class PassThrough : ICanvas2D, GlobalStyle.ISource
        {
            #region data

            private ICoreCanvas2D _Target;

            #endregion

            #region API
            
            bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
            {
                return GlobalStyle.TryGetGlobalProperty(_Target, name, out value);
            }
            
            bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
            {
                return GlobalStyle.TrySetGlobalProperty(_Target, name, value);
            }

            /// <summary>
            /// Sets the target <see cref="ICoreCanvas2D"/> to where the drawing calls will be redirected.
            /// </summary>
            /// <param name="target">The drawing target.</param>
            protected void SetPassThroughTarget(ICoreCanvas2D target)
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
            public virtual void DrawAsset(in Matrix3x2 transform, object asset)
            {
                _Check(); Decompose2D.DrawAsset(_Target, transform, asset);
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

            public void DrawTextLine(in Matrix3x2 transform, string text, float size, FontStyle font)
            {
                _Check(); font.DrawDecomposedTo(_Target, transform, text, size);
            }

            #endregion
        }
    }
}
