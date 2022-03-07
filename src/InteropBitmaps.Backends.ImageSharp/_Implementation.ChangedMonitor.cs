using System;
using System.Collections.Generic;
using System.Text;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes
{
    readonly struct _ImageSharpChangedMonitor : IDisposable
    {
        #region static API        

        public static TResult ReadAsImageSharp<TSelfPixel, TOtherPixel, TResult>(SpanBitmap<TSelfPixel> self, Func<Image<TOtherPixel>, TResult> function)
            where TSelfPixel : unmanaged
            where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            _Verify<TSelfPixel, TOtherPixel>(self.Info);

            if (self.Info.IsContinuous)
            {
                return self.PinReadablePointer(ptr => ReadAsImageSharp<TSelfPixel, TOtherPixel, TResult>(ptr, function));
            }
            else
            {
                using (var selfImg = new Image<TOtherPixel>(self.Width, self.Height))
                {
                    _Implementation.CopyPixels(self, selfImg);
                    return Run(selfImg, function);
                }
            }
        }

        public static TResult ReadAsImageSharp<TSelfPixel, TOtherPixel, TResult>(PointerBitmap self, Func<Image<TOtherPixel>, TResult> function)
            where TSelfPixel : unmanaged
            where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            _Verify<TSelfPixel, TOtherPixel>(self.Info);

            if (self.Info.IsContinuous)
            {
                var m = self.UsingMemory<TOtherPixel>();
                var s = self.Size;

                using (var selfImg = Image.WrapMemory(m, s.Width, s.Height))
                {
                    return Run(selfImg, function);
                }
            }
            else
            {
                using (var selfImg = new Image<TOtherPixel>(self.Width, self.Height))
                {
                    _Implementation.CopyPixels(self, selfImg);
                    return Run(selfImg, function);
                }
            }
        }

        public static void WriteAsImageSharp<TSelfPixel, TOtherPixel>(SpanBitmap<TSelfPixel> self, Action<Image<TOtherPixel>> action)
           where TSelfPixel : unmanaged
           where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            _Verify<TSelfPixel, TOtherPixel>(self.Info);

            if (self.Info.IsContinuous)
            {
                self.PinWritablePointer(ptr => WriteAsImageSharp<TSelfPixel, TOtherPixel>(ptr, action));
                return;
            }
            else
            {
                using (var selfImg = new Image<TOtherPixel>(self.Width, self.Height))
                {
                    _Implementation.CopyPixels(self, selfImg);
                    Run(selfImg, action);
                    _Implementation.CopyPixels(selfImg, self);
                }
            }
        }

        public static void WriteAsImageSharp<TSelfPixel, TOtherPixel>(PointerBitmap self, Action<Image<TOtherPixel>> action)
            where TSelfPixel : unmanaged
            where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            _Verify<TSelfPixel, TOtherPixel>(self.Info);

            if (self.Info.IsContinuous)
            {
                var m = self.UsingMemory<TOtherPixel>();
                var s = self.Size;

                using (var selfImg = Image.WrapMemory(m, s.Width, s.Height))
                {
                    Run(selfImg, action);
                }
            }
            else
            {
                using (var selfImg = new Image<TOtherPixel>(self.Width, self.Height))
                {
                    _Implementation.CopyPixels(self, selfImg);
                    Run(selfImg, action);
                    _Implementation.CopyPixels(selfImg, self);
                }
            }
        }

        #endregion

        #region core

        [System.Diagnostics.DebuggerStepThrough]
        private static unsafe void _Verify<TSelfPixel, TOtherPixel>(BitmapInfo info)
            where TSelfPixel : unmanaged
            where TOtherPixel : unmanaged, IPixel<TOtherPixel>
        {
            if (info.IsEmpty) throw new ArgumentNullException(nameof(info));

            if (sizeof(TSelfPixel) != sizeof(TOtherPixel)) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);
            if (sizeof(TOtherPixel) != info.PixelByteSize) throw new ArgumentException("pixel size mismatch", typeof(TOtherPixel).Name);
        }

        private static void Run<TPixel>(Image<TPixel> image, Action<Image<TPixel>> action)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var monitor = new _ImageSharpChangedMonitor(image)) { action(image); }
        }

        private static TResult Run<TPixel, TResult>(Image<TPixel> image, Func<Image<TPixel>, TResult> function)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            using (var monitor = new _ImageSharpChangedMonitor(image)) { return function(image); }
        }

        private _ImageSharpChangedMonitor(Image image) { _Image = image; _Width = image.Width; _Height = image.Height; }

        public void Dispose()
        {
            bool changed = false;
            changed |= _Image.Width != _Width;
            changed |= _Image.Height != _Height;

            if (changed) throw new InvalidOperationException("Actions can't change image size.");
        }

        #endregion

        #region data

        private readonly Image _Image;
        private readonly int _Width;
        private readonly int _Height;

        #endregion
    }
}
