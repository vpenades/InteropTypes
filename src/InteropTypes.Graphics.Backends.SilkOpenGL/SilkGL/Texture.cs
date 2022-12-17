using System;
using System.Collections.Generic;
using System.Text;

using Silk.NET.OpenGL;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends.SilkGL
{
    [System.Diagnostics.DebuggerDisplay("{_Info}")]
    public class Texture : ContextProvider
    {
        #region lifecycle

        public unsafe Texture(OPENGL gl)
            : base(gl)
        {
            _handle = Context.GenTexture();
            
        }

        protected override void Dispose(OPENGL gl)
        {
            gl?.DeleteTexture(_handle);

            base.Dispose(gl);
        }

        #endregion

        #region data

        internal uint _handle;

        internal InteropTypes.Graphics.Bitmaps.BitmapInfo _Info;

        #endregion

        #region API

        public void SetTexture(Bitmaps.SpanBitmap<Bitmaps.Pixel.RGBA32> bitmap)
        {
            if (bitmap.IsEmpty) throw new ArgumentNullException(nameof(bitmap));
            if (!bitmap.Info.IsContinuous)
            {
                bitmap = bitmap.ToMemoryBitmap();
            }

            _Info = bitmap.Info;


            Bind();

            System.Diagnostics.Debug.Assert(bitmap.Info.IsContinuous);

            var d = bitmap.ReadableBytes;

            uint w = (uint)bitmap.Width;
            uint h = (uint)bitmap.Height;

            Context.TexImage2D(
                TextureTarget.Texture2D,
                0,
                (int)InternalFormat.Rgba,
                w, h,
                0, // value between 0 and 1, to enable glTexParameter(), with the GL_TEXTURE_BORDER_COLOR 
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                d);

            SetParameters();

            Unbind();
        }

        private void SetParameters()
        {
            Context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
            Context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
            Context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            Context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            Context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            Context.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 8);
            Context.GenerateMipmap(TextureTarget.Texture2D);
        }

        public void Bind(TextureUnit textureSlot = TextureUnit.Texture0)
        {
            Context.ActiveTexture(textureSlot);
            Context.BindTexture(TextureTarget.Texture2D, _handle);            
        }

        public void Unbind()
        {            
            Context.BindTexture(TextureTarget.Texture2D, 0);         
        }

        #endregion
    }
}
