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

        private static ONNX.SessionOptions _CreateOptions(int deviceId)
        {
            var providers = ONNX.OrtEnv.Instance().GetAvailableProviders();

            var options = new ONNX.SessionOptions();
            // options.AppendExecutionProvider_DML(0);
            return options;

            // return ONNX.SessionOptions.MakeSessionOptionWithCudaProvider(0);

            return ONNX.SessionOptions.MakeSessionOptionWithTensorrtProvider(0);

            /*
            var options = new ONNX.SessionOptions();
            
            options = new ONNX.SessionOptions
            {
                LogSeverityLevel = ONNX.OrtLoggingLevel.ORT_LOGGING_LEVEL_INFO,
                GraphOptimizationLevel = ONNX.GraphOptimizationLevel.ORT_ENABLE_ALL,
                ExecutionMode = deviceId < 0 ?
                    ONNX.ExecutionMode.ORT_PARALLEL :
                    ONNX.ExecutionMode.ORT_SEQUENTIAL,
                EnableMemoryPattern = deviceId < 0
            };

            if (deviceId >= 0)
            {
                options.AppendExecutionProvider_CUDA(deviceId);
                // options.a(deviceId);
            }

            
            else
            {
                options.IntraOpNumThreads = 2;
                options.ExecutionMode = ONNX.ExecutionMode.ORT_PARALLEL;
                options.InterOpNumThreads = 6;
                options.GraphOptimizationLevel = ONNX.GraphOptimizationLevel.ORT_ENABLE_ALL;                
                options.AppendExecutionProvider_CPU(0);
            }

            return options;
            */
        }

        internal OnnxSession(Byte[] model)
        {
            var options = _CreateOptions(0);

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

            if (input.Value is ONNX.Tensors.DenseTensor<T> denseTensor)
            {
                if (denseTensor.Dimensions.SequenceEqual(dims)) return new OnnxDenseTensor<T>(denseTensor, input.Name);
            }

            denseTensor = new ONNX.Tensors.DenseTensor<T>(dims);

            _Inputs[idx] = ONNX.NamedOnnxValue.CreateFromTensor(input.Name, denseTensor);

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

        public void Inference() { _Session.Run(_Inputs, _Outputs); }        

        #endregion
    }
}
