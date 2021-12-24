﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InteropDrawing.Backends
{
    class MonoGameDrawing2Dex : IMonoGameDrawing2D
    {
        #region lifecycle

        public MonoGameDrawing2Dex(GraphicsDevice device)
        {
            _Device = device;
            _View = System.Numerics.Matrix3x2.Identity;
        }

        public void Dispose()
        {
            _Device = null;
            
            System.Threading.Interlocked.Exchange(ref _Effect, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _WhiteTexture, null)?.Dispose();

            foreach (var tex in _SpriteTextures.Values) tex.Item1.Dispose();
            _SpriteTextures.Clear();
            
            _CurrTexture = null;

            _VectorsBatch.Clear();
        }

        #endregion

        #region data

        private GraphicsDevice _Device;
        private SpriteEffect _Effect;        

        private GlobalState _PrevState;

        private Texture2D _WhiteTexture;
        private readonly Dictionary<Object, (Texture2D, SpriteTextureAttributes)> _SpriteTextures = new Dictionary<Object, (Texture2D, SpriteTextureAttributes)>();

        private Texture2D _CurrTexture;        

        private MeshBuilder _VectorsBatch = new MeshBuilder(false);

        private System.Numerics.Matrix3x2 _View;
        private System.Numerics.Matrix3x2 _Screen;
        private System.Numerics.Matrix3x2 _Final;

        #endregion        

        #region API       

        private void SetTexture(Texture2D texture, in SpriteTextureAttributes attr = default)
        {
            if (texture == null)
            {
                if (_WhiteTexture == null) _WhiteTexture = MonoGameDrawing._CreateSolidTexture(_Device, 16, 16, Color.White);
                texture = _WhiteTexture;
            }

            if (_CurrTexture == texture) return;

            Flush();            
            _CurrTexture = texture;
            _VectorsBatch.SetSpriteTextureSize(_CurrTexture.Width, _CurrTexture.Height);

            _VectorsBatch.SpriteCoordsBleed = attr.TextureBleed;            
            _Device.SamplerStates[0] = attr.Sampler ?? SamplerState.LinearClamp;
        }

        public void Begin(int virtualWidth, int virtualHeight, bool keepAspect)
        {
            _Screen = _Device.CreateVirtualToPhysical((virtualWidth, virtualHeight), keepAspect);
            _Final = _View * _Screen;

            _PrevState = GlobalState.GetCurrent(_Device);
            GlobalState.CreateSpriteState().ApplyTo(_Device);

            _VectorsBatch.SpriteCoordsBleed = 0;
        }

        public void SetCamera(System.Numerics.Matrix3x2 camera)
        {
            System.Numerics.Matrix3x2.Invert(camera, out _View);
            _Final = _View * _Screen;
        }

        public void DrawAsset(in System.Numerics.Matrix3x2 transform, object asset, ColorStyle style)
        {
            if (!style.IsVisible) return;            

            if (asset is IDrawable2D drawable) { drawable.DrawTo(this); return; }

            _VectorsBatch.DrawAsset(transform, asset);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;
            SetTexture(null);
            _VectorsBatch.DrawLines(points, diameter, style);
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            if (!style.IsVisible) return;
            SetTexture(null);
            _VectorsBatch.DrawEllipse(center, width, height, style);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            if (!style.IsVisible) return;
            SetTexture(null);
            _VectorsBatch.DrawPolygon(points, style);
        }        

        public void DrawSprite(in System.Numerics.Matrix3x2 transform, in SpriteStyle style)
        {
            if (!style.IsVisible) return;
            var (tex,attr) = FetchTexture(style.Bitmap.Source);
            if (tex == null) return;
            SetTexture(tex, attr);
            _VectorsBatch.DrawSprite(transform, style);
        }

        public (Texture2D tex, SpriteTextureAttributes bleed) FetchTexture(Object imageSource)
        {
            if (imageSource is Texture2D xnaTex) return (xnaTex, SpriteTextureAttributes.Default);

            if (_SpriteTextures.TryGetValue(imageSource, out (Texture2D, SpriteTextureAttributes) xtex)) return xtex;

            xtex = MonoGameDrawing.CreateTexture(_Device, imageSource);

            return _SpriteTextures[imageSource] = xtex;
        }

        public void Flush()
        {
            if (_VectorsBatch.IsEmpty) return;

            if (_Effect == null || _Effect.IsDisposed) _Effect = new SpriteEffect(_Device);

            _Effect.TransformMatrix = _Final.ToXna();

            foreach (var pass in _Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _Device.Textures[0] = _CurrTexture;

                _VectorsBatch.RenderTo(_Effect.GraphicsDevice);                
            }

            _VectorsBatch.Clear();
        }

        public void End()
        {
            Flush();

            _CurrTexture = null;

            _PrevState.ApplyTo(_Device);
        }

        #endregion
    }
}