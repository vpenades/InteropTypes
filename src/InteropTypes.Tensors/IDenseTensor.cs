﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Numerics.Tensors;
using System.Text;
using System.Threading;

namespace InteropTypes.Tensors
{
    /// <summary>
    /// Declares an object implementing this interface as a Dense Tensor.
    /// </summary>
    public interface ITensor
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
    public interface IDenseTensor<T> : ITensor
        where T:unmanaged
    {
        /// <summary>
        /// Gets the flattened data span of the tensor.
        /// </summary>
        Span<T> Span { get; }              
    }

    /// <summary>
    /// Declares an object implementing this interface as a native Dense Tensor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INativeDenseTensor<T> : IDenseTensor<T>, IDisposable
        where T : unmanaged
    {
        IntPtr DataPointer { get; }

        int ByteSize { get; }        
    }    
}
