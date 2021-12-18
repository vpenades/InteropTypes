using System;
using System.Collections.Generic;
using System.Numerics;
using System.Numerics.Tensors;
using System.Text;
using System.Threading;

using InteropBitmaps;

namespace InteropTensors
{
    /// <summary>
    /// Declares an object implementing this interface as a Dense Tensor.
    /// </summary>
    public interface IDenseTensor
    {
        String Name { get; }
        ReadOnlySpan<int> Dimensions { get; }
    }

    /// <summary>
    /// Declares an object implementing this interface as a Dense Tensor of type <typeparamref name="T"/>
    /// </summary>
    /// <remarks>
    /// Implemented by <see cref="SystemDenseTensor{T}"/> and <see cref="INativeDenseTensor{T}"/>
    /// </remarks>
    /// <typeparam name="T">The element type.</typeparam>
    /// <see href="https://github.com/dotnet/runtime/issues/28867"/>
    public interface IDenseTensor<T> : IDenseTensor
        where T:unmanaged
    {
        /// <summary>
        /// Gets the flattened data span of the tensor.
        /// </summary>
        Span<T> Span { get; }


        [Obsolete]
        DenseTensor<T> ToDenseTensor();        
    }

    public interface INativeDenseTensor<T> : IDenseTensor<T>, IDisposable
        where T : unmanaged
    {
        IntPtr DataPointer { get; }

        int ByteSize { get; }        
    }

    public interface IInputImageTensor : IDenseTensor
    {
        void FitPixels(PointerBitmap src);
        void SetPixels(PointerBitmap src, Matrix3x2 transform);
    }
}
