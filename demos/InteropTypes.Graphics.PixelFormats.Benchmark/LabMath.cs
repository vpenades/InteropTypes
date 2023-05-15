using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;

using BENCHSTRATEGY = BenchmarkDotNet.Engines.RunStrategy;
using MONIKER = BenchmarkDotNet.Jobs.RuntimeMoniker;

namespace InteropTypes.Graphics.Bitmaps
{
    // https://github.com/dotnet/BenchmarkDotNet/issues/257
    // [ClrJob, CoreJob, MonoJob]
    // [MonoJob("Mono x64", @"C:\Program Files\Mono\bin\mono.exe")]    
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.Net472)]
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.Net60)]
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.Net70)]
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.NativeAot70)]
    public class LabMath
    {
        [Benchmark]
        public int TestReferenceAPI1()
        {
            int x = 0;
            for (float i = -255; i < 255; i += 0.3f)
            {
                x |= _ref1(i);
            }
            return x;
        }

        [Benchmark]
        public int TestReferenceAPI2()
        {
            int x = 0;
            for (float i = -255; i < 255; i += 0.3f)
            {
                x |= _ref2(i);
            }
            return x;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int _ref1(float value)
        {
            #if NET472
            return (int)Math.Floor(value);
            #else
            return (int)MathF.Floor(value);
            #endif
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int _ref2(float value)
        {
            int i = (int)value;
            return (i > value) ? i - 1 : i;
        }
    }
}
