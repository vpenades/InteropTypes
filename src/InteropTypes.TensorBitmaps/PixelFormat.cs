using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.TensorBitmaps
{
    public record TensorPixelComponent
    {
        public static TensorPixelComponent Red255 = new TensorPixelComponent("Red", true, 0, 255);
        public static TensorPixelComponent Green255 = new TensorPixelComponent("Green", true, 0, 255);
        public static TensorPixelComponent Blue255 = new TensorPixelComponent("Blue", true, 0, 255);
        public static TensorPixelComponent Alpha255 = new TensorPixelComponent("Alpha", true, 0, 255);

        public static TensorPixelComponent RedScalar = new TensorPixelComponent("Red", false, 0, 1);
        public static TensorPixelComponent GreenScalar = new TensorPixelComponent("Green", false, 0, 1);
        public static TensorPixelComponent BlueScalar = new TensorPixelComponent("Blue", false, 0, 1);
        public static TensorPixelComponent AlphaScalar = new TensorPixelComponent("Alpha", false, 0, 1);

        public TensorPixelComponent(string semantic, bool isInteger, float minValue, float maxValue)
        {
            Semantic = semantic;
            IsInteger = isInteger;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Red, Green, Blue, Alpha, PremulAlpha, Luminance, etc
        /// </summary>
        public string Semantic { get; }
        public bool IsInteger { get; }

        /// <summary>
        /// the minimum value expected to be found in this component
        /// </summary>
        /// <remarks>
        /// This is typically 0, but it can be a negative value if the pixels have been transformed by a std-mean
        /// </remarks>
        public float MinValue { get; }

        /// <summary>
        /// the maximum value expected to be found in this component
        /// </summary>
        /// <remarks>
        /// This is typically 1 or 255, but it can be a different value if the pixels have been transformed by a std-mean
        /// </remarks>
        public float MaxValue { get; }        
    }

    public record TensorPixelFormat
    {
        public static TensorPixelFormat Rgb24 = new TensorPixelFormat(TensorPixelComponent.Red255, TensorPixelComponent.Green255, TensorPixelComponent.Blue255);
        public static TensorPixelFormat Bgr24 = new TensorPixelFormat(TensorPixelComponent.Blue255, TensorPixelComponent.Green255, TensorPixelComponent.Red255);
        public static TensorPixelFormat Rgba32 = new TensorPixelFormat(TensorPixelComponent.Red255, TensorPixelComponent.Green255, TensorPixelComponent.Blue255, TensorPixelComponent.Alpha255);

        public static TensorPixelFormat Rgb96f = new TensorPixelFormat(TensorPixelComponent.RedScalar, TensorPixelComponent.GreenScalar, TensorPixelComponent.BlueScalar);
        public static TensorPixelFormat Bgr96f = new TensorPixelFormat(TensorPixelComponent.BlueScalar, TensorPixelComponent.GreenScalar, TensorPixelComponent.RedScalar);
        public static TensorPixelFormat Rgba128f = new TensorPixelFormat(TensorPixelComponent.RedScalar, TensorPixelComponent.GreenScalar, TensorPixelComponent.BlueScalar, TensorPixelComponent.AlphaScalar);

        public TensorPixelFormat(IReadOnlyList<TensorPixelComponent> components)
        {
            Components = components;
        }

        public TensorPixelFormat(TensorPixelComponent x)
        {
            Components = new TensorPixelComponent[] { x };
        }

        public TensorPixelFormat(TensorPixelComponent x, TensorPixelComponent y)
        {
            Components = new TensorPixelComponent[] { x, y };
        }

        public TensorPixelFormat(TensorPixelComponent x, TensorPixelComponent y, TensorPixelComponent z)
        {
            Components = new TensorPixelComponent[] {x, y, z };
        }

        public TensorPixelFormat(TensorPixelComponent x, TensorPixelComponent y, TensorPixelComponent z, TensorPixelComponent w)
        {
            Components = new TensorPixelComponent[] { x, y, z, w};
        }

        public IReadOnlyList<TensorPixelComponent> Components { get; }
    }    
}
