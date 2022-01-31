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