﻿using System;
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
    [MonoJob("Mono x64", @"C:\Program Files\Mono\bin\mono.exe")]
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.Net472)]
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.Net60)]
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.Net70)]
    [SimpleJob(BENCHSTRATEGY.Throughput, MONIKER.NativeAot70)]
    public class BilinearInterpolation
    {
        private static readonly Pixel.RGBA32 Pixel_A = new Pixel.RGBA32(255, 255, 255, 255);
        private static readonly Pixel.RGBA32 Pixel_B = new Pixel.RGBA32(0, 255, 255, 255);
        private static readonly Pixel.RGBA32 Pixel_C = new Pixel.RGBA32(255, 0, 255, 255);
        private static readonly Pixel.RGBA32 Pixel_D = new Pixel.RGBA32(255, 0, 0, 0);

        private static readonly Pixel.BGRA32 Pixel_0_A = new Pixel.BGRA32(255, 255, 255, 255);
        private static readonly Pixel.BGRA32 Pixel_0_B = new Pixel.BGRA32(0, 255, 255, 255);
        private static readonly Pixel.BGRA32 Pixel_0_C = new Pixel.BGRA32(255, 0, 255, 255);
        private static readonly Pixel.BGRA32 Pixel_0_D = new Pixel.BGRA32(255, 0, 0, 0);

        private static readonly Pixel.BGR24 Pixel_1_A = new Pixel.BGR24(255, 255, 255);
        private static readonly Pixel.BGR24 Pixel_1_B = new Pixel.BGR24(0, 255, 255);
        private static readonly Pixel.BGR24 Pixel_1_C = new Pixel.BGR24(255, 0, 255);
        private static readonly Pixel.BGR24 Pixel_1_D = new Pixel.BGR24(255, 0, 0);

        private static readonly Pixel.BGRA128F Pixel_2_A = new Pixel.BGRA128F(1, 1, 1, 1);
        private static readonly Pixel.BGRA128F Pixel_2_B = new Pixel.BGRA128F(0, 1, 1, 1);
        private static readonly Pixel.BGRA128F Pixel_2_C = new Pixel.BGRA128F(1, 0, 1, 1);
        private static readonly Pixel.BGRA128F Pixel_2_D = new Pixel.BGRA128F(1, 0, 0, 0);

        [Benchmark]
        public Pixel.RGBA32 TestReferenceVector4()
        {
            return BilinearSample(Pixel_A, Pixel_B, Pixel_C, Pixel_D, 0.5f, 0.5f);
        }


        [Benchmark]
        public Pixel.BGRA32 TestProductionAPI_Q_Unpremul()
        {
            return Pixel.BGRA32.Lerp(Pixel_0_A, Pixel_0_B, Pixel_0_C, Pixel_0_D, 511, 251);
        }

        [Benchmark]
        public Pixel.BGRP32 TestProductionAPI_Q_Premul()
        {
            return Pixel.BGRA32.LerpToBGRP32(Pixel_0_A, Pixel_0_B, Pixel_0_C, Pixel_0_D, 1024, 700);
        }

        [Benchmark]
        public Pixel.RGBP128F TestProductionAPI_QtoF_Unpremul()
        {
            return Pixel.BGRA32.LerpToRGBP128F(Pixel_0_A, Pixel_0_B, Pixel_0_C, Pixel_0_D, 0.5f, 0.3f);
        }

        [Benchmark]
        public Pixel.RGBP128F TestProductionAPI_F_Premul()
        {
            return Pixel.BGRA128F.LerpToRGBP128F(Pixel_2_A, Pixel_2_B, Pixel_2_C, Pixel_2_D, 0.5f, 0.3f);
        }

        #region reference

        private const float _Reciprocal255 = 1f / 255f;

        public static Pixel.RGBA32 BilinearSample(in Pixel.RGBA32 p00, in Pixel.RGBA32 p01, in Pixel.RGBA32 p10, in Pixel.RGBA32 p11, float rx, float by)
        {
            // calculate quantized weights
            var lx = 1f - rx;
            var ty = 1f - by;
            var wwww = new Vector4(lx * ty, rx * ty, lx * by, rx * by);

            System.Diagnostics.Debug.Assert(Vector4.Dot(wwww, Vector4.One) == 1f);

            // calculate final alpha

            var aaaa = new Vector4(p00.A, p01.A, p10.A, p11.A) * _Reciprocal255;

            float a = Vector4.Dot(aaaa, wwww);

            if (a == 0) return default;

            // calculate premultiplied RGB

            wwww /= aaaa;

            var r = Vector4.Dot(wwww, new Vector4(p00.R, p01.R, p10.R, p11.R)) * _Reciprocal255;
            var g = Vector4.Dot(wwww, new Vector4(p00.G, p01.G, p10.G, p11.G)) * _Reciprocal255;
            var b = Vector4.Dot(wwww, new Vector4(p00.B, p01.B, p10.B, p11.B)) * _Reciprocal255;

            // unpremultiply RGB

            var rgb = new Vector3(r, g, b) / a;

            return new Pixel.RGBA32(new Pixel.RGBA128F(rgb, a));
        }

        #endregion
    }
}
