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
        private static bool TryGetAvailableSystemMemory(out int memory)
        {
            // var performance = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
            // var memory = performance.NextValue();

            memory = 0;
            return false;
        }

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

            // the strategy should be: whenever possible, try to read from one stream
            // as much as possible before reading from the other stream, to avoid the
            // hard drive head to jump as little as possible.

            long expectedLen = 0;

            if (TryGetLength(x, out var xfilelen) && TryGetLength(y, out var yfilelen))
            {
                System.Diagnostics.Debug.Assert(xfilelen > 0);
                System.Diagnostics.Debug.Assert(yfilelen > 0);

                if (xfilelen != yfilelen) return false;
                expectedLen = Math.Max(xfilelen, yfilelen);
            }

            if (expectedLen == 0) progress = null;

            return _CompareStreamsDirect(x, y, bufferSize);
        }

        private static bool _CompareStreamsDirect(STREAM x, STREAM y, int bufferSize = 0)
        {
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
        class _LargeArray : IEnumerable<Byte>
            , IEquatable<_LargeArray>
            // , IReadOnlyDictionary<long,byte>
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

            public override int GetHashCode()
            {
                return this.Count.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return obj is _LargeArray other && Equals(other);
            }

            public bool Equals(_LargeArray other)
            {
                if (other == null) return false;

                if (this.Count != other.Count) return false;

                var thisChunks = this._Chunks.Select(c => c.Length);
                var otherChunks = other._Chunks.Select(c => c.Length);

                if (thisChunks.SequenceEqual(otherChunks))
                {
                    for(int i=0; i < this._Chunks.Count; i++)
                    {
                        var thisChunk = this._Chunks[i];
                        var otherChunk = other._Chunks[i];

                        if (!thisChunk.AsSpan().SequenceEqual(otherChunk)) return false;
                    }

                    return true;
                }

                return this.SequenceEqual(other);
            }

            #endregion

            #region properties

            public byte this[long index]
            {
                get
                {
                    var chunkId = 0;
                    while(chunkId < _Chunks.Count && index >= _Chunks[chunkId].Length)
                    {
                        index -= _Chunks[chunkId].Length;
                        ++chunkId;
                    }

                    if (chunkId >= _Chunks.Count) throw new ArgumentOutOfRangeException(nameof(index));

                    return _Chunks[chunkId][index];
                }
            }            

            public long Count => _Chunks.Sum(c => (long)c.Length);

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
