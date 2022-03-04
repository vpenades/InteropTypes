using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Tensors
{
    public static class NativeTensorExtensions
    {
        /// <summary>
        /// Throws an exception if <paramref name="tensor"/> name doesn't meet <paramref name="nameConstraint"/>
        /// </summary>
        /// <typeparam name="T">An unmanaged type</typeparam>
        /// <param name="tensor">The tensor to verify.</param>
        /// <param name="nameConstraint">The name contraint to meet.</param>
        /// <returns><paramref name="tensor"/>.</returns>
        public static IDenseTensor<T> VerifyName<T>(this IDenseTensor<T> tensor, Predicate<String> nameConstraint)
            where T:unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (!nameConstraint(tensor.Name)) throw new ArgumentException($"Name mismatch, found '{tensor.Name}'.");

            return tensor;
        }

        public static IDenseTensor<T> VerifyDimensions<T>(this IDenseTensor<T> tensor, params int[] dims)
            where T : unmanaged
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));
            if (tensor.Dimensions.Length != dims.Length) throw new ArgumentException($"Dimensions mismatch, expected 4 but found {tensor.Dimensions.Length}.");

            for(int i=0; i < tensor.Dimensions.Length; ++i)
            {
                var d = dims[i]; if (d <= 0) continue;
                if (d != tensor.Dimensions[i]) throw new ArgumentException($"Dimension[{i}] mismatch, expected {d} but found {tensor.Dimensions[i]}.");
            }

            return tensor;
        }

        /// <summary>
        /// Copies the ROI of an image to the target tensor.
        /// </summary>
        /// <param name="dstTensor">The tensor target.</param>
        /// <param name="dstTensorSizeIdx">
        /// <para>Index in the tensor dimensions representing the bitmap size.</para>
        /// <para>In tensor semantics, the size is represented as Height, followed by Width.</para>
        /// </param>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageROI">The region of interest of the source image.</param>
        /// <param name="dstToSrcXform">A transform to transform from <paramref name="dstTensor"/> coordinates to <paramref name="srcImage"/> coordinates. </param>
        public static void SetPixels(this IInputImageTensor dstTensor, int dstTensorSizeIdx, PointerBitmap srcImage, Matrix3x2 srcImageROI, out Matrix3x2 dstToSrcXform)
        {
            if (dstTensor == null) throw new ArgumentNullException(nameof(dstTensor));            

            for(int i=0; i <= dstTensorSizeIdx+1; ++i)
            {
                if (dstTensor.Dimensions[i] <= 0) throw new ArgumentException($"Dimension[{i}] value is invalid.", nameof(dstTensor));
            }            

            Matrix3x2.Invert(srcImageROI, out Matrix3x2 iform);
            var ih = dstTensor.Dimensions[dstTensorSizeIdx + 0];
            var iw = dstTensor.Dimensions[dstTensorSizeIdx + 1];
            iform *= Matrix3x2.CreateScale(iw, ih);
            Matrix3x2.Invert(iform, out dstToSrcXform);

            dstTensor.SetPixels(srcImage, iform);
        }

        /// <summary>
        /// Copies the ROI of an image to the target tensor.
        /// </summary>
        /// <param name="dstTensor">The tensor target.</param>
        /// <param name="dstTensorSizeIdx">
        /// <para>Index in the tensor dimensions representing the bitmap size.</para>
        /// <para>In tensor semantics, the size is represented as Height, followed by Width.</para>
        /// </param>
        /// <param name="srcImage">The source image.</param>
        /// <param name="srcImageROI">The region of interest of the source image.</param>
        /// <param name="dstToSrcXform">A transform to transform from <paramref name="dstTensor"/> coordinates to <paramref name="srcImage"/> coordinates. </param>
        public static void SetPixels(this IDenseTensor dstTensor, int dstTensorSizeIdx, PointerBitmap srcImage, Matrix3x2 srcImageROI, out Matrix3x2 dstToSrcXform)
        {
            if (!(dstTensor is IInputImageTensor dstImage)) throw new ArgumentException($"Does not implement {nameof(IInputImageTensor)}.", nameof(dstTensor));
            
            dstImage.SetPixels(dstTensorSizeIdx, srcImage, srcImageROI, out dstToSrcXform);            
        }
    }
}
