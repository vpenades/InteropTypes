using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Crypto
{
    public interface IHashValue : IEquatable<IHashValue>
    {
        int ByteCount { get; }
        Byte this[int index] { get; }

        void CopyTo(Span<Byte> target);
    }
}
