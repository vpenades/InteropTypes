using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Tensors
{
    partial struct SpanTensor1<T>
    {
        public SpanTensor1(Span<T> data) : this(data, data.Length) { }        

        public int Length => _Dimensions.Dim0;

        public SpanTensor1<T> Slice(int start)
        {
            return new SpanTensor1<T>(_Buffer.Slice(start), Length - start);
        }

        public SpanTensor1<T> Slice(int start, int count)
        {
            return new SpanTensor1<T>(_Buffer.Slice(start, count), count);
        }

        public unsafe SpanTensor1<TElement> Cast<TElement>()
            where TElement : unmanaged
        {
            if (sizeof(T) * _Dimensions.StepSize != sizeof(TElement)) throw new ArgumentException("Element size mismatch", nameof(TElement));

            var xdata = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(_Buffer);

            var len = _Dimensions.StepSize * sizeof(T) / sizeof(TElement);

            return new SpanTensor1<TElement>(xdata, len);
        }        
    }

    partial struct ReadOnlySpanTensor1<T>
    {
        public ReadOnlySpanTensor1(ReadOnlySpan<T> data) : this(data, data.Length) { }

        public int Length => _Dimensions.Dim0;

        public ReadOnlySpanTensor1<T> Slice(int start)
        {
            return new ReadOnlySpanTensor1<T>(_Buffer.Slice(start), Length - start);
        }

        public ReadOnlySpanTensor1<T> Slice(int start, int count)
        {
            return new ReadOnlySpanTensor1<T>(_Buffer.Slice(start, count), count);
        }

        public unsafe ReadOnlySpanTensor1<TElement> Cast<TElement>()
            where TElement : unmanaged
        {
            if (sizeof(T) * _Dimensions.StepSize != sizeof(TElement)) throw new ArgumentException("Element size mismatch", nameof(TElement));

            var xdata = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(_Buffer);

            var len = _Dimensions.StepSize * sizeof(T) / sizeof(TElement);

            return new ReadOnlySpanTensor1<TElement>(xdata, len);
        }
    }
}
