using System;
using System.Collections.Generic;
using System.Text;
using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public class Shader : ContextProvider
    {
        #region lifecycle

        public static Shader CreateVertexShader(OPENGL gl, string code)
        {
            var s = new Shader(gl, ShaderType.VertexShader);
            s.CompileShader(code);
            return s;
        }

        public static Shader CreateFragmentShader(OPENGL gl, string code)
        {
            var s = new Shader(gl, ShaderType.FragmentShader);
            s.CompileShader(code);
            return s;
        }

        public Shader(OPENGL gl, ShaderType stype) : base(gl)
        {
            _ShaderId = gl.CreateShader(stype);
        }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteShader(_ShaderId);
            _ShaderId = 0;

            base.Dispose(gl);
        }

        #endregion

        #region data

        internal uint _ShaderId;

        #endregion

        #region API

        public void CompileShader(string shaderSourceBody)
        {
            if (string.IsNullOrWhiteSpace(shaderSourceBody)) throw new ArgumentNullException(nameof(shaderSourceBody));

            Context.ShaderSource(_ShaderId, shaderSourceBody);
            Context.CompileShader(_ShaderId);

            string infoLog = Context.GetShaderInfoLog(_ShaderId);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                throw new ArgumentException(infoLog, nameof(shaderSourceBody));
            }
        }

        #endregion
    }

    public class ShaderProgram : ContextProvider
    {
        #region lifecycle

        public static ShaderProgram CreateFromVertexAndFragmentShaders(OPENGL gl, string vertexShader, string fragmentShader)
        {
            using var vs = Shader.CreateVertexShader(gl, vertexShader);
            using var fs = Shader.CreateFragmentShader(gl, fragmentShader);

            return new ShaderProgram(vs, fs);
        }

        public ShaderProgram(Shader vertexShader, Shader fragmentShader)
            : base(vertexShader.Context)
        {
            _ProgramId = Context.CreateProgram();
            Context.AttachShader(_ProgramId, vertexShader._ShaderId);
            Context.AttachShader(_ProgramId, fragmentShader._ShaderId);
            Context.LinkProgram(_ProgramId);

            //Checking the linking for errors.
            Context.GetProgram(_ProgramId, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                var log = Context.GetProgramInfoLog(_ProgramId);
                throw new InvalidOperationException($"Error linking shader {log}");
            }

            // after detaching the shaders we could delete them;
            Context.DetachShader(_ProgramId, vertexShader._ShaderId);
            Context.DetachShader(_ProgramId, fragmentShader._ShaderId);
        }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteProgram(_ProgramId);

            base.Dispose(gl);
        }

        #endregion

        #region data

        private uint _ProgramId;

        #endregion

        #region API

        public unsafe void DrawTriangles(VertexBuffer vertices, IndexBuffer indices)
        {
            vertices.Use();
            indices.Use();

            Context.UseProgram(_ProgramId);

            //Draw the geometry.
            Context.DrawElements(indices.Mode, (uint)indices.Count, indices.Encoding, null);
        }

        #endregion
    }
}
