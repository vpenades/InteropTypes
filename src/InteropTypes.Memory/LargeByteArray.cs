using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using INDEXER = System.Int64;  // INDEXER64
using LBAITEM = InteropTypes.Memory._LargeArrayBlock16; // supports up to 16gb , we could also use Vector2 and Vector4 for 16GB and 32GB

// another way would be to use a LargeStruct<T> { T A0; T A1, T A2, T, A3 ... } so it essentially would make a buffer 8x larger

// but in practice, it only matters to collections of Byte and Short, since collections o items of 4 bytes or more would be already impossible to handle

namespace InteropTypes.Memory
{
    public interface IReadOnlyLargeList<T> : IReadOnlyList<T>
    {
        T this[INDEXER index] { get; }
        new INDEXER Count { get; }        
    }


    /// <summary>
    /// Represents a byte array than can store up to 16gb of continuous managed memory
    /// </summary>
    /// <remarks>
    /// Although theoretically, you can allocate up to 16gb, in practice this collection is designed
    /// to overcome the 2gb limit of Byte[], so on real scenarios the allocation sizes should try to
    /// stay below 4GB
    /// </remarks>
    public sealed class LargeByteArray : IEnumerable<Byte>
    {
        #region lifecycle
        public LargeByteArray(INDEXER length = 0)
        {
            var capacity = _LargeArrayBlock16.GetArrayCapacity(length);
            System.Diagnostics.Debug.Assert(capacity >= 0);
            capacity++;

            _Buffer = new LBAITEM[capacity];
            _ByteLen = length;
        }

        public LargeByteArray(Byte[] array)
        {
            var capacity = _LargeArrayBlock16.GetArrayCapacity(array.Length);

            _Buffer = new LBAITEM[capacity];
            _ByteLen = array.Length;

            var dst = System.Runtime.InteropServices.MemoryMarshal.Cast<LBAITEM, byte>(_Buffer);
            array.AsSpan().CopyTo(dst);
        }

        #endregion

        #region data        

        internal LBAITEM[] _Buffer; // always allocate capacity +1 to have a tail that can simplify calculations
        private INDEXER _ByteLen;

        private ref byte _GetPtr() => ref _LargeArrayBlock16.GetUnsafeBytePointer(_Buffer, 0);

        private ref byte _GetPtr(INDEXER index) => ref _LargeArrayBlock16.GetUnsafeBytePointer(_Buffer, index);

        #endregion

        #region API

        public INDEXER Length => _ByteLen;

        public byte this[INDEXER index]
        {
            get
            {
                return _GetPtr(index);
            }
            set
            {
                ref byte bytePtr = ref _GetPtr(index);

                bytePtr = value;
            }
        }        

        public LargeByteSegment Slice(INDEXER offset, INDEXER count)
        {
            return new LargeByteSegment(this, offset, count);
        }

        public LargeByteSegment Slice(INDEXER offset)
        {
            return new LargeByteSegment(this, offset, _ByteLen - offset);
        }

        public static LargeByteArray Merge(IReadOnlyList<object> buffers)
        {
            // calculate final length
            INDEXER length = 0;

            foreach (var item in buffers)
            {
                switch(item)
                {
                    case null: break;
                    case LargeByteSegment lbs: length += lbs.Count; break;
                    case LargeByteArray lba: length += lba.Length; break;
                    case byte[] array: length += array.Length; break;
                    default: throw new NotImplementedException(item.GetType().Name);
                }
            }

            var dst = new LargeByteArray(length);            
            
            INDEXER offset = 0;            

            foreach (var item in buffers)
            {
                switch (item)
                {
                    case null: break;
                    case LargeByteSegment lbs:
                        {
                            var dstSlice = dst.Slice(offset, lbs.Count);
                            lbs.CopyTo(dstSlice);

                            offset += lbs.Count;
                            break;
                        }
                    case LargeByteArray lba:
                        {
                            var dstSlice = dst.Slice(offset, lba.Length);
                            lba.Slice(0).CopyTo(dstSlice);

                            offset += lba.Length;
                            break;
                        }
                    case byte[] array:
                        {
                            var dstSpan = _LargeArrayBlock16.SliceBytes(dst._Buffer, offset, array.Length);
                            array.AsSpan().CopyTo(dstSpan);

                            offset += array.Length;
                            break;
                        }
                    default: throw new NotImplementedException(item.GetType().Name);
                }
            }            

            return dst;
        }        

        public IEnumerator<byte> GetEnumerator()
        {
            return _LargeArrayBlock16.GetEnumerator(_Buffer, 0, _ByteLen);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }        

        #endregion
    }
    
    /// <summary>
    /// Represents an array segment over a <see cref="LargeByteArray"/>
    /// </summary>
    public readonly struct LargeByteSegment : IList<Byte>, IReadOnlyLargeList<Byte>    
    {
        #region lifecycle

        public static LargeByteSegment Empty { get; } = new LargeByteSegment(new LargeByteArray(0),0,0);

        public LargeByteSegment(LargeByteArray array, INDEXER offset, INDEXER count)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset),"must be non negative");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "must be non negative");

            _array = array;
            _offset = offset;
            _count = count;

            _ShortCount = _count <= int.MaxValue ? (int)_count : -1;
        }

        #endregion

        #region data

        private readonly LargeByteArray _array; // Do not rename (binary serialization)
        private readonly INDEXER _offset; // Do not rename (binary serialization)
        private readonly INDEXER _count; // Do not rename (binary serialization)        

        private readonly int _ShortCount;

        private ref byte _GetPtr() => ref _LargeArrayBlock16.GetUnsafeBytePointer(_array._Buffer, _offset);

        private ref byte _GetPtr(INDEXER index) => ref _LargeArrayBlock16.GetUnsafeBytePointer(_array._Buffer, _offset + index);

        public override int GetHashCode()
        {
            return _array is null ? 0 : HashCode.Combine(_offset, _count, _array.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj is LargeByteSegment other && Equals(other);
        }

        public bool Equals(LargeByteSegment obj)
        {
            return obj._array == _array && obj._offset == _offset && obj._count == _count;
        }

        public static bool operator ==(LargeByteSegment a, LargeByteSegment b) => a.Equals(b);

        public static bool operator !=(LargeByteSegment a, LargeByteSegment b) => !(a == b);

        #endregion

        #region API

        bool ICollection<byte>.IsReadOnly => true; // from ArraySegment<Byte>

        int IReadOnlyCollection<byte>.Count => _ShortCount;

        int ICollection<byte>.Count => _ShortCount;

        public INDEXER Count => _count;

        public LargeByteArray Array => _array;

        public INDEXER Offset => _offset;        

        public Byte this[INDEXER index]
        {
            get
            {
                VerifyIndexInRange(index);

                return _GetPtr(index);
            }
            set
            {
                VerifyIndexInRange(index);

                ref var ptr = ref _GetPtr(index);

                ptr = value;
            }
        }          

        public LargeByteSegment Slice(INDEXER index)
        {            
            VerifyIndexInRange(index);

            return new LargeByteSegment(_array!, _offset + index, _count - index);
        }

        public LargeByteSegment Slice(INDEXER index, INDEXER count)
        {            
            VerifyIndexInRange(index);

            return new LargeByteSegment(_array!, _offset + index, count);
        }

        public INDEXER IndexOf(Byte value)
        {
            ThrowInvalidOperationIfDefault();

            ref var ptr = ref _GetPtr();

            INDEXER idx = 0;
            INDEXER cnt = _count;

            while(cnt > 0)
            {
                if (ptr == value) return idx;
                ++idx;
                --cnt;
                ptr = ref Unsafe.Add(ref ptr, 1);
            }

            return -1;
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            ThrowIfCountTooBig();

            var src = _LargeArrayBlock16.SliceBytes(_array._Buffer, _offset, (int)_count);
            var dst = array.AsSpan(arrayIndex);

            src.CopyTo(dst);
        }

        public void CopyTo(LargeByteSegment dst)
        {
            var l = Math.Min(this.Count, dst.Count);
        }

        public Byte[] ToArray()
        {
            ThrowIfCountTooBig();

            var src = _LargeArrayBlock16.SliceBytes(_array._Buffer, _offset, (int)_count);

            return src.ToArray();
        }

        public void Fill(Byte value)
        {
            ThrowInvalidOperationIfDefault();

            ref var ptr = ref _GetPtr();
            
            INDEXER cnt = _count;

            while (cnt > 0)
            {
                ptr = value;
                
                --cnt;
                ptr = ref Unsafe.Add(ref ptr, 1);
            }
        }

        internal Span<LBAITEM> GetItems()
        {
            ThrowInvalidOperationIfDefault();

            return _LargeArrayBlock16.SliceItemsWithOverflow(_array._Buffer, _offset, _count);
        }

        #endregion

        #region interfaces

        byte IList<byte>.this[int index]
        {
            get { ThrowIfCountTooBig(); return this[index]; }
            set { ThrowIfCountTooBig(); this[index] = value; }
        }

        byte IReadOnlyList<byte>.this[int index]
        {
            get { ThrowIfCountTooBig(); return this[index]; }
        }

        bool ICollection<byte>.Contains(byte item)
        {
            ThrowInvalidOperationIfDefault();

            return IndexOf(item) >= 0;
        }

        int IList<byte>.IndexOf(byte item)
        {
            ThrowIfCountTooBig();

            return (int)IndexOf(item);
        }

        IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
        {
            return _LargeArrayBlock16.GetEnumerator(_array._Buffer, _offset, _count);
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<byte>)this).GetEnumerator();

        void IList<byte>.Insert(int index, byte item) => throw new NotSupportedException();

        void IList<byte>.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection<byte>.Add(byte item) => throw new NotSupportedException();

        void ICollection<byte>.Clear() => throw new NotSupportedException();

        bool ICollection<byte>.Remove(byte item) => throw new NotSupportedException();

        #endregion

        #region validations

        [System.Diagnostics.DebuggerStepThrough]
        private void VerifyIndexInRange(INDEXER index)
        {
            if ((UInt64)index >= (UInt64)_count) throw new IndexOutOfRangeException(nameof(index));
        }

        [System.Diagnostics.DebuggerStepThrough]
        private void ThrowInvalidOperationIfDefault()
        {
            if (_array == null) throw new InvalidOperationException("LargeByteArray is null");
        }

        [System.Diagnostics.DebuggerStepThrough]
        private void ThrowIfCountTooBig()
        {
            if (_count > int.MaxValue) throw new InvalidOperationException("too big");
        }        

        #endregion
    }


    
}
