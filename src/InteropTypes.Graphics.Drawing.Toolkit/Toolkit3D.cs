using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;
using VECTOR2 = System.Numerics.Vector2;
using VECTOR3 = System.Numerics.Vector3;
using VECTOR4 = System.Numerics.Vector4;

using POINT3 = InteropTypes.Graphics.Drawing.Point3;

using XFORM2 = System.Numerics.Matrix3x2;
using XFORM4 = System.Numerics.Matrix4x4;

using BBOX = System.Numerics.Matrix3x2; // Column 0 is Min, Column 1 is Max

namespace InteropTypes.Graphics.Drawing
{
    public static partial class Toolkit
    {
        #region data

        private static readonly System.Threading.ThreadLocal<Record3D> _Model3DBounds = new System.Threading.ThreadLocal<Record3D>(() => new Record3D());

        #endregion

        #region 3D transforms       

        public static ICanvas2D CreateTransformed2D(this ICanvas2D source, XFORM2 xform)
        {
            return xform.IsIdentity ? source : Transforms.Canvas2DTransform.Create(source, xform);
        }

        public static ICoreCanvas2D CreateTransformed2D(this ICoreCanvas2D source, XFORM2 xform)
        {
            return xform.IsIdentity ? source : Transforms.Canvas2DTransform.Create(source, xform);
        }

        public static ICanvas2D CreateTransformed2D(this IScene3D t, XFORM4 xform)
        {
            return Transforms.Scene3DTransform.Create(t, xform);
        }

        public static IScene3D CreateTransformed3D(this IScene3D t, XFORM4 xform)
        {
            return xform.IsIdentity ? t : Transforms.Scene3DTransform.Create(t, xform);
        }

        #endregion

        #region Linq

        public static IDrawingBrush<IScene3D> Combine(params IDrawingBrush<IScene3D>[] drawables)
        {
            return new _Combined<IScene3D>(drawables);
        }

        public static IDrawingBrush<IScene3D> Combine(this IEnumerable<IDrawingBrush<IScene3D>> drawables)
        {
            return new _Combined<IScene3D>(drawables);
        }        

        private readonly struct _Combined<T> : IDrawingBrush<T>
        {
            public _Combined(IEnumerable<IDrawingBrush<T>> elements) { _Elements = elements; }

            private readonly IEnumerable<IDrawingBrush<T>> _Elements;

            public void DrawTo(T context)
            {
                foreach (var element in _Elements) element.DrawTo(context);
            }
        }

        public static IDrawingBrush<T> Tinted<T>(this IDrawingBrush<T> drawable, ColorStyle color)
        {
            return new _Tinted<T>(drawable, color);
        }

        private readonly struct _Tinted<T> : IDrawingBrush<T>
        {
            public _Tinted(IDrawingBrush<T> d, ColorStyle c)
            {
                _Drawable = d;
                _TintColor = c;
            }

            private readonly IDrawingBrush<T> _Drawable;
            private readonly ColorStyle _TintColor;

            public void DrawTo(T context)
            {
                _TintColor.TrySetDefaultTo(context);
                _Drawable.DrawTo(context);
            }
        }

        #endregion

        #region assets

        public static void DrawAsset(this IScene3D dc, in XFORM4 transform, Object asset) { dc.DrawAsset(transform, asset); }

        public static void DrawAssetAsSurfaces(this IScene3D dc, in XFORM4 transform, Object asset)
        {
            // TODO: if dc is IAssetDrawing, call directly

            dc = dc.CreateTransformed3D(transform);
            dc.DrawAssetAsSurfaces(asset);
        }

        public static void DrawAssetAsSurfaces(this IScene3D dc, Object asset)
        {
            if (asset is Asset3D a3d)
            {
                a3d._DrawAsSurfaces(dc);
                return;
            }

            if (asset is IDrawingBrush<ICoreScene3D> mdl1)
            {
                mdl1.DrawTo(dc);
                return;
            }

            if (asset is IDrawingBrush<IScene3D> mdl2)
            {
                mdl2.DrawTo(dc);
                return;
            }
        }

        public static void DrawAssetAsPrimitives(this IScene3D dc, in XFORM4 transform, Object asset)
        {
            // TODO: if dc is IAssetDrawing, call directly

            dc = dc.CreateTransformed3D(transform);
            dc.DrawAssetAsPrimitives(asset);
        }

        public static void DrawAssetAsPrimitives(this IScene3D dc, Object asset)
        {
            if (asset is Asset3D a3d)
            {
                a3d._DrawAsSurfaces(dc);
                return;
            }

            if (asset is Record3D mdl3D)
            {
                mdl3D.DrawTo(dc);
                return;
            }
        }

        #endregion

        #region drawing        

        public static (VECTOR3 Min, VECTOR3 Max)? GetAssetBoundingMinMax(Object asset)
        {
            if (asset is Record3D model3D) return model3D.BoundingBox;
            if (asset is IDrawingBrush<IScene3D> drawable)
            {
                var mdl = _Model3DBounds.Value;
                mdl.Clear();

                drawable.DrawTo(mdl);
                return mdl.BoundingBox;
            }

            return null;
        }

        public static BBOX? GetAssetBoundingMatrix(Object asset)
        {
            if (asset is Record3D model3D) return model3D.BoundingMatrix;
            if (asset is IDrawingBrush<IScene3D> drawable)
            {
                var mdl = _Model3DBounds.Value;
                mdl.Clear();

                drawable.DrawTo(mdl);
                return mdl.BoundingMatrix;
            }

            return null;
        }        

        public static void DrawSegment(this IScene3D dc, POINT3 a, POINT3 b, float diameter, LineStyle style)
        {
            Span<POINT3> points = stackalloc POINT3[2];
            points[0] = a;
            points[1] = b;

            dc.DrawSegments(points, diameter, style);
        }

        public static void DrawSurface(this IScene3D dc, SurfaceStyle brush, params Point3[] points)
        {
            dc.DrawSurface(points, brush);
        }

        public static void DrawPivot(this IScene3D dc, Point3 origin, Single size)
        {
            var xform = XFORM4.CreateTranslation(origin.XYZ);
            DrawPivot(dc, xform, size);
        }

        public static void DrawPivot(this IScene3D dc, in XFORM4 pivot, Single size)
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

        public static void DrawCube(this IScene3D dc, in XFORM4 cube, params SurfaceStyle[] style)
        {
            Span<POINT3> corners = stackalloc POINT3[8];

            var scaleX = new POINT3(cube.M11, cube.M12, cube.M13);
            var scaleY = new POINT3(cube.M21, cube.M22, cube.M23);
            var scaleZ = new POINT3(cube.M31, cube.M32, cube.M33);
            POINT3 origin = cube.Translation;

            corners[0] = origin;
            corners[1] = origin + scaleX;
            corners[2] = origin + scaleX + scaleY;
            corners[3] = origin + scaleY;

            corners[4] = corners[0] + scaleZ;
            corners[5] = corners[1] + scaleZ;
            corners[6] = corners[2] + scaleZ;
            corners[7] = corners[3] + scaleZ;

            Span<POINT3> vertices = stackalloc POINT3[4];

            int idx = 0;

            vertices[0] = corners[3];
            vertices[1] = corners[0];
            vertices[2] = corners[0 + 4];
            vertices[3] = corners[3 + 4];
            dc.DrawSurface(vertices, style[idx]);            

            ++idx; idx %= style.Length;

            vertices[0] = corners[0];
            vertices[1] = corners[1];
            vertices[2] = corners[1 + 4];
            vertices[3] = corners[0 + 4];
            dc.DrawSurface(vertices, style[idx]);

            ++idx; idx %= style.Length;

            vertices[0] = corners[3];
            vertices[1] = corners[2];
            vertices[2] = corners[1];
            vertices[3] = corners[0];
            dc.DrawSurface(vertices, style[idx]);             
             
            ++idx; idx %= style.Length;

            vertices[0] = corners[1];
            vertices[1] = corners[2];
            vertices[2] = corners[2 + 4];
            vertices[3] = corners[1 + 4];
            dc.DrawSurface(vertices, style[idx]);

            ++idx; idx %= style.Length;

            vertices[0] = corners[2];
            vertices[1] = corners[3];
            vertices[2] = corners[3 + 4];
            vertices[3] = corners[2 + 4];
            dc.DrawSurface(vertices, style[idx]);

            ++idx; idx %= style.Length;

            vertices[0] = corners[0 + 4];
            vertices[1] = corners[1 + 4];
            vertices[2] = corners[2 + 4];
            vertices[3] = corners[3 + 4];
            dc.DrawSurface(vertices, style[idx]);
        }        

        public static void DrawCamera(this IScene3D dc, XFORM4 pivot, float cameraSize)
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

        public static void DrawProjectedPlane(this IScene3D dc, XFORM4 pivot, in XFORM4 projection, Single depth, Single diameter, LineStyle brush)
        {
            XFORM4.Invert(projection, out XFORM4 ip);

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

        public static void DrawProjectedPlane(this IScene3D dc, XFORM4 pivot, in XFORM4 projection, Single diameter, LineStyle brush)
        {
            XFORM4.Invert(projection, out XFORM4 ip);

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

        public static void DrawFont(this IScene3D dc, XFORM4 xform, String text, COLOR color)
        {
            Fonts.FontDrawing.DrawFontAsLines(dc, xform, text, color);
        }

        public static void DrawFloorXZ(this IScene3D dc, POINT3 origin, Point2 size, int divisions, OutlineFillStyle oddStyle, OutlineFillStyle evenStyle)
        {
            _DrawFloor(dc, origin, size, divisions, oddStyle, evenStyle, (x, z) => new VECTOR3(x, 0, z));
        }

        public static void DrawFloorXY(this IScene3D dc, POINT3 origin, Point2 size, int divisions, OutlineFillStyle oddStyle, OutlineFillStyle evenStyle)
        {
            _DrawFloor(dc, origin, size, divisions, oddStyle, evenStyle, (x, y) => new VECTOR3(x, y, 0));
        }

        private static void _DrawFloor(IScene3D dc, POINT3 origin, Point2 size, int divisions, OutlineFillStyle oddStyle, OutlineFillStyle evenStyle, Func<float, float, VECTOR3> axisFunc)
        {
            var xdivisions = divisions;
            var ydivisions = divisions;

            if (size.X > size.Y) xdivisions *= (int)(size.X / size.Y);
            else ydivisions *= (int)(size.Y / size.X);

            var step = axisFunc(size.X / xdivisions, size.Y / ydivisions);
            var init = origin;

            for (int y = 0; y < ydivisions; ++y)
            {
                for (int x = 0; x < xdivisions; ++x)
                {
                    var a = init + step * axisFunc(x, y);
                    var b = init + step * axisFunc(x + 1, y);
                    var c = init + step * axisFunc(x + 1, y + 1);
                    var d = init + step * axisFunc(x, y + 1);

                    var style = ((x & 1) ^ (y & 1)) == 0 ? oddStyle : evenStyle;

                    dc.DrawSurface((style,true), a, b, c, d);
                }
            }
        }

        #endregion
    }
}
