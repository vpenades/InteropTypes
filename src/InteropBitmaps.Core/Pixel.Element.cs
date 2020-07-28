using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PEF = Pixel.ElementID;

    partial class Pixel
    {
        [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplay(),nq}")]
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public readonly struct Element : IEquatable<Element>
        {
            #region debug

            private string _GetDebuggerDisplay()
            {
                return Id.ToString();
            }

            #endregion            

            #region lifecycle

            public static implicit operator Element(ElementID id) { return new Element(id); }

            public static implicit operator ElementID(Element fmt) { return fmt.Id; }

            public Element(ElementID id) { _Identifier = (Byte)id; }

            public Element(Byte id) { _Identifier = id; }

            #endregion

            #region data

            private readonly Byte _Identifier;

            public override int GetHashCode() => _Identifier.GetHashCode();

            public override bool Equals(object obj) { return obj is Element other && this._Identifier == other._Identifier; }

            public bool Equals(Element other) => this._Identifier == other._Identifier;

            public static bool operator ==(Element a, Element b) => a._Identifier == b._Identifier;

            public static bool operator !=(Element a, Element b) => a._Identifier != b._Identifier;

            #endregion

            #region properties

            public ElementID Id => (ElementID)_Identifier;

            public bool IsEmpty => _Identifier == 0;

            public int BitCount
            {
                // an alternative to the switch is to have a lookup table
                get
                {

                    switch (Id)
                    {
                        case PEF.Empty: return 0;

                        case PEF.Alpha1:
                        case PEF.Undefined1: return 1;

                        case PEF.Red4:
                        case PEF.Green4:
                        case PEF.Blue4:
                        case PEF.Alpha4:
                        case PEF.Undefined4: return 4;

                        case PEF.Red5:
                        case PEF.Green5:
                        case PEF.Blue5:
                        case PEF.Undefined5: return 5;

                        case PEF.Green6:
                        case PEF.Undefined6: return 6;

                        case PEF.Index8:
                        case PEF.Alpha8:
                        case PEF.Gray8:
                        case PEF.Red8:
                        case PEF.Green8:
                        case PEF.Blue8:
                        case PEF.Undefined8: return 8;

                        case PEF.Index16:
                        case PEF.Gray16:
                        case PEF.Red16:
                        case PEF.Green16:
                        case PEF.Blue16:
                        case PEF.Alpha16:
                        case PEF.DepthMM16:
                        case PEF.Undefined16: return 16;

                        case PEF.Gray32F:
                        case PEF.Red32F:
                        case PEF.Green32F:
                        case PEF.Blue32F:
                        case PEF.Alpha32F:
                        case PEF.Undefined32F: return 32;

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
                        case PEF.Undefined1: return true;
                        case PEF.Undefined4: return true;
                        case PEF.Undefined5: return true;
                        case PEF.Undefined6: return true;
                        case PEF.Undefined8: return true;
                        case PEF.Undefined16: return true;
                        case PEF.Undefined32F: return true;
                        default: return false;
                    }
                }
            }

            public bool IsAlpha
            {
                get
                {
                    switch (Id)
                    {
                        case PEF.Alpha1: return true;
                        case PEF.Alpha4: return true;
                        case PEF.Alpha8: return true;
                        case PEF.Alpha16: return true;
                        case PEF.Alpha32F: return true;
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
                        case PEF.Red4: return true;
                        case PEF.Red5: return true;
                        case PEF.Red8: return true;
                        case PEF.Red16: return true;
                        case PEF.Red32F: return true;
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
                        case PEF.Green4: return true;
                        case PEF.Green5: return true;
                        case PEF.Green8: return true;
                        case PEF.Green6: return true;
                        case PEF.Green16: return true;
                        case PEF.Green32F: return true;
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
                        case PEF.Blue4: return true;
                        case PEF.Blue5: return true;
                        case PEF.Blue8: return true;
                        case PEF.Blue16: return true;
                        case PEF.Blue32F: return true;
                        default: return false;
                    }
                }
            }

            public bool IsGrey
            {
                get
                {
                    switch (Id)
                    {
                        case PEF.Gray8: return true;
                        case PEF.Gray16: return true;
                        case PEF.Gray32F: return true;
                        default: return false;
                    }
                }
            }

            #endregion
        }

    }
}
