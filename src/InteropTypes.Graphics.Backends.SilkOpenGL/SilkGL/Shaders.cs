using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    class Shader : ContextProvider
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

    class ShaderProgram : ContextProvider
    {
        #region lifecycle

        public ShaderProgram(OPENGL gl)
            : base(gl) { }

        public ShaderProgram(Shader vertexShader, Shader fragmentShader)
            : base(vertexShader.Context)
        {
            _Initialize(vertexShader, fragmentShader);
        }

        public void SetShadersFrom(System.Reflection.Assembly resAssembly, string vname, string fname)
        {
            var vcode = resAssembly.ReadAllText(vname) ?? throw new System.IO.FileNotFoundException(vname);
            var fcode = resAssembly.ReadAllText(fname) ?? throw new System.IO.FileNotFoundException(fname);

            SetShadersCode(vcode, fcode);
        }

        public void SetShadersCode(string vertexCode, string fragmentCode)
        {
            using var vs = Shader.CreateVertexShader(this.Context, vertexCode);
            using var fs = Shader.CreateFragmentShader(this.Context, fragmentCode);

            _Initialize(vs, fs);            
        }        

        private void _Initialize(Shader vertexShader, Shader fragmentShader)
        {
            Context.ThrowOnError();

            if (_ProgramId != 0) Context.DeleteProgram(_ProgramId);

            _ProgramId = Context.CreateProgram();
            Context.ThrowOnError();            

            Context.AttachShader(_ProgramId, vertexShader._ShaderId);
            Context.ThrowOnError();

            Context.AttachShader(_ProgramId, fragmentShader._ShaderId);
            Context.ThrowOnError();

            Context.LinkProgram(_ProgramId);
            Context.ThrowOnError();

            //Checking the linking for errors.
            Context.GetProgram(_ProgramId, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                var log = Context.GetProgramInfoLog(_ProgramId);
                throw new InvalidOperationException($"Error linking shader {log}");
            }

            // after detaching the shaders we could delete them;
            Context.DetachShader(_ProgramId, vertexShader._ShaderId);
            Context.ThrowOnError();

            Context.DetachShader(_ProgramId, fragmentShader._ShaderId);
            Context.ThrowOnError();
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

        #region properties

        internal UniformFactory UniformFactory => new UniformFactory(this.Context, _ProgramId);

        #endregion

        #region API

        public void Bind()
        {
            Context.ThrowOnError();
            Context.UseProgram(_ProgramId);
            Context.ThrowOnError();
        }

        

        public void Unbind()
        {
            Context.ThrowOnError();
            Context.UseProgram(0);
            Context.ThrowOnError();            
        }        

        #endregion
    }

    
}
        
