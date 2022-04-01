using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

namespace InteropTypes.Graphics.Bitmaps
{
    // https://github.com/dotnet/BenchmarkDotNet/issues/257
    // [ClrJob, CoreJob, MonoJob]
    // [MonoJob("Mono x64", @"C:\Program Files\Mono\bin\mono.exe")]    
    [SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput, BenchmarkDotNet.Jobs.RuntimeMoniker.Net472)]
    [SimpleJob(BenchmarkDotNet.Engines.RunStrategy.Throughput, BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
    public class PremulMath
    {
        private static Pixel.BGRA32 _SrcColor = new Pixel.BGRA32(255, 255, 255, 255);

        private static Pixel.BGRP32 _PreSrcColor = new Pixel.BGRP32(new Pixel.BGRA32(255, 255, 255, 255));

        [Benchmark]
        public Pixel.BGRP32 TestReferencePremul()
        {
            return ToPremulReference(_SrcColor);
        }

        [Benchmark]
        public Pixel.BGRP32 TestFastPremul()
        {
            return ToPremulFast(_SrcColor);
        }

        [Benchmark]
        public Pixel.BGRA32 TestReferenceUnpremul()
        {
            return ToUnpremulReference(_PreSrcColor);
        }

        [Benchmark]
        public Pixel.BGRA32 TestFastUnpremul()
        {
            return ToUnpremulFast(_PreSrcColor);
        }

        // 7.508 ns
        public static Pixel.BGRP32 ToPremulReference(Pixel.BGRA32 src)
        {
            uint aa = src.A;

            return new Pixel.BGRP32
                (
                (Byte)((src.R * aa) / 255u),
                (Byte)((src.G * aa) / 255u),
                (Byte)((src.B * aa) / 255u),
                src.A);
        }

        //  7.163 ns
        public Pixel.BGRP32 ToPremulFast(Pixel.BGRA32 src)
        {
            var aa = (uint)257 * (uint)src.A;

            return new Pixel.BGRP32
                (
                (Byte)((src.R * aa + 255u) >> 16),
                (Byte)((src.G * aa + 255u) >> 16),
                (Byte)((src.B * aa + 255u) >> 16),
                src.A);
        }

        // 9.215 ns
        public static Pixel.BGRA32 ToUnpremulReference(Pixel.BGRP32 src)
        {
            uint aa = src.A;

            return new Pixel.BGRA32
                (
                (Byte)((src.PreR * 255u) / aa),
                (Byte)((src.PreG * 255u) / aa),
                (Byte)((src.PreB * 255u) / aa),
                src.A);
        }

        // 8.054 ns
        public Pixel.BGRA32 ToUnpremulFast(Pixel.BGRP32 src)
        {
            if (src.A == 0) return default;

            uint rcpA = (65536u * 255u) / (uint)src.A;

            return new Pixel.BGRA32
                (
                (Byte)((src.PreR * rcpA + 255u) >> 16),
                (Byte)((src.PreG * rcpA + 255u) >> 16),
                (Byte)((src.PreB * rcpA + 255u) >> 16),
                src.A);
        }
    }
}
