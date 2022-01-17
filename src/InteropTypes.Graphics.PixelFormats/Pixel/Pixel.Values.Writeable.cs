using System;
using System.Collections.Generic;
using System.Text;

using XYZ = System.Numerics.Vector3;
using XYZA = System.Numerics.Vector4;

namespace InteropBitmaps
{
    partial class Pixel
    {
        partial struct Alpha8
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte A;

               public void SetValue(QVectorBGRP value) { A = value.AQ8; }
            }
        }

        partial struct BGR24
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte B;
                public Byte G;
                public Byte R;

                public void SetValue(QVectorBGRP value) { B = value.PreBQ8; G = value.PreGQ8; R = value.PreRQ8; }
            }
        }

        partial struct RGB24
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte R;
                public Byte G;
                public Byte B;

                public void SetValue(QVectorBGRP value) { B = value.PreRQ8; G = value.PreGQ8;  R = value.PreBQ8; }
            }
        }

        partial struct BGRA32
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte B;
                public Byte G;
                public Byte R;
                public Byte A;

                public void SetValue(QVectorBGRP value)
                {
                    if (value.A == 0) { this = default; return; }
                    var rcpa = FixedMath.GetUnboundedReciprocal8(value.A);
                    B = FixedMath.To255(value.B, rcpa);
                    G = FixedMath.To255(value.G, rcpa);
                    R = FixedMath.To255(value.R, rcpa);
                    A = value.AQ8;
                }
            }
        }

        partial struct RGBA32
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte R;
                public Byte G;
                public Byte B;                
                public Byte A;

                public void SetValue(QVectorBGRP value)
                {
                    if (value.A == 0) { this = default; return; }

                    var rcpa = FixedMath.GetUnboundedReciprocal8(value.A);
                    B = FixedMath.To255(value.B, rcpa);
                    G = FixedMath.To255(value.G, rcpa);
                    R = FixedMath.To255(value.R, rcpa);
                    A = value.AQ8;
                }
            }
        }

        partial struct ARGB32
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte A;
                public Byte R;
                public Byte G;
                public Byte B;                

                public void SetValue(QVectorBGRP value)
                {
                    if (value.A == 0) { this = default; return; }

                    var rcpa = FixedMath.GetUnboundedReciprocal8(value.A);
                    B = FixedMath.To255(value.B, rcpa);
                    G = FixedMath.To255(value.G, rcpa);
                    R = FixedMath.To255(value.R, rcpa);
                    A = value.AQ8;
                }
            }
        }

        partial struct BGRP32
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte B;
                public Byte G;
                public Byte R;
                public Byte A;

                public void SetValue(QVectorBGRP value)
                {
                    B = value.PreBQ8;
                    G = value.PreGQ8;
                    R = value.PreRQ8;
                    A = value.AQ8;
                }
            }
        }

        partial struct RGBP32
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public partial struct Writeable : IValueSetter<QVectorBGRP>
            {
                public Byte R;                
                public Byte G;
                public Byte B;
                public Byte A;

                public void SetValue(QVectorBGRP value)
                {
                    R = value.PreRQ8;
                    G = value.PreGQ8;
                    B = value.PreBQ8;                    
                    A = value.AQ8;
                }
            }
        }


    }
}
