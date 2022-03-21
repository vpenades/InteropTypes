using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Backends.Helpers
{
    /// <summary>
    /// Helper class used to generate the raster scanlines of a convex polygon
    /// </summary>
    sealed class PolygonScanlines
    {
        #region lifecycle

        public PolygonScanlines(int w, int h)
        {
            _RasterWidth = w;
            _RasterHeight = h;

            _Left = new int[h];
            _Right = new int[h];
        }

        #endregion

        #region data        

        private readonly Int32 _RasterWidth;
        private readonly Int32 _RasterHeight;        

        private readonly int[] _Left;
        private readonly int[] _Right;

        private Vector2[] _Points;

        private int _MinY;
        private int _MaxY;

        #endregion

        #region API

        public IEnumerable<(int y, int xmin, int xmax)> GetScanlines(ReadOnlySpan<Vector2> points)
        {
            if (_Points == null || _Points.Length < points.Length)
            {
                _Points = new Vector2[points.Length];
            }

            points.CopyTo(_Points);

            for(int i=0; i < _Points.Length; ++i)
            {
                //_Points[i] += new Vector2(0.5f, 0.5f);
            }

            return _GetScanlines(points.Length);
        }

        private IEnumerable<(int y, int xmin, int xmax)> _GetScanlines(int count)
        {
            _MinY = int.MaxValue;
            _MaxY = int.MinValue;

            _FillSide(_Points[count - 1], _Points[0]);

            for (int i = 1; i < count; ++i)
            {
                _FillSide(_Points[i - 1], _Points[i]);
            }

            for (int y = _MinY; y < _MaxY; ++y)
            {
                var min = _Left[y];
                var max = _Right[y];
                if (min == max) continue;
                if (min < max) yield return (y, min, max);
                else yield return (y, max, min);
            }
        }

        private void _FillSide(Vector2 a, Vector2 b)
        {
            var ay = a.Y.RoundDown()._Clamp(0,_RasterHeight);
            var by = b.Y.RoundDown()._Clamp(0,_RasterHeight);
            if (ay == by) return;

            if (ay < by)
            {
                if (_MinY > ay) _MinY = ay;
                if (_MaxY < by) _MaxY = by;

                for (int y=ay; y < by; ++y)
                {
                    _Right[y] = _GradientLerp(a, b, y + 0.5f).RoundDown()._Clamp(0,_RasterWidth);
                }
            }
            else
            {
                if (_MinY > by) _MinY = by;
                if (_MaxY < ay) _MaxY = ay;

                for (int y = by; y < ay; ++y)
                {
                    _Left[y] = _GradientLerp(a, b, y + 0.5f).RoundDown()._Clamp(0, _RasterWidth);
                }
            }
        }        

        private static float _GradientLerp(in Vector2 a, in Vector2 b, float y)
        {
            // ensure all gradients are evaluated from top to down to ensure floating point consistency.

            if (a.Y < b.Y)
            {
                var gradient = (y - a.Y) / (b.Y - a.Y);

                System.Diagnostics.Debug.Assert(gradient.IsReal());

                return a.X * (1 - gradient) + b.X * gradient;
            }
            else
            {
                var gradient = (y - b.Y) / (a.Y - b.Y);

                System.Diagnostics.Debug.Assert(gradient.IsReal());

                return b.X * (1 - gradient) + a.X * gradient;
            }
        }

        #endregion
    }
}
