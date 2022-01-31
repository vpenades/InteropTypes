using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using XNACOLOR = Microsoft.Xna.Framework.Color;

namespace InteropDrawing.Backends
{
    public interface IMonoGameDrawing2D :
        IDisposableCanvas2D,
        IBackendViewportInfo,
        Transforms.ITransformer2D,
        IServiceProvider
    {
        void Begin(int virtualWidth, int virtualHeight, bool keepAspect);
        void SetCamera(System.Numerics.Matrix3x2 camera);
        void SetSpriteFlip(bool hflip, bool vflip);
        void End();
    }

    public interface IMonoGameDrawing3D : IDisposableScene3D
    {
        void Clear();
        void SetCamera(CameraView3D camera);
        void Render();
    }

    

    public static class MonoGameDrawing
    {
        /// <summary>
        /// Takes a sprite texture image source (a string path, a raw texture, etc) and adds additional attributes
        /// </summary>
        /// <param name="source">The value of <see cref="ImageAsset.Source"/></param>
        /// <param name="sampler">The sampler to associate with the image</param>
        /// <param name="textureBleed">the texture bleed to associate with the image</param>
        /// <returns>A new value that replaces <see cref="ImageAsset.Source"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Object CreateSpriteAssetSource(Object source, SamplerState sampler = null, float textureBleed = 0)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source is MonoGameSpriteTexture mgsa)
            {
                mgsa.Attributes = new SpriteTextureAttributes(sampler, textureBleed);
                return mgsa;
            }

            // at this point, source must be: String or System.IO.FileInfo or Func<System.IO.Stream>

            return new MonoGameSpriteTexture(source, sampler ?? SamplerState.LinearClamp, textureBleed);
        }

        internal static (Texture2D Texture, SpriteTextureAttributes Attributes) CreateTexture(GraphicsDevice gd, Object imageSource)
        {
            var attr = SpriteTextureAttributes.Default;

            if (imageSource is MonoGameSpriteTexture mgasset)
            {
                attr = mgasset.Attributes;
                imageSource = mgasset.Source;
            }            

            if (imageSource is System.IO.FileInfo finfo)
            {
                imageSource = finfo.FullName;
            }            

            if (imageSource is String texPath)
            {
                var tex = _loadTexture(gd, () => System.IO.File.OpenRead(texPath));
                return (tex, attr);
            }

            if (imageSource is Func<System.IO.Stream> document)
            {
                var tex = _loadTexture(gd, document);
                return (tex, attr);
            }

            throw new NotImplementedException($"Unknown source: {imageSource}");
        }

        private static Texture2D _loadTexture(GraphicsDevice gd, Func<System.IO.Stream> openDocFunc)
        {
            Texture2D tex;

            using (var s = openDocFunc())
            {
                tex = Texture2D.FromStream(gd, s);
            }

            _PremultiplyAlpha(tex);

            return tex;
        }

        private static void _PremultiplyAlpha(Texture2D texture)
        {
            var data = new XNACOLOR[texture.Width * texture.Height];
            texture.GetData(data);

            // TODO: we could do with a parallels

            for (int i = 0; i != data.Length; ++i) data[i] = XNACOLOR.FromNonPremultiplied(data[i].ToVector4());

            texture.SetData(data);
        }

        internal static Texture2D _CreateSolidTexture(GraphicsDevice gd, int width, int height, XNACOLOR fillColor)
        {
            var tex = new Texture2D(gd, width, height, false, SurfaceFormat.Bgra4444);

            var data = new ushort[width * height];

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = 0xffff;
            }

            tex.SetData<ushort>(data);

            return tex;
        }

        public static IMonoGameDrawing2D CreateDrawingContext2D(this GraphicsDevice device)
        {
            return new MonoGameDrawing2D(device);
        }

        public static IMonoGameDrawing3D CreateDrawingContext3D(this GraphicsDevice device)
        {
            return new MonoGameDrawing3D(device);
        }
    }
}
