
// GENERATED CODE: using CodeGenUtils.t4

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Numerics;


namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {

        partial struct RGBP32
            : IQuantizedInterpolator<RGBP32, RGBP32>
            , IFloatingInterpolator<RGBP32, RGBP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            RGBP32 IFloatingInterpolator<RGBP32, RGBP32>.InterpolateLinear(in RGBP32 left, in RGBP32 right, float wx) { return Lerp(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            RGBP32 IFloatingInterpolator<RGBP32, RGBP32>.InterpolateBilinear(in RGBP32 tl, in RGBP32 tr, in RGBP32 bl, in RGBP32 br, float wx, float wy) { return Lerp(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            RGBP32 IQuantizedInterpolator<RGBP32, RGBP32>.InterpolateLinear(RGBP32 left, RGBP32 right, uint wx) { return Lerp(left,right,wx); }

            /// <inheritdoc/>
            RGBP32 IQuantizedInterpolator<RGBP32, RGBP32>.InterpolateBilinear(RGBP32 tl, RGBP32 tr, RGBP32 bl, RGBP32 br, uint wx, uint wy) { return Lerp(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBP32 Lerp(RGBP32 left, RGBP32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var result);
                #else
                var result = default(RGBP32);
                #endif
                result.PreR = (Byte)((left.PreR * lx + right.PreR * rx) >> _QLERPSHIFT);
                result.PreG = (Byte)((left.PreG * lx + right.PreG * rx) >> _QLERPSHIFT);
                result.PreB = (Byte)((left.PreB * lx + right.PreB * rx) >> _QLERPSHIFT);
                result.A = (Byte)((left.A * lx + right.A * rx) >> _QLERPSHIFT);
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBP32 Lerp(RGBP32 tl, RGBP32 tr, RGBP32 bl, RGBP32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBP32>(out var result);
                #else
                var result = default(RGBP32);
                #endif
                result.PreR = (Byte)((tl.PreR * wtl + tr.PreR * wtr + bl.PreR * wbl + br.PreR * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (Byte)((tl.PreG * wtl + tr.PreG * wtr + bl.PreG * wbl + br.PreG * wbr) >> _QLERPSHIFTSQUARED);
                result.PreB = (Byte)((tl.PreB * wtl + tr.PreB * wtr + bl.PreB * wbl + br.PreB * wbr) >> _QLERPSHIFTSQUARED);
                result.A = (Byte)((tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED);
                return result;
            }
        }
        partial struct BGRP32
            : IQuantizedInterpolator<BGRP32, BGRP32>
            , IFloatingInterpolator<BGRP32, BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGRP32, BGRP32>.InterpolateLinear(in BGRP32 left, in BGRP32 right, float wx) { return Lerp(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGRP32, BGRP32>.InterpolateBilinear(in BGRP32 tl, in BGRP32 tr, in BGRP32 bl, in BGRP32 br, float wx, float wy) { return Lerp(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGRP32, BGRP32>.InterpolateLinear(BGRP32 left, BGRP32 right, uint wx) { return Lerp(left,right,wx); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGRP32, BGRP32>.InterpolateBilinear(BGRP32 tl, BGRP32 tr, BGRP32 bl, BGRP32 br, uint wx, uint wy) { return Lerp(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 Lerp(BGRP32 left, BGRP32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreB = (Byte)((left.PreB * lx + right.PreB * rx) >> _QLERPSHIFT);
                result.PreG = (Byte)((left.PreG * lx + right.PreG * rx) >> _QLERPSHIFT);
                result.PreR = (Byte)((left.PreR * lx + right.PreR * rx) >> _QLERPSHIFT);
                result.A = (Byte)((left.A * lx + right.A * rx) >> _QLERPSHIFT);
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 Lerp(BGRP32 tl, BGRP32 tr, BGRP32 bl, BGRP32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreB = (Byte)((tl.PreB * wtl + tr.PreB * wtr + bl.PreB * wbl + br.PreB * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (Byte)((tl.PreG * wtl + tr.PreG * wtr + bl.PreG * wbl + br.PreG * wbr) >> _QLERPSHIFTSQUARED);
                result.PreR = (Byte)((tl.PreR * wtl + tr.PreR * wtr + bl.PreR * wbl + br.PreR * wbr) >> _QLERPSHIFTSQUARED);
                result.A = (Byte)((tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED);
                return result;
            }
        }
        partial struct BGR565
            : IQuantizedInterpolator<BGR565, BGRP32>
            , IFloatingInterpolator<BGR565, BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGR565, BGRP32>.InterpolateLinear(in BGR565 left, in BGR565 right, float wx) { return LerpToBGRP32(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGR565, BGRP32>.InterpolateBilinear(in BGR565 tl, in BGR565 tr, in BGR565 bl, in BGR565 br, float wx, float wy) { return LerpToBGRP32(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGR565, BGRP32>.InterpolateLinear(BGR565 left, BGR565 right, uint wx) { return LerpToBGRP32(left,right,wx); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGR565, BGRP32>.InterpolateBilinear(BGR565 tl, BGR565 tr, BGR565 bl, BGR565 br, uint wx, uint wy) { return LerpToBGRP32(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(BGR565 left, BGR565 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreB = (Byte)((left.B * lx + right.B * rx) >> _QLERPSHIFT);
                result.PreG = (Byte)((left.G * lx + right.G * rx) >> _QLERPSHIFT);
                result.PreR = (Byte)((left.R * lx + right.R * rx) >> _QLERPSHIFT);
                result.A = 255;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(BGR565 tl, BGR565 tr, BGR565 bl, BGR565 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreB = (Byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (Byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.PreR = (Byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                result.A = 255;
                return result;
            }
        }
        partial struct RGB24
            : IQuantizedInterpolator<RGB24, RGB24>
            , IQuantizedInterpolator<RGB24, BGRP32>
            , IFloatingInterpolator<RGB24, RGB24>
            , IFloatingInterpolator<RGB24, BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            RGB24 IFloatingInterpolator<RGB24, RGB24>.InterpolateLinear(in RGB24 left, in RGB24 right, float wx) { return Lerp(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            RGB24 IFloatingInterpolator<RGB24, RGB24>.InterpolateBilinear(in RGB24 tl, in RGB24 tr, in RGB24 bl, in RGB24 br, float wx, float wy) { return Lerp(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            RGB24 IQuantizedInterpolator<RGB24, RGB24>.InterpolateLinear(RGB24 left, RGB24 right, uint wx) { return Lerp(left,right,wx); }

            /// <inheritdoc/>
            RGB24 IQuantizedInterpolator<RGB24, RGB24>.InterpolateBilinear(RGB24 tl, RGB24 tr, RGB24 bl, RGB24 br, uint wx, uint wy) { return Lerp(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<RGB24, BGRP32>.InterpolateLinear(in RGB24 left, in RGB24 right, float wx) { return LerpToBGRP32(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<RGB24, BGRP32>.InterpolateBilinear(in RGB24 tl, in RGB24 tr, in RGB24 bl, in RGB24 br, float wx, float wy) { return LerpToBGRP32(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<RGB24, BGRP32>.InterpolateLinear(RGB24 left, RGB24 right, uint wx) { return LerpToBGRP32(left,right,wx); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<RGB24, BGRP32>.InterpolateBilinear(RGB24 tl, RGB24 tr, RGB24 bl, RGB24 br, uint wx, uint wy) { return LerpToBGRP32(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGB24 Lerp(RGB24 left, RGB24 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var result);
                #else
                var result = default(RGB24);
                #endif
                result.R = (Byte)((left.R * lx + right.R * rx) >> _QLERPSHIFT);
                result.G = (Byte)((left.G * lx + right.G * rx) >> _QLERPSHIFT);
                result.B = (Byte)((left.B * lx + right.B * rx) >> _QLERPSHIFT);
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGB24 Lerp(RGB24 tl, RGB24 tr, RGB24 bl, RGB24 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGB24>(out var result);
                #else
                var result = default(RGB24);
                #endif
                result.R = (Byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                result.G = (Byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.B = (Byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(RGB24 left, RGB24 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreR = (Byte)((left.R * lx + right.R * rx) >> _QLERPSHIFT);
                result.PreG = (Byte)((left.G * lx + right.G * rx) >> _QLERPSHIFT);
                result.PreB = (Byte)((left.B * lx + right.B * rx) >> _QLERPSHIFT);
                result.A = 255;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(RGB24 tl, RGB24 tr, RGB24 bl, RGB24 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreR = (Byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (Byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.PreB = (Byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                result.A = 255;
                return result;
            }
        }
        partial struct BGR24
            : IQuantizedInterpolator<BGR24, BGR24>
            , IQuantizedInterpolator<BGR24, BGRP32>
            , IFloatingInterpolator<BGR24, BGR24>
            , IFloatingInterpolator<BGR24, BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            BGR24 IFloatingInterpolator<BGR24, BGR24>.InterpolateLinear(in BGR24 left, in BGR24 right, float wx) { return Lerp(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGR24 IFloatingInterpolator<BGR24, BGR24>.InterpolateBilinear(in BGR24 tl, in BGR24 tr, in BGR24 bl, in BGR24 br, float wx, float wy) { return Lerp(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGR24 IQuantizedInterpolator<BGR24, BGR24>.InterpolateLinear(BGR24 left, BGR24 right, uint wx) { return Lerp(left,right,wx); }

            /// <inheritdoc/>
            BGR24 IQuantizedInterpolator<BGR24, BGR24>.InterpolateBilinear(BGR24 tl, BGR24 tr, BGR24 bl, BGR24 br, uint wx, uint wy) { return Lerp(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGR24, BGRP32>.InterpolateLinear(in BGR24 left, in BGR24 right, float wx) { return LerpToBGRP32(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGR24, BGRP32>.InterpolateBilinear(in BGR24 tl, in BGR24 tr, in BGR24 bl, in BGR24 br, float wx, float wy) { return LerpToBGRP32(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGR24, BGRP32>.InterpolateLinear(BGR24 left, BGR24 right, uint wx) { return LerpToBGRP32(left,right,wx); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGR24, BGRP32>.InterpolateBilinear(BGR24 tl, BGR24 tr, BGR24 bl, BGR24 br, uint wx, uint wy) { return LerpToBGRP32(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGR24 Lerp(BGR24 left, BGR24 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var result);
                #else
                var result = default(BGR24);
                #endif
                result.B = (Byte)((left.B * lx + right.B * rx) >> _QLERPSHIFT);
                result.G = (Byte)((left.G * lx + right.G * rx) >> _QLERPSHIFT);
                result.R = (Byte)((left.R * lx + right.R * rx) >> _QLERPSHIFT);
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGR24 Lerp(BGR24 tl, BGR24 tr, BGR24 bl, BGR24 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGR24>(out var result);
                #else
                var result = default(BGR24);
                #endif
                result.B = (Byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                result.G = (Byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.R = (Byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(BGR24 left, BGR24 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreB = (Byte)((left.B * lx + right.B * rx) >> _QLERPSHIFT);
                result.PreG = (Byte)((left.G * lx + right.G * rx) >> _QLERPSHIFT);
                result.PreR = (Byte)((left.R * lx + right.R * rx) >> _QLERPSHIFT);
                result.A = 255;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(BGR24 tl, BGR24 tr, BGR24 bl, BGR24 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                var lx = _QLERPVALUE - rx;
                var ty = _QLERPVALUE - by;
                var wtl = lx * ty; // top-left weight
                var wtr = rx * ty; // top-right weight
                var wbl = lx * by; // bottom-left weight
                var wbr = rx * by; // bottom-right weight
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // lerp
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif
                result.PreB = (Byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (Byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.PreR = (Byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                result.A = 255;
                return result;
            }
        }
        partial struct RGBA32
            : IQuantizedInterpolator<RGBA32, RGBA32>
            , IQuantizedInterpolator<RGBA32, BGRP32>
            , IFloatingInterpolator<RGBA32, RGBA32>
            , IFloatingInterpolator<RGBA32, BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            RGBA32 IFloatingInterpolator<RGBA32, RGBA32>.InterpolateLinear(in RGBA32 left, in RGBA32 right, float wx) { return Lerp(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            RGBA32 IFloatingInterpolator<RGBA32, RGBA32>.InterpolateBilinear(in RGBA32 tl, in RGBA32 tr, in RGBA32 bl, in RGBA32 br, float wx, float wy) { return Lerp(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            RGBA32 IQuantizedInterpolator<RGBA32, RGBA32>.InterpolateLinear(RGBA32 left, RGBA32 right, uint wx) { return Lerp(left,right,wx); }

            /// <inheritdoc/>
            RGBA32 IQuantizedInterpolator<RGBA32, RGBA32>.InterpolateBilinear(RGBA32 tl, RGBA32 tr, RGBA32 bl, RGBA32 br, uint wx, uint wy) { return Lerp(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<RGBA32, BGRP32>.InterpolateLinear(in RGBA32 left, in RGBA32 right, float wx) { return LerpToBGRP32(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<RGBA32, BGRP32>.InterpolateBilinear(in RGBA32 tl, in RGBA32 tr, in RGBA32 bl, in RGBA32 br, float wx, float wy) { return LerpToBGRP32(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<RGBA32, BGRP32>.InterpolateLinear(RGBA32 left, RGBA32 right, uint wx) { return LerpToBGRP32(left,right,wx); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<RGBA32, BGRP32>.InterpolateBilinear(RGBA32 tl, RGBA32 tr, RGBA32 bl, RGBA32 br, uint wx, uint wy) { return LerpToBGRP32(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBA32 Lerp(RGBA32 left, RGBA32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                rx *= rx;

                // calculate quantized weights
                uint lx = _QLERPVALUESQUARED - rx;

                // calculate final alpha
                uint a = (left.A * lx + right.A * rx) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights
                lx = ((lx * 258) >> 16) * left.A;
                rx = ((rx * 258) >> 16) * right.A;

                uint preR = (left.R * lx + right.R * rx) >> _QLERPSHIFTSQUARED;
                uint preG = (left.G * lx + right.G * rx) >> _QLERPSHIFTSQUARED;
                uint preB = (left.B * lx + right.B * rx) >> _QLERPSHIFTSQUARED;

                // unpremultiply RGB
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var result);
                #else
                var result = default(RGBA32);
                #endif

                System.Diagnostics.Debug.Assert((preR) < 256 && (preG) < 256 && (preB) < 256,"invalid RGB values");
                uint invAlpha = (65536u * 255u) / (uint)a;
                result.R = (byte)(( (preR) * invAlpha + 255u) >> 16);
                result.G = (byte)(( (preG) * invAlpha + 255u) >> 16);
                result.B = (byte)(( (preB) * invAlpha + 255u) >> 16);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static RGBA32 Lerp(RGBA32 tl, RGBA32 tr, RGBA32 bl, RGBA32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                uint lx = _QLERPVALUE - rx;
                uint ty = _QLERPVALUE - by;
                uint wtl = lx * ty;
                uint wtr = rx * ty;
                uint wbl = lx * by;
                uint wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                uint a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights (should divide by 255, but compensate precission loss by multiplying by 257)
                wtl = ((wtl * 258) >> 16) * tl.A;
                wtr = ((wtr * 258) >> 16) * tr.A;
                wbl = ((wbl * 258) >> 16) * bl.A;
                wbr = ((wbr * 258) >> 16) * br.A;
                System.Diagnostics.Debug.Assert((int)(wtl+wtr+wbl+wbr) <= (_QLERPVALUESQUARED + (_QLERPVALUESQUARED >> 8) -1) );

                uint preR = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED;
                uint preG = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED;
                uint preB = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED;

                // unpremultiply RGB
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<RGBA32>(out var result);
                #else
                var result = default(RGBA32);
                #endif

                System.Diagnostics.Debug.Assert((preR) < 256 && (preG) < 256 && (preB) < 256,"invalid RGB values");
                uint invAlpha = (65536u * 255u) / (uint)a;
                result.R = (byte)(( (preR) * invAlpha + 255u) >> 16);
                result.G = (byte)(( (preG) * invAlpha + 255u) >> 16);
                result.B = (byte)(( (preB) * invAlpha + 255u) >> 16);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(RGBA32 left, RGBA32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                rx *= rx;

                // calculate quantized weights
                uint lx = _QLERPVALUESQUARED - rx;

                // calculate final alpha
                uint a = (left.A * lx + right.A * rx) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights
                lx = ((lx * 258) >> 16) * left.A;
                rx = ((rx * 258) >> 16) * right.A;


                // set values
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif

                result.PreR = (byte)((left.R * lx + right.R * rx) >> _QLERPSHIFTSQUARED);
                result.PreG = (byte)((left.G * lx + right.G * rx) >> _QLERPSHIFTSQUARED);
                result.PreB = (byte)((left.B * lx + right.B * rx) >> _QLERPSHIFTSQUARED);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(RGBA32 tl, RGBA32 tr, RGBA32 bl, RGBA32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                uint lx = _QLERPVALUE - rx;
                uint ty = _QLERPVALUE - by;
                uint wtl = lx * ty;
                uint wtr = rx * ty;
                uint wbl = lx * by;
                uint wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                uint a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights (should divide by 255, but compensate precission loss by multiplying by 257)
                wtl = ((wtl * 258) >> 16) * tl.A;
                wtr = ((wtr * 258) >> 16) * tr.A;
                wbl = ((wbl * 258) >> 16) * bl.A;
                wbr = ((wbr * 258) >> 16) * br.A;
                System.Diagnostics.Debug.Assert((int)(wtl+wtr+wbl+wbr) <= (_QLERPVALUESQUARED + (_QLERPVALUESQUARED >> 8) -1) );


                // set values
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif

                result.PreR = (byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.PreB = (byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                result.A = (byte)a;
                return result;
            }
        }
        partial struct BGRA32
            : IQuantizedInterpolator<BGRA32, BGRA32>
            , IQuantizedInterpolator<BGRA32, BGRP32>
            , IFloatingInterpolator<BGRA32, BGRA32>
            , IFloatingInterpolator<BGRA32, BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            BGRA32 IFloatingInterpolator<BGRA32, BGRA32>.InterpolateLinear(in BGRA32 left, in BGRA32 right, float wx) { return Lerp(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRA32 IFloatingInterpolator<BGRA32, BGRA32>.InterpolateBilinear(in BGRA32 tl, in BGRA32 tr, in BGRA32 bl, in BGRA32 br, float wx, float wy) { return Lerp(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRA32 IQuantizedInterpolator<BGRA32, BGRA32>.InterpolateLinear(BGRA32 left, BGRA32 right, uint wx) { return Lerp(left,right,wx); }

            /// <inheritdoc/>
            BGRA32 IQuantizedInterpolator<BGRA32, BGRA32>.InterpolateBilinear(BGRA32 tl, BGRA32 tr, BGRA32 bl, BGRA32 br, uint wx, uint wy) { return Lerp(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGRA32, BGRP32>.InterpolateLinear(in BGRA32 left, in BGRA32 right, float wx) { return LerpToBGRP32(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<BGRA32, BGRP32>.InterpolateBilinear(in BGRA32 tl, in BGRA32 tr, in BGRA32 bl, in BGRA32 br, float wx, float wy) { return LerpToBGRP32(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGRA32, BGRP32>.InterpolateLinear(BGRA32 left, BGRA32 right, uint wx) { return LerpToBGRP32(left,right,wx); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<BGRA32, BGRP32>.InterpolateBilinear(BGRA32 tl, BGRA32 tr, BGRA32 bl, BGRA32 br, uint wx, uint wy) { return LerpToBGRP32(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRA32 Lerp(BGRA32 left, BGRA32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                rx *= rx;

                // calculate quantized weights
                uint lx = _QLERPVALUESQUARED - rx;

                // calculate final alpha
                uint a = (left.A * lx + right.A * rx) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights
                lx = ((lx * 258) >> 16) * left.A;
                rx = ((rx * 258) >> 16) * right.A;

                uint preR = (left.R * lx + right.R * rx) >> _QLERPSHIFTSQUARED;
                uint preG = (left.G * lx + right.G * rx) >> _QLERPSHIFTSQUARED;
                uint preB = (left.B * lx + right.B * rx) >> _QLERPSHIFTSQUARED;

                // unpremultiply RGB
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var result);
                #else
                var result = default(BGRA32);
                #endif

                System.Diagnostics.Debug.Assert((preR) < 256 && (preG) < 256 && (preB) < 256,"invalid RGB values");
                uint invAlpha = (65536u * 255u) / (uint)a;
                result.R = (byte)(( (preR) * invAlpha + 255u) >> 16);
                result.G = (byte)(( (preG) * invAlpha + 255u) >> 16);
                result.B = (byte)(( (preB) * invAlpha + 255u) >> 16);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRA32 Lerp(BGRA32 tl, BGRA32 tr, BGRA32 bl, BGRA32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                uint lx = _QLERPVALUE - rx;
                uint ty = _QLERPVALUE - by;
                uint wtl = lx * ty;
                uint wtr = rx * ty;
                uint wbl = lx * by;
                uint wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                uint a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights (should divide by 255, but compensate precission loss by multiplying by 257)
                wtl = ((wtl * 258) >> 16) * tl.A;
                wtr = ((wtr * 258) >> 16) * tr.A;
                wbl = ((wbl * 258) >> 16) * bl.A;
                wbr = ((wbr * 258) >> 16) * br.A;
                System.Diagnostics.Debug.Assert((int)(wtl+wtr+wbl+wbr) <= (_QLERPVALUESQUARED + (_QLERPVALUESQUARED >> 8) -1) );

                uint preR = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED;
                uint preG = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED;
                uint preB = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED;

                // unpremultiply RGB
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRA32>(out var result);
                #else
                var result = default(BGRA32);
                #endif

                System.Diagnostics.Debug.Assert((preR) < 256 && (preG) < 256 && (preB) < 256,"invalid RGB values");
                uint invAlpha = (65536u * 255u) / (uint)a;
                result.R = (byte)(( (preR) * invAlpha + 255u) >> 16);
                result.G = (byte)(( (preG) * invAlpha + 255u) >> 16);
                result.B = (byte)(( (preB) * invAlpha + 255u) >> 16);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(BGRA32 left, BGRA32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                rx *= rx;

                // calculate quantized weights
                uint lx = _QLERPVALUESQUARED - rx;

                // calculate final alpha
                uint a = (left.A * lx + right.A * rx) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights
                lx = ((lx * 258) >> 16) * left.A;
                rx = ((rx * 258) >> 16) * right.A;


                // set values
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif

                result.PreR = (byte)((left.R * lx + right.R * rx) >> _QLERPSHIFTSQUARED);
                result.PreG = (byte)((left.G * lx + right.G * rx) >> _QLERPSHIFTSQUARED);
                result.PreB = (byte)((left.B * lx + right.B * rx) >> _QLERPSHIFTSQUARED);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(BGRA32 tl, BGRA32 tr, BGRA32 bl, BGRA32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                uint lx = _QLERPVALUE - rx;
                uint ty = _QLERPVALUE - by;
                uint wtl = lx * ty;
                uint wtr = rx * ty;
                uint wbl = lx * by;
                uint wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                uint a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights (should divide by 255, but compensate precission loss by multiplying by 257)
                wtl = ((wtl * 258) >> 16) * tl.A;
                wtr = ((wtr * 258) >> 16) * tr.A;
                wbl = ((wbl * 258) >> 16) * bl.A;
                wbr = ((wbr * 258) >> 16) * br.A;
                System.Diagnostics.Debug.Assert((int)(wtl+wtr+wbl+wbr) <= (_QLERPVALUESQUARED + (_QLERPVALUESQUARED >> 8) -1) );


                // set values
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif

                result.PreR = (byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.PreB = (byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                result.A = (byte)a;
                return result;
            }
        }
        partial struct ARGB32
            : IQuantizedInterpolator<ARGB32, ARGB32>
            , IQuantizedInterpolator<ARGB32, BGRP32>
            , IFloatingInterpolator<ARGB32, ARGB32>
            , IFloatingInterpolator<ARGB32, BGRP32>
        {
            const int _QLERPSHIFT = 11;
            const int _QLERPVALUE = 1 << _QLERPSHIFT;
            const float _QLERPVALUEF = _QLERPVALUE;
            const int _QLERPSHIFTSQUARED = _QLERPSHIFT*2;
            const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

            /// <inheritdoc/>
            public int QuantizedLerpShift => _QLERPSHIFT;

            /// <inheritdoc/>
            ARGB32 IFloatingInterpolator<ARGB32, ARGB32>.InterpolateLinear(in ARGB32 left, in ARGB32 right, float wx) { return Lerp(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            ARGB32 IFloatingInterpolator<ARGB32, ARGB32>.InterpolateBilinear(in ARGB32 tl, in ARGB32 tr, in ARGB32 bl, in ARGB32 br, float wx, float wy) { return Lerp(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            ARGB32 IQuantizedInterpolator<ARGB32, ARGB32>.InterpolateLinear(ARGB32 left, ARGB32 right, uint wx) { return Lerp(left,right,wx); }

            /// <inheritdoc/>
            ARGB32 IQuantizedInterpolator<ARGB32, ARGB32>.InterpolateBilinear(ARGB32 tl, ARGB32 tr, ARGB32 bl, ARGB32 br, uint wx, uint wy) { return Lerp(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<ARGB32, BGRP32>.InterpolateLinear(in ARGB32 left, in ARGB32 right, float wx) { return LerpToBGRP32(left, right, (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IFloatingInterpolator<ARGB32, BGRP32>.InterpolateBilinear(in ARGB32 tl, in ARGB32 tr, in ARGB32 bl, in ARGB32 br, float wx, float wy) { return LerpToBGRP32(tl, tr, bl, br, (uint)(wx*_QLERPVALUEF), (uint)(wx*_QLERPVALUEF) ); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<ARGB32, BGRP32>.InterpolateLinear(ARGB32 left, ARGB32 right, uint wx) { return LerpToBGRP32(left,right,wx); }

            /// <inheritdoc/>
            BGRP32 IQuantizedInterpolator<ARGB32, BGRP32>.InterpolateBilinear(ARGB32 tl, ARGB32 tr, ARGB32 bl, ARGB32 br, uint wx, uint wy) { return LerpToBGRP32(tl,tr,bl,br,wx,wy); }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static ARGB32 Lerp(ARGB32 left, ARGB32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                rx *= rx;

                // calculate quantized weights
                uint lx = _QLERPVALUESQUARED - rx;

                // calculate final alpha
                uint a = (left.A * lx + right.A * rx) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights
                lx = ((lx * 258) >> 16) * left.A;
                rx = ((rx * 258) >> 16) * right.A;

                uint preR = (left.R * lx + right.R * rx) >> _QLERPSHIFTSQUARED;
                uint preG = (left.G * lx + right.G * rx) >> _QLERPSHIFTSQUARED;
                uint preB = (left.B * lx + right.B * rx) >> _QLERPSHIFTSQUARED;

                // unpremultiply RGB
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var result);
                #else
                var result = default(ARGB32);
                #endif

                System.Diagnostics.Debug.Assert((preR) < 256 && (preG) < 256 && (preB) < 256,"invalid RGB values");
                uint invAlpha = (65536u * 255u) / (uint)a;
                result.R = (byte)(( (preR) * invAlpha + 255u) >> 16);
                result.G = (byte)(( (preG) * invAlpha + 255u) >> 16);
                result.B = (byte)(( (preB) * invAlpha + 255u) >> 16);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static ARGB32 Lerp(ARGB32 tl, ARGB32 tr, ARGB32 bl, ARGB32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                uint lx = _QLERPVALUE - rx;
                uint ty = _QLERPVALUE - by;
                uint wtl = lx * ty;
                uint wtr = rx * ty;
                uint wbl = lx * by;
                uint wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                uint a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights (should divide by 255, but compensate precission loss by multiplying by 257)
                wtl = ((wtl * 258) >> 16) * tl.A;
                wtr = ((wtr * 258) >> 16) * tr.A;
                wbl = ((wbl * 258) >> 16) * bl.A;
                wbr = ((wbr * 258) >> 16) * br.A;
                System.Diagnostics.Debug.Assert((int)(wtl+wtr+wbl+wbr) <= (_QLERPVALUESQUARED + (_QLERPVALUESQUARED >> 8) -1) );

                uint preR = (tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED;
                uint preG = (tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED;
                uint preB = (tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED;

                // unpremultiply RGB
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<ARGB32>(out var result);
                #else
                var result = default(ARGB32);
                #endif

                System.Diagnostics.Debug.Assert((preR) < 256 && (preG) < 256 && (preB) < 256,"invalid RGB values");
                uint invAlpha = (65536u * 255u) / (uint)a;
                result.R = (byte)(( (preR) * invAlpha + 255u) >> 16);
                result.G = (byte)(( (preG) * invAlpha + 255u) >> 16);
                result.B = (byte)(( (preB) * invAlpha + 255u) >> 16);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(ARGB32 left, ARGB32 right, uint rx)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                rx *= rx;

                // calculate quantized weights
                uint lx = _QLERPVALUESQUARED - rx;

                // calculate final alpha
                uint a = (left.A * lx + right.A * rx) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights
                lx = ((lx * 258) >> 16) * left.A;
                rx = ((rx * 258) >> 16) * right.A;


                // set values
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif

                result.PreR = (byte)((left.R * lx + right.R * rx) >> _QLERPSHIFTSQUARED);
                result.PreG = (byte)((left.G * lx + right.G * rx) >> _QLERPSHIFTSQUARED);
                result.PreB = (byte)((left.B * lx + right.B * rx) >> _QLERPSHIFTSQUARED);
                result.A = (byte)a;
                return result;
            }

            /// <inheritdoc/>
            [MethodImpl(_PrivateConstants.Fastest)]
            public static BGRP32 LerpToBGRP32(ARGB32 tl, ARGB32 tr, ARGB32 bl, ARGB32 br, uint rx, uint by)
            {
                System.Diagnostics.Debug.Assert((int)rx <= _QLERPVALUE);
                System.Diagnostics.Debug.Assert((int)by <= _QLERPVALUE);

                // calculate quantized weights
                uint lx = _QLERPVALUE - rx;
                uint ty = _QLERPVALUE - by;
                uint wtl = lx * ty;
                uint wtr = rx * ty;
                uint wbl = lx * by;
                uint wbr = rx * by;
                System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == _QLERPVALUESQUARED);

                // calculate final alpha
                uint a = (tl.A * wtl + tr.A * wtr + bl.A * wbl + br.A * wbr) >> _QLERPSHIFTSQUARED;
                if (a == 0) return default;

                // calculate premultiplied weights (should divide by 255, but compensate precission loss by multiplying by 257)
                wtl = ((wtl * 258) >> 16) * tl.A;
                wtr = ((wtr * 258) >> 16) * tr.A;
                wbl = ((wbl * 258) >> 16) * bl.A;
                wbr = ((wbr * 258) >> 16) * br.A;
                System.Diagnostics.Debug.Assert((int)(wtl+wtr+wbl+wbr) <= (_QLERPVALUESQUARED + (_QLERPVALUESQUARED >> 8) -1) );


                // set values
                #if NET5_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif

                result.PreR = (byte)((tl.R * wtl + tr.R * wtr + bl.R * wbl + br.R * wbr) >> _QLERPSHIFTSQUARED);
                result.PreG = (byte)((tl.G * wtl + tr.G * wtr + bl.G * wbl + br.G * wbr) >> _QLERPSHIFTSQUARED);
                result.PreB = (byte)((tl.B * wtl + tr.B * wtr + bl.B * wbl + br.B * wbr) >> _QLERPSHIFTSQUARED);
                result.A = (byte)a;
                return result;
            }
        }

        /// <summary>
        /// Gets an interpolator interface for the given pixel src and dst combination, or NULL if an interpolator doesn't exist.
        /// </summary>
        public static IQuantizedInterpolator<TSrcPixel, TDstPixel> GetQuantizedInterpolator<TSrcPixel, TDstPixel>() where TSrcPixel:unmanaged where TDstPixel:unmanaged
        {
            if (typeof(TDstPixel) == typeof(RGBP32))
            {
                if (typeof(TSrcPixel) == typeof(RGBP32)) return default(RGBP32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(BGRP32))
            {
                if (typeof(TSrcPixel) == typeof(BGRP32)) return default(BGRP32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(BGR565)) return default(BGR565) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(RGB24)) return default(RGB24) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(BGR24)) return default(BGR24) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(RGBA32)) return default(RGBA32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(BGRA32)) return default(BGRA32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(ARGB32)) return default(ARGB32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(RGB24))
            {
                if (typeof(TSrcPixel) == typeof(RGB24)) return default(RGB24) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(BGR24))
            {
                if (typeof(TSrcPixel) == typeof(BGR24)) return default(BGR24) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(RGBA32))
            {
                if (typeof(TSrcPixel) == typeof(RGBA32)) return default(RGBA32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(BGRA32))
            {
                if (typeof(TSrcPixel) == typeof(BGRA32)) return default(BGRA32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(ARGB32))
            {
                if (typeof(TSrcPixel) == typeof(ARGB32)) return default(ARGB32) as IQuantizedInterpolator<TSrcPixel, TDstPixel>;
            }
            return null;
        }

        /// <summary>
        /// Gets an interpolator interface for the given pixel src and dst combination, or NULL if an interpolator doesn't exist.
        /// </summary>
        public static IFloatingInterpolator<TSrcPixel, TDstPixel> GetFloatingInterpolator<TSrcPixel, TDstPixel>() where TSrcPixel:unmanaged where TDstPixel:unmanaged
        {
            if (typeof(TDstPixel) == typeof(RGBP32))
            {
                if (typeof(TSrcPixel) == typeof(RGBP32)) return default(RGBP32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(BGRP32))
            {
                if (typeof(TSrcPixel) == typeof(BGRP32)) return default(BGRP32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(BGR565)) return default(BGR565) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(RGB24)) return default(RGB24) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(BGR24)) return default(BGR24) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(RGBA32)) return default(RGBA32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(BGRA32)) return default(BGRA32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
                if (typeof(TSrcPixel) == typeof(ARGB32)) return default(ARGB32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(RGB24))
            {
                if (typeof(TSrcPixel) == typeof(RGB24)) return default(RGB24) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(BGR24))
            {
                if (typeof(TSrcPixel) == typeof(BGR24)) return default(BGR24) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(RGBA32))
            {
                if (typeof(TSrcPixel) == typeof(RGBA32)) return default(RGBA32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(BGRA32))
            {
                if (typeof(TSrcPixel) == typeof(BGRA32)) return default(BGRA32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
            }
            if (typeof(TDstPixel) == typeof(ARGB32))
            {
                if (typeof(TSrcPixel) == typeof(ARGB32)) return default(ARGB32) as IFloatingInterpolator<TSrcPixel, TDstPixel>;
            }
            return null;
        }

    }
}