using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Drawing.Parametric
{

    /// <summary>
    /// Utility used to clip a polygon against a plane.
    /// </summary>    
    ref struct PolygonClipper3
    {
        /// <summary>
        /// Clips a polygon against a plane
        /// </summary>
        /// <param name="dstVertices">
        /// The buffer where clipped vertices are copied to.
        /// - For triangles and convex polygons, <paramref name="dstVertices"/> must be at least <paramref name="srcVertices"/>.Length + 1.
        /// - For arbitrary polygons, <paramref name="dstVertices"/> must be at least <paramref name="srcVertices"/>.Length x 2.
        /// The current algorythm does not support well polygons with holes, or with intersecting lines. For these cases
        /// it's probably better to decompose the source polygons into flat triangles.
        /// </param>
        /// <param name="srcVertices"></param>
        /// <param name="plane"></param>
        /// <returns>the final number of vertices</returns>
        public static int ClipPolygonToPlane(Span<Vector3> dstVertices, ReadOnlySpan<Vector3> srcVertices, in Plane plane)
        {
            var accum = new PolygonClipper3(dstVertices);

            var lastIdx = srcVertices.Length - 1;

            for (int i = 0; i < srcVertices.Length; ++i)
            {
                int j = i == lastIdx ? 0 : i + 1;

                var a = srcVertices[i];
                var b = srcVertices[j];

                if (a == b) continue;

                if (!ClipLineToPlane(ref a, ref b, plane)) continue;

                accum.AddSegment(a, b);
            }

            return accum._Count;
        }

        public static bool ClipLineToPlane(ref Vector3 a, ref Vector3 b, in Plane plane)
        {
            var aw = Plane.DotCoordinate(plane, a);
            var bw = Plane.DotCoordinate(plane, b);
            if (aw < 0 && bw < 0) return false;

            if (aw < 0)
            {
                var u = bw - aw;
                u = bw / u; // u values close or equal to 0 can result in values with large errors or infinity
                if (u < 1) a = Vector3.Lerp(b, a, u);// so we only clamp when u is within bounds                    

                System.Diagnostics.Debug.Assert(Plane.DotCoordinate(plane, a) >= -0.0001f);
            }
            else if (bw < 0)
            {
                var u = aw - bw;
                u = aw / u; // u values close or equal to 0 can result in values with large errors or infinity
                if (u < 1) b = Vector3.Lerp(a, b, u); // so we only clamp when u is within bounds

                System.Diagnostics.Debug.Assert(Plane.DotCoordinate(plane, b) >= -0.0001f);
            }

            if (a == b) return false;

            return true;
        }

        private PolygonClipper3(Span<Vector3> vertices)
        {
            _Vertices = vertices;
            _Count = 0;
        }

        private readonly Span<Vector3> _Vertices;
        private int _Count;

        private void AddSegment(Vector3 a, Vector3 b)
        {
            if (a == b) return;

            if (_Count == 0)
            {
                _Vertices[_Count++] = a;
                _Vertices[_Count++] = b;
                return;
            }

            if (_Vertices[_Count - 1] != a) _Vertices[_Count++] = a;
            if (_Vertices[0] != b) _Vertices[_Count++] = b;
        }
    }

    /// <summary>
    /// Utility used to clip a polygon against a plane.
    /// </summary>    
    ref struct PolygonClipper4
    {
        /// <summary>
        /// Clips a polygon against a plane
        /// </summary>
        /// <param name="dstVertices">
        /// The buffer where clipped vertices are copied to.
        /// - For triangles and convex polygons, <paramref name="dstVertices"/> must be at least <paramref name="srcVertices"/>.Length + 1.
        /// - For arbitrary polygons, <paramref name="dstVertices"/> must be at least <paramref name="srcVertices"/>.Length x 2.
        /// The current algorythm does not support well polygons with holes, or with intersecting lines. For these cases
        /// it's probably better to decompose the source polygons into flat triangles.
        /// </param>
        /// <param name="srcVertices"></param>
        /// <param name="plane"></param>
        /// <returns>the final number of vertices</returns>
        public static int ClipPolygonToPlane(Span<Vector4> dstVertices, ReadOnlySpan<Vector4> srcVertices, in Plane plane)
        {
            var accum = new PolygonClipper4(dstVertices);

            var lastIdx = srcVertices.Length - 1;

            for (int i = 0; i < srcVertices.Length; ++i)
            {
                int j = i == lastIdx ? 0 : i + 1;

                var a = srcVertices[i];
                var b = srcVertices[j];

                if (a == b) continue;

                if (!ClipLineToPlane(ref a, ref b, plane)) continue;

                accum.AddSegment(a, b);
            }

            return accum._Count;
        }

        public static bool ClipLineToPlane(ref Vector4 a, ref Vector4 b, in Plane plane)
        {
            var aw = Plane.DotCoordinate(plane, a.SelectXYZ());
            var bw = Plane.DotCoordinate(plane, b.SelectXYZ());
            if (aw < 0 && bw < 0) return false;

            if (aw < 0)
            {
                var u = bw - aw;
                u = bw / u; // u values close or equal to 0 can result in values with large errors or infinity
                if (u < 1) a = Vector4.Lerp(b, a, u);// so we only clamp when u is within bounds                    

                System.Diagnostics.Debug.Assert(Plane.DotCoordinate(plane, a.SelectXYZ()) >= -0.0001f);
            }
            else if (bw < 0)
            {
                var u = aw - bw;
                u = aw / u; // u values close or equal to 0 can result in values with large errors or infinity
                if (u < 1) b = Vector4.Lerp(a, b, u); // so we only clamp when u is within bounds

                System.Diagnostics.Debug.Assert(Plane.DotCoordinate(plane, b.SelectXYZ()) >= -0.0001f);
            }

            if (a == b) return false;

            return true;
        }        

        private PolygonClipper4(Span<Vector4> vertices)
        {
            _Vertices = vertices;
            _Count = 0;
        }

        private readonly Span<Vector4> _Vertices;
        private int _Count;

        private void AddSegment(Vector4 a, Vector4 b)
        {
            if (a == b) return;

            if (_Count == 0)
            {
                _Vertices[_Count++] = a;
                _Vertices[_Count++] = b;
                return;
            }

            if (_Vertices[_Count - 1] != a) _Vertices[_Count++] = a;
            if (_Vertices[0] != b) _Vertices[_Count++] = b;
        }
    }
}
