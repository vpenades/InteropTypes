using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    public class Buffer : ContextProvider
    {
        #region lifecycle

        public Buffer(OPENGL gl, BufferUsageARB usage) : base(gl)
        {
            _Usage = usage;
        }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteBuffer(_BufferId);
            _BufferId = 0;

            base.Dispose(gl);
        }

        #endregion

        #region data

        private uint _BufferId;
        private BufferTargetARB _Target;
        private BufferUsageARB _Usage;

        #endregion

        #region API

        public virtual void Use()
        {
            //Binding the buffer.
            Context.BindBuffer(_Target, _BufferId);
        }

        protected unsafe void SetData<T>(ReadOnlySpan<T> data, BufferTargetARB target)
            where T : unmanaged
        {
            _Target = target;

            //Creating the buffer.
            if (_BufferId == 0) _BufferId = Context.GenBuffer();

            Use();            

            Context.BufferData(target, data, _Usage);
        }

        #endregion
    }

    public class VertexBuffer : Buffer
    {
        #region lifecycle

        public VertexBuffer(OPENGL gl, BufferUsageARB usage)
            : base(gl, usage)
        { }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteVertexArray(_VerticesId);
            _VerticesId = 0;

            base.Dispose(gl);
        }

        #endregion

        #region data

        private uint _VerticesId;

        private Type _VertexType;
        private VertexElement[] _Elements;

        #endregion

        #region API

        public void SetData<T>(List<T> data)
            where T : unmanaged, VertexElement.ISource
        {
            #if NETSTANDARD
            SetData<T>(data.ToArray());
            #else
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(data);
            SetData<T>(span);
            #endif
        }

        public void SetData<T>(Span<T> data)
            where T : unmanaged, VertexElement.ISource
        {
            var rdata = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref data[0], data.Length);

            SetData<T>(rdata);
        }

        public void SetData<T>(ReadOnlySpan<T> data)
            where T : unmanaged, VertexElement.ISource
        {
            //Creating a vertex array.
            if (_VerticesId == 0) _VerticesId = Context.GenVertexArray();

            Context.BindVertexArray(_VerticesId);

            SetData(data, BufferTargetARB.ArrayBuffer);

            if (_VertexType != typeof(T))
            {
                _VertexType = typeof(T);
                _Elements = default(T).GetElements().ToArray();

                uint offset = 0;

                for (int i = 0; i < _Elements.Length; ++i)
                {
                    _Elements[i] = _Elements[i].WithIndex(offset);
                    offset += _Elements[i].ByteSize;
                }
            }
        }

        public unsafe override void Use()
        {
            Context.BindVertexArray(_VerticesId);

            base.Use();

            if (_Elements != null)
            {
                //Tell opengl how to give the data to the shaders.
                foreach (var element in _Elements)
                {
                    element.Set(Context);
                }
            }
        }

        #endregion
    }

    public class IndexBuffer : Buffer
    {
        #region lifecycle

        public IndexBuffer(OPENGL gl, BufferUsageARB usage)
            : base(gl, usage)
        { }

        #endregion

        #region data

        public PrimitiveType Mode { get; private set; }
        public DrawElementsType Encoding { get; private set; }
        public int Count { get; private set; }

        #endregion

        #region API

        public void SetData<T>(List<T> data, PrimitiveType mode)
            where T : unmanaged
        {
            #if NETSTANDARD
            SetData<T>(data.ToArray(), mode);
            #else
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(data);
            SetData<T>(span, mode);
            #endif
        }

        public void SetData<T>(Span<T> data, PrimitiveType mode)
            where T : unmanaged
        {
            var rdata = System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref data[0], data.Length);

            SetData<T>(rdata, mode);
        }

        public unsafe void SetData<T>(ReadOnlySpan<T> data, PrimitiveType mode)
            where T : unmanaged
        {
            Mode = mode;
            Count = data.Length;

            switch (sizeof(T))
            {
                case 1: Encoding = DrawElementsType.UnsignedByte; break;
                case 2: Encoding = DrawElementsType.UnsignedShort; break;
                case 4: Encoding = DrawElementsType.UnsignedInt; break;
                default: throw new InvalidOperationException($"{typeof(T).Name} not valid");
            }

            SetData(data, BufferTargetARB.ElementArrayBuffer);
        }

        #endregion
    }
}
