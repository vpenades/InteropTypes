using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropBitmaps
{
    partial class Pixel
    {
        partial struct BGR24 :
            ICopyValueTo<QVectorBGR>, ICopyValueTo<QVectorBGRP>,
            QVectorBGR.IFactory<BGR24>, QVectorBGRP.IFactory<BGR24>
        {
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGR value) { value.SetValue(this); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this);}

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGR24 CreateFrom(in QVectorBGR value) { return new BGR24(value.RQ8, value.GQ8, value.BQ8); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGR24 CreateFrom(in QVectorBGRP value) { return new BGR24(value.PreRQ8, value.PreGQ8, value.PreBQ8); }            
        }

        partial struct RGB24 :
            ICopyValueTo<QVectorBGR>, ICopyValueTo<QVectorBGRP>,
            QVectorBGR.IFactory<RGB24>, QVectorBGRP.IFactory<RGB24>
        {
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGR value) { value.SetValue(this); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGB24 CreateFrom(in QVectorBGR value) { return new RGB24(value.RQ8, value.GQ8, value.BQ8); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGB24 CreateFrom(in QVectorBGRP value) { return new RGB24(value.PreRQ8, value.PreGQ8, value.PreBQ8); }
        }

        partial struct BGRP32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<BGRP32>
        {
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRP32 CreateFrom(in QVectorBGRP value) { return new BGRP32(value.PreRQ8, value.PreGQ8, value.PreBQ8, value.AQ8); }            
        }

        partial struct RGBP32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<RGBP32>
        {
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGBP32 CreateFrom(in QVectorBGRP value) { return new RGBP32(value.PreRQ8, value.PreGQ8, value.PreBQ8, value.AQ8); }            
        }

        partial struct BGRA32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<BGRA32>
        {
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public BGRA32 CreateFrom(in QVectorBGRP value)
            {
                if (value.A == 0) return default;
                var rcpa = FixedMathCC8.ToReciprocalByte(value.A);
                return new BGRA32
                    (
                    FixedMathCC8.ToByte(value.R, rcpa),
                    FixedMathCC8.ToByte(value.G, rcpa),
                    FixedMathCC8.ToByte(value.B, rcpa),
                    value.AQ8);                
            }            
        }

        partial struct RGBA32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<RGBA32>
        {
            [MethodImpl(_PrivateConstants.Fastest)]
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public RGBA32 CreateFrom(in QVectorBGRP value)
            {
                if (value.A == 0) return default;
                var rcpa = FixedMathCC8.ToReciprocalByte(value.A);
                return new RGBA32
                    (
                    FixedMathCC8.ToByte(value.R, rcpa),
                    FixedMathCC8.ToByte(value.G, rcpa),
                    FixedMathCC8.ToByte(value.B, rcpa),
                    value.AQ8);
            }            
        }

        public struct QVectorBGR :
                    IValueSetter<RGB24>,
                    IValueSetter<BGR24>                    
        {
            #region data            

            public uint B;
            public uint G;
            public uint R;

            #endregion

            #region properties

            public byte RQ8
            {
                get => FixedMathCC8.ToByte(R);
                set => R = FixedMathCC8.FromByte(value);
            }

            public byte GQ8
            {
                get => FixedMathCC8.ToByte(G);
                set => G = FixedMathCC8.FromByte(value);
            }

            public byte BQ8
            {
                get => FixedMathCC8.ToByte(B);
                set => B = FixedMathCC8.FromByte(value);
            }

            #endregion

            #region API

            public void SetValue(RGB24 value)
            {
                B = FixedMathCC8.FromByte(value.B);
                G = FixedMathCC8.FromByte(value.G);
                R = FixedMathCC8.FromByte(value.R);
            }

            public void SetValue(BGR24 value)
            {
                B = FixedMathCC8.FromByte(value.B);
                G = FixedMathCC8.FromByte(value.G);
                R = FixedMathCC8.FromByte(value.R);
            }            

            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(BGR24 left, BGR24 right, uint rx, out QVectorBGR result)
            {
                const uint R256 = FixedMathCC8.UnitValue * 256;

                rx *= 256;
                rx /= 255;

                // calculate quantized weights
                var lx = R256 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == R256);
                result.R = (left.R * lx + right.R * rx) / R256;
                result.G = (left.G * lx + right.G * rx) / R256;
                result.B = (left.B * lx + right.B * rx) / R256;                
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(in QVectorBGR left, in QVectorBGR right, uint rx, out QVectorBGR result)
            {
                // calculate quantized weights
                var lx = FixedMathCC8.UnitValue - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == FixedMathCC8.UnitValue);
                result.R = (left.R * lx + right.R * rx) / FixedMathCC8.UnitValue;
                result.G = (left.G * lx + right.G * rx) / FixedMathCC8.UnitValue;
                result.B = (left.B * lx + right.B * rx) / FixedMathCC8.UnitValue;                
            }

            public void ApplyBlend(in QVectorBGRP src)
            {
                if (src.A == 0) return;
                var x = FixedMathCC8.UnitValue - src.A;
                R = (this.R * x + src.R) / FixedMathCC8.UnitValue;
                G = (this.G * x + src.G) / FixedMathCC8.UnitValue;
                B = (this.B * x + src.B) / FixedMathCC8.UnitValue;                
            }

            #endregion            

            #region nested types

            public interface IFactory<TDstPixel>
            {
                TDstPixel CreateFrom(in QVectorBGR value);
            }

            #endregion
        }

        [System.Diagnostics.DebuggerDisplay("R:{RQ8} G:{GQ8} B:{BQ8} A:{AQ8}")]
        public struct QVectorBGRP :
                    IValueSetter<RGB24>,
                    IValueSetter<BGR24>,
                    IValueSetter<RGBA32>,
                    IValueSetter<RGBP32>,
                    IValueSetter<BGRA32>,
                    IValueSetter<BGRP32>   
        {
            #region data            

            public uint B;
            public uint G;
            public uint R;
            public uint A;

            #endregion

            #region properties            

            public byte PreRQ8
            {
                get => FixedMathCC8.ToByte(R);
                set => R = FixedMathCC8.FromByte(value);
            }

            public byte PreGQ8
            {
                get => FixedMathCC8.ToByte(G);
                set => G = FixedMathCC8.FromByte(value);
            }

            public byte PreBQ8
            {
                get => FixedMathCC8.ToByte(B);
                set => B = FixedMathCC8.FromByte(value);
            }

            public byte AQ8
            {
                get => FixedMathCC8.ToByte(A);
                set => A = FixedMathCC8.FromByte(value);
            }

            public byte RQ8 => _UnpremultiplyTo255(R);

            public byte GQ8 => _UnpremultiplyTo255(G);

            public byte BQ8 => _UnpremultiplyTo255(B);

            #endregion

            #region ops

            public void ScaleAlpha(uint amount)
            {
                System.Diagnostics.Debug.Assert(amount <= FixedMathCC8.UnitValue);

                this.A = (this.A * amount) >> FixedMathCC8.UnitShift;
            }

            public static QVectorBGRP operator *(in QVectorBGRP a, uint amount)
            {
                System.Diagnostics.Debug.Assert(amount <= FixedMathCC8.UnitValue);

                var r = default(QVectorBGRP);
                r.B = (a.B * amount) >> FixedMathCC8.UnitShift;
                r.G = (a.G * amount) >> FixedMathCC8.UnitShift;
                r.R = (a.R * amount) >> FixedMathCC8.UnitShift;
                r.A = (a.A * amount) >> FixedMathCC8.UnitShift;
                return r;
            }

            public static QVectorBGRP operator *(in QVectorBGRP a, float b)
            {
                var bb = FixedMathCC8.FromFloat(b);

                var r = default(QVectorBGRP);
                r.B = (a.B * bb) >> FixedMathCC8.UnitShift;
                r.G = (a.G * bb) >> FixedMathCC8.UnitShift;
                r.R = (a.R * bb) >> FixedMathCC8.UnitShift;
                r.A = (a.A * bb) >> FixedMathCC8.UnitShift;
                return r;
            }

            public static QVectorBGRP operator * (in QVectorBGRP a, in QVectorBGRP b)
            {
                var r = default(QVectorBGRP);
                r.B = (a.B * b.B) >> FixedMathCC8.UnitShift;
                r.G = (a.G * b.G) >> FixedMathCC8.UnitShift;
                r.R = (a.R * b.R) >> FixedMathCC8.UnitShift;
                r.A = (a.A * b.A) >> FixedMathCC8.UnitShift;
                return r;
            }

            public static QVectorBGRP operator *(in QVectorBGR a, in QVectorBGRP b)
            {
                var r = default(QVectorBGRP);
                r.B = (a.B * b.B) >> FixedMathCC8.UnitShift;
                r.G = (a.G * b.G) >> FixedMathCC8.UnitShift;
                r.R = (a.R * b.R) >> FixedMathCC8.UnitShift;
                r.A = b.A;
                return r;
            }

            public static QVectorBGRP operator +(in QVectorBGRP a, in QVectorBGRP b)
            {
                var r = default(QVectorBGRP);
                r.B = Math.Min(FixedMathCC8.UnitShift, a.B + b.B);
                r.G = Math.Min(FixedMathCC8.UnitShift, a.G + b.G);
                r.R = Math.Min(FixedMathCC8.UnitShift, a.R + b.R);
                r.A = Math.Min(FixedMathCC8.UnitShift, a.A + b.A);
                return r;
            }

            #endregion

            #region API

            public static uint FromFloat(float opacity) { return FixedMathCC8.FromFloat(opacity); }            


            [MethodImpl(_PrivateConstants.Fastest)]
            private byte _UnpremultiplyTo255(uint component)
            {
                return A == 0
                    ? (byte)0
                    : (Byte)Math.Min(255, 256 * component / A);
            }

            public void SetValue(RGB24 value)
            {
                A = FixedMathCC8.UnitValue;
                B = FixedMathCC8.FromByte(value.B);
                G = FixedMathCC8.FromByte(value.G);
                R = FixedMathCC8.FromByte(value.R);
            }

            public void SetValue(BGR24 value)
            {
                A = FixedMathCC8.UnitValue;
                B = FixedMathCC8.FromByte(value.B);
                G = FixedMathCC8.FromByte(value.G);
                R = FixedMathCC8.FromByte(value.R);
            }            

            public void SetValue(RGBP32 value)
            {
                B = FixedMathCC8.FromByte(value.B);
                G = FixedMathCC8.FromByte(value.G);
                R = FixedMathCC8.FromByte(value.R);
                A = FixedMathCC8.FromByte(value.A);
            }

            public void SetValue(BGRP32 value)
            {
                B = FixedMathCC8.FromByte(value.B);
                G = FixedMathCC8.FromByte(value.G);
                R = FixedMathCC8.FromByte(value.R);
                A = FixedMathCC8.FromByte(value.A);
            }

            public void SetValue(RGBA32 value)
            {
                if (value.A == 0) { this = default; return; }
                A = FixedMathCC8.FromByte(value.A);
                B = (FixedMathCC8.FromByte(value.B) * A) >> FixedMathCC8.UnitShift;
                G = (FixedMathCC8.FromByte(value.G) * A) >> FixedMathCC8.UnitShift;
                R = (FixedMathCC8.FromByte(value.R) * A) >> FixedMathCC8.UnitShift;
            }

            public void SetValue(BGRA32 value)
            {
                if (value.A == 0) { this = default; return; }
                A = FixedMathCC8.FromByte(value.A);
                B = (FixedMathCC8.FromByte(value.B) * A) >> FixedMathCC8.UnitShift;
                G = (FixedMathCC8.FromByte(value.G) * A) >> FixedMathCC8.UnitShift;
                R = (FixedMathCC8.FromByte(value.R) * A) >> FixedMathCC8.UnitShift;
            }            


            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(BGRP32 left, BGRP32 right, uint rx, out QVectorBGRP result)
            {
                const uint R256 = FixedMathCC8.UnitValue * 256;

                rx *= 256;
                rx /= 255;

                // calculate quantized weights
                var lx = R256 - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == R256);
                result.R = (left.PreR * lx + right.PreR * rx) / R256;
                result.G = (left.PreG * lx + right.PreG * rx) / R256;
                result.B = (left.PreB * lx + right.PreB * rx) / R256;
                result.A = (left.A * lx + right.A * rx) / R256;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(in QVectorBGRP left, in QVectorBGRP right, uint rx, out QVectorBGRP result)
            {
                // calculate quantized weights
                var lx = FixedMathCC8.UnitValue - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == FixedMathCC8.UnitValue);
                result.R = (left.R * lx + right.R * rx) / FixedMathCC8.UnitValue;
                result.G = (left.G * lx + right.G * rx) / FixedMathCC8.UnitValue;
                result.B = (left.B * lx + right.B * rx) / FixedMathCC8.UnitValue;
                result.A = (left.A * lx + right.A * rx) / FixedMathCC8.UnitValue;
            }

            #endregion

            #region composition            

            [MethodImpl(_PrivateConstants.Fastest)]
            public void SourceOver(in QVectorBGRP src)
            {
                /*
                if (this.A == Quantized16384.UnitValue)
                {
                    var xa = Quantized16384.UnitValue - src.A;
                    this.R = (this.R * xa + src.R * src.A) >> Quantized16384.UnitShift;
                    this.G = (this.G * xa + src.G * src.A) >> Quantized16384.UnitShift;
                    this.B = (this.B * xa + src.B * src.A) >> Quantized16384.UnitShift;
                    return;
                }*/

                Over(src, src);
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void SourceAtop(in QVectorBGRP src) { Atop(src, src); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void AddOver(in QVectorBGRP src) { Over(src, this + src); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void AddAtop(in QVectorBGRP src) { Atop(src, this + src); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void MultiplyOver(in QVectorBGRP src) { Over(src, this * src); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void MultiplyAtop(in QVectorBGRP src) { Atop(src, this * src); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void Over(in QVectorBGRP src, in QVectorBGRP blend)
            {
                var wmix = (this.A * src.A) >> FixedMathCC8.UnitShift;
                var wdst = this.A - wmix;
                var wsrc = src.A - wmix;

                this.B = (this.B * wdst + src.B * wsrc + blend.B * wmix) >> FixedMathCC8.UnitShift;
                this.G = (this.G * wdst + src.G * wsrc + blend.G * wmix) >> FixedMathCC8.UnitShift;
                this.R = (this.R * wdst + src.R * wsrc + blend.R * wmix) >> FixedMathCC8.UnitShift;
                this.A = wdst + src.A;
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void Atop(in QVectorBGRP src, in QVectorBGRP blend)
            {
                var wmix = (this.A * src.A) >> FixedMathCC8.UnitShift;
                var wdst = this.A - wmix;

                this.B = this.B * wdst + blend.B * wmix;
                this.G = this.G * wdst + blend.G * wmix;
                this.R = this.R * wdst + blend.R * wmix;
            }

            #endregion

            #region nested types

            public interface IFactory<TDstPixel>
            {
                TDstPixel CreateFrom(in QVectorBGRP value);
            }

            #endregion
        }
    }
}
