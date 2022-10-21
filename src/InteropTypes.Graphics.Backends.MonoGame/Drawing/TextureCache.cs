using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using static System.Net.Mime.MediaTypeNames;

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
                var xtex2 = _DynamicTextureUpdate(imageSource, xtex);

                if (xtex.Item1 != xtex2.Item1) // the texture has changed, update dictionary
                {
                    if (xtex2.Item1 == null) _SpriteTextures.Remove(imageSource);
                    else _SpriteTextures[imageSource] = xtex2;
                }

                return xtex2;
            }

            if (imageSource is Bitmaps.BindableBitmap || imageSource is Bitmaps.InterlockedBitmap)
            {
                xtex = _DynamicTextureUpdate(imageSource, (null, SpriteTextureAttributes.Default));
            }
            else
            {
                xtex = MonoGameToolkit._CreateStaticTexture(_Device, imageSource);
            }            

            if (xtex.Item1 != null) _SpriteTextures[imageSource] = xtex;

            return xtex;
        }

        private (Texture2D, SpriteTextureAttributes) _DynamicTextureUpdate(object imageSource, (Texture2D, SpriteTextureAttributes) xtex)
        {
            // if the imageSource is a dinamic texture, update our device texture.

            if (imageSource is Bitmaps.BindableBitmap bindable)
            {
                bindable.UpdateFromQueue();

                var tex = xtex.Item1;

                MonoGameToolkit.Copy(bindable.Bitmap, ref tex, false, _Device);

                xtex = (tex, xtex.Item2);
            }

            if (imageSource is Bitmaps.InterlockedBitmap interlocked)
            {
                var tex = xtex.Item1;

                if (interlocked.TryDequeue(newFrame => MonoGameToolkit.Copy(newFrame, ref tex, false, _Device)))
                {
                    xtex = (tex, xtex.Item2);
                }
            }            

            return xtex; // not dynamic, return as is
        }

        #endregion
    }
}
