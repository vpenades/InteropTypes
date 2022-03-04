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
    public class FixedMath
    {
        [Benchmark]
        public uint TestReferenceAPI1()
        {
            uint x = 0;
            for(byte i=0; i < 255; ++i)
            {
                x |= Convert255To16384_ref1(i);
            }
            return x;
        }

        [Benchmark]
        public uint TestReferenceAPI2()
        {
            uint x = 0;
            for (byte i = 0; i < 255; ++i)
            {
                x |= Convert255To16384_ref2(i);
            }
            return x;
        }

        [Benchmark]
        public uint TestReferenceFast1()
        {
            uint x = 0;
            for (byte i = 0; i < 255; ++i)
            {
                x |= Convert255To16384_Fast1(i);
            }
            return x;
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint Convert255To16384_ref1(uint value)
        {
            return (value * 16384) / 255;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint Convert255To16384_ref2(uint value)
        {
            return (value << 14) / 255;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint Convert255To16384_Fast1(uint value)
        {
            const int s = 32 - 8 - 6 - 1;

            return (value * (uint)8454913) >> s;
        }
    }
}
