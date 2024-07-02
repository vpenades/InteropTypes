using System;
using System.Collections.Generic;
using System.Numerics;
using System.Numerics.Tensors;
using System.Runtime.InteropServices;
using System.Text;

using MMARSHAL = System.Runtime.InteropServices.MemoryMarshal;
using XYZ = System.Numerics.Vector3;
using XYZW = System.Numerics.Vector4;

namespace InteropTypes.Tensors.Imaging
{
    public enum ColorEncoding
    {
        Undefined,
        A, L, RGB, BGR, RGBA, BGRA, ARGB
    }

    internal static class _ColorEncodingExtensions
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static int GetChannelCount(this ColorEncoding encoding)
        {
            switch (encoding)
            {
                case ColorEncoding.A:
                case ColorEncoding.L:
                    return 1;
                case ColorEncoding.RGB:
                case ColorEncoding.BGR:
                    return 3;
                case ColorEncoding.RGBA:
                case ColorEncoding.BGRA:
                case ColorEncoding.ARGB:
                    return 4;

                default: throw new NotImplementedException();
            }
        }

        public static bool NeedsReverseRGB(this ColorEncoding left, ColorEncoding right)
        {
            if (left == ColorEncoding.BGR || left == ColorEncoding.BGRA)
            {
                return right == ColorEncoding.RGB
                    || right == ColorEncoding.RGBA
                    || right == ColorEncoding.ARGB;
            }

            if (right == ColorEncoding.BGR || right == ColorEncoding.BGRA)
            {
                return left == ColorEncoding.RGB
                    || left == ColorEncoding.RGBA
                    || left == ColorEncoding.ARGB;
            }

            return false;
        }

        public static unsafe XYZW ToScaledPixel<TElement>(this ColorEncoding srcEncoding, Span<TElement> srcPixel)
            where TElement : unmanaged, IConvertible
        {
            if (typeof(TElement) == typeof(byte))
            {
                var pixel = MMARSHAL.Cast<TElement, Byte>(srcPixel);

                switch (srcEncoding)
                {
                    case ColorEncoding.A: return new XYZW(255f, 255f, 255f, pixel[0]) / 255f;
                    case ColorEncoding.L: return new XYZW(pixel[0], pixel[0], pixel[0], 255f) / 255f;
                    case ColorEncoding.RGB: return new XYZW(pixel[0], pixel[1], pixel[2], 255) / 255f;
                    case ColorEncoding.BGR: return new XYZW(pixel[2], pixel[1], pixel[0], 255) / 255f;
                    case ColorEncoding.RGBA: return new XYZW(pixel[0], pixel[1], pixel[2], pixel[3]) / 255f;
                    case ColorEncoding.BGRA: return new XYZW(pixel[2], pixel[1], pixel[0], pixel[3]) / 255f;
                    case ColorEncoding.ARGB: return new XYZW(pixel[3], pixel[2], pixel[1], pixel[0]) / 255f;
                    default: throw new NotImplementedException();
                }
            }

            if (typeof(TElement) == typeof(float))
            {
                var pixel = MMARSHAL.Cast<TElement, float>(srcPixel);

                switch (srcEncoding)
                {
                    case ColorEncoding.A: return new XYZW(1, 1, 1, pixel[0]);
                    case ColorEncoding.L: return new XYZW(pixel[0], pixel[0], pixel[0], 1);
                    case ColorEncoding.RGB: return new XYZW(pixel[0], pixel[1], pixel[2], 1);
                    case ColorEncoding.BGR: return new XYZW(pixel[2], pixel[1], pixel[0], 1);
                    case ColorEncoding.RGBA: return new XYZW(pixel[0], pixel[1], pixel[2], pixel[3]);
                    case ColorEncoding.BGRA: return new XYZW(pixel[2], pixel[1], pixel[0], pixel[3]);
                    case ColorEncoding.ARGB: return new XYZW(pixel[3], pixel[2], pixel[1], pixel[0]);
                    default: throw new NotImplementedException();
                }
            }

            throw new NotImplementedException(nameof(TElement));
        }

        public static unsafe void CopyScaledPixelTo<TDstPixel>(this XYZW srcScaledPixel, Span<TDstPixel> dstEncodedPixel, ColorEncoding dstEncoding)
            where TDstPixel : unmanaged
        {
            // destination uses bytes

            if (sizeof(TDstPixel) <= 4 && typeof(TDstPixel) != typeof(float))
            {
                var dstPixels = MMARSHAL.Cast<TDstPixel, byte>(dstEncodedPixel);

                switch (dstEncoding)
                {
                    case ColorEncoding.A: dstPixels[0] = (byte)(srcScaledPixel.W * 255f); return;
                    case ColorEncoding.L: dstPixels[0] = (byte)(srcScaledPixel.Y * 255f); return;

                    case ColorEncoding.RGB:
                        {
                            var quantized = srcScaledPixel * 255f;
                            dstPixels[0] = (byte)quantized.X;
                            dstPixels[1] = (byte)quantized.Y;
                            dstPixels[2] = (byte)quantized.Z;
                            return;
                        }                        
                    case ColorEncoding.BGR:
                        {
                            var quantized = srcScaledPixel * 255f;
                            dstPixels[0] = (byte)quantized.Z;
                            dstPixels[1] = (byte)quantized.Y;
                            dstPixels[2] = (byte)quantized.X;
                            return;
                        }                        
                    case ColorEncoding.RGBA:
                        {
                            var quantized = srcScaledPixel * 255f;
                            dstPixels[0] = (byte)quantized.X;
                            dstPixels[1] = (byte)quantized.Y;
                            dstPixels[2] = (byte)quantized.Z;
                            dstPixels[3] = (byte)quantized.W;
                            return;
                        }
                    case ColorEncoding.BGRA:
                        {
                            var quantized = srcScaledPixel * 255f;
                            dstPixels[0] = (byte)quantized.Z;
                            dstPixels[1] = (byte)quantized.Y;
                            dstPixels[2] = (byte)quantized.X;
                            dstPixels[3] = (byte)quantized.W;
                            return;
                        }
                    case ColorEncoding.ARGB:
                        {
                            var quantized = srcScaledPixel * 255f;
                            dstPixels[0] = (byte)quantized.W;
                            dstPixels[1] = (byte)quantized.X;
                            dstPixels[2] = (byte)quantized.Y;
                            dstPixels[3] = (byte)quantized.Z;
                            return;
                        }                    
                }
            }

            // destination uses floats

            if (typeof(TDstPixel) == typeof(float) || typeof(TDstPixel) == typeof(XYZ) || typeof(TDstPixel) == typeof(XYZW))
            {
                var dstPixels = MMARSHAL.Cast<TDstPixel, float>(dstEncodedPixel);

                switch (dstEncoding)
                {
                    case ColorEncoding.A: dstPixels[0] = srcScaledPixel.W; return;
                    case ColorEncoding.L: dstPixels[0] = srcScaledPixel.Y; return;
                    case ColorEncoding.RGB:
                        dstPixels[0] = srcScaledPixel.X;
                        dstPixels[1] = srcScaledPixel.Y;
                        dstPixels[2] = srcScaledPixel.Z;
                        return;
                    case ColorEncoding.BGR:
                        dstPixels[0] = srcScaledPixel.Z;
                        dstPixels[1] = srcScaledPixel.Y;
                        dstPixels[2] = srcScaledPixel.X;
                        return;
                    case ColorEncoding.RGBA:
                        MMARSHAL.Cast<TDstPixel, XYZW>(dstEncodedPixel)[0] = srcScaledPixel;
                        return;
                    case ColorEncoding.BGRA:
                        dstPixels[0] = srcScaledPixel.Z;
                        dstPixels[1] = srcScaledPixel.Y;
                        dstPixels[2] = srcScaledPixel.X;
                        dstPixels[3] = srcScaledPixel.W;
                        return;
                    case ColorEncoding.ARGB:
                        dstPixels[0] = srcScaledPixel.W;
                        dstPixels[1] = srcScaledPixel.X;
                        dstPixels[2] = srcScaledPixel.Y;
                        dstPixels[3] = srcScaledPixel.Z;
                        return;                    
                }
            }

            throw new NotImplementedException(nameof(TDstPixel));
        }
        public static unsafe void CopyScaledPixelTo<TDstPixel>(this Span<XYZW> srcScaledPixels, Span<TDstPixel> dstEncodedPixels, ColorEncoding dstEncoding)
            where TDstPixel : unmanaged
        {
            CopyScaledPixelTo((ReadOnlySpan<XYZW>)srcScaledPixels, dstEncodedPixels, dstEncoding);
        }
        public static unsafe void CopyScaledPixelTo<TDstPixel>(this ReadOnlySpan<XYZW> srcScaledPixels, Span<TDstPixel> dstEncodedPixels, ColorEncoding dstEncoding)
            where TDstPixel : unmanaged
        {
            // destination uses bytes

            if (sizeof(TDstPixel) <= 4 && typeof(TDstPixel) != typeof(float))
            {
                var srcPixels = MMARSHAL.Cast<XYZW, float>(srcScaledPixels);
                var dstPixels = MMARSHAL.Cast<TDstPixel, byte>(dstEncodedPixels);

                Span<Byte> srcBytePixels = stackalloc byte[srcPixels.Length];
                srcPixels.ScaledCastTo(srcBytePixels);

                _CopyRgbaPixelsTo(srcBytePixels, dstPixels, dstEncoding);
            }

            // destination uses floats

            if (typeof(TDstPixel) == typeof(float) || typeof(TDstPixel) == typeof(XYZ) || typeof(TDstPixel) == typeof(XYZW))
            {
                var srcPixels = MMARSHAL.Cast<XYZW, float>(srcScaledPixels);
                var dstPixels = MMARSHAL.Cast<TDstPixel, float>(dstEncodedPixels);

                _CopyRgbaPixelsTo(srcPixels, dstPixels, dstEncoding);
            }

            throw new NotImplementedException(nameof(TDstPixel));
        }

        private static unsafe void _CopyRgbaPixelsTo<TElement>(ReadOnlySpan<TElement> srcPixels, Span<TElement> dstPixels, ColorEncoding dstEncoding)
            where TElement : unmanaged
        {
            int count = srcPixels.Length / 4;

            // we can use A _Vector4<TElement> to simplify code conversion

            switch (dstEncoding)
            {
                case ColorEncoding.A: for (int i = 0; i < count; ++i) { dstPixels[i] = srcPixels[i * 4 + 3]; } return;
                case ColorEncoding.L: for (int i = 0; i < count; ++i) { dstPixels[i] = srcPixels[i * 3 + 1]; } return;
                case ColorEncoding.RGB:
                    for (int i = 0; i < count; ++i)
                    {
                        dstPixels[i * 3 + 0] = srcPixels[i * 4 + 0];
                        dstPixels[i * 3 + 1] = srcPixels[i * 4 + 1];
                        dstPixels[i * 3 + 2] = srcPixels[i * 4 + 2];
                    }
                    return;
                case ColorEncoding.BGR:
                    for (int i = 0; i < count; ++i)
                    {
                        dstPixels[i * 3 + 0] = srcPixels[i * 4 + 2];
                        dstPixels[i * 3 + 1] = srcPixels[i * 4 + 1];
                        dstPixels[i * 3 + 2] = srcPixels[i * 4 + 0];
                    }
                    return;
                case ColorEncoding.RGBA:
                    for (int i = 0; i < count; ++i)
                    {
                        dstPixels[i * 4 + 0] = srcPixels[i * 4 + 0];
                        dstPixels[i * 4 + 1] = srcPixels[i * 4 + 1];
                        dstPixels[i * 4 + 2] = srcPixels[i * 4 + 2];
                        dstPixels[i * 4 + 3] = srcPixels[i * 4 + 3];
                    }
                    return;
                case ColorEncoding.BGRA:
                    for (int i = 0; i < count; ++i)
                    {
                        dstPixels[i * 4 + 0] = srcPixels[i * 4 + 2];
                        dstPixels[i * 4 + 1] = srcPixels[i * 4 + 1];
                        dstPixels[i * 4 + 2] = srcPixels[i * 4 + 0];
                        dstPixels[i * 4 + 3] = srcPixels[i * 4 + 3];
                    }
                    return;
                case ColorEncoding.ARGB:
                    for (int i = 0; i < count; ++i)
                    {
                        dstPixels[i * 4 + 0] = srcPixels[i * 4 + 3];
                        dstPixels[i * 4 + 1] = srcPixels[i * 4 + 0];
                        dstPixels[i * 4 + 2] = srcPixels[i * 4 + 1];
                        dstPixels[i * 4 + 3] = srcPixels[i * 4 + 2];
                    }
                    return;
                default: throw new NotSupportedException();
            }

            throw new NotImplementedException(nameof(TElement));
        }


        public static unsafe void CopyToRGB<TSrcPixel, TDstElement>(this ReadOnlySpan<TSrcPixel> src, ColorEncoding srcEnc, Span<TDstElement> dst)
            where TSrcPixel : unmanaged
            where TDstElement : unmanaged
        {
            if (src.Length != dst.Length * 3) throw new ArgumentException("length", nameof(dst));

            if (typeof(TDstElement) == typeof(float))
            {
                var dstFFF = MMARSHAL.Cast<TDstElement, float>(dst);

                if (typeof(TSrcPixel) == typeof(XYZ) || typeof(TSrcPixel) == typeof(XYZW))
                {
                    var srcFFF = MMARSHAL.Cast<TSrcPixel, float>(src);
                    _ShuffleToRGB(srcFFF, srcEnc, dstFFF);
                    return;
                }
            }

            if (typeof(TDstElement) == typeof(byte))
            {
                var dstFFF = MMARSHAL.Cast<TDstElement, byte>(dst);

                if (sizeof(TSrcPixel) <= 4)
                {
                    var srcFFF = MMARSHAL.Cast<TSrcPixel, byte>(src);
                    _ShuffleToRGB(srcFFF, srcEnc, dstFFF);
                    return;
                }
            }

            throw new NotImplementedException();
        }

        private static unsafe void _ShuffleToRGB<TElement>(ReadOnlySpan<TElement> srcFFF, ColorEncoding srcEnc, Span<TElement> dstFFF)
            where TElement : unmanaged
        {
            var len = dstFFF.Length / 3;
            if (len != srcFFF.Length / srcEnc.GetChannelCount()) throw new ArgumentException("encoding mismatch", nameof(srcEnc));

            switch (srcEnc)
            {
                case ColorEncoding.RGB:
                    srcFFF.CopyTo(dstFFF); return;
                case ColorEncoding.BGR:
                    for (int i = 0; i < len; ++i)
                    {
                        dstFFF[i * 3 + 0] = srcFFF[i * 3 + 2];
                        dstFFF[i * 3 + 1] = srcFFF[i * 3 + 1];
                        dstFFF[i * 3 + 2] = srcFFF[i * 3 + 0];
                    }; return;
                case ColorEncoding.RGBA:
                    for (int i = 0; i < len; ++i)
                    {
                        dstFFF[i * 3 + 0] = srcFFF[i * 4 + 0];
                        dstFFF[i * 3 + 1] = srcFFF[i * 4 + 1];
                        dstFFF[i * 3 + 2] = srcFFF[i * 4 + 2];
                    }; return;
                case ColorEncoding.BGRA:
                    for (int i = 0; i < len; ++i)
                    {
                        dstFFF[i * 3 + 0] = srcFFF[i * 4 + 2];
                        dstFFF[i * 3 + 1] = srcFFF[i * 4 + 1];
                        dstFFF[i * 3 + 2] = srcFFF[i * 4 + 0];
                    }; return;
                case ColorEncoding.ARGB:
                    for (int i = 0; i < len; ++i)
                    {
                        dstFFF[i * 3 + 0] = srcFFF[i * 4 + 1];
                        dstFFF[i * 3 + 1] = srcFFF[i * 4 + 2];
                        dstFFF[i * 3 + 2] = srcFFF[i * 4 + 3];
                    }; return;
                default: throw new NotImplementedException();
            }
        }
    }
}
