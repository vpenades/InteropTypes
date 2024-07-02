using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    struct Crc32
    {
        #region constants
        private static readonly uint[] CrcTable = new uint[256];

        // Initialize the CRC table for quick lookup
        static Crc32()
        {
            const uint polynomial = 0xEDB88320;
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i;
                for (uint j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ polynomial;
                    else
                        crc >>= 1;
                }
                CrcTable[i] = crc;
            }
        }

        #endregion

        public static Crc32 Create()
        {
            var crc = new Crc32();
            crc.Reset();
            return crc;
        }

        #region data

        private uint _crc;

        public uint Value => ~_crc;

        #endregion

        public static uint ComputeChecksum<T>(Span<T> input) where T:unmanaged
        {
            var crc = Create();
            crc.AppendChecksum(input);
            return crc.Value;
        }

        public static uint ComputeChecksum<T>(ReadOnlySpan<T> input) where T : unmanaged
        {
            var crc = Create();
            crc.AppendChecksum(input);
            return crc.Value;
        }

        public void Reset()
        {
            _crc = 0xFFFFFFFF;
        }

        public void AppendChecksum<T>(T value) where T: unmanaged
        {
            Span<T> tmp = stackalloc T[1];
            tmp[0] = value;
            AppendChecksum(tmp);
        }

        public void AppendChecksum<T>(Span<T> input)
            where T:unmanaged
        {
            var bytes = System.Runtime.InteropServices.MemoryMarshal.Cast<T, Byte>(input);
            _AppendChecksum(bytes);
        }

        public void AppendChecksum<T>(ReadOnlySpan<T> input)
            where T : unmanaged
        {
            var bytes = System.Runtime.InteropServices.MemoryMarshal.Cast<T, Byte>(input);
            _AppendChecksum(bytes);
        }


        // Calculate the CRC32 checksum for a byte array
        private void _AppendChecksum(ReadOnlySpan<Byte> bytes)
        {            
            for (int i = 0; i < bytes.Length; i++)
            {
                byte index = (byte)(((_crc) & 0xFF) ^ bytes[i]);
                _crc = (_crc >> 8) ^ CrcTable[index];
            }            
        }
    }
}
