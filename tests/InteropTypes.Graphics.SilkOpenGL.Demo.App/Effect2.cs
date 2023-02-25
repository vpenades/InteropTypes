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
        //Vertex shaders are run on each vertex.
        private static readonly string VertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec3 vPos;
        layout (location = 1) in vec2 vUv;

        uniform mat4 uModel;
        uniform mat4 uView;
        uniform mat4 uProjection;

        out vec2 fUv;

        void main()
        {
            //Multiplying our uniform with the vertex position, the multiplication order here does matter.
            gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);
            fUv = vUv;
        }
        ";

        //Fragment shaders are run on each fragment/pixel of the geometry.
        private static readonly string FragmentShaderSource = @"
        #version 330 core
        in vec2 fUv;

        uniform sampler2D uTexture0;

        out vec4 FragColor;

        void main()
        {
            FragColor = texture(uTexture0, fUv);
        }
        ";

        private UniformTexture _uTexture0;
        private UniformMatrix<Matrix4x4> _uModel;
        private UniformMatrix<Matrix4x4> _uView;
        private UniformMatrix<Matrix4x4> _uProj;

        public Effect2(OPENGL gl)
        {
            var ufactory = CreateProgram(gl, System.Reflection.Assembly.GetExecutingAssembly(), "Effect1.Shader.vert", "Effect1.Shader.frag");

            /*
            _uTexture0 = this.UniformFactory.UseTexture("uTexture0", Silk.NET.OpenGL.TextureUnit.Texture0);
            _uModel = this.UniformFactory.UseMatrix4x4("uModel", true);
            _uView = this.UniformFactory.UseMatrix4x4("uView", true);
            _uProj = this.UniformFactory.UseMatrix4x4("uProjection", true);
            */
        }

        public void SetTexture(Texture texture) { _uTexture0.Set(texture); }

        public void SetModelMatrix(in Matrix4x4 matrix) { _uModel.Set(matrix); }

        public void SetViewMatrix(in Matrix4x4 matrix) { _uView.Set(matrix); }

        public void SetProjMatrix(in Matrix4x4 matrix) { _uProj.Set(matrix); }

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
