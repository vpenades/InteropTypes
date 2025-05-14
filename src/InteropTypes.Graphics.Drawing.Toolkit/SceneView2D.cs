using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    public class SceneView2D : IPseudoImmutable, CameraTransform2D.ISource
    {
        #region lifecycle

        public static implicit operator SceneView2D(Matrix3x2 xform)
        {
            if (!Matrix3x2.Invert(xform, out Matrix3x2 _)) throw new ArgumentException(nameof(xform));

            return new SceneView2D { _CameraMatrix = xform };
        }

        public SceneView2D WithSceneBounds((Point2 Min, Point2 Max) bounds)
        {
            SetSceneBounds(bounds);
            return this;
        }

        public SceneView2D WithSceneBounds(System.Drawing.RectangleF bounds)
        {
            SetSceneBounds(bounds);
            return this;
        }

        #endregion

        #region data

        private Matrix3x2? _CameraMatrix;

        private (Point2 Min, Point2 Max)? _SceneBounds;

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

        public Matrix3x2? CameraMatrix
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

                if (!Matrix3x2.Invert(value.Value, out Matrix3x2 _)) throw new ArgumentException("Invalid Matrix", nameof(value));

                _CameraMatrix = value;
                _ImmutableKey = null;
            }
        }

        #endregion

        #region API - Setters

        public void Clear()
        {
            _CameraMatrix = null;

            _SceneBounds = null;
            _ImmutableKey = null;
        }

        public void Invalidate() { _ImmutableKey = null; }

        public void SetSceneBounds((Point2 Min, Point2 Max) bounds)
        {
            _SceneBounds = bounds;
            _ImmutableKey = null;
        }

        public void SetSceneBounds(System.Drawing.RectangleF bounds)
        {
            var min = bounds.Location.ToVector2();
            var max = min + bounds.Size.ToVector2();

            _SceneBounds = (min, max);
            _ImmutableKey = null;
        }

        #endregion

        #region API - Evaluators

        public virtual CameraTransform2D GetCameraTransform2D()
        {
            if (_SceneBounds.HasValue)
            {
                var ss = _SceneBounds.Value.Max - _SceneBounds.Value.Min;
                return CameraTransform2D.Create(_CameraMatrix ?? Matrix3x2.Identity, ss);
            }

            return CameraTransform2D.Create(_CameraMatrix ?? Matrix3x2.Identity);
        }

        public virtual (Matrix3x2 Camera, Matrix3x2 Projection) GetMatrices(float renderWidth, float renderHeight)
        {
            var cam = GetCameraMatrix();
            var prj = GetProjectionMatrix(renderWidth, renderHeight);

            return (cam, prj);
        }

        public virtual Matrix3x2 GetProjectionMatrix(float renderWidth, float renderHeight)
        {
            var ar = renderWidth / renderHeight;

            if (_SceneBounds.HasValue)
            {
                var ss = _SceneBounds.Value.Max - _SceneBounds.Value.Min;
                var s = Math.Max(ss.X, ss.Y);

                return (s * ar, s).CreateOrthographic2D();
            }

            return (ar, 1.0f).CreateOrthographic2D();
        }

        public virtual Matrix3x2 GetCameraMatrix()
        {
            if (_CameraMatrix.HasValue) return _CameraMatrix.Value;

            if (_SceneBounds.HasValue) return GetOptimalCamera(_SceneBounds.Value.Min, _SceneBounds.Value.Max);

            return Matrix3x2.Identity;
        }

        protected static Matrix3x2 GetOptimalCamera(Point2 min, Point2 max)
        {
            // camera located at the center of the scene.

            var center = (min + max) * 0.5f;
            return Matrix3x2.CreateTranslation(center);
        }

        #endregion        
    }
}
