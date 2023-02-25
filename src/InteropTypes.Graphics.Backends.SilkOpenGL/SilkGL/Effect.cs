using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public abstract class Effect : IDisposable
    {
        #region lifecycle

        public Effect() { }

        protected UniformFactory CreateProgram(OPENGL gl, System.Reflection.Assembly resAssembly, string vname, string fname)
        {
            var shader = new ShaderProgram(gl);
            shader.SetShadersFrom(resAssembly, vname, fname);
            _Program = shader;

            return _Program.UniformFactory;
        }

        public void Dispose()
        {
            _Program?.Dispose();
            _Program = null;
        }

        #endregion

        #region data

        private ShaderProgram _Program;

        /// <summary>
        /// This method must set the uniforms that are fixed to the shader program and are,
        /// to some degree part of the 'material definition' like textures, samplers, colors,
        /// etc. In other words, properties that don't change over time        
        /// </summary>
        protected abstract void CommitStaticUniforms();

        /// <summary>
        /// Exposes an API that can be used to dynamically change the uniforms that change
        /// continuosly, like transform matrices, lights, etc.
        /// </summary>
        /// <returns></returns>
        protected abstract IEffectUniforms UseDynamicUniforms();

        #endregion

        #region API

        public DrawingAPI Using()
        {
            return new DrawingAPI(this);
        }

        public struct DrawingAPI : IDisposable
        {
            #region lifecycle
            internal DrawingAPI(Effect effect)
            {
                _Effect = effect;

                _Effect._Program.Bind();
                _Effect.CommitStaticUniforms();
                _Uniforms = _Effect.UseDynamicUniforms();
            }

            public void Dispose()
            {
                _Effect._Program.Unbind();
                _Effect = null;
                _Uniforms = null;
            }

            #endregion

            #region data

            private Effect _Effect;
            private IEffectUniforms _Uniforms;

            #endregion

            #region API

            public IEffectUniforms Uniforms => _Uniforms;

            public IDrawingContext UseDC(VertexBufferArray vertices, IndexBuffer indices)
            {
                // TODO: compare effect.program vertices with vertices

                return new DrawingContext(vertices, indices);
            }

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// Cast to <see cref="IEffectTransforms3D"/>
    /// </summary>
    public interface IEffectUniforms
    {

    }

    public interface IEffectTransforms3D
    {
        void SetModelMatrix(Matrix4x4 matrix);

        void SetViewMatrix(Matrix4x4 matrix);

        void SetProjMatrix(Matrix4x4 matrix);
    }
}
