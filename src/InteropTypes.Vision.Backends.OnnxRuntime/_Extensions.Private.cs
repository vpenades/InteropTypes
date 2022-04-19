using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ONNX = Microsoft.ML.OnnxRuntime;

namespace InteropTypes.Vision.Backends
{
    static class _PrivateExtensions
    {
        public static Tensors.ITensor WrapAsInterface(this ONNX.NamedOnnxValue namedValue)
        {
            Tensors.ITensor tensor;

            if (_TryWrap<Single>(namedValue, out tensor)) return tensor;
            if (_TryWrap<Double>(namedValue, out tensor)) return tensor;

            if (_TryWrap<Char>(namedValue, out tensor)) return tensor;
            if (_TryWrap<Boolean>(namedValue, out tensor)) return tensor;

            if (_TryWrap<Byte>(namedValue, out tensor)) return tensor;
            if (_TryWrap<SByte>(namedValue, out tensor)) return tensor;
            if (_TryWrap<UInt16>(namedValue, out tensor)) return tensor;
            if (_TryWrap<Int16>(namedValue, out tensor)) return tensor;
            if (_TryWrap<UInt32>(namedValue, out tensor)) return tensor;
            if (_TryWrap<Int32>(namedValue, out tensor)) return tensor;
            if (_TryWrap<UInt64>(namedValue, out tensor)) return tensor;
            if (_TryWrap<Int64>(namedValue, out tensor)) return tensor;            

            throw new NotSupportedException();
        }

        private static bool _TryWrap<T>(this ONNX.NamedOnnxValue namedValue, out Tensors.ITensor tensor)
            where T:unmanaged
        {
            if (namedValue.Value is ONNX.Tensors.DenseTensor<T> tt)
            {
                tensor = new OnnxDenseTensor<T>(tt, namedValue.Name);
                return true;
            }

            tensor = null;
            return false;
        }

        public static void CopyToByName(this ONNX.IDisposableReadOnlyCollection<ONNX.DisposableNamedOnnxValue> srcCollection, ONNX.NamedOnnxValue[] dstCollection)
        {
            foreach (var src in srcCollection)
            {
                var idx = Array.FindIndex(dstCollection, t => t.Name == src.Name);
                if (idx < 0) throw new InvalidOperationException($"{src.Name} not found in output tensors");

                dstCollection[idx] = src.Clone();
            }
        }

        public static ONNX.NamedOnnxValue Clone(this ONNX.NamedOnnxValue src)
        {
            if (src == null) return null;

            if (src.Value == null) return ONNX.NamedOnnxValue.CreateFromTensor<Byte>(src.Name, null);

            ONNX.NamedOnnxValue dst;

            return src.TryClone(out dst)
                ? dst
                : throw new InvalidOperationException($"Can't clone tensor {src.Value}");
        }

        public static bool TryClone(this ONNX.NamedOnnxValue src, out ONNX.NamedOnnxValue dst)
        {
            if (_TryClone<Single>(src, out dst)) return true;
            if (_TryClone<Double>(src, out dst)) return true;

            if (_TryClone<Boolean>(src, out dst)) return true;
            if (_TryClone<Char>(src, out dst)) return true;

            if (_TryClone<Byte>(src, out dst)) return true;
            if (_TryClone<SByte>(src, out dst)) return true;            
            if (_TryClone<Int16>(src, out dst)) return true;
            if (_TryClone<UInt16>(src, out dst)) return true;
            if (_TryClone<Int32>(src, out dst)) return true;
            if (_TryClone<UInt32>(src, out dst)) return true;
            if (_TryClone<Int64>(src, out dst)) return true;
            if (_TryClone<UInt64>(src, out dst)) return true;

            dst = null;
            return false;
        }

        private static bool _TryClone<TValue>(this ONNX.NamedOnnxValue src, out ONNX.NamedOnnxValue dst)
            where TValue : unmanaged
        {
            if (src.Value is ONNX.Tensors.DenseTensor<TValue> srcTensor)
            {
                var dstTensor = new ONNX.Tensors.DenseTensor<TValue>(srcTensor.Dimensions);
                srcTensor.Buffer.CopyTo(dstTensor.Buffer);
                dst = ONNX.NamedOnnxValue.CreateFromTensor(src.Name, dstTensor);
                return true;
            }            

            dst = null;
            return false;
        }

        public static ONNX.NamedOnnxValue CreateNamedOnnexValue(this ONNX.NodeMetadata metadata, string name)
        {
            System.Diagnostics.Debug.Assert(metadata.OnnxValueType == ONNX.OnnxValueType.ONNX_TYPE_TENSOR);

            if (metadata.IsTensor)
            {
                if (metadata.Dimensions.Any(item => item < 0))
                {
                    // it's a dynamic tensor
                    return ONNX.NamedOnnxValue.CreateFromTensor<float>(name, null);
                }

                if (metadata.ElementType == typeof(Byte))
                {
                    var denseTensor = new ONNX.Tensors.DenseTensor<Byte>(metadata.Dimensions);
                    return ONNX.NamedOnnxValue.CreateFromTensor(name, denseTensor);
                }

                if (metadata.ElementType == typeof(int))
                {
                    var denseTensor = new ONNX.Tensors.DenseTensor<int>(metadata.Dimensions);
                    return ONNX.NamedOnnxValue.CreateFromTensor(name, denseTensor);
                }

                if (metadata.ElementType == typeof(long))
                {
                    var denseTensor = new ONNX.Tensors.DenseTensor<long>(metadata.Dimensions);
                    return ONNX.NamedOnnxValue.CreateFromTensor(name, denseTensor);
                }

                if (metadata.ElementType == typeof(float))
                {
                    var denseTensor = new ONNX.Tensors.DenseTensor<float>(metadata.Dimensions);
                    return ONNX.NamedOnnxValue.CreateFromTensor(name, denseTensor);
                }

                if (metadata.ElementType == typeof(double))
                {
                    var denseTensor = new ONNX.Tensors.DenseTensor<double>(metadata.Dimensions);
                    return ONNX.NamedOnnxValue.CreateFromTensor(name, denseTensor);
                }
            }

            throw new NotImplementedException();
        }
    }
}
