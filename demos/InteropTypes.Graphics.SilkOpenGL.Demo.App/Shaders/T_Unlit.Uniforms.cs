﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

namespace Tutorial.Shaders
{
    sealed class _T_Unlit_Uniforms : IUniforms, IUniformTextures
    {
        public string GetShaderCode()
        {
            return typeof(_PxM_Uniforms).Assembly.ReadAllText("T_Unlit.Shader.frag");
        }

        private UniformTexture _Sampler0;        

        public void Initialize(UniformFactory ufactory)
        {
            _Sampler0 = ufactory.UseTexture("uTexture0");
        }

        public void BindTextures(TextureGroup textures)
        {
            var slot = textures.GetSlot("uTexture0");

            _Sampler0.Set(slot);
        }

        public void UnbindTextures()
        {
            _Sampler0.Set(0);
        }
    }
}
