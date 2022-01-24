﻿using System;
using System.Collections.Generic;
using System.Text;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;

using XFORM3 = System.Numerics.Matrix4x4;
using POINT3 = InteropDrawing.Point3;

namespace InteropDrawing
{
    

    public interface IBounds3D
    {
        (System.Numerics.Vector3 Center, Single Radius) GetBoundingSphere();
    }

    /// <summary>
    /// Represents a render target context where we can draw 3D surfaces
    /// </summary>
    public interface ISurfaceDrawing3D
    {
        void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle style);
    }

    /// <summary>
    /// Represents a render target context where we can draw 3D primitives.
    /// </summary>
    public interface IDrawing3D : ISurfaceDrawing3D
    {
        void DrawAsset(in XFORM3 transform, ASSET asset, ColorStyle style);

        void DrawSegment(POINT3 a, POINT3 b, SCALAR diameter, LineStyle style);

        void DrawSphere(POINT3 center, SCALAR diameter, ColorStyle style);
    }

    public interface IScene3D : IDrawing3D { }


    

    public interface ISceneViewport2D
    {
        (XFORM2 Camera, XFORM2 Projection) GetMatrices(float renderWidth, float renderHeight);
    }

    public interface ISceneViewport3D
    {
        // TODO: include near/far depth plane distance hints

        (XFORM3 Camera, XFORM3 Projection) GetMatrices(float renderWidth, float renderHeight);
    }    
}
