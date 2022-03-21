using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    using PCID = PixelComponentID;

    
    /// <summary>
    /// Wraps <see cref="PCID"/> enumeration, providing rich information.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplay(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly partial struct PixelComponent : IEquatable<PixelComponent>
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

        public bool IsFloating => Id >= PCID.Undefined32F;

        public bool Is8BitOrLess => Id < PCID.Undefined16;

        #endregion
    }
}
