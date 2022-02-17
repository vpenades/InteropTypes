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

        /// <summary>
        /// Near plane must be more than zero and less than <see cref="FarPlane"/>
        /// </summary>
        public float? NearPlane;

        /// <summary>
        /// Far plane must be more than <see cref="NearPlane"/> or infinite.
        /// </summary>
        public float? FarPlane;

        #endregion

        #region API

        public Matrix4x4 CreateProjectionMatrix(float aspectRatio, float? nearPlane = null, float? farPlane = null)
        {
            float near = nearPlane ?? NearPlane ?? 0.1f;
            float far = farPlane ?? FarPlane ?? 1000f;

            if (VerticalFieldOfView.HasValue)
            {
                #if NETSTANDARD2_1_OR_GREATER                
                return Matrix4x4.CreatePerspectiveFieldOfView(VerticalFieldOfView.Value, aspectRatio, near, far);
                #else
                return _CreatePerspectiveFieldOfView(VerticalFieldOfView.Value, aspectRatio, near, far);
                #endif
            }
            else
            {
                var scale = OrthographicScale ?? 1;
                return Matrix4x4.CreateOrthographic(scale, scale, near, far);
            }
        }

        #if !NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix4x4.cs">CreatePerspectiveFieldOfView</see>        
        /// </summary>
        /// <remarks>
        /// Older versions of CreatePerspectiveFieldOfView don't support infinite FarPlane
        /// </remarks>
        private static Matrix4x4 _CreatePerspectiveFieldOfView(float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            if (fieldOfView <= 0.0f || fieldOfView >= Math.PI) throw new ArgumentOutOfRangeException(nameof(fieldOfView));
            if (nearPlaneDistance <= 0.0f) throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));
            if (farPlaneDistance <= 0.0f) throw new ArgumentOutOfRangeException(nameof(farPlaneDistance));
            if (nearPlaneDistance >= farPlaneDistance) throw new ArgumentOutOfRangeException(nameof(nearPlaneDistance));

            float yScale = 1.0f / (float)Math.Tan(fieldOfView * 0.5f);
            float xScale = yScale / aspectRatio;

            Matrix4x4 result;

            result.M11 = xScale;
            result.M12 = result.M13 = result.M14 = 0.0f;

            result.M22 = yScale;
            result.M21 = result.M23 = result.M24 = 0.0f;

            result.M31 = result.M32 = 0.0f;
            float negFarRange = float.IsPositiveInfinity(farPlaneDistance) ? -1.0f : farPlaneDistance / (nearPlaneDistance - farPlaneDistance);
            result.M33 = negFarRange;
            result.M34 = -1.0f;

            result.M41 = result.M42 = result.M44 = 0.0f;
            result.M43 = nearPlaneDistance * negFarRange;

            return result;
        }
        #endif

        #endregion
    }
}
