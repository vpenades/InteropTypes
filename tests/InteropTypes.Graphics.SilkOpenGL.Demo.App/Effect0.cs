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
    internal class Effect0 : ShaderProgram
    {
        private UniformMatrix<Matrix4x4> _uModel;

        public Effect0(OPENGL gl) : base(gl)
        {
            SetShadersFrom(System.Reflection.Assembly.GetExecutingAssembly(), "Effect0.Shader.vert", "Effect0.Shader.frag");

            _uModel = this.UniformFactory.UseMatrix4x4("uModel", true);            
        }

        public Matrix4x4 ModelMatrix { get; set; } = Matrix4x4.Identity;
        public override void CommitUniforms()
        {
            _uModel.Set(ModelMatrix);
        }
    }
}
