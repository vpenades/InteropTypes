using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Drawing.Collection
{
    class CommandList
    {
        #region data

        private byte[] _Buffer;
        private int _Count;

        private List<object> _References;

        #endregion

        #region properties

        public int Count => _Count;

        public IReadOnlyList<object> References => _References;

        public ReadOnlySpan<byte> Buffer => _Buffer == null ? ReadOnlySpan<byte>.Empty : _Buffer.AsSpan(0, _Count);

        #endregion

        #region API

        public void Clear() { _Count = 0; }

        public void Set(CommandList other)
        {
            if (other._Buffer == null) { _Buffer = null; _Count = 0; }
            else
            {
                Array.Resize(ref _Buffer, other._Buffer.Length);
                other._Buffer.CopyTo(_Buffer, 0);
                _Count = other._Count;
            }

            if (other._References == null) { _References = null; }
            else
            {
                _References.Clear();
                _References.AddRange(other._References);
            }
        }

        public IEnumerable<int> GetCommands()
        {
            int offset = 0;

            while (offset < Count)
            {
                var length = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(_Buffer.AsSpan(offset, 4));
                yield return offset;
                offset += length + 4;
            }
        }

        public Span<byte> AddChunk(int byteSize)
        {
            if ((_Buffer?.Length ?? 0) - _Count < byteSize)
            {
                var newLen = (_Count + byteSize) * 100 / 61;
                Array.Resize(ref _Buffer, newLen);
            }

            var span = _Buffer.AsSpan(_Count, byteSize);
            _Count += byteSize;
            return span;
        }

        public Span<T> AddChunk<T>(int byteSize)
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(AddChunk(byteSize));
        }

        public void AddHeader(int type, int length)
        {
            var hdr = AddChunk<int>(8);
            hdr[0] = length;
            hdr[1] = type;
        }

        public unsafe Span<T> AddStruct<T>()
            where T : unmanaged
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(AddChunk(sizeof(T)));
        }

        public unsafe Span<T> AddHeaderAndStruct<T>(int headerType)
            where T : unmanaged
        {
            var chunk = AddChunk(sizeof(T) + 8);
            var header = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(chunk.Slice(0, 8));
            header[0] = 4 + sizeof(T);
            header[1] = headerType;
            return System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(chunk.Slice(8));
        }

        public void AddValue(int value)
        {
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(AddChunk(4), value);
        }

        public void AddArray(ReadOnlySpan<System.Numerics.Vector2> array)
        {
            var b = AddChunk<System.Numerics.Vector2>(8 * array.Length);
            array.CopyTo(b);
        }

        public void AddArray(ReadOnlySpan<System.Numerics.Vector3> array)
        {
            var b = AddChunk<System.Numerics.Vector3>(12 * array.Length);
            array.CopyTo(b);
        }

        public void AddArray(ReadOnlySpan<Point2> array)
        {
            var b = AddChunk<Point2>(8 * array.Length);
            array.CopyTo(b);
        }

        public void AddArray(ReadOnlySpan<Point3> array)
        {
            var b = AddChunk<Point3>(12 * array.Length);
            array.CopyTo(b);
        }



        public int AddReference(object o)
        {
            if (_References == null) _References = new List<object>();

            _References.Add(o);
            return _References.Count - 1;
        }

        public int GetContentHashCode()
        {
            var buffer = Buffer;
            var ints = buffer.Slice(0, buffer.Length & ~3);
            if (ints.IsEmpty) return 0;
            int h = 0;
            foreach (var item in ints) { h += item; h *= 17; }
            return h;
        }

        #endregion
    }
}
