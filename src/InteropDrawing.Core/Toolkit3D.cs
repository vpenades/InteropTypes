using System;
using System.Collections.Generic;
using System.Text;

using COLOR = System.Drawing.Color;
using VECTOR2 = System.Numerics.Vector2;
using VECTOR3 = System.Numerics.Vector3;
using VECTOR4 = System.Numerics.Vector4;

using POINT3 = InteropDrawing.Point3;

using XFORM2 = System.Numerics.Matrix3x2;
using XFORM4 = System.Numerics.Matrix4x4;

using BBOX = System.Numerics.Matrix3x2; // Column 0 is Min, Column 1 is Max

namespace InteropDrawing
{
    public static partial class Toolkit
    {
        #region 3D transforms

        public static IDrawing2D CreateTransformed2D(this IDrawing2D t, XFORM2 xform)
        {
            return xform.IsIdentity ? t : Transforms.Drawing2DTransform.Create(t, xform);
        }

        public static IDrawing2D CreateTransformed2D(this IDrawing3D t, XFORM4 xform)
        {
            return Transforms.Drawing3DTransform.Create(t, xform);
        }

        public static IDrawing3D CreateTransformed3D(this IDrawing3D t, XFORM4 xform)
        {
            return xform.IsIdentity ? t : Transforms.Drawing3DTransform.Create(t, xform);
        }

        #endregion

        #region assets

        public static void DrawAsset(this IDrawing3D dc, in XFORM4 transform, Object asset)
        {
            dc.DrawAsset(transform, asset, COLOR.White);
        }

        public static void DrawAssetAsSurfaces(this IDrawing3D dc, in XFORM4 transform, Object asset, ColorStyle brush)
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

            if (asset is IDrawable3D mdl3D)
            {
                mdl3D.DrawTo(dc);
                return;
            }
        }

        public static void DrawAssetAsPrimitives(this IDrawing3D dc, in XFORM4 transform, Object asset, ColorStyle brush)
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

        public static void DrawSurface(this ISurfaceDrawing3D dc, SurfaceStyle brush, params Point3[] points)
        {
            dc.DrawSurface(points, brush);
        }        

        public static void DrawPivot(this IDrawing3D dc, in XFORM4 pivot, Single size)
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

        public static void DrawCube(this ISurfaceDrawing3D dc, in XFORM4 cube, params SurfaceStyle[] style)
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

        public static (VECTOR3 Min, VECTOR3 Max)? GetAssetBoundingMinMax(Object asset)
        {
            if (asset is Model3D model3D) return model3D.BoundingBox;
            if (asset is IDrawable3D drawable)
            {
                var mdl = new Model3D();
                drawable.DrawTo(mdl);
                return mdl.BoundingBox;
            }

            return null;
        }

        public static BBOX? GetAssetBoundingMatrix(Object asset)
        {
            if (asset is Model3D model3D) return model3D.BoundingMatrix;
            if (asset is IDrawable3D drawable)
            {
                var mdl = new Model3D();
                drawable.DrawTo(mdl);
                return mdl.BoundingMatrix;
            }

            return null;
        }

         public static (VECTOR3 Center,Single Radius)? GetAssetBoundingSphere(Object asset)
        {
            if (asset is Model3D model3D) return model3D.BoundingSphere;            

            return null;
        }
        

        public static void DrawCamera(this IDrawing3D dc, XFORM4 pivot, float cameraSize)
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

        public static void DrawProjectedPlane(this IDrawing3D dc, XFORM4 pivot, in XFORM4 projection, Single depth, Single diameter, LineStyle brush)
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

        public static void DrawProjectedPlane(this IDrawing3D dc, XFORM4 pivot, in XFORM4 projection, Single diameter, LineStyle brush)
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

        public static void DrawFont(this IDrawing3D dc, XFORM4 xform, String text, COLOR color)
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
    }
}
