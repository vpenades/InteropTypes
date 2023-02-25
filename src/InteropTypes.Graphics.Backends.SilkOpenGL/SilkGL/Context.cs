using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Backends.SilkOpenGL.SilkGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public abstract class ContextProvider : IDisposable
    {
        #region lifecycle

        public ContextProvider(OPENGL gl)
        {
            if (gl == null) throw new ArgumentNullException(nameof(gl));
            gl.ThrowOnError();
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

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
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

        #region guards

        public static void GuardValid(ContextProvider a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (a.Context == null) throw new ObjectDisposedException(nameof(a));
        }

        public static void GuardCompatible(ContextProvider a, ContextProvider b)
        {
            GuardValid(a);
            GuardValid(b);

            if (a.Context != b.Context) throw new ArgumentException("context mismatch", nameof(b));
        }

        #endregion
    }

    public abstract class BindableResource<TResource> : ContextProvider  
        where TResource : class
    {
        

        protected BindableResource(OPENGL gl) : base(gl)
        {
        }

        private static readonly Dictionary<OPENGL, TResource> _Bound = new Dictionary<OPENGL, TResource>();

        public virtual void Bind()
        {            
            _Bound[Context] = this as TResource;
        }

        public static TResource GetBound(ContextProvider context)
        {
            return _Bound.TryGetValue(context.Context, out var program) ? program : null;
        }

        public virtual void Unbind()
        {            
            _Bound[Context] = null;
        }
    }



    
}
