
// GENERATED CODE: using CodeGenUtils.t4

using System;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {

        partial struct RGB24 : ISetFromQuantizedLerp<RGB24>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            public void SetFromLerp(RGB24 left, RGB24 right, uint rx)
            {
                var lx = _QLERPVALUE - rx;
                R = (Byte)((left.R * lx + right.R * rx) >> _QLERPSHIFT);
                G = (Byte)((left.G * lx + right.G * rx) >> _QLERPSHIFT);
                B = (Byte)((left.B * lx + right.B * rx) >> _QLERPSHIFT);
            }

            /// <inheritdoc/>
            public void SetFromLerp(RGB24 tl, RGB24 tr, RGB24 bl, RGB24 br, uint rx, uint by)
            {
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);
                R = (Byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                G = (Byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                B = (Byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
            }
        }
        partial struct BGR24 : ISetFromQuantizedLerp<BGR24>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            public void SetFromLerp(BGR24 left, BGR24 right, uint rx)
            {
                var lx = _QLERPVALUE - rx;
                B = (Byte)((left.B * lx + right.B * rx) >> _QLERPSHIFT);
                G = (Byte)((left.G * lx + right.G * rx) >> _QLERPSHIFT);
                R = (Byte)((left.R * lx + right.R * rx) >> _QLERPSHIFT);
            }

            /// <inheritdoc/>
            public void SetFromLerp(BGR24 tl, BGR24 tr, BGR24 bl, BGR24 br, uint rx, uint by)
            {
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);
                B = (Byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                G = (Byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                R = (Byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
            }
        }
        partial struct RGBP32 : ISetFromQuantizedLerp<RGBP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            public void SetFromLerp(RGBP32 left, RGBP32 right, uint rx)
            {
                var lx = _QLERPVALUE - rx;
                PreR = (Byte)((left.PreR * lx + right.PreR * rx) >> _QLERPSHIFT);
                PreG = (Byte)((left.PreG * lx + right.PreG * rx) >> _QLERPSHIFT);
                PreB = (Byte)((left.PreB * lx + right.PreB * rx) >> _QLERPSHIFT);
                A = (Byte)((left.A * lx + right.A * rx) >> _QLERPSHIFT);
            }

            /// <inheritdoc/>
            public void SetFromLerp(RGBP32 tl, RGBP32 tr, RGBP32 bl, RGBP32 br, uint rx, uint by)
            {
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);
                PreR = (Byte)((tl.PreR * wtl + tr.PreR * wtr + bl.PreR * wbl + br.PreR * wbr) >> _QLERPSHIFTSQUARED);
                PreG = (Byte)((tl.PreG * wtl + tr.PreG * wtr + bl.PreG * wbl + br.PreG * wbr) >> _QLERPSHIFTSQUARED);
                PreB = (Byte)((tl.PreB * wtl + tr.PreB * wtr + bl.PreB * wbl + br.PreB * wbr) >> _QLERPSHIFTSQUARED);
                A = (Byte)((tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED);
            }
        }
        partial struct BGRP32 : ISetFromQuantizedLerp<BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            public void SetFromLerp(BGRP32 left, BGRP32 right, uint rx)
            {
                var lx = _QLERPVALUE - rx;
                PreB = (Byte)((left.PreB * lx + right.PreB * rx) >> _QLERPSHIFT);
                PreG = (Byte)((left.PreG * lx + right.PreG * rx) >> _QLERPSHIFT);
                PreR = (Byte)((left.PreR * lx + right.PreR * rx) >> _QLERPSHIFT);
                A = (Byte)((left.A * lx + right.A * rx) >> _QLERPSHIFT);
            }

            /// <inheritdoc/>
            public void SetFromLerp(BGRP32 tl, BGRP32 tr, BGRP32 bl, BGRP32 br, uint rx, uint by)
            {
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);
                PreB = (Byte)((tl.PreB * wtl + tr.PreB * wtr + bl.PreB * wbl + br.PreB * wbr) >> _QLERPSHIFTSQUARED);
                PreG = (Byte)((tl.PreG * wtl + tr.PreG * wtr + bl.PreG * wbl + br.PreG * wbr) >> _QLERPSHIFTSQUARED);
                PreR = (Byte)((tl.PreR * wtl + tr.PreR * wtr + bl.PreR * wbl + br.PreR * wbr) >> _QLERPSHIFTSQUARED);
                A = (Byte)((tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED);
            }

            /// <inheritdoc/>
            public void SetFromLerp(RGBA32 l, RGBA32 r, uint rx)
            {
            }

            /// <inheritdoc/>
            public void SetFromLerp(RGBA32 tl, RGBA32 tr, RGBA32 bl, RGBA32 br, uint rx, uint by)
            {

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty;
                var wtr = rx * ty;
                var wbl = lx * by;
                var wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                var a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) { this = default; return; }

                // calculate premultiplied weights
                wtl *= tl.A;
                wtl >>= _QLERPSHIFT;
                wtr *= tr.A;
                wtr >>= _QLERPSHIFT;
                wbl *= bl.A;
                wbl >>= _QLERPSHIFT;
                wbr *= br.A;
                wbr >>= _QLERPSHIFT;

                // calculate premultiplied RGB
                var r = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFT;
                var g = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFT;
                var b = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFT;

                // set values
                PreR = (byte)r;
                PreG = (byte)g;
                PreB = (byte)b;
                A = (byte)a;
            }

            /// <inheritdoc/>
            public void SetFromLerp(BGRA32 l, BGRA32 r, uint rx)
            {
            }

            /// <inheritdoc/>
            public void SetFromLerp(BGRA32 tl, BGRA32 tr, BGRA32 bl, BGRA32 br, uint rx, uint by)
            {

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty;
                var wtr = rx * ty;
                var wbl = lx * by;
                var wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                var a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) { this = default; return; }

                // calculate premultiplied weights
                wtl *= tl.A;
                wtl >>= _QLERPSHIFT;
                wtr *= tr.A;
                wtr >>= _QLERPSHIFT;
                wbl *= bl.A;
                wbl >>= _QLERPSHIFT;
                wbr *= br.A;
                wbr >>= _QLERPSHIFT;

                // calculate premultiplied RGB
                var r = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFT;
                var g = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFT;
                var b = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFT;

                // set values
                PreR = (byte)r;
                PreG = (byte)g;
                PreB = (byte)b;
                A = (byte)a;
            }

            /// <inheritdoc/>
            public void SetFromLerp(ARGB32 l, ARGB32 r, uint rx)
            {
            }

            /// <inheritdoc/>
            public void SetFromLerp(ARGB32 tl, ARGB32 tr, ARGB32 bl, ARGB32 br, uint rx, uint by)
            {

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty;
                var wtr = rx * ty;
                var wbl = lx * by;
                var wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                var a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) { this = default; return; }

                // calculate premultiplied weights
                wtl *= tl.A;
                wtl >>= _QLERPSHIFT;
                wtr *= tr.A;
                wtr >>= _QLERPSHIFT;
                wbl *= bl.A;
                wbl >>= _QLERPSHIFT;
                wbr *= br.A;
                wbr >>= _QLERPSHIFT;

                // calculate premultiplied RGB
                var r = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFT;
                var g = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFT;
                var b = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFT;

                // set values
                PreR = (byte)r;
                PreG = (byte)g;
                PreB = (byte)b;
                A = (byte)a;
            }
        }
        partial struct RGBA32 : ISetFromQuantizedLerp<RGBA32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            public void SetFromLerp(RGBA32 l, RGBA32 r, uint rx)
            {
            }

            /// <inheritdoc/>
            public void SetFromLerp(RGBA32 tl, RGBA32 tr, RGBA32 bl, RGBA32 br, uint rx, uint by)
            {

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty;
                var wtr = rx * ty;
                var wbl = lx * by;
                var wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                var a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) { this = default; return; }

                // calculate premultiplied weights
                wtl *= tl.A;
                wtl >>= _QLERPSHIFT;
                wtr *= tr.A;
                wtr >>= _QLERPSHIFT;
                wbl *= bl.A;
                wbl >>= _QLERPSHIFT;
                wbr *= br.A;
                wbr >>= _QLERPSHIFT;

                // calculate premultiplied RGB
                var r = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFT;
                var g = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFT;
                var b = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFT;

                // unpremultiply RGB
                R = (byte)(r / a);
                G = (byte)(g / a);
                B = (byte)(b / a);
                A = (byte)a;
            }
        }
        partial struct BGRA32 : ISetFromQuantizedLerp<BGRA32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            public void SetFromLerp(BGRA32 l, BGRA32 r, uint rx)
            {
            }

            /// <inheritdoc/>
            public void SetFromLerp(BGRA32 tl, BGRA32 tr, BGRA32 bl, BGRA32 br, uint rx, uint by)
            {

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty;
                var wtr = rx * ty;
                var wbl = lx * by;
                var wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                var a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) { this = default; return; }

                // calculate premultiplied weights
                wtl *= tl.A;
                wtl >>= _QLERPSHIFT;
                wtr *= tr.A;
                wtr >>= _QLERPSHIFT;
                wbl *= bl.A;
                wbl >>= _QLERPSHIFT;
                wbr *= br.A;
                wbr >>= _QLERPSHIFT;

                // calculate premultiplied RGB
                var r = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFT;
                var g = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFT;
                var b = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFT;

                // unpremultiply RGB
                R = (byte)(r / a);
                G = (byte)(g / a);
                B = (byte)(b / a);
                A = (byte)a;
            }
        }
        partial struct ARGB32 : ISetFromQuantizedLerp<ARGB32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            public void SetFromLerp(ARGB32 l, ARGB32 r, uint rx)
            {
            }

            /// <inheritdoc/>
            public void SetFromLerp(ARGB32 tl, ARGB32 tr, ARGB32 bl, ARGB32 br, uint rx, uint by)
            {

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty;
                var wtr = rx * ty;
                var wbl = lx * by;
                var wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                var a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) { this = default; return; }

                // calculate premultiplied weights
                wtl *= tl.A;
                wtl >>= _QLERPSHIFT;
                wtr *= tr.A;
                wtr >>= _QLERPSHIFT;
                wbl *= bl.A;
                wbl >>= _QLERPSHIFT;
                wbr *= br.A;
                wbr >>= _QLERPSHIFT;

                // calculate premultiplied RGB
                var r = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFT;
                var g = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFT;
                var b = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFT;

                // unpremultiply RGB
                R = (byte)(r / a);
                G = (byte)(g / a);
                B = (byte)(b / a);
                A = (byte)a;
            }
        }

    }
}