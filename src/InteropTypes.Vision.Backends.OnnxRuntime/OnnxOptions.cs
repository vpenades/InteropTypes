using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ONNX = Microsoft.ML.OnnxRuntime;

namespace InteropTypes.Vision.Backends
{
    public class OnnxOptions : IDisposable
    {
        #region lifecycle

        internal OnnxOptions() { }

        public void Dispose()
        {
            var opts = System.Threading.Interlocked.Exchange(ref _Options, null);

            if (opts != null && opts.IsValueCreated)
            {
                opts.Value.Dispose();
            }

            _Options = null;
        }

        #endregion

        #region data

        public static int DeviceID { get; set; } = -1;

        private Lazy<ONNX.SessionOptions> _Options = new Lazy<ONNX.SessionOptions>(_CreateOptions);

        #endregion

        #region API

        public ONNX.SessionOptions Options => _Options.Value;

        public static IReadOnlyList<string> GetProviders()
        {
            return ONNX.OrtEnv.Instance().GetAvailableProviders();
        }

        private static ONNX.SessionOptions _CreateOptions()
        {
            if (DeviceID < 0)
            {
                return _CreateOptionsCPU();
            }

            return CreateOptionsDirectML(DeviceID);
        }

        private static ONNX.SessionOptions _CreateOptionsCPU()
        {
            if (!GetProviders().Contains("CPUExecutionProvider")) throw new InvalidOperationException("CPU device not supported");

            var options = new ONNX.SessionOptions();            
            // options.AppendExecutionProvider_CPU(); // it seems this is not needed for CPU
            return options;

            // apparently everything below here is actually slower and blocks the computer!
            options.ExecutionMode = ONNX.ExecutionMode.ORT_PARALLEL;
            options.EnableMemoryPattern = true;

            options.IntraOpNumThreads = 2;
            options.InterOpNumThreads = 6;
            options.AppendExecutionProvider_CPU();

            // options.IntraOpNumThreads = 2;
            // options.InterOpNumThreads = 6;
            // options.AppendExecutionProvider_CPU(0);
            return options;
        }

        private static ONNX.SessionOptions CreateOptionsDirectML(int deviceId)
        {
            if (!GetProviders().Contains("DmlExecutionProvider")) throw new InvalidOperationException("DirectML device not supported");

            var options = new ONNX.SessionOptions();
            options.AppendExecutionProvider_DML(deviceId);
            return options;
        }

        private static ONNX.SessionOptions _CreateOptionsDevice(int deviceId)
        {
            var options = new ONNX.SessionOptions();

            var providers = ONNX.OrtEnv.Instance().GetAvailableProviders();
            if (providers.Any(item => item == "DmlExecutionProvider")) options.AppendExecutionProvider_DML(0);
            return options;

            // return ONNX.SessionOptions.MakeSessionOptionWithCudaProvider(0);
            // return ONNX.SessionOptions.MakeSessionOptionWithTensorrtProvider(0);

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

        #endregion
    }
}
