using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

namespace Tutorial.Shaders
{
    sealed class _PxM_Uniforms : IUniforms, IUniformTransforms3D
    {
        public string GetShaderCode()
        {
            return typeof(_PxM_Uniforms).Assembly.ReadAllText("PxM.Shader.vert");
        }

        public void Initialize(UniformFactory ufactory)
        {
            _uModel = ufactory.UseMatrix4x4("uModel", true);
        }

        private UniformMatrix<Matrix4x4> _uModel;        

        public void SetModelMatrix(Matrix4x4 matrix) { _uModel.Set(matrix); }

        public void SetProjMatrix(Matrix4x4 matrix) { }

        public void SetViewMatrix(Matrix4x4 matrix) { }
    }
}
