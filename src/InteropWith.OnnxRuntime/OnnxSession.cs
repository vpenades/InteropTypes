using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InteropTensors;

using InteropVision;

using ONNX = Microsoft.ML.OnnxRuntime;

namespace InteropWith
{
    // https://github.com/microsoft/onnxruntime/blob/master/csharp/sample/Microsoft.ML.OnnxRuntime.InferenceSample/Program.cs
    // https://gist.github.com/pranavsharma/939d93fc7291c5a1638bfa3b190d72e3

    class OnnxSession : IModelSession
    {
        #region lifecycle

        internal OnnxSession(Byte[] model, ONNX.SessionOptions options)
        {
            _Session = new ONNX.InferenceSession(model, options);

            _Inputs = _Session.InputMetadata.Select(item => _Create(item.Key, item.Value)).ToArray();
            _Outputs = _Session.OutputMetadata.Select(item => _Create(item.Key, item.Value)).ToArray();
            _OutputNames = _Outputs.Select(item => item.Name).ToArray();
        }             

        private static ONNX.NamedOnnxValue _Create(string name, ONNX.NodeMetadata metadata)
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

                if (metadata.ElementType == typeof(float))
                {                    
                    var denseTensor = new ONNX.Tensors.DenseTensor<float>(metadata.Dimensions);
                    return ONNX.NamedOnnxValue.CreateFromTensor(name, denseTensor);                    
                }                
            }            

            throw new NotImplementedException();
        }

        private static IDenseTensor _Create(ONNX.NamedOnnxValue namedValue)
        {
            if (namedValue.Value is ONNX.Tensors.DenseTensor<byte> tbyte) return new OnnxDenseTensor<byte>(tbyte, namedValue.Name);
            if (namedValue.Value is ONNX.Tensors.DenseTensor<int> tint32) return new OnnxDenseTensor<int>(tint32, namedValue.Name);
            if (namedValue.Value is ONNX.Tensors.DenseTensor<float> tfloat) return new OnnxDenseTensor<float>(tfloat, namedValue.Name);
            throw new NotSupportedException();           
        }

        public void Dispose()
        {
            _Session?.Dispose();
            _Session = null;
        }

        #endregion

        #region data

        private ONNX.InferenceSession _Session;
        private readonly ONNX.NamedOnnxValue[] _Inputs;
        private readonly ONNX.NamedOnnxValue[] _Outputs;
        private readonly string[] _OutputNames;

        // private readonly IDenseTensor[] _InputTensors;
        // private readonly IDenseTensor[] _OutputTensors;

        #endregion

        #region API

        public IReadOnlyList<string> OutputNames => _OutputNames;

        public IDenseTensor<T> UseInputTensor<T>(int idx, params int[] dims) where T : unmanaged
        {
            var input = _Inputs[idx];
            var denseTensor = _UpdateTensor<T>(ref input, dims);
            _Inputs[idx] = input;

            return new OnnxDenseTensor<T>(denseTensor, input.Name);
        }

        public IDenseTensor GetInputTensor(int idx) { return _Create(_Inputs[idx]); }

        public IDenseTensor<T> GetInputTensor<T>(int idx) where T : unmanaged
        {
            var input = _Inputs[idx];
            var denseTensor = input.Value as ONNX.Tensors.DenseTensor<T>;
            return new OnnxDenseTensor<T>(denseTensor, input.Name);
        }

        public IDenseTensor<T> GetOutputTensor<T>(int idx) where T : unmanaged
        {
            var output = _Outputs[idx];
            var denseTensor = output.Value as ONNX.Tensors.DenseTensor<T>;
            return new OnnxDenseTensor<T>(denseTensor, output.Name);
        }

        public IDenseTensor<T> UseOutputTensor<T>(int idx, params int[] dims) where T : unmanaged
        {
            var output = _Outputs[idx];
            var denseTensor = _UpdateTensor<T>(ref output, dims);
            _Outputs[idx] = output;

            return new OnnxDenseTensor<T>(denseTensor, output.Name);
        }

        public void Inference() { _Session.Run(_Inputs, _Outputs); }

        #endregion

        #region helpers

        private ONNX.Tensors.DenseTensor<T> _UpdateTensor<T>(ref ONNX.NamedOnnxValue namedValue, params int[] dims) where T:unmanaged
        {
            if (namedValue.Value is ONNX.Tensors.DenseTensor<T> denseTensor)
            {
                if (denseTensor.Dimensions.SequenceEqual(dims)) return denseTensor;
            }

            denseTensor = new ONNX.Tensors.DenseTensor<T>(dims);

            namedValue = ONNX.NamedOnnxValue.CreateFromTensor(namedValue.Name, denseTensor);

            return denseTensor;
        }

        #endregion
    }
}
