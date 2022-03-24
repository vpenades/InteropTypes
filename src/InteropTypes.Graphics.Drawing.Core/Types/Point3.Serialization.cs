using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{
    partial struct Point3
    {
        


        private static readonly System.Numerics.Vector3 XYZ_127 =new System.Numerics.Vector3(127);
               
        public static void SerializeQuantizedScaled(Action<Byte> writer, in Point3 point)
        {
            var v = point.XYZ;
            Encoded7BitInt32.FromSigned32((int)Math.Round(v.X)).WriteTo(writer);
            Encoded7BitInt32.FromSigned32((int)Math.Round(v.Y)).WriteTo(writer);
            Encoded7BitInt32.FromSigned32((int)Math.Round(v.Z)).WriteTo(writer);
        }

        public static void DeserializeQuantizedScaled(Func<Byte> reader, out Point3 point)
        {
            var x = Encoded7BitInt32.ReadFrom(reader).ToSigned32();
            var y = Encoded7BitInt32.ReadFrom(reader).ToSigned32();
            var z = Encoded7BitInt32.ReadFrom(reader).ToSigned32();
            point = new Point3(x, y, z);
        }

        public static void SerializeQuantizedDirectionLength(Action<Byte> writer, in Point3 point)
        {
            var v = point.XYZ;

            var len = v.Length();
            v /= len;

            writer(ToQuantizedByte(v.X));
            writer(ToQuantizedByte(v.Y));
            writer(ToQuantizedByte(v.Z));
            
            Encoded7BitInt32.FromUnsigned32((uint)Math.Round(len)).WriteTo(writer);
        }

        public static void DeserializeQuantizedDirectionLength(Func<Byte> reader, out Point3 point)
        {
            var x = ToNormalizedFloat(reader());
            var y = ToNormalizedFloat(reader());
            var z = ToNormalizedFloat(reader());
            var l = Encoded7BitInt32.ReadFrom(reader).ToUnsigned32();
            point = new System.Numerics.Vector3(x, y, z) * l;
        }

        private static Byte ToQuantizedByte(float normalized)
        {
            if (normalized < -1) normalized = -1;
            if (normalized > 1) normalized = 1;
            var q = (int)(normalized * 127);

            return (byte)q;
        }

        private static float ToNormalizedFloat(Byte value)
        {
            float x = value > 127
                ? value - 256
                : value;

            return x /= 127f;
        }       
    }


    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    readonly struct Encoded7BitInt32 : IReadOnlyList<Byte>
    {
        #region constructors

        public unsafe static Encoded7BitInt32 ReadFrom(System.IO.Stream stream)
        {
            Span<Byte> data = stackalloc byte[sizeof(Encoded7BitInt32)];
            int count = 0;

            Byte b;

            do
            {
                int bb = stream.ReadByte();
                if (bb < 0) throw new System.IO.EndOfStreamException();
                b = (Byte)bb; data[count++] = b;
            } while ((b & 0x80) != 0);

            if (count > 5) throw new FormatException("Format_Bad7BitInt32");

            data[5] = (byte)count;

            return System.Runtime.InteropServices.MemoryMarshal.Read<Encoded7BitInt32>(data);
        }

        public unsafe static Encoded7BitInt32 ReadFrom(Func<Byte> reader)
        {
            Span<Byte> data = stackalloc byte[sizeof(Encoded7BitInt32)];
            int count = 0;

            Byte b;

            do { b = reader(); data[count++] = b; } while ((b & 0x80) != 0);

            if (count > 5) throw new FormatException("Format_Bad7BitInt32");

            data[5] = (byte)count;

            return System.Runtime.InteropServices.MemoryMarshal.Read<Encoded7BitInt32>(data);
        }

        public static Encoded7BitInt32 FromSigned32(Int32 value)
        {
            var uv = (uint)value;
            RolLeft_ByRef(ref uv);
            return FromUnsigned32(uv);
        }

        public unsafe static Encoded7BitInt32 FromUnsigned32(UInt32 value)
        {
            // Write out an int 7 bits at a time.  The high bit of the byte,
            // when on, tells reader to continue reading more bytes.

            Span<Byte> data = stackalloc byte[sizeof(Encoded7BitInt32)];
            int count = 0;

            while (value >= 0x80)
            {
                data[count++] =(byte)(value | 0x80);
                value >>= 7;
            }

            data[count++] = (byte)value;

            data[5] = (byte)count;

            return System.Runtime.InteropServices.MemoryMarshal.Read<Encoded7BitInt32>(data);
        }

        #endregion

        #region data

        [System.Runtime.InteropServices.FieldOffset(0)]        
        private readonly Byte _B0;
        [System.Runtime.InteropServices.FieldOffset(1)]
        private readonly Byte _B1;
        [System.Runtime.InteropServices.FieldOffset(2)]
        private readonly Byte _B2;
        [System.Runtime.InteropServices.FieldOffset(3)]
        private readonly Byte _B3;
        [System.Runtime.InteropServices.FieldOffset(4)]
        private readonly Byte _B4;
        [System.Runtime.InteropServices.FieldOffset(5)]
        private readonly Byte _Count;

        public override int GetHashCode()
        {
            return ToUnsigned32().GetHashCode();
        }

        #endregion

        #region properties

        public byte this[int index]
        {
            get
            {
                if (index > _Count) throw new ArgumentOutOfRangeException(nameof(index));
                switch(index)
                {
                    case 0: return _B0;
                    case 1: return _B1;
                    case 2: return _B2;
                    case 3: return _B3;
                    case 4: return _B4;
                    default: throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        public int Count => _Count;

        #endregion

        #region API

        public IEnumerator<byte> GetEnumerator()
        {
            if (_Count < 1) yield break;
            yield return _B0;
            if (_Count < 2) yield break;
            yield return _B1;
            if (_Count < 3) yield break;
            yield return _B2;
            if (_Count < 4) yield break;
            yield return _B3;
            if (_Count < 5) yield break;
            yield return _B4;            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_Count < 1) yield break;
            yield return _B0;
            if (_Count < 2) yield break;
            yield return _B1;
            if (_Count < 3) yield break;
            yield return _B2;
            if (_Count < 4) yield break;
            yield return _B3;
            if (_Count < 5) yield break;
            yield return _B4;
        }

        public int ToSigned32()
        {
            var uv = ToUnsigned32();
            RolRight_ByRef(ref uv);
            return (int)uv;
        }

        public uint ToUnsigned32()
        {
            Span<Byte> data = stackalloc Byte[5];
            data[0] = _B0;
            data[1] = _B1;
            data[2] = _B2;
            data[3] = _B3;
            data[4] = _B4;

            // Read out an Int32 7 bits at a time.  The high bit
            // of the byte when on means to continue reading more bytes.
            uint count = 0;
            int shift = 0;
            uint b;
            do
            {
                // Check for a corrupted stream.  Read a max of 5 bytes.
                // In a future version, add a DataFormatException.
                if (shift == 5 * 7)  // 5 bytes max per Int32, shift += 7
                    throw new FormatException("Format_Bad7BitInt32");

                // ReadByte handles end of stream cases for us.
                b = data[0]; data = data.Slice(1);
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }

        // https://stackoverflow.com/questions/812022/c-sharp-bitwise-rotate-left-and-rotate-right
        private static void RolLeft_ByRef(ref uint ul) => ul = (ul << 1) | (ul >> 31);
        private static void RolRight_ByRef(ref uint ul) => ul = (ul >> 1) | (ul << 31);

        public void WriteTo(Action<byte> writer)
        {
            if (_Count < 1) return;
            writer(_B0);
            if (_Count < 2) return;
            writer(_B1);
            if (_Count < 3) return;
            writer(_B2);
            if (_Count < 4) return;
            writer(_B3);
            if (_Count < 5) return;
            writer(_B4);
        }

        public void WriteTo(System.IO.Stream stream)
        {
            if (_Count < 1) return;
            stream.WriteByte(_B0);
            if (_Count < 2) return;
            stream.WriteByte(_B1);
            if (_Count < 3) return;
            stream.WriteByte(_B2);
            if (_Count < 4) return;
            stream.WriteByte(_B3);
            if (_Count < 5) return;
            stream.WriteByte(_B4);
        }

        #endregion
    }
}
