using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InteropVision;

using ONNX = Microsoft.ML.OnnxRuntime;

namespace InteropWith
{
    public class OnnxModel : IModelGraph, IServiceProvider
    {
        #region lifecycle

        public static OnnxModel FromFile(string filePath)
        {
            filePath = System.IO.Path.GetFullPath(filePath);

            byte[] func() => System.IO.File.ReadAllBytes(filePath);

            return new OnnxModel(func);
        }

        public static OnnxModel FromBytes(byte[] model)
        {
            byte[] func() => model;

            return new OnnxModel(func);
        }

        private OnnxModel(Func<Byte[]> modelLoader)
        {
            _Model = new Lazy<byte[]>(modelLoader);
        }

        public void Dispose()
        {
            System.Threading.Interlocked.Exchange(ref _Options, null)?.Dispose();
            _Model = null;
        }

        #endregion

        #region data

        private Lazy<Byte[]> _Model;

        private OnnxOptions _Options = new OnnxOptions();

        #endregion

        #region properties        

        /// <inheritdoc/> 
        public string ModelSha256 => _Model.Value.GetSha256();

        /// <inheritdoc/> 
        public TensorImageSettings InputSettings { get; set; }

        /// <summary>
        /// Gets or set the hardware device to use when creating the session.
        /// </summary>
        /// <remarks>
        /// -1 will default to CPU, 0 will use first hardware device.
        /// </remarks>
        public static int DeviceID
        {
            get => OnnxOptions.DeviceID;
            set => OnnxOptions.DeviceID = value;
        }

        #endregion

        #region API

        /// <inheritdoc/> 
        public IModelSession CreateSession()
        {            
            return new OnnxSession(_Model.Value, _Options.Options);
        }

        /// <inheritdoc/> 
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ONNX.OrtEnv)) return ONNX.OrtEnv.Instance();
            if (serviceType == typeof(ONNX.SessionOptions)) return _Options.Options;
            throw new NotSupportedException();
        }

        #endregion
    }
}
