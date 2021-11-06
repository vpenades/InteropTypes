using System;
using System.Collections.Generic;
using System.Text;

using InteropVision;

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
            
        }

        #endregion

        #region data

        private readonly Lazy<Byte[]> _Model;

        #endregion

        #region properties

        public string ModelSha256 => _Model.Value.GetSha256();

        public TensorImageSettings InputSettings { get; set; }

        #endregion

        #region API

        public IModelSession CreateSession()
        {            
            return new OnnxSession(_Model.Value);
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
