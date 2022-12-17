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
    internal class Effect1 : ShaderProgram
    {
        //Vertex shaders are run on each vertex.
        private static readonly string VertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec3 vPos;
        layout (location = 1) in vec2 vUv;

        uniform mat4 uModel;

        out vec2 fUv;

        void main()
        {
            //Multiplying our uniform with the vertex position, the multiplication order here does matter.
            gl_Position =  uModel * vec4(vPos, 1.0);
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

        public Effect1(OPENGL gl) : base(gl, VertexShaderSource, FragmentShaderSource)
        {
            _uTexture0 = this.UniformFactory.UseTexture("uTexture0", Silk.NET.OpenGL.TextureUnit.Texture0);
            _uModel = this.UniformFactory.UseMatrix4x4("uModel", true);
        }

        public void SetTexture(Texture texture) { _uTexture0.Set(texture); }

        public void SetViewMatrix(in Matrix4x4 matrix) { _uModel.Set(matrix); }
    }
}
