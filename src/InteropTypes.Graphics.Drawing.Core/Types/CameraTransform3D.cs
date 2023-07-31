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
    /// To construct the transform pipeline, the viewport's dimensions and aspect ration are usually required,
    /// which are usually available only at the beginning of the rendering stage. This structure contains
    /// everything's required to build the final pipeline, except the viewport's data.
    /// </para>    
    /// <para>
    /// <see href="https://en.wikipedia.org/wiki/Orthographic_projection#/media/File:Graphical_projection_comparison.png">Graphical projection comparison</see>
    /// </para>
    /// </remarks>
    public struct CameraTransform3D
        : IEquatable<CameraTransform3D>
    {
        #region constructors

        public static bool TryGetFromServiceProvider(Object obj, out CameraTransform3D cameraTransform)
        {
            if (obj is ISource source)
            {
                cameraTransform = source.GetCameraTransform3D();
                return true;
            }

            if (obj is IServiceProvider provider)
            {
                if (provider.GetService(typeof(CameraTransform3D)) is CameraTransform3D ct3d) { cameraTransform = ct3d; return true; }
                if (provider.GetService(typeof(ISource)) is ISource ct3ds) { cameraTransform = ct3ds.GetCameraTransform3D(); return true; }
            }

            cameraTransform = default;
            return false;
        }

        public static CameraTransform3D CreatePerspective(float fov)
        {
            return new CameraTransform3D(fov, null, null, null);
        }

        public static CameraTransform3D CreateOrthographic(float scale)
        {
            return new CameraTransform3D(null, scale, null, null);
        }

        private CameraTransform3D(float? fov, float? ortho, float? near, float? far)
        {            
            nameof(fov).GuardIsFiniteOrNull(fov);
            nameof(ortho).GuardIsFiniteOrNull(ortho);
            nameof(near).GuardIsFiniteOrNull(near);

            if (fov.HasValue && ortho.HasValue) throw new ArgumentException("FOV and Ortho are defined. Only one of the two must be defined", nameof(ortho));

            if (near.HasValue && far.HasValue && far.Value <= near.Value) throw new ArgumentException("far value must be higher than near", nameof(far));

            WorldMatrix = Matrix4x4.Identity;
            AxisMatrix = Matrix4x4.Identity;
            _VerticalFieldOfView = fov;
            _OrthographicScale = ortho;
            _NearPlane = near;
            _FarPlane = far;
        }

        #endregion

        #region constants

        public static CameraTransform3D Empty => default;

        public static CameraTransform3D Identity => new CameraTransform3D(null, null, -1f, 1f);

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
        private float? _VerticalFieldOfView;

        /// <summary>
        /// if defined, the camera is ortographic camera.
        /// </summary>
        private float? _OrthographicScale;

        /// <summary>
        /// Near plane must be more than zero and less than <see cref="_FarPlane"/>
        /// </summary>
        private float? _NearPlane;

        /// <summary>
        /// Far plane must be more than <see cref="_NearPlane"/> or infinite.
        /// </summary>
        private float? _FarPlane;

        /// <inheritdoc/>
        public readonly override int GetHashCode()
        {
            if (!IsInitialized) return 0;

            return WorldMatrix.GetHashCode()
                ^ AxisMatrix.GetHashCode()
                ^ _VerticalFieldOfView.GetHashCode()
                ^ _OrthographicScale.GetHashCode()
                ^ _NearPlane.GetHashCode();
        }

        /// <inheritdoc/>
        public readonly override bool Equals(object obj) { return obj is CameraTransform3D other && Equals(other); }

        /// <inheritdoc/>
        public readonly bool Equals(CameraTransform3D other)
        {
            if (this.WorldMatrix != other.WorldMatrix) return false;
            if (this.AxisMatrix != other.AxisMatrix) return false;

            if (this._VerticalFieldOfView != other._VerticalFieldOfView) return false;
            if (this._OrthographicScale != other._OrthographicScale) return false;

            if (this._NearPlane != other._NearPlane) return false;
            if (this._FarPlane != other._FarPlane) return false;

            return true;
        }

        /// <inheritdoc/>
        public static bool operator == (in CameraTransform3D a, in CameraTransform3D b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(in CameraTransform3D a, in CameraTransform3D b) => !a.Equals(b);

        #endregion

        #region properties

        public readonly bool IsValid
        {
            get
            {
                if (!IsInitialized) return false;
                if (!WorldMatrix.IsFiniteAndNotZero()) return false;
                if (!AxisMatrix.IsFiniteAndNotZero()) return false;
                if (_VerticalFieldOfView.HasValue)
                {
                    if (_VerticalFieldOfView.Value <= 0.0f || _VerticalFieldOfView.Value >= Math.PI) return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object has been initialized.
        /// </summary>
        public readonly bool IsInitialized => _VerticalFieldOfView.HasValue || _OrthographicScale.HasValue;

        #endregion

        #region  fluent API

        public readonly CameraTransform3D WithPlanes(float nearPlane, float farPlane)
        {
            var clone = this;
            clone._NearPlane = nearPlane;
            clone._FarPlane = farPlane;
            return clone;
        }        

        public readonly CameraTransform3D WithAxisMatrix(in Matrix4x4 axisMatrix)
        {
            var clone = this;
            clone.AxisMatrix = axisMatrix;
            return clone;
        }

        public readonly CameraTransform3D WithWorldMatrix(in Matrix4x4 worldMatrix)
        {
            var clone = this;
            clone.WorldMatrix = worldMatrix;
            return clone;
        }

        #endregion

        #region API

        public static CameraTransform3D Multiply(CameraTransform3D camera, in Matrix4x4 xform)
        {
            camera.WorldMatrix = camera.WorldMatrix * xform;
            return camera;
        }

        public static CameraTransform3D Multiply(in Matrix4x4 xform, CameraTransform3D camera)
        {
            camera.WorldMatrix = xform * camera.WorldMatrix;
            return camera;
        }

        #pragma warning disable CA1822 // Mark members as static

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

        /// <summary>
        /// If this represents a perspective camera, it gets the vertical field of view.
        /// </summary>
        /// <param name="verticalFOV">The Vertical Field of View.</param>
        /// <returns>true if this is a perspective camera.</returns>
        public readonly bool TryGetPerspectiveFieldOfView(out float verticalFOV)
        {
            if (_VerticalFieldOfView.HasValue) { verticalFOV = _VerticalFieldOfView.Value; return true; }
            verticalFOV = 0f; return false;
        }

        /// <summary>
        /// If this represents an orthographic camera, it gets the orthographic scale.
        /// </summary>
        /// <param name="scale">The orthographic scale.</param>
        /// <returns>true if this is an orthographic camera.</returns>
        public readonly bool TryGetOrthographicScale(out float scale)
        {
            if (_VerticalFieldOfView.HasValue) { scale = _OrthographicScale.Value; return true; }
            scale = 0f; return false;
        }

        /// <summary>
        /// Gets the actual camera matrix, which is the concatenation of <see cref="AxisMatrix"/> and <see cref="WorldMatrix"/>.
        /// </summary>
        /// <returns>A matrix.</returns>
        public readonly Matrix4x4 CreateCameraMatrix()
        {
            if (!WorldMatrix.IsFiniteAndNotZero()) throw new InvalidOperationException($"Invalid {nameof(WorldMatrix)}");

            return AxisMatrix * WorldMatrix;
        }

        /// <summary>
        /// Gets the inverse of <see cref="CreateCameraMatrix"/>
        /// </summary>
        /// <returns>A matrix.</returns>        
        public readonly Matrix4x4 CreateViewMatrix()
        {
            return !Matrix4x4.Invert(CreateCameraMatrix(), out Matrix4x4 viewMatrix)
                ? Matrix4x4.Identity
                : viewMatrix;
        }

        public readonly Matrix4x4 CreateProjectionMatrix(Point2 screenSize)
        {
            return CreateProjectionMatrix(screenSize.X / screenSize.Y);
        }

        public readonly Matrix4x4 CreateProjectionMatrix(float aspectRatio)
        {
            nameof(aspectRatio).GuardIsFinite(aspectRatio);                      

            float near = _NearPlane ?? 0.1f;
            float far = _FarPlane ?? 1000f;

            if (_VerticalFieldOfView.HasValue)
            {                
                return Matrix4x4.CreatePerspectiveFieldOfView(_VerticalFieldOfView.Value, aspectRatio, near, far);
            }
            
            var scale = _OrthographicScale ?? 1;
            return Matrix4x4.CreateOrthographic(scale, scale, near, far); // TODO: should scale be multiplied by aspect ratio?            
        }

        public readonly Matrix3x2 CreateViewportMatrix(Point2 screenSize)
        {
            return CreateViewportMatrix(screenSize.X, screenSize.Y);
        }
        
        public readonly Matrix3x2 CreateViewportMatrix(float width, float height)
        
        {
            return new Matrix3x2
                (0.5f * width, 0
                , 0, -0.5f * height
                , 0.5f * width, 0.5f * height);
        }

        #if NETSTANDARD2_0
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

        #pragma warning restore CA1822 // Mark members as static

        #endif

        #endregion

        #region nested types

        public interface ISource
        {
            CameraTransform3D GetCameraTransform3D();
        }

        #endregion

        #region helpers

        public readonly void DrawCameraTo(IScene3D dc, float cameraSize)
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

        public readonly void DrawFustrumTo(IScene3D dc, Single lineDiameter, ColorStyle brush)
        {
            DrawFustrumTo((dc, 1), lineDiameter, brush);
        }

        public readonly void DrawFustrumTo((IScene3D Context, float AspectRatio) target, Single lineDiameter, ColorStyle brush)
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
