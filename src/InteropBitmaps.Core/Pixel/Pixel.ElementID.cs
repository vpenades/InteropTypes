using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {        
        partial struct Format
        {
            public enum ElementID
            {
                // 0 bits
                Empty = 0,

                // 1 bit
                Undefined1, Alpha1,

                // 4 bits
                Undefined4, Red4, Green4, Blue4, Alpha4, // PremulAlpha4

                // 5 bits
                Undefined5, Red5, Green5, Blue5,

                // 6 bits
                Undefined6, Green6,

                // 8 bits
                Undefined8, Index8, Red8, Green8, Blue8, Alpha8, Luminance8, // PremulAlpha8

                // 16 bits
                Undefined16, Index16, Red16, Green16, Blue16, Alpha16, Luminance16, DepthMM16,

                // 32 bits (floating point)
                Undefined32F, Red32F, Green32F, Blue32F, Alpha32F, Luminance32F,
            }
        }
    }
}
