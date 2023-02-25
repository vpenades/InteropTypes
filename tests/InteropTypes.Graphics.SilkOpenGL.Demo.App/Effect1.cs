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
    internal class Effect1 : Effect
    {
        public Effect1(OPENGL gl)
        {
            var ufactory = CreateProgram(gl, System.Reflection.Assembly.GetExecutingAssembly(), "Effect1.Shader.vert", "Effect1.Shader.frag");

            

            // _uTexture0 = this.UniformFactory.UseTexture("uTexture0", Silk.NET.OpenGL.TextureUnit.Texture0);
            // _uModel = this.UniformFactory.UseMatrix4x4("uModel", true);
            // _uModel.Set(Matrix4x4.Identity);
        }

        private UniformTexture _uTexture0;
        private UniformMatrix<Matrix4x4> _uModel;

        protected override void CommitStaticUniforms()
        {
            throw new NotImplementedException();
        }

        protected override IEffectUniforms UseDynamicUniforms()
        {
            throw new NotImplementedException();
        }
    }
}
