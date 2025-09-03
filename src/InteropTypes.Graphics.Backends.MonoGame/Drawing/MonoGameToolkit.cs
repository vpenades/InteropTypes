using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;



namespace InteropTypes.Graphics.Backends
{
    public static partial class MonoGameToolkit
    {
        /// <summary>
        /// Takes a sprite texture image source (a string path, a raw texture, etc) and adds additional attributes
        /// </summary>
        /// <param name="source">The value of <see cref="ImageSource.Source"/></param>
        /// <param name="sampler">The sampler to associate with the image</param>
        /// <param name="textureBleed">the texture bleed to associate with the image</param>
        /// <returns>A new value that replaces <see cref="ImageSource.Source"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object CreateSpriteAssetSource(object source, SamplerState sampler = null, float textureBleed = 0)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source is MonoGameSpriteTexture mgsa)
            {
                mgsa.Attributes = new SpriteTextureAttributes(sampler, textureBleed);
                return mgsa;
            }

            // at this point, source must be:
            // - String
            // - System.IO.FileInfo
            // - Microsoft.Extensions.FileProviders.IFileInfo
            // - Func<System.IO.Stream>
            // - (Assembly,string)

            return new MonoGameSpriteTexture(source, sampler ?? SamplerState.LinearClamp, textureBleed);
        }

        internal static (Texture2D Texture, SpriteTextureAttributes Attributes) _CreateStaticTexture(GraphicsDevice gd, object imageSource)
        {
            var attr = SpriteTextureAttributes.Default;

            if (imageSource is MonoGameSpriteTexture mgasset)
            {
                attr = mgasset.Attributes;
                imageSource = mgasset.Source;
            }

            switch (imageSource)
            {
                case COLOR gdicolor:
                    {
                        var tex = _CreateSolidTexture(gd, 16, 16, gdicolor.ToXna());
                        return (tex, attr);
                    }

                case XNACOLOR xnacolor:
                    {
                        var tex = _CreateSolidTexture(gd, 16, 16, xnacolor);
                        return (tex, attr);
                    }

                case Func<System.IO.Stream> reader:
                    {
                        var tex = _LoadTexture(gd, reader);
                        if (tex != null) return (tex, attr);
                        break;
                    }

                case System.IO.FileInfo finfo:
                    {
                        var tex = _LoadTexture(gd, finfo.OpenRead);
                        if (tex != null) return (tex, attr);
                        break;
                    }

                case Microsoft.Extensions.FileProviders.IFileInfo xinfo:
                    {
                        var tex = _LoadTexture(gd, xinfo.CreateReadStream);
                        if (tex != null) return (tex, attr);
                        break;
                    }                
            }

            var texx = _LoadTexture(gd, () => ImageSource.TryOpenRead(imageSource));
            if (texx != null) return (texx, attr);

            // interfaces must be checked at the very end because they're fallbacks

            if (imageSource is Bitmaps.SpanBitmap.ISource ibmp)
            {
                if (TryCreateTexture(ibmp.AsSpanBitmap(), gd, out var tex))
                {
                    return (tex, attr);
                }
            }

            throw new NotImplementedException($"Unknown source: {imageSource}");
        }

        private static Texture2D _LoadTexture(GraphicsDevice gd, Func<System.IO.Stream> openImageFunc)
        {
            if (openImageFunc == null) return null;

            Texture2D tex;

            using (var s = openImageFunc.Invoke())
            {
                if (s == null) return null;

                // Notice that Texture2D.FromStream has this:
                //    Texture2D.FromStream(gd, s, DefaultColorProcessors.PremultiplyAlpha);
                // But looking at the code, it blindly assumes the data is of type RGBA, so it will
                // mess with any other data type like LA16 used by the fonts.

                tex = Texture2D.FromStream(gd, s); // defaults to DefaultColorProcessors.ZeroTransparentPixels
                tex.PremultiplyAlpha(); // NOTE: if it's a DDS texture we should not do this.                
            }            

            return tex;
        }        

        internal static Texture2D _CreateSolidTexture(GraphicsDevice gd, int width, int height, XNACOLOR fillColor)
        {
            var tex = new Texture2D(gd, width, height, false, SurfaceFormat.Bgra4444);

            var data = new ushort[width * height];

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = 0xffff;
            }

            tex.SetData(data);

            return tex;
        }

        public static IMonoGameCanvas2D CreateCanvas2D(this GraphicsDevice device)
        {
            return new MonoGameDrawing2D(device);
        }

        public static IMonoGameScene3D CreateScene3D(this GraphicsDevice device)
        {
            return new MonoGameDrawing3D(device);
        }
    }
}
