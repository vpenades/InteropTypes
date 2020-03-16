using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps
{
    sealed class WritableBitmapLock : ISpanLock
    {
        #region lifecycle

        public static WritableBitmapLock TryLock(System.Windows.Media.Imaging.WriteableBitmap bmp, bool readOnly)
        {
            // https://github.com/dotnet/wpf/blob/c9d96d181b17c71bd7976af8793451bd13a8f66b/src/Microsoft.DotNet.Wpf/src/PresentationCore/System/Windows/Media/Imaging/WriteableBitmap.cs#L251
            if (!bmp.TryLock(System.Windows.Duration.Forever)) return null;

            return new WritableBitmapLock(bmp, readOnly);
        }

        public WritableBitmapLock(System.Windows.Media.Imaging.WriteableBitmap bmp, bool readOnly)
        {
            _Bitmap = bmp;
            _Pointer = new PointerBitmap(bmp.BackBuffer, bmp.GetBitmapInfo(), readOnly);
        }

        public void Dispose()
        {
            if (_Bitmap == null) return;

            if (!_Pointer.IsReadOnly)
            {
                var rect = new System.Windows.Int32Rect(0, 0, _Bitmap.PixelWidth, _Bitmap.PixelHeight);
                _Bitmap.AddDirtyRect(rect);
            }

            _Bitmap.Unlock();

            _Bitmap = null;
            _Pointer = default(PointerBitmap);
        }

        #endregion

        #region data

        private System.Windows.Media.Imaging.WriteableBitmap _Bitmap;
        private PointerBitmap _Pointer;        

        #endregion

        #region Properties

        public BitmapInfo Info => _Pointer.Info;

        public SpanBitmap Span => _Pointer.Bitmap;

        #endregion
    }
}
