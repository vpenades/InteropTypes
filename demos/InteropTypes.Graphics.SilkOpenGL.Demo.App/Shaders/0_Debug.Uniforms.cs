using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

namespace Tutorial.Shaders
{
    sealed class _Zero_Debug_Uniforms : IUniforms
    {
        public string GetShaderCode()
        {
            return typeof(_Zero_Debug_Uniforms).Assembly.ReadAllText("0_Debug.Shader.frag");
        }

        public void Initialize(UniformFactory ufactory)
        {
         
        }
    }
}
