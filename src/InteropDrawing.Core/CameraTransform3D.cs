using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Camera transform in 3D space.
    /// </summary>
    public struct CameraTransform3D
    {
        #region constructors
        
        public CameraTransform3D(Matrix4x4 position, float? fov, float? ortho, float? near, float? far)
        {
            WorldMatrix = position;
            VerticalFieldOfView = fov;
            OrthographicScale = ortho;
            NearPlane = near;
            FarPlane = far;
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
        public float? VerticalFieldOfView;

        /// <summary>
        /// if defined, the camera is ortographic camera.
        /// </summary>
        public float? OrthographicScale;

        public float? NearPlane;
        public float? FarPlane;

        #endregion

        #region API

        public Matrix4x4 CreateProjectionMatrix(float aspectRatio, float? nearPlane = null, float? farPlane = null)
        {
            float near = nearPlane ?? NearPlane ?? 0.1f;
            float far = farPlane ?? FarPlane ?? 1000f;

            if (VerticalFieldOfView.HasValue)
            {
                return Matrix4x4.CreatePerspectiveFieldOfView(VerticalFieldOfView.Value, aspectRatio, near, far);
            }
            else
            {
                var scale = OrthographicScale ?? 1;
                return Matrix4x4.CreateOrthographic(scale, scale, near, far);
            }
        }

        #endregion
    }
}
