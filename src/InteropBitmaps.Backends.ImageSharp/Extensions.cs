using System;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropBitmaps
{
    public static partial class ImageSharpToolkit
    {
        #region Imagesharp facade

        public static Adapters.ImageSharpFactory WithImageSharp(this BitmapInfo binfo) { return new Adapters.ImageSharpFactory(binfo); }

        public static Adapters.ImageSharpSpanAdapter WithImageSharp(this SpanBitmap bitmap) { return new Adapters.ImageSharpSpanAdapter(bitmap); }        

        public static Adapters.ImageSharpSpanAdapter<TPixel> WithImageSharp<TPixel>(this SpanBitmap<TPixel> bitmap)
            where TPixel : unmanaged, IPixel<TPixel>
        { return new Adapters.ImageSharpSpanAdapter<TPixel>(bitmap); }        

        public static Adapters.ImageSharpMemoryAdapter UsingImageSharp(this MemoryBitmap bmp) { return new Adapters.ImageSharpMemoryAdapter(bmp); }

        public static Adapters.ImageSharpMemoryAdapter<TPixel> UsingImageSharp<TPixel>(this MemoryBitmap<TPixel> bmp)
            where TPixel : unmanaged, IPixel<TPixel>
        { return new Adapters.ImageSharpMemoryAdapter<TPixel>(bmp); }

        #endregion

        #region As MemoryBitmap

        public static MemoryBitmap.ISource UsingMemoryBitmap<TPixel>(this Image<TPixel> src, bool owned = false)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return new Adapters.ImageSharpMemoryManager<TPixel>(src, owned);
        }
        
        public static MemoryBitmap<TPixel> ToMemoryBitmap<TPixel>(this Image<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return src.ReadAsSpanBitmap(self => self.ToMemoryBitmap());
        }

        public static MemoryBitmap ToMemoryBitmap(this Image src)            
        {
            return src.ReadAsSpanBitmap(self => self.ToMemoryBitmap());
        }

        #endregion

        #region extras

        public static Image<TPixel> TryWrapAsImageSharp<TPixel>(this MemoryBitmap<TPixel> src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return _Implementation.TryWrapImageSharp<TPixel>(src);
        }
        
        public static Image TryUsingAsImageSharp(this MemoryBitmap src)            
        {
            return _Implementation.TryWrapImageSharp(src);
        }
        
        public static Image<TPixel> TryUsingImageSharp<TPixel>(this PointerBitmap src)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (!src.Info.IsContinuous) return null;

            var m = src.UsingMemory<TPixel>();
            var s = src.Size;            

            return Image.WrapMemory(m, s.Width, s.Height); // We assume here that Image<Tpixel> will dispose IMemoryOwner<T> when disposed.
        }

        #endregion

        #region mutate

        public static TResult ReadAsSpanBitmap<TResult>(this Image self, SpanBitmap.Function1<TResult> function)
        {
            return self.ReadAsSpanBitmap(default, (self2, other) => function(self2));
        }

        /// <summary>
        /// Combines <paramref name="self"/> and <paramref name="other"/> to produce a <typeparamref name="TResult"/> using <paramref name="function"/>.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self">this image.</param>
        /// <param name="other">A read only bitmap.</param>
        /// <param name="function">A function that gets the <see cref="SpanBitmap"/> of <paramref name="self"/> and <paramref name="other"/> and returns a result.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static TResult ReadAsSpanBitmap<TResult>(this Image self, SpanBitmap other, SpanBitmap.Function2<TResult> function)
        {
            if (self is Image<L8> srcL8) { return srcL8.ReadAsSpanBitmap(other, function); }

            if (self is Image<Bgr565> srcBgr565) { return srcBgr565.ReadAsSpanBitmap(other, function); }
            if (self is Image<Bgra4444> srcBgra4444) { return srcBgra4444.ReadAsSpanBitmap(other, function); }
            if (self is Image<Bgra5551> srcBgra5551) { return srcBgra5551.ReadAsSpanBitmap(other, function); }

            if (self is Image<Rgb24> srcRgb24) { return srcRgb24.ReadAsSpanBitmap(other, function); }
            if (self is Image<Bgr24> srcBgr24) { return srcBgr24.ReadAsSpanBitmap(other, function); }

            if (self is Image<Rgba32> srcRgba32) { return srcRgba32.ReadAsSpanBitmap(other, function); }
            if (self is Image<Bgra32> srcBgra32) { return srcBgra32.ReadAsSpanBitmap(other, function); }
            if (self is Image<Argb32> srcArgb32) { return srcArgb32.ReadAsSpanBitmap(other, function); }

            if (self is Image<RgbaVector> srcRgbaVector) { srcRgbaVector.ReadAsSpanBitmap(other, function); }

            throw new NotImplementedException(self.GetType().Name);
        }

        public static TResult ReadAsSpanBitmap<TPixel, TResult>(this Image<TPixel> self, SpanBitmap<TPixel>.Function1<TResult> function)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            return self.ReadAsSpanBitmap(default, (self2,other) => function(self2));
        }

        public static TResult ReadAsSpanBitmap<TPixel,TResult>(this Image<TPixel> self, SpanBitmap<TPixel> other, SpanBitmap<TPixel>.Function2<TResult> function)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            if (!_Implementation.TryGetExactPixelFormat<TPixel>(out var otherFmt)) throw new NotImplementedException($"{typeof(TPixel)}");

            if (self.DangerousTryGetSinglePixelMemory(out Memory<TPixel> selfMem))
            {
                var selfBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TPixel, Byte>(selfMem.Span);
                var selfBmp = new SpanBitmap<TPixel>(selfBytes, self.Width, self.Height, otherFmt);

                return function(selfBmp.AsReadOnly(), other);
            }
            else
            {
                var tempBmp = new MemoryBitmap<TPixel>(self.Width, self.Height, otherFmt);

                _Implementation.Copy(self, tempBmp);
                return function(tempBmp.AsSpanBitmap().AsReadOnly(), other);
            }
        }

        public static unsafe void MutateAsSpanBitmap(this Image self, SpanBitmap other, SpanBitmap.Action2 action)
        {
            if (self is Image<L8> srcL8) { srcL8.MutateAsSpanBitmap(other, action); return; }            

            if (self is Image<Bgr565> srcBgr565) { srcBgr565.MutateAsSpanBitmap(other, action); return; }
            if (self is Image<Bgra4444> srcBgra4444) { srcBgra4444.MutateAsSpanBitmap(other, action); return; }
            if (self is Image<Bgra5551> srcBgra5551) { srcBgra5551.MutateAsSpanBitmap(other, action); return; }

            if (self is Image<Rgb24> srcRgb24) { srcRgb24.MutateAsSpanBitmap(other, action); return; }
            if (self is Image<Bgr24> srcBgr24) { srcBgr24.MutateAsSpanBitmap(other, action); return; }

            if (self is Image<Rgba32> srcRgba32) { srcRgba32.MutateAsSpanBitmap(other, action); return; }
            if (self is Image<Bgra32> srcBgra32) { srcBgra32.MutateAsSpanBitmap(other, action); return; }
            if (self is Image<Argb32> srcArgb32) { srcArgb32.MutateAsSpanBitmap(other, action); return; }

            if (self is Image<RgbaVector> srcRgbaVector) { srcRgbaVector.MutateAsSpanBitmap(other, action); return; }


            throw new NotImplementedException();
        }

        public static unsafe void MutateAsSpanBitmap<TSrcPixel>(this Image<TSrcPixel> self, SpanBitmap.Action1 action)
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>
        {
            self.MutateAsSpanBitmap(default, (self2, other) => action(self2));
        }

        public static unsafe void MutateAsSpanBitmap<TSrcPixel>(this Image<TSrcPixel> self, SpanBitmap other, SpanBitmap.Action2 action)
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            other = other.AsReadOnly();
            if (!_Implementation.TryGetExactPixelFormat<TSrcPixel>(out var otherFmt)) throw new NotImplementedException($"{typeof(TSrcPixel)}");

            if (self.DangerousTryGetSinglePixelMemory(out Memory<TSrcPixel> selfMem))
            {
                var selfBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(selfMem.Span);
                var selfBmp = new SpanBitmap(selfBytes, self.Width, self.Height, otherFmt);

                action(selfBmp, other);                
            }
            else
            {
                var tempBmp = new MemoryBitmap(self.Width, self.Height, otherFmt);

                _Implementation.Copy(self, tempBmp);
                action(tempBmp, other);
                _Implementation.Copy(tempBmp, self);                
            }            
        }

        public static unsafe void MutateAsSpanBitmap<TSrcPixel, TDstPixel>(this Image<TSrcPixel> self, SpanBitmap<TDstPixel> other, SpanBitmap<TDstPixel>.Action2 action)
            where TSrcPixel : unmanaged, IPixel<TSrcPixel>
            where TDstPixel : unmanaged
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            other = other.AsReadOnly();
            if (!_Implementation.TryGetExactPixelFormat<TSrcPixel>(out var otherFmt))
            {
                otherFmt = PixelFormat.TryIdentifyPixel<TDstPixel>();
            }            

            if (self.DangerousTryGetSinglePixelMemory(out Memory<TSrcPixel> selfMem))
            {
                var selfBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TSrcPixel, Byte>(selfMem.Span);
                var selfBmp = new SpanBitmap<TDstPixel>(selfBytes, self.Width, self.Height, otherFmt);

                action(selfBmp, other);
            }
            else
            {
                var tempBmp = new MemoryBitmap<TDstPixel>(self.Width, self.Height, otherFmt);

                _Implementation.Copy(self, tempBmp);
                action(tempBmp, other);
                _Implementation.Copy(tempBmp, self);
            }
        }        

        #endregion
    }
}
