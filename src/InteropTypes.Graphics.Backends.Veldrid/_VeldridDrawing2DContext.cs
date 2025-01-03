﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

using Veldrid;

namespace InteropTypes.Graphics.Backends
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

        /// <inheritdoc/>
        public void DrawConvexPolygon(ReadOnlySpan<Point2> points, ColorStyle color)
        {            
            this.AddConvexPolygon(points, color);
        }

        /// <inheritdoc/>
        public void DrawAsset(in Matrix3x2 transform, object asset)
        {            
            if (asset is IDrawingBrush<ICanvas2D> drawable) { drawable.DrawTo(this); return; }
            _Collapsed2D.DrawAsset(transform, asset);
        }

        /// <inheritdoc/>
        public void DrawEllipse(Point2 center, float width, float height, OutlineFillStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawEllipse(center, width, height, style);
        }

        /// <inheritdoc/>
        public void DrawLines(ReadOnlySpan<Point2> points, float diameter, LineStyle style)
        {
            if (!style.IsVisible) return;
            _Collapsed2D.DrawLines(points, diameter, style);
        }

        /// <inheritdoc/>
        public void DrawPolygon(ReadOnlySpan<Point2> points, PolygonStyle style)
        {
            if (!style.IsVisible) return;
            if (style.HasOutline) { _Collapsed2D.DrawPolygon(points, style); return; }
            if (style.HasFill) this.AddConvexPolygon(points, style.FillColor);
        }

        /// <inheritdoc/>
        public void DrawImage(in Matrix3x2 transform, ImageStyle style)
        {
            var tex = GetTextureInfoFromSource(style.Image.Source);
            style.Image.WithSourceSize((int)tex.Item2.X, (int)tex.Item2.Y);

            Span<Vertex3> vertices = stackalloc Vertex3[4];

            style.TransformVertices(vertices, transform, false, -1);  
            
            var abcd = System.Runtime.InteropServices.MemoryMarshal.Cast<Vertex3,Vertex2D>(vertices);

            AddQuad(abcd[0], abcd[1], abcd[2], abcd[3], tex.Item1);
        }

        /// <inheritdoc/>
        public void DrawTextLine(in Matrix3x2 transform, string text, float size, FontStyle font)
        {
            font.DrawDecomposedTo(this, transform, text, size);
        }



        #endregion
    }
}
