// Copyright (c) InteropTypes 2024 Vicente Penades

#nullable disable

using System;

using InteropTypes.Tensors;
using InteropTypes.Tensors.Imaging;

using ONNXTENSORS = Microsoft.ML.OnnxRuntime.Tensors;

#if INTEROPTYPES_USEINTEROPNAMESPACE
namespace InteropTypes.Tensors
#elif INTEROPTYPES_TENSORS_USEONNXRUNTIMENAMESPACE
namespace Microsoft.ML.OnnxRuntime
#else
namespace $rootnamespace$
#endif
{
    static partial class InteropTensorsForOnnxRuntime
    {
        public static TensorBitmap<T> AsTensorBitmap<T>(this ONNXTENSORS.DenseTensor<T> tensor, ColorEncoding encoding, ColorRanges ranges)
            where T:unmanaged, IConvertible
        {
            var tmp = AsSpanTensor3(tensor);
            return new TensorBitmap<T>(tmp, encoding, ranges);
        }

        public static SpanTensor1<T> AsSpanTensor1<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.IsReadOnly) throw new ArgumentException("Tensor is ReadOnly", nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 1) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor1<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static SpanTensor2<T> AsSpanTensor2<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.IsReadOnly) throw new ArgumentException("Tensor is ReadOnly", nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 2) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor2<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static SpanTensor3<T> AsSpanTensor3<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.IsReadOnly) throw new ArgumentException("Tensor is ReadOnly", nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 3) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor3<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static SpanTensor4<T> AsSpanTensor4<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.IsReadOnly) throw new ArgumentException("Tensor is ReadOnly", nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 4) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor4<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static SpanTensor5<T> AsSpanTensor5<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.IsReadOnly) throw new ArgumentException("Tensor is ReadOnly", nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 5) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor5<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static ReadOnlySpanTensor1<T> AsReadOnlySpanTensor1<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 1) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor1<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static ReadOnlySpanTensor2<T> AsReadOnlySpanTensor2<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 2) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor2<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static ReadOnlySpanTensor3<T> AsReadOnlySpanTensor3<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 3) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor3<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static ReadOnlySpanTensor4<T> AsReadOnlySpanTensor4<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 4) throw new ArgumentException("Dimensions mismatch");
            return new SpanTensor4<T>(tensor.Buffer.Span, tensor.Dimensions);
        }

        public static ReadOnlySpanTensor5<T> AsReadOnlySpanTensor5<T>(this ONNXTENSORS.DenseTensor<T> tensor) where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            if (tensor.Dimensions.Length != 5) throw new ArgumentException("Dimensions mismatch");
            return new ReadOnlySpanTensor5<T>(tensor.Buffer.Span, tensor.Dimensions);
        }        
    }
}
