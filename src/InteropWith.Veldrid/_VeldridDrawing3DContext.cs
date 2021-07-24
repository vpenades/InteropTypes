﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropDrawing;

using Veldrid;

namespace InteropWith
{
    public interface IVeldridDrawingContext3D : IDrawingContext3D
    {
        void FillFrame(System.Drawing.Color color);
    }

    class _VeldridDrawing3DContext : _PrimitivesStack<Vertex3D>, IVeldridDrawingContext3D
    {
        #region lifecycle

        internal _VeldridDrawing3DContext(VeldridDrawingFactory owner)
        {
            _Factory = owner;
            _NullTextureId = _Factory._GetTexture(null).Item1;

            _CommandList = owner._Disposables.Record(owner.GraphicsDevice.ResourceFactory.CreateCommandList());
            _IndexedVertexBuffer = owner._Disposables.Record(new _IndexedVertexBuffer(owner.GraphicsDevice));
        }

        public void Initialize(Framebuffer frameBuffer, Matrix4x4 view, Func<Vector2, Matrix4x4> projFunc)
        {
            Clear();
            _Completed = false;            
            _FillColor = null;

            _Ext_FrameBuffer = frameBuffer;
            _Ext_CommandList = null;

            _ViewMatrix = view;
            _ProjectionFunction = projFunc;
        }

        public void Initialize(Framebuffer frameBuffer, CommandList command, Matrix4x4 view, Func<Vector2, Matrix4x4> projFunc)
        {
            Clear();
            _Completed = false;
            
            _FillColor = null;

            _Ext_FrameBuffer = frameBuffer;
            _Ext_CommandList = command;

            _ViewMatrix = view;
            _ProjectionFunction = projFunc;
        }

        public void Dispose()
        {
            // we just reset the object because it is reusable.

            if (!_Completed)
            {
                this._Submit();
                _Completed = true;
                _Ext_FrameBuffer = null;
                _FillColor = null;
                _Factory._Return(this);
            }
        }

        #endregion

        #region data

        internal readonly VeldridDrawingFactory _Factory;

        private CommandList _CommandList;
        private _IndexedVertexBuffer _IndexedVertexBuffer;

        private CommandList _Ext_CommandList;
        private Framebuffer _Ext_FrameBuffer;

        private Matrix4x4 _ViewMatrix;
        private Func<Vector2, Matrix4x4> _ProjectionFunction;        

        private readonly int _NullTextureId;

        private bool _Completed;        
        private System.Drawing.Color? _FillColor;

        #endregion

        #region API        

        public unsafe void AddPolygon(ReadOnlySpan<Point3> points, System.Drawing.Color color)
        {
            if (_Completed) throw new ObjectDisposedException("Context");

            var c = (UInt32)color.ToArgb(); // color needs to be reversed
            var n = _CalculateNormal(points);

            Span<Vertex3D> vertices = stackalloc Vertex3D[points.Length];            

            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i] = new Vertex3D { Position = points[i].ToNumerics(), Normal =n,  Color = c };
            }

            AddPolygon(vertices, _NullTextureId);
        }

        private static unsafe Vector3 _CalculateNormal(ReadOnlySpan<Point3> points)
        {
            var n = Vector3.Zero;
            var a = points[0].ToNumerics();
            var ab = points[1].ToNumerics() - a;


            for (int i = 2; i < points.Length; ++i)
            {
                var ac = points[i].ToNumerics() - a;
                var x = Vector3.Cross(ab, ac);
                n += x;
            }

            return Vector3.Normalize(n);
        }        

        private void _Submit()
        {
            this.CopyTo(_IndexedVertexBuffer); // in VR, we should do this just once.

            if (_Ext_CommandList != null)
            {
                _Submit(_Ext_CommandList);
                return;
            }

            _CommandList.Begin();
            _CommandList.SetFramebuffer(_Ext_FrameBuffer);

            if (_FillColor.HasValue)
            {
                var color = _FillColor.Value;
                var c = new RgbaFloat(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
                _CommandList.ClearColorTarget(0, c);
            }

            // if (_FrameBuffer.DepthTarget != null)
            _CommandList.ClearDepthStencil(1);

            _Submit(_CommandList);

            _CommandList.End();
            _Factory.GraphicsDevice.SubmitCommands(_CommandList);
        }

        private void _Submit(CommandList cmd)
        {
            var proj = _ProjectionFunction(new Vector2(_Ext_FrameBuffer.Width, _Ext_FrameBuffer.Height));

            var effect = _Factory.SolidEffect;
            effect.Bind(cmd, _Ext_FrameBuffer.OutputDescription);
            effect.SetViewMatrix(_ViewMatrix);
            effect.SetProjMatrix(proj);

            _IndexedVertexBuffer.Bind(cmd);
            foreach (var batch in this.Batches)
            {
                batch.DrawTo(cmd, _IndexedVertexBuffer);
            }
        }

        #endregion

        #region drawing API

        public void FillFrame(System.Drawing.Color color)
        {
            this.Clear();
            _FillColor = color;
        }

        private InteropDrawing.Transforms.Decompose3D _Collapsed3D => new InteropDrawing.Transforms.Decompose3D(this);

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle style)
        {
            /*
            if (style.Style.HasFill && diameter <= MinimumSolidLineDiameter)
            {
                var aa = _Lines.UseVertex(a.ToNumerics(), brush.Style.FillColor);
                var bb = _Lines.UseVertex(b.ToNumerics(), brush.Style.FillColor);
                _Lines.AddLine(aa, bb);

                if (!brush.Style.HasOutline) return;

                brush = brush.With(COLOR.Transparent);
            }*/

            _Collapsed3D.DrawSegment(a, b, diameter, style);
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle style)
        {
            _Collapsed3D.DrawSphere(center, diameter, style);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            AddPolygon(vertices, style.Style.FillColor);
        }


        #endregion
    }
}