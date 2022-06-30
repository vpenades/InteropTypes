using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    using PCID = PixelComponentID;

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
        Undefined8, Index8, Red8, Green8, Blue8, Alpha8, Premul8, Luminance8, UVMacro8,

        // 16 bits
        Undefined16, Index16, Red16, Green16, Blue16, Alpha16, Premul16, Luminance16, Millimeter16,

        // 24 bits
        Undefined24,

        // 32 bits (floating point)
        Undefined32F, Red32F, Green32F, Blue32F, Alpha32F, Premul32F, Luminance32F, Millimeter32F, Meter32F
    }

    partial struct PixelComponent
    {
        public bool IsUndefined
        {
            get
            {
                switch (Id)
                {
                    case PCID.Undefined1: return true;
                    case PCID.Undefined2: return true;
                    case PCID.Undefined3: return true;
                    case PCID.Undefined4: return true;
                    case PCID.Undefined5: return true;
                    case PCID.Undefined6: return true;
                    case PCID.Undefined7: return true;
                    case PCID.Undefined8: return true;
                    case PCID.Undefined16: return true;
                    case PCID.Undefined24: return true;
                    case PCID.Undefined32F: return true;
                    default: return false;
                }
            }
        }

        /// <summary>
        /// Gets the number of BITS required to store a value.
        /// </summary>
        public int BitCount
        {
            // an alternative to the switch is to have a LUT
            get
            {

                switch (Id)
                {
                    case PCID.Empty: return 0;

                    case PCID.Alpha1:
                    case PCID.Undefined1: return 1;

                    case PCID.Undefined2: return 2;

                    case PCID.Undefined3: return 3;

                    case PCID.Red4:
                    case PCID.Green4:
                    case PCID.Blue4:
                    case PCID.Alpha4:
                    case PCID.Premul4:
                    case PCID.Undefined4: return 4;

                    case PCID.Red5:
                    case PCID.Green5:
                    case PCID.Blue5:
                    case PCID.Undefined5: return 5;

                    case PCID.Green6:
                    case PCID.Undefined6: return 6;

                    case PCID.Undefined7: return 7;

                    default: return this.ByteCount * 8;
                }
            }
        }

        /// <summary>
        /// Gets the number of BYTES required to store a value
        /// </summary>
        public int ByteCount
        {
            // an alternative to the switch is to have a lookup table
            get
            {
                switch (Id)
                {
                    case PCID.Empty: return 0;

                    case PCID.Index8:
                    case PCID.Alpha8:
                    case PCID.Premul8:
                    case PCID.Luminance8:
                    case PCID.Red8:
                    case PCID.Green8:
                    case PCID.Blue8:
                    case PCID.Undefined8: return 1;

                    case PCID.Index16:
                    case PCID.Luminance16:
                    case PCID.Red16:
                    case PCID.Green16:
                    case PCID.Blue16:
                    case PCID.Alpha16:
                    case PCID.Premul16:
                    case PCID.Millimeter16:
                    case PCID.Undefined16: return 2;

                    case PCID.Undefined24: return 3;

                    case PCID.Luminance32F:
                    case PCID.Red32F:
                    case PCID.Green32F:
                    case PCID.Blue32F:
                    case PCID.Alpha32F:
                    case PCID.Premul32F:
                    case PCID.Millimeter32F:
                    case PCID.Meter32F:
                    case PCID.Undefined32F: return 4;

                    default:
                        {
                            if (IsEmpty) throw new InvalidOperationException("Component is empty");
                            if (IsUndefined) throw new InvalidOperationException("Component is undefined");
                            throw new InvalidOperationException($"{Id} bit count is not multiple of 8");
                        }
                }
            }
        }

        /// <summary>
        /// True if component is a Non premultiplied alpha channel.
        /// </summary>
        public bool IsUnpremulAlpha
        {
            get
            {
                switch (Id)
                {
                    case PCID.Alpha1: return true;
                    case PCID.Alpha4: return true;
                    case PCID.Alpha8: return true;
                    case PCID.Alpha16: return true;
                    case PCID.Alpha32F: return true;
                    default: return false;
                }
            }
        }

        /// <summary>
        /// True if component is apremultiplied alpha channel.
        /// </summary>
        public bool IsPremulAlpha
        {
            get
            {
                switch (Id)
                {
                    case PCID.Premul4: return true;
                    case PCID.Premul8: return true;
                    case PCID.Premul16: return true;
                    case PCID.Premul32F: return true;
                    default: return false;
                }
            }
        }

        /// <summary>
        /// True if component is a Red channel.
        /// </summary>
        public bool IsRed
        {
            get
            {
                switch (Id)
                {
                    case PCID.Red4: return true;
                    case PCID.Red5: return true;
                    case PCID.Red8: return true;
                    case PCID.Red16: return true;
                    case PCID.Red32F: return true;
                    default: return false;
                }
            }
        }

        /// <summary>
        /// True if component is a Green channel.
        /// </summary>
        public bool IsGreen
        {
            get
            {
                switch (Id)
                {
                    case PCID.Green4: return true;
                    case PCID.Green5: return true;
                    case PCID.Green6: return true;
                    case PCID.Green8: return true;                    
                    case PCID.Green16: return true;
                    case PCID.Green32F: return true;
                    default: return false;
                }
            }
        }

        /// <summary>
        /// True if component is a Blue channel.
        /// </summary>
        public bool IsBlue
        {
            get
            {
                switch (Id)
                {
                    case PCID.Blue4: return true;
                    case PCID.Blue5: return true;
                    case PCID.Blue8: return true;
                    case PCID.Blue16: return true;
                    case PCID.Blue32F: return true;
                    default: return false;
                }
            }
        }

        /// <summary>
        /// True if component is a Luminance channel.
        /// </summary>
        public bool IsLuminance
        {
            get
            {
                switch (Id)
                {
                    case PCID.Luminance8: return true;
                    case PCID.Luminance16: return true;
                    case PCID.Luminance32F: return true;
                    default: return false;
                }
            }
        }
    }
}
