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
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
            internal partial struct Factory
            {
                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public static implicit operator Alpha8(Factory pixel) { return pixel.Value; }

                [System.Runtime.InteropServices.FieldOffset(0)]
                public Alpha8 Value;

                [System.Runtime.InteropServices.FieldOffset(0)]
                public Byte A;
            }            
        }
        
        partial struct Luminance8
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
            internal partial struct Factory
            {
                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public static implicit operator Luminance8(Factory pixel) { return pixel.Value; }

                [System.Runtime.InteropServices.FieldOffset(0)]
                public Luminance8 Value;

                [System.Runtime.InteropServices.FieldOffset(0)]
                public Byte L;
            }
            
        }

        partial struct Luminance16
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
            internal partial struct Factory
            {
                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public static implicit operator Luminance16(Factory pixel) { return pixel.Value; }

                [System.Runtime.InteropServices.FieldOffset(0)]
                public Luminance16 Value;

                [System.Runtime.InteropServices.FieldOffset(0)]
                public UInt16 L;
            }            
        }
        
        partial struct Luminance32F
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
            internal partial struct Factory
            {
                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public static implicit operator Luminance32F(Factory pixel) { return pixel.Value; }

                [System.Runtime.InteropServices.FieldOffset(0)]
                public Luminance32F Value;

                [System.Runtime.InteropServices.FieldOffset(0)]
                public float L;
            }
        }

        partial struct BGR565
        {
            internal partial struct Factory
            {
                [System.Runtime.CompilerServices.MethodImpl(_PrivateConstants.Fastest)]
                public static implicit operator BGR565(Factory pixel) { return pixel.Value; }

                public BGR565 Value => new BGR565(PackRGB(R, G, B));

                public int R;
                public int G;
                public int B;

                public static UInt16 PackRGB(int red, int green, int blue)
                {
                    int bgr = red << 8;
                    bgr &= 0b1111100000000000;
                    bgr |= green << 3;
                    bgr &= 0b1111111111100000;
                    bgr |= blue >> 3;

                    return (UInt16)bgr;
                }
            }
        }
    }
}