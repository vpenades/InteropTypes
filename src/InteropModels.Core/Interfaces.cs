using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using InteropBitmaps;
using InteropTensors;

namespace InteropModels
{
    public delegate IModelGraph ModelReadCallback(string name, Byte[] data, TensorImageSettings? settings);    

    

    public interface IModelGraph : IDisposable
    {
        TensorImageSettings InputSettings { get; set; }

        IModelSession CreateSession();
    }

    /// <summary>
    /// Wraps a runtime inference engine.
    /// </summary>
    public interface IModelSession : IDisposable
    {
        IDenseTensor GetInputTensor(int idx);

        IDenseTensor<T> GetInputTensor<T>(int idx) where T : unmanaged;        

        IDenseTensor<T> UseInputTensor<T>(int idx, params int[] dims) where T : unmanaged;        

        public void Inference();

        IReadOnlyList<string> OutputNames { get; }

        IDenseTensor<T> GetOutputTensor<T>(int idx) where T : unmanaged;
    }


    public interface IImageInference<T> : IDisposable
    {
        void Inference(T result, InferenceInput input, Rectangle? inputWindow = null);
    }

    public interface INarrowInference<T> : IImageInference<T>
    {
        RectangleF GetNextDetectionWindow(DetectedFrame t1, DetectedFrame? t0 = null);
    }



}
