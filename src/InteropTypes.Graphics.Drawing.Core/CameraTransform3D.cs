using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Camera transform in 3D space.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All proyection parameters used to construct a projection matrix can be set up in advanced except one;
    /// <b>Aspect Ratio</b>, which depends on the actual display window.
    /// </para>
    /// <para>
    /// This structure is divided into two section:<br/>
    /// - A <see cref="WorldMatrix"/> representing the position of the camera within the scene.
    /// - A <see cref="CreateProjectionMatrix(float, float?, float?)"/> method to retrieve the projection matrix.
    /// </para>    
    /// </remarks>
    public struct CameraTransform3D : IEquatable<CameraTransform3D>
    {
        #region constructors

        public static bool TryGetFromServiceProvider(Object obj, out CameraTransform3D cameraTransform)
        {
            if (obj is IServiceProvider provider)
            {
                if (provider.GetService(typeof(CameraTransform3D)) is CameraTransform3D ct3d) { cameraTransform = ct3d; return true; }
            }

            cameraTransform = default;
            return false;
        }
        
        public CameraTransform3D(Matrix4x4 position, float? fov, float? ortho, float? near, float? far)
        {
            nameof(position).GuardIsFinite(position);
            nameof(fov).GuardIsFiniteOrNull(fov);
            nameof(ortho).GuardIsFiniteOrNull(ortho);
            nameof(near).GuardIsFiniteOrNull(near);

            if (near.HasValue && far.HasValue && far.Value <= near.Value) throw new ArgumentException("far value must be higher than near", nameof(far));

            WorldMatrix = position;
            VerticalFieldOfView = fov;
            OrthographicScale = ortho;
            NearPlane = near;
            FarPlane = far;
        }

        #endregion

        #region constants

        public static CameraTransform3D Empty => default;

        public static CameraTransform3D Identity => new CameraTransform3D(Matrix4x4.Identity, null, null, -1f, 1f);

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

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (!IsInitialized) return 0;

            return WorldMatrix.GetHashCode()
                ^ VerticalFieldOfView.GetHashCode()
                ^ OrthographicScale.GetHashCode()
                ^ NearPlane.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is CameraTransform3D other && Equals(other); }

        /// <inheritdoc/>
        public bool Equals(CameraTransform3D other)
        {
            if (!this.IsInitialized && !other.IsInitialized) return true;            

            if (this.WorldMatrix != other.WorldMatrix) return false;

            if (this.VerticalFieldOfView != other.VerticalFieldOfView) return false;
            if (this.OrthographicScale != other.OrthographicScale) return false;

            if (this.NearPlane != other.NearPlane) return false;
            if (this.FarPlane != other.FarPlane) return false;

            return true;
        }

        /// <inheritdoc/>
        public static bool operator == (in CameraTransform3D a, in CameraTransform3D b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(in CameraTransform3D a, in CameraTransform3D b) => !a.Equals(b);

        #endregion

        #region properties

        public bool IsValid
        {
            get
            {
                if (!IsInitialized) return false;
                if (!WorldMatrix.IsFiniteAndNotZero()) return false;
                if (VerticalFieldOfView.HasValue)
                {
                    if (VerticalFieldOfView.Value <= 0.0f || VerticalFieldOfView.Value >= Math.PI) return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object has been initialized.
        /// </summary>
        public bool IsInitialized => VerticalFieldOfView.HasValue || OrthographicScale.HasValue;

        #endregion

        #region API

        public Matrix4x4 CreateProjectionMatrix(float aspectRatio, float? nearPlane = null, float? farPlane = null)
        {
            nameof(aspectRatio).GuardIsFinite(aspectRatio);
            nameof(nearPlane).GuardIsFiniteOrNull(nearPlane);
            nameof(farPlane).GuardIsFiniteOrNull(farPlane);            

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
            
            var scale = OrthographicScale ?? 1;
            return Matrix4x4.CreateOrthographic(scale, scale, near, far); // TODO: should scale be multiplied by aspect ratio?            
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
