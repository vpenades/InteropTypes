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
        public abstract class PassImageThrough : ICanvas2D, ColorStyle.IDefaultValue
        {
            #region data

            private IPrimitiveCanvas2D _Target;

            private InteropTypes.Graphics.Backends.IBackendCanvas2D _Backend;

            #endregion

            #region API

            /// <inheritdoc/>
            public ColorStyle DefaultColorStyle
            {
                get => ColorStyle.TryGetDefaultFrom(_Target);
                set => value.TrySetDefaultTo(_Target);
            }

            /// <summary>
            /// Sets the target <see cref="IPrimitiveCanvas2D"/> to where the drawing calls will be redirected.
            /// </summary>
            /// <param name="target">The drawing target.</param>
            protected void SetPassImageThroughTarget(IPrimitiveCanvas2D target)
            {
                if (object.ReferenceEquals(target, this)) throw new ArgumentException($"{nameof(target)} must not reference itself to avod a circular dependency.");
                _Target = target;

                _Backend = target as Backends.IBackendCanvas2D;
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
            public virtual void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
            {
                _Check();
                
                // we need to redirect the call through ourselves so the SetImage method is properly called.
                Decompose2D.DrawAsset(this, transform, asset, style);
            }

            /// <inheritdoc/>
            public void DrawImage(in Matrix3x2 transform, ImageStyle style)
            {
                _Check(); SetImage(style.Bitmap); _Target.DrawImage(transform, style);
            }

            /// <inheritdoc/>
            public void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle fillColor)
            {
                _Check(); SetImage(null); _Target.DrawConvexPolygon(points, fillColor);
            }            

            /// <inheritdoc/>
            public void DrawEllipse(POINT2 center, float width, float height, OutlineFillStyle style)
            {
                _Check(); SetImage(null);

                if (_Backend != null) Decompose2D.DrawEllipse(_Backend, center, width, height, style);
                else Decompose2D.DrawEllipse(_Target, center, width, height, style);
            }

            /// <inheritdoc/>
            public void DrawLines(ReadOnlySpan<POINT2> points, float diameter, LineStyle style)
            {
                _Check(); SetImage(null);

                if (_Backend != null) Decompose2D.DrawLines(_Backend, points, diameter, style);
                else Decompose2D.DrawLines(_Target, points, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawPolygon(ReadOnlySpan<POINT2> points, PolygonStyle style)
            {
                _Check(); SetImage(null);

                if (_Backend != null) Decompose2D.DrawPolygon(_Backend, points, style);
                else Decompose2D.DrawPolygon(_Target, points, style);
            }

            #endregion
        }
    }
}
