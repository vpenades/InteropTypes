using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Diagnostics;

using TRANSFORM = System.Numerics.Matrix3x2;
using MMARSHAL = System.Runtime.InteropServices.MemoryMarshal;


namespace InteropTypes.Tensors.Imaging
{

    /// <summary>
    /// Represents a bitmap created upon the data of a <see cref="SpanTensor2{T}"/> or <see cref="SpanTensor3{T}"/>
    /// </summary>
    /// <remarks>
    /// This structure can represent RGB values as pixels with interleaved channels, or as separated planes.
    /// </remarks>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public readonly ref struct TensorBitmap<T>
        where T: unmanaged, IConvertible
    {
        #region diagnostics
        private string GetDebuggerDisplay()
        {
            return $"{Width}x{Height}x{_ColorEncoding}";
        }

        #endregion

        #region constructors

        public static TensorBitmap<T> CreateInterleaved(int width, int height, ColorEncoding encoding)
        {
            return CreateInterleaved(width, height, encoding, ColorRanges.GetRangesFor<T>());
        }

        public static TensorBitmap<T> CreateInterleaved(int width, int height, ColorEncoding encoding, ColorRanges ranges)
        {
            _GuardTemplateParam();

            var t3 = new SpanTensor3<T>(height, width, encoding.GetChannelCount());
            return new TensorBitmap<T>(t3, encoding, ranges);            
        }

        public static TensorBitmap<T> CreatePlanes(int width, int height, ColorEncoding encoding)
        {
            return CreatePlanes(width, height, encoding, ColorRanges.GetRangesFor<T>());
        }

        public static TensorBitmap<T> CreatePlanes(int width, int height, ColorEncoding encoding, ColorRanges ranges)
        {
            _GuardTemplateParam();

            var t3 = new SpanTensor3<T>(encoding.GetChannelCount(), height, width);
            return new TensorBitmap<T>(t3, encoding, ranges);
        }

        public static unsafe TensorBitmap<T> CreateFrom<TOther>(SpanTensor2<TOther> srcTensor, ColorEncoding srcEncoding)
            where TOther : unmanaged
        {
            return CreateFrom(srcTensor, srcEncoding, ColorRanges.GetRangesFor<T>());
        }

        public static unsafe TensorBitmap<T> CreateFrom<TOther>(SpanTensor2<TOther> srcTensor, ColorEncoding srcEncoding, ColorRanges srcRanges)
            where TOther: unmanaged
        {
            _GuardTemplateParam();

            int srcElementCount = -1;

            if (typeof(T) == typeof(float))
            {
                if (typeof(TOther) == typeof(float)) srcElementCount = 1;
                if (typeof(TOther) == typeof(System.Numerics.Vector2)) srcElementCount = 2;
                if (typeof(TOther) == typeof(System.Numerics.Vector3)) srcElementCount = 3;
                if (typeof(TOther) == typeof(System.Numerics.Vector4)) srcElementCount = 4;
                if (typeof(TOther) == typeof(System.Numerics.Quaternion)) srcElementCount = 4;                
            }

            if (typeof(T) == typeof(Byte))
            {
                srcElementCount = sizeof(TOther);

                if (typeof(TOther) == typeof(float)) srcElementCount = -1;
            }

            if (srcEncoding == ColorEncoding.Undefined)
            {
                switch (srcElementCount)
                {
                    case 1: srcEncoding = ColorEncoding.A; break;
                    case 3: srcEncoding = ColorEncoding.RGB; break;
                    case 4: srcEncoding = ColorEncoding.RGBA; break;
                }
            }

            if (srcEncoding.GetChannelCount() != srcElementCount) throw new ArgumentException("encoding mismatch", nameof(srcEncoding));

            var downCast = srcTensor.DownCast<T>();
            return new Imaging.TensorBitmap<T>(downCast, srcEncoding, srcRanges);
        }

        public TensorBitmap(SpanTensor3<T> srcTensor, ColorEncoding encoding, ColorRanges ranges)
        {
            _GuardTemplateParam();

            if (srcTensor.Dimensions.Dim0 == 1) // reshape to interleaved
            {
                srcTensor = srcTensor.Reshaped(srcTensor.Dimensions.Dim1, srcTensor.Dimensions.Dim2, 1);
            }

            _ColorEncoding = encoding;
            _ColorRanges = ranges;

            _Interleaved = srcTensor;

            if (srcTensor.Dimensions.Dim0 < srcTensor.Dimensions.Dim2) // planes
            {
                _Width = srcTensor.Dimensions.Dim2;
                _Height = srcTensor.Dimensions.Dim1;

                _PlanesCount = srcTensor.Dimensions[0];
                _Planes = SpanPlanesBitmapRGBA<T>.CreateFrom(srcTensor, encoding);
            }

            else // interleaved
            {
                // notice that in here, we could support both float[3] and Vector3[1]

                _Width = _Interleaved.Dimensions.Dim1;
                _Height = _Interleaved.Dimensions.Dim0;

                _PlanesCount = 0;
                _Planes = default;
            }
        }

        private static void _GuardTemplateParam()
        {
            if (typeof(T) == typeof(Byte)) return;
            if (typeof(T) == typeof(float)) return;

            throw new InvalidOperationException("must by byte or float");
        }

        #endregion

        #region data

        internal readonly SpanTensor3<T> _Interleaved;
        private readonly SpanPlanesBitmapRGBA<T> _Planes;        

        /// <summary>
        /// stores the number of planes used.
        /// </summary>
        /// <remarks>
        /// If value is 1 and <see cref="T"/> is a composite type like <see cref="Vector3"/> or <see cref="_PixelXYZ24"/> then this is an interleaved value
        /// </remarks>

        internal readonly int _Width;
        internal readonly int _Height;

        internal readonly int _PlanesCount;
        internal readonly ColorEncoding _ColorEncoding;
        private readonly ColorRanges _ColorRanges;

        #endregion

        #region properties
        public ColorEncoding Encoding => _ColorEncoding;
        public int NumPlanes => _PlanesCount;
        public int Width => _Width;
        public int Height => _Height;

        public bool HasPlanes => _PlanesCount != 0;

        public static readonly bool ElementsAreBytes = typeof(T) == typeof(byte);

        public static readonly bool ElementsAreSingles = typeof(T) == typeof(float);
        public unsafe bool PixelIsInteger
        {
            get
            {               
                if (typeof(T) == typeof(byte)) return true;
                if (sizeof(T) == 3) return true;

                return !PixelIsFloating;
            }
        }

        public unsafe bool PixelIsFloating
        {
            get
            {
                if (typeof(T) == typeof(Half)) return false;
                if (typeof(T) == typeof(Single)) return false;
                if (typeof(T) == typeof(Double)) return false;
                if (typeof(T) == typeof(System.Numerics.Vector2)) return false;
                if (typeof(T) == typeof(System.Numerics.Vector3)) return false;
                if (typeof(T) == typeof(System.Numerics.Vector4)) return false;
                return false;
            }
        }

        public ScaledPixelsAccessor ScaledPixels => new ScaledPixelsAccessor(this);

        #endregion

        #region API

        public static unsafe bool ElementsAreCompatible<TOther>()
            where TOther: unmanaged
        {
            if (typeof(T) == typeof(Byte))
            {
                if (typeof(TOther) == typeof(float)) return false;
                return sizeof(TOther) <= 4;
            }

            if (typeof(T) == typeof(float))
            {
                if (typeof(TOther) == typeof(float)) return true;
                if (typeof(TOther) == typeof(System.Numerics.Vector2)) return true;
                if (typeof(TOther) == typeof(System.Numerics.Vector3)) return true;
                if (typeof(TOther) == typeof(System.Numerics.Vector4)) return true;
            }

            return false;
        }

        public unsafe bool TryGetTyped<TTyped>(out TensorBitmap<TTyped> result)
            where TTyped : unmanaged, IConvertible
        {
            if (typeof(T) != typeof(TTyped)) { result = default; return false; }
            result = new TensorBitmap<TTyped>(_Interleaved.Cast<TTyped>(), _ColorEncoding, _ColorRanges);
            return true;
        }

        public unsafe bool TryGetImageInterleaved<TOther>(out SpanTensor2<TOther> dst)
            where TOther : unmanaged
        {
            dst = default;
            if (_PlanesCount != 0) return false;

            if (sizeof(T) * _Interleaved.Dimensions.Last != sizeof(TOther)) return false;

            dst = _Interleaved.UpCast<TOther>();

            if (this.Height != dst.Dimensions[0]) return false;
            if (this.Width != dst.Dimensions[1]) return false;

            return true;
        }

        public unsafe bool TryGetImageInterleaved(out SpanTensor3<T> dst)
        {
            dst = default;
            if (_PlanesCount != 0) return false;

            dst = _Interleaved;

            if (this.Height != dst.Dimensions[0]) return false;
            if (this.Width != dst.Dimensions[1]) return false;

            return true;
        }

        public unsafe bool TryGetImagePlanes<TOther>(out SpanTensor2<TOther> redPlane, out SpanTensor2<TOther> greenPlane, out SpanTensor2<TOther> bluePlane)
            where TOther : unmanaged
        {
            if (_PlanesCount < 3 || sizeof(T) != sizeof(TOther))
            {
                redPlane = default;
                greenPlane = default;
                bluePlane = default;
                return false;
            }

            redPlane = _Planes.PlaneRed.Cast<TOther>();
            greenPlane = _Planes.PlaneGreen.Cast<TOther>();
            bluePlane = _Planes.PlaneBlue.Cast<TOther>();

            System.Diagnostics.Debug.Assert(redPlane.Dimensions == greenPlane.Dimensions);
            System.Diagnostics.Debug.Assert(redPlane.Dimensions == bluePlane.Dimensions);

            return true;
        }

        public void SetPixels<TOther>(TensorBitmap<TOther> src)
            where TOther:unmanaged, IConvertible
        {
            if (this._Interleaved.Dimensions != src._Interleaved.Dimensions) throw new ArgumentException("dims mismatch", nameof(src));

            if (this.HasPlanes && src.HasPlanes) // planes to planes
            {
                var rgbaRangeTransfer
                    = this._ColorRanges.ToMultiplyAdd().GetShuffled(this._ColorEncoding)
                    * src._ColorRanges.ToMultiplyAdd().GetShuffled(src._ColorEncoding).GetInverse();

                this._Planes.SetPixels(src._Planes, rgbaRangeTransfer);
                return;
            }

            if (!this.HasPlanes)
            {
                this.SetPixels<TOther>(src._Interleaved, src._ColorEncoding, src._ColorRanges);
            }
            
            // slow fallback

            this.ScaledPixels.SetPixels(src.ScaledPixels);
        }

        public void SetPixels<TOther>(ReadOnlySpanTensor3<TOther> srcBitmap, ColorEncoding srcPixEncoding, ColorRanges srcPixRanges)
            where TOther: unmanaged, IConvertible
        {
            if (typeof(TOther) != typeof(Byte) && typeof(TOther) != typeof(float)) throw new InvalidOperationException();

            if (HasPlanes)
            {
                throw new NotImplementedException();
            }


            var rgbaRangeTransfer = ColorRanges.GetConversionTransform(srcPixRanges, srcPixEncoding, _ColorRanges, _ColorEncoding);

            if (this._Interleaved.Dimensions != srcBitmap.Dimensions)
            {
                if (_ColorEncoding == srcPixEncoding)
                {
                    _Interleaved.FillFrom(srcBitmap, rgbaRangeTransfer);
                    return;
                }

                // TODO: do a row per row conversion
            }            

            throw new NotImplementedException();
        }        

        public void SetRowScaledPixels(int y, ReadOnlySpan<Vector4> scaledRowPixels)
        {
            if (_PlanesCount == 0)
            {
                scaledRowPixels.CopyScaledPixelTo(_Interleaved[y].Span,_ColorEncoding);
            }
            else
            {
                var tensor = new ReadOnlySpanTensor2<Vector4>(scaledRowPixels, scaledRowPixels.Length, 1).Cast<float>();

                _Planes.SetInterleavedRow(y, tensor, ColorEncoding.RGBA);
            }
        }

        public void SetRowPixels<TSrc>(int y, ReadOnlySpanTensor2<TSrc> srcRowPixels, ColorEncoding srcPixEncoding, ColorRanges srcPixRanges)
            where TSrc: unmanaged
        {
            var rgbaRangeTransfer = ColorRanges.GetConversionTransform(srcPixRanges, srcPixEncoding, _ColorRanges, _ColorEncoding);

            if (HasPlanes)
            {
                if (typeof(TSrc) == typeof(Byte))
                {
                    _Planes.SetInterleavedRow(y, srcRowPixels.Cast<Byte>(), srcPixEncoding);
                    return;
                }

                throw new NotImplementedException();                
            }
            
            if (_ColorEncoding == srcPixEncoding)
            {
                _Interleaved[y].FillFrom(srcRowPixels, rgbaRangeTransfer);
                return;
            }            
        }                

        public void ApplyMultiplyAdd(MultiplyAdd mad, bool shuffleMadRGBA = false)
        {
            if (HasPlanes)
            {
                _Planes.ApplyMultiplyAdd(mad);
                return;
            }

            if (shuffleMadRGBA) mad = mad.GetShuffled(_ColorEncoding);

            switch(_ColorEncoding)
            {
                case ColorEncoding.RGBA:
                case ColorEncoding.BGRA:
                case ColorEncoding.ARGB:
                    if (_Interleaved.TryUpCast<System.Numerics.Vector4>(out var t4))
                    {
                        mad.ApplyTransformTo(t4.Span);
                        return;
                    }
                    break;
                case ColorEncoding.RGB:
                case ColorEncoding.BGR:                
                    if (_Interleaved.TryUpCast<System.Numerics.Vector3>(out var t3))
                    {
                        mad.ApplyTransformTo(t3.Span);
                        return;
                    }
                    break;

                case ColorEncoding.A:
                case ColorEncoding.L:
                    if (_Interleaved.TryUpCast<float>(out var t1))
                    {
                        mad.ApplyTransformTo(t1.Span);
                        return;
                    }
                    break;
            }

            throw new NotImplementedException();
        }

        public ColorRanges EvaluateGetContentColorRanges()
        {
            if (_PlanesCount > 0) return _Planes.EvaluateGetContentColorRanges();

            var ranges = new ColorRanges.Serializable(float.MaxValue, float.MinValue);

            for(int y=0; y < _Interleaved.Dimensions.Dim0; ++y)
            {
                var row = _Interleaved[y];

                for (int x = 0; x < row.Dimensions.Dim0; ++x)
                {
                    var pixel = row[x].Span;
                    T r = default;
                    T g = default;
                    T b = default;
                    T a = default;

                    switch(_ColorEncoding)
                    {
                        case ColorEncoding.A: a = pixel[0]; break;
                        case ColorEncoding.L: a = pixel[0]; break;
                        case ColorEncoding.RGB: r = pixel[0]; g = pixel[1]; b = pixel[2]; break;
                        case ColorEncoding.BGR: r = pixel[2]; g = pixel[1]; b = pixel[0]; break;
                        case ColorEncoding.RGBA: r = pixel[0]; g = pixel[1]; b = pixel[2]; a = pixel[3]; break;
                        case ColorEncoding.BGRA: r = pixel[2]; g = pixel[1]; b = pixel[0]; a = pixel[3]; break;
                        case ColorEncoding.ARGB: r = pixel[3]; g = pixel[0]; b = pixel[1]; a = pixel[2]; break;
                        default: throw new NotImplementedException();
                    }

                    var rr = r.ToSingle(System.Globalization.CultureInfo.InvariantCulture);
                    var gg = g.ToSingle(System.Globalization.CultureInfo.InvariantCulture);
                    var bb = b.ToSingle(System.Globalization.CultureInfo.InvariantCulture);
                    var aa = a.ToSingle(System.Globalization.CultureInfo.InvariantCulture);

                    ranges.RedMin = Math.Min(ranges.RedMin, rr);
                    ranges.RedMax = Math.Max(ranges.RedMax, rr);

                    ranges.GreenMin = Math.Min(ranges.GreenMin, gg);
                    ranges.GreenMax = Math.Max(ranges.GreenMax, gg);

                    ranges.BlueMin = Math.Min(ranges.BlueMin, bb);
                    ranges.BlueMax = Math.Max(ranges.BlueMax, bb);

                    ranges.AlphaMin = Math.Min(ranges.AlphaMin, rr);
                    ranges.AlphaMax = Math.Max(ranges.AlphaMax, rr);
                }
            }

            

                
            return ranges;
        }

        #endregion

        #region sampler filling API

        public void FillPixels<TSrcElement>(TensorBitmap<TSrcElement> source, BitmapTransform xform)
            where TSrcElement : unmanaged, IConvertible
        {
            if (source.NumPlanes > 0) throw new ArgumentException("source as planes not supported", nameof(source));

            xform.ColorTransform = source._ColorRanges.ToMultiplyAdd().GetInverse() * xform.ColorTransform;

            if (typeof(TSrcElement) == typeof(Byte))
            {
                var sourceTensor = source._Interleaved.Cast<Byte>();

                switch (source.Encoding)
                {
                    case ColorEncoding.RGB:
                    case ColorEncoding.BGR:
                        var sampler3 = BitmapSampler<_PixelXYZ24>.From(source._Interleaved.UpCast<_PixelXYZ24>(), source.Encoding);
                        FillPixels(sampler3, xform);
                        return;

                    case ColorEncoding.RGBA:
                    case ColorEncoding.BGRA:
                    case ColorEncoding.ARGB:
                        var sampler4 = BitmapSampler<uint>.From(source._Interleaved.UpCast<uint>(), source.Encoding);
                        FillPixels(sampler4, xform);
                        return;

                    default: throw new NotImplementedException();
                }
            }

            if (typeof(TSrcElement) == typeof(float))
            {
                switch (source.Encoding)
                {
                    case ColorEncoding.RGB:
                    case ColorEncoding.BGR:                        
                        var sampler3 = BitmapSampler<Vector3>.From(source._Interleaved.UpCast<Vector3>(), source.Encoding);
                        FillPixels(sampler3, xform);
                        return;

                    case ColorEncoding.RGBA:
                    case ColorEncoding.BGRA:
                    case ColorEncoding.ARGB:                        
                        var sampler4 = BitmapSampler<Vector4>.From(source._Interleaved.UpCast<Vector4>(), source.Encoding);
                        FillPixels(sampler4, xform);
                        return;

                    default: throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
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
            // most probably, the fastest way to process

            xform.FillPixels(this, source);
        }        

        #endregion

        #region nested type        

        /// <summary>
        /// Represents an accessor that allows reading and writing individual pixels in a standarized Vector4-as-RGBA style.
        /// </summary>
        /// <remarks>
        /// Use <see cref="TensorBitmap{T}.ScaledPixels"/> for access.
        /// </remarks>
        public readonly ref struct ScaledPixelsAccessor
        {
            #region lifecycle
            internal ScaledPixelsAccessor(TensorBitmap<T> tensor)
            {
                _Tensor = tensor;
                _Forward = tensor._ColorRanges.ToMultiplyAdd();
                _Inverse = _Forward.GetInverse();
            }

            #endregion

            #region data

            private readonly TensorBitmap<T> _Tensor;
            private readonly MultiplyAdd _Forward;
            private readonly MultiplyAdd _Inverse;

            #endregion

            #region properties
            public int Width => _Tensor.Width;
            public int Height => _Tensor.Height;

            #endregion

            #region API

            public void SetPixels<TOther>(TensorBitmap<TOther>.ScaledPixelsAccessor srcPixels)
                where TOther : unmanaged, IConvertible
            {
                // the only optimization we can do here is to do a row transfer using an intermediate row buffer

                if (true) // use ROWS
                {
                    Span<Vector4> transfer = stackalloc Vector4[this.Width];

                    for (int y = 0; y < this.Height; ++y)
                    {
                        srcPixels.GetRowPixels(y, transfer);
                        this.SetRowPixels(y, transfer);
                    }

                }
                else // use individual pixels
                {
                    for (int y = 0; y < this.Height; ++y)
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            var value = srcPixels.GetPixelUnchecked(x, y);
                            SetPixelUnchecked(x, y, value);
                        }
                    }
                }
            }

            public Vector4 GetPixel(int x, int y)
            {
                x = Math.Clamp(x, 0, _Tensor.Width - 1);
                y = Math.Clamp(y, 0, _Tensor.Height - 1);
                return GetPixelUnchecked(x, y);
            }
            public void SetPixel(int x, int y, Vector4 value)
            {
                if (x < 0) return;
                if (x >= _Tensor.Width) return;

                if (y < 0) return;
                if (y >= _Tensor.Height) return;

                SetPixelUnchecked(x, y, value);
            }

            public void GetRowPixels(int y, Span<Vector4> dst)
            {
                if (_Tensor._PlanesCount == 0)
                {
                    var srcRow = _Tensor._Interleaved[y];

                    if (_Inverse.IsIdentity)
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            dst[x] = _Tensor._ColorEncoding.ToScaledPixel(srcRow[x].Span);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            var value = _Tensor._ColorEncoding.ToScaledPixel(srcRow[x].Span);
                            dst[x] = _Inverse.Transform(value);
                        }
                    }
                }
                else
                {
                    if (_Inverse.IsIdentity)
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            var value = _Tensor._Planes._GetScalarPixelUnchecked(x, y);                            
                        }
                    }
                    else
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            var value = _Tensor._Planes._GetScalarPixelUnchecked(x, y);
                            dst[x] = _Inverse.Transform(value);
                        }
                    }
                }
            }
            public void SetRowPixels(int y, ReadOnlySpan<Vector4> src)
            {
                if (_Tensor._PlanesCount == 0)
                {
                    var dstRow = _Tensor._Interleaved[y];

                    if (_Forward.IsIdentity)
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            src[x].CopyScaledPixelTo(dstRow[x].Span, _Tensor._ColorEncoding);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            var value = _Forward.Transform(src[x]);

                            value.CopyScaledPixelTo(dstRow[x].Span, _Tensor._ColorEncoding);
                        }
                    }
                }
                else
                {
                    if (_Forward.IsIdentity)
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            _Tensor._Planes._SetScalarPixelUnchecked(x, y, src[x]);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < this.Width; ++x)
                        {
                            var value = _Forward.Transform(src[x]);

                            _Tensor._Planes._SetScalarPixelUnchecked(x, y, value);
                        }
                    }
                }
            }

            public Vector4 GetPixelUnchecked(int x, int y)
            {
                var v4 = _Tensor._PlanesCount == 0
                    ? _Tensor._ColorEncoding.ToScaledPixel(_Tensor._Interleaved[y][x].Span)
                    : _Tensor._Planes._GetScalarPixelUnchecked(x, y);
                
                return _Inverse.Transform(v4);
            }
            public void SetPixelUnchecked(int x, int y, Vector4 value)
            {
                value = _Forward.Transform(value);

                if (_Tensor._PlanesCount == 0) value.CopyScaledPixelTo(_Tensor._Interleaved[y][x].Span, _Tensor._ColorEncoding);
                else _Tensor._Planes._SetScalarPixelUnchecked(x, y, value);
            }

            #endregion
        }

        /// <summary>
        /// Represents a context that is capable of Fill a <see cref="TensorBitmap{T}"/> using various transforms
        /// </summary>
        /// <remarks>
        /// Objects implementing this interface usually wrap a source image
        /// </remarks>
        public interface IFactory
        {
            public (int Width, int Height) OriginalSize { get; }

            /// <summary>
            /// If called before a <see cref="TryFitPixelsToTensorBitmap(TensorBitmap{T}, float?)"/> call, it will cache the resulting image for subsequent calls.
            /// </summary>        
            public void CacheNext();

            public bool TryTransferPixelsToTensorBitmap(TensorBitmap<T> dst)
            {
                return TryFitPixelsToTensorBitmap(dst, TRANSFORM.Identity);
            }

            public bool TryFitPixelsToTensorBitmap(TensorBitmap<T> dst, System.Drawing.RectangleF srcRect, float? aspectRatioscaleFactor = null)
            {
                var dstRect = new System.Drawing.RectangleF(0, 0, dst.Width, dst.Height);
                var xform = _Transforms.GetFitMatrix(srcRect, dstRect, aspectRatioscaleFactor);

                // TODO: if srcRect is outside OriginalSize, simply fill dst with black

                return TryFitPixelsToTensorBitmap(dst, xform);
            }

            public bool TryFitPixelsToTensorBitmap(TensorBitmap<T> dst, float? aspectRatioscaleFactor = null)
            {
                TRANSFORM xform = GetFitMatrix(dst.Width, dst.Height, aspectRatioscaleFactor);

                return TryFitPixelsToTensorBitmap(dst, xform);
            }

            public TRANSFORM GetFitMatrix(float dstWidth, float dstHeight, float? aspectRatioScaleFactor = null)
            {
                var srcRect = new System.Drawing.RectangleF(0, 0, OriginalSize.Width, OriginalSize.Height);
                var dstRect = new System.Drawing.RectangleF(0, 0, dstWidth, dstHeight);

                return _Transforms.GetFitMatrix(srcRect, dstRect, aspectRatioScaleFactor);
            }

            public bool TryFitPixelsToTensorBitmap(TensorBitmap<T> dst, TRANSFORM xform);
        }

        #endregion
    }

}
