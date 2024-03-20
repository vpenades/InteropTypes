using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;



namespace InteropTypes.Tensors.Imaging
{
    /// <summary>
    /// Represents a bitmap image defined as three R,G and B separated planes
    /// </summary>
    /// <typeparam name="T">The component type, typically <see cref="Byte"/> or <see cref="Single"/></typeparam>
    [System.Diagnostics.DebuggerDisplay("{_GetDebuggerDisplayString(),nq}")]
    public readonly ref struct SpanPlanesBitmapRGB<T>
        where T: unmanaged
    {
        #region diagnostics

        private string _GetDebuggerDisplayString()
        {
            var text = $"{Width}x{Height}x{typeof(T).Name} ";

            /*
            var (minR, maxR) = _ChannelR.GetMinMax();
            var (minG, maxG) = _ChannelG.GetMinMax();
            var (minB, maxB) = _ChannelB.GetMinMax();

            text += $" R:[{minR} < {maxR}]";
            text += $" G:[{minG} < {maxG}]";
            text += $" B:[{minB} < {maxB}]";
            */

            return text;
        }

        #endregion

        #region lifecycle

        public SpanPlanesBitmapRGB(int width, int height)
        {
            _Width = width;
            _Height = height;
            var len = width * width;

            _ChannelR = new T[len];
            _ChannelG = new T[len];
            _ChannelB = new T[len];

        }
        public SpanPlanesBitmapRGB(SpanTensor3<T> tensor, ColorEncoding encoding)
        {
            if (tensor.Dimensions[0] != 3) throw new ArgumentException("must have 3 planes", nameof(tensor));

            _Width = tensor.Dimensions[2];
            _Height = tensor.Dimensions[1];

            _Shuffle<T>(tensor.GetSubTensor(0).Span, tensor.GetSubTensor(1).Span, tensor.GetSubTensor(2).Span, encoding, out var r, out var g, out var b);

            _ChannelR = r;
            _ChannelG = g;
            _ChannelB = b;
        }

        #endregion

        #region data

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly int _Width;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly int _Height;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly Span<T> _ChannelR;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly Span<T> _ChannelG;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        internal readonly Span<T> _ChannelB;

        #endregion

        #region properties
        public int Width => _Width;
        public int Height => _Height;
        public SpanTensor2<T> ChannelR => new SpanTensor2<T>(_ChannelR, Height, Width);
        public SpanTensor2<T> ChannelG => new SpanTensor2<T>(_ChannelR, Height, Width);
        public SpanTensor2<T> ChannelB => new SpanTensor2<T>(_ChannelR, Height, Width);

        #endregion

        #region API

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetPixel(int x, int y, T r, T g, T b)
        {
            var idx = y * _Width + x;
            _ChannelR[idx] = r;
            _ChannelG[idx] = g;
            _ChannelB[idx] = b;
        }

        public (T r, T g, T b) GetPixel(int x, int y)
        {
            var idx = y * _Width + x;

            return (_ChannelR[idx], _ChannelG[idx], _ChannelB[idx]);
        }


        public void SetBitmap<TPixel>(ReadOnlySpanTensor2<TPixel> bitmap, ColorEncoding encoding)
            where TPixel:unmanaged
        {
            if (bitmap.Dimensions[0] != _Height) throw new ArgumentException("Height mismatch", nameof(bitmap));
            if (bitmap.Dimensions[1] != _Width) throw new ArgumentException("Width mismatch", nameof(bitmap));            

            for (int y = 0; y < _Height; ++y)
            {
                var srcRow = bitmap.GetSubTensor(y);
                SetPixelsRow<TPixel>(y, srcRow.Span, encoding);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetPixelsRow<TPixel>(int y, ReadOnlySpan<TPixel> row, ColorEncoding encoding)
            where TPixel: unmanaged
        {
            if (typeof(T) == typeof(float) && sizeof(TPixel) == 3)
            {
                var rowBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(row);
                Span<T> rowSingles = stackalloc T[rowBytes.Length];

                _ArrayUtilities.TryConvertSpan(rowBytes, rowSingles);

                SetRow(y, rowSingles, encoding);
                return;
            }

            if (typeof(T) == typeof(Byte) && typeof(TPixel) == typeof(Vector3) )
            {
                var rowBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, float>(row);
                Span<T> rowSingles = stackalloc T[rowBytes.Length];

                _ArrayUtilities.TryConvertSpan(rowBytes, rowSingles);

                SetRow(y, rowSingles, encoding);
                return;
            }


            switch (encoding)
            {
                case ColorEncoding.RGB:
                    if (sizeof(T) * 3 != sizeof(TPixel)) throw new InvalidOperationException("TPixel size mismatch");
                    break;
                case ColorEncoding.BGR:
                    if (sizeof(T) * 3 != sizeof(TPixel)) throw new InvalidOperationException("TPixel size mismatch");
                    break;
                default: throw new ArgumentException("invalid encoding", nameof(encoding));
            }

            if (row.Length < _Width) throw new ArgumentException("too small", nameof(row));
            row = row.Slice(0, _Width);
            var srcRGB = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel,T>(row);            

            SetRow(y, srcRGB, encoding);
        }

        public void SetBitmap(ReadOnlySpanTensor3<T> bitmap, ColorEncoding encoding)
        {
            if (bitmap.Dimensions[0] != _Height) throw new ArgumentException("Height mismatch", nameof(bitmap));
            if (bitmap.Dimensions[1] != _Width) throw new ArgumentException("Width mismatch", nameof(bitmap));
            if (bitmap.Dimensions[2] != 3) throw new ArgumentException("invalid pixel format", nameof(bitmap));

            for (int y = 0; y < _Height; ++y)
            {
                var srcRow = bitmap.GetSubTensor(y);
                this.SetRow(y, srcRow.Span, encoding);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetRow(int y, ReadOnlySpanTensor2<T> row, ColorEncoding encoding)
        {
            if (row.Dimensions[0] != _Width) throw new ArgumentException("Invalid width", nameof(row));
            if (row.Dimensions[1] != 3) throw new ArgumentException("Invalid pixel size", nameof(row));

            SetRow(y, row.Span, encoding);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SetRow(int y, ReadOnlySpan<T> row, ColorEncoding encoding)
        {
            if (row.Length < _Width * 3) throw new ArgumentException("too small", nameof(row));

            row = row.Slice(0, _Width);
            var srcRGB = row;

            var idx = y * _Width;

            _Shuffle<T>(_ChannelR, _ChannelG, _ChannelB, encoding, out var dstR, out var dstG, out var dstB);
            dstR = dstR.Slice(idx, _Width);
            dstG = dstG.Slice(idx, _Width);
            dstB = dstB.Slice(idx, _Width);

            ref var srcPtr = ref System.Runtime.InteropServices.MemoryMarshal.GetReference(srcRGB);

            for (int i = 0; i < dstR.Length; i++)
            {
                dstR[i] = srcPtr;
                srcPtr = ref Unsafe.Add(ref srcPtr, 1);
                dstG[i] = srcPtr;
                srcPtr = ref Unsafe.Add(ref srcPtr, 1);
                dstB[i] = srcPtr;
                srcPtr = ref Unsafe.Add(ref srcPtr, 1);
            }
        }
        

        public void ApplyMultiplyAdd(MultiplyAdd mad)
        {
            if (typeof(T) != typeof(float)) throw new InvalidOperationException("invalid type");

            var rrr = System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(_ChannelR);            
            mad.X.ApplyTransformTo(rrr);

            var ggg = System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(_ChannelG);            
            mad.Y.ApplyTransformTo(ggg);

            var bbb = System.Runtime.InteropServices.MemoryMarshal.Cast<T, float>(_ChannelB);            
            mad.Z.ApplyTransformTo(bbb);
        }        


        public unsafe void CopyTo<TPixel>(SpanTensor2<TPixel> dst, ColorEncoding dstEncoding)
            where TPixel: unmanaged
        {
            if (dst.BitmapSize.Width != _Width) throw new ArgumentException("Width mismatch", nameof(dst));
            if (dst.BitmapSize.Height != _Height) throw new ArgumentException("Height mismatch", nameof(dst));            

            if (typeof(T) == typeof(float))
            {
                _Shuffle<float>(_ChannelR, _ChannelG, _ChannelB, dstEncoding, out var srcRRR, out var srcGGG, out var srcBBB);

                if (typeof(TPixel) == typeof(Vector3))
                {
                    var dst3 = dst.DownCast<float>();

                    for (int y = 0; y < _Height; y++)
                    {
                        var dstRow = dst3[y].Span;

                        var step = y * _Height;

                        for (int x = 0; x < _Width; x++)
                        {
                            dstRow[x * 3 + 0] = srcRRR[x + step];
                            dstRow[x * 3 + 1] = srcGGG[x + step];
                            dstRow[x * 3 + 2] = srcBBB[x + step];
                        }
                    }

                    return;
                }

                if (sizeof(TPixel) == 3) // any type with a size of 3 bytes
                {
                    Span<float> tmpRow = stackalloc float[_Width * 3];

                    for (int y = 0; y < _Height; y++)
                    {
                        var step = y * _Height;

                        for (int x = 0; x < _Width; x++)
                        {
                            tmpRow[x * 3 + 0] = srcRRR[x + step];
                            tmpRow[x * 3 + 1] = srcGGG[x + step];
                            tmpRow[x * 3 + 2] = srcBBB[x + step];
                        }

                        var dstRow = dst.DownCast<Byte>()[y].Span;

                        _ArrayUtilities.TryConvertSpan<float, byte>(tmpRow, dstRow);
                    }

                    return;
                }                
            }

            if (typeof(T) == typeof(byte))
            {
                _Shuffle<byte>(_ChannelR, _ChannelG, _ChannelB, dstEncoding, out var srcRRR, out var srcGGG, out var srcBBB);

                if (sizeof(TPixel) == 3) // any type with a size of 3 bytes
                {
                    var dst3 = dst.DownCast<byte>();

                    for (int y = 0; y < _Height; y++)
                    {
                        var dstRow = dst3[y].Span;

                        var step = y * _Height;

                        for (int x = 0; x < _Width; x++)
                        {
                            dstRow[x * 3 + 0] = srcRRR[x + step];
                            dstRow[x * 3 + 1] = srcGGG[x + step];
                            dstRow[x * 3 + 2] = srcBBB[x + step];
                        }
                    }

                    return;
                }
            }

            throw new NotImplementedException();
        }

        public unsafe void CopyToMultiplexedRow(int y, Span<T> dstRow, ColorEncoding encoding)
        {
            if (dstRow.Length < _Width * 3) throw new ArgumentException("too small", nameof(dstRow));

            var srcR = _ChannelR.Slice(y * _Width, _Width);
            var srcG = _ChannelR.Slice(y * _Width, _Width);
            var srcB = _ChannelR.Slice(y * _Width, _Width);

            _Shuffle<T>(srcR, srcG, srcB, encoding, out var srcRR, out var srcGG, out var srcBB);

            for (int x=0; x < _Width; ++x)
            {
                dstRow[x * 3 + 0] = srcRR[x];
                dstRow[x * 3 + 1] = srcGG[x];
                dstRow[x * 3 + 2] = srcBB[x];
            }
        }

        private static unsafe void _Shuffle<TElement>(Span<T> srcR, Span<T> srcG, Span<T> srcB, ColorEncoding encoding, out Span<TElement> dstR, out Span<TElement> dstG, out Span<TElement> dstB)
            where TElement : unmanaged
        {
            if (sizeof(T) != sizeof(TElement)) throw new InvalidOperationException("TElement must be of the same size as T");

            switch (encoding)
            {
                case ColorEncoding.RGB:
                    dstR = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(srcR);
                    dstG = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(srcG);
                    dstB = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(srcB);
                    break;
                case ColorEncoding.BGR:
                    dstB = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(srcR);
                    dstG = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(srcG);
                    dstR = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TElement>(srcB);
                    break; // swap R and G channels
                default: throw new ArgumentException("invalid encoding", nameof(encoding));
            }

        }

        #endregion
    }
}
