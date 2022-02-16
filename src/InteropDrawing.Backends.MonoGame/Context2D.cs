using System;
using System.Collections.Generic;

using InteropTypes.Graphics.Drawing;
using InteropTypes.Graphics.Drawing.Transforms;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InteropTypes.Graphics.Backends
{
    class MonoGameDrawing2D :
        Decompose2D.PassImageThrough,
        IMonoGameDrawing2D
    {
        #region lifecycle

        public MonoGameDrawing2D(GraphicsDevice device)
        {
            _Device = device;
            _View = System.Numerics.Matrix3x2.Identity;
            SetPassImageThroughTarget(_VectorsBatch);
        }

        public void Dispose()
        {
            SetPassImageThroughTarget(null);

            _Device = null;

            Interlocked.Exchange(ref _Effect, null)?.Dispose();
            Interlocked.Exchange(ref _WhiteTexture, null)?.Dispose();

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
        private readonly Dictionary<object, (Texture2D, SpriteTextureAttributes)> _SpriteTextures = new Dictionary<object, (Texture2D, SpriteTextureAttributes)>();

        private Texture2D _CurrTexture;

        private MeshBuilder _VectorsBatch = new MeshBuilder(false);

        private System.Numerics.Matrix3x2 _View;
        private System.Numerics.Matrix3x2 _Screen;
        private System.Numerics.Matrix3x2 _FinalForward;
        private System.Numerics.Matrix3x2 _FinalInverse;
        private float _ScalarForward;
        private float _ScalarInverse;

        #endregion

        #region service provider

        /// <inheritdoc/>        
        public object GetService(Type serviceType)
        {
            if (serviceType.IsAssignableFrom(GetType())) return this;
            return null;
        }

        #endregion

        #region API - Windows

        public int PixelsWidth => _Device.Viewport.Width;

        public int PixelsHeight => _Device.Viewport.Height;

        public float DotsPerInchX => 96;

        public float DotsPerInchY => 96;

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

        /// <inheritdoc />
        public void Begin(int virtualWidth, int virtualHeight, bool keepAspect)
        {
            _Screen = _Device.CreateVirtualToPhysical((virtualWidth, virtualHeight), keepAspect);
            _UpdateMatrices();

            _PrevState = GlobalState.GetCurrent(_Device);
            GlobalState.CreateSpriteState().ApplyTo(_Device);

            _VectorsBatch.SpriteCoordsBleed = 0;
        }

        /// <inheritdoc />
        public void SetCamera(System.Numerics.Matrix3x2 camera)
        {
            System.Numerics.Matrix3x2.Invert(camera, out _View);
            _UpdateMatrices();
        }

        private void _UpdateMatrices()
        {
            _FinalForward = _View * _Screen;
            System.Numerics.Matrix3x2.Invert(_FinalForward, out _FinalInverse);

            _ScalarForward = _FinalForward.DecomposeScale();
            _ScalarInverse = 1f / _ScalarForward;

            Span<float> scalars = stackalloc float[1];
            scalars[0] = 1;
            TransformScalarsInverse(scalars);

            _VectorsBatch.SetThinLinesPixelSize(scalars[0] * 1.25f);
        }

        /// <inheritdoc />
        public void SetSpriteFlip(bool hflip, bool vflip)
        {
            _VectorsBatch.SetSpriteGlobalFlip(hflip, vflip);
        }

        /// <inheritdoc />
        public (Texture2D tex, SpriteTextureAttributes bleed) FetchTexture(object imageSource)
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

            _Effect.TransformMatrix = _FinalForward.ToXna();

            foreach (var pass in _Effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _Device.Textures[0] = _CurrTexture;

                _VectorsBatch.RenderTo(_Effect.GraphicsDevice);
            }

            _VectorsBatch.Clear();
        }

        /// <inheritdoc />
        public void End()
        {
            Flush();

            _CurrTexture = null;

            _PrevState.ApplyTo(_Device);
        }

        #endregion

        #region API - ITransformer2D

        /// <inheritdoc />
        public void TransformForward(Span<Point2> points) { Point2.Transform(points, _FinalForward); }

        /// <inheritdoc />
        public void TransformInverse(Span<Point2> points) { Point2.Transform(points, _FinalInverse); }

        /// <inheritdoc />
        public void TransformNormalsForward(Span<Point2> vectors) { Point2.TransformNormals(vectors, _FinalForward); }

        /// <inheritdoc />
        public void TransformNormalsInverse(Span<Point2> vectors) { Point2.TransformNormals(vectors, _FinalInverse); }

        /// <inheritdoc />
        public void TransformScalarsForward(Span<float> scalars) { for (int i = 0; i < scalars.Length; ++i) { scalars[i] *= _ScalarForward; } }

        /// <inheritdoc />
        public void TransformScalarsInverse(Span<float> scalars) { for (int i = 0; i < scalars.Length; ++i) { scalars[i] *= _ScalarInverse; } }

        #endregion

        #region API - IDrawing2D        

        protected override void SetImage(ImageAsset image)
        {
            if (image == null) { SetTexture(null); return; }

            var (tex, attr) = FetchTexture(image.Source);
            if (tex == null) { SetTexture(null); return; }

            SetTexture(tex, attr);
        }

        public void DrawMesh(ReadOnlySpan<Vertex2> vertices, ReadOnlySpan<int> indices, object texture)
        {
            var (tex, attr) = FetchTexture(texture);
            if (tex == null) { SetTexture(null); return; }

            SetTexture(tex, attr);

            _VectorsBatch.DrawMesh(vertices,indices,texture);
        }

        #endregion
    }
}
