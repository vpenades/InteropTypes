using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

using InteropDrawing;

using POINT = InteropDrawing.Point2;

namespace InteropVision
{
    partial class MarkersContext : IDrawingBrush<IDrawing2D>
    {
        public void DrawTo(IDrawing2D dc)
        {
            new _MarkersContextDebugView(this).DrawTo(dc);
        }

        readonly struct _MarkersContextDebugView : IDrawingBrush<IDrawing2D>
        {
            #region constructor

            public _MarkersContextDebugView(MarkersContext ctx)
            {
                _Detected = ctx._Detected;
                _CameraMirror = ctx._CameraMirror;
                _ScreenWidth = ctx._ScreenWidth;
                _CameraTransform = ctx._CameraTransform;
                _CameraDistortion = ctx._CameraDistortion;
            }

            #endregion

            #region data

            private readonly IReadOnlyList<Marker> _Detected;

            private readonly bool _CameraMirror;
            private readonly int _ScreenWidth;

            private readonly Matrix4x4 _CameraTransform;
            private readonly float[] _CameraDistortion;

            #endregion

            #region API

            private POINT _Mirrored(POINT p)
            {
                if (!_CameraMirror) return p;
                return new POINT(_ScreenWidth - p.X, p.Y);
            }

            public void DrawTo(IDrawing2D dc)
            {
                foreach (var aruco in _Detected)
                {
                    var aa = _Mirrored(aruco.A);
                    var bb = _Mirrored(aruco.B);
                    var cc = _Mirrored(aruco.C);
                    var dd = _Mirrored(aruco.D);

                    var color = Color.Green;

                    dc.DrawPolygon((color, 1), aa, bb, cc, dd);

                    if (_CameraDistortion != null)
                    {
                        var center = GetProjected(Vector3.Transform(Vector3.Zero, aruco.Transform));
                        var axisX = GetProjected(Vector3.Transform(Vector3.UnitX * 60, aruco.Transform));
                        var axisY = GetProjected(Vector3.Transform(Vector3.UnitY * 60, aruco.Transform));
                        var axisZ = GetProjected(Vector3.Transform(Vector3.UnitZ * 60, aruco.Transform));

                        center = _Mirrored(center);
                        axisX = _Mirrored(axisX);
                        axisY = _Mirrored(axisY);
                        axisZ = _Mirrored(axisZ);

                        dc.DrawLine(center, axisX, 1, Color.Red);
                        dc.DrawLine(center, axisY, 1, Color.Green);
                        dc.DrawLine(center, axisZ, 1, Color.Blue);
                    }
                }
            }

            private POINT GetProjected(Vector3 point)
            {
                point = Vector3.Transform(point, _CameraTransform);

                point /= point.Z;

                return (point.X, point.Y);
            }

            #endregion
        }
    }
}
