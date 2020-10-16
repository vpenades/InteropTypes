using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InteropBitmaps.Adapters
{
    public readonly ref struct WPFAdapter // WpfSpanBitmap
    {
        #region constructor

        public WPFAdapter(SpanBitmap bmp)
        {
            _Bitmap = bmp;
            _Exact = _Implementation.ToPixelFormat(bmp.PixelFormat);
        }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;
        private readonly System.Windows.Media.PixelFormat _Exact;

        #endregion

        #region properties

        public SpanBitmap Source => _Bitmap;

        #endregion

        #region API        

        public bool CopyTo(ref WriteableBitmap dst, bool allowCompatibleFormats = true)
        {
            var dstHdr = dst?.GetBitmapInfo() ?? default;

            if (dst != null && _Bitmap.Info != dstHdr)
            {
                if (!allowCompatibleFormats || dstHdr.Size != _Bitmap.Info.Size) dst = null;
                else
                {
                    var expected = _Implementation.ToBestMatch(_Bitmap.Info.PixelFormat);
                    if (expected != dst.Format) dst = null;
                }
                
            }

            if (dst == null) { dst = CloneToWritableBitmap(allowCompatibleFormats); return true; }
            else { dst.SetPixels(0, 0, _Bitmap); return false; }
        }

        public bool CopyTo(ref CroppedBitmap dst, bool allowCompatibleFormats = true)
        {
            // CroppedBitmap implements the ISupportInitialize interface to optimize initialization on multiple properties.
            // Property changes can occur only during object initialization.
            // Call BeginInit to signal that initialization has begun and EndInit to signal that initialization has completed.
            // AFTER INITIALIZATION, PROPERTY CHANGES ARE IGNORED.

            // update writeable bitmap
            
            var dstWrt = dst?.Source as WriteableBitmap;
            var dstRct = dst?.SourceRect ?? System.Windows.Int32Rect.Empty;

            if (dstWrt != null)
            {
                var dstInfo = dst.GetBitmapInfo();
                if (dstInfo.Width < _Bitmap.Width) dstWrt = null;
                if (dstInfo.Height < _Bitmap.Height) dstWrt = null;
                if (dstInfo.PixelFormat != _Bitmap.PixelFormat)
                {
                    if (!allowCompatibleFormats) dstWrt = null;
                    else
                    {
                        var expected = _Implementation.ToBestMatch(_Bitmap.Info.PixelFormat);
                        if (expected != dst.Format) dstWrt = null;
                    }
                }
            }            

            if (dstWrt == null) dstWrt = _Implementation.ToWritableBitmap(_Bitmap.Info);

            dstWrt.SetPixels(0, 0, _Bitmap);

            dstRct = new System.Windows.Int32Rect(0, 0, _Bitmap.Width, _Bitmap.Height);            

            if (dst != null)
            {
                if (!Object.ReferenceEquals(dst.Source, dstWrt)) dst = null;
                if (dst != null && dst.SourceRect != dstRct) dst = null;
            }

            if (dst != null) return false;
            
            dst = new CroppedBitmap(dstWrt, dstRct);
            return true;            
        }        

        public WriteableBitmap CloneToWritableBitmap(bool allowCompatibleFormats = false)
        {
            return _Implementation.ToWritableBitmap(_Bitmap);
        }        

        public void Draw(Action<System.Windows.Media.DrawingContext> onDraw)
        {
            // WIP, DO NOT USE IT YET

            if (_Exact != System.Windows.Media.PixelFormats.Bgra32) throw new NotSupportedException("the only supported format is Bgra32");

            var fmt = _Exact;            

            _Bitmap.PinWritablePointer(ptr => _Draw(ptr, fmt, onDraw));
        }

        private static void _Draw(PointerBitmap ptr, System.Windows.Media.PixelFormat fmt, Action<System.Windows.Media.DrawingContext> onDraw)
        {
            // https://stackoverflow.com/questions/88488/getting-a-drawingcontext-for-a-wpf-writeablebitmap
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/84299fec-94a1-49a1-b3bc-ec48b8bdf04f/getting-a-drawingcontext-for-a-writeablebitmap?forum=wpf
            // https://stackoverflow.com/questions/7250282/how-to-draw-directly-on-bitmap-bitmapsource-writeablebitmap-in-wpf

            var src = BitmapSource.Create(ptr.Info.Width, ptr.Info.Height, 96, 96, fmt, null, ptr.Pointer, ptr.Info.BitmapByteSize, ptr.Info.StepByteSize);

            var dv = new System.Windows.Media.DrawingVisual();
            var dc = dv.RenderOpen();
            {
                // dc.DrawImage(src, new System.Windows.Rect(0, 0, src.PixelWidth, src.PixelHeight));

                dc.DrawRectangle(System.Windows.Media.Brushes.Green, null, new System.Windows.Rect(50, 50, 200, 100));

                onDraw(dc);                
            }

            var rt = new RenderTargetBitmap(src.PixelWidth, src.PixelHeight, src.DpiX, src.DpiY, System.Windows.Media.PixelFormats.Pbgra32);
            rt.Render(dv);

            

            // ptr.Bitmap.SetPixels(0, 0, rt.ToMemoryBitmap());            

            rt.CopyPixels(System.Windows.Int32Rect.Empty, ptr.Pointer, ptr.Info.BitmapByteSize, ptr.Info.StepByteSize);
        }

        #endregion
    }
}
