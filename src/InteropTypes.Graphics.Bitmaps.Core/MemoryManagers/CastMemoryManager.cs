using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.MemoryManagers
{
    // https://stackoverflow.com/questions/54511330/how-can-i-cast-memoryt-to-another

    sealed class CastMemoryManager<TFrom, TTo> : MemoryManager<TTo>
        where TFrom : unmanaged
        where TTo : unmanaged
    {
        private readonly Memory<TFrom> _from;

        public CastMemoryManager(Memory<TFrom> from) => _from = from;

        protected override void Dispose(bool disposing) { }

        public override Span<TTo> GetSpan() => MemoryMarshal.Cast<TFrom, TTo>(_from.Span);


        // In order to implement a pin/unpin mechanism we would need a thread safe reference counter mechanism        
        public override MemoryHandle Pin(int elementIndex = 0) => throw new NotSupportedException();

        public override void Unpin() => throw new NotSupportedException();
    }
}
