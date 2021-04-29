using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

using XYZ = System.Numerics.Vector3;
using POINT3 = InteropDrawing.Point3;
using XFORM3 = System.Numerics.Matrix4x4;


namespace InteropDrawing
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(_CommandStream3D_DebuggerProxy))]
    sealed class _CommandStream3D : Collection.CommandList, IDrawing3D
    {
        #region IDrawing3D

        public void DrawAsset(in XFORM3 transform, object asset, ColorStyle brush)
        {
            var xref = AddHeaderAndStruct<_PrimitiveAsset>((int)_PrimitiveType.Asset);
            xref[0].Transform = transform;
            xref[0].AssetRef = AddReference(asset);
            xref[0].Style = brush;
        }

        public void DrawSegment(POINT3 a, POINT3 b, float diameter, LineStyle brush)
        {
            var xref = AddHeaderAndStruct<_PrimitiveSegment>((int)_PrimitiveType.Segment);
            xref[0].PointA = a;
            xref[0].PointB = b;
            xref[0].Diameter = diameter;
            xref[0].Style = brush;
        }

        public void DrawSphere(POINT3 center, float diameter, ColorStyle brush)
        {
            var xref = AddHeaderAndStruct<_PrimitiveSphere>((int)_PrimitiveType.Sphere);
            xref[0].Center = center;            
            xref[0].Diameter = diameter;
            xref[0].Style = brush;
        }

        public unsafe void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle brush)
        {
            AddHeader((int)_PrimitiveType.Surface, 4 + sizeof(_PrimitiveSurface) + vertices.Length * 12);

            var xref = AddStruct<_PrimitiveSurface>();
            xref[0].Count = vertices.Length;
            xref[0].Style = brush;

            AddArray(vertices);
        }

        #endregion

        #region API        

        public IEnumerable<int> GetCommandList(Func<XYZ,Single> scoreFunc)
        {
            var drawingOrder = new List<_SortableItem>();

            // We can use BinarySearch to locate the insertion point and do a sorted insertion.
            // Span<_SortableItem> drawingOrder = stackalloc _SortableItem[scene.Commands.Count];            

            foreach (var offset in this.GetCommands())
            {
                var center = this.GetCenter(offset);

                var item = new _SortableItem
                {
                    Offset = offset,
                    Score = scoreFunc(center)
                };

                drawingOrder.Add(item);
            }

            drawingOrder.Sort();

            return drawingOrder.Select(item => item.Offset);
        }


        private ReadOnlySpan<byte> _GetCommandBytes(int offset, out _PrimitiveType type, out int size)
        {
            var span = this.Buffer.Slice(offset);
            size = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            span = span.Slice(4, size);

            type = (_PrimitiveType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            return span.Slice(4);
        }

        public (XYZ Min, XYZ Max, XYZ Center, Single Radius) GetBounds()
        {
            var bounds = _BoundsContext.CreateEmpty();

            foreach (var offset in this.GetCommands())
            {
                var span = _GetCommandBytes(offset, out _PrimitiveType type, out int size);                

                switch (type)
                {
                    case _PrimitiveType.Segment: _PrimitiveSegment.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Sphere: _PrimitiveSphere.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Surface: _PrimitiveSurface.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Asset: _PrimitiveAsset.GetBounds(ref bounds, span, this.References); break;
                }
            }

            return (bounds.Min, bounds.Max, bounds.Sphere.Center, bounds.Sphere.Radius);            
        }

        public XYZ GetCenter(int offset)
        {
            var span = _GetCommandBytes(offset, out _PrimitiveType type, out _);

            switch (type)
            {
                case _PrimitiveType.Segment: return _PrimitiveSegment.GetCenter(span);
                case _PrimitiveType.Sphere: return _PrimitiveSphere.GetCenter(span);
                case _PrimitiveType.Surface: return _PrimitiveSurface.GetCenter(span);
                case _PrimitiveType.Asset: return _PrimitiveAsset.GetCenter(span);
            }

            throw new NotImplementedException();
        }        

        public int DrawTo(int offset, IDrawing3D dc, bool collapseAssets)
        {
            var span = _GetCommandBytes(offset, out _PrimitiveType type, out int size);

            switch (type)
            {
                case _PrimitiveType.Segment: { _PrimitiveSegment.DrawTo(dc, span); break; }
                case _PrimitiveType.Sphere: { _PrimitiveSphere.DrawTo(dc, span); break; }
                case _PrimitiveType.Surface: { _PrimitiveSurface.DrawTo(dc, span); break; }
                case _PrimitiveType.Asset: { _PrimitiveAsset.DrawTo(dc, span, References, collapseAssets); break; }
            }

            return size;
        }

        #endregion

        #region nested types

        [System.Diagnostics.DebuggerDisplay("{Index} at {Distance}")]
        struct _SortableItem : IComparable<_SortableItem>
        {
            public float Score;
            public int Offset;

            public int CompareTo(_SortableItem other)
            {
                return this.Score.CompareTo(other.Score);
            }
        }
        struct _BoundsContext
        {
            public static _BoundsContext CreateEmpty()
            {
                return new _BoundsContext
                {
                    Min = new XYZ(Single.MaxValue),
                    Max = new XYZ(Single.MinValue),
                    Sphere = (XYZ.Zero, -1)
                };
            }

            public XYZ Min;
            public XYZ Max;

            public (XYZ Center, Single Radius) Sphere;

            public void AddVertex(in Point3 vertex, float radius)
            {
                var v = vertex.ToNumerics();

                // Bounding Box
                Min = XYZ.Min(Min, v - new XYZ(radius));
                Max = XYZ.Max(Max, v + new XYZ(radius));

                // Bounding Sphere
                if (Sphere.Radius < 0) { Sphere = (v, radius); return; }

                Sphere = _MergeSpheres(Sphere, (v, radius));
            }

            private static (XYZ Center, Single Radius) _MergeSpheres(in (XYZ Center, Single Radius) left, in (XYZ Center, Single Radius) right)
            {
                var relative = right.Center - left.Center;

                float distance = relative.Length();

                // check if one sphere is inside the other
                if (distance <= left.Radius + right.Radius)
                {
                    if (distance <= left.Radius - right.Radius) return left;
                    if (distance <= right.Radius - left.Radius) return right;
                }

                float leftRadius = Math.Max(left.Radius - distance, right.Radius);
                float Rightradius = Math.Max(left.Radius + distance, right.Radius);

                relative = relative + (((leftRadius - Rightradius) / (2 * distance)) * relative);

                return (left.Center + relative, (leftRadius + Rightradius) / 2);
            }
        }

        struct _PrimitiveSegment
        {
            public POINT3 PointA;
            public POINT3 PointB;
            public Single Diameter;
            public LineStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSegment>(command)[0];

                var r = src.Diameter * 0.5f + src.Style.Style.OutlineWidth;
                bounds.AddVertex(src.PointA, r);
                bounds.AddVertex(src.PointB, r);
            }

            public static XYZ GetCenter(ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSegment>(command)[0];

                var a = src.PointA.ToNumerics();
                var b = src.PointB.ToNumerics();

                return (a + b) * 0.5f;
            }           

            public static void DrawTo(IDrawing3D dst, ReadOnlySpan<Byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSegment>(command)[0];
                dst.DrawSegment(body.PointA, body.PointB, body.Diameter, body.Style);
            }
        }

        struct _PrimitiveSphere
        {
            public POINT3 Center;            
            public Single Diameter;
            public ColorStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSphere>(command)[0];

                var r = src.Diameter * 0.5f + src.Style.OutlineWidth;
                bounds.AddVertex(src.Center, r);                
            }

            public static XYZ GetCenter(ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSphere>(command)[0];
                return src.Center.ToNumerics();
            }

            public static void DrawTo(IDrawing3D dst, ReadOnlySpan<Byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSphere>(command)[0];
                dst.DrawSphere(body.Center, body.Diameter, body.Style);
            }
        }

        struct _PrimitiveSurface
        {
            public int Count;            
            public SurfaceStyle Style;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSurface>(command)[0];
                var points = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, XYZ>(command.Slice(sizeof(_PrimitiveSurface)));

                foreach (var v in points) bounds.AddVertex(v, src.Style.Style.OutlineWidth);
            }

            public static unsafe XYZ GetCenter(ReadOnlySpan<Byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSurface>(command)[0];
                var points = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, XYZ>(command.Slice(sizeof(_PrimitiveSurface)));

                var center = XYZ.Zero;                

                for(int i=0; i < body.Count; ++i) { center += points[i]; }

                return body.Count == 0 ? XYZ.Zero : center / body.Count;
            }            

            public static unsafe void DrawTo(IDrawing3D dst, ReadOnlySpan<Byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSurface>(command)[0];
                var points = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, POINT3>(command.Slice(sizeof(_PrimitiveSurface)));

                dst.DrawSurface(points.Slice(0, body.Count), body.Style);
            }
        }

        struct _PrimitiveAsset
        {
            public XFORM3 Transform;
            public int AssetRef;
            public ColorStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command, IReadOnlyList<Object> references)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveAsset>(command)[0];
                var xref = references[src.AssetRef];

                if (src.Transform == XFORM3.Identity)
                {
                    var box = Toolkit.GetAssetBoundingMinMax(xref);

                    // todo: add all 8 vertices
                    if (box.HasValue)
                    {
                        bounds.AddVertex(box.Value.Min, 0);
                        bounds.AddVertex(box.Value.Max, 0);
                    }
                }
                else
                {
                    var sphere = Toolkit.GetAssetBoundingSphere(xref);
                    if (sphere.HasValue)
                    {
                        var (c, s) = src.Transform.TransformSphere(sphere.Value);
                        bounds.AddVertex(c, s);
                    }
                }
            }

            public static XYZ GetCenter(ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveAsset>(command)[0];
                return src.Transform.Translation;
            }

            public static void DrawTo(IDrawing3D dst, ReadOnlySpan<Byte> command, IReadOnlyList<Object> references, bool collapse)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveAsset>(command)[0];

                if (collapse) { dst.DrawAssetAsSurfaces(body.Transform, references[body.AssetRef], body.Style); return; }

                dst.DrawAsset(body.Transform, references[body.AssetRef], body.Style);
            }
        }

        private enum _PrimitiveType
        {
            None,
            Segment,
            Sphere,
            Surface,
            Billboard,
            Asset
        }

        #endregion
    }

    class _CommandStream3D_DebuggerProxy
    {
        public _CommandStream3D_DebuggerProxy(_CommandStream3D src)
        {
            var sb = new List<string>();
            var target = Diagnostics.CommandLogger.From(sb);

            foreach (var offset in src.GetCommands())
            {
                src.DrawTo(offset, target, false);
            }

            Primitives = sb.ToArray();
        }

        public readonly string[] Primitives;
    }
}
