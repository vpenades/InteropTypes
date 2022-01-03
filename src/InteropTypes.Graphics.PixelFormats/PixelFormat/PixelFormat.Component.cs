using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PFCID = PixelFormat.ComponentID;

    partial struct PixelFormat
    {
        /// <summary>
        /// Wraps <see cref="PFCID"/> enumeration, providing rich information.
        /// </summary>
        [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplay(),nq}")]
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public readonly struct Component : IEquatable<Component>
        {
            #region debug

            private string _GetDebuggerDisplay()
            {
                return Id.ToString();
            }

            #endregion

            #region lifecycle

            public static implicit operator Component(PFCID id) { return new Component(id); }

            public static implicit operator PFCID(Component fmt) { return fmt.Id; }

            public Component(PFCID id) { _Identifier = (Byte)id; }

            public Component(Byte id) { _Identifier = id; }

            #endregion

            #region data

            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
            private readonly Byte _Identifier;

            /// <inheritdoc />
            public override int GetHashCode() => _Identifier.GetHashCode();

            /// <inheritdoc />
            public override bool Equals(object obj) { return obj is Component other && this._Identifier == other._Identifier; }

            /// <inheritdoc />
            public bool Equals(Component other) => this._Identifier == other._Identifier;

            /// <inheritdoc />
            public static bool operator ==(Component a, Component b) => a._Identifier == b._Identifier;

            /// <inheritdoc />
            public static bool operator !=(Component a, Component b) => a._Identifier != b._Identifier;

            #endregion

            #region properties

            public PFCID Id => (PFCID)_Identifier;

            public bool IsEmpty => _Identifier == 0;

            public int BitCount
            {
                // an alternative to the switch is to have a LUT
                get
                {

                    switch (Id)
                    {
                        case PFCID.Empty: return 0;

                        case PFCID.Alpha1:
                        case PFCID.Undefined1: return 1;

                        case PFCID.Red4:
                        case PFCID.Green4:
                        case PFCID.Blue4:
                        case PFCID.Alpha4:
                        case PFCID.Premul4:
                        case PFCID.Undefined4: return 4;

                        case PFCID.Red5:
                        case PFCID.Green5:
                        case PFCID.Blue5:
                        case PFCID.Undefined5: return 5;

                        case PFCID.Green6:
                        case PFCID.Undefined6: return 6;

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
                        case PFCID.Empty: return 0;

                        case PFCID.Index8:
                        case PFCID.Alpha8:
                        case PFCID.Premul8:
                        case PFCID.Luminance8:
                        case PFCID.Red8:
                        case PFCID.Green8:
                        case PFCID.Blue8:
                        case PFCID.Undefined8: return 1;

                        case PFCID.Index16:
                        case PFCID.Luminance16:
                        case PFCID.Red16:
                        case PFCID.Green16:
                        case PFCID.Blue16:
                        case PFCID.Alpha16:
                        case PFCID.Premul16:
                        case PFCID.Millimeter16:
                        case PFCID.Undefined16: return 2;

                        case PFCID.Luminance32F:
                        case PFCID.Red32F:
                        case PFCID.Green32F:
                        case PFCID.Blue32F:
                        case PFCID.Alpha32F:
                        case PFCID.Premul32F:
                        case PFCID.Millimeter32:
                        case PFCID.Meter32F:
                        case PFCID.Undefined32F: return 4;

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
                        case PFCID.Undefined1: return true;
                        case PFCID.Undefined4: return true;
                        case PFCID.Undefined5: return true;
                        case PFCID.Undefined6: return true;
                        case PFCID.Undefined8: return true;
                        case PFCID.Undefined16: return true;
                        case PFCID.Undefined32F: return true;
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
                        case PFCID.Alpha1: return true;
                        case PFCID.Alpha4: return true;
                        case PFCID.Alpha8: return true;
                        case PFCID.Alpha16: return true;
                        case PFCID.Alpha32F: return true;
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
                        case PFCID.Premul4: return true;
                        case PFCID.Premul8: return true;
                        case PFCID.Premul16: return true;
                        case PFCID.Premul32F: return true;
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
                        case PFCID.Red4: return true;
                        case PFCID.Red5: return true;
                        case PFCID.Red8: return true;
                        case PFCID.Red16: return true;
                        case PFCID.Red32F: return true;
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
                        case PFCID.Green4: return true;
                        case PFCID.Green5: return true;
                        case PFCID.Green8: return true;
                        case PFCID.Green6: return true;
                        case PFCID.Green16: return true;
                        case PFCID.Green32F: return true;
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
                        case PFCID.Blue4: return true;
                        case PFCID.Blue5: return true;
                        case PFCID.Blue8: return true;
                        case PFCID.Blue16: return true;
                        case PFCID.Blue32F: return true;
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
                        case PFCID.Luminance8: return true;
                        case PFCID.Luminance16: return true;
                        case PFCID.Luminance32F: return true;
                        default: return false;
                    }
                }
            }

            public bool IsFloating => Id >= PFCID.Undefined32F;

            public bool Is8BitOrLess => Id < PFCID.Undefined16;

            #endregion
        }
    }


}
