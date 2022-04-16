using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Tensors.Imaging;

using TRANSFORM = System.Numerics.Matrix3x2;
using MMARSHAL = System.Runtime.InteropServices.MemoryMarshal;

namespace InteropTypes.Tensors.Imaging
{
    /// <summary>
    /// Represents a bitmap created upon the data of a <see cref="SpanTensor2{T}"/> or <see cref="SpanTensor3{T}"/>
    /// </summary>
    public readonly ref struct TensorBitmap<T>
        where T: unmanaged
    {
        #region constructors
        public TensorBitmap(SpanTensor2<T> tensor, ColorEncoding encoding)
        {
            _ChannelX = tensor.Span;
            _ChannelY = default;
            _ChannelZ = default;
            _ChannelW = default;
            _Channels = 1;
            _Width = tensor.Dimensions[1];
            _Height = tensor.Dimensions[0];
            _Encoding = encoding;
        }

        public TensorBitmap(SpanTensor3<T> tensor, ColorEncoding encoding)
        {
            _ChannelX = tensor.Dimensions[0] > 0 ? tensor[0].Span : default;
            _ChannelY = tensor.Dimensions[0] > 1 ? tensor[1].Span : default;
            _ChannelZ = tensor.Dimensions[0] > 2 ? tensor[2].Span : default;
            _ChannelW = tensor.Dimensions[0] > 3 ? tensor[3].Span : default;
            _Channels = tensor.Dimensions[0];
            _Width = tensor.Dimensions[2];
            _Height = tensor.Dimensions[1];
            _Encoding = encoding;
        }

        #endregion

        #region data

        internal readonly Span<T> _ChannelX;
        internal readonly Span<T> _ChannelY;
        internal readonly Span<T> _ChannelZ;
        internal readonly Span<T> _ChannelW;

        internal readonly int _Channels;
        internal readonly int _Width;
        internal readonly int _Height;
        internal readonly ColorEncoding _Encoding;

        #endregion

        #region API

        public unsafe SpanTensor2<TT> GetTensorX<TT>()
            where TT : unmanaged
        {
            return new SpanTensor2<TT>(GetChannelX<TT>(), _Height, _Width);
        }

        public unsafe SpanTensor2<TT> GetTensorY<TT>()
            where TT : unmanaged
        {
            return new SpanTensor2<TT>(GetChannelY<TT>(), _Height, _Width);
        }

        public unsafe SpanTensor2<TT> GetTensorZ<TT>()
            where TT : unmanaged
        {
            return new SpanTensor2<TT>(GetChannelZ<TT>(), _Height, _Width);
        }

        public unsafe SpanTensor2<TT> GetTensorW<TT>()
            where TT : unmanaged
        {
            return new SpanTensor2<TT>(GetChannelW<TT>(), _Height, _Width);
        }

        public unsafe Span<TT> GetChannelX<TT>()
            where TT : unmanaged
        {
            return sizeof(T) != sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelX);
        }

        public unsafe Span<TT> GetChannelY<TT>()
            where TT : unmanaged
        {
            return sizeof(T) != sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelY);
        }

        public unsafe Span<TT> GetChannelZ<TT>()
            where TT : unmanaged
        {
            return sizeof(T) != sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelZ);
        }

        public unsafe Span<TT> GetChannelW<TT>()
            where TT : unmanaged
        {
            return sizeof(T) != sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelW);
        }

        public void FitPixels<TSrcPixel>(BitmapSampler<TSrcPixel> source, BitmapTransform xform)
        where TSrcPixel : unmanaged
        {
            var ww = (float)source.Width / (float)this._Width;
            var hh = (float)source.Height / (float)this._Height;
            xform.Transform *= TRANSFORM.CreateScale(ww, hh);

            xform.FillPixels(this, source);
        }

        public void FillPixels<TSrcPixel>(BitmapSampler<TSrcPixel> source, BitmapTransform xform)
            where TSrcPixel : unmanaged
        {
            xform.FillPixels(this, source);
        }


        #endregion
    }       
}
