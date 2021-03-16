using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps.Drawing
{
    struct _Triangle
    {
        public static _Triangle Create((float x, float y) a, (float x, float y) b, (float x, float y) c)
        {
            return new _Triangle()
            {
                A = new InteropDrawing.Point2(a.x, a.y),
                B = new InteropDrawing.Point2(b.x, b.y),
                C = new InteropDrawing.Point2(c.x, c.y)
            };
        }

        public InteropDrawing.Point2 A;
        public InteropDrawing.Point2 B;
        public InteropDrawing.Point2 C;        

        // Triangle rasterization rules
        // https://msdn.microsoft.com/en-us/library/windows/desktop/cc627092(v=vs.85).aspx
        public static IEnumerable<_Triangle> GetFillRuleTriangles()
        {
            // big top left triangle
            yield return Create((1, 1), (6, 2), (2, 4));
            // point
            yield return Create((0.45f, 0.5f), (4.5f, 0.5f), (4.5f, 0.45f));
            // displaced small one
            yield return Create((6.25f, 0.25f), (6.25f, 1.25f), (5.25f, 1.25f));
            // displaced small one 2
            yield return Create((7.5f, 0.5f), (7.5f, 1.5f), (6.5f, 1.5f));

            // bottom triangle
            yield return Create((9.5f, 7.5f), (10.5f, 7.5f), (9.5f, 9.5f));

            // big bottom left
            yield return Create((1, 6), (7, 4), (5, 6));
            yield return Create((7, 4), (5, 6), (8, 7));
            yield return Create((7, 4), (8, 7), (9.5f, 5.5f));

            // mid triangle DOWN
            yield return Create((9.5f, 5.25f), (7.75f, 2.5f), (11.75f, 2.5f));
            // mid triangle UP
            yield return Create((9.75f, 0.75f), (7.75f, 2.5f), (11.75f, 2.5f));


            // small triangle island
            yield return Create((11.5f, 4.5f), (11.5f, 6.5f), (12.5f, 5.5f));
            // small triangle island at bottom
            yield return Create((9.5f, 7.5f), (10.5f, 7.5f), (9.5f, 9.5f));

            // top right triangle 1
            yield return Create((15f, 0f), (14.5f, 2.5f), (13.5f, 1.5f));
            yield return Create((13.5f, 1.5f), (14.5f, 2.5f), (14.5f, 4.5f));

            // bottom right square
            yield return Create((13.5f, 5.5f), (15.5f, 5.5f), (13.5f, 7.5f));
            yield return Create((13.5f, 7.5f), (15.5f, 5.5f), (15.5f, 7.5f));

        }

        private static byte[] _ExpectedRuleTestBitmap = new byte[]
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,
            0,2,2,0,0,0,0,0,0,1,1,0,0,1,1,0,
            0,2,2,2,2,0,0,0,2,2,2,2,0,0,0,0,
            0,0,2,0,0,0,0,0,2,2,2,0,0,0,0,0,
            0,0,0,0,0,2,1,2,0,1,0,0,0,0,0,0,
            0,0,1,1,1,2,2,1,1,0,0,1,0,2,2,0,
            0,0,0,0,0,0,2,2,0,0,0,0,0,2,1,0,
            0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,
        };
    }
}
