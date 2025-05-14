using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public abstract class Effect : ContextProvider
    {
        #region lifecycle

        public Effect(OPENGL gl) : base(gl)
        {
            _TextureSlots = new TextureGroup(gl);
        }

        protected UniformFactory CreateProgram(System.Reflection.Assembly resAssembly, string vname, string fname)
        {
            _Program = ShaderProgram.CreateFrom(this.Context, resAssembly,vname,fname);

            return _Program.UniformFactory;
        }

        protected UniformFactory CreateProgram(string vbody, string fbody)
        {
            if (string.IsNullOrWhiteSpace(vbody)) throw new ArgumentNullException(nameof(vbody));
            if (string.IsNullOrWhiteSpace(fbody)) throw new ArgumentNullException(nameof(fbody));

            _Program = ShaderProgram.CreateFromCode(this.Context, vbody, fbody);

            return _Program.UniformFactory;
        }

        protected override void Dispose(OPENGL gl)
        {
            _Program?.Dispose();
            _Program = null;

            base.Dispose(gl);
        }

        #endregion

        #region data

        private ShaderProgram _Program;

        private TextureGroup _TextureSlots;

        #endregion

        #region properties

        public TextureGroup Slots => _TextureSlots;

        #endregion

        #region API

        /// <summary>
        /// Exposes an API that can be used to dynamically change the uniforms that change
        /// continuosly, like transform matrices, lights, etc.
        /// </summary>
        /// <returns></returns>
        protected abstract (IUniforms Vertex, IUniforms Fragment) UseDynamicUniforms();

        #endregion

        #region API

        /// <summary>
        /// Creates a temporary object that can be used for rendering
        /// </summary>
        /// <returns></returns>
        public DrawingAPI Using()
        {
            if (_Program?.Context == null) throw new ObjectDisposedException("Program");            

            return new DrawingAPI(this);
        }

        /// <summary>
        /// Effects Drawing API
        /// </summary>
        /// <remarks>
        /// Use <see cref="Using"/> to initialize.
        /// </remarks>
        public struct DrawingAPI : IDisposable
        {
            #region lifecycle
            internal DrawingAPI(Effect effect)
            {
                _Slots = effect._TextureSlots;
                _Slots.Bind();
                
                _Program = effect._Program;
                _Program.Bind();                
                
                (_VertexUniforms, _FragmentUniforms) = effect.UseDynamicUniforms();

                if (_FragmentUniforms is IUniformTextures tex)
                {
                    tex.BindTextures(_Slots);
                }
            }

            public void Dispose()
            {
                if (_FragmentUniforms is IUniformTextures utex) utex.UnbindTextures();

                _Program.Unbind();
                _Program = null;

                _Slots.Unbind();
                _Slots = null;

                _VertexUniforms = null;
                _FragmentUniforms = null;
            }

            #endregion

            #region data

            private TextureGroup _Slots;
            private ShaderProgram _Program;
            private IUniforms _VertexUniforms;
            private IUniforms _FragmentUniforms;

            #endregion

            #region API

            // split between VertexUniforms and FragmentUniforms

            public IUniforms VertexUniforms => _VertexUniforms;
            public IUniforms FragmentUniforms => _FragmentUniforms;

            public IDrawingContext UseDC(VertexBufferArray vertices, IndexBuffer indices)
            {
                GuardCompatible(_Program, vertices);
                GuardCompatible(_Program, indices);

                // TODO: compare effect.program vertices with vertices

                return new DrawingContext(vertices, indices);
            }

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// Cast to <see cref="IUniformTransforms3D"/>
    /// </summary>
    public interface IUniforms
    {
        string GetShaderCode();
        void Initialize(UniformFactory ufactory);
    }

    public interface IUniformTransforms3D
    {
        void SetModelMatrix(Matrix4x4 matrix);

        void SetViewMatrix(Matrix4x4 matrix);

        void SetProjMatrix(Matrix4x4 matrix);

        void SetCameraMatrix(Matrix4x4 matrix)
        {
            if (!Matrix4x4.Invert(matrix, out var inverted)) throw new ArgumentException("Invalid matrix", nameof(matrix));
            SetViewMatrix(inverted);
        }
    }

    public interface IUniformTextures
    {
        void BindTextures(TextureGroup slots);

        void UnbindTextures();
    }
}
