﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace Tutorial
{
    internal class Effect0 : Effect
    {
        #region lifecycle

        public Effect0(OPENGL gl) : base(gl)
        {
            var ufactory = CreateProgram(System.Reflection.Assembly.GetExecutingAssembly(), "Effect0.Shader.vert", "Effect0.Shader.frag");

            _Vertex = new _VertexUniforms(ufactory);
        }

        #endregion

        #region data

        private _VertexUniforms _Vertex;

        #endregion

        #region API

        protected override (IEffectUniforms Vertex, IEffectUniforms Fragment) UseDynamicUniforms()
        {
            return (_Vertex, null);
        }

        sealed class _VertexUniforms : IEffectUniforms, IEffectTransforms3D
        {
            public _VertexUniforms(UniformFactory ufactory)
            {
                _uModel = ufactory.UseMatrix4x4("uModel", true);
            }

            private UniformMatrix<Matrix4x4> _uModel;

            public void OnBind(Effect effect)
            {
                if (effect is not Effect0) throw new ArgumentException("type mismatch", nameof(effect));

                SetModelMatrix(Matrix4x4.Identity);
            }

            public void SetModelMatrix(Matrix4x4 matrix)
            {
                _uModel.Set(matrix);
            }

            public void SetProjMatrix(Matrix4x4 matrix) { }

            public void SetViewMatrix(Matrix4x4 matrix) { }            
        }

        #endregion
    }
}
