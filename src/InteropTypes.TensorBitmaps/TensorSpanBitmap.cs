using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics.Tensors;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.TensorBitmaps
{
    /// <summary>
    /// A bitmap backed by a <see cref="TensorSpan{TElement}"/>
    /// </summary>
    /// <typeparam name="TElement">The type of the backing tensor</typeparam>
    /// <typeparam name="TPixel">The type of the bitmap's pixel</typeparam>
    [System.Diagnostics.DebuggerDisplay("TensorBitmap {Width}x{Height}")]
    public readonly ref struct TensorSpanBitmap<TElement, TPixel>
        where TElement : unmanaged
        where TPixel : unmanaged
    {
        public static void CreatePlaneBitmaps(System.Numerics.Tensors.TensorSpan<TElement> tensor, TensorPixelFormat format, out TensorSpanBitmap<TElement, TElement> x, out TensorSpanBitmap<TElement, TElement> y, out TensorSpanBitmap<TElement, TElement> z)
        {
            if (tensor.Lengths[0] < 3) throw new IndexOutOfRangeException("the tensor has less than 3 planes");

            var planes = tensor.GetDimensionSpan(0);

            x = new TensorSpanBitmap<TElement, TElement>(planes[0], new TensorPixelFormat(format.Components[0]));
            y = new TensorSpanBitmap<TElement, TElement>(planes[1], new TensorPixelFormat(format.Components[1]));
            z = new TensorSpanBitmap<TElement, TElement>(planes[2], new TensorPixelFormat(format.Components[2]));
        }

        public static implicit operator TensorSpanBitmap<TElement, TPixel>(TensorBitmap<TElement, TPixel> bitmap)
        {
            var tensor = bitmap.Tensor.AsTensorSpan();
            return new TensorSpanBitmap<TElement, TPixel>(tensor, bitmap._Info, bitmap.Format);
        }        

        private TensorSpanBitmap(System.Numerics.Tensors.TensorSpan<TElement> tensor, _TensorBitmapInfo info, TensorPixelFormat format)
        {
            _Info = info;
            Format = format;
            Width = _Info.GetWidthFrom(tensor);
            Height = _Info.GetHeightFrom(tensor);
            Tensor = tensor;
        }

        public TensorSpanBitmap(System.Numerics.Tensors.TensorSpan<TElement> tensor, TensorPixelFormat format)
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

        public System.Numerics.Tensors.TensorSpan<TElement> Tensor { get; }

        private readonly TensorDimensionSpan<TElement> _Rows;

        public Span<TPixel> GetRowPixelsSpan(int y)
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
        public TensorSpanBitmap<TElement, TPixel> GetCropped(System.Drawing.Rectangle rectangle)
        {
            rectangle.Intersect(new System.Drawing.Rectangle(0, 0, Width, Height));
            if (rectangle.IsEmpty) throw new ArgumentException("nothing to crop");

            var ranges = _Info.CalculateSlice(Tensor.Rank, rectangle);

            return new TensorSpanBitmap<TElement, TPixel>(Tensor.Slice(ranges), Format);
        }
    }
}
