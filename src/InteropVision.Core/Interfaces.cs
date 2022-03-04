using System;
using System.Collections.Generic;
using System.Drawing;

using InteropTypes.Graphics.Bitmaps;
using InteropTypes.Tensors;

namespace InteropTypes.Vision
{
    /// <summary>
    /// Loads of DNN model (TFLite, ONNX, etc).
    /// </summary>
    /// <param name="name">The name of the model</param>
    /// <param name="data">The actual model bytecode.</param>
    /// <param name="settings">Image input settings.</param>
    /// <returns></returns>
    public delegate IModelGraph ModelReadCallback(string name, Byte[] data, TensorImageSettings? settings);

    /// <summary>
    /// Represents the instance of a DNN model in memory, created from ONNX, Caffee, TFLite, etc<br/>    
    /// </summary>
    /// <remarks>
    /// Use <see cref="ModelReadCallback"/> to create instances of <see cref="IModelGraph"/>.
    /// </remarks>
    public interface IModelGraph : IDisposable
    {
        /// <summary>
        /// Represents the Base64(Sha256) hash of the model's file.
        /// </summary>
        string ModelSha256 { get; }

        /// <summary>
        /// Represents the image constrainsts the input image needs to meet<br/>
        /// to be compatible with this model.
        /// </summary>
        TensorImageSettings InputSettings { get; set; }

        /// <summary>
        /// Creates an execution engine capable of executing this model.
        /// </summary>
        /// <returns>The execution engine.</returns>
        IModelSession CreateSession();
    }

    /// <summary>
    /// Represents an execution provider able to execute an <see cref="IModelGraph"/> model.
    /// </summary>
    public interface IModelSession : IDisposable
    {
        IDenseTensor GetInputTensor(int idx);

        /// <summary>
        /// Gets an existing tensor.
        /// </summary>
        /// <typeparam name="T">The tensor's element type.</typeparam>
        /// <param name="idx">The tens</param>
        /// <returns></returns>
        IDenseTensor<T> GetInputTensor<T>(int idx) where T : unmanaged;        

        IDenseTensor<T> UseInputTensor<T>(int idx, params int[] dims) where T : unmanaged;        

        public void Inference();

        IReadOnlyList<string> OutputNames { get; }

        IDenseTensor<T> GetOutputTensor<T>(int idx) where T : unmanaged;

        IDenseTensor<T> UseOutputTensor<T>(int idx, params int[] dims) where T : unmanaged;
    }

    /// <summary>
    /// Represents an execution provider able to execute an <see cref="IModelGraph"/> model.
    /// </summary>
    /// <remarks>
    /// Optionally implemented by <see cref="IModelSession"/> objects.
    /// </remarks>    
    public interface IInferenceContext<TInput, TResult> : IDisposable
        where TResult:class
    {
        void Inference(TResult result, InferenceInput<TInput> input, Rectangle? inputWindow = null);
    }    

    public interface INarrowInference<TImage, TResult> : IInferenceContext<TImage,TResult>
        where TResult:class
    {
        RectangleF GetNextDetectionWindow(DetectedFrame t1, DetectedFrame? t0 = null);
    }  

    public interface ITensorImageProcessor<TTensorPixel>
        where TTensorPixel:unmanaged
    {
        void CopyImage(SpanBitmap src, SpanTensor3<TTensorPixel> dst);

        void CopyImage(SpanTensor3<TTensorPixel> src, SpanBitmap dst);
    }

}
