using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics
{
    /// <summary>
    /// Represents the meaning of a <see cref="PixelFormat"/>'s channel.<br/>
    /// It can be wrapped by a <see cref="PixelComponent"/> for additional information.
    /// </summary>
    /// <remarks>
    /// <para>This enumeration is required to be in the range of 0-255 to fit in 1 byte.</para>
    /// <para>
    /// This enumeration is evolving, so if serialization is needed,
    /// it is recomended to serialize the values as strings.
    /// </para>
    /// </remarks>
    public enum PixelComponentID
    {
        // 0 bits
        Empty = 0,

        // 1 bit
        Undefined1, Alpha1,

        // 2 bit
        Undefined2,

        // 3 bit
        Undefined3,

        // 4 bits
        Undefined4, Red4, Green4, Blue4, Premul4, Alpha4,

        // 5 bits
        Undefined5, Red5, Green5, Blue5,

        // 6 bits
        Undefined6, Green6,

        // 7 bit
        Undefined7,

        // 8 bits
        Undefined8, Index8, Red8, Green8, Blue8, Alpha8, Premul8, Luminance8,

        // 16 bits
        Undefined16, Index16, Red16, Green16, Blue16, Alpha16, Premul16, Luminance16, Millimeter16,

        // 24 bits
        Undefined24,

        // 32 bits (floating point)
        Undefined32F, Red32F, Green32F, Blue32F, Alpha32F, Premul32F, Luminance32F, Millimeter32F, Meter32F
    }
}
