using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using STREAM = System.IO.Stream;

namespace InteropTypes.IO
{
    partial class XStream
    {
        public static bool AreStreamsContentEqual(this Func<STREAM> a, Func<STREAM> b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            using (var sa = a.Invoke())
            {
                using (var sb = b.Invoke())
                {
                    return AreStreamsContentEqual(sa, sb);
                }
            }
        }

        /// <summary>
        /// Compares the contents of two streams for equality
        /// </summary>
        /// <param name="x">the 1st stream</param>
        /// <param name="y">the 2nd stream</param>
        /// <returns>true if the content of both streams is equal</returns>
        public static bool AreStreamsContentEqual(STREAM x, STREAM y, int bufferSize = 0, IProgress<int> progress = null)
        {
            if (x == y) return true;
            if (x == null) return false;
            if (y == null) return false;

            GuardReadable(x);
            GuardReadable(y);

            long flen = 0;

            if (TryGetLength(x, out var xfilelen) && TryGetLength(y, out var yfilelen))
            {
                flen = Math.Max(xfilelen, yfilelen);
            }
            else
            {
                progress = null;
            }

            if (bufferSize <= 0) bufferSize = 1024 * 1024 * 64; // 64 mb

            Span<Byte> xbuff = new byte[bufferSize];
            Span<Byte> ybuff = new byte[bufferSize];

            while (true)
            {
                var xlen = x.TryReadBytes(xbuff);
                var ylen = y.TryReadBytes(ybuff);

                if (xlen != ylen) return false; // length mismatch
                if (xlen == 0) break; // EOF on both files

                var xslice = xbuff.Slice(0, xlen);
                var yslice = ybuff.Slice(0, xlen);

                if (!xslice.SequenceEqual(yslice)) return false;
            }

            return true;
        }


        /// <summary>
        /// When comparing large files in memory, it is better to load large files
        /// into small array chunks to avoid adding too much pressure to the GC.
        /// </summary>
        class _LargeArray : IReadOnlyList<Byte>
        {
            #region lifecycle
            public _LargeArray(STREAM stream)
            {
                var chunk = new byte[65536];

                while (true)
                {
                    var len = stream.TryReadBytes(chunk);
                    if (len == 0) break; // EOF

                    _Chunks.Add(len == chunk.Length ? chunk : chunk.AsSpan(0,len).ToArray());
                    chunk = new byte[65536];
                }
            }

            #endregion

            #region data

            private readonly List<Byte[]> _Chunks = new List<byte[]>();

            #endregion

            #region properties

            public byte this[int index]
            {
                get
                {
                    var chunkId = 0;
                    while(index >= _Chunks[chunkId].Length)
                    {
                        index -= _Chunks[chunkId].Length;
                        ++chunkId;
                    }

                    return _Chunks[chunkId][index];
                }
            }

            public int Count => _Chunks.Sum(c=>c.Length);

            public IEnumerator<byte> GetEnumerator()
            {
                return _Chunks.SelectMany(item => item).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _Chunks.SelectMany(item => item).GetEnumerator();
            }

            #endregion
        }
    }
}
