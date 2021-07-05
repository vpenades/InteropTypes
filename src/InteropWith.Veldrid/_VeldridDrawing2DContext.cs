using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropDrawing;

using Veldrid;

namespace InteropWith
{
    class _VeldridDrawing2DContext : _PrimitivesStack, IDrawingContext2D
    {
        #region lifecycle

        internal _VeldridDrawing2DContext(VeldridDrawingFactory owner)
        {
            _Factory = owner;
            _NullTextureId = _Factory._GetTexture(null).Item1;
        }

        public void Initialize(Framebuffer frameBuffer, System.Drawing.Color? color)
        {
            Clear();
            _Completed = false;
            _FrameBuffer = frameBuffer;
            _FillColor = color;
        }

        public void Dispose()
        {
            if (!_Completed)
            {
                this._Factory.Draw(this);
                _Completed = true;
                _FrameBuffer = null;
                _FillColor = null;
                _Factory._ReturnDrawing2DContext(this);
            }
        }

        #endregion

        #region data

        internal readonly VeldridDrawingFactory _Factory;

        private readonly int _NullTextureId;

        private bool _Completed;
        private Framebuffer _FrameBuffer;
        private System.Drawing.Color? _FillColor;

        #endregion

        #region API

        private InteropDrawing.Transforms.Decompose2D _Collapsed2D => new InteropDrawing.Transforms.Decompose2D(this);

        public unsafe void AddPolygon(ReadOnlySpan<Point2> points, System.Drawing.Color color)
        {
            if (_Completed) throw new ObjectDisposedException("Context");
            AddPolygon(points, color, _NullTextureId);
        }

        public (int, Vector2) GetTextureInfoFromSource(Object source)
        {
            if (_Completed) throw new ObjectDisposedException("Context");
            return _Factory._GetTexture(source);
        }

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (asset is IDrawable2D drawable) { drawable.DrawTo(this); return; }
            _Collapsed2D.DrawAsset(transform, asset);
        }

        public void DrawEllipse(Point2 center, float width, float height, ColorStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawEllipse(center, width, height, style);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawLines(points, diameter, style);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (style.HasOutline) { _Collapsed2D.DrawPolygon(points, style); return; }
            if (style.HasFill) this.AddPolygon(points, style.FillColor);
        }

        public void DrawSprite(in Matrix3x2 transform, in SpriteStyle style)
        {
            var final = Matrix3x2.CreateScale(style.Bitmap.Width, style.Bitmap.Height) * transform;

            Vertex p0 = final.Translation;
            Vertex p1 = Vector2.Transform(Vector2.UnitX, final);
            Vertex p2 = Vector2.Transform(Vector2.UnitX + Vector2.UnitY, final);
            Vertex p3 = Vector2.Transform(Vector2.UnitY, final);

            var tex = GetTextureInfoFromSource(style.Bitmap.Source);

            p0.TextureCoordinates = style.Bitmap.UV0 / tex.Item2;
            p1.TextureCoordinates = style.Bitmap.UV1 / tex.Item2;
            p2.TextureCoordinates = style.Bitmap.UV2 / tex.Item2;
            p3.TextureCoordinates = style.Bitmap.UV3 / tex.Item2;

            // tex = _TextureFactory.Invoke(null);

            AddQuad(p0, p1, p2, p3, tex.Item1);
        }

        internal void DrawTo(CommandList cmd, _IndexedVertexBuffer vb)
        {
            var effect = _Factory.CurrentEffect;

            cmd.SetFramebuffer(_FrameBuffer);

            var wvp = Matrix4x4.CreateOrthographicOffCenter(0, _FrameBuffer.Width, _FrameBuffer.Height, 0, 0, 1);

            effect.Bind(cmd, _FrameBuffer.OutputDescription);
            effect.SetWorldMatrix(wvp);

            if (_FillColor.HasValue)
            {
                var color = _FillColor.Value;
                var c = new RgbaFloat(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                cmd.ClearColorTarget(0, c);
            }

            vb.Bind(cmd);
            foreach (var batch in this.Batches)
            {
                _Factory._SetTexture(cmd, batch.TextureId);

                batch.DrawTo(cmd, vb);
            }
        }

        #endregion
    }
}
