using System;
using System.Collections.Generic;

using OPENGL = Silk.NET.OpenGL.GL;

using VEC2 = System.Numerics.Vector2;
using VEC3 = System.Numerics.Vector3;
using VEC4 = System.Numerics.Vector4;
using MAT4X4 = System.Numerics.Matrix4x4;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public readonly struct UniformFactory
    {
        #region diagnostics

        private static readonly Dictionary<OPENGL, uint> _CurrentProgram = new Dictionary<OPENGL, uint>();

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void VerifyIsCurrentProgram(OPENGL gl, uint program)
        {
            return;

            if (!_CurrentProgram.TryGetValue(gl, out uint currProgId)) throw new InvalidOperationException("Wrong program");

            if (program != currProgId) throw new InvalidOperationException("Wrong program");            
        }

        #endregion

        #region lifecycle

        internal UniformFactory(OPENGL context, uint program)
        {
            _Context = context;
            _ProgramId= program;
        }

        #endregion

        #region data

        private readonly OPENGL _Context;
        private readonly uint _ProgramId;

        #endregion

        #region API

        private int UseLocation(string name)
        {
            //Setting a uniform on a shader using a name.
            int location = _Context.GetUniformLocation(_ProgramId, name);

            if (location == -1) //If GetUniformLocation returns -1 the uniform is not found.
            {
                throw new ArgumentException($"{name} uniform not found on shader.", nameof(name));
            }

            return location;
        }

        public UniformTexture UseTexture(string name)
        {
            return new UniformTexture(this._Context, _ProgramId, UseLocation(name));
        }

        public UniformVector<float> UseScalar(string name)
        {
            return new UniformVector<float>(this._Context, _ProgramId, UseLocation(name));
        }

        public UniformVector<VEC2> UseVector2(string name)
        {
            return new UniformVector<VEC2>(this._Context, _ProgramId, UseLocation(name));
        }

        public UniformVector<VEC3> UseVector3(string name)
        {
            return new UniformVector<VEC3>(this._Context, _ProgramId, UseLocation(name));
        }

        public UniformVector<VEC4> UseVector4(string name)
        {
            return new UniformVector<VEC4>(this._Context, _ProgramId, UseLocation(name));
        }

        public UniformMatrix<MAT4X4> UseMatrix4x4(string name, bool transpose)
        {
            return new UniformMatrix<MAT4X4>(this._Context, _ProgramId, UseLocation(name), transpose);
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{_Value}")]
    public struct UniformTexture
    {
        #region constructor

        internal UniformTexture(OPENGL gl, uint program, int index)
        {
            _gl = gl;
            _Program = program;
            _Index = index;            

            #if DEBUG            
            _Value = default;            
            #endif
        }

        #endregion

        #region data

        private readonly OPENGL _gl;
        internal uint _Program;
        private readonly int _Index;        

        #if DEBUG
        private int _Value;
        #endif

        #endregion

        #region API
        public void Set(int slotIndex)
        {
            #if DEBUG
            _Value = slotIndex;
            #endif

            _gl.ThrowOnError();
            _gl.Uniform1(_Index, slotIndex);
            _gl.ThrowOnError();
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{_Value}")]
    public struct UniformVector<T>
        where T:unmanaged
    {
        #region constructor
        internal UniformVector(OPENGL gl, uint program, int index)
        {
            _gl = gl;
            _Program = program;
            _Index = index;

            #if DEBUG
            _Value = default;
            #endif
        }

        #endregion

        #region data

        private readonly OPENGL _gl;
        private readonly uint _Program;
        private readonly int _Index;

        #if DEBUG        
        private T _Value;
        #endif

        #endregion

        #region API

        public unsafe void Set(T value)
        {
            #if DEBUG
            _Value = value;
            #endif

            UniformFactory.VerifyIsCurrentProgram(_gl, _Program);

            _gl.ThrowOnError();

            switch(value)
            {
                case Single typedVal: _gl.Uniform1(_Index, typedVal); break;
                case VEC2 typedVal: _gl.Uniform2(_Index, typedVal); break;
                case VEC3 typedVal: _gl.Uniform3(_Index, typedVal); break;
                case VEC4 typedVal: _gl.Uniform4(_Index, typedVal); break;
            }

            _gl.ThrowOnError();
        }

        #endregion
    }

    [System.Diagnostics.DebuggerDisplay("{_Value}")]
    public struct UniformMatrix<T>
        where T : unmanaged
    {
        #region constructor
        internal UniformMatrix(OPENGL gl, uint program, int index, bool transpose)
        {
            _gl = gl;
            _Program = program;
            _Index = index;
            _Transpose = transpose;

            #if DEBUG            
            _Value = default;
            #endif
        }
        #endregion

        #region data

        private readonly OPENGL _gl;
        private readonly uint _Program;
        private readonly int _Index;
        private readonly bool _Transpose;

        #if DEBUG        
        private T _Value;
        #endif

        #endregion

        #region API

        public unsafe void Set(T value)
        {
            #if DEBUG
            _Value = value;
            #endif

            UniformFactory.VerifyIsCurrentProgram(_gl, _Program);

            if (typeof(T) == typeof(MAT4X4))
            {
                _gl.ThrowOnError();
                _gl.UniformMatrix4(_Index, 1, _Transpose, (float*)&value);
                _gl.ThrowOnError();
            }
        }

        #endregion
    }
}
