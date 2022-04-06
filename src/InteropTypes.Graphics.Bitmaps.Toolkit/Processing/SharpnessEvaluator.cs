using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    public class SharpnessEvaluator
    {
        // https://www.sciencedirect.com/science/article/pii/S1877705813016007
        // https://github.com/justadudewhohacks/opencv4nodejs/issues/448#issuecomment-436341650
        // https://stackoverflow.com/questions/17887883/image-sharpness-metric
        // https://jivp-eurasipjournals.springeropen.com/articles/10.1186/1687-5281-2012-8
        // https://www.sciencedirect.com/science/article/pii/S1877705813016007
        // https://ieeexplore.ieee.org/document/8073580
        // https://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.478.82&rep=rep1&type=pdf

        public static Double Evaluate(SpanBitmap image, double power=1)
        {
            var evaluator = new SharpnessEvaluator(power);
            evaluator.AddImage(image);
            return evaluator.SharpnessFactor;
        }

        public SharpnessEvaluator(double power) { _Power = power; }

        private double _Power;
        private _VarianceAccumulator _Variance;

        public Double SharpnessFactor => Math.Pow(_Variance.StandardDeviation, _Power);

        protected void AddSample(double sample) { _Variance.AddSample(sample); }

        public void AddImage(SpanBitmap image)
        {
            Kernel3x3<Pixel.BGR96F>.Process(image, _Laplacian_EdgeDetector, pixel => this.AddSample(_MaxOf(pixel)));
        }

        private static float _MaxOf(in Pixel.BGR96F color)
        {
            const float RLuminanceWeightF = 0.2989f;
            const float GLuminanceWeightF = 0.5870f;
            const float BLuminanceWeightF = 0.1140f;

            var r = color.R * RLuminanceWeightF;
            var g = color.G * GLuminanceWeightF;
            var b = color.B * BLuminanceWeightF;

            return Math.Max(Math.Max(r, g), b) / GLuminanceWeightF;
        }

        private static Pixel.BGR96F _Laplacian_EdgeDetector(in Kernel3x3<Pixel.BGR96F> kernel)
        {
            var v = System.Numerics.Vector3.Zero;
            v -= kernel.P11.BGR;
            v -= kernel.P12.BGR;
            v -= kernel.P13.BGR;
            v -= kernel.P21.BGR;
            v += kernel.P22.BGR * 8;
            v -= kernel.P23.BGR;
            v -= kernel.P31.BGR;
            v -= kernel.P32.BGR;
            v -= kernel.P33.BGR;
            v = System.Numerics.Vector3.Min(System.Numerics.Vector3.One, v);
            v = System.Numerics.Vector3.Max(System.Numerics.Vector3.Zero, v);

            return new Pixel.BGR96F(v.Z, v.Y, v.X);
        }
    }

    struct _VarianceAccumulator
    {
        private ulong _Count;
        private double _Sum;
        private double _Value;

        public void Clear()
        {
            _Count = 0;
            _Sum = 0;
            _Value = 0;
        }

        public void AddSample(double sample)
        {
            if (_Count == 0)
            {
                _Count++;
                _Sum = sample;
            }
            else
            {
                _Count++;
                _Sum += sample;

                double x = (double)_Count * sample - _Sum;
                _Value += x * x / (double)(_Count * (_Count - 1));
            }
        }

        public double Variance
        {
            get
            {
                if (_Count <= 1) return double.NaN;
                return _Value / (double)(_Count - 1);
            }
        }

        public double StandardDeviation => Math.Sqrt(Variance);

    }
}
