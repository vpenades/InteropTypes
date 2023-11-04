using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.IO
{
    /// <summary>
    /// This is an in-memory stream to simulate a very large stream of random data.
    /// </summary>
    class RandomStream : System.IO.Stream
    {
        #region lifecycle
        public RandomStream(long len, int seed)
        {
            _Len = len;
            _Seed = seed;
            _Rnd = new Random(seed);
        }

        #endregion

        #region data

        private readonly int _Seed;
        private readonly long _Len;
        private long _Pos;

        private Random _Rnd = new Random();

        #endregion

        #region API

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => _Len;

        public override long Position
        {
            get => _Pos;
            set
            {
                _Pos = value;
                _Rnd = new Random(_Seed);
                for (int i = 0; i < _Pos; ++i) _Rnd.Next();
            }
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var len = (int)Math.Min(count, _Len - _Pos);

            _Rnd.NextBytes(buffer.AsSpan(offset, len));

            _Pos += len;

            return len;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
