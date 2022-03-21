using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Tensors;

using Microsoft.ML.OnnxRuntime.Tensors;

namespace InteropTypes.Vision.Backends
{
    /// <summary>
    /// <see cref="IDenseTensor{T}"/> implementation over <see cref="System.Numerics.Tensors.DenseTensor{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <see href="https://devblogs.microsoft.com/dotnet/introducing-tensor-for-multi-dimensional-machine-learning-and-ai-data/"/>
    /// <seealso href="https://github.com/dotnet/runtime/labels/area-System.Numerics.Tensors"/>
    readonly struct OnnxDenseTensor<T> : IDenseTensor<T> , IServiceProvider
        where T : unmanaged
    {
        #region constructor
        public OnnxDenseTensor(DenseTensor<T> tensor, string name)
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

        public Span<T> Span => _Tensor.Buffer.Span;

        #endregion

        #region API              

        System.Numerics.Tensors.DenseTensor<T> IDenseTensor<T>.ToDenseTensor()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(DenseTensor<T>)) return _Tensor;
            return null;
        }

        #endregion
    }
}
