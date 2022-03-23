using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;

namespace InteropTypes.Graphics.Drawing.Collection
{
    class CommandList
    {
        #region lifecycle

        public CommandList() { }

        public CommandList(CommandList other)
        {
            this.Set(other);
        }

        #endregion

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

        #region API - Read

        public IEnumerable<int> GetCommands()
        {
            int offset = 0;

            while (offset < Count)
            {
                var length = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(_Buffer.AsSpan(offset, 4));

                System.Diagnostics.Debug.Assert(length > 8);

                yield return offset;
                offset += length + 4;
            }
        }

        #endregion

        #region API - Write

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

        private Span<byte> _AddChunk(int byteSize)
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

        public unsafe Span<T> AddHeaderAndStruct<T>(int headerType)
            where T : unmanaged
        {
            var chunk = _AddChunk(8 + sizeof(T));
            var header = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(chunk.Slice(0, 8));
            header[0] = 4 + sizeof(T);
            header[1] = headerType;

            chunk = chunk.Slice(8);

            return System.Runtime.InteropServices.MemoryMarshal.Cast<byte, T>(chunk);
        }

        public unsafe Span<T> AddHeaderAndStruct<T>(int headerType, ReadOnlySpan<Point2> points)
            where T : unmanaged
        {
            return AddHeaderAndStruct<T, Point2>(headerType, points);
        }

        public unsafe Span<T> AddHeaderAndStruct<T>(int headerType, ReadOnlySpan<Point3> points)
            where T : unmanaged
        {
            return AddHeaderAndStruct<T,Point3>(headerType, points);
        }


        private unsafe Span<THeader> AddHeaderAndStruct<THeader, TPoint>(int headerType, ReadOnlySpan<TPoint> points)
            where THeader : unmanaged
            where TPoint : unmanaged
        {
            var len = 8 + sizeof(THeader) + points.Length * sizeof(TPoint);

            var chunk = _AddChunk(len);

            var header = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, int>(chunk.Slice(0, 8));
            header[0] = len - 4;
            header[1] = headerType;

            chunk = chunk.Slice(8);

            var dst = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, TPoint>(chunk.Slice(sizeof(THeader)));
            points.CopyTo(dst);

            return System.Runtime.InteropServices.MemoryMarshal.Cast<byte, THeader>(chunk);
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
