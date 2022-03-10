using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Transforms
{
    /// <summary>
    /// Creates a transformation that renders 3D content over a <see cref="ICanvas2D"/> surface.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Direct calls to this <see cref="IScene3D"/> will render the primitives directly to the<br/>
    /// underlaying <see cref="ICanvas2D"/> surface. So if we need to render the primitives<br/>
    /// from back to front, it's better to render the scene in two steps:
    /// <list type="number">
    /// <item>Draw the 3D scene to a temporary <see cref="Record3D"/></item>
    /// <item>Render the temporary <see cref="Record3D"/> using <see cref="DrawScene(Record3D, bool)"/></item>
    /// </list>
    /// </para>
    /// </remarks>
    partial class PerspectiveTransform :
        IScene3D,
        IServiceProvider
    {
        #region lifecycle

        public static PerspectiveTransform CreateLookingAtCenter((ICanvas2D rt, float w, float h) viewport, (float x, float y, float z) cameraPosition)
        {
            var cam = Matrix4x4.CreateWorld(cameraPosition.ToVector(), Vector3.Normalize(-cameraPosition.ToVector()), Vector3.UnitY);

            return CreatePerspective(viewport, 1.2f, cam);
        }

        public static PerspectiveTransform CreatePerspective((ICanvas2D rt, float w, float h) viewport, float projectionFOV, Matrix4x4 camera)
        {
            var cam = new CameraTransform3D(camera, projectionFOV, null, 0.1f, 1000);

            return Create(viewport, cam);
        }        

        public static PerspectiveTransform Create((ICanvas2D rt, float w, float h) viewport, CameraTransform3D camera, Func<Vector2, Vector2> distort = null)
        {
            return new PerspectiveTransform(viewport.rt, (viewport.w, viewport.h), camera, distort);
        }

        private PerspectiveTransform(ICanvas2D renderTarget, Point2 physicalSize, CameraTransform3D camera, Func<Vector2, Vector2> distorsion = null)
        {
            if (renderTarget == null) throw new ArgumentNullException(nameof(renderTarget));
            if (!camera.IsValid) throw new ArgumentException("Invalid", nameof(camera));

            const float nearPlane = 0.1f;
            var viewMatrix = camera.CreateViewMatrix();
            var projMatrix = camera.CreateProjectionMatrix(physicalSize);
            var portMatrix = camera.CreateViewportMatrix(physicalSize);

            _Camera = camera.CreateCameraMatrix();
            _CameraScale = _DecomposeScale(_Camera);

            _Final = viewMatrix * projMatrix;
            _ClipPlane = new Plane(Vector3.UnitZ, -nearPlane * 2f);

            _Viewport = portMatrix;            
            _ViewportScale = new Vector2(portMatrix.M12, portMatrix.M22).Length(); // we only use the Y Axis to avoid problems with the AspectRatio
            // _ViewportScale = _DecomposeScale(viewport);

            _Distorsion = distorsion;

            _RenderTarget = renderTarget;
        }

        private static float _DecomposeScale(in Matrix3x2 xform)
        {
            var det = xform.GetDeterminant();
            var area = Math.Abs(det);

            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Sqrt(area);
            #else
            return (float)Math.Sqrt(area);
            #endif
        }

        private static Single _DecomposeScale(in Matrix4x4 matrix)
        {
            var det = matrix.GetDeterminant();
            var volume = Math.Abs(det);

            #if NETSTANDARD2_1_OR_GREATER
            return MathF.Pow(volume, (float)1 / 3);
            #else
            return (float)Math.Pow(volume, (double)1 / 3);
            #endif
        }

        #endregion

        #region data

        // http://xdpixel.com/decoding-a-projection-matrix/

        private readonly Matrix4x4 _Camera;
        private readonly float _CameraScale;

        private readonly Matrix4x4 _Final;
        private readonly Plane _ClipPlane;

        private readonly Matrix3x2 _Viewport;
        private readonly float _ViewportScale;

        private readonly Func<Vector2, Vector2> _Distorsion;

        private readonly ICanvas2D _RenderTarget;

        #endregion

        #region API

        /// <inheritdoc/>
        public object GetService(Type serviceType)
        {
            return this.TryGetService(serviceType, _RenderTarget);
        }

        public Vector4 GetProjection(Point3 v)
        {
            System.Diagnostics.Debug.Assert(Point3.IsFinite(v));
            return Vector4.Transform(v.XYZ, _Final);
        }

        public Vector2 GetPerspective(Vector4 v)
        {
            System.Diagnostics.Debug.Assert(v.IsFinite(), "Invalid Vertex: NaN");
            // System.Diagnostics.Debug.Assert(v.W > 0, "Invalid Vertex, W must be positive");

            var xy = new Vector2(v.X, v.Y) / v.W;
            xy = Vector2.Transform(xy, _Viewport);

            if (_Distorsion != null) xy = _Distorsion(xy);

            return xy;
        }        

        public float GetPerspectiveRadius(float radius, Point3 center)
        {
            // we transform two points separated by the radius distance
            // and we project them. This also allows handling distortion.

            var xradius = Vector3.TransformNormal(Vector3.UnitY * radius, _Camera);

            var a = Vector4.Transform(center.XYZ - xradius * 0.5f, _Final);
            var b = Vector4.Transform(center.XYZ + xradius * 0.5f, _Final);
            var aa = GetPerspective(a);
            var bb = GetPerspective(b);
            return (aa - bb).Length();
        }

        /// <inheritdoc/>
        public void DrawAsset(in Matrix4x4 transform, object asset)
        {
            var sphere = BoundingSphere.FromAsset(asset);

            if (!sphere.IsValid)
            {
                this.DrawAssetAsSurfaces(transform, asset);
                return;
            }

            var s = BoundingSphere.Transform(sphere, transform);

            if (_ClipPlane.IsInPositiveSideOfPlane(s.Center, s.Radius))
            {
                DrawAsset(transform, asset);
                return;
            }

            this.DrawAssetAsPrimitives(transform, asset);
        }

        /// <inheritdoc/>
        public void DrawSegments(ReadOnlySpan<Point3> vertices, float diameter, LineStyle brush)
        {
            for(int i=1; i < vertices.Length; ++i)
            {
                _DrawSegment(vertices[i-1],vertices[i],diameter,brush);
            }
        }

        /// <inheritdoc/>
        private void _DrawSegment(Point3 a, Point3 b, float diameter, LineStyle brush)
        {
            var aa = GetProjection(a);
            var bb = GetProjection(b);

            if (!_ClipPlane.ClipLineToPlane(ref aa, ref bb)) return;

            var cen = (a + b) * 0.5f;
            var aaa = GetPerspective(aa);
            var bbb = GetPerspective(bb);
            diameter = GetPerspectiveRadius(diameter, cen);
            var ow = GetPerspectiveRadius(brush.Style.OutlineWidth, cen);

            _RenderTarget.DrawLine(aaa, bbb, diameter, brush.WithOutline(ow));
        }        

        /// <inheritdoc/>
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            var aa = GetProjection(center);

            if (!_ClipPlane.IsInPositiveSideOfPlane(aa.SelectXYZ(), 0)) return;
            
            var aaa = GetPerspective(aa);
            diameter = GetPerspectiveRadius(diameter, center);
            var ow = GetPerspectiveRadius(brush.OutlineWidth, center);

            _RenderTarget.DrawEllipse(aaa, diameter, diameter, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
        public void DrawSurface(ReadOnlySpan<Point3> vertices, SurfaceStyle brush)
        {
            brush = brush.WithOutline(brush.Style.OutlineWidth * _CameraScale);

            Span<Vector4> projected = stackalloc Vector4[vertices.Length];
            for (int i = 0; i < projected.Length; ++i) projected[i] = GetProjection(vertices[i]);

            if (_ClipPlane.IsInPositiveSideOfPlane(projected))
            {
                _DrawProjectedPolygon(projected, Point3.Centroid(vertices), brush);
                return;
            }

            // clip polygon
            Span<Vector4> clippedProjected = stackalloc Vector4[vertices.Length * 2];
            var cvertices = _ClipPlane.ClipPolygonToPlane(clippedProjected, projected);
            if (cvertices < 3) return;
            clippedProjected = clippedProjected.Slice(0, cvertices);

            _DrawProjectedPolygon(clippedProjected, Point3.Centroid(vertices), brush);
        }

        /// <inheritdoc/>
        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle brush)
        {
            Span<Vector4> projected = stackalloc Vector4[vertices.Length];
            for (int i = 0; i < projected.Length; ++i) projected[i] = GetProjection(vertices[i]);

            if (_ClipPlane.IsInPositiveSideOfPlane(projected))
            {
                _DrawProjectedPolygon(projected, Point3.Centroid(vertices), brush);
                return;
            }

            // clip polygon
            Span<Vector4> clippedProjected = stackalloc Vector4[vertices.Length * 2];
            var cvertices = _ClipPlane.ClipPolygonToPlane(clippedProjected, projected);
            if (cvertices < 3) return;
            clippedProjected = clippedProjected.Slice(0, cvertices);

            _DrawProjectedPolygon(clippedProjected, Point3.Centroid(vertices), brush);
        }

        private void _DrawProjectedPolygon(ReadOnlySpan<Vector4> projected, Vector3 center, SurfaceStyle brush)
        {
            if (!brush.DoubleSided)
            {
                // TODO: face culling
            }

            Vector4 www = Vector4.Zero;

            Span<Point2> perspective = stackalloc Point2[projected.Length];
            for (int i = 0; i < perspective.Length; ++i)
            {
                var v = projected[i];
                www += v;
                perspective[i] = GetPerspective(v);
            }            

            brush = brush.WithOutline(GetPerspectiveRadius(brush.Style.OutlineWidth, center));

            _RenderTarget.DrawPolygon(perspective, brush.Style);
        }


        /// <summary>
        /// Draws the predefined scene using <see href="https://en.wikipedia.org/wiki/Painter%27s_algorithm"/>
        /// </summary>
        /// <param name="scene">the scene to render</param>
        public void DrawScene(Record3D scene, bool decompose = false)
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
                return Distance.CompareTo(other.Distance);
            }
        }

        #endregion
    }
}
