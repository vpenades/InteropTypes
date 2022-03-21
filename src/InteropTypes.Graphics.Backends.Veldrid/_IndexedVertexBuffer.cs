using System;
using System.Collections.Generic;
using System.Text;

using Veldrid;

namespace InteropTypes.Graphics.Backends
{
    class _IndexedVertexBuffer : IDisposable
    {
        #region lifecycle
        public _IndexedVertexBuffer(GraphicsDevice device) { _Device = device; }

        public void Dispose()
        {
            if (_vertexBuffer != null) { _Device.DisposeWhenIdle(_vertexBuffer); _vertexBuffer = null; }
            if (_indexBuffer != null) { _Device.DisposeWhenIdle(_indexBuffer); _indexBuffer = null; }
        }

        #endregion

        #region data

        private readonly GraphicsDevice _Device;

        private DeviceBuffer _vertexBuffer;
        private DeviceBuffer _indexBuffer;

        #endregion

        #region API

        public unsafe void SetData<TVertex>(Span<TVertex> vertexBuffer, Span<int> indexBuffer)
            where TVertex:unmanaged
        {
            _EnsureVertexBufferSize<TVertex>(vertexBuffer.Length);
            _EnsureIndexBufferSize(indexBuffer.Length);            

            _Device.UpdateBuffer(_vertexBuffer, 0, ref vertexBuffer[0], (uint)(vertexBuffer.Length * sizeof(TVertex)));
            _Device.UpdateBuffer(_indexBuffer, 0, ref indexBuffer[0], (uint)(indexBuffer.Length * sizeof(int)));            
        }

        public unsafe void SetData<TVertex>(CommandList cmd, Span<TVertex> vertexBuffer, Span<int> indexBuffer)
            where TVertex : unmanaged
        {
            _EnsureVertexBufferSize<TVertex>(vertexBuffer.Length);
            _EnsureIndexBufferSize(indexBuffer.Length);

            cmd.UpdateBuffer(_vertexBuffer, 0, ref vertexBuffer[0], (uint)(vertexBuffer.Length * sizeof(TVertex)));
            cmd.UpdateBuffer(_indexBuffer, 0, ref indexBuffer[0], (uint)(indexBuffer.Length * sizeof(int)));
        }

        public void Bind(CommandList cmdList)
        {
            cmdList.SetVertexBuffer(0, _vertexBuffer);
            cmdList.SetIndexBuffer(_indexBuffer, IndexFormat.UInt32);
        }

        public void DrawIndexed(CommandList cmdList, int startIndex, int indexCount)
        {
            cmdList.DrawIndexed(
                indexCount: (uint)indexCount,
                instanceCount: 1,
                indexStart: (uint)startIndex,
                vertexOffset: 0,
                instanceStart: 0);
        }

        private unsafe void _EnsureVertexBufferSize<TVertex>(int vertexCount)
            where TVertex : unmanaged
        {
            _EnsureBufferSize(ref _vertexBuffer, vertexCount * sizeof(TVertex), BufferUsage.VertexBuffer);
        }

        private void _EnsureIndexBufferSize(int indexCount)
        {
            _EnsureBufferSize(ref _indexBuffer, indexCount * sizeof(int), BufferUsage.IndexBuffer);
        }

        private void _EnsureBufferSize(ref DeviceBuffer buffer, int requiredSize, BufferUsage usage)
        {
            if (buffer != null && buffer.SizeInBytes < requiredSize)
            {
                _Device.DisposeWhenIdle(buffer);
                buffer = null;
            }

            if (buffer == null)
            {
                var descr = new BufferDescription((uint)requiredSize, usage);
                buffer = _Device.ResourceFactory.CreateBuffer(ref descr);
            }
        }

        #endregion
    }
}
