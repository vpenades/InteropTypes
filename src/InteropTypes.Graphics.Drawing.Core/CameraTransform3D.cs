using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using COLOR = System.Drawing.Color;

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
    /// - A <see cref="CreateProjectionMatrix(float)"/> method to retrieve the projection matrix.
    /// </para>
    /// <para>
    /// <see href="https://en.wikipedia.org/wiki/Orthographic_projection#/media/File:Graphical_projection_comparison.png">Graphical projection comparison</see>
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

        public CameraTransform3D(float? fov, float? ortho, float? near, float? far)
        {            
            nameof(fov).GuardIsFiniteOrNull(fov);
            nameof(ortho).GuardIsFiniteOrNull(ortho);
            nameof(near).GuardIsFiniteOrNull(near);

            if (fov.HasValue && ortho.HasValue) throw new ArgumentException("FOV and Ortho are defined. Only one of the two must be defined", nameof(ortho));

            if (near.HasValue && far.HasValue && far.Value <= near.Value) throw new ArgumentException("far value must be higher than near", nameof(far));

            WorldMatrix = Matrix4x4.Identity;
            AxisMatrix = Matrix4x4.Identity;
            VerticalFieldOfView = fov;
            OrthographicScale = ortho;
            NearPlane = near;
            FarPlane = far;
        }

        public CameraTransform3D(Matrix4x4 position, float? fov, float? ortho, float? near, float? far)
        {
            nameof(position).GuardIsFinite(position);
            nameof(fov).GuardIsFiniteOrNull(fov);
            nameof(ortho).GuardIsFiniteOrNull(ortho);
            nameof(near).GuardIsFiniteOrNull(near);

            if (fov.HasValue && ortho.HasValue) throw new ArgumentException("FOV and Ortho are defined. Only one of the two must be defined", nameof(ortho));

            if (near.HasValue && far.HasValue && far.Value <= near.Value) throw new ArgumentException("far value must be higher than near", nameof(far));
            
            WorldMatrix = position;
            AxisMatrix = Matrix4x4.Identity;
            VerticalFieldOfView = fov;
            OrthographicScale = ortho;
            NearPlane = near;
            FarPlane = far;
        }

        #endregion

        #region constants

        public static CameraTransform3D Empty => default;

        public static CameraTransform3D Identity => new CameraTransform3D(Matrix4x4.Identity, null, null, -1f, 1f);

        /// <summary>
        /// Use this matrix on <see cref="AxisMatrix"/> to set up a Z up camera.
        /// </summary>
        public static Matrix4x4 ZUpAxisMatrix => new Matrix4x4(1, 0, 0, 0, 0, 0, 1, 0, 0, -1, 0, 0, 0, 0, 0, 1);

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
        /// Transform matrix that determines the direction of the Up Axis
        /// </summary>
        /// <remarks>
        /// This matrix is used to calculate the ViewMatrix in <see cref="CreateViewMatrix"/>
        /// </remarks>
        public Matrix4x4 AxisMatrix;

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
                ^ AxisMatrix.GetHashCode()
                ^ VerticalFieldOfView.GetHashCode()
                ^ OrthographicScale.GetHashCode()
                ^ NearPlane.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is CameraTransform3D other && Equals(other); }

        /// <inheritdoc/>
        public bool Equals(CameraTransform3D other)
        {
            if (this.WorldMatrix != other.WorldMatrix) return false;
            if (this.AxisMatrix != other.AxisMatrix) return false;

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
                if (!AxisMatrix.IsFiniteAndNotZero()) return false;
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

        /// <summary>
        /// Sets <see cref="WorldMatrix"/> so it points at <paramref name="target"/>
        /// </summary>
        /// <param name="target">The target to look at.</param>
        /// <param name="distanceToTarget">The distance from the target.</param>
        /// <param name="yaw">The camera's yaw angle, in radians.</param>
        /// <param name="pitch">The camera's pitch angle, in radians.</param>
        /// <param name="roll">The camera's roll angle, in radians.</param>
        /// <remarks>
        /// This call takes <see cref="AxisMatrix"/> into account, so it must be set
        /// before calling this method.
        /// </remarks>
        public void SetOrbitWorldMatrix(Point3 target, float distanceToTarget, float yaw, float pitch, float roll)
        {
            var axisPitch = new Vector3(AxisMatrix.M11, AxisMatrix.M12, AxisMatrix.M13);
            var axisYaw = new Vector3(AxisMatrix.M21, AxisMatrix.M22, AxisMatrix.M23);
            var axisForward = new Vector3(AxisMatrix.M31, AxisMatrix.M32, AxisMatrix.M33);            

            WorldMatrix = Matrix4x4.CreateTranslation(axisForward * distanceToTarget)
                * Matrix4x4.CreateFromAxisAngle(axisForward, roll)
                * Matrix4x4.CreateFromAxisAngle(axisPitch, pitch)
                * Matrix4x4.CreateFromAxisAngle(axisYaw, yaw)
                * Matrix4x4.CreateTranslation(target.XYZ);
        }

        public Matrix4x4 CreateCameraMatrix()
        {
            return AxisMatrix * WorldMatrix;
        }

        public Matrix4x4 CreateViewMatrix()
        {
            return !Matrix4x4.Invert(CreateCameraMatrix(), out Matrix4x4 viewMatrix)
                ? throw new InvalidOperationException("Can't invert")
                : viewMatrix;
        }

        public Matrix4x4 CreateProjectionMatrix(Point2 screenSize)
        {
            return CreateProjectionMatrix(screenSize.X / screenSize.Y);
        }

        public Matrix4x4 CreateProjectionMatrix(float aspectRatio)
        {
            nameof(aspectRatio).GuardIsFinite(aspectRatio);                      

            float near = NearPlane ?? 0.1f;
            float far = FarPlane ?? 1000f;

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

        public Matrix3x2 CreateViewportMatrix(Point2 screenSize)
        {
            return CreateViewportMatrix(screenSize.X, screenSize.Y);
        }

        #pragma warning disable CA1822 // Mark members as static
        public Matrix3x2 CreateViewportMatrix(float width, float height)
        #pragma warning restore CA1822 // Mark members as static
        {
            return new Matrix3x2
                (0.5f * width, 0
                , 0, -0.5f * height
                , 0.5f * width, 0.5f * height);
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

        #region nested types

        public interface ISource
        {
            CameraTransform3D GetCameraTransform3D();
        }

        #endregion

        #region helpers

        public void DrawCameraTo(IScene3D dc, float cameraSize)
        {
            if (dc == null) throw new ArgumentNullException(nameof(dc));

            if (!this.IsValid) return;

            // draw the camera object

            var pivot = this.CreateCameraMatrix();

            var style = (COLOR.Gray, COLOR.Black, cameraSize * 0.05f);
            var center = Vector3.Transform(Vector3.Zero, pivot);
            var back = Vector3.Transform(Vector3.UnitZ * cameraSize, pivot);
            var roll1 = Vector3.Transform(new Vector3(0, 0.7f, 0.3f) * cameraSize, pivot);
            var roll2 = Vector3.Transform(new Vector3(0, 0.7f, 0.9f) * cameraSize, pivot);

            dc.DrawSphere(center, cameraSize * 0.35f, COLOR.Black); // lens

            dc.DrawSegments(Point3.Array(Vector3.Lerp(back, center, 0.7f), center), cameraSize * 0.5f, style); // objective
            dc.DrawSegments(Point3.Array(back, Vector3.Lerp(back, center, 0.7f)), cameraSize * 0.8f, style); // body
            dc.DrawSphere(roll1, 0.45f * cameraSize, style);
            dc.DrawSphere(roll2, 0.45f * cameraSize, style);            

            // draw the actual axes

            pivot = this.WorldMatrix;

            var xxx = Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitX, pivot)) * cameraSize;
            var yyy = Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitY, pivot)) * cameraSize;
            var zzz = Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitZ, pivot)) * cameraSize;

            var colorX = COLOR.Red;
            var colorY = COLOR.Green;
            var colorZ = COLOR.Blue;

            dc.DrawSegments(Point3.Array(center + xxx * 0.5f, center + xxx * 1.5f), cameraSize * 0.1f, (colorX, LineCapStyle.Round, LineCapStyle.Triangle));
            dc.DrawSegments(Point3.Array(center + yyy * 1.0f, center + yyy * 2.0f), cameraSize * 0.1f, (colorY, LineCapStyle.Round, LineCapStyle.Triangle));
            dc.DrawSegments(Point3.Array(center + zzz * 1.2f, center + zzz * 2.2f), cameraSize * 0.1f, (colorZ, LineCapStyle.Round, LineCapStyle.Triangle));
        }

        public void DrawFustrumTo(IScene3D dc, Single lineDiameter, ColorStyle brush)
        {
            DrawFustrumTo((dc, 1), lineDiameter, brush);
        }

        public void DrawFustrumTo((IScene3D Context, float AspectRatio) target, Single lineDiameter, ColorStyle brush)
        {
            Matrix4x4.Invert(CreateProjectionMatrix(target.AspectRatio), out Matrix4x4 ip);

            var dc = target.Context;

            var cam = CreateCameraMatrix();

            for (int i = 0; i < 2; ++i)
            {
                var z = (float)i *  10f;

                var a = new Vector3(-1, -1, -z);
                var b = new Vector3(+1, -1, -z);
                var c = new Vector3(+1, +1, -z);
                var d = new Vector3(-1, +1, -z);

                // to camera space

                var aw = Vector4.Transform(a, ip);
                var bw = Vector4.Transform(b, ip);
                var cw = Vector4.Transform(c, ip);
                var dw = Vector4.Transform(d, ip);

                aw *= aw.W;
                bw *= bw.W;
                cw *= cw.W;
                dw *= dw.W;

                a = new Vector3(aw.X, aw.Y, aw.Z);
                b = new Vector3(bw.X, bw.Y, bw.Z);
                c = new Vector3(cw.X, cw.Y, cw.Z);
                d = new Vector3(dw.X, dw.Y, dw.Z);

                // to world space                

                a = Vector3.Transform(a, cam);
                b = Vector3.Transform(b, cam);
                c = Vector3.Transform(c, cam);
                d = Vector3.Transform(d, cam);

                dc.DrawSegments(Point3.Array(a, b), lineDiameter, brush);
                dc.DrawSegments(Point3.Array(b, c), lineDiameter, brush);
                dc.DrawSegments(Point3.Array(c, d), lineDiameter, brush);
                dc.DrawSegments(Point3.Array(d, a), lineDiameter, brush);
            }
        }

        #endregion
    }
}
