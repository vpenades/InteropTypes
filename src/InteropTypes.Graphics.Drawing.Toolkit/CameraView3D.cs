using System;
using System.Numerics;

using XYZ = System.Numerics.Vector3;
using BBOX = System.Numerics.Matrix3x2;
using COLOR = System.Drawing.Color;

namespace InteropTypes.Graphics.Drawing
{
    public struct CameraView3D
    {
        #region constructors

        public static implicit operator CameraTransform3D(CameraView3D cam) { return cam.Camera; }

        public static CameraView3D CreateDefaultFrom(BBOX bounds)
        {
            // if (bounds.IsInvalid) return CreatePerspective(XYZ.UnitZ * 100, XYZ.Zero);

            var center = bounds.MinMaxCenter();
            var from = bounds.ColumnY() - center;

            from *= 4;

            return CreatePerspective(from, center);
        }

        public static CameraView3D CreatePerspective(Point3 from, Point3 to)
        {
            var cam = new CameraTransform3D
            {
                WorldMatrix = CreateWorldMatrix(from, to, 1),
                VerticalFieldOfView = MathF.PI / 4
            };

            return new CameraView3D(cam);
        }

        public static CameraView3D CreateLookingAt(Record3D scene, XYZ direction)
        {
            if (direction == XYZ.Zero) direction = -XYZ.UnitZ;

            direction = XYZ.Normalize(direction);

            throw new NotImplementedException();

            // var bounds = scene.SphereBounds;

            // return CreatePerspective(bounds.Center - direction * bounds.Radius * 2, bounds.Center);
        }        

        public CameraView3D(CameraTransform3D cam) { Camera = cam; }

        #endregion

        #region data

        public CameraTransform3D Camera;

        #endregion

        #region properties

        /// <summary>
        /// Transform Matrix of the camera in World Space.
        /// </summary>
        /// <remarks>
        /// The camera looks into the negative Z
        /// </remarks>
        public Matrix4x4 WorldMatrix => Camera.WorldMatrix;

        /// <summary>
        /// If defined, the camera is a perspective matrix
        /// </summary>
        public float? VerticalFieldOfView => Camera.VerticalFieldOfView;

        /// <summary>
        /// if defined, the camera is ortographic camera.
        /// </summary>
        public float? OrthographicScale => Camera.OrthographicScale;

        public float? NearPlane => Camera.NearPlane;
        public float? FarPlane => Camera.FarPlane;

        #endregion        

        #region API

        public void DrawTo(IScene3D dc, float cameraSize)
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
            return Camera.CreateProjectionMatrix(aspectRatio, nearPlane, farPlane);
        }

        #endregion
    }

    
}
