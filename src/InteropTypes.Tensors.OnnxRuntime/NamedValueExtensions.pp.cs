// Copyright (c) InteropTypes 2024 Vicente Penades

#nullable disable

using System;
using System.Linq;
using System.Numerics;

using __ONNX = Microsoft.ML.OnnxRuntime;
using __ONNXTENSORS = Microsoft.ML.OnnxRuntime.Tensors;

using __NAMEDVALUE = Microsoft.ML.OnnxRuntime.NamedOnnxValue;

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
        public static Type GetElementType(this __ONNXTENSORS.TensorElementType etype)
        {
            switch (etype)
            {
                case __ONNXTENSORS.TensorElementType.Bool: return typeof(Boolean);

                case __ONNXTENSORS.TensorElementType.Int8: return typeof(SByte);
                case __ONNXTENSORS.TensorElementType.UInt8: return typeof(Byte);

                case __ONNXTENSORS.TensorElementType.Int16: return typeof(Int16);
                case __ONNXTENSORS.TensorElementType.UInt16: return typeof(UInt16);

                case __ONNXTENSORS.TensorElementType.Int32: return typeof(Int32);
                case __ONNXTENSORS.TensorElementType.UInt32: return typeof(UInt32);

                case __ONNXTENSORS.TensorElementType.Int64: return typeof(Int64);
                case __ONNXTENSORS.TensorElementType.UInt64: return typeof(UInt64);

                case __ONNXTENSORS.TensorElementType.Float16: return typeof(Half);
                case __ONNXTENSORS.TensorElementType.Float: return typeof(Single);
                case __ONNXTENSORS.TensorElementType.Double: return typeof(Double);

                case __ONNXTENSORS.TensorElementType.String: return typeof(String);

                case __ONNXTENSORS.TensorElementType.Complex64: return typeof(Complex); //  needs checking

                default: throw new NotImplementedException(etype.ToString());
            }
        }

        public static __ONNXTENSORS.DenseTensor<T> AsDenseTensor<T>(this __NAMEDVALUE nvalue)
        {
            if (nvalue.Value is __ONNXTENSORS.DenseTensor<T> dtensor) return dtensor;

            return nvalue.AsTensor<T>().ToDenseTensor();
        }

        public static __NAMEDVALUE CreateNamedTensor(this __ONNX.NodeMetadata metadata, string name, ReadOnlySpan<int> dimensions)
        {
            if (dimensions.IsEmpty)
            {
                dimensions = metadata.Dimensions;

                if (metadata.Dimensions.Any(dim => dim <= 0))
                {
                    var array = dimensions.ToArray();
                    for (int i = 0; i < dimensions.Length; ++i)
                    {
                        if (array[i] <= 0) array[i] = 1;
                    }
                    dimensions = array;
                }
            }

            if (metadata.ElementType == typeof(Boolean)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Boolean>(dimensions));
            if (metadata.ElementType == typeof(Char)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Char>(dimensions));

            if (metadata.ElementType == typeof(SByte)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<SByte>(dimensions));
            if (metadata.ElementType == typeof(Byte)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Byte>(dimensions));

            if (metadata.ElementType == typeof(Int16)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Int16>(dimensions));
            if (metadata.ElementType == typeof(UInt16)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<UInt16>(dimensions));

            if (metadata.ElementType == typeof(Int32)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Int32>(dimensions));
            if (metadata.ElementType == typeof(UInt32)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<UInt32>(dimensions));

            if (metadata.ElementType == typeof(Int64)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Int64>(dimensions));
            if (metadata.ElementType == typeof(UInt64)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<UInt64>(dimensions));

            if (metadata.ElementType == typeof(Half)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Half>(dimensions));
            if (metadata.ElementType == typeof(Single)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Single>(dimensions));
            if (metadata.ElementType == typeof(Double)) return __NAMEDVALUE.CreateFromTensor(name, new __ONNXTENSORS.DenseTensor<Double>(dimensions));

            throw new NotImplementedException();
        }
    }

}