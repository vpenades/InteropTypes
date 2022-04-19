using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InteropTypes.Tensors;

using ONNX = Microsoft.ML.OnnxRuntime;

namespace InteropTypes.Vision.Backends
{
    // https://github.com/microsoft/onnxruntime/blob/master/csharp/sample/Microsoft.ML.OnnxRuntime.InferenceSample/Program.cs
    // https://gist.github.com/pranavsharma/939d93fc7291c5a1638bfa3b190d72e3

    class OnnxSession : IModelSession
    {
        #region lifecycle

        internal OnnxSession(Byte[] model, ONNX.SessionOptions options)
        {
            _Session = new ONNX.InferenceSession(model, options);

            _InputMeta = _Session.InputMetadata.ToArray();
            _OutputMeta = _Session.OutputMetadata.ToArray();

            _Inputs = _InputMeta.Select(item => item.Value.CreateNamedOnnexValue(item.Key)).ToArray();
            _Outputs = _OutputMeta.Select(item => item.Value.CreateNamedOnnexValue(item.Key)).ToArray();
            _OutputNames = _Outputs.Select(item => item.Name).ToArray();
        }        

        

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _Session, null)?.Dispose();
        }

        #endregion

        #region data

        private ONNX.InferenceSession _Session;
        private readonly KeyValuePair<string, ONNX.NodeMetadata>[] _InputMeta;
        private readonly KeyValuePair<string, ONNX.NodeMetadata>[] _OutputMeta;

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

        public ITensor GetInputTensor(int idx) { return _Inputs[idx].WrapAsInterface(); }

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
            var rdims = _OutputMeta[idx].Value.Dimensions;

            for(int i=0; i < rdims.Length; ++i)
            {
                if (rdims[i] < 0) continue;
                if (rdims[i] != dims[i]) throw new ArgumentException(nameof(dims));
            }

            var output = _Outputs[idx];
            var denseTensor = _UpdateTensor<T>(ref output, dims);
            _Outputs[idx] = output;

            return new OnnxDenseTensor<T>(denseTensor, output.Name);
        }

        public void Inference()
        {
            var allNull = _Outputs.All(item => item.Value == null);

            if (allNull)
            {
                using (var outputs = _Session.Run(_Inputs))
                {
                    outputs.CopyToByName(_Outputs);
                }
            }
            else
            {
                _Session.Run(_Inputs, _Outputs);
            }            
        }

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
