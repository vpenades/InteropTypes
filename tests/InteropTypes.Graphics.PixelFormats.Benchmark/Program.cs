using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Running;

namespace InteropBitmaps
{
    public class Program
    {
        // https://benchmarkdotnet.org/articles/guides/getting-started.html

        public static void Main(string[] args)
        {
            #if DEBUG
            var cfg = new BenchmarkDotNet.Configs.DebugInProcessConfig();
            #else
            var cfg = BenchmarkDotNet.Configs.DefaultConfig.Instance;
            #endif            

            var summary = BenchmarkSwitcher
                .FromAssembly(typeof(Program).Assembly)
                .Run(args);
        }
    }
}