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
    /// <summary>
    /// Represents a collection of 3D points produced by an inference model.
    /// </summary>
    public class Landmarks3D : IDrawingBrush<IScene3D> //, InteropDrawing.IBounds3D
    {
        #region lifecycle

        public Landmarks3D()
        {
            _OnDraw = _DrawPoints;
        }
        
        public Landmarks3D(params int[] innerIndices)
        {
            void drawLines(IScene3D dc, Landmarks3D lll)
            {
                // ideally this should be half the distance of the two closest points
                // but that's not trivial to calculate.
                var r = 0.25f * _GetSize().Length() / (Landmarks.Count + 1);

                for (int i = 1; i < innerIndices.Length; i += 2)
                {
                    var a = innerIndices[i - 1];                    
                    if (a >= lll._Landmarks.Length) continue;

                    var b = innerIndices[i + 0];
                    if (b >= lll._Landmarks.Length) continue;

                    var aa = lll._Landmarks[a];
                    var bb = lll._Landmarks[b];

                    dc.DrawSegment(aa, bb, r * 2, Color.White);
                }                
            }

            _OnDraw = drawLines;
        }

        #endregion

        #region data        

        private Score _Score;
        private XYZ[] _Landmarks;
        private RectangleF? _Bounds;
        private XYZ? _Size;

        private Action<IScene3D, Landmarks3D> _OnDraw;

        #endregion

        #region properties

        public Score Score => _Score;

        public string Name { get; set; }

        public DateTime CaptureTime { get; set; }

        public IReadOnlyList<XYZ> Landmarks => _Landmarks ?? Array.Empty<XYZ>();        

        /// <summary>
        /// Represents the landmarks bounds in the XY Axis.
        /// </summary>
        public RectangleF Bounds
        {
            get
            {
                if (_Bounds.HasValue) return _Bounds.Value;

                var min = _Landmarks.Aggregate((a, b) => XYZ.Min(a, b));
                var max = _Landmarks.Aggregate((a, b) => XYZ.Max(a, b));
                var siz = max - min;

                _Bounds = new RectangleF(min.X, min.Y, siz.X, siz.Y);

                return _Bounds.Value;
            }
        }

        #endregion

        #region API

        public void Clear()
        {
            _Score = (0, false);
            _Bounds = null;
            _Size = null;
        }

        public void CopyTo(ref Landmarks3D other)
        {
            if (other == null) other = new Landmarks3D();

            other._OnDraw = this._OnDraw;

            other.Name = this.Name;
            other.CaptureTime = this.CaptureTime;

            other.SetLandmarks(this.Score, this._Landmarks);
            other._Bounds = this._Bounds;
            other._Size = this._Size;
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
            _Bounds = null;
            _Size = null;
            if (_Landmarks == null) return;
            if (!_Score.IsValid) return;

            _Landmarks.TransformBy(xform);
        }

        public void TransformBy(Matrix3x2 xform)
        {
            _Bounds = null;
            _Size = null;
            if (_Landmarks == null) return;
            if (!_Score.IsValid) return;

            _Landmarks.TransformBy(xform);
        }

        public void TransformByOffCenter(Matrix4x4 xform)
        {
            _Bounds = null;
            _Size = null;
            if (_Landmarks == null) return;
            if (!_Score.IsValid) return;

            var bounds = this.Bounds;
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

        public virtual void DrawTo(IScene3D dc) { _OnDraw?.Invoke(dc, this); }

        private static void _DrawPoints(IScene3D dc, Landmarks3D lll)
        {
            // ideally this should be half the distance of the two closest points
            // but that's not trivial to calculate.
            var r = 0.25f * lll._GetSize().Length() / (lll.Landmarks.Count + 1);

            foreach (var l in lll._Landmarks)
            {
                dc.DrawSphere(l, r * 2f, Color.White);
            }            
        }

        #endregion
    }
}
