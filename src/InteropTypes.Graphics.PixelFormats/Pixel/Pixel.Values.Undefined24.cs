using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        /// <summary>
        /// represents a 24 bit pixel format with undefined components.
        /// </summary>
        /// <remarks>
        /// This type is useful in cases where we need a 24 bit pixel and we don't care about the pixel components.
        /// </remarks>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        [System.Diagnostics.DebuggerDisplay("{X} {Y} {Z}")]
        public partial struct Undefined24 : IReflection, IEquatable<Undefined24>
        {
            #region constants

            public const uint Code = _PackedPixelCodes.Undefined24;
            public static readonly PixelFormat Format = new PixelFormat(Code);

            #endregion

            #region constructors           

            public Undefined24(int x, int y, int z)
            {
                X = (byte)x;
                Y = (byte)y;
                Z = (byte)z;
            }

            #endregion

            #region data

            public Byte X;
            public Byte Y;
            public Byte Z;            

            /// <inheritdoc/>
            public static bool operator ==(in Undefined24 a, in Undefined24 b) { return a.Equals(b); }

            /// <inheritdoc/>
            public static bool operator !=(in Undefined24 a, in Undefined24 b) { return !a.Equals(b); }

            /// <inheritdoc/>
            public override bool Equals(Object obj) { return obj is Undefined24 other && Equals(other); }

            /// <inheritdoc/>
            public bool Equals(Undefined24 other)
            {
                if (this.X != other.X) return false;
                if (this.Y != other.Y) return false;
                if (this.Z != other.Z) return false;
                return true;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                int h = 0;
                h ^= this.X; h <<= 8;
                h ^= this.Y; h <<= 8;
                h ^= this.Z; h <<= 8;
                return h;
            }            

            #endregion

            #region reflection

            public bool IsOpaque => true;

            public bool IsPremultiplied => false;

            public uint GetCode() { return Code; }

            public PixelFormat GetPixelFormat() { return Format; }

            #endregion
        }
    }
}
