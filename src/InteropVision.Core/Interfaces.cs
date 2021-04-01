using System;
using System.Collections.Generic;
using System.Drawing;

using InteropTensors;

namespace InteropVision
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
    }

    /// <summary>
    /// Represents an execution provider able to execute an <see cref="IModelGraph"/> model.
    /// </summary>
    /// <remarks>
    /// Optionally implemented by <see cref="IModelSession"/> objects.
    /// </remarks>    
    public interface IImageInference<TImage, TResult> : IDisposable
        where TResult:class
    {
        void Inference(TResult result, InferenceInput<TImage> input, Rectangle? inputWindow = null);
    }


    [Obsolete("Use IImageInference<TImage, TResult>")]
    public interface IImageInference<T> : IDisposable
    {
        void Inference(T result, InferenceInput input, Rectangle? inputWindow = null);
    }

    public interface INarrowInference<T> : IImageInference<T>
    {
        RectangleF GetNextDetectionWindow(DetectedFrame t1, DetectedFrame? t0 = null);
    }



}
