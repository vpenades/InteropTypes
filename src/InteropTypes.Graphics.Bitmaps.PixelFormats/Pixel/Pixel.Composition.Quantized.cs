using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public interface IQuantizedCompositionSetter<TSrcPixel>
            where TSrcPixel: unmanaged
        {
            void SetSourceOver(TSrcPixel src, int opacity256);
            void AddSourceOver(TSrcPixel src, int opacity256);
            void MulSourceOver(BGRP32 src, int opacity256);
        }

        partial struct BGRP32 : IQuantizedCompositionSetter<BGRP32>
        {
            #region COMPOSITION

            public static BGRP32 operator *(BGRP32 a, BGRP32 b)
            {
                #if NET6_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif

                result.PreR = (Byte)((a.PreR * b.PreR * 258) >> 16);
                result.PreG = (Byte)((a.PreG * b.PreG * 258) >> 16);
                result.PreB = (Byte)((a.PreB * b.PreB * 258) >> 16);
                result.A = (Byte)((a.A * b.A * 258) >> 16);

                return result;
            }

            public static BGRP32 operator +(BGRP32 a, BGRP32 b)
            {
                #if NET6_0_OR_GREATER
                Unsafe.SkipInit<BGRP32>(out var result);
                #else
                var result = default(BGRP32);
                #endif                

                result.PreR = (Byte)Math.Min(255, a.PreR + b.PreR);
                result.PreG = (Byte)Math.Min(255, a.PreG + b.PreG);
                result.PreB = (Byte)Math.Min(255, a.PreB + b.PreB);
                result.A = (Byte)Math.Min(255, a.A + b.A);

                return result;
            }            

            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetSourceOver(BGRP32 src, int opacity256) { SetValueOver(src, src, opacity256); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void AddSourceOver(BGRP32 src, int opacity256) { SetValueOver(src, src + src, opacity256); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void MulSourceOver(BGRP32 src, int opacity256) { SetValueOver(src, src * src, opacity256); }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValueOver(BGRP32 src, BGRP32 blend)
            {
                var wmix = (this.A * src.A) >> 8;
                var wsrc = src.A - wmix;
                var wdst = 256 - wsrc - wmix; // technically should be (dst.A - wmix), but we subtract from 256 to compensate weights.

                System.Diagnostics.Debug.Assert(wmix + wdst + wsrc == 256);

                this.PreB = (Byte)((this.PreB * wdst + src.PreB * wsrc + blend.PreB * wmix) >> 8);
                this.PreG = (Byte)((this.PreG * wdst + src.PreG * wsrc + blend.PreG * wmix) >> 8);
                this.PreR = (Byte)((this.PreR * wdst + src.PreR * wsrc + blend.PreR * wmix) >> 8);
                this.A = (Byte)(((wdst + src.A) * 255) >> 8);
            }

            [MethodImpl(_PrivateConstants.Fastest)]
            public void SetValueOver(BGRP32 src, BGRP32 blend, int opacity256)
            {
                var a = (src.A * opacity256) >> 8;
                var wmix = (this.A * a) >> 8;
                var wsrc = a - wmix;
                var wdst = 256 - wsrc - wmix; // technically should be (dst.A - wmix), but we subtract from 256 to compensate weights.

                System.Diagnostics.Debug.Assert(wmix + wdst + wsrc == 256);

                this.PreB = (Byte)((this.PreB * wdst + src.PreB * wsrc + blend.PreB * wmix) >> 8);
                this.PreG = (Byte)((this.PreG * wdst + src.PreG * wsrc + blend.PreG * wmix) >> 8);
                this.PreR = (Byte)((this.PreR * wdst + src.PreR * wsrc + blend.PreR * wmix) >> 8);
                this.A = (Byte)(((wdst + a) * 255) >> 8);
            }

            #endregion
        }
    }
}
