using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using STREAM = System.IO.Stream;
using MEMSTREAM = System.IO.MemoryStream;

namespace InteropTypes.IO
{
    /// <summary>
    /// Context used to compare two streams
    /// </summary>
    /// <remarks>
    /// Use <see cref="StreamEqualityComparer.Default"/> as API entry point.
    /// </remarks>
    public class StreamEqualityComparer
    {
        #region static

        public static StreamEqualityComparer Default { get; } = new RecyclableContext();

        #endregion

        #region properties

        const int _COMPARE_BUFFERREADSIZE_SMALL = 8192;
        const int _COMPARE_BUFFERREADSIZE_LARGE = 1024 * 1024 * 512; // 512 mb;

        public int BufferSize { get; set; } = _COMPARE_BUFFERREADSIZE_LARGE;

        #endregion

        #region API

        private static bool TryGetAvailableSystemMemory(out int memory)
        {
            // var performance = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
            // var memory = performance.NextValue();

            memory = 0;
            return false;
        }

        public bool AreStreamsContentEqual(Func<STREAM> a, Func<STREAM> b)
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

        public async ValueTask<bool> AreStreamsContentEqualAsync(Func<STREAM> a, Func<STREAM> b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));

            using (var sa = a.Invoke())
            {
                using (var sb = b.Invoke())
                {
                    return await AreStreamsContentEqualAsync(sa, sb).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Compares the contents of two streams for equality
        /// </summary>
        /// <param name="x">the 1st stream</param>
        /// <param name="y">the 2nd stream</param>
        /// <returns>true if the content of both streams is equal</returns>
        public bool AreStreamsContentEqual(STREAM x, STREAM y)
        {
            if (x == y) return true;
            if (x == null) return false;
            if (y == null) return false;

            XStream.GuardReadable(x);
            XStream.GuardReadable(y);

            // if lengths don't match,
            // files are guaranteed to be different so there's nothing else to do.

            if (XStream.TryGetLength(x, out var xfilelen) && XStream.TryGetLength(y, out var yfilelen))
            {
                System.Diagnostics.Debug.Assert(xfilelen > 0);
                System.Diagnostics.Debug.Assert(yfilelen > 0);

                if (xfilelen != yfilelen) return false;                
            }

            // if one of the two streams is a MemoryStream,
            // we can run a stream-to-stream comparison straight away:

            if (x is MEMSTREAM || y is MEMSTREAM)
            {
                return _CompareStreamsDirect(x, y, long.MaxValue, _COMPARE_BUFFERREADSIZE_SMALL);            
            }

            // if len is small enough to fit into a MemoryStream,
            // try to preload the stream into a memorystream

            if (XStream.TryGetLength(x, out xfilelen) && xfilelen > 0)
            {
                using(var mx = _CreateMemoryStream(xfilelen))
                {
                    if (mx != null)
                    {
                        x.CopyTo(mx);
                        mx.Position = 0;
                        return AreStreamsContentEqual(mx, y);
                    }
                }
            }

            if (XStream.TryGetLength(y, out yfilelen) && yfilelen > 0)
            {
                using (var my = _CreateMemoryStream(yfilelen))
                {
                    if (my != null)
                    {
                        y.CopyTo(my);
                        my.Position = 0;
                        return AreStreamsContentEqual(x, my);
                    }
                }
            }

            // we have to do direct stream-to-stream comparison:

            // compare headers for early rejection.
            if (!_CompareStreamsDirect(x, y, _COMPARE_BUFFERREADSIZE_SMALL, _COMPARE_BUFFERREADSIZE_SMALL)) return false;

            // compare the rest of the stream.
            return _CompareStreamsDirect(x, y, long.MaxValue, this.BufferSize);
        }

        public async ValueTask<bool> AreStreamsContentEqualAsync(STREAM x, STREAM y)
        {
            if (x == y) return true;
            if (x == null) return false;
            if (y == null) return false;

            XStream.GuardReadable(x);
            XStream.GuardReadable(y);

            // if lengths don't match,
            // files are guaranteed to be different so there's nothing else to do.

            if (XStream.TryGetLength(x, out var xfilelen) && XStream.TryGetLength(y, out var yfilelen))
            {
                System.Diagnostics.Debug.Assert(xfilelen > 0);
                System.Diagnostics.Debug.Assert(yfilelen > 0);

                if (xfilelen != yfilelen) return false;
            }

            // if one of the two streams is a MemoryStream,
            // we can run a stream-to-stream comparison straight away:

            if (x is System.IO.MemoryStream || y is System.IO.MemoryStream)
            {
                return await _CompareStreamsDirectAsync(x, y, long.MaxValue, _COMPARE_BUFFERREADSIZE_SMALL).ConfigureAwait(false);
            }

            // if len is small enough to fit into a MemoryStream,
            // try to preload the stream into a memorystream

            if (XStream.TryGetLength(x, out xfilelen) && xfilelen > 0 && xfilelen < int.MaxValue)
            {
                using (var mx = new System.IO.MemoryStream((int)xfilelen))
                {
                    await x.CopyToAsync(mx).ConfigureAwait(false);
                    mx.Position = 0;
                    return await AreStreamsContentEqualAsync(mx, y).ConfigureAwait(false);
                }
            }

            if (XStream.TryGetLength(y, out yfilelen) && yfilelen > 0 && yfilelen < int.MaxValue)
            {
                using (var my = new System.IO.MemoryStream((int)yfilelen))
                {
                    await y.CopyToAsync(my).ConfigureAwait(false);
                    my.Position = 0;
                    return await AreStreamsContentEqualAsync(x, my).ConfigureAwait(false);
                }
            }

            // we have to do direct stream-to-stream comparison:

            // compare headers for early rejection.
            var r = await _CompareStreamsDirectAsync(x, y, _COMPARE_BUFFERREADSIZE_SMALL, _COMPARE_BUFFERREADSIZE_SMALL).ConfigureAwait(false);
            if (r!) return false;

            // compare the rest of the stream.
            return await _CompareStreamsDirectAsync(x, y, long.MaxValue, this.BufferSize).ConfigureAwait(false);
        }


        private MEMSTREAM _CreateMemoryStream(long capacity)
        {
            try
            {
                return TryCreateMemoryStream(capacity, out var ms) ? ms : null;
            }
            catch(System.OutOfMemoryException)
            {
                GC.Collect();
                return null;
            }
            
        }

        protected virtual bool TryCreateMemoryStream(long capacity,  out MEMSTREAM memoryStream)
        {
            if (capacity >= int.MaxValue) { memoryStream = null; return false; }

            memoryStream = new MEMSTREAM((int)capacity);
            return true;
        }

        #endregion

        #region core

        private static bool _CompareStreamsDirect(STREAM x, STREAM y, long maxLen = long.MaxValue, int bufferSize = 0)
        {
            // stretegies to determine default page size, specially on android and iOS, where much smaller buffers may be required
            // Environment.SystemPageSize

            if (bufferSize <= 0) bufferSize = _COMPARE_BUFFERREADSIZE_LARGE;

            var buff = bufferSize > 8192 ? _TryAllocate(bufferSize * 2) : stackalloc byte[bufferSize * 2];

            Span<Byte> xbuff = buff.Slice(0,buff.Length/2); // 1st half
            Span<Byte> ybuff = buff.Slice(buff.Length / 2); // 2nd half

            while (maxLen > 0)
            {
                var xlen = x.TryReadBytesToEnd(xbuff);
                var ylen = y.TryReadBytesToEnd(ybuff);

                if (xlen != ylen) return false; // length mismatch
                if (xlen == 0) break; // EOF on both files

                var xslice = xbuff.Slice(0, xlen);
                var yslice = ybuff.Slice(0, xlen);

                if (!xslice.SequenceEqual(yslice)) return false;

                maxLen -= xlen;
            }

            return true;
        }

        private static async ValueTask<bool> _CompareStreamsDirectAsync(STREAM x, STREAM y, long maxLen = long.MaxValue, int bufferSize = 0)
        {
            // stretgies to determine default page size, specially on android and iOS, where much smaller buffers may be required
            // Environment.SystemPageSize

            if (bufferSize <= 0) bufferSize = _COMPARE_BUFFERREADSIZE_LARGE;

            var buff = _TryAllocate(bufferSize).AsMemory();

            Memory<Byte> xbuff = buff.Slice(0, buff.Length / 2); // 1st half
            Memory<Byte> ybuff = buff.Slice(buff.Length / 2); // 2nd half

            while (maxLen > 0)
            {
                var xlen = await x.TryReadBytesToEndAsync(xbuff, System.Threading.CancellationToken.None).ConfigureAwait(false);
                var ylen = await y.TryReadBytesToEndAsync(ybuff, System.Threading.CancellationToken.None).ConfigureAwait(false);

                if (xlen != ylen) return false; // length mismatch
                if (xlen == 0) break; // EOF on both files

                var xslice = xbuff.Slice(0, xlen);
                var yslice = ybuff.Slice(0, xlen);

                if (!xslice.Span.SequenceEqual(yslice.Span)) return false;

                maxLen -= xlen;
            }

            return true;
        }

        private static Byte[] _TryAllocate(int bufferSize)
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

        #endregion

        #region nested types

        // TODO: an alternate implementation would be to compare the SHA512 of each stream

        /// <summary>
        /// Represents a context that uses <see cref="Microsoft.IO.RecyclableMemoryStream"/> as stream backing.
        /// </summary>
        public class RecyclableContext : StreamEqualityComparer
        {
            private readonly WeakReference<Microsoft.IO.RecyclableMemoryStreamManager> _Manager = new WeakReference<Microsoft.IO.RecyclableMemoryStreamManager>(null);

            private Microsoft.IO.RecyclableMemoryStreamManager _GetManager()
            {
                if (_Manager.TryGetTarget(out var manager)) return manager;

                manager = new Microsoft.IO.RecyclableMemoryStreamManager();

                _Manager.SetTarget(manager);

                return manager;                
            }

            protected override bool TryCreateMemoryStream(long capacity, out MEMSTREAM memoryStream)
            {
                var manager = _GetManager();

                memoryStream = new Microsoft.IO.RecyclableMemoryStream(manager, string.Empty, capacity);
                return true;
            }
        }        

        #endregion
    }
}
