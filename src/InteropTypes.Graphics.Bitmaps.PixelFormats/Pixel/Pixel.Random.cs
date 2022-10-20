// GENERATED CODE: using CodeGenUtils.t4
// GENERATED CODE: using Pixel.Constants.t4

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace InteropTypes.Graphics.Bitmaps
{
    #pragma warning disable CA5394 // Do not use insecure randomness
    partial class Pixel
    {
        partial struct Alpha8
        {
            public void SetRandom(Random rnd) { this.A = (Byte)rnd.Next(); }
        }

        partial struct Luminance8
        {
            public void SetRandom(Random rnd) { this.L = (Byte)rnd.Next(); }
        }

        partial struct Luminance16
        {
            public void SetRandom(Random rnd) { this.L = (ushort)rnd.Next(); }
        }

        partial struct Luminance32F
        {
            public void SetRandom(Random rnd)
            {
                #if NET6_0_OR_GREATER
                this.L = rnd.NextSingle();
                #else
                this.L = (float)rnd.NextDouble();                
                #endif
            }
        }

        partial struct RGB24
        {
            public void SetRandom(Random rnd)
            {
                var v = rnd.Next();
                this.B = (Byte)v;
                this.G = (Byte)(v >> 8);
                this.R = (Byte)(v >> 16);
            }
        }

        partial struct BGR24
        {
            public void SetRandom(Random rnd)
            {
                var v = rnd.Next();
                this.B = (Byte)v;
                this.G = (Byte)(v>>8);
                this.R = (Byte)(v>>16);
            }
        }

        partial struct RGBA32
        {
            public void SetRandom(Random rnd)
            {
                var packed = (uint)rnd.Next(int.MinValue, int.MaxValue);
                this = Unsafe.As<uint, RGBA32>(ref packed);
            }

            public void SetRandom(Random rnd, int alpha)
            {
                var packed = (uint)rnd.Next(int.MinValue, int.MaxValue);
                this = Unsafe.As<uint, RGBA32>(ref packed);
                this.A = (byte)alpha;
            }
        }

        partial struct BGRA32
        {
            public void SetRandom(Random rnd)
            {
                var packed = (uint)rnd.Next(int.MinValue, int.MaxValue);
                this = Unsafe.As<uint, BGRA32>(ref packed);
            }

            public void SetRandom(Random rnd, int alpha)
            {
                var packed = (uint)rnd.Next(int.MinValue, int.MaxValue);
                this = Unsafe.As<uint, BGRA32>(ref packed);
                this.A = (byte)alpha;
            }
        }

        partial struct ARGB32
        {
            public void SetRandom(Random rnd)
            {
                var packed = (uint)rnd.Next(int.MinValue, int.MaxValue);
                this = Unsafe.As<uint, ARGB32>(ref packed);
            }

            public void SetRandom(Random rnd, int alpha)
            {
                var packed = (uint)rnd.Next(int.MinValue, int.MaxValue);
                this = Unsafe.As<uint, ARGB32>(ref packed);
                this.A = (byte)alpha;
            }
        }

        partial struct RGB96F
        {
            public void SetRandom(Random rnd)
            {
                #if NET6_0_OR_GREATER
                this.R = rnd.NextSingle();
                this.G = rnd.NextSingle();
                this.B = rnd.NextSingle();
                #else
                this.R = (float)rnd.NextDouble();
                this.G = (float)rnd.NextDouble();
                this.B = (float)rnd.NextDouble();
                #endif
            }
        }

        partial struct BGR96F
        {
            public void SetRandom(Random rnd)
            {
                #if NET6_0_OR_GREATER
                this.R = rnd.NextSingle();
                this.G = rnd.NextSingle();
                this.B = rnd.NextSingle();
                #else
                this.R = (float)rnd.NextDouble();
                this.G = (float)rnd.NextDouble();
                this.B = (float)rnd.NextDouble();
                #endif
            }
        }

        partial struct RGBA128F
        {
            public void SetRandom(Random rnd)
            {
                #if NET6_0_OR_GREATER
                this.R = rnd.NextSingle();
                this.G = rnd.NextSingle();
                this.B = rnd.NextSingle();
                this.A = rnd.NextSingle();
                #else
                this.R = (float)rnd.NextDouble();
                this.G = (float)rnd.NextDouble();
                this.B = (float)rnd.NextDouble();
                this.A = (float)rnd.NextDouble();
                #endif
            }

            public void SetRandom(Random rnd, float alpha)
            {
                #if NET6_0_OR_GREATER
                this.R = rnd.NextSingle();
                this.G = rnd.NextSingle();
                this.B = rnd.NextSingle();
                this.A = alpha;
                #else
                this.R = (float)rnd.NextDouble();
                this.G = (float)rnd.NextDouble();
                this.B = (float)rnd.NextDouble();
                this.A = alpha;
                #endif
            }
        }

        partial struct BGRA128F
        {
            public void SetRandom(Random rnd)
            {
                #if NET6_0_OR_GREATER
                this.R = rnd.NextSingle();
                this.G = rnd.NextSingle();
                this.B = rnd.NextSingle();
                this.A = rnd.NextSingle();
                #else
                this.R = (float)rnd.NextDouble();
                this.G = (float)rnd.NextDouble();
                this.B = (float)rnd.NextDouble();
                this.A = (float)rnd.NextDouble();
                #endif
            }

            public void SetRandom(Random rnd, float alpha)
            {
                #if NET6_0_OR_GREATER
                this.R = rnd.NextSingle();
                this.G = rnd.NextSingle();
                this.B = rnd.NextSingle();
                this.A = alpha;
                #else
                this.R = (float)rnd.NextDouble();
                this.G = (float)rnd.NextDouble();
                this.B = (float)rnd.NextDouble();
                this.A = alpha;
                #endif
            }
        }
    }
    #pragma warning restore CA5394 // Do not use insecure randomness
}