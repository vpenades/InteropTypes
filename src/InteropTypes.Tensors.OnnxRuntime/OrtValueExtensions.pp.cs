// Copyright (c) InteropTypes 2024 Vicente Penades

#nullable disable

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.CompilerServices;

using InteropTypes.Tensors;
using Microsoft.ML.OnnxRuntime;

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
        public static ReadOnlySpanTensor1<T> AsReadOnlySpanTensor1<T>(this OrtValue value)
            where T: unmanaged
        {
            return TryGetReadOnlySpanTensor1<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetReadOnlySpanTensor1<T>(this OrtValue value, out ReadOnlySpanTensor1<T> tensor)
            where T:unmanaged
        {            
            if (!_TryGetTypeAndShape(value, 1, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();
            tensor = new ReadOnlySpanTensor1<T>(data, (int)info.Shape[0]);
            return true;
        }        

        public static SpanTensor1<T> AsSpanTensor1<T>(this OrtValue value)
            where T : unmanaged
        {
            return TryGetSpanTensor1<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetSpanTensor1<T>(this OrtValue value, out SpanTensor1<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTypeAndShape(value, 1, out var info)) { tensor = default; return false; }

            var data = value.GetTensorMutableDataAsSpan<T>();

            tensor = new SpanTensor1<T>(data, (int)info.Shape[0]);

            return true;
        }

        public static ReadOnlySpanTensor2<T> AsReadOnlySpanTensor2<T>(this OrtValue value)
            where T : unmanaged
        {
            return TryGetReadOnlySpanTensor2<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetReadOnlySpanTensor2<T>(this OrtValue value, out ReadOnlySpanTensor2<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTypeAndShape(value, 2, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();

            tensor = new ReadOnlySpanTensor2<T>(data, (int)info.Shape[0], (int)info.Shape[1]);

            return true;
        }

        public static SpanTensor2<T> AsSpanTensor2<T>(this OrtValue value)
            where T : unmanaged
        {
            return TryGetSpanTensor2<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetSpanTensor2<T>(this OrtValue value, out SpanTensor2<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTypeAndShape(value, 2, out var info)) { tensor = default; return false; }

            var data = value.GetTensorMutableDataAsSpan<T>();

            tensor = new SpanTensor2<T>(data, (int)info.Shape[0], (int)info.Shape[1]);

            return true;
        }

        public static ReadOnlySpanTensor3<T> AsReadOnlySpanTensor3<T>(this OrtValue value)
            where T : unmanaged
        {
            return TryGetReadOnlySpanTensor3<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetReadOnlySpanTensor3<T>(this OrtValue value, out ReadOnlySpanTensor3<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTypeAndShape(value, 3, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();

            tensor = new ReadOnlySpanTensor3<T>(data, (int)info.Shape[0], (int)info.Shape[1], (int)info.Shape[2]);

            return true;
        }

        public static SpanTensor3<T> AsSpanTensor3<T>(this OrtValue value)
            where T : unmanaged
        {
            return TryGetSpanTensor3<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetSpanTensor3<T>(this OrtValue value, out SpanTensor3<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTypeAndShape(value, 3, out var info)) { tensor = default; return false; }

            var data = value.GetTensorMutableDataAsSpan<T>();

            tensor = new SpanTensor3<T>(data, (int)info.Shape[0], (int)info.Shape[1], (int)info.Shape[2]);

            return true;
        }

        public static ReadOnlySpanTensor4<T> AsReadOnlySpanTensor4<T>(this OrtValue value)
            where T : unmanaged
        {
            return TryGetReadOnlySpanTensor4<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetReadOnlySpanTensor4<T>(this OrtValue value, out ReadOnlySpanTensor4<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTypeAndShape(value, 4, out var info)) { tensor = default; return false; }

            var data = value.GetTensorDataAsSpan<T>();

            tensor = new ReadOnlySpanTensor4<T>(data, (int)info.Shape[0], (int)info.Shape[1], (int)info.Shape[2], (int)info.Shape[3]);

            return true;
        }

        public static SpanTensor4<T> AsSpanTensor4<T>(this OrtValue value)
            where T : unmanaged
        {
            return TryGetSpanTensor4<T>(value, out var tensor) ? tensor : throw new ArgumentException("invalid tensor", nameof(OrtValue));
        }

        public static bool TryGetSpanTensor4<T>(this OrtValue value, out SpanTensor4<T> tensor)
            where T : unmanaged
        {
            if (!_TryGetTypeAndShape(value, 4, out var info)) { tensor = default; return false; }

            var data = value.GetTensorMutableDataAsSpan<T>();

            tensor = new SpanTensor4<T>(data, (int)info.Shape[0], (int)info.Shape[1], (int)info.Shape[2], (int)info.Shape[3]);

            return true;
        }

        private static bool _TryGetTypeAndShape(OrtValue value, int dims, out OrtTensorTypeAndShapeInfo info)
        {
            info = default;
            if (value == null) return false;
            if (!value.IsTensor) return false;
            if (value.IsSparseTensor) return false;

            info = value.GetTensorTypeAndShape();
            if (info.Shape.Length != dims) return false;
            return true;
        }
    }

}