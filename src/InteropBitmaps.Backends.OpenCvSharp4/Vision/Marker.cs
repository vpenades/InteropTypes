using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using POINT = InteropDrawing.Point2;

namespace InteropVision
{
    [System.Diagnostics.DebuggerDisplay("{Id}")]
    public struct Marker
    {
        public int Id;
        public POINT A;
        public POINT B;
        public POINT C;
        public POINT D;

        public Vector3 Rotation;
        public Vector3 Translation;

        public Matrix4x4 Transform
        {
            get
            {
                var x = Matrix4x4.CreateFromAxisAngle(Vector3.Normalize(Rotation), Rotation.Length());
                x.Translation = Translation;
                return x;
            }
        }
    }
}
