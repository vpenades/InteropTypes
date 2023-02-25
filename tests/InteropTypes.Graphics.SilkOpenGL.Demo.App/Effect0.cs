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
    internal class Effect0 : Effect
    {
        public Effect0(OPENGL gl)
        {
            var ufactory = CreateProgram(gl, System.Reflection.Assembly.GetExecutingAssembly(), "Effect0.Shader.vert", "Effect0.Shader.frag");

            _uModel = ufactory.UseMatrix4x4("uModel", true);
        }

        private UniformMatrix<Matrix4x4> _uModel;           

        protected override void CommitStaticUniforms()
        {
            _uModel.Set(Matrix4x4.Identity);
        }

        protected override IEffectUniforms UseDynamicUniforms()
        {
            return new _BoundUniforms(this);
        }

        readonly struct _BoundUniforms : IEffectUniforms, IEffectTransforms3D
        {
            public _BoundUniforms(Effect0 effect) { _Effect= effect; }

            private readonly Effect0 _Effect;

            public void SetModelMatrix(Matrix4x4 matrix)
            {
                _Effect._uModel.Set(matrix);
            }

            public void SetProjMatrix(Matrix4x4 matrix)
            {
                
            }

            public void SetViewMatrix(Matrix4x4 matrix)
            {
                
            }
        }
    }
}
