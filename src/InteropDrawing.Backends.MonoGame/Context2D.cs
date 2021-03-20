using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using V2 = System.Numerics.Vector2;

using POINT2 = InteropDrawing.Point2;

using XNACOLOR = Microsoft.Xna.Framework.Color;
using XNAV2 = Microsoft.Xna.Framework.Vector2;

namespace InteropDrawing.Backends
{
    /// <summary>
    /// This is an experimental version attempting
    /// to merge <see cref="MonoGameDeferredDrawing2D"/>
    /// and <see cref="MonoGameSprites"/>
    /// </summary>
    public class MonoGameDrawing2D : IDisposable, IDrawing2D
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

            _SpritesBatch?.Dispose();
            _SpritesBatch = null;

            _VectorsEffect?.Dispose();
            _VectorsEffect = null;

            foreach (var tex in _Textures.Values) tex.Dispose();
            _Textures.Clear();

            _WhiteTexture?.Dispose();
            _WhiteTexture = null;

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

        private MonoGameSolidMeshBuilder _VectorsBatch = new MonoGameSolidMeshBuilder(false);

        private System.Numerics.Matrix3x2 _View;
        private System.Numerics.Matrix3x2 _Screen;
        private System.Numerics.Matrix3x2 _Final;

        #endregion

        #region API

        public Texture2D FetchTexture(Object imageSource)
        {
            var imagePath = imageSource as string;

            if (_Textures.TryGetValue(imagePath, out Texture2D tex)) return tex;

            using (var s = System.IO.File.OpenRead(imagePath))
            {
                tex = Texture2D.FromStream(_Device, s);
            }

            _PremultiplyAlpha(tex);

            return _Textures[imagePath] = tex;
        }

        public void Begin(int virtualWidth, int virtualHeight, bool keepAspect)
        {
            _Screen = _Device.CreateVirtualToPhysical((virtualWidth, virtualHeight), keepAspect);
            _Final = _View * _Screen;
        }

        public void SetCamera(System.Numerics.Matrix3x2 camera)
        {
            System.Numerics.Matrix3x2.Invert(camera, out _View);
            _Final = _View * _Screen;
        }

        public void DrawAsset(in System.Numerics.Matrix3x2 transform, object asset, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();
            _VectorsBatch.DrawAsset(transform, asset, style);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();
            _VectorsBatch.DrawLines(points, diameter, style);
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();
            _VectorsBatch.DrawEllipse(center, width, height, style);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (_SpritesDirty) Flush();
            _VectorsBatch.DrawPolygon(points, style);
        }

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

            var xform = transform * _Final;

            xform.Decompose(out V2 s, out float r, out V2 t);

            var offset = _ToXna(sprite.Pivot);
            var scale = _ToXna(s) * sprite.Scale;
            var rotation = r;
            var translation = _ToXna(t);

            var effects = SpriteEffects.None;
            if (style.FlipHorizontal) effects |= SpriteEffects.FlipHorizontally;
            if (style.FlipVertical) effects |= SpriteEffects.FlipVertically;

            _SpritesBatch.Draw(tex, translation, new Rectangle(sprite.Left, sprite.Top, sprite.Width, sprite.Height), style.Color.ToXnaPremul(), rotation, offset, scale, effects, 0);
        }

        public void Flush()
        {
            if (!_VectorsBatch.IsEmpty)
            {
                if (_VectorsEffect == null || _VectorsEffect.IsDisposed) _VectorsEffect = new SpriteEffect(_Device);

                _VectorsEffect.TransformMatrix = _ToXna(_Final);

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

        public void End()
        {
            Flush();
        }

        #endregion

        #region utils        

        private static XNAV2 _ToXna(V2 v) { return new XNAV2(v.X, v.Y); }

        private static Matrix _ToXna(in System.Numerics.Matrix3x2 m)
        {
            return new Matrix
                (m.M11, m.M12, 0, 0
                , m.M21, m.M22, 0, 0
                , 0, 0, 1, 0
                , m.M31, m.M32, 0, 1);
        }

        



        public static IEnumerable<(string, Rectangle?, V2)> CreateSpriteGrid(string assetName, int stride, (int w, int h) cell, int count, (int x, int y) offset)
        {
            for (int idx = 0; idx < count; ++idx)
            {
                var idx_x = idx % stride;
                var idx_y = idx / stride;

                yield return (assetName, new Rectangle(idx_x * cell.w, idx_y * cell.h, cell.w, cell.h), new V2(offset.x, offset.y));
            }
        }

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
            if (_WhiteTexture == null) _WhiteTexture = _CreateSolidTexture(16, 16, Color.White);

            _OldTexture = _Device.Textures[0];
            _Device.Textures[0] = _WhiteTexture;
        }

        private void _DisableWhiteTexture()
        {
            _Device.Textures[0] = _OldTexture;
            _OldTexture = null;
        }

        private Texture2D _CreateSolidTexture(int width, int height, XNACOLOR fillColor)
        {
            var tex = new Texture2D(_Device, width, height, false, SurfaceFormat.Bgra4444);

            var data = new ushort[width * height];

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = 0xffff;
            }

            tex.SetData<ushort>(data);

            return tex;
        }

        #endregion
    }
}
