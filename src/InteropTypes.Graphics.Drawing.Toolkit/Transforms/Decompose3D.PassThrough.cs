using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;



namespace InteropTypes.Graphics.Drawing.Transforms
{
    partial struct Decompose3D
    {
        /// <summary>
        /// Base class for <see cref="IPrimitiveScene3D"/> pass through classes.
        /// </summary>
        public abstract class PassThrough : IScene3D
        {
            #region data

            private IPrimitiveScene3D _Target;

            #endregion

            #region API

            /// <summary>
            /// Sets the target <see cref="IPrimitiveScene3D"/> to where the drawing calls will be redirected.
            /// </summary>
            /// <param name="target">The drawing target.</param>
            protected void SetPassThroughTarget(IPrimitiveScene3D target)
            {
                if (object.ReferenceEquals(target, this)) throw new ArgumentException($"{nameof(target)} must not reference itself to avod a circular dependency. Derive from {nameof(PassToSelf)} instead.");
                _Target = target;
            }

            private void _Check()
            {
                if (_Target == null) throw new ObjectDisposedException(nameof(_Target));
            }

            #endregion

            #region API - IScene3D

            /// <inheritdoc/>
            public virtual void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle style)
            {
                _Check(); Decompose3D.DrawAsset(_Target, transform, asset, style);
            }

            /// <inheritdoc/>
            public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
            {
                _Check(); _Target.DrawConvexSurface(vertices, style);
            }            

            /// <inheritdoc/>
            public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle style)
            {
                _Check(); Decompose3D.DrawSegment(_Target, a, b, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawSphere(Point3 center, float diameter, OutlineFillStyle style)
            {
                _Check(); Decompose3D.DrawSphere(_Target, center, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
            {
                _Check(); Decompose3D.DrawSurface(_Target, vertices, style);
            }

            #endregion
        }
    }
}
