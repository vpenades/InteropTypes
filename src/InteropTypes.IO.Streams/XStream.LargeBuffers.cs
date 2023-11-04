using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.Buffers;

using STREAM = System.IO.Stream;

using BYTESSEGMENT = System.ArraySegment<byte>;

using LARGEBYTESEGMENT = System.Buffers.ReadOnlySequence<byte>; // used by RecyclableMemoryStream

using LARGESTREAMMANAGER = Microsoft.IO.RecyclableMemoryStreamManager;

namespace InteropTypes.IO
{
    partial class XStream
    {
        private static readonly WeakReference<LARGESTREAMMANAGER> _Manager = new WeakReference<LARGESTREAMMANAGER>(null);

        internal static LARGESTREAMMANAGER _GetLargeStreamManager()
        {
            if (_Manager.TryGetTarget(out var manager)) return manager;

            manager = new LARGESTREAMMANAGER();

            _Manager.SetTarget(manager);

            return manager;
        }

        public static MemoryStream CreateMemoryStream(BYTESSEGMENT bytes, bool writable = false)
        {
            return bytes.Count == 0
                ? new System.IO.MemoryStream(Array.Empty<byte>(), writable)
                : new System.IO.MemoryStream(bytes.Array, bytes.Offset, bytes.Count, writable);
        }

        public static MemoryStream CreateMemoryStream(bool useRecyclable)
        {
            var manager = useRecyclable ? _GetLargeStreamManager() : null;
            return CreateMemoryStream(manager);
        }

        public static MemoryStream CreateMemoryStream(LARGESTREAMMANAGER manager, string tag = null)
        {
            return manager == null
                ? new System.IO.MemoryStream()
                : new Microsoft.IO.RecyclableMemoryStream(manager, tag);
        }

        public static MemoryStream CreateMemoryStream(Func<STREAM> srcStream, bool useRecyclable)
        {
            if (srcStream == null) throw new ArgumentNullException(nameof(srcStream));
            using var s = srcStream();
            return CreateMemoryStream(s, useRecyclable);
        }

        public static MemoryStream CreateMemoryStream(Func<STREAM> srcStream, LARGESTREAMMANAGER manager)
        {
            if (srcStream == null) throw new ArgumentNullException(nameof(srcStream));
            using var s = srcStream();
            return CreateMemoryStream(s, manager);
        }

        public static MemoryStream CreateMemoryStream(STREAM srcStream, bool useRecyclable)
        {
            GuardReadable(srcStream);
            return CreateMemoryStream(srcStream, useRecyclable ? _GetLargeStreamManager() : null);
        }

        public static MemoryStream CreateMemoryStream(STREAM srcStream, LARGESTREAMMANAGER manager)
        {
            GuardReadable(srcStream);
            var dstStream = CreateMemoryStream(manager);
            srcStream.CopyTo(dstStream);
            dstStream.Position = 0;
            return dstStream;
        }

        public static async Task<MemoryStream> CreateMemoryStreamAsync(STREAM srcStream, bool useRecyclable, CancellationToken token)
        {
            GuardReadable(srcStream);
            var mgr = useRecyclable ? _GetLargeStreamManager() : null;
            return await CreateMemoryStreamAsync(srcStream, mgr, token).ConfigureAwait(false);
        }

        public static async Task<MemoryStream> CreateMemoryStreamAsync(STREAM srcStream, LARGESTREAMMANAGER manager, CancellationToken token)
        {
            GuardReadable(srcStream);
            var dstStream = CreateMemoryStream(manager);
            await srcStream.CopyToAsync(dstStream, token).ConfigureAwait(false);
            dstStream.Position = 0;
            return dstStream;
        }

        public static void WriteAllBytes(STREAM dstStream, LARGEBYTESEGMENT segment)
        {
            GuardWriteable(dstStream);
            foreach (var block in segment)
            {
                dstStream.Write(block.Span);
            }
        }

        public static async Task WriteAllBytesAsync(STREAM dstStream, LARGEBYTESEGMENT segment, CancellationToken token)
        {
            GuardWriteable(dstStream);
            foreach (var block in segment)
            {
                await dstStream.WriteAsync(block, token).ConfigureAwait(false);
            }
        }

        public static LARGEBYTESEGMENT ReadAllBytesSequence(STREAM srcStream)
        {
            GuardReadable(srcStream);
            _BlockSegment first = null;
            _BlockSegment last = null;
            long totalLength = 0;

            while (true)
            {
                var buffer = new Byte[LARGESTREAMMANAGER.DefaultBlockSize];

                var len = srcStream.TryReadBytesToEnd(buffer);
                if (len == 0) break;

                totalLength += len;

                var mem = buffer.AsMemory(0, len);

                switch (first)
                {
                    case null: first = last = new _BlockSegment(mem); break;
                    default: last = last.Append(mem); break;
                }
            }

            if (Object.ReferenceEquals(first, last))
            {
                return new LARGEBYTESEGMENT(last.Memory);
            }

            return new LARGEBYTESEGMENT(first, 0, last, (int)(totalLength - last.RunningIndex));
        }

        public static async Task<LARGEBYTESEGMENT> ReadAllBytesSequenceAsync(STREAM srcStream, CancellationToken token)
        {
            GuardReadable(srcStream);
            _BlockSegment first = null;
            _BlockSegment last = null;
            long totalLength = 0;

            while (true)
            {
                var buffer = new Byte[LARGESTREAMMANAGER.DefaultBlockSize];

                var len = await srcStream.TryReadBytesToEndAsync(buffer, token).ConfigureAwait(false);
                if (len == 0) break;

                totalLength += len;

                var mem = buffer.AsMemory(0, len);

                switch (first)
                {
                    case null: first = last = new _BlockSegment(mem); break;
                    default: last = last.Append(mem); break;
                }
            }

            if (Object.ReferenceEquals(first, last))
            {
                return new LARGEBYTESEGMENT(last.Memory);
            }

            return new LARGEBYTESEGMENT(first, 0, last, (int)(totalLength - last.RunningIndex));
        }

        /// <remarks>
        /// Taken from <see cref="Microsoft.IO.RecyclableMemoryStream.GetReadOnlySequence"/>
        /// </remarks>
        private sealed class _BlockSegment : ReadOnlySequenceSegment<byte>
        {
            public _BlockSegment(Memory<byte> memory) => Memory = memory;

            public _BlockSegment Append(Memory<byte> memory)
            {
                var nextSegment = new _BlockSegment(memory) { RunningIndex = RunningIndex + Memory.Length };
                Next = nextSegment;
                return nextSegment;
            }
        }
    }
}
