using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;
using VECTOR2 = System.Numerics.Vector2;
using VECTOR3 = System.Numerics.Vector3;
using VECTOR4 = System.Numerics.Vector4;
using XFORM = System.Numerics.Matrix4x4;

using BRECT = System.Drawing.RectangleF;
using BBOX = System.Numerics.Matrix3x2; // Column 0 is Min, Column 1 is Max

namespace InteropDrawing
{
    public static partial class Toolkit
    {
        #region 3D transforms

        public static IDrawing2D CreateTransformed2D(this IDrawing2D t, System.Numerics.Matrix3x2 xform)
        {
            if (xform == System.Numerics.Matrix3x2.Identity) return t;

            return Transforms.Drawing2DTransform.Create(t, xform);
        }

        public static IDrawing2D CreateTransformed2D(this IDrawing3D t, XFORM xform)
        {
            return Transforms.Drawing3DTransform.Create(t, xform);
        }

        public static IDrawing3D CreateTransformed3D(this IDrawing3D t, XFORM xform)
        {
            return Transforms.Drawing3DTransform.Create(t, xform);
        }

        #endregion

        #region assets

        public static void DrawAsset(this IDrawing3D dc, in XFORM transform, Object asset)
        {
            dc.DrawAsset(transform, asset, COLOR.White);
        }

        public static void DrawAssetAsSurfaces(this IDrawing3D dc, in XFORM transform, Object asset, ColorStyle brush)
        {
            // TODO: if dc is IAssetDrawing, call directly

            dc = dc.CreateTransformed3D(transform);
            dc.DrawAssetAsSurfaces(asset, brush);
        }

        public static void DrawAssetAsSurfaces(this IDrawing3D dc, Object asset, ColorStyle brush)
        {
            if (asset is Asset3D a3d)
            {
                a3d._DrawAsSurfaces(dc);
                return;
            }

            if (asset is Model3D mdl3D)
            {
                mdl3D.DrawTo(dc);
                return;
            }
        }

        public static void DrawAssetAsPrimitives(this IDrawing3D dc, in XFORM transform, Object asset, ColorStyle brush)
        {
            // TODO: if dc is IAssetDrawing, call directly

            dc = dc.CreateTransformed3D(transform);
            dc.DrawAssetAsPrimitives(asset, brush);
        }

        public static void DrawAssetAsPrimitives(this IDrawing3D dc, Object asset, ColorStyle brush)
        {
            if (asset is Asset3D a3d)
            {
                a3d._DrawAsSurfaces(dc);
                return;
            }

            if (asset is Model3D mdl3D)
            {
                mdl3D.DrawTo(dc);
                return;
            }
        }

        #endregion

        #region drawing        

        public static void DrawSurface(this IDrawing3D dc, SurfaceStyle brush, params Point3[] points)
        {
            dc.DrawSurface(points, brush);
        }        

        public static void DrawPivot(this IDrawing3D dc, in XFORM pivot, Single size)
        {
            var center = pivot.Translation;

            var xxx = VECTOR3.TransformNormal(VECTOR3.UnitX, pivot).Normalized();
            var yyy = VECTOR3.TransformNormal(VECTOR3.UnitY, pivot).Normalized();
            var zzz = VECTOR3.TransformNormal(VECTOR3.UnitZ, pivot).Normalized();

            var colorX = COLOR.Red;
            var colorY = COLOR.Green;
            var colorZ = COLOR.Blue;

            dc.DrawSegment(center + xxx * size * 0.1f, center + xxx * size, size * 0.1f, (colorX, LineCapStyle.Round, LineCapStyle.Triangle));
            dc.DrawSegment(center + yyy * size * 0.1f, center + yyy * size, size * 0.1f, (colorY, LineCapStyle.Round, LineCapStyle.Triangle));
            dc.DrawSegment(center + zzz * size * 0.1f, center + zzz * size, size * 0.1f, (colorZ, LineCapStyle.Round, LineCapStyle.Triangle));
        }

        

        public static BBOX? GetAssetBoundingBox(Object asset)
        {
            if (asset is Model3D model3D) return model3D.BoundingMatrix;

            return null;
        }

         public static (VECTOR3 Center,Single Radius)? GetAssetBoundingSphere(Object asset)
        {
            if (asset is Model3D model3D) return model3D.BoundingSphere;            

            return null;
        }
        

        public static void DrawCamera(this IDrawing3D dc, XFORM pivot, float cameraSize)
        {
            if (pivot.GetDeterminant() == 0) return;

            var style = (COLOR.Gray, COLOR.Black, cameraSize * 0.05f);
            var center = VECTOR3.Transform(VECTOR3.Zero, pivot);
            var back = VECTOR3.Transform(VECTOR3.UnitZ * cameraSize, pivot);
            var roll1 = VECTOR3.Transform(new VECTOR3(0, 0.7f, 0.3f) * cameraSize, pivot);
            var roll2 = VECTOR3.Transform(new VECTOR3(0, 0.7f, 0.9f) * cameraSize, pivot);

            dc.DrawSphere(center, cameraSize * 0.35f, COLOR.Black); // lens

            dc.DrawSegment(VECTOR3.Lerp(back, center, 0.7f), center, cameraSize * 0.5f, style); // objective
            dc.DrawSegment(back, VECTOR3.Lerp(back, center, 0.7f), cameraSize * 0.8f, style); // body
            dc.DrawSphere(roll1, 0.45f * cameraSize, style);
            dc.DrawSphere(roll2, 0.45f * cameraSize, style);


            var xxx = VECTOR3.TransformNormal(VECTOR3.UnitX, pivot).Normalized() * cameraSize;
            var yyy = VECTOR3.TransformNormal(VECTOR3.UnitY, pivot).Normalized() * cameraSize;
            var zzz = VECTOR3.TransformNormal(VECTOR3.UnitZ, pivot).Normalized() * cameraSize;

            var colorX = COLOR.Red;
            var colorY = COLOR.Green;
            var colorZ = COLOR.Blue;

            dc.DrawSegment(center + xxx * 0.5f, center + xxx * 1.5f, cameraSize * 0.1f, (colorX, LineCapStyle.Round, LineCapStyle.Triangle));
            dc.DrawSegment(center + yyy * 1.0f, center + yyy * 2.0f, cameraSize * 0.1f, (colorY, LineCapStyle.Round, LineCapStyle.Triangle));
            dc.DrawSegment(center + zzz * 1.2f, center + zzz * 2.2f, cameraSize * 0.1f, (colorZ, LineCapStyle.Round, LineCapStyle.Triangle));
        }

        public static void DrawProjectedPlane(this IDrawing3D dc, XFORM pivot, in XFORM projection, Single depth, Single diameter, LineStyle brush)
        {
            XFORM.Invert(projection, out XFORM ip);

            var a = new VECTOR3(-1, -1, 1) * depth;
            var b = new VECTOR3(+1, -1, 1) * depth;
            var c = new VECTOR3(+1, +1, 1) * depth;
            var d = new VECTOR3(-1, +1, 1) * depth;

            var aa = VECTOR4.Transform(a, ip);
            var bb = VECTOR4.Transform(b, ip);
            var cc = VECTOR4.Transform(c, ip);
            var dd = VECTOR4.Transform(d, ip);

            a = aa.SelectXYZ() / aa.W;
            b = bb.SelectXYZ() / bb.W;
            c = cc.SelectXYZ() / cc.W;
            d = dd.SelectXYZ() / dd.W;

            a = VECTOR3.Transform(a, pivot);
            b = VECTOR3.Transform(b, pivot);
            c = VECTOR3.Transform(c, pivot);
            d = VECTOR3.Transform(d, pivot);

            dc.DrawSegment(a, b, diameter, brush);
            dc.DrawSegment(b, c, diameter, brush);
            dc.DrawSegment(c, d, diameter, brush);
            dc.DrawSegment(d, a, diameter, brush);
        }

        public static void DrawProjectedPlane(this IDrawing3D dc, XFORM pivot, in XFORM projection, Single diameter, LineStyle brush)
        {
            XFORM.Invert(projection, out XFORM ip);

            var a = new VECTOR3(-1, -1, 0);
            var b = new VECTOR3(+1, -1, 0);
            var c = new VECTOR3(+1, +1, 0);
            var d = new VECTOR3(-1, +1, 0);

            // to camera space

            a = VECTOR3.Transform(a, ip);
            b = VECTOR3.Transform(b, ip);
            c = VECTOR3.Transform(c, ip);
            d = VECTOR3.Transform(d, ip);

            // to world space            

            a = VECTOR3.Transform(a, pivot);
            b = VECTOR3.Transform(b, pivot);
            c = VECTOR3.Transform(c, pivot);
            d = VECTOR3.Transform(d, pivot);

            dc.DrawSegment(a, b, diameter, brush);
            dc.DrawSegment(b, c, diameter, brush);
            dc.DrawSegment(c, d, diameter, brush);
            dc.DrawSegment(d, a, diameter, brush);
        }

        public static void DrawFont(this IDrawing3D dc, XFORM xform, String text, COLOR color)
        {
            Fonts.FontDrawing.DrawFontAsLines(dc, xform, text, color);
        }

        public static void DrawFloor(this IDrawing3D dc, VECTOR2 origin, VECTOR2 size, int divisions, ColorStyle oddStyle, ColorStyle evenStyle)
        {
            var xdivisions = divisions;
            var ydivisions = divisions;

            if (size.X > size.Y) xdivisions *= (int)(size.X / size.Y);
            else ydivisions *= (int)(size.Y / size.X);

            var step = new VECTOR3(size.X, 0, size.Y) / new VECTOR3(xdivisions, 1, ydivisions);
            var init = new VECTOR3(origin.X, 0, origin.Y);

            for (int y = 0; y < ydivisions; ++y)
            {
                for (int x = 0; x < xdivisions; ++x)
                {
                    var a = init + step * new VECTOR3(x, 0, y);
                    var b = init + step * new VECTOR3(x + 1, 0, y);
                    var c = init + step * new VECTOR3(x + 1, 0, y + 1);
                    var d = init + step * new VECTOR3(x, 0, y + 1);

                    var style = ((x & 1) ^ (y & 1)) == 0 ? oddStyle : evenStyle;

                    dc.DrawSurface(style, a, b, c, d);
                }
            }
        }

        #endregion

        #region surface drawing

        public static void DrawCylinderAsSurfaces(this IDrawing3D dc, Point3 a, Single diameterA, Point3 b, Single diameterB, int divisions, LineStyle brush)
        {
            var outr = Math.Abs(brush.Style.OutlineWidth);

            diameterA -= outr;
            diameterB -= outr;

            var aradius = diameterA * 0.5f;
            var bradius = diameterB * 0.5f;

            var av = a.ToNumerics();
            var bv = b.ToNumerics();
            var ab = bv - av;

            // degenerated lines are rendered as "points"
            if (ab.Length() <= Math.Max(aradius, bradius) * 0.1f)
            {
                dc.DrawSphereAsSurfaces((av + bv) * 0.5f, Math.Max(aradius, bradius), 0, brush.Style);
                return;
            }

            ab = VECTOR3.Normalize(ab);

            if (brush.Style.HasOutline)
            {
                var aa = a.ToNumerics();
                var bb = b.ToNumerics();

                if (brush.StartCap != LineCapStyle.Flat) aa -= ab * outr * 0.5f;
                if (brush.EndCap != LineCapStyle.Flat) bb += ab * outr * 0.5f;

                _DrawCylinderInternal(dc, aa, aradius + outr, bb, bradius + outr, divisions, brush.Style.OutlineColor, true, brush.StartCap, brush.EndCap);
            }

            if (brush.Style.HasFill)
            {
                _DrawCylinderInternal(dc, av, aradius, bv, bradius, divisions, brush.Style.FillColor, false, brush.StartCap, brush.EndCap);
            }
        }

        public static void DrawSphereAsSurfaces(this IDrawing3D dc, Point3 center, Single diameter, int lod, ColorStyle brush)
        {
            lod = lod.Clamp(1, 5); // more than 5 lods will create way too many polygons

            var radius = diameter * 0.5f;

            if (brush.HasOutline)
            {
                var r = radius + brush.OutlineWidth * 0.5f;
                if (r > 0) Parametric.PlatonicFactory.DrawOctahedron(dc, center.ToNumerics(), r, lod, brush.OutlineColor, true);
            }

            if (brush.HasFill)
            {
                var r = radius - brush.OutlineWidth * 0.5f;
                if (r > 0) Parametric.PlatonicFactory.DrawOctahedron(dc, center.ToNumerics(), r, lod, brush.FillColor, false);
            }
        }

        public static void DrawOutlineAsSegments(this IDrawing3D dc, ReadOnlySpan<Point3> vertices, Single diameter, COLOR color)
        {
            var c = new LineStyle(color);

            for (int i = 1; i < vertices.Length; ++i)
            {
                dc.DrawSegment(vertices[i - 1], vertices[i], diameter, c);
            }

            dc.DrawSegment(vertices[vertices.Length - 1], vertices[0], diameter, c);
        }

        #endregion

        #region internals

        private static void _DrawCylinderInternal(this IDrawing3D dc, VECTOR3 a, Single aradius, VECTOR3 b, Single bradius, int divisions, COLOR color, bool flipFaces, LineCapStyle startCap, LineCapStyle endCap)
        {
            if (aradius < 0.0001f && bradius < 0.0001f)
            {
                aradius = bradius = 0.0001f;
                divisions = 3;

                // todo: draw a segment with 6 triangles
            }

            aradius = _AdjustNGonRadius(aradius, divisions);
            bradius = _AdjustNGonRadius(bradius, divisions);

            var nz = VECTOR3.Normalize(b - a);
            var nx = VECTOR3.Normalize(nz.PerpendicularAxis());
            var ny = VECTOR3.Normalize(VECTOR3.Cross(nz, nx));

            Span<Point3> aa = stackalloc Point3[divisions];
            Span<Point3> bb = stackalloc Point3[divisions];

            var brush = new SurfaceStyle(color, false);

            for (int i = 0; i < divisions; ++i)
            {
                var angle = -MathF.PI * 2 * i / divisions;
                var p = (nx * MathF.Cos(angle) + ny * MathF.Sin(angle));
                aa[i] = a + p * aradius;
                bb[i] = b + p * bradius;
            }

            for (int i = 0; i < aa.Length; ++i)
            {
                var j = (i + 1) % aa.Length;

                if (flipFaces) dc.DrawSurface(brush, aa[i], aa[j], bb[j], bb[i]);
                else dc.DrawSurface(brush, aa[j], aa[i], bb[i], bb[j]);
            }

            if (flipFaces)
            {
                for (int i = 0; i < aa.Length / 2; ++i)
                {
                    var j = aa.Length - 1 - i;
                    var k = aa[i];
                    aa[i] = aa[j];
                    aa[j] = k;
                }
            }

            _DrawCylinderCap(dc, brush, startCap, a, -nz * aradius, aa);

            if (!flipFaces)
            {
                for (int i = 0; i < bb.Length / 2; ++i)
                {
                    var j = bb.Length - 1 - i;
                    var k = bb[i];
                    bb[i] = bb[j];
                    bb[j] = k;
                }
            }

            _DrawCylinderCap(dc, brush, endCap, b, nz * bradius, bb);
        }

        /// <summary>
        /// Adjusts the radius of a circle so a n-Gon will have the same cross area.
        /// </summary>
        /// <param name="circleRadius">The target circle radius</param>
        /// <param name="divisions">The number of corners of the n-Gon</param>
        /// <returns>the radius of the n-Gon corner</returns>
        internal static float _AdjustNGonRadius(float circleRadius, int divisions)
        {
            System.Diagnostics.Debug.Assert(circleRadius > 0, nameof(circleRadius));
            System.Diagnostics.Debug.Assert(divisions >= 3, nameof(divisions));

            // taking 2. Given the radius (circumradius) from here: https://www.mathopenref.com/polygonregulararea.html            
            // and resolving 'r' gives this formula:

            var circleArea = MathF.PI * circleRadius * circleRadius;

            var k = MathF.Sin(MathF.PI * 2.0f / divisions);

            return MathF.Sqrt(circleArea * 2 / divisions * k);
        }

        private static void _DrawCylinderCap(IDrawing3D dc, SurfaceStyle brush, LineCapStyle cap, VECTOR3 center, VECTOR3 axis, Span<Point3> corners)
        {
            switch (cap)
            {
                case LineCapStyle.Round:
                    for (int i = 0; i < corners.Length; ++i)
                    {
                        var j = (i + 1) % corners.Length;

                        var i0 = corners[i].ToNumerics();
                        var j0 = corners[j].ToNumerics();
                        var i1 = VECTOR3.Lerp(center, i0 + axis, 0.7f);
                        var j1 = VECTOR3.Lerp(center, j0 + axis, 0.7f);

                        dc.DrawSurface(brush, i0, j0, j1, i1);
                        dc.DrawSurface(brush, center + axis, i1, j1);
                    }
                    break;

                case LineCapStyle.Triangle:
                    for (int i = 0; i < corners.Length; ++i)
                    {
                        var j = (i + 1) % corners.Length;
                        dc.DrawSurface(brush, center + axis, corners[i], corners[j]);
                    }
                    break;


                default: dc.DrawSurface(corners, brush); break;
            }
        }

        #endregion
    }


}
