using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace InteropTypes.Tensors
{
    ref struct _ResizeBGR
    {
        public static void Resize(SpanTensor2<Vector3> src, SpanTensor2<Vector3> dst, float offset = 0, float scale = 1)
        {
            Span<float> wx = stackalloc float[src.BitmapSize.Width * 2];
            Span<float> wy = stackalloc float[src.BitmapSize.Height * 2];

            var context = new _ResizeBGR(wx, wy, dst.BitmapSize);

            context._Shrink(src, dst, offset, scale);
        }

        public _ResizeBGR(Span<float> srcWeightX, Span<float> srcWeightY, System.Drawing.Size dstS)
        {
            _SrcWeightsX = srcWeightX;
            _SrcWeightsY = srcWeightY;

            _SetupShrinkWeights(_SrcWeightsX, srcWeightX.Length / 2, dstS.Width);
            _SetupShrinkWeights(_SrcWeightsY, srcWeightY.Length / 2, dstS.Height);
        }
        
        // how much weight has every src column over the final dst column
        Span<float> _SrcWeightsX;

        // how much weight has every src row over the final dst row
        Span<float> _SrcWeightsY;

        private void _Shrink(SpanTensor2<Vector3> src, SpanTensor2<Vector3> dst, float offset = 0, float scale = 1)
        {
            if (src.BitmapSize.Width != _SrcWeightsX.Length) throw new ArgumentException(nameof(src.BitmapSize));
            if (src.BitmapSize.Height != _SrcWeightsY.Length) throw new ArgumentException(nameof(src.BitmapSize));

            var stepy = 65536 * dst.BitmapSize.Height / src.BitmapSize.Height;
            var lasty = -1;

            for (int srcy=0; srcy < src.BitmapSize.Height; ++srcy)
            {
                var dsty = srcy * stepy / 65536;

                var srcRow = src.GetRowSpan(srcy);
                var dstRow = dst.GetRowSpan(dsty);

                if (dsty != lasty) dstRow.Fill(new Vector3(offset)); // initialize the new row

                _AddRowBilinear(srcRow, _SrcWeightsX, dstRow, _SrcWeightsY[srcy] * scale);

                lasty = dsty;
            }
        }        

        private static void _Expand(ReadOnlySpan<byte> src, Span<Vector3> dst)
        {
            _Expand(src, System.Runtime.InteropServices.MemoryMarshal.Cast<Vector3, float>(dst));
        }

        private static void _Expand(ReadOnlySpan<byte> src, Span<float> dst)
        {            
            for (int i = 0; i < src.Length; ++i) { dst[i] = src[i]; }
        }

        private static void _SetupShrinkWeights(Span<float> w, int large, int small)
        {
            // src: A  B  C
            // dst:  AB BC

            if (large == small) { w.Fill(1); return; }
            if (large == small * 2) { w.Fill(1f / 2f); return; }
            if (large == small * 3) { w.Fill(1f / 3f); return; }
            if (large == small * 4) { w.Fill(1f / 4f); return; }

            var istep = 65536 * small / large;            

            for (int sidx = 0; sidx < w.Length; ++sidx)
            {
                var didx = (sidx * istep / 65536); // the weights need to be duplicated, and acounted twice like a LERP
                
            }
        }


        private static void _AddRowBilinear(ReadOnlySpan<Vector3> s, ReadOnlySpan<float> w, Span<Vector3> d, float scale)
        {
            if (d.Length < s.Length) _AddRowBilinearShrink(s, w, d, scale);
            throw new NotImplementedException();
        }

        private static void _AddRowBilinearShrink(ReadOnlySpan<Vector3> s, ReadOnlySpan<float> w, Span<Vector3> d, float scale)
        {
            System.Diagnostics.Debug.Assert(d.Length < s.Length);
            
            var istep = 65536 * d.Length / s.Length;

            for (int sidx = 0; sidx < s.Length; ++sidx)
            {
                var didx = sidx * istep / 65536;
                d[didx] += s[sidx + 0] * w[sidx * 2 + 0] * scale;
                d[didx] += s[sidx + 1] * w[sidx * 2 + 1] * scale;
            }
        }

        struct _bgr
        {
            public Byte B;
            public Byte G;
            public Byte R;
            public Vector3 ToVector3() => new Vector3(B, G, R);
        }
    }
}
