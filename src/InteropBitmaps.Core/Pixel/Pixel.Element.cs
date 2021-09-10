using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    using PEF = Pixel.Format.ElementID;

    partial class Pixel
    {
        partial struct Format
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
                            case PEF.Premul4:
                            case PEF.Undefined4: return 4;

                            case PEF.Red5:
                            case PEF.Green5:
                            case PEF.Blue5:
                            case PEF.Undefined5: return 5;

                            case PEF.Green6:
                            case PEF.Undefined6: return 6;

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
                            case PEF.Empty: return 0;                            

                            case PEF.Index8:
                            case PEF.Alpha8:
                            case PEF.Premul8:
                            case PEF.Luminance8:
                            case PEF.Red8:
                            case PEF.Green8:
                            case PEF.Blue8:
                            case PEF.Undefined8: return 1;

                            case PEF.Index16:
                            case PEF.Luminance16:
                            case PEF.Red16:
                            case PEF.Green16:
                            case PEF.Blue16:
                            case PEF.Alpha16:
                            case PEF.Premul16:
                            case PEF.Millimeter16:
                            case PEF.Undefined16: return 2;

                            case PEF.Luminance32F:
                            case PEF.Red32F:
                            case PEF.Green32F:
                            case PEF.Blue32F:
                            case PEF.Alpha32F:
                            case PEF.Premul32F:
                            case PEF.Millimeter32:
                            case PEF.Meter32F:
                            case PEF.Undefined32F: return 4;

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

                public bool IsPremul
                {
                    get
                    {
                        switch (Id)
                        {                            
                            case PEF.Premul4: return true;
                            case PEF.Premul8: return true;
                            case PEF.Premul16: return true;
                            case PEF.Premul32F: return true;
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
                            case PEF.Luminance8: return true;
                            case PEF.Luminance16: return true;
                            case PEF.Luminance32F: return true;
                            default: return false;
                        }
                    }
                }

                #endregion
            }
        }

    }
}
