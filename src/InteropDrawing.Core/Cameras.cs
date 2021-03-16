using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using XYZW = System.Numerics.Vector4;

using SIZE = System.Numerics.Vector2;
using PLANE = System.Numerics.Plane;

using BBOX = System.Numerics.Matrix3x2;

using COLOR = System.Drawing.Color;

namespace InteropDrawing
{
    public struct CameraView3D
    {
        #region constructors

        public static CameraView3D CreatePerspective(Point3 from, Point3 to)
        {
            return new CameraView3D
            {
                WorldMatrix = CreateWorldMatrix(from, to, 1),
                VerticalFieldOfView = MathF.PI / 4
            };
        }

        public static CameraView3D CreateLookingAt(Model3D scene, XYZ direction)
        {
            if (direction == XYZ.Zero) direction = -XYZ.UnitZ;

            direction = XYZ.Normalize(direction);

            throw new NotImplementedException();

            // var bounds = scene.SphereBounds;

            // return CreatePerspective(bounds.Center - direction * bounds.Radius * 2, bounds.Center);
        }

        
        public static CameraView3D CreateDefaultFrom(BBOX bounds)
        {
            // if (bounds.IsInvalid) return CreatePerspective(XYZ.UnitZ * 100, XYZ.Zero);

            var center = bounds.MinMaxCenter();
            var from = bounds.ColumnY() - center;

            return CreatePerspective(from, center);
        }

        #endregion

        #region data

        /// <summary>
        /// Transform Matrix of the camera in World Space.
        /// </summary>
        /// <remarks>
        /// The camera looks into the negative Z
        /// </remarks>
        public Matrix4x4 WorldMatrix;

        /// <summary>
        /// If defined, the camera is a perspective matrix
        /// </summary>
        public Single? VerticalFieldOfView;

        /// <summary>
        /// if defined, the camera is ortographic camera.
        /// </summary>
        public Single? OrthographicScale;

        public Single? NearPlane;
        public Single? FarPlane;

        #endregion        

        #region API

        public void DrawTo(IDrawing3D dc, float cameraSize)
        {
            dc.DrawCamera(WorldMatrix, cameraSize);

            var p = CreateProjectionMatrix(1);

            dc.DrawProjectedPlane(WorldMatrix, p, cameraSize * 0.05f, COLOR.BlueViolet);            
        }

        /// <summary>
        /// Creates a world matrix for a camera looking into negative Z axis
        /// </summary>
        /// <param name="fromPosition">camera location in world space.</param>
        /// <param name="targetPosition">target location in world space.</param>
        /// <param name="minDist">minimum distance between camera and target.</param>
        /// <returns>A world matrix camera.</returns>
        public static Matrix4x4 CreateWorldMatrix(Point3 fromPosition, Point3 targetPosition, float minDist = 0)
        {
            var targetPos = targetPosition.ToNumerics();
            var froPos = fromPosition.ToNumerics();

            var d = targetPos - froPos;

            if (minDist > 0 && d.Length() < minDist)
            {
                d = d.WithLength(minDist);
                fromPosition = targetPos - d;
            }

            return Matrix4x4.CreateWorld(froPos, d, XYZ.UnitY);
        }

        public Matrix4x4 CreateProjectionMatrix(float aspectRatio, float? nearPlane = null, float? farPlane = null)
        {
            float near = nearPlane ?? this.NearPlane ?? 0.1f;
            float far = farPlane ?? this.FarPlane ?? 1000f;

            if (VerticalFieldOfView.HasValue)
            {
                return Matrix4x4.CreatePerspectiveFieldOfView(VerticalFieldOfView.Value, aspectRatio, near, far);
            }
            else
            {
                return Matrix4x4.CreateOrthographic(OrthographicScale.Value, OrthographicScale.Value, near, far);
            }
        }
        
        #endregion
    }

    public class CameraProjection3D
    {
        #region CONSTANTS

        public static readonly CameraProjection3D AutomaticPerspective = new CameraProjection3D(null, default(PLANE));

        // https://en.wikipedia.org/wiki/Orthographic_projection#/media/File:Graphical_projection_comparison.png

        public static readonly CameraProjection3D AutomaticIsometric = new CameraProjection3D(null, default(PLANE));

        #endregion

        #region lifecycle        

        public static CameraProjection3D CreateOrtho(int viewportWidth, int viewportHeight, float sceneSize)
        {
            var camera = Matrix4x4.CreateWorld(XYZ.UnitZ * sceneSize, -XYZ.UnitZ, XYZ.UnitY);

            return CreateOrtho(new SIZE(viewportWidth, viewportHeight), camera);
        }

        public static CameraProjection3D CreatePerspective(int viewportWidth, int viewportHeight, float sceneSize)
        {
            return CreatePerspective(new SIZE(viewportWidth, viewportHeight), XYZ.UnitZ * sceneSize, XYZ.Zero, -XYZ.UnitY);
        }

        public static CameraProjection3D CreatePerspective(SIZE viewport, XYZ cameraPosition, XYZ cameraTarget, XYZ cameraUpVector)
        {
            var camera = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
            return CreatePerspective(viewport, camera);
        }

        public static CameraProjection3D CreatePerspective(SIZE viewport, Matrix4x4 camMatrix)
        {
            // create frustum clipping plane (performed in worldspace)

            var dir = XYZ.TransformNormal(XYZ.UnitZ, camMatrix);
            var frustumNearPlane = new PLANE(dir, -XYZ.Dot(dir, camMatrix.Translation));

            // create world-to-view transform

            Matrix4x4.Invert(camMatrix, out Matrix4x4 viewMatrix);

            var projMatrix = Matrix4x4.CreatePerspectiveFieldOfView(1, viewport.X / viewport.Y, 0.5f, 10);

            viewport *= 0.5f;

            var portMatrix = new Matrix4x4
                (
                viewport.X, 0, 0, 0,
                0, viewport.Y, 0, 0,
                0, 0, 1, 0,
                viewport.X, viewport.Y, 0, 1
                );

            XYZ projFunc(XYZ p)
            {
                // classical transform pipeline
                p = XYZ.Transform(p, viewMatrix);
                p = XYZ.Transform(p, projMatrix);
                p.X /= p.Z;
                p.Y /= p.Z;
                p = XYZ.Transform(p, portMatrix);
                return p;
            }

            return new CameraProjection3D(projFunc, frustumNearPlane);
        }

        public static CameraProjection3D CreateOrtho(SIZE viewport, Matrix4x4 camMatrix)
        {




            // create world-to-view transform
            Matrix4x4.Invert(camMatrix, out Matrix4x4 viewMatrix);

            // create frustum clipping plane (performed in worldspace)
            var dir = XYZ.TransformNormal(XYZ.UnitZ, viewMatrix);
            var frustumNearPlane = new PLANE(dir, -XYZ.Dot(dir, viewMatrix.Translation));

            //var projMatrix = Matrix4x4.CreateOrthographic(viewport.X, viewport.Y, -0.5f, -1000);

            viewport *= 0.5f;
            var projMatrix = Matrix4x4.CreateOrthographicOffCenter(-viewport.X, +viewport.X, -viewport.Y, viewport.Y, -0.5f, -1000);

            var portMatrix = new Matrix4x4
                (
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                viewport.X, viewport.Y, 0, 1
                );

            XYZ projFunc(XYZ p)
            {
                // classical transform pipeline
                p = XYZ.Transform(p, viewMatrix);
                p = XYZ.Transform(p, projMatrix);
                p.X /= p.Z;
                p.Y /= p.Z;
                p = XYZ.Transform(p, portMatrix);
                return p;
            }

            return new CameraProjection3D(projFunc, frustumNearPlane);
        }

        public static CameraProjection3D Create(Transforms.ProjectPointCallback projfunc, XYZ cameraPosition, XYZ cameraAdvancePosition)
        {
            var direction = XYZ.Normalize(cameraAdvancePosition - cameraPosition);
            var plane = new PLANE(direction, -XYZ.Dot(direction, cameraPosition));

            return Create(projfunc, plane);
        }

        public static CameraProjection3D Create(Transforms.ProjectPointCallback projfunc, PLANE frustumNearPlane)
        {
            if (projfunc == null) return null;
            return new CameraProjection3D(projfunc, frustumNearPlane);
        }

        private CameraProjection3D(Transforms.ProjectPointCallback proj, PLANE np)
        {
            _ProjFunc = proj;
            _NearPlane = np;
        }

        public static CameraProjection3D CreatePerspectiveOpenGL(SIZE viewport, Matrix4x4 camMatrix)
        {
            // create frustum clipping plane (performed in worldspace)

            var dir = XYZ.TransformNormal(-XYZ.UnitZ, camMatrix);
            var frustumNearPlane = new PLANE(dir, -XYZ.Dot(dir, camMatrix.Translation));

            // create world-to-view transform

            Matrix4x4.Invert(camMatrix, out Matrix4x4 viewMatrix);

            var projMatrix = Matrix4x4.CreatePerspectiveFieldOfView(1, viewport.X / viewport.Y, 0.5f, 10);

            viewport *= 0.5f;

            var portMatrix = new Matrix4x4
                (
                viewport.X, 0, 0, 0,
                0, viewport.Y, 0, 0,
                0, 0, 1, 0,
                viewport.X, viewport.Y, 0, 1
                );

            XYZ projFunc(XYZ p)
            {
                // classical transform pipeline
                p = XYZ.Transform(p, viewMatrix);
                p = XYZ.Transform(p, projMatrix);
                p.X /= p.Z;
                p.Y /= -p.Z;
                p = XYZ.Transform(p, portMatrix);
                return p;
            }

            return new CameraProjection3D(projFunc, frustumNearPlane);
        }

        #endregion

        #region data

        private Transforms.ProjectPointCallback _ProjFunc;
        private PLANE _NearPlane;

        #endregion

        #region API

        public void GetProjectionInfo(out Transforms.ProjectPointCallback proj, out PLANE plane, Model3D scene)
        {
            proj = _ProjFunc;
            plane = _NearPlane;
        }

        public bool IsVisible(Vector3 p)
        {
            var d = PLANE.DotCoordinate(_NearPlane, p);

            return d > 0;
        }

        #endregion
    }
}
