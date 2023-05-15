using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

namespace Tutorial.Shaders
{
    sealed class _PTxMVP_T_Uniforms : IUniforms, IUniformTransforms3D
    {
        public string GetShaderCode()
        {
            return typeof(_PTxMVP_T_Uniforms).Assembly.ReadAllText("PTxMVP=T.Shader.vert");
        }

        public void Initialize(UniformFactory ufactory)
        {
            _uModel = ufactory.UseMatrix4x4("uModel", true);
            _uView = ufactory.UseMatrix4x4("uView", true);
            _uProj = ufactory.UseMatrix4x4("uProjection", true);
        }

        private UniformMatrix<Matrix4x4> _uModel;
        private UniformMatrix<Matrix4x4> _uView;
        private UniformMatrix<Matrix4x4> _uProj;        

        public void SetModelMatrix(Matrix4x4 matrix)
        {
            _uModel.Set(matrix);
        }

        public void SetProjMatrix(Matrix4x4 matrix)
        {
            _uView.Set(matrix);
        }

        public void SetViewMatrix(Matrix4x4 matrix)
        {
            _uProj.Set(matrix);
        }
    }
}
