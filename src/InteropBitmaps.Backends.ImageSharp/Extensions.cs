using System;

using InteropTypes.Graphics.Bitmaps;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace InteropTypes.Graphics.Backends
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

        #region ImageSharp.ReadAsSpanBitmap( x => x )

        public static TResult ReadAsSpanBitmap<TResult>(this Image self, SpanBitmap.Function1<TResult> function)
        {
            return self.ReadAsSpanBitmap(default, (self2, other) => function(self2));
        }
        
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

        #endregion

        #region ImageSharp.WriteAsSpanBitmap( x => x );

        public static void WriteAsSpanBitmap(this Image self, SpanBitmap.Action1 action)
        {
            self.WriteAsSpanBitmap(default, (s, o) => action(s));
        }

        public static void WriteAsSpanBitmap(this Image self, SpanBitmap other, SpanBitmap.Action2 action)
        {
            if (self is Image<A8> srcA8) { srcA8.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.Alpha8>(), (s, o) => action(s, o)); return; }
            if (self is Image<L8> srcL8) { srcL8.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.Luminance8>(), (s,o) => action(s,o)); return; }

            if (self is Image<Bgr565> srcBgr565) { srcBgr565.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.BGR565>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgra4444> srcBgra4444) { srcBgra4444.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.BGRA4444>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgra5551> srcBgra5551) { srcBgra5551.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.BGRA5551>(), (s, o) => action(s, o)); return; }

            if (self is Image<Rgb24> srcRgb24) { srcRgb24.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.BGR24>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgr24> srcBgr24) { srcBgr24.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.RGB24>(), (s, o) => action(s, o)); return; }

            if (self is Image<Rgba32> srcRgba32) { srcRgba32.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.RGBA32>(), (s, o) => action(s, o)); return; }
            if (self is Image<Bgra32> srcBgra32) { srcBgra32.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.BGRA32>(), (s, o) => action(s, o)); return; }
            if (self is Image<Argb32> srcArgb32) { srcArgb32.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.ARGB32>(), (s, o) => action(s, o)); return; }

            if (self is Image<RgbaVector> srcRgbaVector) { srcRgbaVector.WriteAsSpanBitmap(other.OfTypeOrDefault<Pixel.RGBA128F>(), (s, o) => action(s, o)); return; }

            throw new NotImplementedException();
        }

        public static void WriteAsSpanBitmap<TPixel>(this Image<TPixel> self, SpanBitmap<TPixel>.Action1 action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            self.WriteAsSpanBitmap(default, (self2, other) => action(self2));
        }

        public static void WriteAsSpanBitmap<TPixel>(this Image<TPixel> self, SpanBitmap<TPixel> other, SpanBitmap<TPixel>.Action2 action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            self.WriteAsSpanBitmap<TPixel,TPixel>(other, (s,o) => action(s,o));
        }

        public static unsafe void WriteAsSpanBitmap<TSelfPixel, TOtherPixel>(this Image<TSelfPixel> self, SpanBitmap<TOtherPixel> other, SpanBitmap<TOtherPixel>.Action2 action)
            where TSelfPixel : unmanaged, IPixel<TSelfPixel>
            where TOtherPixel : unmanaged
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            if (sizeof(TSelfPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            other = other.AsReadOnly();

            if (!_Implementation.TryGetExactPixelFormat<TSelfPixel>(out var otherFmt))
            {
                otherFmt = PixelFormat.TryIdentifyPixel<TOtherPixel>();
            }            

            if (self.DangerousTryGetSinglePixelMemory(out Memory<TSelfPixel> selfMem))
            {
                var selfBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TSelfPixel, Byte>(selfMem.Span);
                var selfBmp = new SpanBitmap<TOtherPixel>(selfBytes, self.Width, self.Height, otherFmt);
                action(selfBmp, other);
            }
            else
            {
                var tempBmp = new MemoryBitmap<TOtherPixel>(self.Width, self.Height, otherFmt);

                _Implementation.Copy(self, tempBmp);
                action(tempBmp, other);
                _Implementation.Copy(tempBmp, self);
            }
        }

        public static void WriteAsSpanBitmap<TPixel>(this SpanBitmap<TPixel> self, Image<TPixel> other, SpanBitmap<TPixel>.Action2 action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            self.WriteAsSpanBitmap<TPixel, TPixel>(other, action);
        }

        public static unsafe void WriteAsSpanBitmap<TSelfPixel, TOtherPixel>(this SpanBitmap<TSelfPixel> self, Image<TOtherPixel> other, SpanBitmap<TSelfPixel>.Action2 action)
            where TSelfPixel : unmanaged
            where TOtherPixel : unmanaged, IPixel<TOtherPixel>
            
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            if (sizeof(TSelfPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            if (!_Implementation.TryGetExactPixelFormat<TOtherPixel>(out var otherFmt))
            {
                otherFmt = PixelFormat.TryIdentifyPixel<TSelfPixel>();
            }

            if (other.DangerousTryGetSinglePixelMemory(out Memory<TOtherPixel> otherMem))
            {
                var otherBytes = System.Runtime.InteropServices.MemoryMarshal.Cast<TOtherPixel, Byte>(otherMem.Span);
                var otherBmp = new SpanBitmap<TSelfPixel>(otherBytes, other.Width, other.Height, otherFmt);
                otherBmp.AsReadOnly();
                action(self, otherBmp);
            }
            else
            {
                var tempBmp = new MemoryBitmap<TSelfPixel>(other.Width, other.Height, otherFmt);                
                _Implementation.Copy(tempBmp, other);

                action(self, tempBmp.AsSpanBitmap().AsReadOnly());                
            }
        }

        public static unsafe void WriteAsSpanBitmap<TPixel>(this SpanBitmap<TPixel> self, Action<Image<TPixel>> action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            self.WriteAsSpanBitmap<TPixel, TPixel>(action);
        }        

        public static unsafe void WriteAsSpanBitmap<TSelfPixel, TOtherPixel>(this SpanBitmap<TSelfPixel> self, Action<Image<TOtherPixel>> action)
            where TSelfPixel : unmanaged
            where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            if (sizeof(TSelfPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            if (self.Info.IsContinuous)
            {
                self.PinWritablePointer(ptr => ptr.WriteAsImageSharp(action));
            }
            else
            {
                using (var selfImg = new Image<TOtherPixel>(self.Width, self.Height))
                {
                    _Implementation.Copy(self, selfImg);
                    action(selfImg);
                    _Implementation.Copy(selfImg, self);
                }
            }
        }
        
        public static void WriteAsSpanBitmap(this MemoryBitmap self, Action<Image> action)            
        {
            switch(self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: self.OfType<Pixel.Luminance8>().WriteAsImageSharp<Pixel.Luminance8, L8>(Image => action(Image)); break;
                case Pixel.BGR24.Code: self.OfType<Pixel.BGR24>().WriteAsImageSharp<Pixel.BGR24, Bgr24>(Image => action(Image)); break;
                case Pixel.RGBA32.Code: self.OfType<Pixel.RGBA32>().WriteAsImageSharp<Pixel.RGBA32, Rgba32>(Image => action(Image)); break;
                case Pixel.BGRA32.Code: self.OfType<Pixel.BGRA32>().WriteAsImageSharp<Pixel.BGRA32, Bgra32>(Image => action(Image)); break;
                case Pixel.ARGB32.Code: self.OfType<Pixel.ARGB32>().WriteAsImageSharp<Pixel.ARGB32, Argb32>(Image => action(Image)); break;
                default: throw new NotImplementedException();
            }            
        }

        public static void WriteAsSpanBitmap<TPixel>(this MemoryBitmap<TPixel> self, Action<Image<TPixel>> action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            self.WriteAsImageSharp<TPixel, TPixel>(action);
        }

        #endregion

        #region *Bitmap.WriteAsImageSharp( x => x );

        public static unsafe void WriteAsImageSharp<TSelfPixel, TOtherPixel>(this SpanBitmap<TSelfPixel> self, Action<Image<TOtherPixel>> action)
           where TSelfPixel : unmanaged
           where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            if (sizeof(TSelfPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            if (self.Info.IsContinuous)
            {
                self.PinWritablePointer(ptr => ptr.WriteAsImageSharp(action));
                return;
            }
            else
            {
                using (var selfImg = new Image<TOtherPixel>(self.Width, self.Height))
                {
                    _Implementation.Copy(self, selfImg);
                    _ImageSharpChangedMonitor.Run(selfImg, action);
                    _Implementation.Copy(selfImg, self);
                }
            }
        }

        public static unsafe void WriteAsImageSharp<TSelfPixel, TOtherPixel>(this MemoryBitmap<TSelfPixel> self, Action<Image<TOtherPixel>> action)
            where TSelfPixel : unmanaged
            where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            if (sizeof(TSelfPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);

            if (self.Info.IsContinuous)
            {
                var selfMem = self.GetPixelMemory<TOtherPixel>();
                using (var selfImg = Image.WrapMemory(selfMem, self.Width, self.Height))
                {
                    _ImageSharpChangedMonitor.Run(selfImg, action);
                }
                    
            }
            else
            {
                using (var selfImg = new Image<TOtherPixel>(self.Width, self.Height))
                {
                    _Implementation.Copy(self, selfImg);
                    _ImageSharpChangedMonitor.Run(selfImg, action);
                    _Implementation.Copy(selfImg, self);
                }
            }
        }

        public static void WriteAsImageSharp(this PointerBitmap self, Action<Image> action)
        {
            void _run<TPixel>(PointerBitmap bmp) where TPixel : unmanaged, IPixel<TPixel>
            {
                bmp.WriteAsImageSharp<TPixel>(Image => action(Image));
            }

            switch (self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: _run<L8>(self); break;
                case Pixel.BGR24.Code: _run<Bgr24>(self); break;
                case Pixel.RGBA32.Code: _run<Rgba32>(self); break;
                case Pixel.BGRA32.Code: _run<Bgra32>(self); break;
                case Pixel.ARGB32.Code: _run<Argb32>(self); break;
                default: throw new NotImplementedException();
            }
        }

        public static unsafe void WriteAsImageSharp<TPixel>(this PointerBitmap self, Action<Image<TPixel>> action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            if (sizeof(TPixel) != self.Info.PixelByteSize) throw new ArgumentException("pixel size mismatch", typeof(TPixel).Name);

            if (self.Info.IsContinuous)
            {
                var m = self.UsingMemory<TPixel>();
                var s = self.Size;

                using (var selfImg = Image.WrapMemory(m, s.Width, s.Height))
                {
                    _ImageSharpChangedMonitor.Run(selfImg, action);
                }
            }
            else
            {
                using (var selfImg = new Image<TPixel>(self.Width, self.Height))
                {
                    _Implementation.Copy(self, selfImg);
                    _ImageSharpChangedMonitor.Run(selfImg, action);
                    _Implementation.Copy(selfImg, self);
                }
            }
        }

        #endregion

        #region *Bitmap.MutateAsImageSharp( x => x );

        

        public static void MutateAsImageSharp<TPixel>(this SpanBitmap<TPixel> self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation)
            where TPixel:unmanaged
        {
            // [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            void _run<TPixelOut>(SpanBitmap<TPixel> bmp)                
                where TPixelOut : unmanaged, IPixel<TPixelOut>
            {
                bmp.WriteAsImageSharp<TPixel, TPixelOut>(img => SixLabors.ImageSharp.Processing.ProcessingExtensions.Mutate(img, operation));
            }

            if (typeof(TPixel) == typeof(Pixel.Luminance8)) { _run<L8>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGR565)) { _run<Bgr565>(self); }
            if (typeof(TPixel) == typeof(Pixel.BGRA4444)) { _run<Bgra4444>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGRA5551)) { _run<Bgra5551>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGR24)) { _run<Bgr24>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.RGBA32)) { _run<Rgba32>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.BGRA32)) { _run<Bgra32>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.ARGB32)) { _run<Argb32>(self); return; }
            if (typeof(TPixel) == typeof(Pixel.RGBA128F)) { _run<RgbaVector>(self); return; }            

            // fallback

            self.AsTypeless().MutateAsImageSharp(operation);
        }        

        public static void MutateAsImageSharp(this SpanBitmap self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation)
        {
            void _run<TPixelIn,TPixelOut>(SpanBitmap bmp)
                where TPixelIn : unmanaged
                where TPixelOut : unmanaged, IPixel<TPixelOut>
            {
                bmp.OfType<TPixelIn>().WriteAsImageSharp<TPixelIn, TPixelOut>(img => SixLabors.ImageSharp.Processing.ProcessingExtensions.Mutate(img, operation));
            }

            switch (self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: _run<Pixel.Luminance8, L8>(self); break;
                case Pixel.BGR565.Code: _run<Pixel.BGR565, Bgr565>(self); break;
                case Pixel.BGRA4444.Code: _run<Pixel.BGRA4444, Bgra4444>(self); break;
                case Pixel.BGRA5551.Code: _run<Pixel.BGRA5551, Bgra5551>(self); break;
                case Pixel.BGR24.Code: _run<Pixel.BGR24, Bgr24>(self); break;
                case Pixel.RGBA32.Code: _run<Pixel.RGBA32, Rgba32>(self); break;
                case Pixel.BGRA32.Code: _run<Pixel.BGRA32, Bgra32>(self); break;
                case Pixel.ARGB32.Code: _run<Pixel.ARGB32, Argb32>(self); break;
                case Pixel.RGBA128F.Code: _run<Pixel.RGBA128F, RgbaVector>(self); break;
                default: throw new NotImplementedException();
            }
        }

        public static void MutateAsImageSharp(this PointerBitmap self, Action<SixLabors.ImageSharp.Processing.IImageProcessingContext> operation)
        {
            void _run<TPixel>(PointerBitmap bmp) where TPixel : unmanaged, IPixel<TPixel>
            {
                bmp.WriteAsImageSharp<TPixel>(img => SixLabors.ImageSharp.Processing.ProcessingExtensions.Mutate(img, operation));
            }

            switch (self.Info.PixelFormat.Code)
            {
                case Pixel.Luminance8.Code: _run<L8>(self); break;
                case Pixel.BGR565.Code: _run<Bgr565>(self); break;
                case Pixel.BGRA4444.Code: _run<Bgra4444>(self); break;
                case Pixel.BGRA5551.Code: _run<Bgra5551>(self); break;
                case Pixel.BGR24.Code: _run<Bgr24>(self); break;
                case Pixel.RGBA32.Code: _run<Rgba32>(self); break;
                case Pixel.BGRA32.Code: _run<Bgra32>(self); break;
                case Pixel.ARGB32.Code: _run<Argb32>(self); break;
                case Pixel.RGBA128F.Code: _run<RgbaVector>(self); break;
                default: throw new NotImplementedException();
            }
        }

        #endregion

        #region nested types

        readonly struct _ImageSharpChangedMonitor : IDisposable
        {
            public static void Run<TPixel>(Image<TPixel> image, Action<Image<TPixel>> action)
                where TPixel : unmanaged, IPixel<TPixel>
            {
                using(var monitor = new _ImageSharpChangedMonitor(image)) { action(image); }                
            }

            public _ImageSharpChangedMonitor(Image image) { _Image = image; _Width = image.Width; _Height = image.Height; }

            public void Dispose()
            {
                bool changed = false;
                changed |= _Image.Width != _Width;
                changed |= _Image.Height != _Height;

                if (changed) throw new InvalidOperationException("Actions can't change image size.");
            }

            private readonly Image _Image;
            private readonly int _Width;
            private readonly int _Height;            
        }

        #endregion
    }
}
