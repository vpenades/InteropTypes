using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Backends.SilkOpenGL.SilkGL;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    /// <summary>
    /// Represents the low level information associated to an OpenGL Texture.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{_ToDebuggerDisplay(),nq}")]
    public readonly struct TextureInfo
    {
        // there is a similar Silk.NET.OpenGL.Texture  struct, but looks fairly useless other than making it strong typed.

        #region debug

        internal string _ToDebuggerDisplay()
        {
            if (Id == 0) return "<DISPOSED>";
            return $"{Id} => {Target}";
        }

        #endregion

        #region constructor

        public static TextureInfo Create(OPENGL gl, TextureTarget target)
        {
            gl.ThrowOnError();
            var id = gl.GenTexture();
            gl.ThrowOnError();
            return new TextureInfo(id, target);
        }

        public TextureInfo(uint id, TextureTarget target)
        {
            Id = id;
            Target = target;         
        }        

        public static void Release(OPENGL gl, ref TextureInfo binfo)
        {
            if (binfo.Id == 0) return;

            gl?.DeleteTexture(binfo.Id);
            binfo = default;
        }

        #endregion

        #region data

        public readonly uint Id;
        public readonly TextureTarget Target;

        public static readonly TextureInfo EmptyTexture2D = new TextureInfo(0, TextureTarget.Texture2D);

        #endregion

        #region API

        private static GetPName _GetTargetBinding(TextureTarget target)
        {
            switch(target)
            {
                case TextureTarget.Texture1D: return GetPName.TextureBinding1D;
                case TextureTarget.Texture1DArray: return GetPName.TextureBinding1DArray;
                case TextureTarget.Texture2D: return GetPName.TextureBinding2D;
                case TextureTarget.Texture2DArray: return GetPName.TextureBinding2DArray;
                case TextureTarget.Texture3D: return GetPName.TextureBinding3D;                
                case TextureTarget.TextureCubeMap: return GetPName.TextureBindingCubeMap;                
                default: throw new NotImplementedException();
            }
        }

        public UpdateAPI Using(OPENGL gl)
        {
            if (Id == 0) throw new ObjectDisposedException(nameof(Id));

            gl.ThrowOnError();
            return new UpdateAPI(gl, this);
        }

        public readonly struct UpdateAPI : IDisposable
        {
            #region lifecycle

            internal UpdateAPI(OPENGL context, in TextureInfo info)
            {
                // recover texture currently bound to the target
                var prevTexture = 0;
                context.GetInteger(_GetTargetBinding(info.Target), out prevTexture);
                context.ThrowOnError();

                _Context = context;
                _Info = info;
                _PreviousBinding = (uint)prevTexture;

                if (_PreviousBinding != info.Id)
                {
                    _Context.BindTexture(_Info.Target, info.Id);
                    _Context.ThrowOnError();
                }

                _Guard.Bind(_Info.Target, info.Id);
            }

            public void Dispose()
            {
                _Guard.Unbind(_Info.Target);

                if (_PreviousBinding != _Info.Id)
                {
                    _Context.BindTexture(_Info.Target, _PreviousBinding);
                    _Context.ThrowOnError();
                }
            }

            private static _BindingsGuard<TextureTarget, uint> _Guard = new _BindingsGuard<TextureTarget, uint>();

            #endregion

            #region data

            private readonly OPENGL _Context;
            private readonly TextureInfo _Info;
            private readonly uint _PreviousBinding;

            #endregion

            #region API

            public void SetPixels(Bitmaps.SpanBitmap<Bitmaps.Pixel.RGBA32> bitmap)
            {
                if (bitmap.IsEmpty) throw new ArgumentNullException(nameof(bitmap));
                if (!bitmap.Info.IsContinuous)
                {
                    bitmap = bitmap.ToMemoryBitmap();
                }

                System.Diagnostics.Debug.Assert(bitmap.Info.IsContinuous);

                SetPixels(bitmap.Width, bitmap.Height, bitmap.ReadableBytes);
            }

            public void SetPixels(int w, int h, ReadOnlySpan<Byte> data)
            {
                // // https://github.com/WholesomeIsland/Silk.NET.OpenGL.Abstractions/blob/main/Texture.cs

                _Context.ThrowOnError();

                var dataFmt = PixelFormat.Rgba;

                var internalFmt = (InternalFormat)dataFmt;

                _Context.TexImage2D(
                    _Info.Target,
                    0,
                    (int)internalFmt,
                    (uint)w,
                    (uint)h,
                    0, // value between 0 and 1, to enable glTexParameter(), with the GL_TEXTURE_BORDER_COLOR 
                    dataFmt,
                    PixelType.UnsignedByte,
                    data);

                _Context.ThrowOnError();

                _Context.Finish();

                _Context.ThrowOnError();

                // determine mipmap levels
                int s = Math.Min(w, h);
                int l = 0;
                while (s > 2) { s /= 2; l += 1; }

                GenerateMipmaps(l);
            }

            private void GenerateMipmaps(int levels)
            {
                _Context.ThrowOnError();

                _Context.TexParameter(_Info.Target, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
                _Context.TexParameter(_Info.Target, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);

                _Context.TexParameter(_Info.Target, TextureParameterName.TextureBaseLevel, 0);
                _Context.TexParameter(_Info.Target, TextureParameterName.TextureMaxLevel, levels);

                _Context.ThrowOnError();

                _Context.GenerateMipmap(_Info.Target);

                _Context.ThrowOnError();
            }

            public void SetWrapping(TextureWrapMode s, TextureWrapMode t)
            {
                _Context.TexParameter(_Info.Target, TextureParameterName.TextureWrapS, (int)s);
                _Context.TexParameter(_Info.Target, TextureParameterName.TextureWrapT, (int)t);
            }

            #endregion
        }        

        #endregion
    }


    [System.Diagnostics.DebuggerDisplay("{_Handle}")]
    public class Texture : ContextProvider
    {
        #region lifecycle        

        public unsafe Texture(OPENGL gl)
            : base(gl)
        {
            _Handle = TextureInfo.Create(gl, TextureTarget.Texture2D);
        }

        protected override void Dispose(OPENGL gl)
        {
            TextureInfo.Release(gl, ref _Handle);

            base.Dispose(gl);
        }

        #endregion

        #region data

        internal TextureInfo _Handle;        

        private TextureUnit? _Bound;

        #endregion

        #region API        

        public TextureInfo.UpdateAPI Using()
        {
            return _Handle.Using(this.Context);
        }        

        #endregion
    }


    public class TextureGroup : ContextProvider
    {
        public TextureGroup(OPENGL gl) : base(gl)
        {
            gl.GetInteger(GetPName.MaxTextureImageUnits, out var texture_units);

            _MaxSlots = texture_units;
        }

        private readonly Dictionary<string, Texture> _Textures = new Dictionary<string, Texture>();

        private readonly int _MaxSlots;

        private readonly List<Texture> _Slots = new List<Texture>();        

        public void SetTextures(params (string,Texture)[] texs)
        {
            _Textures.Clear();
            foreach (var tex in texs)
            {
                if (tex.Item2 == null) continue;
                _Textures[tex.Item1] = tex.Item2;
            }
        }

        public void Bind()
        {
            if (_Slots.Count != 0) throw new InvalidOperationException("already bound");

            foreach(var t in _Textures.Values)
            {
                if (_Slots.Contains(t)) continue;

                int slotIdx = _Slots.Count;
                _Apply(slotIdx, t._Handle);
                _Slots.Add(t);
            }

            for (int i=_Slots.Count; i < _MaxSlots; i++)
            {
                _Apply(i, TextureInfo.EmptyTexture2D);
            }
        }        

        public int GetSlot(string name)
        {
            if (!_Textures.TryGetValue(name, out var texture)) return _Slots.Count;
            var idx = _Slots.IndexOf(texture);
            return idx < 0 ? _Slots.Count : idx;
        }

        public void Unbind()
        {
            for(int i=0; i < _Slots.Count; i++)
            {
                var tinfo = new TextureInfo(0, _Slots[i]._Handle.Target);
                _Apply(i, tinfo);
            }

            _Slots.Clear();
        }

        private void _Apply(int slotIdx, TextureInfo tinfo)
        {
            Context.ThrowOnError();
            Context.ActiveTexture(TextureUnit.Texture0 + slotIdx);
            Context.ThrowOnError();
            Context.BindTexture(tinfo.Target, tinfo.Id);
            Context.ThrowOnError();
        }

    }
}
