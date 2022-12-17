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

        public virtual void Bind()
        {            
            Context.BindBuffer(_Target, _BufferId);
        }

        public virtual void Unbind()
        {            
            Context.BindBuffer(_Target, 0);
        }

        protected unsafe void SetData<T>(ReadOnlySpan<T> data, BufferTargetARB target)
            where T : unmanaged
        {
            _Target = target;

            //Creating the buffer.
            if (_BufferId == 0) _BufferId = Context.GenBuffer();

            Bind();            

            Context.BufferData(target, data, _Usage);

            Unbind();
        }

        #endregion
    }

    public class VertexBuffer : Buffer
    {
        #region lifecycle

        public VertexBuffer(OPENGL gl, BufferUsageARB usage)
            : base(gl, usage)
        { }

        #endregion

        #region data        

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

        public unsafe override void Bind()
        {
            base.Bind();

            /*

            Because if you keep that last VAO bound over to next frame, and then do some operations on buffers using glBindBuffer() (but without binding appropriate VAO, because you don't have to) you'll be overwriting your currently bound VAO state, which in most cases may cause nasty and hard to track disasters :)

            Edit: Though now that I think about it, I'm not so sure. I think I had issue with overriding VAO state when I kept VAO bound between frames so I started to reset it. At some point I was convinced that only glVertexAttribPointer() and friends affect VAO state, and these require glBindBuffer to work with. But I _think_ had cases where glBindBuffer (because I was uploading new data) somehow messed my other VAO state. So I can't give you 100 % guarantee that what I say is correct, but I think it may be a good practice to know that VAO is enabled when it's needed and when you're done it's 0.

            Edit2: I did a small mistake, it was ELEMENT_ARRAY_BUFFER binding that was the problem, so if you keep VAO bound and somewhere you manipulate some index buffer by binding it, you may override the one pointed to by currently bound VAO.It isn't affected by just binding GL_ARRAY_BUFFER, as I previously speculated.

            */

            // In other words: is this bound to the vertexbuffer, or to the vertex array
            // I think to the vertex array!  but then we must rebuild the VAO of the vertex type changes!

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

    public class PrimitiveBuffer : ContextProvider
    {
        #region lifecycle

        public PrimitiveBuffer(VertexBuffer vb, IndexBuffer ib)
            : base(ib.Context)
        {
            Vertices = vb;
            Indices = ib;

            //Creating a vertex array.
            _ArrayId = Context.GenVertexArray();
            System.Diagnostics.Debug.Assert(_ArrayId != 0);

            this.Bind();
            vb.Bind();
            ib.Bind();
            Unbind(); // prevent subsequent vertex/index bind calls to break this object
        }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteVertexArray(_ArrayId);
            _ArrayId = 0;

            base.Dispose(gl);
        }

        #endregion

        #region data

        private uint _ArrayId;

        internal VertexBuffer Vertices { get; }
        internal IndexBuffer Indices { get; }

        #endregion

        #region API        

        public void Bind()
        {
            Context.BindVertexArray(_ArrayId);            
        }

        public void Unbind()
        {
            Context.BindVertexArray(0);
        }

        #endregion
    }
}
