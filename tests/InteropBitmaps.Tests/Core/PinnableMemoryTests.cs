using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace InteropBitmaps.Core
{
    public class PinnableMemoryTests
    {
        [Test]
        public unsafe void TestPinnableMemory()
        {
            Span<Byte> kk = new byte[3];

            // System.Buffers.MemoryHandle

            // var h = System.Runtime.InteropServices.GCHandle.Alloc(kk.GetPinnableReference());

            // var p = (IntPtr)System.Runtime.CompilerServices.Unsafe.AsPointer(ref kk.GetPinnableReference());

            // h.Free();
        }
    }
}
