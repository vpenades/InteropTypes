using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InteropTypes.IO
{
    /// <summary>
    /// A stream that invokes an action when it is closed.
    /// </summary>    
    internal class _CloseActionStream : System.IO.Stream
    {
        #region lifecycle

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="stream">the underlaying stream</param>
        /// <param name="leaveOpen">whether to keep the <paramref name="stream"/> open after closing.</param>
        /// <param name="onClosing">action called before closing the stream, or null</param>
        /// <param name="onClosed">action called after closing the stream, or null</param>
        public _CloseActionStream(System.IO.Stream stream, bool leaveOpen, Func<System.IO.Stream, bool> onClosing, Action<long> onClosed)
        {
            _Stream = stream;
            _LeaveOpen = leaveOpen;
            _OnClosingStream = onClosing;
            _OnClosedStream = onClosed;
        }

        public override void Close()
        {
            if (_Stream != null && _OnClosingStream != null)
            {
                if (!_OnClosingStream.Invoke(_Stream)) return;
            }

            _OnClosingStream = null;

            XStream.TryGetLength(_Stream, out var length);
            if (!_LeaveOpen) _Stream?.Dispose();
            _Stream = null;

            base.Close();            

            _OnClosedStream?.Invoke(length);
            _OnClosedStream = null;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]        
        private readonly bool _LeaveOpen;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private System.IO.Stream _Stream;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Func<System.IO.Stream, bool> _OnClosingStream;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Action<long> _OnClosedStream;

        #endregion

        #region properties

        public System.IO.Stream BaseStream => _Stream ?? throw new ObjectDisposedException("BaseStream");

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => BaseStream.CanWrite;

        public override long Length => BaseStream.Length;

        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        #endregion

        #region API

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        #endregion
    }
}
