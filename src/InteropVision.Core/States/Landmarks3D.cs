using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Numerics;

using XYZ = System.Numerics.Vector3;
using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Vision
{
    public class Landmarks3D : IDrawingBrush<IScene3D> //, InteropDrawing.IBounds3D
    {
        #region lifecycle

        public Landmarks3D() { }
        
        public Landmarks3D(params int[] innerIndices)
        {
            _InnerIndices = innerIndices;
        }

        #endregion

        #region data

        private readonly int[] _InnerIndices;

        private Score _Score;
        private XYZ[] _Landmarks;

        private RectangleF? _InnerBounds;
        private RectangleF? _OuterBounds;
        private XYZ? _Size;

        // private (XYZ Center, float Radius)? _BoundingSphere;

        #endregion

        #region properties

        public Score Score => _Score;        

        public IReadOnlyList<XYZ> Landmarks => _Landmarks ?? Array.Empty<XYZ>();

        /// <summary>
        /// Represents the bounds of the landmarks defined by <see cref="_InnerIndices"/>.<br/>
        /// Otherwise, it's the same as OuterBounds.
        /// </summary>
        public RectangleF InnerBounds
        {
            get
            {
                if (_InnerBounds.HasValue) return _InnerBounds.Value;

                if (_InnerIndices == null) { _InnerBounds = this.OuterBounds; }
                else
                {
                    var landmarks = _InnerIndices.Select(idx => _Landmarks[idx]);

                    // it could be useful to have an (min,max) = AggregateTuple((a,b) => min, (a,b)=> max);
                    var min = landmarks.Aggregate((a, b) => XYZ.Min(a, b));
                    var max = landmarks.Aggregate((a, b) => XYZ.Max(a, b));
                    var siz = max - min;

                    _InnerBounds = new RectangleF(min.X, min.Y, siz.X, siz.Y);
                }

                return _InnerBounds.Value;
            }
        }

        /// <summary>
        /// Represents the landmarks bounds in the XY Axis.
        /// </summary>
        public RectangleF OuterBounds
        {
            get
            {
                if (_OuterBounds.HasValue) return _OuterBounds.Value;

                var min = _Landmarks.Aggregate((a, b) => XYZ.Min(a, b));
                var max = _Landmarks.Aggregate((a, b) => XYZ.Max(a, b));
                var siz = max - min;

                _OuterBounds = new RectangleF(min.X, min.Y, siz.X, siz.Y);

                return _OuterBounds.Value;
            }
        }

        #endregion

        #region API

        public void Clear()
        {
            _Score = (0,false);
            _OuterBounds = null;
            _InnerBounds = null;
        }        

        public void SetLandmarks(Score score)
        {
            Clear();
            _Score = score;
        }

        public void SetLandmarks(Score score, ReadOnlySpan<XYZ> points)
        {
            if (!score.IsValid) { SetLandmarks(score); return; }

            Clear();
            _Score = score;

            if (_Landmarks == null || _Landmarks.Length != points.Length) _Landmarks = new XYZ[points.Length];

            points.CopyTo(_Landmarks);
        }

        public void TransformBy(Matrix4x4 xform)
        {
            _InnerBounds = null;
            _OuterBounds = null;
            if (_Landmarks == null) return;
            if (!_Score.IsValid) return;

            _Landmarks.TransformBy(xform);
        }

        public void TransformBy(Matrix3x2 xform)
        {
            _InnerBounds = null;
            _OuterBounds = null;
            if (_Landmarks == null) return;
            if (!_Score.IsValid) return;

            _Landmarks.TransformBy(xform);
        }

        public void TransformByOffCenter(Matrix4x4 xform)
        {
            _InnerBounds = null;
            _OuterBounds = null;
            if (_Landmarks == null) return;
            if (!_Score.IsValid) return;

            var bounds = this.OuterBounds;
            var center = new Vector3(bounds.X +bounds.Width*0.5f, bounds.Y + bounds.Height * 0.5f, 0);

            xform = Matrix4x4.CreateTranslation(-center) * xform * Matrix4x4.CreateTranslation(center);

            TransformBy(xform);

        }

        #endregion

        #region drawing        

        private XYZ _GetSize()
        {
            if (_Size.HasValue) return _Size.Value;

            var min = _Landmarks.Aggregate((a, b) => XYZ.Min(a, b));
            var max = _Landmarks.Aggregate((a, b) => XYZ.Max(a, b));
            _Size = max - min;

            return _Size.Value;
        }

        public virtual void DrawTo(InteropTypes.Graphics.Drawing.IScene3D dc)
        {
            // ideally this should be half the distance of the two closest points
            // but that's not trivial to calculate.
            var r = 0.25f * _GetSize().Length() / (Landmarks.Count + 1);

            foreach(var l in Landmarks)
            {
                dc.DrawSphere(l, r * 2f, Color.White);
            }
        }

        /*
        public (XYZ Center, float Radius) GetBoundingSphere()
        {
            if (_BoundingSphere.HasValue) return _BoundingSphere.Value;

            throw new NotImplementedException();
        }*/

        #endregion
    }
}
