using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Represents a Camera transform in 2D space.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("")]
    public struct CameraTransform2D
        : IEquatable<CameraTransform2D>
    {
        #region constructors

        public static bool TryGetFromServiceProvider(Object obj, out CameraTransform2D cameraTransform)
        {
            if (obj is ISource source)
            {
                cameraTransform = source.GetCameraTransform2D();
                return true;
            }

            if (obj is IServiceProvider provider)
            {
                if (provider.GetService(typeof(CameraTransform2D)) is CameraTransform2D ct2d) { cameraTransform = ct2d; return true; }
                if (provider.GetService(typeof(ISource)) is ISource ct2ds) { cameraTransform = ct2ds.GetCameraTransform2D(); return true; }
            }

            cameraTransform = default;
            return false;
        }

        /// <summary>
        /// Tries to get the physical render target size and returns it converted to virtual space.
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static bool TryGetOuterCamera(ICoreCanvas2D dc, out CameraTransform2D camera)
        {
            camera = default;

            if (!TryGetRenderTargetInfo(dc, out var vinfo)) return false;

            if (vinfo == null) return false;

            var w = vinfo.PixelsWidth;
            var h = vinfo.PixelsHeight;
            if (w <= 0 || h <= 0) return false;

            // query for any in between transformations
            if (dc is ITransformer2D xform)
            {
                // transform points from physical screen space to virtual space
                Span<Point2> points = stackalloc Point2[3];
                points[0] = (0, 0);
                points[1] = (w, 0);
                points[2] = (0, h);
                xform.TransformInverse(points); 

                // create matrix from points            
                var dx = Point2.Normalize(points[1] - points[0], out var ww);
                var dy = Point2.Normalize(points[2] - points[0], out var hh);
                var m = new Matrix3x2(dx.X, dx.Y, dy.X, dy.Y, points[0].X, points[0].Y);

                // create camera
                camera = new CameraTransform2D(m, new Vector2(ww, hh), false);                
            }
            else
            {
                camera = Create(Matrix3x2.Identity, new Vector2(w, h));                
            }

            return true;


        }

        public static CameraTransform2D Create(Matrix3x2 worldMatrix)
        {
            return new CameraTransform2D(worldMatrix, null, null);
        }

        public static CameraTransform2D Create(Matrix3x2 worldMatrix, Point2 virtualSize) 
        {
            return new CameraTransform2D(worldMatrix, virtualSize.XY, true);
        }

        public static CameraTransform2D Create(Matrix3x2 worldMatrix, Point2 virtualSize, bool keepAspectRatio)
        {
            return new CameraTransform2D(worldMatrix, virtualSize.XY, keepAspectRatio);
        }

        private CameraTransform2D(Matrix3x2 worldMatrix, Vector2? virtualSize, bool? keepAspectRatio)
        {
            nameof(worldMatrix).GuardIsFinite(worldMatrix);

            this.WorldMatrix = worldMatrix;
            this.VirtualSize = virtualSize;
            this.KeepAspectRatio = keepAspectRatio;
        }

        #endregion

        #region constants

        public static CameraTransform2D Empty => default;

        public static CameraTransform2D Identity => new CameraTransform2D(Matrix3x2.Identity, null, null);

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
        /// <remarks>
        /// X and Y values cannot be zero, but they can be negative to reverse the axis.
        /// </remarks>
        public Vector2? VirtualSize;

        /// <summary>
        /// true to keep aspect ratio
        /// </summary>
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
        
        public bool TryCreateFinalMatrix(ICoreCanvas2D dc, out Matrix3x2 finalMatrix)
        {
            finalMatrix = Matrix3x2.Identity;

            if (!TryGetRenderTargetInfo(dc, out var vinfo)) return false;
            if (vinfo == null) return false;

            int w = vinfo.PixelsWidth;
            int h = vinfo.PixelsHeight;
            if (w <= 0 || h <= 0) return false;

            finalMatrix = CreateFinalMatrix((vinfo.PixelsWidth, vinfo.PixelsHeight));
            return true;
        }        

        public static bool TryGetRenderTargetInfo(ICoreCanvas2D dc, out IRenderTargetInfo rtinfo)
        {
            rtinfo = null;

            // if this is a render target, return direct values.
            if (dc is IRenderTargetInfo vinfo0)
            {
                rtinfo = vinfo0;
                return true;
            }

            // query for the IRenderTargetInfo down the chain.
            if (!(dc is IServiceProvider srv)) return false;

            if (srv.GetService(typeof(IRenderTargetInfo)) is IRenderTargetInfo vinfo)
            {
                rtinfo = vinfo;
                return true;
            }

            return false;
        }

        public static CameraTransform2D Multiply(CameraTransform2D camera, in Matrix3x2 xform)
        {
            camera.WorldMatrix = camera.WorldMatrix * xform;
            return camera;
        }

        public static CameraTransform2D Multiply(in Matrix3x2 xform, CameraTransform2D camera)
        {
            camera.WorldMatrix = xform * camera.WorldMatrix;
            return camera;
        }

        public System.Drawing.RectangleF GetOuterBoundingRect()
        {
            var vs = VirtualSize ?? Vector2.One;

            Span<Vector2> points = stackalloc Vector2[4];
            points[0] = Vector2.Transform(Vector2.Zero, WorldMatrix);
            points[1] = Vector2.Transform(new Vector2(vs.X,0), WorldMatrix);
            points[2] = Vector2.Transform(new Vector2(vs.X, vs.Y), WorldMatrix);
            points[3] = Vector2.Transform(new Vector2(0, vs.Y), WorldMatrix);

            bool first = true;

            System.Drawing.RectangleF bounds = default;

            foreach (var p in points)
            {
                var other = new System.Drawing.RectangleF(p.X, p.Y, 0, 0);
                bounds = first ? other : System.Drawing.RectangleF.Union(bounds, other);
                first = false;
            }

            return bounds;
        }

        public Matrix3x2 CreateFinalMatrix(Point2 physicalSize)
        {
            return CreateViewMatrix() * CreateViewportMatrix(physicalSize);
        }        

        /// <summary>
        /// Gets the inverse of <see cref="WorldMatrix"/>
        /// </summary>
        /// <returns>A matrix.</returns>        
        public Matrix3x2 CreateViewMatrix()
        {
            if (!WorldMatrix.IsFiniteAndNotZero()) throw new InvalidOperationException($"Invalid {nameof(WorldMatrix)}");

            return !Matrix3x2.Invert(WorldMatrix, out Matrix3x2 viewMatrix)
                ? Matrix3x2.Identity
                : viewMatrix;
        }

        public Matrix3x2 CreateViewportMatrix(Point2 physicalSize)
        {            
            return _CreateVirtualToPhysical(physicalSize.XY, this.VirtualSize ?? physicalSize.XY, this.KeepAspectRatio ?? false);
        }

        private static Matrix3x2 _CreateVirtualToPhysical(Vector2 physicalSize, Vector2 virtualSize, bool keepAspect)
        {
            if (virtualSize.X == 0 || virtualSize.Y == 0) throw new ArgumentException("Must not be zero", nameof(virtualSize));
            if (physicalSize.X <= 0 || physicalSize.Y <= 0) throw new ArgumentException("Must be positive", nameof(virtualSize));

            var ws = physicalSize.X / Math.Abs(virtualSize.X);
            var hs = physicalSize.Y / Math.Abs(virtualSize.Y);

            if (keepAspect) ws = hs;
            var xform = Matrix3x2.CreateScale(ws, hs);

            if (virtualSize.X < 0) xform.M11 *= -1;
            if (virtualSize.Y < 0) xform.M22 *= -1;

            var offsx = (physicalSize.X - virtualSize.X * hs) * 0.5f;
            var offsy = (physicalSize.Y - virtualSize.Y * hs) * 0.5f;

            return xform * Matrix3x2.CreateTranslation(offsx, offsy);
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
