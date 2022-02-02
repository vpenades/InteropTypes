using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Drawing
{

    /// <summary>
    /// Exposes an API to transform points, vectors and scalars from virtual space to screen space and back.
    /// </summary>
    /// <remarks>
    /// Optionally implemented by <see cref="ICanvas2D"/> classes.
    /// </remarks>
    public interface ITransformer2D
    {
        /// <summary>
        /// Transforms the given points from virtual space to screen space.
        /// </summary>
        /// <param name="points"></param>
        void TransformForward(Span<Point2> points);

        /// <summary>
        /// Transforms the given vectors from virtual space to screen space.
        /// </summary>
        /// <param name="vectors"></param>
        void TransformNormalsForward(Span<Point2> vectors);

        /// <summary>
        /// Transforms the given scalars from virtual space to screen space.
        /// </summary>
        /// <param name="scalars"></param>
        void TransformScalarsForward(Span<float> scalars);

        /// <summary>
        /// Transforms the given points from screen space to virtual space.
        /// </summary>
        /// <param name="points"></param>
        void TransformInverse(Span<Point2> points);

        /// <summary>
        /// Transforms the given vectors from screen space to virtual space.
        /// </summary>
        /// <param name="vectors"></param>
        void TransformNormalsInverse(Span<Point2> vectors);

        /// <summary>
        /// Transforms the given scalars from virtual space to screen space.
        /// </summary>
        /// <param name="scalars"></param>
        void TransformScalarsInverse(Span<float> scalars);
    }
}
