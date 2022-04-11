using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Tensors
{
    [System.Diagnostics.DebuggerDisplay("{X} {Y} {Z}")]
    struct _PixelXYZ24
    {
        #region data

        public Byte X;
        public Byte Y;
        public Byte Z;

        #endregion

        #region API

        const int _QLERPSHIFT = 10;
        const int _QLERPVALUE = 1 << _QLERPSHIFT;        
        const int _QLERPSHIFTSQUARED = _QLERPSHIFT * 2;
        const int _QLERPVALUESQUARED = 1 << _QLERPSHIFTSQUARED;

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static _PixelXYZ24 BiLerp(_PixelXYZ24 tl, _PixelXYZ24 tr, _PixelXYZ24 bl, _PixelXYZ24 br, uint rx, uint by)
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
            Unsafe.SkipInit<_PixelXYZ24>(out var result);
            #else
            var result = default(_PixelXYZ24);
            #endif

            result.X = (Byte)((tl.X * wtl + tr.X * wtr + bl.X * wbl + br.X * wbr) >> _QLERPSHIFTSQUARED);
            result.Y = (Byte)((tl.Y * wtl + tr.Y * wtr + bl.Y * wbl + br.Y * wbr) >> _QLERPSHIFTSQUARED);
            result.Z = (Byte)((tl.Z * wtl + tr.Z * wtr + bl.Z * wbl + br.Z * wbr) >> _QLERPSHIFTSQUARED);            
            
            return result;
        }

        #endregion
    }
}
