using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace InteropTypes.Graphics.Backends
{
    internal class TextureCache
    {
        #region lifecycle

        public TextureCache(GraphicsDevice device)
        {
            _Device = device;            
        }

        public void Dispose()
        {
            _Device = null;            

            foreach (var tex in _SpriteTextures.Values) tex.Item1.Dispose();
            _SpriteTextures.Clear();
        }

        #endregion

        #region data

        private GraphicsDevice _Device;
        
        private readonly Dictionary<object, (Texture2D, SpriteTextureAttributes)> _SpriteTextures = new Dictionary<object, (Texture2D, SpriteTextureAttributes)>();

        #endregion

        #region API

        public (Texture2D tex, SpriteTextureAttributes bleed) FetchTexture(object imageSource)
        {
            if (imageSource is Texture2D xnaTex) return (xnaTex, SpriteTextureAttributes.Default);

            if (_SpriteTextures.TryGetValue(imageSource, out (Texture2D, SpriteTextureAttributes) xtex))
            {
                return DynamicTextureUpdate(imageSource, xtex);
            }

            xtex = MonoGameToolkit._CreateTexture(_Device, imageSource);

            return _SpriteTextures[imageSource] = xtex;
        }

        private (Texture2D, SpriteTextureAttributes) DynamicTextureUpdate(object imageSource, (Texture2D, SpriteTextureAttributes) xtex)
        {
            // if the imageSource is a dinamic texture, update our device texture.

            if (imageSource is Bitmaps.InterlockedBitmap interlocked)
            {
                var tex = xtex.Item1;

                if (interlocked.TryDequeue(newFrame => MonoGameToolkit.Copy(newFrame, ref tex, false, _Device)))
                {
                    xtex = (tex, xtex.Item2);

                    if (tex != xtex.Item1)
                    {
                        _SpriteTextures[imageSource] = xtex;
                    }
                }
            }

            return xtex;
        }

        #endregion
    }
}
