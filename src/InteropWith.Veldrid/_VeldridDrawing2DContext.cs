using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

using Veldrid;

namespace InteropWith
{
    public interface IVeldridDrawingContext2D : IDisposableCanvas2D
    {
        void FillFrame(System.Drawing.Color color);
    }

    class _VeldridDrawing2DContext : _PrimitivesStack<Vertex2D>, IVeldridDrawingContext2D
    {
        #region lifecycle

        internal _VeldridDrawing2DContext(VeldridDrawingFactory owner)
        {
            _Factory = owner;
            _NullTextureId = _Factory._GetTexture(null).Item1;

            _CommandList = owner._Disposables.Record(owner.GraphicsDevice.ResourceFactory.CreateCommandList());
            _IndexedVertexBuffer = owner._Disposables.Record(new _IndexedVertexBuffer(owner.GraphicsDevice));
        }

        public void Initialize(Framebuffer frameBuffer)
        {
            Clear();
            _Completed = false;
            _FrameBuffer = frameBuffer;
            _FillColor = null;
        }

        public void Dispose()
        {
            // we just reset the object because it is reusable.

            if (!_Completed)
            {
                this._Submit();
                _Completed = true;
                _FrameBuffer = null;
                _FillColor = null;
                _Factory._Return(this);
            }
        }

        #endregion

        #region data

        internal readonly VeldridDrawingFactory _Factory;

        private CommandList _CommandList;
        private _IndexedVertexBuffer _IndexedVertexBuffer;

        private readonly int _NullTextureId;

        private bool _Completed;
        private Framebuffer _FrameBuffer;
        private System.Drawing.Color? _FillColor;

        #endregion

        #region core API

        private InteropTypes.Graphics.Drawing.Transforms.Decompose2D _Collapsed2D => new InteropTypes.Graphics.Drawing.Transforms.Decompose2D(this);

        public unsafe void AddConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {
            if (_Completed) throw new ObjectDisposedException("Context");

            var c = color.Packed; // color needs to be reversed

            Span<Vertex2D> vertices = stackalloc Vertex2D[points.Length];

            for(int i=0; i < vertices.Length; ++i)
            {
                vertices[i] = new Vertex2D(points[i], c);
            }
            
            AddConvexPolygon(vertices, _NullTextureId);
        }

        public (int, Vector2) GetTextureInfoFromSource(Object source)
        {
            if (_Completed) throw new ObjectDisposedException("Context");
            return _Factory._GetTexture(source);
        }        

        private void _Submit()
        {
            this.CopyTo(_IndexedVertexBuffer);

            

            _CommandList.Begin();

            _CommandList.SetFramebuffer(_FrameBuffer);

            if (_FillColor.HasValue)
            {
                var color = _FillColor.Value;
                var c = new RgbaFloat(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                _CommandList.ClearColorTarget(0, c);
            }

            var wvp = Matrix4x4.CreateOrthographicOffCenter(0, _FrameBuffer.Width, _FrameBuffer.Height, 0, 0, 1);

            var effect = _Factory.SpriteEffect;
            effect.Bind(_CommandList, _FrameBuffer.OutputDescription);
            effect.SetWorldMatrix(wvp);            

            _IndexedVertexBuffer.Bind(_CommandList);
            foreach (var batch in this.Batches)
            {
                _Factory._SetTexture(_CommandList, batch.TextureId);

                batch.DrawTo(_CommandList, _IndexedVertexBuffer);
            }

            _CommandList.End();
            _Factory.GraphicsDevice.SubmitCommands(_CommandList);
        }

        #endregion

        #region drawing API

        public void FillFrame(System.Drawing.Color color)
        {
            this.Clear();
            _FillColor = color;
        }

        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {            
            this.AddConvexPolygon(points, color);
        }

        public void DrawAsset(in Matrix3x2 transform, object asset, ColorStyle style)
        {
            if (!style.IsVisible) return;
            if (asset is IDrawingBrush<ICanvas2D> drawable) { drawable.DrawTo(this); return; }
            _Collapsed2D.DrawAsset(transform, asset);
        }

        public void DrawEllipse(Point2 center, float width, float height, OutlineFillStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawEllipse(center, width, height, style);
        }

        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawLines(points, diameter, style);
        }

        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle style)
        {
            if (!style.IsVisible) return;
            if (style.HasOutline) { _Collapsed2D.DrawPolygon(points, style); return; }
            if (style.HasFill) this.AddConvexPolygon(points, style.FillColor);
        }

        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            var final = style.GetTransform(style.FlipHorizontal, style.FlipVertical) * transform;

            Vertex2D p0 = final.Translation;
            Vertex2D p1 = Vector2.Transform(Vector2.UnitX, final);
            Vertex2D p2 = Vector2.Transform(Vector2.UnitX + Vector2.UnitY, final);
            Vertex2D p3 = Vector2.Transform(Vector2.UnitY, final);

            var tex = GetTextureInfoFromSource(style.Bitmap.Source);

            p0.TextureCoordinates = style.Bitmap.UV0 / tex.Item2;
            p1.TextureCoordinates = style.Bitmap.UV1 / tex.Item2;
            p2.TextureCoordinates = style.Bitmap.UV2 / tex.Item2;
            p3.TextureCoordinates = style.Bitmap.UV3 / tex.Item2;

            // tex = _TextureFactory.Invoke(null);

            AddQuad(p0, p1, p2, p3, tex.Item1);
        }

        

        #endregion
    }
}
