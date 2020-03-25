using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps
{
    public class OwnedMemoryBitmap : MemoryBitmap, System.Buffers.IMemoryOwner<Byte>
    {
        #region lifecycle

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownedMemory">An instance of <see cref="System.Buffers.IMemoryOwner{T}"/> like <see cref="System.Buffers.MemoryManager{T}"/></param>
        /// <param name="info"></param>
        public OwnedMemoryBitmap(System.Buffers.IMemoryOwner<Byte> ownedMemory, in BitmapInfo info) : base(ownedMemory.Memory, info)
        {
            _OwnedMemory = ownedMemory;
        }

        public void Dispose()
        {
            this.DisposeBuffers();
            _OwnedMemory?.Dispose();
            _OwnedMemory = null;
        }

        #endregion

        #region data

        private System.Buffers.IMemoryOwner<Byte> _OwnedMemory;

        #endregion

    }
}
