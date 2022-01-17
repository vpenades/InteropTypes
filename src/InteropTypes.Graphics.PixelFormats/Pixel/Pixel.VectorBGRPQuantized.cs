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
            public void CopyTo(ref QVectorBGR value) { value.SetValue(this); }
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this);}
            public BGR24 CreateFrom(in QVectorBGR value) { return new BGR24(value.RQ8, value.GQ8, value.BQ8); }
            public BGR24 CreateFrom(in QVectorBGRP value) { return new BGR24(value.PreRQ8, value.PreGQ8, value.PreBQ8); }

            partial struct Writeable : ICopyValueTo<QVectorBGR>, ICopyValueTo<QVectorBGRP>
            {
                public void CopyTo(ref QVectorBGR value) { value.SetValue(this); }
                public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }
            }
        }

        partial struct RGB24 :
            ICopyValueTo<QVectorBGR>, ICopyValueTo<QVectorBGRP>,
            QVectorBGR.IFactory<RGB24>, QVectorBGRP.IFactory<RGB24>
        {
            public void CopyTo(ref QVectorBGR value) { value.SetValue(this); }
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }
            public RGB24 CreateFrom(in QVectorBGR value) { return new RGB24(value.RQ8, value.GQ8, value.BQ8); }
            public RGB24 CreateFrom(in QVectorBGRP value) { return new RGB24(value.PreRQ8, value.PreGQ8, value.PreBQ8); }

            partial struct Writeable : ICopyValueTo<QVectorBGR>, ICopyValueTo<QVectorBGRP>
            {
                public void CopyTo(ref QVectorBGR value) { value.SetValue(this); }
                public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }
            }
        }

        partial struct BGRP32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<BGRP32>
        {
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            public BGRP32 CreateFrom(in QVectorBGRP value) { return new BGRP32(value.PreRQ8, value.PreGQ8, value.PreBQ8, value.AQ8); }

            partial struct Writeable : ICopyValueTo<QVectorBGRP>
            {
                public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }
            }
        }

        partial struct RGBP32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<RGBP32>
        {
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            public RGBP32 CreateFrom(in QVectorBGRP value) { return new RGBP32(value.PreRQ8, value.PreGQ8, value.PreBQ8, value.AQ8); }

            partial struct Writeable : ICopyValueTo<QVectorBGRP>
            {
                public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }
            }
        }

        partial struct BGRA32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<BGRA32>
        {
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            public BGRA32 CreateFrom(in QVectorBGRP value)
            {
                if (value.A == 0) return default;
                var rcpa = FixedMath.GetUnboundedReciprocal8(value.A);
                return new BGRA32
                    (
                    FixedMath.To255(value.R, rcpa),
                    FixedMath.To255(value.G, rcpa),
                    FixedMath.To255(value.B, rcpa),
                    value.AQ8);                
            }

            partial struct Writeable : ICopyValueTo<QVectorBGRP>
            {
                public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }
            }
        }

        partial struct RGBA32 : ICopyValueTo<QVectorBGRP>, QVectorBGRP.IFactory<RGBA32>
        {
            public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }

            public RGBA32 CreateFrom(in QVectorBGRP value)
            {
                if (value.A == 0) return default;
                var rcpa = FixedMath.GetUnboundedReciprocal8(value.A);
                return new RGBA32
                    (
                    FixedMath.To255(value.R, rcpa),
                    FixedMath.To255(value.G, rcpa),
                    FixedMath.To255(value.B, rcpa),
                    value.AQ8);
            }

            partial struct Writeable : ICopyValueTo<QVectorBGRP>
            {
                public void CopyTo(ref QVectorBGRP value) { value.SetValue(this); }
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
                get => FixedMath.To255(R);
                set => R = FixedMath.From255(value);
            }

            public byte GQ8
            {
                get => FixedMath.To255(G);
                set => G = FixedMath.From255(value);
            }

            public byte BQ8
            {
                get => FixedMath.To255(B);
                set => B = FixedMath.From255(value);
            }

            #endregion

            #region API

            public void SetValue(RGB24 value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }

            public void SetValue(BGR24 value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }

            public void SetValue(RGB24.Writeable value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }

            public void SetValue(BGR24.Writeable value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(BGR24 left, BGR24 right, uint rx, out QVectorBGR result)
            {
                const uint R256 = FixedMath.UnitValue * 256;

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
                var lx = FixedMath.UnitValue - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == FixedMath.UnitValue);
                result.R = (left.R * lx + right.R * rx) / FixedMath.UnitValue;
                result.G = (left.G * lx + right.G * rx) / FixedMath.UnitValue;
                result.B = (left.B * lx + right.B * rx) / FixedMath.UnitValue;                
            }

            public void ApplyBlend(in QVectorBGRP src)
            {
                if (src.A == 0) return;
                var x = FixedMath.UnitValue - src.A;
                R = (this.R * x + src.R) / FixedMath.UnitValue;
                G = (this.G * x + src.G) / FixedMath.UnitValue;
                B = (this.B * x + src.B) / FixedMath.UnitValue;                
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
                get => FixedMath.To255(R);
                set => R = FixedMath.From255(value);
            }

            public byte PreGQ8
            {
                get => FixedMath.To255(G);
                set => G = FixedMath.From255(value);
            }

            public byte PreBQ8
            {
                get => FixedMath.To255(B);
                set => B = FixedMath.From255(value);
            }

            public byte AQ8
            {
                get => FixedMath.To255(A);
                set => A = FixedMath.From255(value);
            }

            public byte RQ8 => UnpremultiplyTo255(R);

            public byte GQ8 => UnpremultiplyTo255(G);

            public byte BQ8 => UnpremultiplyTo255(B);

            #endregion

            #region ops

            public static QVectorBGRP operator * (in QVectorBGRP a, in QVectorBGRP b)
            {
                var r = default(QVectorBGRP);
                r.B = (a.B * b.B) >> FixedMath.UnitShift;
                r.G = (a.G * b.G) >> FixedMath.UnitShift;
                r.R = (a.R * b.R) >> FixedMath.UnitShift;
                r.A = (a.A * b.A) >> FixedMath.UnitShift;
                return r;
            }

            public static QVectorBGRP operator *(in QVectorBGR a, in QVectorBGRP b)
            {
                var r = default(QVectorBGRP);
                r.B = (a.B * b.B) >> FixedMath.UnitShift;
                r.G = (a.G * b.G) >> FixedMath.UnitShift;
                r.R = (a.R * b.R) >> FixedMath.UnitShift;
                r.A = b.A;
                return r;
            }

            public static QVectorBGRP operator +(in QVectorBGRP a, in QVectorBGRP b)
            {
                var r = default(QVectorBGRP);
                r.B = Math.Min(FixedMath.UnitShift, a.B + b.B);
                r.G = Math.Min(FixedMath.UnitShift, a.G + b.G);
                r.R = Math.Min(FixedMath.UnitShift, a.R + b.R);
                r.A = Math.Min(FixedMath.UnitShift, a.A + b.A);
                return r;
            }

            #endregion

            #region API

            public static uint FromFloat(float opacity) { return FixedMath.FromFloat(opacity); }


            [MethodImpl(_PrivateConstants.Fastest)]
            private byte UnpremultiplyTo255(uint component)
            {
                return A == 0
                    ? (byte)0
                    : FixedMath.To255(component, FixedMath.GetUnboundedReciprocal8(A));
            }

            public void SetValue(RGB24 value)
            {
                A = FixedMath.UnitValue;
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }

            public void SetValue(BGR24 value)
            {
                A = FixedMath.UnitValue;
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }            

            public void SetValue(RGBP32 value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
                A = FixedMath.From255(value.A);
            }

            public void SetValue(BGRP32 value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
                A = FixedMath.From255(value.A);
            }

            public void SetValue(RGBA32 value)
            {
                if (value.A == 0) { this = default; return; }
                A = FixedMath.From255(value.A);
                B = FixedMath.From255(value.B) * A >> FixedMath.UnitShift;
                G = FixedMath.From255(value.G) * A >> FixedMath.UnitShift;
                R = FixedMath.From255(value.R) * A >> FixedMath.UnitShift;
            }

            public void SetValue(BGRA32 value)
            {
                if (value.A == 0) { this = default; return; }
                A = FixedMath.From255(value.A);                
                B = FixedMath.From255(value.B) * A >> FixedMath.UnitShift;
                G = FixedMath.From255(value.G) * A >> FixedMath.UnitShift;
                R = FixedMath.From255(value.R) * A >> FixedMath.UnitShift;
            }

            public void SetValue(RGB24.Writeable value)
            {
                A = FixedMath.UnitValue;
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }

            public void SetValue(BGR24.Writeable value)
            {
                A = FixedMath.UnitValue;
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
            }

            public void SetValue(RGBP32.Writeable value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
                A = FixedMath.From255(value.A);
            }

            public void SetValue(BGRP32.Writeable value)
            {
                B = FixedMath.From255(value.B);
                G = FixedMath.From255(value.G);
                R = FixedMath.From255(value.R);
                A = FixedMath.From255(value.A);
            }

            public void SetValue(RGBA32.Writeable value)
            {
                if (value.A == 0) { this = default; return; }
                A = FixedMath.From255(value.A);
                B = FixedMath.From255(value.B) * A >> FixedMath.UnitShift;
                G = FixedMath.From255(value.G) * A >> FixedMath.UnitShift;
                R = FixedMath.From255(value.R) * A >> FixedMath.UnitShift;
            }

            public void SetValue(BGRA32.Writeable value)
            {
                if (value.A == 0) { this = default; return; }
                A = FixedMath.From255(value.A);
                B = FixedMath.From255(value.B) * A >> FixedMath.UnitShift;
                G = FixedMath.From255(value.G) * A >> FixedMath.UnitShift;
                R = FixedMath.From255(value.R) * A >> FixedMath.UnitShift;
            }


            [MethodImpl(_PrivateConstants.Fastest)]
            public static void Lerp(BGRP32 left, BGRP32 right, uint rx, out QVectorBGRP result)
            {
                const uint R256 = FixedMath.UnitValue * 256;

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
                var lx = FixedMath.UnitValue - rx;
                System.Diagnostics.Debug.Assert((lx + rx) == FixedMath.UnitValue);
                result.R = (left.R * lx + right.R * rx) / FixedMath.UnitValue;
                result.G = (left.G * lx + right.G * rx) / FixedMath.UnitValue;
                result.B = (left.B * lx + right.B * rx) / FixedMath.UnitValue;
                result.A = (left.A * lx + right.A * rx) / FixedMath.UnitValue;
            }
            
            #endregion

            #region composition            

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

            public void SourceAtop(in QVectorBGRP src) { Atop(src, src); }
            public void AddOver(in QVectorBGRP src) { Over(src, this + src); }
            public void AddAtop(in QVectorBGRP src) { Atop(src, this + src); }
            public void MultiplyOver(in QVectorBGRP src) { Over(src, this * src); }
            public void MultiplyAtop(in QVectorBGRP src) { Atop(src, this * src); }

            public void Over(in QVectorBGRP src, in QVectorBGRP blend)
            {
                var wmix = (this.A * src.A) >> FixedMath.UnitShift;
                var wdst = this.A - wmix;
                var wsrc = src.A - wmix;

                this.B = (this.B * wdst + src.B * wsrc + blend.B * wmix) >> FixedMath.UnitShift;
                this.G = (this.G * wdst + src.G * wsrc + blend.G * wmix) >> FixedMath.UnitShift;
                this.R = (this.R * wdst + src.R * wsrc + blend.R * wmix) >> FixedMath.UnitShift;
                this.A = wdst + src.A;
            }

            public void Atop(in QVectorBGRP src, in QVectorBGRP blend)
            {
                var wmix = (this.A * src.A) >> FixedMath.UnitShift;
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
