using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Tensors.Imaging
{
    /// <summary>
    /// Represents the valid ranges each color component can have.
    /// </summary>
    public class ColorRanges
    {
        #region lifecycle
        public ColorRanges() { }

        public ColorRanges(float min, float max)
        {
            RedMin = GreenMin = BlueMin = AlphaMin = min;
            RedMax = GreenMax = BlueMax = AlphaMax = max;
        }

        #endregion

        #region properties

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

        #region API

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

        public static MultiplyAdd GetConversionTransform(ColorRanges from, ColorRanges to)
        {            
            return to.ToMultiplyAdd() * from.ToMultiplyAdd().GetInverse();
        }

        #endregion
    }
}
