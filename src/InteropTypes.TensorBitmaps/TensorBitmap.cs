using System;
using System.Buffers;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;

namespace InteropTypes.TensorBitmaps
{
    /// <summary>
    /// A bitmap backed by a <see cref="Tensor{TElement}"/>
    /// </summary>
    /// <typeparam name="TElement">The type of the backing tensor</typeparam>
    /// <typeparam name="TPixel">The type of the bitmap's pixel</typeparam>
    [System.Diagnostics.DebuggerDisplay("TensorBitmap {Width}x{Height}")]
    public class TensorBitmap<TElement,TPixel>
        where TElement: unmanaged
        where TPixel: unmanaged
    {
        public static TensorBitmap<TElement, TPixel> Create(int width, int height, TensorPixelFormat format)
        {
            var channels = _TensorBitmapInfo.GetChannelsFrom<TElement, TPixel>();            

            var buffer = new TElement[width * height * channels];

            var lengths = new nint[3];
            lengths[0] = height;
            lengths[1] = width;
            lengths[2] = channels;

            var tensor = System.Numerics.Tensors.Tensor.Create(buffer, lengths);

            return new TensorBitmap<TElement, TPixel>(tensor, format);
        }

        public TensorBitmap(System.Numerics.Tensors.Tensor<TElement> tensor, TensorPixelFormat format)
        {
            if (tensor == null) throw new ArgumentNullException(nameof(tensor));

            _Info = new _TensorBitmapInfo(tensor.Lengths);            

            _Info._Validate<TElement, TPixel>(tensor, format);
            
            Format = format;
            Width = _Info.GetWidthFrom(tensor);
            Height = _Info.GetHeightFrom(tensor);
            Tensor = tensor;
        }

        internal readonly _TensorBitmapInfo _Info;
        public TensorPixelFormat Format { get; }
        public int Width { get; }
        public int Height { get; }

        public System.Numerics.Tensors.Tensor<TElement> Tensor { get; }

        public Span<TPixel> GetRowPixelsSpan(int y)
        {
            var row = _Info.GetRow(Tensor,y);

            var pixels = System.Runtime.InteropServices.MemoryMarshal.Cast<TElement, TPixel>(row);

            System.Diagnostics.Debug.Assert(pixels.Length == Width);

            return pixels;            
        }


        /// <summary>
        /// Gets a new cropped bitmap that references the original surface without allocating new memory.
        /// </summary>
        public TensorBitmap<TElement, TPixel> GetCropped(System.Drawing.Rectangle rectangle)
        {
            rectangle.Intersect(new System.Drawing.Rectangle(0, 0, Width, Height));
            if (rectangle.IsEmpty) throw new ArgumentException("nothing to crop");

            var ranges = _Info.CalculateSlice(Tensor.Rank, rectangle);

            return new TensorBitmap<TElement, TPixel>(Tensor.Slice(ranges), Format);
        }
    }
}
