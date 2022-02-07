using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;
using XYZW = System.Numerics.Vector4;

using SIZE = System.Numerics.Vector2;
using PLANE = System.Numerics.Plane;

using BBOX = System.Numerics.Matrix3x2;

using COLOR = System.Drawing.Color;


namespace InteropTypes.Graphics.Drawing
{
    public class CameraProjection3D
    {
        #region CONSTANTS

        public static readonly CameraProjection3D AutomaticPerspective = new CameraProjection3D(null, default);

        // https://en.wikipedia.org/wiki/Orthographic_projection#/media/File:Graphical_projection_comparison.png

        public static readonly CameraProjection3D AutomaticIsometric = new CameraProjection3D(null, default);

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



        /* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
        Before:
                private CameraProjection3D(Transforms.ProjectPointCallback proj, PLANE np)
        After:
                private CameraProjection3D(ProjectPointCallback proj, PLANE np)
        */
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


        /* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
        Before:
                private Transforms.ProjectPointCallback _ProjFunc;
        After:
                private ProjectPointCallback _ProjFunc;
        */
        private Transforms.ProjectPointCallback _ProjFunc;
        private PLANE _NearPlane;

        #endregion

        #region API


        /* Unmerged change from project 'InteropTypes.Graphics.Drawing.Toolkit (netstandard2.1)'
        Before:
                public void GetProjectionInfo(out Transforms.ProjectPointCallback proj, out PLANE plane, Record3D scene)
        After:
                public void GetProjectionInfo(out ProjectPointCallback proj, out PLANE plane, Record3D scene)
        */
        public void GetProjectionInfo(out Transforms.ProjectPointCallback proj, out PLANE plane, Record3D scene)
        {
            proj = _ProjFunc;
            plane = _NearPlane;
        }

        public bool IsVisible(XYZ p)
        {
            var d = PLANE.DotCoordinate(_NearPlane, p);

            return d > 0;
        }

        #endregion
    }
}
