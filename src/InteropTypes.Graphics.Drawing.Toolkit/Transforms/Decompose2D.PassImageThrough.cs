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
        public abstract class PassImageThrough : ICanvas2D, GlobalStyle.ISource
        {
            #region data

            private ICoreCanvas2D _Target;

            private Backends.IBackendCanvas2D _Backend;

            #endregion

            #region API
            
            /// <inheritdoc/>
            bool GlobalStyle.ISource.TryGetGlobalProperty<T>(string name, out T value)
            {
                return GlobalStyle.TryGetGlobalProperty(_Target, name, out value);
            }

            /// <inheritdoc/>
            bool GlobalStyle.ISource.TrySetGlobalProperty<T>(string name, T value)
            {
                return GlobalStyle.TrySetGlobalProperty(_Target, name, value);
            }

            /// <summary>
            /// Sets the target <see cref="ICoreCanvas2D"/> to where the drawing calls will be redirected.
            /// </summary>
            /// <param name="target">The drawing target.</param>
            protected void SetPassImageThroughTarget(ICoreCanvas2D target)
            {
                if (object.ReferenceEquals(target, this)) throw new ArgumentException($"{nameof(target)} must not reference itself to avod a circular dependency.");
                _Target = target;

                _Backend = target as Backends.IBackendCanvas2D;
            }

            /// <summary>
            /// This method is called to set or clear the current image drawing
            /// </summary>
            /// <param name="image">the image being set, or null</param>
            protected abstract void SetImage(ImageSource image);

            private void _Check()
            {
                if (_Target == null) throw new ObjectDisposedException(nameof(_Target));
            }

            #endregion

            #region API - ICanvas2D

            /// <inheritdoc/>
            public virtual void DrawAsset(in Matrix3x2 transform, object asset)
            {
                _Check();
                
                // we need to redirect the call through ourselves so the SetImage method is properly called.
                Decompose2D.DrawAsset(this, transform, asset);
            }

            /// <inheritdoc/>
            public void DrawImage(in Matrix3x2 transform, ImageStyle style)
            {
                _Check(); SetImage(style.Image); _Target.DrawImage(transform, style);
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

            public void DrawTextLine(in Matrix3x2 transform, string text, float size, FontStyle font)
            {
                // Text can be drawn either as vectorial shapes or by using sprite sheets.

                _Check();

                if (_Backend != null) font.DrawDecomposedTo(_Backend, transform, text, size);
                else font.DrawDecomposedTo(_Target, transform, text, size);
            }

            #endregion
        }
    }
}
