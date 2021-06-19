using System;
using System.Collections.Generic;
using System.Text;

using Veldrid;

namespace InteropWith
{
    class _EffectUniform : IDisposable
    {
        public static implicit operator ResourceLayout(_EffectUniform uniform) => uniform._ResourceLayout;

        public _EffectUniform(GraphicsDevice device, string name, ShaderStages stage, int byteCount)
        {
            _Device = device;

            var desc = new ResourceLayoutElementDescription(name, ResourceKind.UniformBuffer, stage);

            _ByteCount = byteCount;

            _DeviceBuffer = device.ResourceFactory.CreateBuffer(new BufferDescription((uint)byteCount, BufferUsage.UniformBuffer));
            _ResourceLayout = device.ResourceFactory.CreateResourceLayout(new ResourceLayoutDescription(desc));
            _ResourceSet = device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(_ResourceLayout, _DeviceBuffer));
        }

        public void Dispose()
        {
            _ResourceSet?.Dispose();
            _ResourceSet = null;

            _ResourceLayout?.Dispose();
            _ResourceLayout = null;            

            _DeviceBuffer?.Dispose();
            _DeviceBuffer = null;

            _Device = null;
        }

        private GraphicsDevice _Device;

        private DeviceBuffer _DeviceBuffer;
        private ResourceSet _ResourceSet;
        private ResourceLayout _ResourceLayout;

        private int _ByteCount;

        public unsafe void Update<T>(ref T value) where T:unmanaged
        {
            System.Diagnostics.Debug.Assert(sizeof(T) <= _ByteCount);

            _Device.UpdateBuffer(_DeviceBuffer, 0, ref value);
        }

        public void Bind(CommandList cmdList, uint index)
        {
            cmdList.SetGraphicsResourceSet(index, _ResourceSet);
        }
    }
}
