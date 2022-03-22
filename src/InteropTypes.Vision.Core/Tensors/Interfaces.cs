using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Tensors
{
    public interface IInputImageTensor : IDenseTensor
    {
        void FitPixels(PointerBitmap src);
        void SetPixels(PointerBitmap src, Matrix3x2 transform);
    }
}
