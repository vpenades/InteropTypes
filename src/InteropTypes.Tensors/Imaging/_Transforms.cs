using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

using XY = System.Numerics.Vector2;

namespace InteropTypes.Tensors.Imaging
{
    internal static class _Transforms
    {
        /// <summary>
        /// creates a fitting matrix from <paramref name="srcRect"/> to <paramref name="dstRect"/>
        /// </summary>
        /// <param name="srcRect">The source rectangle</param>
        /// <param name="dstRect">The target rectangle</param>
        /// <param name="aspectRatioScaleFactor">if set, it will adjust the matrix so the <paramref name="srcRect"/> will fit into <paramref name="dstRect"/> while keeping the aspect ratio.</param>
        /// <returns>The transfer matrix</returns>
        public static Matrix3x2 GetFitMatrix(System.Drawing.RectangleF srcRect, System.Drawing.RectangleF dstRect, float? aspectRatioScaleFactor = null)
        {
            var scaleX = dstRect.Width / srcRect.Width;
            var scaleY = dstRect.Height / srcRect.Height;

            Vector2 offset = Vector2.Zero;

            if (aspectRatioScaleFactor.HasValue)
            {
                var min = Math.Min(scaleX, scaleY);
                var max = Math.Max(scaleX, scaleY);

                var inv = 1f - aspectRatioScaleFactor.Value;

                scaleX = scaleY = min * inv + max * aspectRatioScaleFactor.Value;

                var pixSize = new Vector2(srcRect.Width, srcRect.Height) * scaleX;

                offset += new Vector2(dstRect.Width, dstRect.Height) - pixSize;
                offset /= 2;
            }

            var xform = Matrix3x2.CreateScale(scaleX, scaleY);

            offset -= Vector2.Transform(new Vector2(srcRect.Left, srcRect.Top), xform);
            offset += new Vector2(dstRect.Left, dstRect.Top);

            xform.Translation = offset;
            return xform;
        }











        [System.Diagnostics.DebuggerDisplay("{Coef0}, {Coef1}, {Constant0}, {Constant1}")]
        public struct LinearEquation
        {
            // https://github.com/dougieduh/Gaussian-Elimination/blob/main/Gaussian%20Elimination.cs

            /* code to map a list of points using least squares
            using MathNet.Numerics.LinearAlgebra;
            using MathNet.Numerics.LinearAlgebra.Double;

            public Matrix<double> ComputeTransformationMatrix(double[,] sourcePoints, double[,] destinationPoints)
            {
                // Create matrices from the points
                var sourceMatrix = DenseMatrix.OfArray(sourcePoints);
                var destinationMatrix = DenseMatrix.OfArray(destinationPoints);

                // Perform least squares calculation to find the transformation matrix
                var transformationMatrix = sourceMatrix.QR().Solve(destinationMatrix);

                return transformationMatrix;
            }*/

            public float Coef0;
            public float Coef1;
            public float Constant0;
            public float Constant1; // maybe this one is not needed

            public static LinearEquation[] PrepareEquations(IReadOnlyList<XY> sourcePoints, IReadOnlyList<XY> destinationPoints)
            {
                if (sourcePoints.Count != destinationPoints.Count)
                    throw new ArgumentException("The number of source and destination points must be the same.");

                var equations = new LinearEquation[sourcePoints.Count];

                for (int i = 0; i < equations.Length; i++)
                {
                    equations[i] = new LinearEquation
                    {
                        Coef0 = sourcePoints[i].X,
                        Coef1 = sourcePoints[i].Y,
                        Constant0 = destinationPoints[i].X,
                        Constant1 = destinationPoints[i].Y
                    };                    
                }

                return equations;
            }

            public static float[] Solve_GaussianElimination(Span<LinearEquation> equations)
            {
                int n = equations.Length;

                // Gaussian elimination
                for (int i = 0; i < n; i++)
                {
                    // Find the pivot
                    int max = i;
                    for (int j = i + 1; j < n; j++)
                    {
                        if (Math.Abs(equations[j].Coef0) > Math.Abs(equations[max].Coef0))
                        {
                            max = j;
                        }
                    }

                    // Swap rows
                    var temp = equations[max];
                    equations[max] = equations[i];
                    equations[i] = temp;

                    // Normalize pivot row
                    float pivot = equations[i].Coef0;
                    equations[i].Coef0 /= pivot;
                    equations[i].Coef1 /= pivot;
                    equations[i].Constant0 /= pivot;
                    equations[i].Constant1 /= pivot;

                    // Eliminate below
                    for (int j = i + 1; j < n; j++)
                    {
                        float factor = equations[j].Coef0 / equations[i].Coef0;
                        equations[j].Coef0 -= factor * equations[i].Coef0;
                        equations[j].Coef1 -= factor * equations[i].Coef1;
                        equations[j].Constant0 -= factor * equations[i].Constant0;
                        equations[j].Constant1 -= factor * equations[i].Constant1;
                    }
                }

                // Back substitution
                float[] solutions = new float[n];
                for (int i = n - 1; i >= 0; i--)
                {
                    solutions[i] = equations[i].Constant0;
                    for (int j = i + 1; j < n; j++)
                    {
                        solutions[i] -= equations[i].Coef1 * solutions[j];
                    }
                    solutions[i] /= equations[i].Coef0;
                }

                // The 'solutions' array now contains the solution to the system

                return solutions;
            }
        }
    }
}
