using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;

namespace InteropDrawing.Transforms
{
    /// <summary>
    /// Creates a transformation that renders 3D content over a <see cref="IDrawing2D"/> surface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Direct calls to this <see cref="IDrawing3D"/> will render the primitives directly to the<br/>
    /// underlaying <see cref="IDrawing2D"/> surface. So if we need to render the primitives<br/>
    /// from back to front, it's better to render the scene in two steps:
    /// <list type="number">
    /// <item>Draw the 3D scene to a temporary <see cref="Model3D"/></item>
    /// <item>Render the temporary <see cref="Model3D"/> using <see cref="DrawScene(Model3D, bool)"/></item>
    /// </list>
    /// </para>
    /// </remarks>
    class PerspectiveTransform : IDrawing3D
    {
        #region lifecycle

        public static PerspectiveTransform CreateLookingAtCenter((IDrawing2D rt, float w, float h) viewport, (float x, float y, float z) cameraPosition)
        {
            var cam = Matrix4x4.CreateWorld(cameraPosition.ToVector(), Vector3.Normalize(-cameraPosition.ToVector()), Vector3.UnitY);

            return CreatePerspective(viewport, 1.2f, cam);
        }

        public static PerspectiveTransform CreatePerspective((IDrawing2D rt, float w, float h) viewport, float projectionFOV, Matrix4x4 camera)
        {
            var nearPlane = 0.1f;

            var proj = Matrix4x4.CreatePerspectiveFieldOfView(projectionFOV, viewport.w / viewport.h, nearPlane, 1000);            

            return Create(viewport, proj, camera);
        }

        public static PerspectiveTransform Create((IDrawing2D rt, float w, float h) viewport, Matrix4x4 projection, Matrix4x4 camera, Func<Vector2, Vector2> distort = null)
        {
            if (!Matrix4x4.Invert(camera, out Matrix4x4 viewMatrix)) throw new ArgumentException(nameof(camera));

            var nearPlane = 0.1f;

            var vp = new Matrix3x2
                (0.5f * viewport.w, 0
                , 0, -0.5f * viewport.h
                , 0.5f * viewport.w, 0.5f * viewport.h);

            return new PerspectiveTransform(viewport.rt, distort, vp, viewMatrix * projection, nearPlane);
        }

        private PerspectiveTransform(IDrawing2D renderTarget, Func<Vector2, Vector2> distorsion, Matrix3x2 viewport, Matrix4x4 projview, float nearPlane)
        {
            System.Diagnostics.Debug.Assert(nearPlane > 0);

            _Projection = projview;
            _ProjectionScale = new Vector3(projview.M12, projview.M22, projview.M32).Length(); // we only use the Y Axis to avoid problems with the AspectRatio

            _ClipPlane = new Plane(Vector3.UnitZ, -nearPlane);            

            _Viewport = viewport;
            _ViewportScale = new Vector2(viewport.M12, viewport.M22).Length(); // we only use the Y Axis to avoid problems with the AspectRatio

            _Distorsion = distorsion;

            _RenderTarget = renderTarget;
        }

        #endregion

        #region data

        // http://xdpixel.com/decoding-a-projection-matrix/
        private readonly Matrix4x4 _Projection;
        private readonly Single _ProjectionScale;        

        private readonly Plane _ClipPlane;

        private readonly Matrix3x2 _Viewport;
        private readonly Single _ViewportScale;

        private readonly Func<Vector2, Vector2> _Distorsion;

        private readonly IDrawing2D _RenderTarget;

        #endregion

        #region API

        public Vector4 GetProjection(Point3 v)
        {
            System.Diagnostics.Debug.Assert(v.IsReal);
            return Vector4.Transform(v.ToNumerics(), _Projection);
        }

        public Vector2 GetPerspective(Vector4 v)
        {
            System.Diagnostics.Debug.Assert(v.IsReal(), "Invalid Vertex: NaN");
            System.Diagnostics.Debug.Assert(v.W > 0, "Invalid Vertex, W must be positive");

            var xy = new Vector2(v.X, v.Y) / v.W;
            xy = Vector2.Transform(xy, _Viewport);

            if (_Distorsion != null) xy = _Distorsion(xy);

            return xy;
        }

        public float GetPerspective(float value, float w)
        {
            return value * _ProjectionScale * _ViewportScale / w;
        }

        public void DrawAsset(in Matrix4x4 transform, object asset, ColorStyle brush)
        {            
            var sphere = Toolkit.GetAssetBoundingSphere(asset);

            if (!sphere.HasValue)
            {
                this.DrawAssetAsSurfaces(transform, asset, brush);
                return;
            }

            var (center, radius) = transform.TransformSphere(sphere.Value);

            if (_ClipPlane.IsInPositiveSideOfPlane(center, radius))
            {
                this.DrawAsset(transform, asset, brush);
                return;
            }

            this.DrawAssetAsPrimitives(transform, asset, brush);
        }

        public void DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            var aa = GetProjection(a);
            var bb = GetProjection(b);

            if (!_ClipPlane.ClipLineToPlane(ref aa, ref bb)) return;

            var www = (aa.W + bb.W) * 0.5f;
            var aaa = GetPerspective(aa);
            var bbb = GetPerspective(bb);
            diameter = GetPerspective(diameter, www);
            var ow = GetPerspective(brush.Style.OutlineWidth, www);

            _RenderTarget.DrawLine(aaa, bbb, diameter, brush = brush.WithOutline(ow));
        }

        public void DrawSphere(Point3 center, float diameter, ColorStyle brush)
        {
            var aa = GetProjection(center);

            if (!_ClipPlane.IsInPositiveSideOfPlane(aa.SelectXYZ(), 0)) return;

            var www = aa.W;
            var aaa = GetPerspective(aa);
            diameter = GetPerspective(diameter, www);
            var ow = GetPerspective(brush.OutlineWidth, www);

            _RenderTarget.DrawEllipse(aaa, diameter, diameter, brush.WithOutline(ow));
        }

        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            brush = brush.WithOutline(brush.Style.OutlineWidth * _ProjectionScale);

            Span<Vector4> projected = stackalloc Vector4[vertices.Length];
            for (int i = 0; i < projected.Length; ++i) projected[i] = GetProjection(vertices[i]);
            
            if (_ClipPlane.IsInPositiveSideOfPlane(projected))
            {
                _DrawProjectedPolygon(projected, brush);
                return;
            }

            // clip polygon
            Span<Vector4> clippedProjected = stackalloc Vector4[vertices.Length * 2];
            var cvertices = _ClipPlane.ClipPolygonToPlane(clippedProjected, projected);
            if (cvertices < 3) return;
            clippedProjected = clippedProjected.Slice(0, cvertices);

            _DrawProjectedPolygon(clippedProjected, brush);
        }

        private void _DrawProjectedPolygon(ReadOnlySpan<Vector4> projected, SurfaceStyle brush)
        {
            if (!brush.DoubleSided)
            {
                // TODO: face culling
            }

            float www = 0;

            Span<Point2> perspective = stackalloc Point2[projected.Length];
            for(int i=0; i < perspective.Length; ++i)
            {
                var v = projected[i];
                www += v.W;
                perspective[i] = GetPerspective(v);
            }

            www /= perspective.Length;

            brush = brush.WithOutline(GetPerspective(brush.Style.OutlineWidth, www));

            _RenderTarget.DrawPolygon(perspective, brush.Style);
        }

        
        /// <summary>
        /// Draws the predefined scene using <see href="https://en.wikipedia.org/wiki/Painter%27s_algorithm"/>
        /// </summary>
        /// <param name="scene">the scene to render</param>
        public void DrawScene(Model3D scene, bool decompose = false)
        {

            if (true)
            {
                foreach (var offset in scene._Commands.GetCommandList(center => -GetProjection(center).Z))
                {
                    scene._Commands.DrawTo(offset, this, decompose);
                }

                return;
            }
            else
            {
                var drawingOrder = new List<_SortableItem>();

                // We can use BinarySearch to locate the insertion point and do a sorted insertion.
                // Span<_SortableItem> drawingOrder = stackalloc _SortableItem[scene.Commands.Count];            

                foreach (var offset in scene._Commands.GetCommands())
                {
                    var center = scene._Commands.GetCenter(offset);

                    var item = new _SortableItem
                    {
                        Index = offset,
                        Distance = -GetProjection(center).Z
                    };

                    drawingOrder.Add(item);
                }

                drawingOrder.Sort();

                foreach (var item in drawingOrder)
                {
                    scene._Commands.DrawTo(item.Index, this, decompose);
                }
            }
        }

        #endregion

        #region types

        [System.Diagnostics.DebuggerDisplay("{Index} at {Distance}")]
        private struct _SortableItem : IComparable<_SortableItem>
        {
            public float Distance;
            public int Index;

            public int CompareTo(_SortableItem other)
            {
                return this.Distance.CompareTo(other.Distance);
            }
        }

        #endregion
    }
}
