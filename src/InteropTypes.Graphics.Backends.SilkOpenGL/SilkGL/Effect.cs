﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public abstract class Effect : ContextProvider
    {
        #region lifecycle

        public Effect(OPENGL gl) : base(gl) { }

        protected UniformFactory CreateProgram(System.Reflection.Assembly resAssembly, string vname, string fname)
        {
            _Program = ShaderProgram.CreateFrom(this.Context, resAssembly,vname,fname);

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

        /// <summary>
        /// Exposes an API that can be used to dynamically change the uniforms that change
        /// continuosly, like transform matrices, lights, etc.
        /// </summary>
        /// <returns></returns>
        protected abstract IEffectUniforms UseDynamicUniforms();

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
                _Program = effect._Program;
                _Program.Bind();                
                
                _Uniforms = effect.UseDynamicUniforms();
                _Uniforms.OnBind(effect);
            }

            public void Dispose()
            {
                _Program.Unbind();
                _Program = null;
                _Uniforms = null;
            }

            #endregion

            #region data

            private ShaderProgram _Program;
            private IEffectUniforms _Uniforms;

            #endregion

            #region API

            public IEffectUniforms Uniforms => _Uniforms;

            public IDrawingContext UseDC(VertexBufferArray vertices, IndexBuffer indices)
            {
                ContextProvider.GuardCompatible(_Program, vertices);
                ContextProvider.GuardCompatible(_Program, indices);

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
        void OnBind(Effect effect);
    }

    public interface IEffectTransforms3D
    {
        void SetModelMatrix(Matrix4x4 matrix);

        void SetViewMatrix(Matrix4x4 matrix);

        void SetProjMatrix(Matrix4x4 matrix);
    }
}
