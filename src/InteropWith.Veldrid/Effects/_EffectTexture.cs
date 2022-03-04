using System;
using System.Collections.Generic;
using System.Text;

using Veldrid;

namespace InteropTypes.Graphics.Backends
{
    class _EffectTexture : IEffectInput, IDisposable
    {
        #region lifecycle

        public static implicit operator ResourceLayout(_EffectTexture texture) => texture._OwnedTextureLayout;

        public _EffectTexture(GraphicsDevice device, string inputName)
        {
            Name = inputName;
            _ExtDevice = device;

            // Input
            var texDesc = new ResourceLayoutElementDescription(inputName, ResourceKind.TextureReadOnly, ShaderStages.Fragment);
            _OwnedTextureLayout = device.ResourceFactory.CreateResourceLayout(new ResourceLayoutDescription(texDesc));            
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _OwnedTextureLayout, null)?.Dispose();

            if (_OwnerTextureResourceSets != null)
            {
                foreach (var tset in _OwnerTextureResourceSets.Values) tset.Dispose();
                _OwnerTextureResourceSets = null;
            }

            _ExtTextureSet = null;            
            _ExtDevice = null;
        }

        #endregion

        #region data

        public string Name { get; private set; }

        ResourceLayout IEffectInput.GetResourceLayout() => _OwnedTextureLayout;

        private GraphicsDevice _ExtDevice;

        private ResourceSet _ExtTextureSet;
        internal ResourceLayout _OwnedTextureLayout;

        private Dictionary<TextureView, ResourceSet> _OwnerTextureResourceSets = new Dictionary<TextureView, ResourceSet>();        

        #endregion

        #region API

        public void SetTexture(TextureView texView)
        {
            if (texView == null) { _ExtTextureSet = null; return; }

            if (_OwnerTextureResourceSets.TryGetValue(texView, out var textureSet)) { _ExtTextureSet = textureSet; return; }

            var rsetDesc = new ResourceSetDescription(_OwnedTextureLayout, texView);
            textureSet = _ExtDevice.ResourceFactory.CreateResourceSet(rsetDesc);
            _OwnerTextureResourceSets[texView] = textureSet;

            _ExtTextureSet = textureSet;
        }        

        public void Bind(CommandList cmdList, uint resourceIndex)
        {
            if (_ExtTextureSet == null) return;

            cmdList.SetGraphicsResourceSet(resourceIndex, _ExtTextureSet);
            
        }

        #endregion
    }

    class _EffectSampler : IEffectInput, IDisposable
    {
        #region lifecycle

        public static implicit operator ResourceLayout(_EffectSampler sampler) => sampler._OwnedSamplerLayout;

        public _EffectSampler(GraphicsDevice device, string samplerName)
        {
            Name = samplerName;
            _ExtDevice = device;
            
            var smpDesc = new ResourceLayoutElementDescription(samplerName, ResourceKind.Sampler, ShaderStages.Fragment);

            _OwnedSamplerLayout = device.ResourceFactory.CreateResourceLayout(new ResourceLayoutDescription(smpDesc));

            _OwnedSamplerResourceSets = new _EffectSamplers(device, _OwnedSamplerLayout);
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _OwnedSamplerLayout, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _OwnedSamplerResourceSets, null)?.Dispose();

            _ExtSamplerSet = null;
            _ExtDevice = null;
        }

        #endregion

        #region data

        public string Name { get; private set; }

        private GraphicsDevice _ExtDevice;
        private ResourceSet _ExtSamplerSet;

        internal ResourceLayout _OwnedSamplerLayout;
        private _EffectSamplers _OwnedSamplerResourceSets;

        #endregion

        #region API

        ResourceLayout IEffectInput.GetResourceLayout() => _OwnedSamplerLayout;

        public void SetSampler(int filtering, bool clamp)
        {
            _ExtSamplerSet = _OwnedSamplerResourceSets[filtering, clamp];
        }

        public void Bind(CommandList cmdList, uint samplerIndex)
        {
            if (_ExtSamplerSet == null) return;

            cmdList.SetGraphicsResourceSet(samplerIndex, _ExtSamplerSet);
        }

        #endregion
    }

    class _EffectSamplers : IDisposable
    {
        #region lifecycle

        public _EffectSamplers(GraphicsDevice device, ResourceLayout layout)
        {
            _SamplerResourceSets[0] = new Lazy<ResourceSet>(() => _SamplerFactory(device, layout, 1, true), false);
            _SamplerResourceSets[1] = new Lazy<ResourceSet>(() => _SamplerFactory(device, layout, 1, false), false);
            _SamplerResourceSets[2] = new Lazy<ResourceSet>(() => _SamplerFactory(device, layout, 0, true), false);
            _SamplerResourceSets[3] = new Lazy<ResourceSet>(() => _SamplerFactory(device, layout, 0, false), false);
            _SamplerResourceSets[4] = new Lazy<ResourceSet>(() => _SamplerFactory(device, layout, 2, true), false);
            _SamplerResourceSets[5] = new Lazy<ResourceSet>(() => _SamplerFactory(device, layout, 2, false), false);
        }

        public void Dispose()
        {
            _Disposables.Dispose();

            Array.Clear(_SamplerResourceSets,0,_SamplerResourceSets.Length);
        }

        #endregion

        #region data

        private readonly Lazy<ResourceSet>[] _SamplerResourceSets = new Lazy<ResourceSet>[6];

        private readonly _DisposablesRecorder _Disposables = new _DisposablesRecorder();

        #endregion

        #region  API
        public ResourceSet this[int filtering, bool clamp]
        {
            get
            {
                var index = filtering * 2;
                if (!clamp) index += 1;

                return _SamplerResourceSets[index].Value;
            }
        }

        private ResourceSet _SamplerFactory(GraphicsDevice device, ResourceLayout layout, int filtering, bool clamp)
        {            
            ResourceSetDescription resourceSetDescr = default;            

            if (clamp)
            {
                var clampDescr = SamplerDescription.Linear;

                switch (filtering)
                {
                    case 0: clampDescr = SamplerDescription.Point; break;
                    case 1: clampDescr = SamplerDescription.Linear; break;
                    case 2: clampDescr = SamplerDescription.Aniso4x; break;
                }

                clampDescr.AddressModeU = SamplerAddressMode.Clamp;
                clampDescr.AddressModeV = SamplerAddressMode.Clamp;
                clampDescr.AddressModeW = SamplerAddressMode.Clamp;

                var clampSampler = _Disposables.Record(device.ResourceFactory.CreateSampler(clampDescr));

                resourceSetDescr = new ResourceSetDescription(layout, clampSampler);                
            }
            else
            {
                switch (filtering)
                {
                    case 0: resourceSetDescr = new ResourceSetDescription(layout, device.PointSampler); break;
                    case 1: resourceSetDescr = new ResourceSetDescription(layout, device.LinearSampler); break;
                    case 2: resourceSetDescr = new ResourceSetDescription(layout, device.Aniso4xSampler); break;
                }
            }

            return _Disposables.Record(device.ResourceFactory.CreateResourceSet(ref resourceSetDescr));
        }

        #endregion
    }
}
