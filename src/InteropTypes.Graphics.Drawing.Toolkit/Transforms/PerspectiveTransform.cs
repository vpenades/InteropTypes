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
            return Create(viewport, new CameraTransform3D(camera, projectionFOV, null, 0.1f, 1000));
        }        

        public static PerspectiveTransform Create((ICanvas2D rt, float w, float h) viewport, CameraTransform3D camera, Func<Vector2, Vector2> distort = null)
        {
            if (!camera.IsValid) throw new ArgumentException("Invalid", nameof(camera));

            var viewMatrix = camera.CreateViewMatrix();
            var projMatrix = camera.CreateProjectionMatrix(viewport.w / viewport.h);

            var nearPlane = 0.1f;

            var vp = new Matrix3x2
                (0.5f * viewport.w, 0
                , 0, -0.5f * viewport.h
                , 0.5f * viewport.w, 0.5f * viewport.h);

            return new PerspectiveTransform(viewport.rt, distort, vp, projMatrix, viewMatrix, nearPlane);
        }

        private PerspectiveTransform(ICanvas2D renderTarget, Func<Vector2, Vector2> distorsion, Matrix3x2 viewport, Matrix4x4 proj, Matrix4x4 view, float nearPlane)
        {
            System.Diagnostics.Debug.Assert(nearPlane > 0);

            _Projection = view * proj;
            _ProjectionScale = _DecomposeScale(view); // we only use the Y Axis to avoid problems with the AspectRatio

            _ClipPlane = new Plane(Vector3.UnitZ, -nearPlane * 2f);

            _Viewport = viewport;
            _ViewportScale = new Vector2(viewport.M12, viewport.M22).Length(); // we only use the Y Axis to avoid problems with the AspectRatio

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

        private readonly Matrix4x4 _Projection;
        private readonly float _ProjectionScale;

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
            return Vector4.Transform(v.XYZ, _Projection);
        }

        public Vector2 GetPerspective(Vector4 v)
        {
            System.Diagnostics.Debug.Assert(v.IsFinite(), "Invalid Vertex: NaN");
            // System.Diagnostics.Debug.Assert(v.W > 0, "Invalid Vertex, W must be positive");

            var xy = new Vector2(v.X, v.Y) / v.Z;
            xy = Vector2.Transform(xy, _Viewport);

            if (_Distorsion != null) xy = _Distorsion(xy);

            return xy;
        }

        public float GetPerspective(float value, in Vector4 w)
        {
            return value * _ProjectionScale * _ViewportScale / w.Z;
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

            var www = (aa + bb) * 0.5f;
            var aaa = GetPerspective(aa);
            var bbb = GetPerspective(bb);
            diameter = GetPerspective(diameter, www);
            var ow = GetPerspective(brush.Style.OutlineWidth, www);

            _RenderTarget.DrawLine(aaa, bbb, diameter, brush = brush.WithOutline(ow));
        }        

        /// <inheritdoc/>
        public void DrawSphere(Point3 center, float diameter, OutlineFillStyle brush)
        {
            var aa = GetProjection(center);

            if (!_ClipPlane.IsInPositiveSideOfPlane(aa.SelectXYZ(), 0)) return;
            
            var aaa = GetPerspective(aa);
            diameter = GetPerspective(diameter, aa);
            var ow = GetPerspective(brush.OutlineWidth, aa);

            _RenderTarget.DrawEllipse(aaa, diameter, diameter, brush.WithOutline(ow));
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void DrawConvexSurface(ReadOnlySpan<Point3> vertices, ColorStyle brush)
        {
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

            Vector4 www = Vector4.Zero;

            Span<Point2> perspective = stackalloc Point2[projected.Length];
            for (int i = 0; i < perspective.Length; ++i)
            {
                var v = projected[i];
                www += v;
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
