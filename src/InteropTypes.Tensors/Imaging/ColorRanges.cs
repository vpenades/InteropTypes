using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Tensors.Imaging
{
    /// <summary>
    /// Represents the valid ranges each color component can have.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack=1)]
    public readonly struct ColorRanges
    {
        #region lifecycle        

        public ColorRanges(float min, float max)
        {
            RedMin = GreenMin = BlueMin = AlphaMin = min;
            RedMax = GreenMax = BlueMax = AlphaMax = max;
        }

        public static implicit operator ColorRanges(Serializable s) { return new ColorRanges(s); }

        public ColorRanges(Serializable s)
        {
            this.RedMin = s.RedMin;
            this.RedMax = s.RedMax;

            this.GreenMin = s.GreenMin;
            this.GreenMax = s.GreenMax;

            this.BlueMin = s.BlueMin;
            this.BlueMax = s.BlueMax;

            this.AlphaMin = s.AlphaMin;
            this.AlphaMax = s.AlphaMax;
        }

        #endregion

        #region properties

        public static readonly ColorRanges Identity = new ColorRanges(0, 1);

        public readonly float RedMin;
        public readonly float RedMax;

        public readonly float GreenMin;
        public readonly float GreenMax;

        public readonly float BlueMin;
        public readonly float BlueMax;
        
        public readonly float AlphaMin;        
        public readonly float AlphaMax;

        #endregion

        #region API        

        public static unsafe ColorRanges GetRangesFor<T>()
            where T:unmanaged
        {
            if (sizeof(T) == 3) return new ColorRanges(0, 255);
            if (typeof(T) == typeof(Byte)) return new ColorRanges(0, 255);            
            if (typeof(T) == typeof(float)) return new ColorRanges(0, 1);
            if (typeof(T) == typeof(System.Numerics.Vector2)) return new ColorRanges(0, 1);
            if (typeof(T) == typeof(System.Numerics.Vector3)) return new ColorRanges(0, 1);
            if (typeof(T) == typeof(System.Numerics.Vector4)) return new ColorRanges(0, 1);            

            throw new NotImplementedException();
        }

        public MultiplyAdd ToMultiplyAdd()
        {
            float amin = AlphaMin;
            float amax = AlphaMax;

            if (amin == amax)
            {
                amin = BlueMin;
                amax = BlueMax;
            }

            var min = new Vector4(RedMin, GreenMin, BlueMin, amin);
            var max = new Vector4(RedMax, GreenMax, BlueMax, amax);

            return new MultiplyAdd(max - min, min);
        }

        public static MultiplyAdd GetConversionTransform(in ColorRanges from, ColorEncoding fromEnc, in ColorRanges to, ColorEncoding toEnc)
        {
            var srcMad = from.ToMultiplyAdd().GetShuffled(fromEnc);
            var dstMad = to.ToMultiplyAdd().GetShuffled(toEnc);
            if (srcMad.IsIdentity) return dstMad;

            return dstMad * srcMad.GetInverse();
        }

        public static MultiplyAdd GetConversionTransform(ColorRanges from, ColorRanges to)
        {
            var srcMad = from.ToMultiplyAdd();
            var dstMad = to.ToMultiplyAdd();
            if (srcMad.IsIdentity) return dstMad;

            return dstMad * srcMad.GetInverse();
        }

        #endregion

        #region nested types

        public sealed class Serializable
        {
            #region lifecycle
            public Serializable() { }

            public Serializable(float min, float max)
            {
                RedMin = GreenMin = BlueMin = AlphaMin = min;
                RedMax = GreenMax = BlueMax = AlphaMax = max;
            }

            #endregion

            #region properties

            public static readonly Serializable Identity = new Serializable(0, 1);

            public float RedMin { get; set; }
            public float RedMax { get; set; } = 1;

            public float GreenMin { get; set; }
            public float GreenMax { get; set; } = 1;

            public float BlueMin { get; set; }
            public float BlueMax { get; set; } = 1;

            #if !NETSTANDARD
            [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
            #endif
            public float AlphaMin { get; set; }

            #if !NETSTANDARD
            [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
            #endif
            public float AlphaMax { get; set; }

            #endregion            
        }

        #endregion
    }
}
