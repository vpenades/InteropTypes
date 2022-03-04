using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics
{
    using PCID = PixelComponentID;

    
    /// <summary>
    /// Wraps <see cref="PCID"/> enumeration, providing rich information.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplay(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly struct PixelComponent : IEquatable<PixelComponent>
    {
        #region debug

        private string _GetDebuggerDisplay()
        {
            return Id.ToString();
        }

        #endregion

        #region lifecycle

        public static implicit operator PixelComponent(PCID id) { return new PixelComponent(id); }

        public static implicit operator PCID(PixelComponent fmt) { return fmt.Id; }

        public PixelComponent(PCID id) { _Identifier = (Byte)id; }

        public PixelComponent(Byte id) { _Identifier = id; }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly Byte _Identifier;

        /// <inheritdoc />
        public override int GetHashCode() => _Identifier.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object obj) { return obj is PixelComponent other && this._Identifier == other._Identifier; }

        /// <inheritdoc />
        public bool Equals(PixelComponent other) => this._Identifier == other._Identifier;

        /// <inheritdoc />
        public static bool operator ==(PixelComponent a, PixelComponent b) => a._Identifier == b._Identifier;

        /// <inheritdoc />
        public static bool operator !=(PixelComponent a, PixelComponent b) => a._Identifier != b._Identifier;

        #endregion

        #region properties

        public PCID Id => (PCID)_Identifier;

        public bool IsEmpty => _Identifier == 0;

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

                    default: throw new NotImplementedException($"Not implemented:{Id}");
                }
            }
        }

        public bool IsUndefined
        {
            get
            {
                switch (Id)
                {
                    case PCID.Undefined1: return true;
                    case PCID.Undefined4: return true;
                    case PCID.Undefined5: return true;
                    case PCID.Undefined6: return true;
                    case PCID.Undefined8: return true;
                    case PCID.Undefined16: return true;
                    case PCID.Undefined32F: return true;
                    default: return false;
                }
            }
        }

        /// <summary>
        /// True if Non premultiplied alpha
        /// </summary>
        public bool IsAlpha
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
        /// True if premultiplied alpha
        /// </summary>
        public bool IsPremul
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

        public bool IsGreen
        {
            get
            {
                switch (Id)
                {
                    case PCID.Green4: return true;
                    case PCID.Green5: return true;
                    case PCID.Green8: return true;
                    case PCID.Green6: return true;
                    case PCID.Green16: return true;
                    case PCID.Green32F: return true;
                    default: return false;
                }
            }
        }

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

        public bool IsFloating => Id >= PCID.Undefined32F;

        public bool Is8BitOrLess => Id < PCID.Undefined16;

        #endregion
    }
}
