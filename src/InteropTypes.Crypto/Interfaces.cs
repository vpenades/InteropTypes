using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Crypto
{
    public interface IHashValue : IEquatable<IHashValue>
    {
        /// <summary>
        /// Gets the number of bytes used by this hash value.
        /// </summary>
        int ByteCount { get; }

        /// <summary>
        /// Gets the byte value at the given index.
        /// </summary>
        /// <param name="index">The index to check</param>
        /// <returns>A byte value.</returns>
        Byte this[int index] { get; }

        /// <summary>
        /// Copies the hash bytes to the target.
        /// </summary>
        /// <param name="target">the destination to where the bytes are copied to.</param>
        void CopyTo(Span<Byte> target);
    }
}
