using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    public abstract class Statistics
    {
        public static unsafe Statistics Create<T>(ReadOnlySpan<T> span)
            where T : unmanaged
        {
            if (sizeof(T) == 1)
            {
                var floatSpan = System.Runtime.InteropServices.MemoryMarshal.Cast<T, byte>(span);
                return new Scalar(floatSpan);
            }

            if (sizeof(T) == 3)
            {
                var byteSpan = System.Runtime.InteropServices.MemoryMarshal.Cast<T, Byte>(span);

                var v3Span = new System.Numerics.Vector3[byteSpan.Length / 3];

                for (int i = 0; i < v3Span.Length; ++i)
                {
                    v3Span[i] = new System.Numerics.Vector3(byteSpan[i * 3 + 0], byteSpan[i * 3 + 1], byteSpan[i * 3 + 2]);
                }

                return new Vector3(v3Span);
            }

            if (typeof(T) == typeof(float))
            {
                var floatSpan = System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(span);
                return new Scalar(floatSpan);
            }

            if (typeof(T) == typeof(System.Numerics.Vector3))
            {
                var v3Span = System.Runtime.InteropServices.MemoryMarshal.Cast<T, System.Numerics.Vector3>(span);
                return new Vector3(v3Span);
            }            

            throw new NotImplementedException();
        }

        #region nested types

        [System.Diagnostics.DebuggerDisplay("{Min} < x̄={Mean} > {Max}")]
        public class Scalar : Statistics
        {
            public Scalar(ReadOnlySpan<Byte> values)
            {
                Min = double.MaxValue;
                Mean = 0;
                Max = double.MinValue;

                _Count = 0;

                foreach (var v in values) _Append(v);

                if (_Count > 0)
                {
                    Mean /= _Count;
                }
            }

            public Scalar(ReadOnlySpan<float> values)
            {
                Min = double.MaxValue;
                Mean = 0;
                Max = double.MinValue;

                _Count = 0;

                foreach (var v in values) _Append(v);

                if (_Count > 0)
                {
                    Mean /= _Count;
                }
            }

            private int _Count;

            public double Min { get; private set; }
            public double Mean { get; private set; }
            public double Max { get; private set; }
            private void _Append(float value)
            {
                Min = Math.Min(Min, value);
                Max = Math.Max(Max, value);
                Mean += value;
                ++_Count;
            }
        }

        [System.Diagnostics.DebuggerDisplay("{Min} < x̄={Mean} > {Max}")]
        public class Vector3 : Statistics
        {
            public Vector3(ReadOnlySpan<System.Numerics.Vector3> values)
            {
                Min = new System.Numerics.Vector3(float.MaxValue);
                Mean = System.Numerics.Vector3.Zero;
                Max = new System.Numerics.Vector3(float.MinValue);

                _Count = 0;

                foreach (var v in values) _Append(v);

                if (_Count > 0)
                {
                    Mean /= _Count;

                    Variance = _CalcVariance(values, Mean);
                    StandardDeviation = new System.Numerics.Vector3((float)Math.Sqrt(Variance.X), (float)Math.Sqrt(Variance.Y), (float)Math.Sqrt(Variance.Z));
                }
            }

            private int _Count;

            public System.Numerics.Vector3 Min { get; private set; }
            public System.Numerics.Vector3 Mean { get; private set; }
            public System.Numerics.Vector3 Variance { get; private set; }
            public System.Numerics.Vector3 StandardDeviation { get; private set; }
            public System.Numerics.Vector3 Max { get; private set; }
            private void _Append(System.Numerics.Vector3 value)
            {
                Min = System.Numerics.Vector3.Min(Min, value);
                Max = System.Numerics.Vector3.Max(Max, value);
                Mean += value;
                ++_Count;
            }

            private static System.Numerics.Vector3 _CalcVariance(ReadOnlySpan<System.Numerics.Vector3> values, System.Numerics.Vector3 mean)
            {
                var var = System.Numerics.Vector3.Zero;

                foreach (var v in values)
                {
                    var l = v - mean;
                    var += l * l;
                }

                return var / values.Length;
            }
        }

        #endregion

    }


}
