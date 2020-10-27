using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropBitmaps;

namespace InteropTensors
{
    partial struct SpanTensor2<T>
    {
        public int BitmapWidth => this._Dimensions.Dim1;
        public int BitmapHeight => this._Dimensions.Dim0;

        public Span<T> GetRowSpan(int y)
        {
            return _Buffer.Slice(y * this._Dimensions.Dim0, this._Dimensions.Dim0);
        }

        public SpanBitmap<T> AsSpanBitmap(Pixel.Format fmt)
        {
            return new SpanBitmap<T>(this._Buffer, BitmapWidth, BitmapHeight, fmt);
        }

        public unsafe SpanBitmap<T> AsSpanBitmap()
        {
            var l = sizeof(T);

            if (l == 1) return AsSpanBitmap(Pixel.Standard.Gray8);
            if (l == 3) return AsSpanBitmap(Pixel.Standard.BGR24);

            if (typeof(T) == typeof(Single)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 1));
            if (typeof(T) == typeof(Vector2)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 2));
            if (typeof(T) == typeof(Vector3)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 3));
            if (typeof(T) == typeof(Vector4)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 4));

            throw new NotImplementedException();
        }

        
    }
}
