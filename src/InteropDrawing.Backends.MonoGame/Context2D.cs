using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using V2 = System.Numerics.Vector2;

using XNACOLOR = Microsoft.Xna.Framework.Color;
using XNAV2 = Microsoft.Xna.Framework.Vector2;

namespace InteropDrawing.Backends
{    
    [Obsolete("Use MonoGameDrawing2Dex")]
    class MonoGameDrawing2D : IMonoGameDrawing2D
    {
        #region lifecycle

        public MonoGameDrawing2D(GraphicsDevice device)
        {
            _Device = device;
            _View = System.Numerics.Matrix3x2.Identity;
        }

        public void Dispose()
        {
            _Device = null;

            System.Threading.Interlocked.Exchange(ref _SpritesBatch, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _VectorsEffect, null)?.Dispose();
            System.Threading.Interlocked.Exchange(ref _WhiteTexture, null)?.Dispose();

            foreach (var tex in _Textures.Values) tex.Dispose();
            _Textures.Clear();            

            _OldTexture = null;

            _VectorsBatch.Clear();
        }

        #endregion

        #region data

        private GraphicsDevice _Device;

        private SpriteEffect _VectorsEffect;

        private SpriteBatch _SpritesBatch;
        private bool _SpritesDirty;

        private Texture2D _WhiteTexture;
        private Texture _OldTexture;

        private readonly Dictionary<string, Texture2D> _Textures = new Dictionary<string, Texture2D>();

        private MeshBuilder _VectorsBatch = new MeshBuilder(false);

        private System.Numerics.Matrix3x2 _View; // CameraInverse
        private System.Numerics.Matrix3x2 _Screen;
        private System.Numerics.Matrix3x2 _FinalForward;
        private System.Numerics.Matrix3x2 _FinalInverse;

        public float SpriteCoordsBleed { get; set; }

        public int PixelsWidth => _Device.Viewport.Width;

        public int PixelsHeight => _Device.Viewport.Height;        

        #endregion

        #region API

        public Texture2D FetchTexture(Object imageSource)
        {
            if (imageSource is Texture2D xnaTex) return xnaTex;

            var imagePath = imageSource as string;

            if (_Textures.TryGetValue(imagePath, out Texture2D tex)) return tex;

            using (var s = System.IO.File.OpenRead(imagePath))
            {
                tex = Texture2D.FromStream(_Device, s);
            }

            _PremultiplyAlpha(tex);

            return _Textures[imagePath] = tex;
        }

        /// <inheritdoc />
        public void Begin(int virtualWidth, int virtualHeight, bool keepAspect)
        {
            _Screen = _Device.CreateVirtualToPhysical((virtualWidth, virtualHeight), keepAspect);            
            _FinalForward = _View * _Screen;
            System.Numerics.Matrix3x2.Invert(_FinalForward, out _FinalInverse);
        }

        /// <inheritdoc />
        public void SetSpriteFlip(bool hflip, bool vflip)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetCamera(System.Numerics.Matrix3x2 camera)
        {
            System.Numerics.Matrix3x2.Invert(camera, out _View);
            _FinalForward = _View * _Screen;
            System.Numerics.Matrix3x2.Invert(_FinalForward, out _FinalInverse);
        }

        /// <inheritdoc />
        public void TransformForward(Span<Point2> points) { Point2.Transform(points, _FinalForward); }

        /// <inheritdoc />
        public void TransformInverse(Span<Point2> points) { Point2.Transform(points, _FinalInverse); }

        /// <inheritdoc />
        public void TransformNormalsForward(Span<Point2> vectors) { Point2.TransformNormals(vectors, _FinalForward); }

        /// <inheritdoc />
        public void TransformNormalsInverse(Span<Point2> vectors) { Point2.TransformNormals(vectors, _FinalInverse); }

        /// <inheritdoc />
        public void TransformScalarsForward(Span<Single> scalars) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void TransformScalarsInverse(Span<Single> scalars) { throw new NotImplementedException(); }

        /// <inheritdoc />
        public void DrawAsset(in System.Numerics.Matrix3x2 transform, object asset, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();            

            if (asset is IDrawable2D drawable) { drawable.DrawTo(this); return; }

            _VectorsBatch.DrawAsset(transform, asset);
        }

        /// <inheritdoc />
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();
            _VectorsBatch.DrawLines(points, diameter, style);
        }

        /// <inheritdoc />
        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();
            _VectorsBatch.DrawEllipse(center, width, height, style);
        }

        /// <inheritdoc />
        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();
            _VectorsBatch.DrawPolygon(points, style);
        }

        /// <inheritdoc />
        public void DrawSprite(in System.Numerics.Matrix3x2 transform, in SpriteStyle style)
        {
            if (!style.IsVisible) return;

            if (!_VectorsBatch.IsEmpty) Flush();

            if (!_SpritesDirty)
            {
                if (_SpritesBatch == null || _SpritesBatch.IsDisposed)
                {
                    _SpritesBatch = new SpriteBatch(_Device);
                }

                // _SpritesBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _ToXna(_Final));
                _SpritesBatch.Begin();
                _SpritesDirty = true;
            }

            var sprite = style.Bitmap;

            var tex = FetchTexture(sprite.Source);
            if (tex == null) return;

            var xform = transform * _FinalForward;

            xform.Decompose(out V2 s, out float r, out V2 t);

            var offset = sprite.Pivot.ToXna();
            var scale = s.ToXna() * sprite.Scale;
            var rotation = r;
            var translation = t.ToXna();
            var color = style.Color.ToXnaPremul();

            var effects = SpriteEffects.None;
            if (style.FlipHorizontal) effects |= SpriteEffects.FlipHorizontally;
            if (style.FlipVertical) effects |= SpriteEffects.FlipVertically;

            var uvOffset = sprite.UV0;
            var uvSize = sprite.UV2 - sprite.UV0;

            var rect = new Rectangle((int)uvOffset.X, (int)uvOffset.Y, (int)uvSize.X, (int)uvSize.Y);

            _SpritesBatch.Draw(tex, translation, rect, color, rotation, offset, scale, effects, 0);
        }

        public void Flush()
        {
            if (!_VectorsBatch.IsEmpty)
            {
                if (_VectorsEffect == null || _VectorsEffect.IsDisposed) _VectorsEffect = new SpriteEffect(_Device);

                _VectorsEffect.TransformMatrix = _FinalForward.ToXna();

                foreach (var pass in _VectorsEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    this._EnableWhiteTexture();

                    _VectorsBatch.RenderTo(_VectorsEffect.GraphicsDevice);

                    this._DisableWhiteTexture();
                }

                _VectorsBatch.Clear();
            }

            if (_SpritesDirty)
            {
                _SpritesBatch.End();
                _SpritesDirty = false;
            }
        }

        /// <inheritdoc />
        public void End()
        {
            Flush();
        }

        #endregion

        #region utils        

        private static void _PremultiplyAlpha(Texture2D texture)
        {
            var data = new XNACOLOR[texture.Width * texture.Height];
            texture.GetData(data);

            // TODO: we could do with a parallels

            for (int i = 0; i != data.Length; ++i) data[i] = XNACOLOR.FromNonPremultiplied(data[i].ToVector4());

            texture.SetData(data);
        }


        private void _EnableWhiteTexture()
        {
            if (_WhiteTexture == null) _WhiteTexture = MonoGameDrawing._CreateSolidTexture(_Device, 16, 16, Color.White);

            _OldTexture = _Device.Textures[0];
            _Device.Textures[0] = _WhiteTexture;
        }

        private void _DisableWhiteTexture()
        {
            _Device.Textures[0] = _OldTexture;
            _OldTexture = null;
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
