using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Veldrid;

namespace InteropTypes.Graphics.Backends
{
    class _EffectShader : IDisposable
    {
        #region lifecycle

        public _EffectShader(ShaderSetDescription shaderSet, params IEffectInput[] inputs)
        {
            _ShaderSet = shaderSet;
            _ResourceLayouts = inputs.Select(item => item.GetResourceLayout()).ToArray();
            _EffectInputs = inputs.ToArray();
        }

        public void Dispose()
        {
            if (_ShaderSet.Shaders != null)
            {
                foreach (var s in _ShaderSet.Shaders) s.Dispose();
            }

            _ShaderSet = default;

            _ResourceLayouts = null;
            _EffectInputs = null;
        }

        #endregion

        #region data

        private ShaderSetDescription _ShaderSet;
        private ResourceLayout[] _ResourceLayouts;
        private IEffectInput[] _EffectInputs;

        #endregion

        #region API

        public void  FillPipelineDesc(ref GraphicsPipelineDescription pipelineDesc)
        {
            pipelineDesc.ShaderSet = _ShaderSet;
            pipelineDesc.ResourceLayouts = _ResourceLayouts;            
        }

        public void BindInputs(CommandList cmdList)
        {
            for(int i=0; i < _EffectInputs.Length; ++i)
            {
                _EffectInputs[i].Bind(cmdList, (uint)i);
            }
        }

        #endregion
    }
}
