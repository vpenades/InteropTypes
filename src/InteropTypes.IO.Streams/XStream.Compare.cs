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
        const int _COMPARE_SMALLBUFFERREADSIZE = 8192;

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

            // if lengths don't match,
            // files are guaranteed to be different so there's nothing else to do.

            if (TryGetLength(x, out var xfilelen) && TryGetLength(y, out var yfilelen))
            {
                System.Diagnostics.Debug.Assert(xfilelen > 0);
                System.Diagnostics.Debug.Assert(yfilelen > 0);

                if (xfilelen != yfilelen) return false;                
            }

            // if one of the two streams is a MemoryStream,
            // we can run a stream-to-stream comparison straight away:

            if (x is System.IO.MemoryStream || y is System.IO.MemoryStream)
            {
                return _CompareStreamsDirect(x, y, long.MaxValue, _COMPARE_SMALLBUFFERREADSIZE);            
            }

            // if len is small enough to fit into a MemoryStream,
            // try to preload the stream into a memorystream

            if (TryGetLength(x, out xfilelen) && xfilelen > 0 && xfilelen < int.MaxValue )
            {
                using(var mx = new System.IO.MemoryStream((int)xfilelen))
                {
                    x.CopyTo(mx);
                    mx.Position = 0;
                    return AreStreamsContentEqual(mx,y, bufferSize);
                }
            }

            if (TryGetLength(y, out yfilelen) && yfilelen > 0 && yfilelen < int.MaxValue)
            {
                using (var my = new System.IO.MemoryStream((int)yfilelen))
                {
                    y.CopyTo(my);
                    my.Position = 0;
                    return AreStreamsContentEqual(x, my, bufferSize);
                }
            }

            // we have to do direct stream-to-stream comparison:

            // compare headers for early rejection.
            if (!_CompareStreamsDirect(x, y, _COMPARE_SMALLBUFFERREADSIZE, _COMPARE_SMALLBUFFERREADSIZE)) return false;

            // compare the rest of the stream.
            return _CompareStreamsDirect(x, y, long.MaxValue, bufferSize);
        }

        /// <summary>
        /// Compares the contents of two streams until EOF or until <paramref name="maxLen"/> bytes are reached.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="maxLen"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        private static bool _CompareStreamsDirect(STREAM x, STREAM y, long maxLen = long.MaxValue, int bufferSize = 0)
        {
            // stretgies to determine default page size, specially on android and iOS, where much smaller buffers may be required
            // Environment.SystemPageSize

            if (bufferSize <= 0) bufferSize = 1024 * 1024 * 512; // 512 mb

            var buff = bufferSize > 8192 ? _TryAllocate(bufferSize) : stackalloc byte[bufferSize];

            Span<Byte> xbuff = buff.Slice(0,buff.Length/2); // 1st half
            Span<Byte> ybuff = buff.Slice(buff.Length / 2); // 2nd half

            while (maxLen > 0)
            {
                var xlen = x.TryReadBytes(xbuff);
                var ylen = y.TryReadBytes(ybuff);

                if (xlen != ylen) return false; // length mismatch
                if (xlen == 0) break; // EOF on both files

                var xslice = xbuff.Slice(0, xlen);
                var yslice = ybuff.Slice(0, xlen);

                if (!xslice.SequenceEqual(yslice)) return false;

                maxLen -= xlen;
            }

            return true;
        }

        private static Span<Byte> _TryAllocate(int bufferSize)
        {
            try
            {
                return new byte[bufferSize];
            }
            catch(System.OutOfMemoryException)
            {
                GC.Collect();
                bufferSize /= 2;
                return new Byte[bufferSize];
            }

            
        }


        // TODO: Create a LargeMemoryStream

        /// <summary>
        /// When comparing large files in memory, it is better to load large files
        /// into small array chunks to avoid adding too much pressure to the GC.
        /// </summary>
        /// <see cref="https://stackoverflow.com/questions/17921880/alternative-to-memorystream-for-large-data-volumes"/>        
        class _LargeArray : IEnumerable<Byte>
            , IEquatable<_LargeArray>
            // , IReadOnlyDictionary<long,byte>
        {
            #region lifecycle

            const int CHUNKSIZE = 65536;

            public _LargeArray(STREAM stream)
            {
                var chunk = new byte[CHUNKSIZE];

                while (true)
                {
                    var len = stream.TryReadBytes(chunk);
                    if (len == 0) break; // EOF

                    _Chunks.Add(len == chunk.Length ? chunk : chunk.AsSpan(0,len).ToArray());
                    chunk = new byte[CHUNKSIZE];
                }
            }

            public _LargeArray(long length)
            {
                while(length >= CHUNKSIZE)
                {
                    _Chunks.Add(new byte[CHUNKSIZE]);
                    length -= CHUNKSIZE;
                }

                if (length > 0) _Chunks.Add(new byte[length]);
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

            #region API

            public void Fill(STREAM stream)
            {
                foreach(var c in _Chunks)
                {
                    var len = stream.TryReadBytes(c);
                    if (len != c.Length) throw new InvalidOperationException();
                }
            }

            #endregion
        }
    }
}
