// Copyright (c) InteropTypes 2024 Vicente Penades

#nullable disable

using System;
using System.Linq;

using __ONNX = Microsoft.ML.OnnxRuntime;
using __ONNXTENSORS = Microsoft.ML.OnnxRuntime.Tensors;

using __NAMEDVALUE = Microsoft.ML.OnnxRuntime.NamedOnnxValue;
using __TENSORBASE = Microsoft.ML.OnnxRuntime.Tensors.TensorBase;

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
        public static Type GetElementType(this __TENSORBASE tensor)
        {
            return tensor.GetTypeInfo().ElementType.GetElementType();
        }

        /// <summary>
        /// Gets the length of a specific dimension.
        /// </summary>
        /// <param name="tensor">the input tensor</param>
        /// <param name="index">the dimensionIndex</param>
        /// <returns></returns>
        public static int GetLength(this __TENSORBASE tensor, int dimensionIndex)
        {
            var dims = tensor.GetDimensions();
            if (dims.Length <= dimensionIndex) return 0;
            return dims[dimensionIndex];
        }

        public static ReadOnlySpan<int> GetDimensions(this __TENSORBASE tensor)
        {
            switch (tensor)
            {
                case null: throw new ArgumentNullException(nameof(tensor));
                case __ONNXTENSORS.Tensor<Boolean> typedTensor: return typedTensor.Dimensions;
                case __ONNXTENSORS.Tensor<Char> typedTensor: return typedTensor.Dimensions;

                case __ONNXTENSORS.Tensor<SByte> typedTensor: return typedTensor.Dimensions;
                case __ONNXTENSORS.Tensor<Byte> typedTensor: return typedTensor.Dimensions;
                
                case __ONNXTENSORS.Tensor<Int16> typedTensor: return typedTensor.Dimensions;
                case __ONNXTENSORS.Tensor<UInt16> typedTensor: return typedTensor.Dimensions;

                case __ONNXTENSORS.Tensor<Int32> typedTensor: return typedTensor.Dimensions;
                case __ONNXTENSORS.Tensor<UInt32> typedTensor: return typedTensor.Dimensions;

                case __ONNXTENSORS.Tensor<Int64> typedTensor: return typedTensor.Dimensions;
                case __ONNXTENSORS.Tensor<UInt64> typedTensor: return typedTensor.Dimensions;

                case __ONNXTENSORS.Tensor<Half> typedTensor: return typedTensor.Dimensions;
                case __ONNXTENSORS.Tensor<Single> typedTensor: return typedTensor.Dimensions;
                case __ONNXTENSORS.Tensor<Double> typedTensor: return typedTensor.Dimensions;

                default: throw new NotImplementedException(tensor.GetType().Name);
            }
        }           

        public static bool TryCopyTo(this __TENSORBASE src, __TENSORBASE dst)
        {
            if (!src.GetDimensions().SequenceEqual(dst.GetDimensions())) return false;

            if (src is __ONNXTENSORS.DenseTensor<Byte> srcByte && dst is __ONNXTENSORS.DenseTensor<byte> dstByte)
            {
                srcByte.Buffer.CopyTo(dstByte.Buffer);
                return true;
            }

            if (src is __ONNXTENSORS.DenseTensor<float> srcFloat && dst is __ONNXTENSORS.DenseTensor<float> dstFloat)
            {
                srcFloat.Buffer.CopyTo(dstFloat.Buffer);
                return true;
            }

            return false;
        }

        public static __ONNX.NodeMetadata CreateNodeMetadata(this __TENSORBASE tensor)
        {
            var arg0 = __ONNX.OnnxValueType.ONNX_TYPE_TENSOR;
            var arg1 = tensor.GetDimensions().ToArray();
            var arg2 = new string[arg1.Length];
            var arg3 = tensor.GetElementType();

            var metadata = Activator.CreateInstance(typeof(__ONNX.NodeMetadata), arg0, arg1, arg2, arg3) as __ONNX.NodeMetadata;

            return metadata;
        }

        public static __NAMEDVALUE CreateNamedValue(this __TENSORBASE tensor, string name)
        {
            switch(tensor)
            {
                case __ONNXTENSORS.Tensor<Boolean> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case __ONNXTENSORS.Tensor<SByte> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case __ONNXTENSORS.Tensor<Byte> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case __ONNXTENSORS.Tensor<Char> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case __ONNXTENSORS.Tensor<Int16> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case __ONNXTENSORS.Tensor<UInt16> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case __ONNXTENSORS.Tensor<Int32> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case __ONNXTENSORS.Tensor<UInt32> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case __ONNXTENSORS.Tensor<Int64> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case __ONNXTENSORS.Tensor<UInt64> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);

                case __ONNXTENSORS.Tensor<Half> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case __ONNXTENSORS.Tensor<Single> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);
                case __ONNXTENSORS.Tensor<Double> typedTensor: return __NAMEDVALUE.CreateFromTensor(name, typedTensor);

                default: throw new NotImplementedException(tensor.GetElementType().Name);
            }            
        }        

        public static bool TryCreateOnnxDenseTensor<T>(this (int, int, int, int) dims, ref __ONNXTENSORS.DenseTensor<T> tensor)
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
                tensor = new __ONNXTENSORS.DenseTensor<T>(new int[] { dims.Item1, dims.Item2, dims.Item3, dims.Item4 });
                return true;
            }

            return false;
        }

        public static __TENSORBASE GetSubTensor(this __TENSORBASE tensor, int idx)
        {
            switch(tensor)
            {
                case null: throw new ArgumentNullException(nameof(tensor));

                case __ONNXTENSORS.DenseTensor<Boolean> t: return t.GetSubTensor(idx);
                case __ONNXTENSORS.DenseTensor<Char> t: return t.GetSubTensor(idx);

                case __ONNXTENSORS.DenseTensor<Byte> t: return t.GetSubTensor(idx);
                case __ONNXTENSORS.DenseTensor<SByte> t: return t.GetSubTensor(idx);                

                case __ONNXTENSORS.DenseTensor<Int16> t: return t.GetSubTensor(idx);
                case __ONNXTENSORS.DenseTensor<UInt16> t: return t.GetSubTensor(idx);

                case __ONNXTENSORS.DenseTensor<Int32> t: return t.GetSubTensor(idx);
                case __ONNXTENSORS.DenseTensor<UInt32> t: return t.GetSubTensor(idx);

                case __ONNXTENSORS.DenseTensor<Int64> t: return t.GetSubTensor(idx);
                case __ONNXTENSORS.DenseTensor<UInt64> t: return t.GetSubTensor(idx);

                case __ONNXTENSORS.DenseTensor<Half> t: return t.GetSubTensor(idx);
                case __ONNXTENSORS.DenseTensor<Single> t: return t.GetSubTensor(idx);
                case __ONNXTENSORS.DenseTensor<Double> t: return t.GetSubTensor(idx);
            }

            throw new NotImplementedException();
        }

        public static __ONNXTENSORS.DenseTensor<T> GetSubTensor<T>(this __ONNXTENSORS.DenseTensor<T> tensor, int idx)
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.IsReversedStride) throw new ArgumentException("reversed strides");

            int len = 1; for (int i = 1; i < tensor.Dimensions.Length; ++i) len *= tensor.Dimensions[i];
            var subBuffer = tensor.Buffer.Slice(idx * len, len);
            return new __ONNXTENSORS.DenseTensor<T>(subBuffer, tensor.Dimensions.Slice(1));
        }        
    }
}
