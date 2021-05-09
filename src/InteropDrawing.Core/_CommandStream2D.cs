using System;
using System.Collections.Generic;

using XY = System.Numerics.Vector2;

using COLOR = System.Drawing.Color;
using POINT2 = InteropDrawing.Point2;
using XFORM2 = System.Numerics.Matrix3x2;


namespace InteropDrawing
{
    [System.Diagnostics.DebuggerTypeProxy(typeof(_CommandStream2D_DebuggerProxy))]
    sealed class _CommandStream2D : Collection.CommandList, IDrawing2D
    {
        #region constructors 2D

        public unsafe void DrawLines(ReadOnlySpan<Point2> points, Single diameter, LineStyle brush)
        {
            if (points.Length > 2)
            {
                AddHeader((int)_PrimitiveType.PolyLine, 4 + sizeof(_PrimitivePolyLine) + points.Length * 8);

                var xref = AddStruct<_PrimitivePolyLine>();
                xref[0].Count = points.Length;
                xref[0].Diameter = diameter;
                xref[0].Style = brush;

                AddArray(points);
            }
            else
            {
                var xref = AddHeaderAndStruct<_PrimitiveLine>((int)_PrimitiveType.Line);
                xref[0].PointA = points[0];
                xref[0].PointB = points[1];
                xref[0].Diameter = diameter;
                xref[0].Style = brush;
            }
        }

        public unsafe void DrawEllipse(Point2 center, Single width, Single height, ColorStyle brush)
        {
            var xref = AddHeaderAndStruct<_PrimitiveEllipse>((int)_PrimitiveType.Ellipse);
            xref[0].Center = center;
            xref[0].Width = width;
            xref[0].Height = height;
            xref[0].Style = brush;
        }

        public unsafe void DrawPolygon(ReadOnlySpan<Point2> points, ColorStyle brush)
        {
            int count = this.Count;

            AddHeader((int)_PrimitiveType.Polygon, 4 + sizeof(_PrimitivePolygon) + points.Length * 8);

            var xref = AddStruct<_PrimitivePolygon>();
            xref[0].Count = points.Length;            
            xref[0].Style = brush;

            AddArray(points);
        }

        public void DrawSprite(in XFORM2 xform, in SpriteStyle style)
        {
            var xref = AddHeaderAndStruct<_PrimitiveSprite>((int)_PrimitiveType.Sprite);
            xref[0].Transform = xform;
            xref[0].BitmapCellRef = AddReference(style.Bitmap);
            xref[0].Flags = (style.FlipHorizontal ? 0 : 1) | (style.FlipVertical ? 0 : 2);
            xref[0].Color = style.Color.ToArgb();
        }

        public void DrawAsset(in XFORM2 xform, Object asset, ColorStyle brush)
        {
            var xref = AddHeaderAndStruct<_PrimitiveAsset>((int)_PrimitiveType.Asset);
            xref[0].Transform = xform;
            xref[0].AssetRef = AddReference(asset);
            xref[0].Style = brush;
        }

        #endregion

        #region API        

        private ReadOnlySpan<byte> _GetCommandBytes(int offset, out _PrimitiveType type, out int size)
        {
            var span = this.Buffer.Slice(offset);
            size = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            span = span.Slice(4, size);

            type = (_PrimitiveType)System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(span);
            return span.Slice(4);
        }

        public (XY Min, XY Max, XY Center, Single Radius) GetBounds()
        {
            var bounds = _BoundsContext.CreateEmpty();            

            foreach (var offset in this.GetCommands())
            {
                var span = _GetCommandBytes(offset, out _PrimitiveType type, out int size);                

                switch (type)
                {
                    case _PrimitiveType.Line: _PrimitiveLine.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.PolyLine: _PrimitiveLine.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Ellipse: _PrimitiveEllipse.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Polygon: _PrimitivePolygon.GetBounds(ref bounds, span); break;
                    case _PrimitiveType.Sprite: _PrimitiveSprite.GetBounds(ref bounds, span, this.References); break;
                    case _PrimitiveType.Asset: _PrimitiveAsset.GetBounds(ref bounds, span, this.References); break;
                }
            }

            return (bounds.Min, bounds.Max, bounds.Circle.Center, bounds.Circle.Radius);
        }

        public unsafe void DrawTo(int offset, IDrawing2D dc, bool collapseAssets)
        {
            var span = _GetCommandBytes(offset, out _PrimitiveType type, out _);

            switch (type)
            {
                case _PrimitiveType.Line: { _PrimitiveLine.DrawTo(dc, span); break; }
                case _PrimitiveType.PolyLine: { _PrimitivePolyLine.DrawTo(dc, span); break; }
                case _PrimitiveType.Ellipse: { _PrimitiveEllipse.DrawTo(dc, span); break; }
                case _PrimitiveType.Polygon: { _PrimitivePolygon.DrawTo(dc, span); break; }
                case _PrimitiveType.Sprite: { _PrimitiveSprite.DrawTo(dc, span, this.References); break; }
                case _PrimitiveType.Asset: { _PrimitiveAsset.DrawTo(dc, span, this.References, collapseAssets); break; }
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
                    Min = new XY(Single.MaxValue),
                    Max = new XY(Single.MinValue),
                    Circle = (XY.Zero, -1)
                };
            }

            public XY Min;
            public XY Max;

            public (XY Center, Single Radius) Circle;

            public void AddVertex(in XY vertex, float radius)
            {
                // Bounding Box
                Min = XY.Min(Min, vertex - new XY(radius));
                Max = XY.Max(Max, vertex + new XY(radius));

                // Bounding Circle
                if (Circle.Radius < 0) { Circle = (vertex, radius); return; }

                Circle = _MergeCircles(Circle, (vertex, radius));
            }

            private static (XY Center, Single Radius) _MergeCircles(in (XY Center, Single Radius) left, in (XY Center, Single Radius) right)
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

        struct _PrimitiveLine
        {
            public Point2 PointA;
            public Point2 PointB;
            public Single Diameter;
            public LineStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveLine>(command)[0];

                var r = src.Diameter * 0.5f + src.Style.Style.OutlineWidth;

                var a = src.PointA.ToNumerics();
                var b = src.PointB.ToNumerics();

                bounds.AddVertex(a, r);
                bounds.AddVertex(b, r);
            }

            public static unsafe void DrawTo(IDrawing2D dst, ReadOnlySpan<Byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveLine>(command)[0];
                dst.DrawLine(body.PointA, body.PointB, body.Diameter, body.Style);
            }            
        }

        struct _PrimitivePolyLine
        {
            public int Count;
            public Single Diameter;
            public LineStyle Style;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitivePolyLine>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, XY>(command.Slice(sizeof(_PrimitivePolyLine)));

                var r = src.Diameter * 0.5f + src.Style.Style.OutlineWidth;

                foreach (var p in pts) bounds.AddVertex(p, r);
            }

            public static unsafe void DrawTo(IDrawing2D dst, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitivePolyLine>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Point2>(command.Slice(sizeof(_PrimitivePolyLine)));

                dst.DrawLines(pts.Slice(0, src.Count), src.Diameter, src.Style);
            }            
        }            

        struct _PrimitiveEllipse
        {            
            public Point2 Center;
            public float Width;
            public float Height;
            public ColorStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveEllipse>(command)[0];

                var c = src.Center.ToNumerics();
                var r = src.Style.OutlineWidth;
                var rr = new XY(src.Width, src.Height) * 0.5f;
                
                bounds.AddVertex(c - rr, r);
                bounds.AddVertex(c + rr, r);
            }

            public static unsafe void DrawTo(IDrawing2D dst, ReadOnlySpan<Byte> command)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveEllipse>(command)[0];
                dst.DrawEllipse(body.Center, body.Width, body.Height, body.Style);
            }            
        }

        struct _PrimitivePolygon
        {
            public int Count;            
            public ColorStyle Style;

            public static unsafe void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitivePolygon>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, XY>(command.Slice(sizeof(_PrimitivePolygon)));

                var r = src.Style.OutlineWidth;

                foreach (var p in pts) bounds.AddVertex(p, r);
            }

            public static unsafe void DrawTo(IDrawing2D dst, ReadOnlySpan<Byte> command)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitivePolygon>(command)[0];
                var pts = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, Point2>(command.Slice(sizeof(_PrimitivePolygon)));

                dst.DrawPolygon(pts.Slice(0, src.Count), src.Style);
            }            
        }

        struct _PrimitiveSprite
        {
            public XFORM2 Transform;
            public int BitmapCellRef;
            public int Flags;
            public int Color;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command, IReadOnlyList<Object> references)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSprite>(command)[0];                

                var cell = references[src.BitmapCellRef] as SpriteAsset;

                var offset = -cell.Pivot;

                var a = XY.Zero + offset;
                var b = new XY(cell.Width, 0) + offset;
                var c = new XY(cell.Width, cell.Height) + offset;
                var d = new XY(0, cell.Height) + offset;

                a = XY.Transform(cell.Scale * a, src.Transform);
                b = XY.Transform(cell.Scale * b, src.Transform);
                c = XY.Transform(cell.Scale * c, src.Transform);
                d = XY.Transform(cell.Scale * d, src.Transform);

                bounds.AddVertex(a, 0);
                bounds.AddVertex(b, 0);
                bounds.AddVertex(c, 0);
                bounds.AddVertex(d, 0);
            }

            public static void DrawTo(IDrawing2D dst, ReadOnlySpan<Byte> command, IReadOnlyList<Object> references)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveSprite>(command)[0];

                var xref = (SpriteAsset)references[body.BitmapCellRef];
                var style = new SpriteStyle(xref, COLOR.FromArgb(body.Color), (body.Flags & 1) != 0, (body.Flags & 2) != 0);

                dst.DrawSprite(body.Transform, style);
            }            
        }

        struct _PrimitiveAsset
        {
            public XFORM2 Transform;
            public int AssetRef;            
            public ColorStyle Style;

            public static void GetBounds(ref _BoundsContext bounds, ReadOnlySpan<Byte> command, IReadOnlyList<Object> references)
            {
                var src = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveAsset>(command)[0];

                var asset = references[src.AssetRef];

                if (src.Transform == XFORM2.Identity)
                {
                    var rect = Toolkit.GetAssetBoundingRect(asset);

                    // todo: add all 8 vertices
                    if (rect.HasValue)
                    {
                        bounds.AddVertex(rect.Value.Location.ToVector2(), 0);
                        bounds.AddVertex((rect.Value.Location.ToVector2() + rect.Value.Size.ToVector2()), 0);
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

            public static void DrawTo(IDrawing2D dst, ReadOnlySpan<Byte> command, IReadOnlyList<Object> references, bool collapse)
            {
                var body = System.Runtime.InteropServices.MemoryMarshal.Cast<Byte, _PrimitiveAsset>(command)[0];

                if (collapse)
                {
                    new Transforms.Decompose2D(dst).DrawAsset(body.Transform, references[body.AssetRef], body.Style);
                    return;
                }

                dst.DrawAsset(body.Transform, references[body.AssetRef], body.Style);
            }
        }

        private enum _PrimitiveType
        {
            None,
            Line,
            PolyLine,
            Ellipse,
            Polygon,
            Sprite,
            Asset
        }

        #endregion
    }

    class _CommandStream2D_DebuggerProxy
    {
        public _CommandStream2D_DebuggerProxy(_CommandStream2D src)
        {
            var sb = new List<string>();
            var target = Diagnostics.CommandLogger.From(sb);

            foreach(var offset in src.GetCommands())
            {
                src.DrawTo(offset, target, false);
            }

            Primitives = sb.ToArray();
        }

        public readonly string[] Primitives;
    }

    
}
