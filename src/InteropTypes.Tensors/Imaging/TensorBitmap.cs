using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

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

        #region properties

        public ColorEncoding Encoding => _Encoding;

        public int NumChannels => _Channels;

        public int Width => _Width;

        public int Height => _Height;

        #endregion

        #region API

        public void CopyTo<TOther>(TensorBitmap<TOther> dst)
            where TOther : unmanaged
        {
            if (this._Width != dst._Width) throw new ArgumentException("width mismatch",nameof(dst));
            if (this._Height != dst._Height) throw new ArgumentException("height mismatch", nameof(dst));

            for(int y=0; y < dst._Height; ++y)
            {
                for (int x = 0; x < dst._Width; ++x)
                {
                    var value = this.GetPixel(x, y);
                    dst.SetPixel(x, y, value);
                }
            }
        }

        public Vector4  GetPixel(int x, int y)
        {
            switch(_Channels)
            {
                case 1: return _GetPixelX(x, y);
                case 3: return _GetPixelXYZ(x, y);
            }

            throw new NotImplementedException();
        }

        public void SetPixel(int x, int y, in Vector4 value)
        {
            switch(_Channels)
            {
                case 1: _SetPixelX(x, y, value); return;
            }
        }

        private unsafe Vector4 _GetPixelX(int x, int y)
        {
            if (sizeof(T) == 1)
            {
                var v = GetChannelX<Byte>()[y * _Width + x];
                switch (_Encoding)
                {
                    case ColorEncoding.A: return new Vector4(255f, 255f, 255f, v) / 255f;
                    default: return new Vector4(v, v, v, 255f) / 255f;
                }
            }

            if (sizeof(T) == 3)
            {                
                var xyz = GetChannelX<_PixelXYZ24>()[y * _Width + x];
                switch(_Encoding)
                {
                    case ColorEncoding.BGR: return new Vector4(xyz.Z, xyz.Y, xyz.X, 255f) / 255f;
                    default: return new Vector4(xyz.X, xyz.Y, xyz.Z, 255f) / 255f;
                }
            }

            if (sizeof(T) == 4)
            {
                var bytes = MMARSHAL.Cast<T,Byte>(_ChannelX.Slice(y*_Width+x,4));
                
                switch (_Encoding)
                {                    
                    case ColorEncoding.BGRA: return new Vector4(bytes[2], bytes[1], bytes[0], bytes[3]) / 255f;
                    case ColorEncoding.ARGB: return new Vector4(bytes[1], bytes[2], bytes[3], bytes[0]) / 255f;
                    default: return new Vector4(bytes[0], bytes[1], bytes[2], bytes[3]) / 255f;
                }
            }

            if (typeof(T) == typeof(Vector3))
            {
                var xyz = GetChannelX<Vector3>()[y * _Width + x];
                switch (_Encoding)
                {
                    case ColorEncoding.BGR: return new Vector4(xyz.Z, xyz.Y, xyz.X, 1);
                    default: return new Vector4(xyz, 1);
                }
            }

            if (typeof(T) == typeof(Vector4))
            {
                var xyzw = GetChannelX<Vector4>()[y * _Width + x];
                switch (_Encoding)
                {
                    case ColorEncoding.BGRA: return new Vector4(xyzw.Z, xyzw.Y, xyzw.X, xyzw.W);
                    case ColorEncoding.ARGB: return new Vector4(xyzw.W, xyzw.X, xyzw.Y, xyzw.Z);
                    default: return xyzw;
                }
            }

            throw new NotImplementedException();
        }

        private Vector4 _GetPixelXYZ(int x, int y)
        {
            int idx = y * _Width + x;
            if (typeof(T) == typeof(float))
            {
                var xx = GetChannelX<float>()[idx];
                var yy = GetChannelY<float>()[idx];
                var zz = GetChannelZ<float>()[idx];

                if (_Encoding == ColorEncoding.BGR)
                {
                    var t = xx;
                    xx = zz;
                    zz = t;
                }

                var rgba = new Vector4(xx, yy, zz, 1);

                rgba = Vector4.Min(Vector4.One, rgba);
                rgba = Vector4.Max(Vector4.Zero, rgba);
                return rgba;
            }

            throw new NotImplementedException();
        }

        private unsafe void _SetPixelX(int x, int y, in Vector4 value)
        {
            if (sizeof(T) == 1)
            {
                var src = value * 255;
                ref var dst = ref GetChannelX<Byte>()[y * _Width + x];
                switch (_Encoding)
                {
                    case ColorEncoding.A:
                        dst = (Byte)src.W;
                        break;
                    default:
                        dst = (Byte)((src.X + src.Y + src.Z) / 3f);
                        break;                    
                }
            }

            if (sizeof(T) == 3)
            {
                var src = value * 255;
                ref var dst = ref GetChannelX<_PixelXYZ24>()[y * _Width + x];
                switch (_Encoding)
                {
                    case ColorEncoding.BGR:
                        dst.X = (Byte)src.Z;
                        dst.Y = (Byte)src.Y;
                        dst.Z = (Byte)src.X;
                        break;
                    default:
                        dst.X = (Byte)src.X;
                        dst.Y = (Byte)src.Y;
                        dst.Z = (Byte)src.Z;
                        break;
                }
            }
        }

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
            return sizeof(T) < sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelX);
        }

        public unsafe Span<TT> GetChannelY<TT>()
            where TT : unmanaged
        {
            return sizeof(T) < sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelY);
        }

        public unsafe Span<TT> GetChannelZ<TT>()
            where TT : unmanaged
        {
            return sizeof(T) < sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelZ);
        }

        public unsafe Span<TT> GetChannelW<TT>()
            where TT : unmanaged
        {
            return sizeof(T) < sizeof(TT) ? throw new ArgumentException(typeof(T).Name) : MMARSHAL.Cast<T, TT>(_ChannelW);
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
