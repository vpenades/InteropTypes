// Copyright (c) InteropTypes 2024 Vicente Penades

#nullable disable

using System;
using System.Linq;

using ONNX = Microsoft.ML.OnnxRuntime;
using ONNXTENSORS = Microsoft.ML.OnnxRuntime.Tensors;

using NAMEDVALUE = Microsoft.ML.OnnxRuntime.NamedOnnxValue;
using TENSORBASE = Microsoft.ML.OnnxRuntime.Tensors.TensorBase;

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
        public static Type GetElementType(this TENSORBASE tensor)
        {
            return tensor.GetTypeInfo().ElementType.GetElementType();
        }

        /// <summary>
        /// Gets the length of a specific dimension.
        /// </summary>
        /// <param name="tensor">the input tensor</param>
        /// <param name="index">the dimensionIndex</param>
        /// <returns></returns>
        public static int GetLength(this TENSORBASE tensor, int dimensionIndex)
        {
            var dims = tensor.GetDimensions();
            if (dims.Length <= dimensionIndex) return 0;
            return dims[dimensionIndex];
        }

        public static ReadOnlySpan<int> GetDimensions(this TENSORBASE tensor)
        {
            switch (tensor)
            {
                case null: throw new ArgumentNullException(nameof(tensor));
                case ONNXTENSORS.Tensor<Boolean> typedTensor: return typedTensor.Dimensions;
                case ONNXTENSORS.Tensor<Char> typedTensor: return typedTensor.Dimensions;

                case ONNXTENSORS.Tensor<SByte> typedTensor: return typedTensor.Dimensions;
                case ONNXTENSORS.Tensor<Byte> typedTensor: return typedTensor.Dimensions;
                
                case ONNXTENSORS.Tensor<Int16> typedTensor: return typedTensor.Dimensions;
                case ONNXTENSORS.Tensor<UInt16> typedTensor: return typedTensor.Dimensions;

                case ONNXTENSORS.Tensor<Int32> typedTensor: return typedTensor.Dimensions;
                case ONNXTENSORS.Tensor<UInt32> typedTensor: return typedTensor.Dimensions;

                case ONNXTENSORS.Tensor<Int64> typedTensor: return typedTensor.Dimensions;
                case ONNXTENSORS.Tensor<UInt64> typedTensor: return typedTensor.Dimensions;

                case ONNXTENSORS.Tensor<Half> typedTensor: return typedTensor.Dimensions;
                case ONNXTENSORS.Tensor<Single> typedTensor: return typedTensor.Dimensions;
                case ONNXTENSORS.Tensor<Double> typedTensor: return typedTensor.Dimensions;

                default: throw new NotImplementedException(tensor.GetType().Name);
            }
        }           

        public static bool TryCopyTo(this TENSORBASE src, TENSORBASE dst)
        {
            if (!src.GetDimensions().SequenceEqual(dst.GetDimensions())) return false;

            if (src is ONNXTENSORS.DenseTensor<Byte> srcByte && dst is ONNXTENSORS.DenseTensor<byte> dstByte)
            {
                srcByte.Buffer.CopyTo(dstByte.Buffer);
                return true;
            }

            if (src is ONNXTENSORS.DenseTensor<float> srcFloat && dst is ONNXTENSORS.DenseTensor<float> dstFloat)
            {
                srcFloat.Buffer.CopyTo(dstFloat.Buffer);
                return true;
            }

            return false;
        }

        public static ONNX.NodeMetadata CreateNodeMetadata(this TENSORBASE tensor)
        {
            var arg0 = ONNX.OnnxValueType.ONNX_TYPE_TENSOR;
            var arg1 = tensor.GetDimensions().ToArray();
            var arg2 = new string[arg1.Length];
            var arg3 = tensor.GetElementType();

            var metadata = Activator.CreateInstance(typeof(ONNX.NodeMetadata), arg0, arg1, arg2, arg3) as ONNX.NodeMetadata;

            return metadata;
        }

        public static NAMEDVALUE CreateNamedValue(this TENSORBASE tensor, string name)
        {
            switch(tensor)
            {
                case ONNXTENSORS.Tensor<Boolean> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case ONNXTENSORS.Tensor<SByte> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case ONNXTENSORS.Tensor<Byte> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case ONNXTENSORS.Tensor<Char> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case ONNXTENSORS.Tensor<Int16> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case ONNXTENSORS.Tensor<UInt16> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case ONNXTENSORS.Tensor<Int32> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case ONNXTENSORS.Tensor<UInt32> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case ONNXTENSORS.Tensor<Int64> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case ONNXTENSORS.Tensor<UInt64> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case ONNXTENSORS.Tensor<Half> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case ONNXTENSORS.Tensor<Single> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case ONNXTENSORS.Tensor<Double> typedTensor: return NAMEDVALUE.CreateFromTensor(name, typedTensor);

                default: throw new NotImplementedException(tensor.GetElementType().Name);
            }            
        }        

        public static bool TryCreateOnnxDenseTensor<T>(this (int, int, int, int) dims, ref ONNXTENSORS.DenseTensor<T> tensor)
        {
            var isSame = false;
            if (tensor != null)
            {
                isSame = true;
                if (tensor.Dimensions.Length != 4) isSame = false;
                else if (tensor.Dimensions[0] != dims.Item1) isSame = false;
                else if (tensor.Dimensions[1] != dims.Item2) isSame = false;
                else if (tensor.Dimensions[2] != dims.Item3) isSame = false;
                else if (tensor.Dimensions[3] != dims.Item4) isSame = false;
            }

            if (!isSame)
            {
                tensor = new ONNXTENSORS.DenseTensor<T>(new int[] { dims.Item1, dims.Item2, dims.Item3, dims.Item4 });
                return true;
            }

            return false;
        }

        public static TENSORBASE GetSubTensor(this TENSORBASE tensor, int idx)
        {
            switch(tensor)
            {
                case null: throw new ArgumentNullException(nameof(tensor));

                case ONNXTENSORS.DenseTensor<Boolean> t: return t.GetSubTensor(idx);
                case ONNXTENSORS.DenseTensor<Char> t: return t.GetSubTensor(idx);

                case ONNXTENSORS.DenseTensor<Byte> t: return t.GetSubTensor(idx);
                case ONNXTENSORS.DenseTensor<SByte> t: return t.GetSubTensor(idx);                

                case ONNXTENSORS.DenseTensor<Int16> t: return t.GetSubTensor(idx);
                case ONNXTENSORS.DenseTensor<UInt16> t: return t.GetSubTensor(idx);

                case ONNXTENSORS.DenseTensor<Int32> t: return t.GetSubTensor(idx);
                case ONNXTENSORS.DenseTensor<UInt32> t: return t.GetSubTensor(idx);

                case ONNXTENSORS.DenseTensor<Int64> t: return t.GetSubTensor(idx);
                case ONNXTENSORS.DenseTensor<UInt64> t: return t.GetSubTensor(idx);

                case ONNXTENSORS.DenseTensor<Half> t: return t.GetSubTensor(idx);
                case ONNXTENSORS.DenseTensor<Single> t: return t.GetSubTensor(idx);
                case ONNXTENSORS.DenseTensor<Double> t: return t.GetSubTensor(idx);
            }

            throw new NotImplementedException();
        }

        public static ONNXTENSORS.DenseTensor<T> GetSubTensor<T>(this ONNXTENSORS.DenseTensor<T> tensor, int idx)
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            int len = 1; for (int i = 1; i < tensor.Dimensions.Length; ++i) len *= tensor.Dimensions[i];
            var subBuffer = tensor.Buffer.Slice(idx * len, len);
            return new ONNXTENSORS.DenseTensor<T>(subBuffer, tensor.Dimensions.Slice(1));
        }        
    }
}
