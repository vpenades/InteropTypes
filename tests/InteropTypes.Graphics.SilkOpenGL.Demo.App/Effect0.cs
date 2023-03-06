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
    internal class Effect0 : Effect
    {
        #region lifecycle

        public Effect0(OPENGL gl) : base(gl)
        {
            _Vertex = new _PxM_Uniforms();
            _Fragment = new _Zero_Debug_Uniforms();

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

        protected override (IUniforms Vertex, IUniforms Fragment) UseDynamicUniforms()
        {
            return (_Vertex, _Fragment);
        }

        #endregion
    }
}
