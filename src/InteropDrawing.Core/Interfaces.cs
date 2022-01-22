using System;

namespace InteropDrawing
{
    /// <summary>
    /// provides additional information about the rendering backend
    /// </summary>
    /// <remarks>
    /// This interface must be implemented by the final rendering backend, and
    /// queried through any exposed <see cref="IDrawable2D"/> casted to a
    /// <see cref="IServiceProvider"/>
    /// </remarks>
    public interface IBackendViewportInfo
    {
        /// <summary>
        /// The viewport width i pixels
        /// </summary>
        int PixelsWidth { get; }

        /// <summary>
        /// The viewport height in pixels
        /// </summary>
        int PixelsHeight { get; }
    }


    /// <summary>
    /// Represents an object that can be drawn to a <typeparamref name="TContext"/>
    /// </summary>
    public interface IDrawingBrush<TContext>
    {
        /// <summary>
        /// Draws this object into the <typeparamref name="TContext"/> context.
        /// </summary>
        /// <param name="context">The drawing context.</param>
        void DrawTo(TContext context);
    }

    [Obsolete("Use IDrawingBrush<T>")]
    public interface IDrawable2D : IDrawingBrush<IDrawable2D> { void DrawTo(IDrawing2D dc); }


    [Obsolete("Use IDrawingBrush<T>")]
    public interface IDrawable3D : IDrawingBrush<IDrawable3D> { void DrawTo(IDrawing3D dc); }


    /// <summary>
    /// defines an objects that exposes a unique key that doesn't change as long as
    /// the object itself doesn't change, and can be used by other objects to determine
    /// if this object has changed.    
    /// </summary>
    /// <example>
    /// <code>
    /// var model1 = new Model3D();
    /// var gpuDict = new Dictionary&gt;Object,GPUModel&lt;();
    /// gpuDict[model1.ImmutableKey] = new GPUModel(model1);
    /// </code>
    /// </example>
    public interface IPseudoImmutable
    {
        Object ImmutableKey { get; }

        // void InvalidateImmutableKey();
    }
    
    

    
}