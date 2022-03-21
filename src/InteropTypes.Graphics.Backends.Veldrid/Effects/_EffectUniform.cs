using System;
using System.Collections.Generic;
using System.Text;

using Veldrid;

namespace InteropTypes.Graphics.Backends
{
    class _EffectUniform : IEffectInput, IDisposable
    {
        #region lifecycle

        public static implicit operator ResourceLayout(_EffectUniform uniform) => uniform._ResourceLayout;

        public _EffectUniform(GraphicsDevice device, string name, ShaderStages stage, int byteCount)
        {
            _Device = device;
            Name = name;

            var desc = new ResourceLayoutElementDescription(name, ResourceKind.UniformBuffer, stage);

            _ByteCount = byteCount;

            _DeviceBuffer = device.ResourceFactory.CreateBuffer(new BufferDescription((uint)byteCount, BufferUsage.UniformBuffer));
            _ResourceLayout = device.ResourceFactory.CreateResourceLayout(new ResourceLayoutDescription(desc));
            _ResourceSet = device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(_ResourceLayout, _DeviceBuffer));
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _ResourceSet, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _ResourceLayout, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _DeviceBuffer, null)?.Dispose();

            _Device = null;
        }

        #endregion

        #region data

        protected GraphicsDevice _Device;

        protected DeviceBuffer _DeviceBuffer;
        private ResourceSet _ResourceSet;
        private ResourceLayout _ResourceLayout;

        protected readonly int _ByteCount;        

        #endregion

        #region API

        public string Name { get; private set; }

        ResourceLayout IEffectInput.GetResourceLayout() => _ResourceLayout;        

        public virtual void Bind(CommandList cmdList, uint index)
        {
            cmdList.SetGraphicsResourceSet(index, _ResourceSet);
        }

        #endregion
    }

    class _EffectUniform<T> : _EffectUniform
        where T:unmanaged
    {
        public unsafe _EffectUniform(GraphicsDevice device, string name, ShaderStages stage)
            : base(device,name,stage, sizeof(T)) { }

        private T _Value;

        public unsafe void Update(ref T value)
        {
            _Value = value;

            // _Device.UpdateBuffer(_DeviceBuffer, 0, ref value);
        }

        public override void Bind(CommandList cmdList, uint index)
        {
            cmdList.UpdateBuffer(_DeviceBuffer, 0, ref _Value);

            base.Bind(cmdList, index);
        }
    }
}
