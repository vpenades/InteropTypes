using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {        
        [MethodImpl(_PrivateConstants.Fastest)]
        public static TDstPixel LerpQuantized<TSrcPixel,TDstPixel>(in TSrcPixel left, in TSrcPixel right, uint rx)
            where TSrcPixel: unmanaged
            where TDstPixel: unmanaged
        {
            System.Diagnostics.Debug.Assert(rx <= 1024u);

            // calculate quantized weights
            var lx = 1024 - rx;

            // lerp
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<BGRP32>(out var result);
            #else
            var result = default(BGRP32);
            #endif

            var l = BGRP32.From(left);
            var r = BGRP32.From(right);

            result.PreR = (Byte)((l.PreR * lx + r.PreR * rx) >> 10);
            result.PreG = (Byte)((l.PreG * lx + r.PreG * rx) >> 10);
            result.PreB = (Byte)((l.PreB * lx + r.PreB * rx) >> 10);
            result.A = (Byte)((l.A * lx + r.A * rx) >> 10);

            return result.To<TDstPixel>();
        }
        
        [MethodImpl(_PrivateConstants.Fastest)]
        public static TDstPixel LerpQuantized<TSrcPixel, TDstPixel>(in TSrcPixel tl, in TSrcPixel tr, in TSrcPixel bl, in TSrcPixel br, uint rx, uint by)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            System.Diagnostics.Debug.Assert(rx <= 1024u);
            System.Diagnostics.Debug.Assert(by <= 1024u);

            // calculate quantized weights
            var lx = 1024 - rx;
            var ty = 1024 - by;
            var wtl = lx * ty; // top-left weight
            var wtr = rx * ty; // top-right weight
            var wbl = lx * by; // bottom-left weight
            var wbr = rx * by; // bottom-right weight
            System.Diagnostics.Debug.Assert(wtl + wtr + wbl + wbr == 1024 * 1024);

            // lerp
            #if NET5_0_OR_GREATER
            Unsafe.SkipInit<BGRP32>(out var result);
            #else
            var result = default(BGRP32);
            #endif

            // write here optimized if/elses for BGR and BGRA and BGRP

            var ptl = BGRP32.From(tl);
            var ptr = BGRP32.From(tr);
            var pbl = BGRP32.From(bl);
            var pbr = BGRP32.From(br);

            result.PreR = (Byte)((ptl.PreR * wtl + ptr.PreR * wtr + pbl.PreR * wbl + pbr.PreR * wbr) >> 20);
            result.PreG = (Byte)((ptl.PreG * wtl + ptr.PreG * wtr + pbl.PreG * wbl + pbr.PreG * wbr) >> 20);
            result.PreB = (Byte)((ptl.PreB * wtl + ptr.PreB * wtr + pbl.PreB * wbl + pbr.PreB * wbr) >> 20);
            result.A = (Byte)((ptl.A * wtl + ptr.A * wtr + pbl.A * wbl + pbr.A * wbr) >> 20);

            return result.To<TDstPixel>();
        }
    }
}