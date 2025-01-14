// Copyright (c) InteropTypes 2024 Vicente Penades

#nullable disable

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

#nullable disable

using InteropTypes.Tensors;
using InteropTypes.Tensors.Imaging;

using ORTVALUE = Microsoft.ML.OnnxRuntime.OrtValue;
using ORTVALUEINFO = Microsoft.ML.OnnxRuntime.OrtTensorTypeAndShapeInfo;

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
        public static bool TryGetTensorBitmap<T>(this ORTVALUE value, ColorEncoding encoding,  out TensorBitmap<T> result)
            where T : unmanaged, IConvertible
        {
            return TryGetTensorBitmap(value, encoding, ColorRanges.Identity, out result);
        }

        public static bool TryGetTensorBitmap<T>(this ORTVALUE value, ColorEncoding encoding, ColorRanges ranges, out TensorBitmap<T> result)
            where T : unmanaged, IConvertible
        {
            if (!TryGetSpanTensor3<T>(value, out var t3)) { result = default; return false; }

            if (t3.Dimensions.Dim0 > 4 || t3.Dimensions.Dim2 > 4) { result = default; return false; }
            
            result = new TensorBitmap<T>(t3, encoding, ranges);
            return true;
        }

        public static ReadOnlySpanTensor1<T> AsReadOnlySpanTensor1<T>(this ORTVALUE value)
            where T: unmanaged
        {
            return TryGetReadOnlySpanTensor1<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetReadOnlySpanTensor1<T>(this ORTVALUE value, out ReadOnlySpanTensor1<T> tensor)
            where T:unmanaged
        {            
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();
            var dims = TensorSize1.FromAny(info.Shape.AsSpan());

            tensor = new ReadOnlySpanTensor1<T>(data, dims);
            return true;
        }        

        public static SpanTensor1<T> AsSpanTensor1<T>(this ORTVALUE value)
            where T : unmanaged
        {
            return TryGetSpanTensor1<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetSpanTensor1<T>(this ORTVALUE value, out SpanTensor1<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }

            var data = value.GetTensorMutableDataAsSpan<T>();
            var dims = TensorSize1.FromAny(info.Shape.AsSpan());

            tensor = new SpanTensor1<T>(data, dims);

            return true;
        }

        public static ReadOnlySpanTensor2<T> AsReadOnlySpanTensor2<T>(this ORTVALUE value)
            where T : unmanaged
        {
            return TryGetReadOnlySpanTensor2<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetReadOnlySpanTensor2<T>(this ORTVALUE value, out ReadOnlySpanTensor2<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();
            var dims = TensorSize2.FromAny(info.Shape.AsSpan());

            tensor = new ReadOnlySpanTensor2<T>(data, dims);

            return true;
        }

        public static SpanTensor2<T> AsSpanTensor2<T>(this ORTVALUE value)
            where T : unmanaged
        {
            return TryGetSpanTensor2<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetSpanTensor2<T>(this ORTVALUE value, out SpanTensor2<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }            

            var data = value.GetTensorMutableDataAsSpan<T>();
            var dims = TensorSize2.FromAny(info.Shape.AsSpan());

            tensor = new SpanTensor2<T>(data, dims);

            return true;
        }

        public static ReadOnlySpanTensor3<T> AsReadOnlySpanTensor3<T>(this ORTVALUE value)
            where T : unmanaged
        {
            return TryGetReadOnlySpanTensor3<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetReadOnlySpanTensor3<T>(this ORTVALUE value, out ReadOnlySpanTensor3<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();
            var dims = TensorSize3.FromAny(info.Shape.AsSpan());

            tensor = new ReadOnlySpanTensor3<T>(data, dims);

            return true;
        }

        public static SpanTensor3<T> AsSpanTensor3<T>(this ORTVALUE value)
            where T : unmanaged
        {
            return TryGetSpanTensor3<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetSpanTensor3<T>(this ORTVALUE value, out SpanTensor3<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }

            var data = value.GetTensorMutableDataAsSpan<T>();
            var dims = TensorSize3.FromAny(info.Shape.AsSpan());

            tensor = new SpanTensor3<T>(data, dims);

            return true;
        }

        public static ReadOnlySpanTensor4<T> AsReadOnlySpanTensor4<T>(this ORTVALUE value)
            where T : unmanaged
        {
            return TryGetReadOnlySpanTensor4<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetReadOnlySpanTensor4<T>(this ORTVALUE value, out ReadOnlySpanTensor4<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();
            var dims = TensorSize4.FromAny(info.Shape.AsSpan());

            tensor = new ReadOnlySpanTensor4<T>(data, dims);

            return true;
        }

        public static SpanTensor4<T> AsSpanTensor4<T>(this ORTVALUE value)
            where T : unmanaged
        {
            return TryGetSpanTensor4<T>(value, out var tensor)
                ? tensor
                : throw _GetTensorInfoException<T>(value, nameof(value));
        }

        public static bool TryGetSpanTensor4<T>(this ORTVALUE value, out SpanTensor4<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTensorInfo<T>(value, out var info)) { tensor = default; return false; }

            var data = value.GetTensorMutableDataAsSpan<T>();
            var dims = TensorSize4.FromAny(info.Shape.AsSpan());

            tensor = new SpanTensor4<T>(data, dims);

            return true;
        }

        private static bool _TryGetTensorInfo<T>(ORTVALUE value, out ORTVALUEINFO info)
        {
            info = default;
            if (value == null) return false;
            if (!value.IsTensor) return false;
            if (value.IsSparseTensor) return false;            

            info = value.GetTensorTypeAndShape();

            var dataType = info.ElementDataType.GetElementType();

            if (dataType != typeof(T)) return false;

            return true;
        }

        private static Exception _GetTensorInfoException<T>(ORTVALUE value, string name)
        {
            
            if (value == null) return new ArgumentNullException(name);
            if (!value.IsTensor) return new ArgumentException("Not a tensor", name);
            if (value.IsSparseTensor) return new ArgumentException("Not dense tensor", name);

            var info = value.GetTensorTypeAndShape();

            var dataType = info.ElementDataType.GetElementType();

            if (dataType != typeof(T)) return new ArgumentException($"Type mismatch, expected: {info.ElementDataType} but was {typeof(T).Name}", name);

            return new ArgumentException("Error", name);
        }
    }

}