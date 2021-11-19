using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using InteropBitmaps;

using SIZE = System.Drawing.Size;

namespace InteropTensors
{
    partial struct SpanTensor2<T>
    {

        public SIZE BitmapSize => new SIZE(this._Dimensions.Dim1, this._Dimensions.Dim0);

        public Span<T> GetRowSpan(int y)
        {
            return _Buffer.Slice(y * this._Dimensions.Dim0, this._Dimensions.Dim0);
        }

        public SpanBitmap<T> AsSpanBitmap(Pixel.Format fmt)
        {
            return new SpanBitmap<T>(this._Buffer, BitmapSize.Width, BitmapSize.Height, fmt);
        }

        public unsafe SpanBitmap<T> AsSpanBitmap()
        {
            var l = sizeof(T);

            if (l == 1) return AsSpanBitmap(Pixel.Luminance8.Format);
            if (l == 3) return AsSpanBitmap(Pixel.BGR24.Format);

            if (typeof(T) == typeof(Single)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 1));
            if (typeof(T) == typeof(Vector2)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 2));
            if (typeof(T) == typeof(Vector3)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 3));
            if (typeof(T) == typeof(Vector4)) return AsSpanBitmap(Pixel.Format.GetFromDepthAndChannels(typeof(float), 4));

            throw new NotImplementedException();
        }

        public unsafe SpanBitmap<TPixel> AsSpanBitmap<TPixel>()
            where TPixel:unmanaged
        {
            if (sizeof(T) != sizeof(TPixel)) throw new ArgumentException(nameof(TPixel));

            var data = System.Runtime.InteropServices.MemoryMarshal.Cast<T, TPixel>(this._Buffer);
            var pfmt = Pixel.Format.TryIdentifyPixel<TPixel>();

            return new SpanBitmap<TPixel>(data, BitmapSize.Width, BitmapSize.Height, pfmt);
        }
    }
}
