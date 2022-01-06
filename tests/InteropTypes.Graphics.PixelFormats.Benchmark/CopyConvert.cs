using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace InteropBitmaps
{
    [MonoJob]
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net472), SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
    public class CopyConvert
    {
        #region init

        public CopyConvert()
        {
            var rnd = new Random(117);
            Vector4Streaming.Noise(_RGBA32Pixels.AsSpan().AsBytes(), rnd);
            Vector4Streaming.Noise(_RGBA128FPixels.AsSpan().AsSingles(), rnd);
        }

        #endregion

        #region data

        private readonly Pixel.RGBA32[] _RGBA32Pixels = new Pixel.RGBA32[65536];
        private readonly Pixel.RGBA128F[] _RGBA128FPixels = new Pixel.RGBA128F[65536];
        private readonly Pixel.CopyConverterCallback<Pixel.RGBA32, Pixel.RGBA128F> _Function = Pixel.GetPixelCopyConverter<Pixel.RGBA32, Pixel.RGBA128F>();

        

        #endregion

        #region tests

        [Benchmark]
        public void TestCopy()
        {
            _Function(_RGBA32Pixels, _RGBA128FPixels);
        }

        [Benchmark]
        public void TestDirect()
        {
            var src = _RGBA32Pixels.AsSpan().AsBytes();
            var dst = _RGBA128FPixels.AsSpan().AsSingles();

            Vector4Streaming.BytesToUnits(src, dst);
        }

        [Benchmark]
        public void TestDirectNaive()
        {
            var src = _RGBA32Pixels.AsSpan().AsBytes();
            var dst = _RGBA128FPixels.AsSpan().AsSingles();

            BytesToUnitsNaive(src, dst);
        }

        [Benchmark]
        public void TestDirectFastConvert()
        {
            var src = _RGBA32Pixels.AsSpan().AsBytes();
            var dst = _RGBA128FPixels.AsSpan().AsSingles();

            BytesToUnitsFastConvert(src, dst);
        }

        [Benchmark]
        public void TestDirectPtr()
        {
            var src = _RGBA32Pixels.AsSpan().AsBytes();
            var dst = _RGBA128FPixels.AsSpan().AsSingles();

            BytesToUnitsPtr(src, dst);
        }

        [Benchmark]
        public void TestDirectLUT()
        {
            var src = _RGBA32Pixels.AsSpan().AsBytes();
            var dst = _RGBA128FPixels.AsSpan().AsSingles();

            BytesToUnitsLUT(src, dst);
        }

        #endregion

        #region reference implementations

        private const float _Reciprocal255 = 1f / 255f;

        private static readonly float[] _ByteToFloatLUT = Enumerable.Range(0, 256).Select(idx => (float)idx / 255f).ToArray();

        private static Span<Vector4> _ToVector4(Span<Single> span)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, span.Length & ~3));
        }

        private static ReadOnlySpan<Vector4> _ToVector4(ReadOnlySpan<Single> span)
        {
            return System.Runtime.InteropServices.MemoryMarshal.Cast<float, Vector4>(span.Slice(0, span.Length & ~3));
        }

        public static void BytesToUnitsNaive(ReadOnlySpan<byte> src, Span<float> dst)
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = src[i] * _Reciprocal255;
            }
        }

        public static void BytesToUnitsFastConvert(ReadOnlySpan<byte> src, Span<float> dst)
        {
            for (int i = 0; i < dst.Length; ++i)
            {
                dst[i] = FastByteToFloat.ToFloat(src[i]);
            }
        }        

        public static void BytesToUnitsLUT(ReadOnlySpan<byte> src, Span<float> dst)
        {
            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref float dPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst);

            int dLen = dst.Length;

            while (dLen > 0)
            {
                --dLen;
                dPtr = _ByteToFloatLUT[sPtr];
                sPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 1);
                dPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref dPtr, 1);
            }
        }

        public static void BytesToUnitsPtr(ReadOnlySpan<byte> src, Span<float> dst)
        {
            if (dst.Length < src.Length) throw new ArgumentException();

            var dst4 = _ToVector4(dst);

            ref byte sPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(src);
            ref Vector4 d4Ptr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(dst4);
            int l = dst4.Length;

            while (l-- > 0)
            {
                d4Ptr = new Vector4
                    (sPtr,
                    System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 1),
                    System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 2),
                    System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 3)) * _Reciprocal255;

                sPtr = ref System.Runtime.CompilerServices.Unsafe.Add(ref sPtr, 4);
                d4Ptr = ref System.Runtime.CompilerServices.Unsafe.Add(ref d4Ptr, 1);
            }

            for (int i = dst4.Length * 4; i < dst.Length; ++i)
            {
                dst[i] = src[i] * _Reciprocal255;
            }
        }

        #endregion
    }
}
