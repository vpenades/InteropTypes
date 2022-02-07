using System;

using XYZ = System.Numerics.Vector3;

namespace InteropTypes.Graphics.Drawing.Parametric
{
    partial class ShapeFactory3D
    {
        public struct PlatonicFactory
        {
            // http://blog.andreaskahler.com/2009/06/creating-icosphere-mesh-in-code.html

            #region Tetrahedron constants

            private static readonly XYZ _TETRAVRT0 = new XYZ(1, 1, 1);
            private static readonly XYZ _TETRAVRT1 = new XYZ(1, -1, -1);
            private static readonly XYZ _TETRAVRT2 = new XYZ(-1, 1, -1);
            private static readonly XYZ _TETRAVRT3 = new XYZ(-1, -1, 1);

            #endregion

            #region Icosahedron constants

            private const float _ICOFACTOR = 2.1180339887498949f;

            private static readonly XYZ _ICOVRT0 = new XYZ(-1, _ICOFACTOR, 0);
            private static readonly XYZ _ICOVRT1 = new XYZ(1, _ICOFACTOR, 0);
            private static readonly XYZ _ICOVRT2 = new XYZ(-1, -_ICOFACTOR, 0);
            private static readonly XYZ _ICOVRT3 = new XYZ(1, -_ICOFACTOR, 0);

            private static readonly XYZ _ICOVRT4 = new XYZ(0, -1, _ICOFACTOR);
            private static readonly XYZ _ICOVRT5 = new XYZ(0, 1, _ICOFACTOR);
            private static readonly XYZ _ICOVRT6 = new XYZ(0, -1, -_ICOFACTOR);
            private static readonly XYZ _ICOVRT7 = new XYZ(0, 1, -_ICOFACTOR);

            private static readonly XYZ _ICOVRT8 = new XYZ(_ICOFACTOR, 0, -1);
            private static readonly XYZ _ICOVRT9 = new XYZ(_ICOFACTOR, 0, 1);
            private static readonly XYZ _ICOVRT10 = new XYZ(-_ICOFACTOR, 0, -1);
            private static readonly XYZ _ICOVRT11 = new XYZ(-_ICOFACTOR, 0, 1);

            #endregion

            #region data

            private XYZ _Center;
            private float _Radius;
            private ColorStyle _Color;
            private bool _FaceFlip;

            private IPrimitiveScene3D _Context;

            #endregion

            #region API

            // https://en.wikipedia.org/wiki/Platonic_solid

            public static void DrawTetrahedron(IPrimitiveScene3D dc, XYZ center, float radius, int lod, ColorStyle color, bool faceFlip)
            {
                if (lod < 0) return;

                var ctx = new PlatonicFactory
                {
                    _Center = center,
                    _Radius = radius,
                    _Color = color,
                    _FaceFlip = faceFlip,
                    _Context = dc
                };

                ctx.DrawTriangle(lod, _TETRAVRT0, _TETRAVRT1, _TETRAVRT2);
                ctx.DrawTriangle(lod, _TETRAVRT3, _TETRAVRT0, _TETRAVRT2);
                ctx.DrawTriangle(lod, _TETRAVRT3, _TETRAVRT1, _TETRAVRT0);
                ctx.DrawTriangle(lod, _TETRAVRT3, _TETRAVRT2, _TETRAVRT1);
            }

            public static void DrawOctahedron(IPrimitiveScene3D dc, XYZ center, float radius, int lod, ColorStyle color, bool faceFlip)
            {
                if (lod < 0) return;

                var ctx = new PlatonicFactory
                {
                    _Center = center,
                    _Radius = radius,
                    _Color = color,
                    _FaceFlip = faceFlip,
                    _Context = dc
                };

                // top 4
                ctx.DrawTriangle(lod, XYZ.UnitX, XYZ.UnitY, XYZ.UnitZ);
                ctx.DrawTriangle(lod, XYZ.UnitZ, XYZ.UnitY, -XYZ.UnitX);
                ctx.DrawTriangle(lod, -XYZ.UnitX, XYZ.UnitY, -XYZ.UnitZ);
                ctx.DrawTriangle(lod, -XYZ.UnitZ, XYZ.UnitY, XYZ.UnitX);

                // bottom 4
                ctx._FaceFlip = !ctx._FaceFlip;
                ctx.DrawTriangle(lod, XYZ.UnitX, -XYZ.UnitY, XYZ.UnitZ);
                ctx.DrawTriangle(lod, XYZ.UnitZ, -XYZ.UnitY, -XYZ.UnitX);
                ctx.DrawTriangle(lod, -XYZ.UnitX, -XYZ.UnitY, -XYZ.UnitZ);
                ctx.DrawTriangle(lod, -XYZ.UnitZ, -XYZ.UnitY, XYZ.UnitX);
            }

            public static void DrawIcosahedron(IPrimitiveScene3D dc, XYZ center, float radius, int lod, ColorStyle color, bool faceFlip)
            {
                if (lod < 0) return;

                var ctx = new PlatonicFactory
                {
                    _Center = center,
                    _Radius = radius,
                    _Color = color,
                    _FaceFlip = faceFlip,
                    _Context = dc
                };

                // 5 faces around point 0
                ctx.DrawTriangle(lod, _ICOVRT0, _ICOVRT11, _ICOVRT5);
                ctx.DrawTriangle(lod, _ICOVRT0, _ICOVRT5, _ICOVRT1);
                ctx.DrawTriangle(lod, _ICOVRT0, _ICOVRT1, _ICOVRT7);
                ctx.DrawTriangle(lod, _ICOVRT0, _ICOVRT7, _ICOVRT10);
                ctx.DrawTriangle(lod, _ICOVRT0, _ICOVRT10, _ICOVRT11);

                // 5 adjacent faces
                ctx.DrawTriangle(lod, _ICOVRT1, _ICOVRT5, _ICOVRT9);
                ctx.DrawTriangle(lod, _ICOVRT5, _ICOVRT11, _ICOVRT4);
                ctx.DrawTriangle(lod, _ICOVRT11, _ICOVRT10, _ICOVRT2);
                ctx.DrawTriangle(lod, _ICOVRT10, _ICOVRT7, _ICOVRT6);
                ctx.DrawTriangle(lod, _ICOVRT7, _ICOVRT1, _ICOVRT8);

                // 5 faces around point 3
                ctx.DrawTriangle(lod, _ICOVRT3, _ICOVRT9, _ICOVRT4);
                ctx.DrawTriangle(lod, _ICOVRT3, _ICOVRT4, _ICOVRT2);
                ctx.DrawTriangle(lod, _ICOVRT3, _ICOVRT2, _ICOVRT6);
                ctx.DrawTriangle(lod, _ICOVRT3, _ICOVRT6, _ICOVRT8);
                ctx.DrawTriangle(lod, _ICOVRT3, _ICOVRT8, _ICOVRT9);

                // 5 adjacent faces
                ctx.DrawTriangle(lod, _ICOVRT4, _ICOVRT9, _ICOVRT5);
                ctx.DrawTriangle(lod, _ICOVRT2, _ICOVRT4, _ICOVRT11);
                ctx.DrawTriangle(lod, _ICOVRT6, _ICOVRT2, _ICOVRT10);
                ctx.DrawTriangle(lod, _ICOVRT8, _ICOVRT6, _ICOVRT7);
                ctx.DrawTriangle(lod, _ICOVRT9, _ICOVRT8, _ICOVRT1);
            }

            private void DrawTriangle(int lod, XYZ a, XYZ b, XYZ c)
            {
                a = XYZ.Normalize(a);
                b = XYZ.Normalize(b);
                c = XYZ.Normalize(c);

                if (lod <= 0)
                {
                    Span<Point3> abc = stackalloc Point3[3];

                    if (_FaceFlip)
                    {
                        abc[2] = _Center + a * _Radius;
                        abc[1] = _Center + b * _Radius;
                        abc[0] = _Center + c * _Radius;
                    }
                    else
                    {
                        abc[0] = _Center + a * _Radius;
                        abc[1] = _Center + b * _Radius;
                        abc[2] = _Center + c * _Radius;
                    }

                    _Context.DrawConvexSurface(abc, _Color);

                    return;
                }

                --lod;

                var ab = (a + b) * 0.5f;
                var bc = (b + c) * 0.5f;
                var ca = (c + a) * 0.5f;

                DrawTriangle(lod, a, ab, ca);
                DrawTriangle(lod, ab, b, bc);
                DrawTriangle(lod, bc, c, ca);
                DrawTriangle(lod, ab, bc, ca);
            }

            #endregion
        }
    }
}
