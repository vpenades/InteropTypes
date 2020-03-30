using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace InteropBitmaps.Adapters
{
    public readonly ref struct WPFAdapter
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

        #region API        

        public void Update(ref WriteableBitmap bmp)
        {
            // TODO: check compatible

            if (bmp == null) bmp = CloneToWritableBitmap(true);
            else bmp.SetPixels(0, 0, _Bitmap);
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

            var src = BitmapSource.Create(ptr.Info.Width, ptr.Info.Height, 96, 96, fmt, null, ptr.Pointer, ptr.Info.BitmapByteSize, ptr.Info.ScanlineByteSize);

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

            rt.CopyPixels(System.Windows.Int32Rect.Empty, ptr.Pointer, ptr.Info.BitmapByteSize, ptr.Info.ScanlineByteSize);
        }

        #endregion
    }
}
