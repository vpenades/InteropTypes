using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Drawing;



namespace InteropTypes.Graphics.Backends
{
    public interface IMonoGameCanvas2D :
        IRenderTargetInfo,
        GlobalStyle.ISource,
        IDisposableCanvas2D,
        IMeshCanvas2D,
        ITransformer2D,
        IServiceProvider
    {
        void Begin(int virtualWidth, int virtualHeight, bool keepAspect);
        void SetCamera(System.Numerics.Matrix3x2 camera);        
        void End();
    }

    public interface IMonoGameScene3D :
        IRenderTargetInfo,
        GlobalStyle.ISource,
        IDisposableScene3D,
        // IMeshScene3D,
        IServiceProvider
    {
        void Clear();
        void SetCamera(CameraTransform3D camera);
        void Render();
    }

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

            // at this point, source must be: String or System.IO.FileInfo or Func<System.IO.Stream> or (Assembly,string)

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
                case System.Drawing.Color gdicolor:
                    {
                        var tex = _CreateSolidTexture(gd, 16, 16, gdicolor.ToXna());
                        return (tex, attr);
                    }

                case XNACOLOR xnacolor:
                    {
                        var tex = _CreateSolidTexture(gd, 16, 16, xnacolor);
                        return (tex, attr);
                    }

                case Microsoft.Extensions.FileProviders.IFileInfo xinfo:
                    {
                        var tex = _loadTexture(gd, xinfo.CreateReadStream);
                        if (tex != null) return (tex, attr);
                        break;
                    }
            }

            var texx = _loadTexture(gd, () => ImageSource.TryOpenRead(imageSource));
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

        private static Texture2D _loadTexture(GraphicsDevice gd, Func<System.IO.Stream> openDocFunc)
        {
            Texture2D tex;

            using (var s = openDocFunc())
            {
                if (s == null) return null;

                tex = Texture2D.FromStream(gd, s);
            }

            tex.PremultiplyAlpha();

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
