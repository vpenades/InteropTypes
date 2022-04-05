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
            where TDstPixel : unmanaged, IValueSetter<RGBA128F>
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                var v = System.Numerics.Vector4.Lerp(left[i].To<RGBA128F>().RGBA, right[i].To<RGBA128F>().RGBA, amount);
                dst[i].SetValue(new RGBA128F(v));
            }
        }

        public static void LerpArray<TDstPixel>(ReadOnlySpan<System.Numerics.Vector3> left, ReadOnlySpan<System.Numerics.Vector3> right, float amount, Span<TDstPixel> dst)
            where TDstPixel : unmanaged, IValueSetter<RGB96F>
        {
            ref var lftP = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(left);
            ref var rgtP = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(right);
            ref var dstP = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);
            var dstL = dst.Length;

            while(dstL-- > 0)
            {
                var v = System.Numerics.Vector3.Lerp(lftP, rgtP, amount);
                dstP.SetValue(new RGB96F(v));

                dstP = ref Unsafe.Add(ref dstP, 1);
                lftP = ref Unsafe.Add(ref lftP, 1);
                rgtP = ref Unsafe.Add(ref rgtP, 1);
            }
        }        
    }
}
