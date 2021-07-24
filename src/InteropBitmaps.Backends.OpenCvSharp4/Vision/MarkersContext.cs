using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropVision
{
    public partial class MarkersContext
    {
        #region lifecycle

        public MarkersContext()
        {
            _CameraTransform = new Matrix4x4
            (
                1009, 0, 0, 0,
                0, 1009, 0, 0,
                640, 360, 0, 0,
                0, 0, 0, 1
            );

            _CameraDistortion = new float[5];
        }

        #endregion

        #region data

        private readonly List<Marker> _Detected = new List<Marker>();

        private bool _CameraMirror;
        private int _ScreenWidth;

        private Matrix4x4 _CameraTransform;
        private float[] _CameraDistortion;

        #endregion

        public IReadOnlyList<Marker> Markers => _Detected;        
    }
}
