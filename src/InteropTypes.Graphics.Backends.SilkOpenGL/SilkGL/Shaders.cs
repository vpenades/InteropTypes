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

        public static ShaderProgram CreateFrom(OPENGL gl, System.Reflection.Assembly resAssembly, string vname, string fname)
        {
            var vcode = resAssembly.ReadAllText(vname) ?? throw new System.IO.FileNotFoundException(vname);
            var fcode = resAssembly.ReadAllText(fname) ?? throw new System.IO.FileNotFoundException(fname);

            return CreateFromCode(gl, vcode, fcode);
        }

        public static ShaderProgram CreateFromCode(OPENGL gl, string vertexCode, string fragmentCode)
        {
            using var vs = Shader.CreateVertexShader(gl, vertexCode);
            using var fs = Shader.CreateFragmentShader(gl, fragmentCode);

            return Create(vs, fs);
        }

        public static ShaderProgram Create(Shader vertexShader, Shader fragmentShader)
        {
            if (vertexShader == null) throw new ArgumentNullException(nameof(vertexShader));
            if (fragmentShader == null) throw new ArgumentNullException(nameof(fragmentShader));

            if (vertexShader.Context != fragmentShader.Context) throw new ArgumentException("context mismatch");

            var ctx = vertexShader.Context;
            if (ctx == null) throw new ObjectDisposedException(nameof(vertexShader));

            ctx.ThrowOnError();            

            var programId = ctx.CreateProgram();
            ctx.ThrowOnError();

            ctx.AttachShader(programId, vertexShader._ShaderId);
            ctx.ThrowOnError();

            ctx.AttachShader(programId, fragmentShader._ShaderId);
            ctx.ThrowOnError();

            ctx.LinkProgram(programId);
            ctx.ThrowOnError();

            //Checking the linking for errors.
            ctx.GetProgram(programId, GLEnum.LinkStatus, out var status);
            if (status == 0)
            {
                var log = ctx.GetProgramInfoLog(programId);
                throw new InvalidOperationException($"Error linking shader {log}");
            }

            // after detaching the shaders we could delete them;
            ctx.DetachShader(programId, vertexShader._ShaderId);
            ctx.ThrowOnError();

            ctx.DetachShader(programId, fragmentShader._ShaderId);
            ctx.ThrowOnError();

            return new ShaderProgram(ctx, programId);
        }

        private ShaderProgram(OPENGL gl, uint programId)
            : base(gl)
        {
            _ProgramId = programId;
        }

        protected override void Dispose(OPENGL gl)
        {
            if (_ProgramId != 0)
            {
                gl?.DeleteProgram(_ProgramId);
                _ProgramId = 0;
            }

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
        
