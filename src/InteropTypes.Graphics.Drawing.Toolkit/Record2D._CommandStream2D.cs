using System;
using System.Collections.Generic;

using XY = System.Numerics.Vector2;
using XFORM2 = System.Numerics.Matrix3x2;

using COLOR = System.Drawing.Color;
using POINT2 = InteropTypes.Graphics.Drawing.Point2;

namespace InteropTypes.Graphics.Drawing
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(_CommandStream2D_DebuggerProxy))]
    sealed class _CommandStream2D : Collection.CommandList, ICanvas2D
    {
        #region constructors 2D

        /// <inheritdoc/>
        public void DrawAsset(in XFORM2 xform, object asset, ColorStyle color)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveAsset>((int)_PrimitiveType.Asset)[0];
            xref.Transform = xform;
            xref.AssetRef = AddReference(asset);
            xref.Style = color;
        }

        /// <inheritdoc/>
        public unsafe void DrawConvexPolygon(ReadOnlySpan<POINT2> points, ColorStyle color)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveConvex>((int)_PrimitiveType.Convex, points)[0];            
            xref.Count = points.Length;
            xref.Color = color;
        }

        /// <inheritdoc/>
        public unsafe void DrawLines(ReadOnlySpan<POINT2> points, float diameter, LineStyle brush)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitivePolyLine>((int)_PrimitiveType.PolyLine, points)[0];
            xref.Count = points.Length;
            xref.Diameter = diameter;
            xref.Style = brush;
        }

        /// <inheritdoc/>
        public unsafe void DrawEllipse(POINT2 center, float width, float height, OutlineFillStyle brush)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveEllipse>((int)_PrimitiveType.Ellipse)[0];
            xref.Center = center;
            xref.Width = width;
            xref.Height = height;
            xref.Style = brush;
        }

        /// <inheritdoc/>
        public unsafe void DrawPolygon(ReadOnlySpan<POINT2> points, PolygonStyle brush)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitivePolygon>((int)_PrimitiveType.Polygon, points)[0];
            xref.Count = points.Length;
            xref.Style = brush;
        }

        /// <inheritdoc/>
        public void DrawImage(in XFORM2 xform, ImageStyle style)
        {
            ref var xref = ref AddHeaderAndStruct<_PrimitiveImage>((int)_PrimitiveType.Image)[0];
            xref.Transform = xform;
            xref.ImageRef = AddReference(style.Bitmap);
            xref.Flags = style.Flags;
            xref.Color = style.Color;
        }

        #endregion

        #region API        

        private ReadOnlySpan<byte> _GetCommandBytes(int offset, out _PrimitiveType type, out int size)
        {
            var span = Buffer.Slice(offset);
            size = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            span = span.Slice(4, size);

            type = (_PrimitiveType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            return span.Slice(4);
        }

        public (XY Min, XY Max, XY Center, float Radius) GetBounds()
        {
            var bounds = _BoundsContext.CreateEmpty();

            foreach (var offset in GetCommands())
            {
                var span = _GetCommandBytes(offset, out _PrimitiveType type, out int size);

                switch (type)
                {
                    case _PrimitiveType.PolyLine: _PrimitivePolyLine.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Ellipse: _PrimitiveEllipse.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Polygon: _PrimitivePolygon.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Convex: { _PrimitiveConvex.GetBounds(ref bounds, span); break; }
                    case _PrimitiveType.Image: _PrimitiveImage.GetBounds(ref bounds, span, References); break;
                    case _PrimitiveType.Asset: _PrimitiveAsset.GetBounds(ref bounds, span, References); break;
                }
            }

            return (bounds.Min, bounds.Max, bounds.Circle.Center, bounds.Circle.Radius);
        }

        public unsafe void DrawTo(int offset, ICanvas2D dc, bool collapseAssets)
        {
            var span = _GetCommandBytes(offset, out _PrimitiveType type, out _);

            switch (type)
            {                
                case _PrimitiveType.PolyLine: { _PrimitivePolyLine.DrawTo(dc, span); break; }
                case _PrimitiveType.Ellipse: { _PrimitiveEllipse.DrawTo(dc, span); break; }
                case _PrimitiveType.Polygon: { _PrimitivePolygon.DrawTo(dc, span); break; }
                case _PrimitiveType.Convex: { _PrimitiveConvex.DrawTo(dc, span); break; }
                case _PrimitiveType.Image: { _PrimitiveImage.DrawTo(dc, span, References); break; }                
                case _PrimitiveType.Asset: { _PrimitiveAsset.DrawTo(dc, span, References, collapseAssets); break; }
            }
        }

        #endregion

        #region nested types     

        struct _BoundsContext
        {
            public static _BoundsContext CreateEmpty()
            {
                return new _BoundsContext
                {
                    Min = new XY(float.MaxValue),
                    Max = new XY(float.MinValue),
                    Circle = (XY.Zero, -1)
                };
            }

            public XY Min;
            public XY Max;

            public (XY Center, float Radius) Circle;

            public void AddVertex(in XY vertex, float radius)
            {
                // Bounding Box
                Min = XY.Min(Min, vertex - new XY(radius));
                Max = XY.Max(Max, vertex + new XY(radius));

                // Bounding Circle
                if (Circle.Radius < 0) { Circle = (vertex, radius); return; }

                Circle = _MergeCircles(Circle, (vertex, radius));
            }

            private static (XY Center, float Radius) _MergeCircles(in (XY Center, float Radius) left, in (XY Center, float Radius) right)
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

                relative += (leftRadius - Rightradius) / (2 * distance) * relative;

                return (left.Center + relative, (leftRadius + Rightradius) / 2);
            }
        }

        struct _PrimitiveConvex
        {
            public int Count;
            public ColorStyle Color;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, XY>(command.Slice(sizeof(_PrimitiveConvex)));
                foreach (var p in pts) bounds.AddVertex(p, 0);
            }

            public static unsafe void DrawTo(IPrimitiveCanvas2D dst, ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveConvex>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, POINT2>(command.Slice(sizeof(_PrimitiveConvex)));

                dst.DrawConvexPolygon(pts.Slice(0, src.Count), src.Color);
            }
        }        

        struct _PrimitivePolyLine
        {
            public int Count;
            public float Diameter;
            public LineStyle Style;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitivePolyLine>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, XY>(command.Slice(sizeof(_PrimitivePolyLine)));

                var r = src.Diameter * 0.5f + src.Style.Style.OutlineWidth;

                foreach (var p in pts) bounds.AddVertex(p, r);
            }

            public static unsafe void DrawTo(ICanvas2D dst, ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitivePolyLine>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, POINT2>(command.Slice(sizeof(_PrimitivePolyLine)));

                dst.DrawLines(pts.Slice(0, src.Count), src.Diameter, src.Style);
            }
        }

        struct _PrimitiveEllipse
        {
            public POINT2 Center;
            public float Width;
            public float Height;
            public OutlineFillStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveEllipse>(command)[0];

                var c = src.Center.ToNumerics();
                var r = src.Style.OutlineWidth;
                var rr = new XY(src.Width, src.Height) * 0.5f;

                bounds.AddVertex(c - rr, r);
                bounds.AddVertex(c + rr, r);
            }

            public static unsafe void DrawTo(ICanvas2D dst, ReadOnlySpan<byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveEllipse>(command)[0];
                dst.DrawEllipse(body.Center, body.Width, body.Height, body.Style);
            }
        }

        struct _PrimitivePolygon
        {
            public int Count;
            public PolygonStyle Style;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitivePolygon>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, XY>(command.Slice(sizeof(_PrimitivePolygon)));

                var r = src.Style.OutlineWidth;

                foreach (var p in pts) bounds.AddVertex(p, r);
            }

            public static unsafe void DrawTo(ICanvas2D dst, ReadOnlySpan<byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitivePolygon>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, POINT2>(command.Slice(sizeof(_PrimitivePolygon)));

                dst.DrawPolygon(pts.Slice(0, src.Count), src.Style);
            }
        }

        struct _PrimitiveImage
        {
            public XFORM2 Transform;
            public int ImageRef;
            public int Flags;
            public ColorStyle Color;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command, IReadOnlyList<object> references)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveImage>(command)[0];                

                var xref = (ImageAsset)references[src.ImageRef];
                var style = new ImageStyle(xref, src.Color, src.Flags);

                var xform = src.Transform;
                style.PrependTransform(ref xform);

                var a = XY.Transform(XY.Zero, xform);
                var b = XY.Transform(XY.UnitX, xform);
                var c = XY.Transform(XY.One, xform);
                var d = XY.Transform(XY.UnitY, xform);

                bounds.AddVertex(a, 0);
                bounds.AddVertex(b, 0);
                bounds.AddVertex(c, 0);
                bounds.AddVertex(d, 0);
            }

            public static void DrawTo(IPrimitiveCanvas2D dst, ReadOnlySpan<byte> command, IReadOnlyList<object> references)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveImage>(command)[0];

                var xref = (ImageAsset)references[src.ImageRef];
                var style = new ImageStyle(xref, src.Color, src.Flags);

                dst.DrawImage(src.Transform, style);
            }
        }

        struct _PrimitiveAsset
        {
            public XFORM2 Transform;
            public int AssetRef;
            public ColorStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<byte> command, IReadOnlyList<object> references)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveAsset>(command)[0];

                var asset = references[src.AssetRef];

                if (src.Transform == XFORM2.Identity)
                {
                    var rect = Toolkit.GetAssetBoundingRect(asset);

                    // todo: add all 8 vertices
                    if (rect.HasValue)
                    {
                        bounds.AddVertex(rect.Value.Location.ToVector2(), 0);
                        bounds.AddVertex(rect.Value.Location.ToVector2() + rect.Value.Size.ToVector2(), 0);
                    }
                }
                else
                {
                    var circle = Toolkit.GetAssetBoundingCircle(asset);
                    if (circle.HasValue)
                    {
                        var (c, r) = src.Transform.TransformCircle(circle.Value);
                        bounds.AddVertex(c, r);
                    }
                }

            }

            public static void DrawTo(ICanvas2D dst, ReadOnlySpan<byte> command, IReadOnlyList<object> references, bool collapse)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, _PrimitiveAsset>(command)[0];

                if (collapse)
                {
                    new Transforms.Decompose2D(dst).DrawAsset(body.Transform, references[body.AssetRef], body.Style);
                    return;
                }

                dst.DrawAsset(body.Transform, references[body.AssetRef], ColorStyle.White);
            }
        }

        private enum _PrimitiveType
        {
            None,            
            Convex,
            Image,
            Asset,
            PolyLine,
            Ellipse,
            Polygon
        }

        #endregion
    }

    class _CommandStream2D_DebuggerProxy
    {
        public _CommandStream2D_DebuggerProxy(_CommandStream2D src)
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
