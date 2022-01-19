using System;
using System.Collections.Generic;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropBitmaps
{
    partial class Pixel
    {
        partial struct Alpha8 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value) { A = value.AQ8; }
        }

        partial struct BGR24 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value) { B = value.PreBQ8; G = value.PreGQ8; R = value.PreRQ8; }
        }

        partial struct RGB24 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value) { B = value.PreRQ8; G = value.PreGQ8; R = value.PreBQ8; }
        }

        partial struct BGRA32 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value)
            {
                if (value.A == 0) { this = default; return; }
                var rcpa = FixedMathCC8.ToReciprocalByte(value.A);
                B = FixedMathCC8.ToByte(value.B, rcpa);
                G = FixedMathCC8.ToByte(value.G, rcpa);
                R = FixedMathCC8.ToByte(value.R, rcpa);
                A = value.AQ8;
            }
        }

        partial struct RGBA32 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value)
            {
                if (value.A == 0) { this = default; return; }

                var rcpa = FixedMathCC8.ToReciprocalByte(value.A);
                B = FixedMathCC8.ToByte(value.B, rcpa);
                G = FixedMathCC8.ToByte(value.G, rcpa);
                R = FixedMathCC8.ToByte(value.R, rcpa);
                A = value.AQ8;
            }
        }

        partial struct ARGB32 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value)
            {
                if (value.A == 0) { this = default; return; }

                var rcpa = FixedMathCC8.ToReciprocalByte(value.A);
                B = FixedMathCC8.ToByte(value.B, rcpa);
                G = FixedMathCC8.ToByte(value.G, rcpa);
                R = FixedMathCC8.ToByte(value.R, rcpa);
                A = value.AQ8;
            }
        }

        partial struct BGRP32 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value)
            {
                PreB = value.PreBQ8;
                PreG = value.PreGQ8;
                PreR = value.PreRQ8;
                A = value.AQ8;
            }
        }

        partial struct RGBP32 : IValueSetter<QVectorBGRP>
        {
            public void SetValue(QVectorBGRP value)
            {
                PreR = value.PreRQ8;
                PreG = value.PreGQ8;
                PreB = value.PreBQ8;
                A = value.AQ8;
            }
        }


    }
}
