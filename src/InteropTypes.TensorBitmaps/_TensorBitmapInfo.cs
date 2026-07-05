using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics.Tensors;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InteropTypes.TensorBitmaps
{
    internal readonly struct _TensorBitmapInfo
    {
        public _TensorBitmapInfo(ReadOnlySpan<nint> lengths)
        {
            _WidthIndex = -1;
            _HeightIndex = -1;
            _ChannelsIndex = -1;

            for (int i = 0; i < lengths.Length; i++)
            {
                var l = lengths[i];
                if (l == 0) throw new ArgumentException($"lengths[{i}] is zero", nameof(lengths));
                if (l == 1) continue;
                if (_HeightIndex < 0) { _HeightIndex = i; continue; }
                if (_WidthIndex < 0) { _WidthIndex = i; continue; }
                if (_ChannelsIndex < 0) { _ChannelsIndex = i; continue; }
                throw new ArgumentException($"only 2 or 3 dimensions allowed", nameof(lengths));
            }

            if (_HeightIndex < 0) throw new ArgumentException("Height not found", nameof(lengths));
            if (_WidthIndex < 0) throw new ArgumentException("Width not found", nameof(lengths));
            // channels is optional

            _RowIndices = new nint[lengths.Length - _WidthIndex];            
        }

        private readonly int _WidthIndex;
        private readonly int _HeightIndex;
        private readonly int _ChannelsIndex;

        internal readonly nint[] _RowIndices; // we could avoid this with a stackalloc        

        private void _ValidateFormat<TElement, TPixel>(nint tensorChannelCount, TensorPixelFormat format)
            where TElement : unmanaged
            where TPixel : unmanaged
        {
            if (format.Components.Count != tensorChannelCount)
            {
                throw new ArgumentException("Channels count mismatch", nameof(tensorChannelCount));
            }
        }

        public static int GetChannelsFrom<TElement, TPixel>()
            where TElement:unmanaged
            where TPixel:unmanaged
        {
            var (q, r) = Math.DivRem(Unsafe.SizeOf<TPixel>(), Unsafe.SizeOf<TElement>());
            if (q == 0 || r != 0) throw new InvalidOperationException($"{typeof(TElement).Name} and {typeof(TPixel).Name} mismatch");
            return q;            
        }

        #region Tensor

        public void _Validate<TElement, TPixel>(Tensor<TElement> tensor, TensorPixelFormat format)
            where TElement : unmanaged
            where TPixel : unmanaged
        {
            if (!tensor.GetDimensionSpan(_HeightIndex)[0].IsDense)
            {
                throw new ArgumentException("Rows must be dense", nameof(tensor));
            }

            _ValidateFormat<TElement, TPixel>(GetChannelsCountFrom(tensor), format);
        }

        public int GetWidthFrom<T>(Tensor<T> tensor) { return (int) tensor.Lengths[_WidthIndex]; }
        public int GetHeightFrom<T>(Tensor<T> tensor) { return (int)tensor.Lengths[_HeightIndex]; }
        public int GetChannelsCountFrom<T>(Tensor<T> tensor) { return _ChannelsIndex < 0 ? 1 : (int)tensor.Lengths[_ChannelsIndex]; }

        public Span<T> GetRow<T>(Tensor<T> tensor, int y)
        {
            var row = tensor.GetDimensionSpan(_HeightIndex)[y];
            return row.GetSpan(_RowIndices, (int)row.FlattenedLength);
        }

        #endregion

        #region TensorSpan

        public void _Validate<TElement, TPixel>(TensorSpan<TElement> tensor, TensorPixelFormat format)
           where TElement : unmanaged
           where TPixel : unmanaged
        {
            if (!tensor.GetDimensionSpan(_HeightIndex)[0].IsDense)
            {
                throw new ArgumentException("Rows must be dense", nameof(tensor));
            }

            _ValidateFormat<TElement, TPixel>(GetChannelsCountFrom(tensor), format);
        }

        public int GetWidthFrom<T>(TensorSpan<T> tensor) { return (int)tensor.Lengths[_WidthIndex]; }
        public int GetHeightFrom<T>(TensorSpan<T> tensor) { return (int)tensor.Lengths[_HeightIndex]; }
        public int GetChannelsCountFrom<T>(TensorSpan<T> tensor) { return _ChannelsIndex < 0 ? 1 : (int)tensor.Lengths[_ChannelsIndex]; }        

        public Span<T> GetRow<T>(TensorSpan<T> tensor, int y)
        {
            var row = GetRows(tensor)[y];
            return row.GetSpan(_RowIndices, (int)row.FlattenedLength);
        }

        public TensorDimensionSpan<T> GetRows<T>(TensorSpan<T> tensor)
        {
            return tensor.GetDimensionSpan(_HeightIndex);
        }

        #endregion

        #region ReadOnlyTensorSpan

        public void _Validate<TElement, TPixel>(ReadOnlyTensorSpan<TElement> tensor, TensorPixelFormat format)
           where TElement : unmanaged
           where TPixel : unmanaged
        {
            if (!tensor.GetDimensionSpan(_HeightIndex)[0].IsDense)
            {
                throw new ArgumentException("Rows must be dense", nameof(tensor));
            }

            _ValidateFormat<TElement, TPixel>(GetChannelsCountFrom(tensor), format);
        }

        public int GetWidthFrom<T>(ReadOnlyTensorSpan<T> tensor) { return (int)tensor.Lengths[_WidthIndex]; }
        public int GetHeightFrom<T>(ReadOnlyTensorSpan<T> tensor) { return (int)tensor.Lengths[_HeightIndex]; }
        public int GetChannelsCountFrom<T>(ReadOnlyTensorSpan<T> tensor) { return _ChannelsIndex < 0 ? 1 : (int)tensor.Lengths[_ChannelsIndex]; }
        
        public ReadOnlySpan<T> GetRow<T>(ReadOnlyTensorSpan<T> tensor, int y)
        {
            var row = GetRows(tensor)[y];
            return row.GetSpan(_RowIndices, (int)row.FlattenedLength);
        }

        public ReadOnlyTensorDimensionSpan<T> GetRows<T>(ReadOnlyTensorSpan<T> tensor)
        {
            return tensor.GetDimensionSpan(_HeightIndex);
        }

        #endregion

        #region slicing

        public NRange[] CalculateSlice(int rank, System.Drawing.Rectangle rect)
        {
            NRange[] ranges = new NRange[rank];
            for (int i = 0; i < ranges.Length; i++)
            {
                ranges[i] = NRange.All;
            }

            // Set the crop ranges for height and width dimensions
            ranges[_HeightIndex] = new NRange(
                new NIndex(rect.Y),
                new NIndex(rect.Y + rect.Height));

            ranges[_WidthIndex] = new NRange(
                new NIndex(rect.X),
                new NIndex(rect.X + rect.Width));

            return ranges;
        }

        #endregion
    }
}
