using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends.SilkGL;

using Tutorial.Shaders;

using OPENGL = Silk.NET.OpenGL.GL;

namespace Tutorial
{
    internal class Effect1 : Effect
    {
        #region lifecycle

        public Effect1(OPENGL gl) : base(gl)
        {
            _Vertex = new _PTxMVP_T_Uniforms();
            _Fragment = new _T_Unlit_Uniforms();

            var ufactory = CreateProgram(_Vertex.GetShaderCode(), _Fragment.GetShaderCode());

            _Vertex.Initialize(ufactory);
            _Fragment.Initialize(ufactory);
        }

        #endregion

        #region data

        private IUniforms _Vertex;
        private IUniforms _Fragment;

        #endregion

        #region API

        public Texture SolidTexture { get; set; }

        protected override (IUniforms Vertex, IUniforms Fragment) UseDynamicUniforms()
        {
            if (_Fragment is IUniformTextures tex)
            {
                tex.SetTexture(SolidTexture);
            }

            return (_Vertex, _Fragment);
        }

        #endregion
    }
}
