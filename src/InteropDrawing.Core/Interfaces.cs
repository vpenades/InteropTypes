using System;

using ASSET = System.Object;
using SCALAR = System.Single;

using XFORM2 = System.Numerics.Matrix3x2;
using POINT2 = InteropDrawing.Point2;

using XFORM3 = System.Numerics.Matrix4x4;
using POINT3 = InteropDrawing.Point3;

namespace InteropDrawing
{
    /// <summary>
    /// defines an objects that exposes a unique key that doesn't change as long as
    /// the object itself doesn't change, and can be used by other objects to determine
    /// if this object has changed.    
    /// </summary>
    /// <example>
    /// var model1 = new Model3D();
    /// var gpuDict = new Dictionary<Object,GPUModel>();
    /// gpuDict[model1.ImmutableKey] = new GPUModel(model1);
    /// </example>
    public interface IPseudoImmutable
    {
        Object ImmutableKey { get; }

        // void InvalidateImmutableKey();
    }
    
    public interface IPolygonDrawing2D
    {
        void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style);
    }

    public interface IDrawing2D : IPolygonDrawing2D
    {
        // methods could return a value:
        // - a boolean, indicating success or failure/unsupported
        // - or an object that could be used to fill additional metadata.

        // metadata could be set with Push/Pop methods


        void DrawAsset(in XFORM2 transform, ASSET asset, ColorStyle style);

        void DrawLines(ReadOnlySpan<Point2> points, SCALAR diameter, LineStyle style);

        void DrawEllipse(Point2 center, SCALAR width, SCALAR height, ColorStyle style);        

        void DrawSprite(in XFORM2 transform, in SpriteStyle style);
    }

    public interface ISurfaceDrawing3D
    {
        void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle style);
    }

    public interface IDrawing3D : ISurfaceDrawing3D
    {
        void DrawAsset(in XFORM3 transform, ASSET asset, ColorStyle style);

        void DrawSegment(POINT3 a, POINT3 b, SCALAR diameter, LineStyle style);

        void DrawSphere(POINT3 center, SCALAR diameter, ColorStyle style);        
    }

    

    public delegate void Drawing2DAction(IDrawing2D context, Point2 viewport);

    public interface IDrawingContext2D : IDrawing2D, IDisposable { }

    public interface IDrawingContext3D : IDrawing3D, IDisposable { }

    public interface ISceneViewport2D
    {
        (XFORM2 Camera, XFORM2 Projection) GetMatrices(float renderWidth, float renderHeight);
    }

    public interface ISceneViewport3D
    {
        (XFORM3 Camera, XFORM3 Projection) GetMatrices(float renderWidth, float renderHeight);
    }
}