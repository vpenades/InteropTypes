using System;

namespace InteropDrawing
{
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
    public interface IDrawable2D : IDrawingBrush<IDrawing2D> { }


    [Obsolete("Use IDrawingBrush<T>")]
    public interface IDrawable3D : IDrawingBrush<IDrawing3D> { }


    /// <summary>
    /// Represents a disposable <see cref="IDrawing2D"/>.
    /// </summary>
    public interface IDisposableDrawing2D : IDrawing2D, IDisposable { }

    /// <summary>
    /// Represents a disposable <see cref="IDrawing3D"/>.
    /// </summary>
    public interface IDisposableDrawing3D : IDrawing3D, IDisposable { }


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