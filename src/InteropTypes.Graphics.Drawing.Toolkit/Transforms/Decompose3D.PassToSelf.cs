﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropDrawing.Transforms
{
    partial struct Decompose3D
    {        
        public abstract class PassToSelf : IScene3D
        {
            /// <inheritdoc/>
            public abstract void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style);

            /// <inheritdoc/>
            public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle style)
            {
                Decompose3D.DrawAsset(this, transform, asset, style);
            }

            /// <inheritdoc/>
            public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle style)
            {
                Decompose3D.DrawSegment(this, a, b, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawSphere(Point3 center, float diameter, OutlineFillStyle style)
            {
                Decompose3D.DrawSphere(this, center, diameter, style);
            }

            /// <inheritdoc/>
            public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
            {
                Decompose3D.DrawSurface(this, vertices, style);
            }
        }
    }
}