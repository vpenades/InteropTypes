using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace Tutorial
{
    internal class Effect2 : Effect
    {
        public Effect2(OPENGL gl) : base(gl)
        {
            var ufactory = CreateProgram(System.Reflection.Assembly.GetExecutingAssembly(), "Effect2.Shader.vert", "Effect0.Shader.frag");

            _VertexProperties = new _VertexUniforms(ufactory);
        }

        private readonly _VertexUniforms _VertexProperties;

        protected override (IEffectUniforms Vertex, IEffectUniforms Fragment) UseDynamicUniforms()
        {
            return (_VertexProperties, null);
        }

        sealed class _VertexUniforms : IEffectUniforms, IEffectTransforms3D
        {
            public _VertexUniforms(UniformFactory ufactory)
            {
                _uModel = ufactory.UseMatrix4x4("uModel", true);
                _uView = ufactory.UseMatrix4x4("uView", true);
                _uProj = ufactory.UseMatrix4x4("uProjection", true);
            }
            
            private UniformMatrix<Matrix4x4> _uModel;
            private UniformMatrix<Matrix4x4> _uView;
            private UniformMatrix<Matrix4x4> _uProj;

            public void OnBind(Effect effect)
            {
                if (effect is not Effect2) throw new ArgumentException("type mismatch", nameof(effect));

                SetModelMatrix(Matrix4x4.Identity);
                SetProjMatrix(Matrix4x4.Identity);
                SetViewMatrix(Matrix4x4.Identity);
            }

            public void SetModelMatrix(Matrix4x4 matrix) { _uModel.Set(matrix); }

            public void SetProjMatrix(Matrix4x4 matrix) { _uView.Set(matrix); }

            public void SetViewMatrix(Matrix4x4 matrix) { _uProj.Set(matrix); }
        }
    }
}
