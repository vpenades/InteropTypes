using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Drawing;

using OPENGL = Silk.NET.OpenGL.GL;

namespace InteropTypes.Graphics.Backends
{
    public class OpenGLDrawing3D : IDisposableScene3D
    {
        public OpenGLDrawing3D(OPENGL glContext)
        {
            _glContext = glContext;
        }

        private OPENGL _glContext;

        private List<IDisposable> _disposables = new List<IDisposable>();
        private Drawing.Transforms.Decompose3D _Collapsed3D => new Drawing.Transforms.Decompose3D(this);

        private void _Add(IDisposable disposable)
        {
            if (_disposables == null) _disposables = new List<IDisposable>();
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            var list = System.Threading.Interlocked.Exchange(ref _disposables, null);
            if (list != null)
            {
                foreach (IDisposable disposable in list) { disposable.Dispose(); }
            }
        }

        #region drawing API

        public void FillFrame(System.Drawing.Color color)
        {
            // this.Clear();
            // _FillColor = color;
        }       

        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            throw new NotImplementedException();
        }

        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle style)
        {
            _Collapsed3D.DrawSegments(vertices, diameter, style);
        }

        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle style)
        {
            _Collapsed3D.DrawSphere(center, diameter, style);
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            AddPolygon(vertices, style.Style.FillColor.ToGDI());
        }

        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            AddPolygon(vertices, style.ToGDI());
        }


        #endregion

        private readonly List<int> _Indices = new List<int>();
        private readonly List<Vertex3> _Vertices = new List<Vertex3>();

        public void Clear()
        {
            _Indices.Clear();
            _Vertices.Clear();
        }

        void AddVertex(Vertex3 v)
        {
            _Indices.Add(_Indices.Count);
            _Vertices.Add(v);
        }

        void AddPolygon(ReadOnlySpan<Point3> points, System.Drawing.Color color)
        {
            for(int i=2; i <points.Length; i++)
            {
                var v0 = new Vertex3(points[0], color);
                var v1 = new Vertex3(points[i-1], color);
                var v2 = new Vertex3(points[i], color);

                AddVertex(v0);
                AddVertex(v1);
                AddVertex(v2);
            }
        }

        public void Render()
        {

        }
    }
}
