using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.MemoryManagers
{
    // https://stackoverflow.com/questions/52190423/c-sharp-access-unmanaged-array-using-memoryt-or-arraysegmentt
    // https://github.com/mgravell/Pipelines.Sockets.Unofficial/blob/master/src/Pipelines.Sockets.Unofficial/UnsafeMemory.cs


    // https://gist.github.com/GrabYourPitchforks/8efb15abbd90bc5b128f64981766e834#replacing-the-memoryt-implementation

    // https://www.codemag.com/Article/1807051/Introducing-.NET-Core-2.1-Flagship-Types-Span-T-and-Memory-T

    sealed unsafe class UnmanagedMemoryManager<T> : MemoryManager<T>
        where T:unmanaged
    {
        #region lifecycle

        /// <summary>
        /// Create a new UnmanagedMemoryManager instance at the given pointer and size
        /// </summary>
        public UnmanagedMemoryManager(T* pointer, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _pointer = pointer;
            _length = length;
        }

        /// <summary>
        /// Create a new UnmanagedMemoryManager instance at the given pointer and size
        /// </summary>
        public UnmanagedMemoryManager(IntPtr ptr, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            _pointer = (T*)ptr.ToPointer();
            _length = length;
        }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pointer = null;
                _length = 0;
            }
        }

        #endregion

        #region data

        private T* _pointer;
        private int _length;        

        #endregion

        #region API
        
        /// <summary>
        /// Obtains a span that represents the region
        /// </summary>
        public override Span<T> GetSpan() => new Span<T>(_pointer, _length);

        /// <summary>
        /// Provides access to a pointer that represents the data (note: no actual pin occurs)
        /// </summary>
        public override MemoryHandle Pin(int elementIndex = 0)
        {
            if (elementIndex < 0 || elementIndex >= _length) throw new ArgumentOutOfRangeException(nameof(elementIndex));
            return new MemoryHandle(_pointer + elementIndex, pinnable:this);
        }

        /// <summary>
        /// Has no effect
        /// </summary>
        public override void Unpin() { }        

        #endregion
    }
}
