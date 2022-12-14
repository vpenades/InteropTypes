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

        public void Dispose()
        {
            
        }

        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            throw new NotImplementedException();
        }

        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle style)
        {
            throw new NotImplementedException();
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle style)
        {
            throw new NotImplementedException();
        }
    }
}
