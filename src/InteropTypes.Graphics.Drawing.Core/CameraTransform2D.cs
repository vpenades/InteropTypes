using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Camera transform in 2D space.
    /// </summary>       
    public struct CameraTransform2D : IEquatable<CameraTransform2D>
    {
        #region constructors

        public static bool TryGetFromServiceProvider(Object obj, out CameraTransform2D cameraTransform)
        {
            if (obj is IServiceProvider provider)
            {
                if (provider.GetService(typeof(CameraTransform2D)) is CameraTransform2D ct3d) { cameraTransform = ct3d; return true; }
            }

            cameraTransform = default;
            return false;
        }

        public CameraTransform2D(Matrix3x2 position, Point2? virtualSize)
        {
            nameof(position).GuardIsFinite(position);

            this.WorldMatrix = position;
            this.VirtualSize = virtualSize.HasValue ? virtualSize.Value.XY : (Vector2?)null;

            if (virtualSize.HasValue) this.KeepAspectRatio = true;
            else this.KeepAspectRatio = null;
        }

        public CameraTransform2D(Matrix3x2 position, Point2 virtualSize, bool keepAspectRatio)
        {
            nameof(position).GuardIsFinite(position);

            this.WorldMatrix = position;
            this.VirtualSize = virtualSize.XY;
            this.KeepAspectRatio = keepAspectRatio;
        }

        #endregion

        #region constants

        public static CameraTransform2D Empty => default;

        public static CameraTransform2D Identity => new CameraTransform2D(Matrix3x2.Identity, null);

        #endregion

        #region data

        /// <summary>
        /// Transform Matrix of the camera in World Space.
        /// </summary>
        /// <remarks>
        /// The camera looks into the negative Z
        /// </remarks>
        public Matrix3x2 WorldMatrix;

        /// <summary>
        /// Represents the world's scale size onscreen.
        /// </summary>
        public Vector2? VirtualSize;


        public bool? KeepAspectRatio;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            if (!IsInitialized) return 0;

            return WorldMatrix.GetHashCode() ^ VirtualSize.GetHashCode() ^ KeepAspectRatio.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) { return obj is CameraTransform2D other && Equals(other); }

        /// <inheritdoc/>
        public bool Equals(CameraTransform2D other)
        {
            if (!this.IsInitialized && !other.IsInitialized) return true;

            if (this.WorldMatrix != other.WorldMatrix) return false;
            if (this.VirtualSize != other.VirtualSize) return false;

            return true;
        }

        /// <inheritdoc/>
        public static bool operator ==(in CameraTransform2D a, in CameraTransform2D b) => a.Equals(b);

        /// <inheritdoc/>
        public static bool operator !=(in CameraTransform2D a, in CameraTransform2D b) => !a.Equals(b);

        #endregion

        #region properties

        public bool IsValid
        {
            get
            {
                if (!IsInitialized) return false;
                
                if (VirtualSize.HasValue)
                {
                    if (VirtualSize.Value.X == 0) return false;
                    if (VirtualSize.Value.Y == 0) return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object has been initialized.
        /// </summary>
        public bool IsInitialized => WorldMatrix.IsFiniteAndNotZero();

        #endregion

        #region API        

        public Matrix3x2 CreateFinalMatrix(Point2 physicalSize)
        {
            if (!WorldMatrix.IsFiniteAndNotZero()) throw new InvalidOperationException();

            Matrix3x2.Invert(WorldMatrix, out Matrix3x2 xform);

            if (VirtualSize.HasValue)
            {
                xform *= _CreateVirtualToPhysical(physicalSize.XY, this.VirtualSize.Value, this.KeepAspectRatio ?? false);
            }
            
            return xform;
        }

        public Matrix3x2 CreateViewportMatrix(Point2 physicalSize)
        {
            if (VirtualSize.HasValue)
            {
                return _CreateVirtualToPhysical(physicalSize.XY, this.VirtualSize.Value, this.KeepAspectRatio ?? false);
            }

            return Matrix3x2.Identity;
        }

        private static Matrix3x2 _CreateVirtualToPhysical(Vector2 physicalSize, Vector2 virtualSize, bool keepAspect)
        {
            var ws = physicalSize.X / Math.Abs(virtualSize.X);
            var hs = physicalSize.Y / Math.Abs(virtualSize.Y);

            if (keepAspect) ws = hs;
            var xform = Matrix3x2.CreateScale(ws, hs);

            if (virtualSize.X < 0) xform.M11 *= -1;
            if (virtualSize.Y < 0) xform.M22 *= -1;

            var offsx = (physicalSize.X - virtualSize.X * hs) * 0.5f;
            var offsy = (physicalSize.Y - virtualSize.Y * hs) * 0.5f;

            xform *= Matrix3x2.CreateTranslation(offsx, offsy);
            return xform;
        }

        #endregion

        #region nested types

        public interface ISource
        {
            CameraTransform2D GetCameraTransform2D();
        }

        #endregion
    }
}
