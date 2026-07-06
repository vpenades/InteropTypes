using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics.Tensors;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.TensorBitmaps
{
    /// <summary>
    /// A bitmap backed by a <see cref="ReadOnlyTensorSpanBitmap{TElement}"/>
    /// </summary>
    /// <typeparam name="TElement">The type of the backing tensor</typeparam>
    /// <typeparam name="TPixel">The type of the bitmap's pixel</typeparam>
    public readonly ref struct ReadOnlyTensorSpanBitmap<TElement, TPixel>
        where TElement : unmanaged
        where TPixel : unmanaged
    {
        public static void CreatePlaneBitmaps(System.Numerics.Tensors.ReadOnlyTensorSpan<TElement> tensor, TensorPixelFormat format, out ReadOnlyTensorSpanBitmap<TElement, TElement> x, out ReadOnlyTensorSpanBitmap<TElement, TElement> y, out ReadOnlyTensorSpanBitmap<TElement, TElement> z)
        {
            if (tensor.Lengths[0] < 3) throw new IndexOutOfRangeException("the tensor has less than 3 planes");

            var planes = tensor.GetDimensionSpan(0);

            x = new ReadOnlyTensorSpanBitmap<TElement, TElement>(planes[0], new TensorPixelFormat(format.Components[0]));
            y = new ReadOnlyTensorSpanBitmap<TElement, TElement>(planes[1], new TensorPixelFormat(format.Components[1]));
            z = new ReadOnlyTensorSpanBitmap<TElement, TElement>(planes[2], new TensorPixelFormat(format.Components[2]));
        }

        public static implicit operator ReadOnlyTensorSpanBitmap<TElement, TPixel>(TensorBitmap<TElement, TPixel> bitmap)
        {
            var tensor = bitmap.Tensor.AsReadOnlyTensorSpan();
            return new ReadOnlyTensorSpanBitmap<TElement, TPixel>(tensor, bitmap._Info, bitmap.Format);
        }

        public static implicit operator ReadOnlyTensorSpanBitmap<TElement, TPixel>(TensorSpanBitmap<TElement, TPixel> bitmap)
        {
            var tensor = bitmap.Tensor.AsReadOnlyTensorSpan();
            return new ReadOnlyTensorSpanBitmap<TElement, TPixel>(tensor, bitmap._Info, bitmap.Format);
        }

        private ReadOnlyTensorSpanBitmap(System.Numerics.Tensors.ReadOnlyTensorSpan<TElement> tensor, _TensorBitmapInfo info, TensorPixelFormat format)
        {
            _Info = info;
            Format = format;
            Width = _Info.GetWidthFrom(tensor);
            Height = _Info.GetHeightFrom(tensor);
            Tensor = tensor;
        }

        public ReadOnlyTensorSpanBitmap(System.Numerics.Tensors.ReadOnlyTensorSpan<TElement> tensor, TensorPixelFormat format)
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            _Info = new _TensorBitmapInfo(tensor.Lengths);

            _Info._Validate<TElement, TPixel>(tensor, format);

            Format = format;
            Width = _Info.GetWidthFrom(tensor);
            Height = _Info.GetHeightFrom(tensor);
            Tensor = tensor;
            _Rows = _Info.GetRows(tensor);
        }        

        internal readonly _TensorBitmapInfo _Info;
        public TensorPixelFormat Format { get; }
        public int Width { get; }
        public int Height { get; }

        public System.Numerics.Tensors.ReadOnlyTensorSpan<TElement> Tensor { get; }

        private readonly ReadOnlyTensorDimensionSpan<TElement> _Rows;



        public ReadOnlySpan<TPixel> GetRowPixelsSpan(int y)
        {
            var trow = _Rows[y];

            var row = trow.GetSpan(_Info._RowIndices, (int)trow.FlattenedLength);

            var pixels = System.Runtime.InteropServices.MemoryMarshal.Cast<TElement, TPixel>(row);

            System.Diagnostics.Debug.Assert(pixels.Length == Width);

            return pixels;
        }

        /// <summary>
        /// Gets a new cropped bitmap that references the original surface without allocating new memory.
        /// </summary>
        public ReadOnlyTensorSpanBitmap<TElement, TPixel> GetCropped(System.Drawing.Rectangle rectangle)
        {
            rectangle.Intersect(new System.Drawing.Rectangle(0, 0, Width, Height));
            if (rectangle.IsEmpty) throw new ArgumentException("nothing to crop");

            var ranges = _Info.CalculateSlice(Tensor.Rank, rectangle);

            return new ReadOnlyTensorSpanBitmap<TElement, TPixel>(Tensor.Slice(ranges), Format);
        }
    }
}
