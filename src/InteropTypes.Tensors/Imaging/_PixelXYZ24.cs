using System;
using System.Collections.Generic;
using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Text;

using InteropTypes.Tensors.Imaging;

namespace InteropTypes.Tensors
{

    interface IPixelNearest<TSrcPixel>
    {        
        void SetFromNearest(TSrcPixel pixel);
    }

    interface IPixelBiLerp<TSrcPixel> // this can be implemented by an intermediate format, so we could have an integer and a floating implementation
    {        
        /// <param name="rx">0 to 1024</param>
        /// <param name="by">0 to 1024</param>
        void SetFromBiLerp(TSrcPixel tl, TSrcPixel tr, TSrcPixel bl, TSrcPixel br, uint rx, uint by);
    }


    interface IPixelOps<TPixel>
        where TPixel : IPixelOps<TPixel>
    {
        void Apply(MultiplyAdd mad);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tl"></param>
        /// <param name="tr"></param>
        /// <param name="bl"></param>
        /// <param name="br"></param>
        /// <param name="rx">0 to 1024</param>
        /// <param name="by">0 to 1024</param>
        void SetFromBiLerp(TPixel tl, TPixel tr, TPixel bl, TPixel br, uint rx, uint by);
    }

    /// <summary>
    /// Represents a 24 bit pixel components
    /// </summary>
    /// <remarks>The R,G,B component order is undefined.</remarks>
    [System.Diagnostics.DebuggerDisplay("{X} {Y} {Z}")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    struct _PixelXYZ24 : IPixelOps<_PixelXYZ24>
    {
        #region constructor

        public static _PixelXYZ24 FromNormalizedVector4(System.Numerics.Vector4 v)
        {
            v *= 255f;
            return new _PixelXYZ24((byte)v.X, (byte)v.Y, (byte)v.Z);
        }

        public static _PixelXYZ24 FromNormalizedVector4(System.Numerics.Vector4 v, ColorEncoding encoding)
        {
            v *= 255f;
            switch(encoding)
            {
                case ColorEncoding.RGB: return new _PixelXYZ24((byte)v.X, (byte)v.Y, (byte)v.Z);
                case ColorEncoding.BGR: return new _PixelXYZ24((byte)v.Z, (byte)v.Y, (byte)v.X);
                default: throw new NotImplementedException();
            }
        }

        public _PixelXYZ24 GetReverse()
        {
            return new _PixelXYZ24(Z, Y, X);
        }

        public _PixelXYZ24(Byte x, Byte y, Byte z)
        {
            X = x; Y = y; Z = z;
        }

        #endregion

        #region data

        public Byte X;
        public Byte Y;
        public Byte Z;
        public Byte Gray => (Byte)((int)X + (int)Y + (int)Z);        

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

        public void SetFromBiLerp(_PixelXYZ24 tl, _PixelXYZ24 tr, _PixelXYZ24 bl, _PixelXYZ24 br, uint rx, uint by)
        {
            this = BiLerp(tl, tr, bl, br, rx, by);
        }
        public void Apply(MultiplyAdd mad)
        {
            if (!mad.IsIdentity) throw new NotImplementedException();   
        }

        public System.Numerics.Vector4 ToNormalizedVector4()
        {
            return new Vector4(X, Y, Z, 255f) / 255f;
        }
        public System.Numerics.Vector4 ToNormalizedVector4(ColorEncoding encoding)
        {
            switch(encoding)
            {
                case ColorEncoding.RGB: return new Vector4(X, Y, Z, 255f) / 255f;
                case ColorEncoding.BGR: return new Vector4(Z, Y, X, 255f) / 255f;
                default: throw new NotSupportedException();
            }            
        }

        public T Cast<T>()
        {
            return Unsafe.As<_PixelXYZ24, T>(ref this);
        }

        

        #endregion
    }


    


    interface IConvertiblePixelContract<TPixel>
        where TPixel:unmanaged
    {
        TPixel Convert(); // we should be able to apply to MultipleAdd in between        
    }


    


    


}
