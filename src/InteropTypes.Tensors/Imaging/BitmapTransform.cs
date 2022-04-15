using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors.Imaging
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    using SAMPLERXYZ24 = BitmapSampler<_PixelXYZ24>;
    using SAMPLERXYZW32 = BitmapSampler<uint>;
    using SAMPLERXYZ96F = BitmapSampler<System.Numerics.Vector3>;
    using SAMPLERXYZ128F = BitmapSampler<System.Numerics.Vector4>;

    using TENSOR32F = SpanTensor2<float>;
    using TENSORXYZ24 = SpanTensor2<_PixelXYZ24>;
    using TENSORXYZ96F = SpanTensor2<System.Numerics.Vector3>;
    using TENSORXYZ128F = SpanTensor2<System.Numerics.Vector4>;

    using TARGET32F = Span<float>;
    using TARGETXYZ24 = Span<_PixelXYZ24>;
    using TARGETXYZ96F = Span<System.Numerics.Vector3>;
    using TARGETXYZ128F = Span<System.Numerics.Vector4>;

    using SAMPLERITERATOR = _RowTransformIterator;

    public enum ColorEncoding
    {
        Undefined,
        X,XY,XYZ,XYZW,
        A, L, RGB, BGR, RGBA, BGRA, ARGB
    }

    public struct BitmapTransform
    {
        #region constructor

        public static implicit operator BitmapTransform(MultiplyAdd mad) { return new BitmapTransform(TRANSFORM.Identity, mad, true); }

        public static implicit operator BitmapTransform(TRANSFORM xform) { return new BitmapTransform(xform, MultiplyAdd.Identity, true); }

        public static implicit operator BitmapTransform((TRANSFORM x, MultiplyAdd ma) xform) { return new BitmapTransform(xform.x, xform.ma, true); }

        public static implicit operator BitmapTransform((TRANSFORM x, MultiplyAdd ma, bool ub) xform) { return new BitmapTransform(xform.x, xform.ma, xform.ub); }

        public BitmapTransform(System.Drawing.Size src, System.Drawing.Size dst)
        {
            var w = (float)src.Width / (float)dst.Width;
            var h = (float)src.Height / (float)dst.Height;

            Transform = TRANSFORM.CreateScale(w, h);
            UseBilinear = true;
            ColorTransform = MultiplyAdd.Identity;
        }

        public BitmapTransform(TRANSFORM xform, MultiplyAdd mad, bool bilinear)
        {
            Transform = xform;
            UseBilinear = bilinear;
            ColorTransform = mad;
        }        

        #endregion

        #region data

        public TRANSFORM Transform;
        public bool UseBilinear;
        
        public MultiplyAdd ColorTransform;        

        #endregion

        #region API        

        static bool _NeedsReverse(ColorEncoding a, ColorEncoding b)
        {
            if (a == ColorEncoding.BGR || a == ColorEncoding.BGRA)
            {
                return b == ColorEncoding.RGB || b == ColorEncoding.RGBA || b == ColorEncoding.ARGB;
            }

            if (b == ColorEncoding.BGR || b == ColorEncoding.BGRA)
            {
                return a == ColorEncoding.RGB || a == ColorEncoding.RGBA || a == ColorEncoding.ARGB;
            }

            return false;
        }

        public unsafe void FillPixels<TSrcPixel, TDstPixel>(SpanTensor2<TDstPixel> target, BitmapSampler<TSrcPixel> source, ColorEncoding targetEncoding)
            where TSrcPixel : unmanaged
            where TDstPixel : unmanaged
        {
            // TODO: if Transform is identity, do a plain copy

            var reverseRGB = _NeedsReverse(targetEncoding, source.Encoding);

            if (sizeof(TSrcPixel) == 3)
            {
                var sampler = source.Cast<_PixelXYZ24>();

                if (typeof(TDstPixel) == typeof(System.Numerics.Vector3)) // RGB 24 to RGB float
                {
                    var dstXYZ = target.Cast<System.Numerics.Vector3>();
                    _TransferPixels(sampler, dstXYZ, reverseRGB);
                    return;
                }

                if (typeof(TDstPixel) == typeof(float)) // RGB24 to gray
                {
                    var dstGray = target.Cast<float>();
                    _TransferPixels(sampler, dstGray);
                    return;
                }

                if (sizeof(TDstPixel) == 3) // RGB 24 to RGB 24
                {
                    var dstXYZ = target.Cast<_PixelXYZ24>();
                    _TransferPixels(sampler, dstXYZ);
                    return;
                }
            }

            if (sizeof(TSrcPixel) == 4)
            {
                var sampler = source.Cast<uint>();

                if (typeof(TDstPixel) == typeof(System.Numerics.Vector3)) // RGB 24 to RGB float
                {
                    var dstXYZ = target.Cast<System.Numerics.Vector3>();
                    _TransferPixels(sampler, dstXYZ, reverseRGB);
                    return;
                }

                if (typeof(TDstPixel) == typeof(float)) // RGB24 to gray
                {
                    var dstGray = target.Cast<float>();
                    _TransferPixels(sampler, dstGray);
                    return;
                }                
            }

            if (typeof(TSrcPixel) == typeof(System.Numerics.Vector3))
            {
                var sampler = source.Cast<System.Numerics.Vector3>();

                if (typeof(TDstPixel) == typeof(System.Numerics.Vector3)) // RGB float to RGB float
                {
                    var dstXYZ = target.Cast<System.Numerics.Vector3>();
                    _TransferPixels(sampler, dstXYZ, reverseRGB);
                    return;
                }
            }

            if (typeof(TSrcPixel) == typeof(System.Numerics.Vector4))
            {
                var sampler = source.Cast<System.Numerics.Vector4>();

                if (typeof(TDstPixel) == typeof(System.Numerics.Vector4)) // RGBA float to RGBA float
                {
                    var dstXYZW = target.Cast<System.Numerics.Vector4>();
                    _TransferPixels(sampler, dstXYZW);
                    return;
                }
            }

            _ThrowUnsupported(typeof(TDstPixel));
        }        

        public unsafe void FillPixels<TSrcPixel, TComponent>(SpanTensor2<TComponent> targetX, SpanTensor2<TComponent> targetY, SpanTensor2<TComponent> targetZ, BitmapSampler<TSrcPixel> source, ColorEncoding targetEncoding)
            where TSrcPixel : unmanaged
            where TComponent : unmanaged
        {
            // TODO: if Transform is identity, do a plain copy

            var reverseRGB = _NeedsReverse(targetEncoding, source.Encoding);

            if (sizeof(TSrcPixel) == 3)
            {
                var sampler = source.Cast<_PixelXYZ24>();

                if (typeof(TComponent) == typeof(float)) // RGB 24 to RGB split
                {
                    var dstX = targetX.Cast<float>();
                    var dstY = targetY.Cast<float>();
                    var dstZ = targetZ.Cast<float>();
                    _TransferPixels(sampler, dstX, dstY, dstZ, reverseRGB);
                    return;
                }
            }

            if (sizeof(TSrcPixel) == 4)
            {
                var sampler = source.Cast<uint>();

                if (typeof(TComponent) == typeof(float)) // RGB 24 to RGB split
                {
                    var dstX = targetX.Cast<float>();
                    var dstY = targetY.Cast<float>();
                    var dstZ = targetZ.Cast<float>();
                    _TransferPixels(sampler, dstX, dstY, dstZ, reverseRGB);
                    return;
                }
            }

            _ThrowUnsupported(typeof(TComponent));
        }        

        [System.Diagnostics.DebuggerStepThrough]
        static void _ThrowUnsupported(Type type)
        {
            throw new NotImplementedException(type.Name);
        }

        #endregion

        #region API - Core

        void _TransferPixels(SAMPLERXYZ24 srcSampler, TENSORXYZ24 dst)
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                var context = new _RowProcessorInfo<_PixelXYZ24,_PixelXYZ24>(this.UseBilinear, dstRow, srcSampler, ColorTransform);

                _rowProcessor(context, iter);
            }
        }          

        void _TransferPixels<TSrcPixel>(BitmapSampler<TSrcPixel> srcSampler, TENSOR32F dst)
            where TSrcPixel : unmanaged
        {
            SAMPLERITERATOR iter;

            var colorXform = ColorTransform.ConcatMul(0.2989f, 0.5870f, 0.1140f); // RGB to Gray

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);                
                var context = new _RowProcessorInfo<TSrcPixel, float>(this.UseBilinear, dstRow, srcSampler, colorXform);

                _rowProcessor(context, iter);
            }
        }        

        void _TransferPixels<TSrcPixel>(BitmapSampler<TSrcPixel> srcSampler, TENSORXYZ96F dst, bool reverse)
            where TSrcPixel : unmanaged
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                var context = new _RowProcessorInfo<TSrcPixel, System.Numerics.Vector3>(this.UseBilinear, dstRow, srcSampler, ColorTransform);

                if (reverse) _rowProcessorReverse(context, iter);
                else _rowProcessorForward(context, iter);
            }
        }

        void _TransferPixels<TSrcPixel>(BitmapSampler<TSrcPixel> srcSampler, TENSORXYZ128F dst)
            where TSrcPixel : unmanaged
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dst.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRow = dst[dy].Span.Slice(0, dst.BitmapSize.Width);

                var context = new _RowProcessorInfo<TSrcPixel, System.Numerics.Vector4>(this.UseBilinear, dstRow, srcSampler, ColorTransform);

                _rowProcessor(context, iter);
            }
        }        

        void _TransferPixels<TSrcPixel>(BitmapSampler<TSrcPixel> srcSampler, TENSOR32F dstX, TENSOR32F dstY, TENSOR32F dstZ, bool reverse)
            where TSrcPixel:unmanaged
        {
            SAMPLERITERATOR iter;

            for (int dy = 0; dy < dstX.BitmapSize.Height; ++dy)
            {
                iter = new SAMPLERITERATOR(0, dy, this.Transform);

                var dstRowX = dstX[dy].Span.Slice(0, dstX.BitmapSize.Width);
                var dstRowY = dstY[dy].Span.Slice(0, dstY.BitmapSize.Width);
                var dstRowZ = dstZ[dy].Span.Slice(0, dstZ.BitmapSize.Width);

                if (reverse)
                {
                    var tmp = dstRowX;
                    dstRowX = dstRowZ;
                    dstRowZ = tmp;
                }

                _rowProcessorSplit(dstRowX, dstRowY, dstRowZ, srcSampler, iter, this.ColorTransform, this.UseBilinear);
            }
        }

        // RGB24 to RGB24
        static void _rowProcessor(_RowProcessorInfo<_PixelXYZ24,_PixelXYZ24> context, SAMPLERITERATOR srcIterator)
        {
            var mul = new _PixelXYZ24(1, 1, 1);
            var add = new _PixelXYZ24(0, 0, 0);

            if (context.UseBilinear)
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    context.Target[i] = context.Source.GetSample(sx, sy, rx, ry, context.ColorTransform);
                }
            }
            else
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);
                    context.Target[i] = context.Source.GetPixel(sx, sy);
                }
            }            
        }

        // Any to float gray
        static void _rowProcessor<TSrcPixel>(_RowProcessorInfo<TSrcPixel, float> context, SAMPLERITERATOR srcIterator)
            where TSrcPixel : unmanaged
        {
            if (context.UseBilinear)
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    var sample = context.Source.GetVector3Sample(sx, sy, rx, ry);
                    sample = context.ColorTransform.Transform(sample);
                    context.Target[i] = sample.X + sample.Y + sample.Z;
                }
            }
            else
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);
                    var sample = context.Source.GetVector3Pixel(sx, sy);
                    sample = context.ColorTransform.Transform(sample);
                    context.Target[i] = sample.X + sample.Y + sample.Z;
                }
            }
        }        

        // Any to Vector3
        static void _rowProcessorForward<TSrcPixel>(_RowProcessorInfo<TSrcPixel, System.Numerics.Vector3> context, SAMPLERITERATOR srcIterator)
            where TSrcPixel: unmanaged
        {
            if (context.UseBilinear)
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    context.Target[i] = context.Source.GetVector3Sample(sx, sy, rx, ry);
                    context.ColorTransform.Transform(ref context.Target[i]);
                }
            }
            else
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);
                    context.Source.GetVector3Pixel(sx, sy, out context.Target[i]);
                    context.ColorTransform.Transform(ref context.Target[i]);
                }
            }
        }

        static void _rowProcessorReverse<TSrcPixel>(_RowProcessorInfo<TSrcPixel, System.Numerics.Vector3> context, SAMPLERITERATOR srcIterator)
            where TSrcPixel : unmanaged
        {
            var xtarget = System.Runtime.InteropServices.MemoryMarshal.Cast<System.Numerics.Vector3,float>(context.Target);

            if (context.UseBilinear)
            {
                for (int i = 0; i < xtarget.Length; i+=3)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    var sample = context.Source.GetVector3Sample(sx, sy, rx, ry);
                    context.ColorTransform.Transform(ref sample);
                    xtarget[i + 0] = sample.Z;
                    xtarget[i + 1] = sample.Y;
                    xtarget[i + 2] = sample.X;
                }
            }
            else
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);
                    var sample = context.Source.GetVector3Pixel(sx, sy);
                    context.ColorTransform.Transform(ref sample);
                    xtarget[i + 0] = sample.Z;
                    xtarget[i + 1] = sample.Y;
                    xtarget[i + 2] = sample.X;
                }
            }
        }

        // Any to Vector4
        static void _rowProcessor<TSrcPixel>(_RowProcessorInfo<TSrcPixel, System.Numerics.Vector4> context, SAMPLERITERATOR srcIterator)
            where TSrcPixel : unmanaged
        {
            if (context.UseBilinear)
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    context.Target[i] = context.Source.GetVector4Sample(sx, sy, rx, ry);
                    context.ColorTransform.Transform(ref context.Target[i]);
                }
            }
            else
            {
                for (int i = 0; i < context.Target.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);
                    context.Target[i] = context.Source.GetVector4Pixel(sx, sy);
                    context.ColorTransform.Transform(ref context.Target[i]);
                }
            }
        }

        static void _rowProcessorSplit<TSrcPixel>(TARGET32F rowDstX, TARGET32F rowDstY, TARGET32F rowDstZ, BitmapSampler<TSrcPixel> rowSrc, SAMPLERITERATOR srcIterator, MultiplyAdd mad, bool useBilinear)
            where TSrcPixel:unmanaged
        {
            if (useBilinear)
            {
                for (int i = 0; i < rowDstX.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy, out int rx, out int ry);
                    var sample = rowSrc.GetVector3Sample(sx, sy, rx, ry);
                    sample = mad.Transform(sample);
                    rowDstX[i] = sample.X;
                    rowDstY[i] = sample.Y;
                    rowDstZ[i] = sample.Z;
                }
            }
            else
            {
                for (int i = 0; i < rowDstX.Length; ++i)
                {
                    srcIterator.MoveNext(out int sx, out int sy);
                    var sample = rowSrc.GetVector3Pixel(sx, sy);
                    sample = mad.Transform(sample);
                    rowDstX[i] = sample.X;
                    rowDstY[i] = sample.Y;
                    rowDstZ[i] = sample.Z;
                }
            }
        }     

        #endregion
    }

    readonly ref struct _RowProcessorInfo<TSrcPixel,TDstPixel>
        where TSrcPixel: unmanaged
        where TDstPixel: unmanaged
    {
        public _RowProcessorInfo(bool bl, Span<TDstPixel> target, BitmapSampler<TSrcPixel> source, MultiplyAdd mad)
        {
            UseBilinear = bl;
            Target = target;
            Source = source;
            ColorTransform = mad;
        }
        
        public readonly Span<TDstPixel> Target;
        public readonly BitmapSampler<TSrcPixel> Source;
        public readonly MultiplyAdd ColorTransform;
        public readonly bool UseBilinear;
    }
}
