using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Crypto
{
    public sealed class HashValue : IHashValue
    {
        internal IHashValue _Hash;

        public byte this[int index] => _Hash?[index] ?? 0;

        public int ByteCount => _Hash?.ByteCount ?? 0;        

        public bool Equals(IHashValue other)
        {
            if (_Hash == null && other == null) return true;
            return _Hash?.Equals(other) ?? false;
        }

        public void CopyTo(Span<byte> target)
        {
            _Hash?.CopyTo(target);
        }
    }
}
