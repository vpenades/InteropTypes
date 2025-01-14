// Copyright (c) InteropTypes 2024 Vicente Penades

#nullable disable

using System;
using System.Linq;
using System.Numerics;

using ONNX = Microsoft.ML.OnnxRuntime;
using ONNXTENSORS = Microsoft.ML.OnnxRuntime.Tensors;

using NAMEDVALUE = Microsoft.ML.OnnxRuntime.NamedOnnxValue;

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
        public static Type GetElementType(this ONNXTENSORS.TensorElementType etype)
        {
            switch (etype)
            {
                case ONNXTENSORS.TensorElementType.Bool: return typeof(Boolean);

                case ONNXTENSORS.TensorElementType.Int8: return typeof(SByte);
                case ONNXTENSORS.TensorElementType.UInt8: return typeof(Byte);

                case ONNXTENSORS.TensorElementType.Int16: return typeof(Int16);
                case ONNXTENSORS.TensorElementType.UInt16: return typeof(UInt16);

                case ONNXTENSORS.TensorElementType.Int32: return typeof(Int32);
                case ONNXTENSORS.TensorElementType.UInt32: return typeof(UInt32);

                case ONNXTENSORS.TensorElementType.Int64: return typeof(Int64);
                case ONNXTENSORS.TensorElementType.UInt64: return typeof(UInt64);

                case ONNXTENSORS.TensorElementType.Float16: return typeof(Half);
                case ONNXTENSORS.TensorElementType.Float: return typeof(Single);
                case ONNXTENSORS.TensorElementType.Double: return typeof(Double);

                case ONNXTENSORS.TensorElementType.String: return typeof(String);

                case ONNXTENSORS.TensorElementType.Complex64: return typeof(Complex); //  needs checking

                default: throw new NotImplementedException(etype.ToString());
            }
        }

        public static ONNXTENSORS.DenseTensor<T> AsDenseTensor<T>(this NAMEDVALUE nvalue)
        {
            if (nvalue.Value is ONNXTENSORS.DenseTensor<T> dtensor) return dtensor;

            return nvalue.AsTensor<T>().ToDenseTensor();
        }

        public static NAMEDVALUE CreateNamedTensor(this ONNX.NodeMetadata metadata, string name, ReadOnlySpan<int> dimensions)
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

            if (metadata.ElementType == typeof(Boolean)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Boolean>(dimensions));
            if (metadata.ElementType == typeof(Char)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Char>(dimensions));

            if (metadata.ElementType == typeof(SByte)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<SByte>(dimensions));
            if (metadata.ElementType == typeof(Byte)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Byte>(dimensions));

            if (metadata.ElementType == typeof(Int16)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Int16>(dimensions));
            if (metadata.ElementType == typeof(UInt16)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<UInt16>(dimensions));

            if (metadata.ElementType == typeof(Int32)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Int32>(dimensions));
            if (metadata.ElementType == typeof(UInt32)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<UInt32>(dimensions));

            if (metadata.ElementType == typeof(Int64)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Int64>(dimensions));
            if (metadata.ElementType == typeof(UInt64)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<UInt64>(dimensions));

            if (metadata.ElementType == typeof(Half)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Half>(dimensions));
            if (metadata.ElementType == typeof(Single)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Single>(dimensions));
            if (metadata.ElementType == typeof(Double)) return NAMEDVALUE.CreateFromTensor(name, new ONNXTENSORS.DenseTensor<Double>(dimensions));

            throw new NotImplementedException();
        }
    }

}