using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using INDEXER = System.Int64;  // INDEXER64

namespace InteropTypes.Memory
{
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplay(),nq}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    readonly struct _LargeArrayBlock16
    {
        #region debug
        private string _GetDebuggerDisplay()
        {
            return $"{_0:02x} {_1:02x} {_2:02x}";
        }

        #endregion

        #region data

        public readonly Byte _0;
        public readonly Byte _1;
        public readonly Byte _2;
        public readonly Byte _3;
        public readonly Byte _4;
        public readonly Byte _5;
        public readonly Byte _6;
        public readonly Byte _7;
        public readonly Byte _8;
        public readonly Byte _9;
        public readonly Byte _A;
        public readonly Byte _B;
        public readonly Byte _C;
        public readonly Byte _D;
        public readonly Byte _E;
        public readonly Byte _F;

        private const int INDEXERSHIFT = 4;
        private const INDEXER INDEXERMASK = 15;

        #endregion

        #region API

        /// <summary>
        /// Gets the <see cref="_LargeArrayBlock16"/>[capacity] required to store <paramref name="byteLength"/> bytes.
        /// </summary>
        /// <param name="byteLength">the number of bytes to store</param>
        /// <returns>The capacity required to initialize the block array</returns>        
        public static int GetArrayCapacity(INDEXER byteLength)
        {
            if (byteLength < 0) throw new ArgumentOutOfRangeException(nameof(byteLength), "must not be negative");

            byteLength >>= INDEXERSHIFT;
            byteLength += 1;
            return (int)byteLength;
        }

        public static void Copy(_LargeArrayBlock16[] src, INDEXER srcIdx, _LargeArrayBlock16[] dst, INDEXER dstIdx, INDEXER byteCount)
        {
            var h = (int)(byteCount >> INDEXERSHIFT);

            if (h > 0)
            {
                var srcSlice = SliceItemsWithOverflow(src, srcIdx, byteCount).Slice(0, h);
                var dstSlice = SliceItemsWithOverflow(dst, dstIdx, byteCount).Slice(0, h);
                srcSlice.CopyTo(dstSlice);
            }

            var l = (int)(byteCount & INDEXERMASK);

            if (l >= 0) // remainder
            {
                byteCount -= l;

                var srcSlice = SliceBytes(src, srcIdx + byteCount, l);
                var dstSlice = SliceBytes(dst, dstIdx + byteCount, l);
                srcSlice.CopyTo(dstSlice);
            }
        }

        public static Span<Byte> SliceBytes(_LargeArrayBlock16[] array, INDEXER byteOffset, int byteLength)
        {
            if (byteOffset < 0) throw new ArgumentOutOfRangeException(nameof(byteOffset));
            if (byteLength < 0) throw new ArgumentOutOfRangeException(nameof(byteLength));
            if (array.Length < (byteOffset + byteLength) >> INDEXERSHIFT) throw new ArgumentOutOfRangeException(nameof(byteLength));

            ref byte bytePtr = ref GetUnsafeBytePointer(array, byteOffset);

            return System.Runtime.InteropServices.MemoryMarshal.CreateSpan(ref bytePtr, byteLength);
        }

        /// <summary>
        /// Gets a span representing the slice of requested bytes.
        /// </summary>
        /// <remarks>
        /// it may return more bytes than requested (overflow) due to the function being able to return chunks.
        /// </remarks>
        public static Span<_LargeArrayBlock16> SliceItemsWithOverflow(_LargeArrayBlock16[] array, INDEXER byteOffset, INDEXER byteLength)
        {
            if (byteOffset < 0) throw new ArgumentOutOfRangeException(nameof(byteOffset));
            if (byteLength < 0) throw new ArgumentOutOfRangeException(nameof(byteLength));

            var xlen = byteLength + (byteOffset & INDEXERMASK);

            var blockOffset = (int)(byteOffset >> INDEXERSHIFT);
            var blockLength = (int)(xlen >> INDEXERSHIFT) + (xlen & INDEXERMASK) > 0 ? 1 : 0;

            var arraySpan = array.AsSpan(blockOffset, blockLength);

            return SliceBytes(arraySpan, byteOffset & INDEXERMASK);
        }

        private static Span<_LargeArrayBlock16> SliceBytes(Span<_LargeArrayBlock16> arraySpan, INDEXER byteOffset)
        {
            if (byteOffset < 0) throw new ArgumentOutOfRangeException(nameof(byteOffset));
            
            if (byteOffset == 0) return arraySpan;

            var blockLen = arraySpan.Length - 1 - (byteOffset >> INDEXERSHIFT);
            
            byteOffset &= INDEXERSHIFT;            

            ref var bytePtr = ref System.Runtime.CompilerServices.Unsafe.As<_LargeArrayBlock16, byte>(ref arraySpan[0]); // bounds check is here
            bytePtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref bytePtr, (int)byteOffset);

            ref var blocPtr = ref System.Runtime.CompilerServices.Unsafe.As<Byte, _LargeArrayBlock16>(ref bytePtr);
            return System.Runtime.InteropServices.MemoryMarshal.CreateSpan(ref blocPtr, (int)blockLen);
        }

        public static ref byte GetUnsafeBytePointer(Span<_LargeArrayBlock16> array, INDEXER byteIndex)
        {
            // this is a critical section so avoid overloading it witch checks if possible

            var h = (int)(byteIndex >> INDEXERSHIFT);
            var l = (int)(byteIndex & INDEXERMASK);            

            ref var bytePtr = ref System.Runtime.CompilerServices.Unsafe.As<_LargeArrayBlock16, byte>(ref array[h]); // bounds check is here
            bytePtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref bytePtr, l);
            return ref bytePtr;
        }

        public static IEnumerator<byte> GetEnumerator(_LargeArrayBlock16[] array, INDEXER offset, INDEXER length)
        {
            return new _Enumerator(array, offset, length);
        }

        #endregion

        #region nested types

        internal struct _Enumerator : IEnumerator<byte>
        {
            #region lifecycle
            public _Enumerator(_LargeArrayBlock16[] chunks, INDEXER byteOffset, INDEXER byteLen)
            {
                System.Diagnostics.Debug.Assert(byteOffset >= 0);
                System.Diagnostics.Debug.Assert(byteOffset >= 0);
                if (chunks.Length >= (byteOffset + byteLen) >> INDEXERSHIFT) throw new ArgumentOutOfRangeException(nameof(byteLen));

                _Chunks = chunks;
                _ByteOffset = byteOffset;
                _ByteLen = byteLen;

                Reset();
            }

            public void Dispose() { }

            #endregion

            #region data

            private readonly _LargeArrayBlock16[] _Chunks;
            private readonly INDEXER _ByteOffset;
            private readonly INDEXER _ByteLen;

            private INDEXER _ByteIndex;

            #endregion

            #region API

            object IEnumerator.Current => Current;

            public byte Current { get; private set; }

            public void Reset()
            {
                _ByteIndex = _ByteOffset - 1;
                Current = 0;
            }

            public bool MoveNext()
            {
                _ByteIndex++;
                if (_ByteIndex >= _ByteLen) { _ByteIndex--; return false; }
                System.Diagnostics.Debug.Assert(_ByteIndex >= 0);                

                ref var bytePtr = ref _LargeArrayBlock16.GetUnsafeBytePointer(_Chunks, _ByteIndex);

                Current = bytePtr;

                return true;
            }

            #endregion
        }

        #endregion

    }
}
