using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

// using InteropTypes.Graphics;
// using InteropTypes.Graphics.Bitmaps;

using SIZE = System.Drawing.Size;

namespace InteropTypes.Tensors
{
    public delegate void Kernel2Copy<Tin, Tout>(SpanTensor2<Tin> src, SpanTensor2<Tout> dst)
        where Tin: unmanaged
        where Tout: unmanaged;

    partial struct SpanTensor2<T>
    {
        public SIZE BitmapSize => new SIZE(this._Dimensions.Dim1, this._Dimensions.Dim0);

        public ReadOnlySpan<T> GetRowSpan(int y)
        {
            return _Buffer.Slice(y * this._Dimensions.Dim0, this._Dimensions.Dim0);
        }

        public Span<T> UseRowSpan(int y)
        {
            return _Buffer.Slice(y * this._Dimensions.Dim0, this._Dimensions.Dim0);
        }

        /*
        public SpanBitmap<T> AsSpanBitmap(PixelFormat fmt)
        {
            return new SpanBitmap<T>(this._Buffer, BitmapSize.Width, BitmapSize.Height, fmt);
        }

        public unsafe SpanBitmap<T> AsSpanBitmap()
        {
            var l = sizeof(T);
            
            if (typeof(Pixel.IReflection).IsAssignableFrom(typeof(T))) return AsSpanBitmap<T>();

            if (typeof(T) == typeof(Single)) return AsSpanBitmap(PixelFormat.CreateFromDepthAndChannels(typeof(float), 1));
            if (typeof(T) == typeof(Vector2)) return AsSpanBitmap(PixelFormat.CreateFromDepthAndChannels(typeof(float), 2));
            if (typeof(T) == typeof(Vector3)) return AsSpanBitmap(PixelFormat.CreateFromDepthAndChannels(typeof(float), 3));
            if (typeof(T) == typeof(Vector4)) return AsSpanBitmap(PixelFormat.CreateFromDepthAndChannels(typeof(float), 4));

            if (l == 1) return AsSpanBitmap(Pixel.Luminance8.Format);
            if (l == 3) return AsSpanBitmap(Pixel.BGR24.Format);

            throw new NotImplementedException();
        }

        public unsafe SpanBitmap<TPixel> AsSpanBitmap<TPixel>()
            where TPixel:unmanaged
        {
            if (sizeof(T) != sizeof(TPixel)) throw new ArgumentException(nameof(TPixel));

            var data = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TPixel>(this._Buffer);
            var pfmt = PixelFormat.TryIdentifyFormat<TPixel>();

            return new SpanBitmap<TPixel>(data, BitmapSize.Width, BitmapSize.Height, pfmt);
        }*/

        public void DrawLine(Vector2 a, Vector2 b, T value)
        {
            Imaging._Drawing.DrawPixelLine(this, a, b, value);
        }

        

        public void CopyTo(TensorIndices2 srcOffset, SpanTensor2<T> dst, TensorIndices2 dstOffset, TensorSize2 size)
        {
            size = TensorSize2.Min(size, TensorSize2.ExclusiveUnion(this.Dimensions, ref srcOffset, dst.Dimensions, ref dstOffset));

            for (int i0 = 0; i0 < size[0]; ++i0)
            {
                var srcSpan =this[srcOffset[0] + i0].Span.Slice(srcOffset[1]);
                var dstSpan = dst[dstOffset[0] + i0].Span.Slice(dstOffset[1]);

                var l1 = Math.Min(srcSpan.Length, dstSpan.Length);
                l1 = Math.Min(l1, size[1]);                

                srcSpan.Slice(0, l1).CopyTo(dstSpan);
            }
        }        

        public void CopyTo<Tout>(SpanTensor2<Tout> masterOut, TensorSize2 kernelSize, Kernel2Copy<T,Tout> kernel, TensorIndices2 margin = default)
            where Tout:unmanaged
        {
            var tmpSrc = new SpanTensor2<T>(kernelSize);
            var tmpDst = new SpanTensor2<Tout>(kernelSize);

            var stepSize = kernelSize - margin - margin;

            for (int i0=0; i0 < masterOut.Dimensions[0]; i0+= stepSize[0])
            {
                for(int i1=0; i1 < masterOut.Dimensions[1]; i1+= stepSize[1])
                {
                    var offset = new TensorIndices2(i0, i1);
                    this.CopyTo(offset, tmpSrc, default, kernelSize);
                    kernel(tmpSrc, tmpDst);

                    var extra = new TensorIndices2(i0 == 0 ? margin.Index0 : 0, i1 == 0 ? margin.Index1 : 0);

                    tmpDst.CopyTo(margin - extra, masterOut, offset + margin - extra, stepSize + extra);
                }
            }
        }
    }
}
