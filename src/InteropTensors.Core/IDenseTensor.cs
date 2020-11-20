using System;
using System.Collections.Generic;
using System.Numerics;
using System.Numerics.Tensors;
using System.Text;
using System.Threading;

using InteropBitmaps;

namespace InteropTensors
{
    public interface IDenseTensor
    {
        String Name { get; }
        ReadOnlySpan<int> Dimensions { get; }
    }

    /// <summary>
    /// Wraps a dense tensor; implemented by <see cref="SystemDenseTensor{T}"/> and <see cref="INativeDenseTensor{T}"/>
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <see href="https://github.com/dotnet/runtime/issues/28867"/>
    public interface IDenseTensor<T> : IDenseTensor
        where T:unmanaged
    {          
        Span<T> Span { get; }
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
