using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

using XYZ = System.Numerics.Vector3;
using XFORM3 = System.Numerics.Matrix4x4;

using POINT3 = InteropTypes.Graphics.Drawing.Point3;


namespace InteropTypes.Graphics.Drawing
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(_CommandStream3D_DebuggerProxy))]    
    sealed class _CommandStream3D : Collection.CommandList, IScene3D
    {
        #region IScene3D

        public void DrawAsset(in XFORM3 transform, object asset)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveAsset>((int)_PrimitiveType.Asset)[0];
            xref.Transform = transform;
            xref.AssetRef = AddReference(asset);            
        }
        
        public unsafe void DrawSegments(ReadOnlySpan<POINT3> vertices, float diameter, LineStyle brush)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveSegments>((int)_PrimitiveType.Segments, vertices)[0];
            xref.Count = vertices.Length;
            xref.Diameter = diameter;
            xref.Style = brush;
        }

        public void DrawSphere(POINT3 center, float diameter, OutlineFillStyle brush)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveSphere>((int)_PrimitiveType.Sphere)[0];
            xref.Center = center;
            xref.Diameter = diameter;
            xref.Style = brush;
        }

        public unsafe void DrawSurface(ReadOnlySpan<POINT3> vertices, SurfaceStyle brush)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveSurface>((int)_PrimitiveType.Surface, vertices)[0];
            xref.Count = vertices.Length;
            xref.Style = brush;            
        }

        public unsafe void DrawConvexSurface(ReadOnlySpan<POINT3> vertices, ColorStyle style)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveConvex>((int)_PrimitiveType.Convex, vertices)[0];            
            xref.Count = vertices.Length;
            xref.Style = style;            
        }        

        #endregion

        #region API        

        public IEnumerable<int> GetCommandList(Func<XYZ, float> scoreFunc)
        {
            var drawingOrder = new List<_SortableItem>();

            // We can use BinarySearch to locate the insertion point and do a sorted insertion.
            // Span<_SortableItem> drawingOrder = stackalloc _SortableItem[scene.Commands.Count];            

            foreach (var offset in GetCommands())
            {
                var center = GetCenter(offset);

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

        private ReadOnlySpan<byte> _GetCommandBytes(int offset, out _PrimitiveType type)
        {
            var span = Buffer.Slice(offset);
            var size = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);

            span = span.Slice(4, size);

            type = (_PrimitiveType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);

            return span.Slice(4);
        }

        public (XYZ Min, XYZ Max, BoundingSphere Sphere) GetBounds()
        {
            var bounds = _BoundsContext.CreateEmpty();

            foreach (var offset in GetCommands())
            {
                var span = _GetCommandBytes(offset, out _PrimitiveType type);

                switch (type)
                {
                    case _PrimitiveType.Convex: _PrimitiveConvex.GetBounds(ref bounds, span); break;                    
                    case _PrimitiveType.Segments: _PrimitiveSegments.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Sphere: _PrimitiveSphere.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Surface: _PrimitiveSurface.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Asset: _PrimitiveAsset.GetBounds(ref bounds, span, References); break;
                    default: throw new NotImplementedException();
                }
            }

            return (bounds.Min, bounds.Max, bounds.Sphere);
        }

        public XYZ GetCenter(int offset)
        {
            var span = _GetCommandBytes(offset, out _PrimitiveType type);

            switch (type)
            {
                case _PrimitiveType.Convex: return _PrimitiveConvex.GetCenter(span);                
                case _PrimitiveType.Segments: return _PrimitiveSegments.GetCenter(span);
                case _PrimitiveType.Sphere: return _PrimitiveSphere.GetCenter(span);
                case _PrimitiveType.Surface: return _PrimitiveSurface.GetCenter(span);
                case _PrimitiveType.Asset: return _PrimitiveAsset.GetCenter(span);
                default: throw new NotImplementedException();
            }         
        }

        public void DrawTo(int offset, IScene3D dc, bool collapseAssets)
        {
            var span = _GetCommandBytes(offset, out _PrimitiveType type);

            switch (type)
            {
                case _PrimitiveType.Convex: { _PrimitiveConvex.DrawTo(dc, span); break; }
                case _PrimitiveType.Segments: { _PrimitiveSegments.DrawTo(dc, span); break; }
                case _PrimitiveType.Sphere: { _PrimitiveSphere.DrawTo(dc, span); break; }
                case _PrimitiveType.Surface: { _PrimitiveSurface.DrawTo(dc, span); break; }
                case _PrimitiveType.Asset: { _PrimitiveAsset.DrawTo(dc, span, References, collapseAssets); break; }
                default: throw new NotImplementedException();
            }        
        }

        #endregion

        #region nested types

        private static unsafe void _GetHeaderAndPoints<THeader>(ReadOnlySpan<byte> stream, ref THeader header, out ReadOnlySpan<POINT3> points)
            where THeader:unmanaged, IPointsCount
        {
            header = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, THeader>(stream)[0];

            points = System.Runtime.InteropServices.MemoryMarshal
                .Cast<byte, POINT3>(stream.Slice(sizeof(THeader)))
                .Slice(0, header.GetCount());
        }

        private static unsafe void _GetHeaderAndVectors<THeader>(ReadOnlySpan<byte> stream, ref THeader header, out ReadOnlySpan<XYZ> points)
            where THeader : unmanaged, IPointsCount
        {
            header = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, THeader>(stream)[0];

            points = System.Runtime.InteropServices.MemoryMarshal
                .Cast<byte, XYZ>(stream.Slice(sizeof(THeader)))
                .Slice(0, header.GetCount());
        }

        [System.Diagnostics.DebuggerDisplay("{Index} at {Distance}")]
        struct _SortableItem : IComparable<_SortableItem>
        {
            public float Score;
            public int Offset;

            public int CompareTo(_SortableItem other)
            {
                return Score.CompareTo(other.Score);
            }
        }

        struct _BoundsContext
        {
            public static _BoundsContext CreateEmpty()
            {
                return new _BoundsContext
                {
                    Min = new XYZ(float.MaxValue),
                    Max = new XYZ(float.MinValue),
                    Sphere = (XYZ.Zero, -1)
                };
            }

            public XYZ Min;
            public XYZ Max;

            public BoundingSphere Sphere;

            public void AddVertex(in POINT3 vertex, float radius)
            {
                var v = vertex.ToNumerics();

                // Bounding Box
                Min = XYZ.Min(Min, v - new XYZ(radius));
                Max = XYZ.Max(Max, v + new XYZ(radius));

                // Bounding Sphere
                if (Sphere.Radius < 0) { Sphere = (v, radius); return; }

                Sphere = BoundingSphere.Merge(Sphere, (v, radius));
            }

            
        }

        interface IPointsCount { int GetCount(); }        

        struct _PrimitiveSegments : IPointsCount
        {
            public int Count;
            public float Diameter;
            public LineStyle Style;

            int IPointsCount.GetCount() => Count;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                _PrimitiveSegments header = default;
                _GetHeaderAndPoints(command, ref header, out var points);

                var r = header.Diameter * 0.5f + header.Style.Style.OutlineWidth;

                foreach (var v in points) bounds.AddVertex(v, r);
            }

            public static unsafe XYZ GetCenter(ReadOnlySpan<byte> command)
            {
                _PrimitiveSegments header = default;
                _GetHeaderAndVectors(command, ref header, out var points);

                var center = XYZ.Zero;

                for (int i = 0; i < header.Count; ++i) { center += points[i]; }

                return header.Count == 0 ? XYZ.Zero : center / header.Count;
            }

            public static unsafe void DrawTo(IScene3D dst, ReadOnlySpan<byte> command)
            {
                _PrimitiveSegments header = default;
                _GetHeaderAndPoints(command, ref header, out var points);

                dst.DrawSegments(points.Slice(0, header.Count), header.Diameter, header.Style);
            }
        }

        struct _PrimitiveSphere
        {
            public POINT3 Center;
            public float Diameter;
            public OutlineFillStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveSphere>(command)[0];

                var r = src.Diameter * 0.5f + src.Style.OutlineWidth;
                bounds.AddVertex(src.Center, r);
            }

            public static XYZ GetCenter(ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveSphere>(command)[0];
                return src.Center.ToNumerics();
            }

            public static void DrawTo(IScene3D dst, ReadOnlySpan<byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveSphere>(command)[0];
                dst.DrawSphere(body.Center, body.Diameter, body.Style);
            }
        }

        struct _PrimitiveConvex : IPointsCount
        {
            public int Count;
            public ColorStyle Style;

            int IPointsCount.GetCount() => Count;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                _PrimitiveConvex header = default;
                _GetHeaderAndVectors(command, ref header, out var points);

                foreach (var v in points) bounds.AddVertex(v, 0);
            }

            public static unsafe XYZ GetCenter(ReadOnlySpan<byte> command)
            {
                _PrimitiveConvex header = default;
                _GetHeaderAndVectors(command, ref header, out var points);

                var center = XYZ.Zero;

                for (int i = 0; i < header.Count; ++i) { center += points[i]; }

                return header.Count == 0 ? XYZ.Zero : center / header.Count;
            }

            public static unsafe void DrawTo(IScene3D dst, ReadOnlySpan<byte> command)
            {
                _PrimitiveConvex header = default;
                _GetHeaderAndPoints(command, ref header, out var points);

                dst.DrawConvexSurface(points.Slice(0, header.Count), header.Style);
            }
        }

        struct _PrimitiveSurface : IPointsCount
        {
            public int Count;
            public SurfaceStyle Style;

            int IPointsCount.GetCount() => Count;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                _PrimitiveSurface header = default;
                _GetHeaderAndPoints(command, ref header, out var points);

                foreach (var v in points) bounds.AddVertex(v, header.Style.Style.OutlineWidth);
            }

            public static unsafe XYZ GetCenter(ReadOnlySpan<byte> command)
            {
                _PrimitiveSurface header = default;
                _GetHeaderAndVectors(command, ref header, out var points);

                var center = XYZ.Zero;

                for (int i = 0; i < header.Count; ++i) { center += points[i]; }

                return header.Count == 0 ? XYZ.Zero : center / header.Count;
            }

            public static unsafe void DrawTo(IScene3D dst, ReadOnlySpan<byte> command)
            {
                _PrimitiveSurface header = default;
                _GetHeaderAndPoints(command, ref header, out var points);

                dst.DrawSurface(points.Slice(0, header.Count), header.Style);
            }
        }

        struct _PrimitiveAsset
        {
            public XFORM3 Transform;
            public int AssetRef;            

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command, IReadOnlyList<object> references)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveAsset>(command)[0];
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
                    var sphere = BoundingSphere.FromAsset(xref);
                    if (sphere.IsValid)
                    {
                        var s = BoundingSphere.Transform(sphere, src.Transform);
                        bounds.AddVertex(s.Center, s.Radius);
                    }
                }
            }

            public static XYZ GetCenter(ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveAsset>(command)[0];
                return src.Transform.Translation;
            }

            public static void DrawTo(IScene3D dst, ReadOnlySpan<byte> command, IReadOnlyList<object> references, bool collapse)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveAsset>(command)[0];

                if (collapse) { dst.DrawAssetAsSurfaces(body.Transform, references[body.AssetRef]); return; }

                dst.DrawAsset(body.Transform, references[body.AssetRef]);
            }
        }

        private enum _PrimitiveType
        {
            None,
            Convex,            
            Segments,
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
