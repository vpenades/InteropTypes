using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace InteropTypes.Graphics.Backends
{
    [System.Diagnostics.DebuggerDisplay("{Source} {Attributes}")]
    sealed class MonoGameSpriteTexture
    {
        public MonoGameSpriteTexture(object source, float texbleed = 0)
        {
            Source = source;
            Attributes = new SpriteTextureAttributes(null, texbleed);
        }

        public MonoGameSpriteTexture(object source, SamplerState sampler, float texbleed = 0)
        {
            Source = source;
            Attributes = new SpriteTextureAttributes(sampler, texbleed);
        }

        public object Source { get; set; }

        public SpriteTextureAttributes Attributes { get; set; }

    }

    [System.Diagnostics.DebuggerDisplay("{Sampler} {TextureBleed}")]
    readonly struct SpriteTextureAttributes
    {
        public SpriteTextureAttributes(SamplerState sampler, float tb)
        {
            if (sampler == null) sampler = SamplerState.LinearClamp;
            Sampler = sampler;
            TextureBleed = tb;
        }

        public static readonly SpriteTextureAttributes Default = new SpriteTextureAttributes(SamplerState.LinearClamp, 0);

        public SamplerState Sampler { get; }
        public float TextureBleed { get; }
    }
}
