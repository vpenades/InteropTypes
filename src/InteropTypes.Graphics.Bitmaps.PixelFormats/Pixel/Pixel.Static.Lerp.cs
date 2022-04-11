using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    partial class Pixel
    {
        public static void LerpArray<TSrcPixel, TDstPixel>(ReadOnlySpan<TSrcPixel> left, ReadOnlySpan<TSrcPixel> right, float amount, Span<TDstPixel> dst)
            where TSrcPixel : unmanaged, IConvertTo
            where TDstPixel : unmanaged
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector4.Lerp(left[i].To<RGBA128F>().RGBA, right[i].To<RGBA128F>().RGBA, amount);
                dst[i] = new RGBA128F(v).To<TDstPixel>();
            }
        }

        public static void LerpArray<TDstPixel>(ReadOnlySpan<System.Numerics.Vector3> left, ReadOnlySpan<System.Numerics.Vector3> right, float amount, Span<TDstPixel> dst)
            where TDstPixel : unmanaged
        {
            ref var lftP = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(left);
            ref var rgtP = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(right);
            ref var dstP = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
            var dstL = dst.Length;

            while (dstL-- > 0)
            {
                var v = System.Numerics.Vector3.Lerp(lftP, rgtP, amount);

                Unsafe.As<System.Numerics.Vector3, RGB96F>(ref v).CopyTo(ref dstP);

                dstP = ref Unsafe.Add(ref dstP, 1);
                lftP = ref Unsafe.Add(ref lftP, 1);
                rgtP = ref Unsafe.Add(ref rgtP, 1);
            }
        }


        [MethodImpl(_PrivateConstants.Fastest)]
        public static TDstPixel LerpQuantized<TSrcPixel,TDstPixel>(in TSrcPixel left, in TSrcPixel right, uint rx)
            where TSrcPixel: unmanaged
            where TDstPixel: unmanaged
        {
            // TODO: we could use some common traits here

            return BGRP32.Lerp(BGRP32.From(left), BGRP32.From(right), rx).To<TDstPixel>();
        }
        
        [MethodImpl(_PrivateConstants.Fastest)]
        public static TDstPixel LerpQuantized<TSrcPixel, TDstPixel>(in TSrcPixel tl, in TSrcPixel tr, in TSrcPixel bl, in TSrcPixel br, uint rx, uint by)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            // TODO: we could use some common traits here

            return BGRP32.Lerp(BGRP32.From(tl), BGRP32.From(tr), BGRP32.From(bl), BGRP32.From(br), rx, by).To<TDstPixel>();
        }        
    }
}