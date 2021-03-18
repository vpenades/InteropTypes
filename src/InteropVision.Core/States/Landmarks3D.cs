using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Numerics;

using InteropTensors;

using XY = System.Numerics.Vector2;
using XYZ = System.Numerics.Vector3;

using DISPRIM = InteropVision.DisplayPrimitive;


namespace InteropVision
{
    public class Landmarks3D : DISPRIM.ISource
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

        private Func<Landmarks3D, IEnumerable<DISPRIM>> _DisplayFunc;

        #endregion

        #region properties

        public Score Score => _Score;        

        public IReadOnlyList<XYZ> Landmarks => _Landmarks ?? Array.Empty<XYZ>();

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

        public void SetLandmarks(Score score, ReadOnlySpan<Vector3> points)
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

        public void DrawTo(InteropDrawing.IDrawing2D dc)
        {
            DISPRIM.Draw(dc, GetDisplayPrimitives());
        }

        public IEnumerable<DISPRIM> GetDisplayPrimitives()
        {
            if (_DisplayFunc == null) return Enumerable.Empty<DISPRIM>();

            return _DisplayFunc(this);
        }

        public void SetDisplayFunc(Func<Landmarks3D, IEnumerable<DISPRIM>> f) { _DisplayFunc = f; }

        #endregion
    }
}
