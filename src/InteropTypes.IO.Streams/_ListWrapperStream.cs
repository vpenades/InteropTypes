using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.IO
{
    /// <summary>
    /// Wraps an existing <see cref="List{T}"/> of bytes (both readable and writeable) and exposes it as a Stream
    /// </summary>
    /// <typeparam name="TList">A list of bytes</typeparam>
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplay(),nq}")]
    class _ListWrapperStream<TList> : System.IO.Stream
        where TList : IReadOnlyList<Byte>
    {
        #region diagnostics

        internal string ToDebuggerDisplay()
        {
            if (_Reader == null || _Writer == null) return "DISPOSED";

            var data = string.Empty;

            for(var i= Math.Max(0, Position -3); i < Math.Min(Length,Position+3); ++i)
            {
                if (data.Length > 0) data += " ";

                var c = _Reader[(int)i].ToString("X2");
                if (i == Position) c= "["+c+"]";

                data += c;                
            }

            return $"[0..[{Position}]..{Length}] => " + data;
        }

        #endregion

        #region lifecycle        

        public static _ListWrapperStream<TList> Open(TList list, FileMode mode)
        {
            if (mode == FileMode.Create || mode == FileMode.CreateNew || mode == FileMode.Truncate)
            {
                if (list is IList<Byte> wlist) wlist.Clear();
            }

            #pragma warning disable CA2000 // Dispose objects before losing scope

            _ListWrapperStream<TList> listStream = null;

            #if !NETSTANDARD
            if (list is List<Byte> rwlist) listStream = new _ListWrapperStreamNet6(rwlist) as _ListWrapperStream<TList>;
            #endif

            listStream ??= new _ListWrapperStream<TList>(list);
            
            if (mode == FileMode.Append)
            {
                listStream.Position = listStream.Length;
            }

            return listStream;

            #pragma warning restore CA2000 // Dispose objects before losing scope
        }

        protected _ListWrapperStream(TList list)
        {
            _Reader = list;
            _Writer = list as IList<Byte>;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _Reader = default;
                _Writer = default;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        protected readonly Object _Mutex = new object();

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        protected int _Position;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        protected TList _Reader;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        protected IList<Byte> _Writer;

        #endregion

        #region properties

        public override long Length => _Reader?.Count ?? throw new ObjectDisposedException("List");

        public override long Position
        {
            get
            {
                if (_Reader == null) throw new ObjectDisposedException("List");
                return _Position;
            }

            set
            {
                if (_Reader == null) throw new ObjectDisposedException("List");
                lock (_Mutex)
                {
                    if (value < 0 || value >= _Reader.Count) throw new ArgumentOutOfRangeException(nameof(value));
                    _Position = (int)value;
                }
            }
        }

        public override bool CanRead => _Reader != null;
        public override bool CanSeek => true;
        public override bool CanWrite => _Writer != null;

        #endregion

        #region API

        protected void _GuardDisposed()
        {
            if (_Reader == null || _Writer == null) throw new ObjectDisposedException("List");
        }

        public override void Flush() { _GuardDisposed(); }

        public override long Seek(long offset, SeekOrigin origin)
        {
            _GuardDisposed();            

            switch (origin)
            {
                case SeekOrigin.Begin: Position = offset; break;
                case SeekOrigin.End: Position = Length - offset; break;
                case SeekOrigin.Current: Position += offset; break;
            }

            return Position;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_Reader == null) throw new NotSupportedException();

            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            lock (_Mutex)
            {
                int remaining = _Reader.Count - _Position;
                if (remaining == 0) return 0;
                if (remaining < 0) throw new EndOfStreamException();

                count = Math.Min(count, remaining);

                for (int i = 0; i < count; i++)
                {
                    buffer[offset + i] = _Reader[_Position + i];
                }

                _Position += count;

                return count;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_Writer == null) throw new NotSupportedException();

            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            lock (_Mutex)
            {
                for (int i = 0; i < count; i++)
                {
                    var val = buffer[offset + i];

                    // insert
                    if (_Position < _Writer.Count) _Writer[i] = val;
                    else _Writer.Add(val);

                    ++_Position;
                }
            }
        }        

        public override void SetLength(long value)
        {
            if (_Writer == null) throw new NotSupportedException();

            if (value > int.MaxValue) throw new ArgumentOutOfRangeException(nameof(value));

            lock (_Mutex)
            {
                var len = (int)value;

                while (_Writer.Count < len) _Writer.Add(0);
                while (_Writer.Count > len) _Writer.RemoveAt(_Writer.Count - 1);

                if (_Position >= _Writer.Count) _Position = _Writer.Count;
            }
        }        

        #endregion
    }

    /// <summary>
    /// Specialised <see cref="List{T}"/> wrapper stream using Net6+ CollectionsMarshal
    /// </summary>
    #if !NETSTANDARD
    [System.Diagnostics.DebuggerDisplay("{ToDebuggerDisplay(),nq}")]
    class _ListWrapperStreamNet6 : _ListWrapperStream<List<Byte>>
    {
        public _ListWrapperStreamNet6(List<byte> list) : base(list)
        {
            
        }
        
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            var span = buffer.AsSpan(offset, count);
            return Read(span);
        }

        public override int ReadByte()
        {
            Span<byte> buffer = stackalloc byte[1];
            int count = Read(buffer);
            return count == 1 ? buffer[0] : -1;
        }

        public override int Read(Span<byte> target)
        {
            if (_Reader == null) throw new NotSupportedException();

            lock (_Mutex)
            {
                int remaining = _Reader.Count - _Position;
                if (remaining == 0) return 0;
                if (remaining < 0) throw new EndOfStreamException();

                var source = System.Runtime.InteropServices.CollectionsMarshal
                    .AsSpan(_Reader)
                    .Slice(_Position, Math.Min(target.Length, remaining));

                source.CopyTo(target);

                _Position += source.Length;

                return source.Length;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            var span = buffer.AsSpan(offset, count);
            Write(span);
        }

        public override void WriteByte(byte value)
        {
            Span<byte> buffer = stackalloc byte[1];
            buffer[0] = value;
            Write(buffer);
        }

        public override void Write(ReadOnlySpan<Byte> source)
        {
            // _Reader is a full List<Byte> so we don't need _Writer
            if (_Reader == null) throw new NotSupportedException();

            lock (_Mutex)
            {
                int growth = _Position + source.Length - _Reader.Count;

                while (growth > 0)
                {
                    _Reader.Add(0);
                    growth--;
                }

                var target = System.Runtime.InteropServices.CollectionsMarshal
                    .AsSpan(_Reader)
                    .Slice(_Position, source.Length);

                source.CopyTo(target);

                _Position += target.Length;
            }
        }
    }
    #endif

}
