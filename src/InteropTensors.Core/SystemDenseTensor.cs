using System;
using System.Collections.Generic;
using System.Numerics.Tensors;
using System.Text;

using InteropBitmaps;

namespace InteropTensors
{
    /// <summary>
    /// <see cref="IDenseTensor{T}"/> implementation over <see cref="System.Numerics.Tensors.DenseTensor{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <see href="https://devblogs.microsoft.com/dotnet/introducing-tensor-for-multi-dimensional-machine-learning-and-ai-data/"/>
    /// <seealso href="https://github.com/dotnet/runtime/labels/area-System.Numerics.Tensors"/>
    public struct SystemDenseTensor<T> : IDenseTensor<T>
        where T : unmanaged
    {
        #region constructor
        public SystemDenseTensor(DenseTensor<T> tensor, string name)
        {
            _Tensor = tensor;
            _Name = name;
        }

        #endregion

        #region data

        private readonly DenseTensor<T> _Tensor;
        private readonly string _Name;

        #endregion

        #region properties

        public string Name => _Name;

        public ReadOnlySpan<int> Dimensions => _Tensor.Dimensions;

        public IntPtr DataPointer => throw new NotImplementedException();

        public Span<T> Span => _Tensor.Buffer.Span;

        #endregion

        #region API

        public DenseTensor<T> ToDenseTensor()
        {
            return _Tensor.Clone() as DenseTensor<T>;
        }

        public void FitPixels(SpanBitmap src, TensorImageSettings mis)
        {
            throw new NotImplementedException();
        }

        public SpanBitmap AsSpanBitmap() { return _Implementation.GetSpanBitmap(this); }

        #endregion
    }
}
