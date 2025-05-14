using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using BBOX = System.Numerics.Matrix3x2;

namespace InteropTypes.Graphics.Drawing
{
    /// <summary>
    /// Defines a Camera in 3D space, that can be used to create Camera and Projection matrices.
    /// </summary>
    public class SceneView3D : IPseudoImmutable, CameraTransform3D.ISource
    {
        #region lifecycle

        public static implicit operator SceneView3D(Matrix4x4 xform)
        {
            if (!Matrix4x4.Invert(xform, out Matrix4x4 _)) throw new ArgumentException("Invalid matrix", nameof(xform));

            return new SceneView3D { _CameraMatrix = xform };
        }

        public SceneView3D WithSceneBounds((Point3 Min, Point3 Max) bounds)
        {
            SetSceneBounds(bounds);
            return this;
        }

        public SceneView3D WithSceneBounds(in BBOX bounds)
        {
            SetSceneBounds(bounds);
            return this;
        }

        #endregion

        #region data

        private Matrix4x4? _CameraMatrix;
        private Vector3? _CameraPosition;

        private (Point3 Min, Point3 Max)? _SceneBounds;

        // When Positive: Perspective Vertical FOV in radians
        // When Negative: Ortographic Vertical Scale
        private float _ProjectionFactor = 1.2f;

        private object _ImmutableKey;

        #endregion

        #region properties

        public object ImmutableKey
        {
            get
            {
                if (_ImmutableKey == null) _ImmutableKey = new object();
                return _ImmutableKey;
            }
        }

        public Matrix4x4? CameraMatrix
        {
            get => _CameraMatrix;
            set
            {
                if (!value.HasValue)
                {
                    _CameraMatrix = null;
                    _ImmutableKey = null;
                    return;
                }

                if (!Matrix4x4.Invert(value.Value, out Matrix4x4 _)) throw new ArgumentException(nameof(value));

                _CameraPosition = null;
                _CameraMatrix = value;
                _ImmutableKey = null;
            }
        }

        #endregion

        #region API - Setters

        public void Clear()
        {
            _CameraPosition = null;
            _CameraMatrix = null;
            _SceneBounds = null;
            _ProjectionFactor = 1.2f;
            _ImmutableKey = null;
        }

        public void Invalidate() { _ImmutableKey = null; }

        public void SetCameraLookingAtCenter(Vector3 cameraPosition)
        {
            _CameraPosition = cameraPosition;
            _CameraMatrix = null;
            _ImmutableKey = null;
        }

        public void SetPerspectiveFOV(float fov)
        {
            if (fov <= 0 || fov > MathF.PI) throw new ArgumentOutOfRangeException(nameof(fov));

            _ProjectionFactor = fov;
            _ImmutableKey = null;
        }

        public void SetOrthographicScale(float scale)
        {
            if (scale <= 0) throw new ArgumentOutOfRangeException(nameof(scale));
            _ProjectionFactor = -scale;
            _ImmutableKey = null;
        }

        public void SetSceneBounds((Point3 Min, Point3 Max) bounds)
        {
            _SceneBounds = bounds;
            _ImmutableKey = null;
        }

        public void SetSceneBounds(BBOX bounds)
        {
            _SceneBounds = (bounds.ColumnX(), bounds.ColumnY());
            _ImmutableKey = null;
        }

        #endregion

        #region API - Evaluators

        public CameraTransform3D GetCameraTransform3D()
        {
            var cam = _ProjectionFactor > 0
                ? CameraTransform3D.CreatePerspective(_ProjectionFactor)
                : CameraTransform3D.CreateOrthographic(-_ProjectionFactor);

            return cam.WithPlanes(0.1f, 10000f).WithWorldMatrix(GetCameraMatrix());
        }        

        public Matrix4x4 GetProjectionMatrix(float width, float height)
        {
            var ar = width / height;

            return _ProjectionFactor >= 0
               ?
               Matrix4x4.CreatePerspectiveFieldOfView(_ProjectionFactor, ar, 0.1f, 10000)
               :
               Matrix4x4.CreateOrthographic(-_ProjectionFactor * ar, -_ProjectionFactor, 0.1f, 10000);
        }

        public Matrix4x4 GetCameraMatrix()
        {
            if (_CameraMatrix.HasValue) return _CameraMatrix.Value;

            if (_CameraPosition.HasValue) return _GetLookingCamera(_CameraPosition.Value);

            if (_SceneBounds.HasValue) return _GetOptimalCamera(_SceneBounds.Value.Min, _SceneBounds.Value.Max);

            return Matrix4x4.Identity;
        }

        private Matrix4x4 _GetLookingCamera(Point3 campos)
        {
            var sceneCenter = _SceneBounds.HasValue ? (_SceneBounds.Value.Max - _SceneBounds.Value.Min) * 0.5f : Vector3.Zero;
            return Matrix4x4.CreateWorld(campos.XYZ, Vector3.Normalize(sceneCenter - campos), Vector3.UnitY);
        }

        private static Matrix4x4 _GetOptimalCamera(Point3 min, Point3 max)
        {
            var center = (min + max) * 0.5f;
            var size = max - min;

            var distance = (size.X + size.Y + size.Z) / 2.1f;

            var rx = Math.Max(0.01f, size.X);
            var ry = Math.Max(0.01f, size.Y);
            var rz = Math.Max(0.01f, size.Z);

            rx /= MathF.Pow(rx, 2f);
            ry /= MathF.Pow(ry, 2.5f);
            rz /= MathF.Pow(rz, 2f);

            var forward = -Vector3.Normalize(new Vector3(rx, ry, rz));

            return Matrix4x4.CreateWorld((center - forward * distance), forward, Vector3.UnitY);
        }        

        #endregion
    }
}
