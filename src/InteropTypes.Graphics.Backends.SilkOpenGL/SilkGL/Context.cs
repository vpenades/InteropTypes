using System;
using System.Collections.Generic;
using System.Text;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public abstract class ContextProvider : IDisposable
    {
        #region lifecycle

        public ContextProvider(OPENGL gl)
        {
            _gl = gl;
        }

        public void Dispose()
        {
            var gl = System.Threading.Interlocked.Exchange(ref _gl, null);            

            if (gl != null) Dispose(gl);
        }

        protected virtual void Dispose(OPENGL gl) { }

        #endregion

        #region data

        private OPENGL _gl;

        #endregion

        #region properties

        public OPENGL Context
        {
            get
            {
                System.Diagnostics.Debug.Assert(_gl != null, "Object disposed");
                return _gl;
            }
        }

        #endregion
    }
}
