using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropVision
{
    public readonly struct ImageRegion
    {
        #region factories

        public static implicit operator ImageRegion(System.Numerics.Matrix3x2 roi)
        {
            return new ImageRegion(roi);
        }

        public static implicit operator ImageRegion(System.Drawing.Rectangle roi)
        {
            return new ImageRegion(roi);
        }

        public static implicit operator ImageRegion(System.Drawing.RectangleF roi)
        {
            return new ImageRegion(roi);
        }

        public static ImageRegion FromCenterTop(Vector2 center, Vector2 top)
        {
            var upVector = top - center;

            // https://github.com/google/mediapipe/blob/master/mediapipe/modules/pose_landmark/pose_detection_to_roi.pbtxt#L41
            var radius = upVector.Length() * 1.5f;

            var S = new Vector2(radius * 2, radius * 2);
            var R = (float)Math.Atan2(upVector.X, -upVector.Y);
            var T = new Vector2(center.X, center.Y);

            return FromOrientedRect(T, S, R);
        }

        public static ImageRegion FromOrientedRect(Vector2 center, Vector2 size, Single radians)
        {
            var xform = Matrix3x2.CreateScale(size) * Matrix3x2.CreateRotation(radians);

            // move origin to rectangle's corner
            var offset = Vector2.Transform(-Vector2.One * 0.5f, xform);

            xform.Translation = center + offset;

            return new ImageRegion(xform);
        }

        #endregion

        #region constructors        

        public ImageRegion(Matrix3x2 roi)
        {
            _Forward = roi;
            Matrix3x2.Invert(_Forward, out _Inverse);
        }

        public ImageRegion(System.Drawing.RectangleF roi)
        {
            _Forward = Matrix3x2.CreateScale(roi.Width, roi.Height);
            _Forward.Translation = new Vector2(roi.X, roi.Y);

            Matrix3x2.Invert(_Forward, out _Inverse);
        }

        private ImageRegion(in Matrix3x2 fw, in Matrix3x2 iv)
        {
            _Forward = fw;
            _Inverse = iv;
        }

        #endregion

        private readonly Matrix3x2 _Forward;
        private readonly Matrix3x2 _Inverse;

        public bool HasRotation => _Forward.M21 != 0 && _Forward.M12 != 0;

        public Matrix3x2 ROI => _Forward;
        public float ROIWidth => new Vector2(_Forward.M11, _Forward.M12).Length();
        public float ROIHeight => new Vector2(_Forward.M21, _Forward.M22).Length();

        public Matrix3x2 Transform => _Inverse;        

        public Vector2 Center => Vector2.Transform(Vector2.One * 0.5f, _Forward);

        /// <summary>
        /// Gets the matrix used to transform the source image to the ROI.
        /// </summary>
        /// <param name="imageSize">the source image size, in pixels.</param>
        /// <param name="roiToImage">returns a matrix that can convert points in cropped space to points in full image space.</param>
        /// <returns>A tranform from Full image to ROI.</returns>
        public Matrix3x2 GetImageTransform(InteropDrawing.Point2 imageSize, out Matrix3x2 roiToImage)
        {            
            var iform = _Inverse *  Matrix3x2.CreateScale(imageSize.X, imageSize.Y);
            Matrix3x2.Invert(iform, out roiToImage);
            return iform;
        }

        public static ImageRegion Multiply(in ImageRegion left, in ImageRegion right)
        {
            var iv = left._Inverse * right._Inverse;
            Matrix3x2.Invert(iv, out var fw);

            return new ImageRegion(fw, iv);            
        }
    }
}
