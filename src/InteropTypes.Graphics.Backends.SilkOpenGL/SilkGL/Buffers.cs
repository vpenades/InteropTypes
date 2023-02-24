using System;
using System.Collections.Generic;

using InteropTypes.Graphics.Backends.SilkOpenGL.SilkGL;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplay(),nq}")]
    public readonly struct BufferInfo
    {
        #region debug

        internal string _ToDebuggerDisplay()
        {
            if (Id == 0) return "<DISPOSED>";
            return $"{Id} => {Target} {Usage}";
        }

        #endregion

        #region constructor

        public static BufferInfo Create(OPENGL gl, BufferTargetARB target, BufferUsageARB usage)
        {
            gl.ThrowOnError();
            var id = gl.GenBuffer();
            gl.ThrowOnError();
            return new BufferInfo(id, target,usage);            
        }

        public BufferInfo(uint id, BufferTargetARB target, BufferUsageARB usage)
        {
            Id = id;
            Target = target;
            Usage = usage;
        }

        #endregion

        #region data

        public readonly uint Id;
        public readonly BufferTargetARB Target;
        public readonly BufferUsageARB Usage;

        #endregion

        #region API

        internal BoundAPI _Bind(OPENGL gl) { gl.ThrowOnError(); return new BoundAPI(gl, this); }        

        public readonly struct BoundAPI : IDisposable
        {
            #region lifecycle

            internal BoundAPI(OPENGL context, in BufferInfo info)
            {
                _Context = context;
                _Info = info;
                
                _Context.BindBuffer(_Info.Target, info.Id);
                _Context.ThrowOnError();

                _Guard.Bind(_Info.Target, info.Id);
            }

            public void Dispose()
            {
                _Guard.Unbind(_Info.Target);

                _Context.BindBuffer(_Info.Target, 0);
                _Context.ThrowOnError();                
            }

            private static _BindingsGuard<BufferTargetARB, uint> _Guard = new _BindingsGuard<BufferTargetARB, uint>();

            #endregion

            #region data

            private readonly OPENGL _Context;
            private readonly BufferInfo _Info;

            #endregion

            #region API

            public void SetData<T>(List<T> data)
            where T : unmanaged
            {
                #if NETSTANDARD
                SetData<T>(data.ToArray());
                #else
                var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(data);
                SetData<T>(span);
                #endif
            }

            public void SetData<T>(T[] data)
                where T : unmanaged
            {
                SetData((ReadOnlySpan<T>)data);
            }

            public void SetData<T>(Span<T> data)
                where T : unmanaged
            {
                SetData((ReadOnlySpan<T>)data);
            }

            public void SetData<T>(ReadOnlySpan<T> data)
                where T : unmanaged
            {
                _Context.ThrowOnError();
                _Context.BufferData(_Info.Target, data, _Info.Usage);
                _Context.ThrowOnError();
            }

            #endregion
        }

        #endregion
    }

    public abstract class Buffer : ContextProvider
    {
        #region lifecycle

        public Buffer(OPENGL gl, BufferTargetARB target, BufferUsageARB usage) : base(gl)
        {
            _Info = BufferInfo.Create(Context, target, usage);
        }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteBuffer(_Info.Id);
            _Info = default;

            base.Dispose(gl);
        }

        #endregion

        #region data

        protected BufferInfo _Info;

        public int UpdateVersion { get; private set; }

        #endregion

        #region API

        public BufferInfo.BoundAPI Using()
        {
            return _Info._Bind(Context);
        }

        #endregion
    }

    public class IndexBuffer : Buffer
    {
        #region lifecycle

        public IndexBuffer(OPENGL gl, BufferUsageARB usage)
            : base(gl, BufferTargetARB.ElementArrayBuffer, usage)
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
            this.SetData<T>(data.ToArray(), mode);
            #else
            var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(data);
            this.SetData<T>(span, mode);
            #endif
        }

        public void SetData<T>(Span<T> data, PrimitiveType mode)
            where T : unmanaged
        {
            this.SetData((ReadOnlySpan<T>)data, mode);
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

            using (var api = Using()) api.SetData(data);
        }        

        #endregion
    }

    public class VertexBuffer : Buffer
    {
        #region lifecycle

        protected VertexBuffer(OPENGL gl, BufferUsageARB usage)
            : base(gl, BufferTargetARB.ArrayBuffer, usage)
        { }

        #endregion        
    }

    public class VertexBuffer<TVertex> : VertexBuffer
        where TVertex : unmanaged, VertexElement.ISource
    {
        #region lifecycle

        public VertexBuffer(OPENGL gl, BufferUsageARB usage)
            : base(gl, usage)
        { }        

        #endregion

        #region API        

        public void SetData(List<TVertex> data) { using(var api = Using()) api.SetData(data); }

        public void SetData(TVertex[] data) { using (var api = Using()) api.SetData(data); }

        public void SetData(Span<TVertex> data) { using (var api = Using()) api.SetData(data); }

        public void SetData(ReadOnlySpan<TVertex> data) { using (var api = Using()) api.SetData(data); }        

        #endregion
    }

    /// <summary>
    /// Defines the pipeline the shader will use to read vertices from one or multiple <see cref="VertexBuffer"/>
    /// </summary>
    public class VertexLayout : ContextProvider
    {
        #region lifecycle

        public VertexLayout(OPENGL gl)
            : base(gl)
        {
            Context.ThrowOnError();
            _ArrayId = Context.GenVertexArray();
            Context.ThrowOnError();
            System.Diagnostics.Debug.Assert(_ArrayId != 0);            
        }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteVertexArray(_ArrayId);
            _ArrayId = 0;

            base.Dispose(gl);
        }

        #endregion

        #region data        

        internal uint _ArrayId;
        internal readonly VertexElement.Collection _Elements = new VertexElement.Collection();

        #endregion

        #region API

        public void SetLayoutFrom<TVertex>(VertexBuffer vertexBuffer)
            where TVertex : unmanaged, VertexElement.ISource
        {
            // https://community.khronos.org/t/binding-orders-to-update-information-in-multiple-vao-vbo/75066/2

            _Elements.SetElements<TVertex>();

            Context.ThrowOnError();
            Context.BindVertexArray(_ArrayId);
            Context.ThrowOnError();

            using (var api = vertexBuffer.Using())
            {
                _Elements.Set(Context);
            }

            _Elements.Enable(Context);

            Context.BindVertexArray(0);
        }

        public void Bind()
        {
            Context.ThrowOnError();
            Context.BindVertexArray(_ArrayId);
            Context.ThrowOnError();
        }

        public void Unbind()
        {
            Context.ThrowOnError();
            Context.BindVertexArray(0);
            Context.ThrowOnError();
        }        

        #endregion
    }
}
