﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace InteropDrawing.Backends
{
    internal struct GlobalState
    {
        public static GlobalState CreateSpriteState()
        {
            return new GlobalState
            {
                blendState = BlendState.AlphaBlend,
                samplerState = SamplerState.LinearWrap,
                textureState = null,
                depthStencilState = DepthStencilState.None,
                rasterizerState = RasterizerState.CullNone                
            };
        }

        public static GlobalState CreateSpriteBatchState()
        {
            return new GlobalState
            {
                blendState = BlendState.AlphaBlend,
                samplerState = SamplerState.LinearClamp,
                textureState = null,
                depthStencilState = DepthStencilState.None,
                rasterizerState = RasterizerState.CullCounterClockwise
            };
        }

        public static GlobalState GetCurrent(GraphicsDevice device)
        {
            return new GlobalState
            {
                blendState = device.BlendState,
                samplerState = device.SamplerStates[0],
                textureState = device.Textures[0],
                depthStencilState = device.DepthStencilState,
                rasterizerState = device.RasterizerState
            };
        }

        public void ApplyTo(GraphicsDevice device)
        {
            device.BlendState = this.blendState;
            device.SamplerStates[0] = this.samplerState;
            device.Textures[0] = this.textureState;
            device.DepthStencilState = this.depthStencilState;
            device.RasterizerState = this.rasterizerState;
        }        

        public BlendState blendState;
        public SamplerState samplerState;
        public Texture textureState;
        public DepthStencilState depthStencilState;
        public RasterizerState rasterizerState;
    }
}
