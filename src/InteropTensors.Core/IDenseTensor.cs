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
    /// Wraps a dense tensor; implemented by <see cref="SystemDenseTensor{T}"/> and <see cref="INativeDenseTensor{T}"/>
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <see href="https://github.com/dotnet/runtime/issues/28867"/>
    public interface IDenseTensor<T>
        where T:unmanaged
    {
        String Name { get; }

        ReadOnlySpan<int> Dimensions { get; }
        
        Span<T> Span { get; }

        
        DenseTensor<T> ToDenseTensor();

        void FitPixels(SpanBitmap src, TensorImageSettings mis);
    }

    public interface INativeDenseTensor<T> : IDenseTensor<T>, IDisposable
        where T : unmanaged
    {
        IntPtr DataPointer { get; }

        int ByteSize { get; }

        void FitPixels(PointerBitmap src, TensorImageSettings mis);

        void SetPixels(PointerBitmap src, Matrix3x2 transform, TensorImageSettings mis);
    }
}
