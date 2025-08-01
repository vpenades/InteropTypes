using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Backends;
using InteropTypes.Graphics.Bitmaps;

using AVALONIAFORMAT = Avalonia.Platform.PixelFormat;
using AVALONIAALPHA = Avalonia.Platform.AlphaFormat;

using AVLWBITMAP = Avalonia.Media.Imaging.WriteableBitmap;

namespace InteropTypes.Graphics.Adapters
{
    public readonly ref struct AVLAdapter
    {
        #region constructor

        public AVLAdapter(SpanBitmap bmp)
        {
            _Bitmap = bmp;

            if (!_Implementation.TryGetPixelFormat(bmp.PixelFormat, out _ColorExact, out _AlphaExact))
            {
                throw new Diagnostics.PixelFormatNotSupportedException(bmp.PixelFormat, nameof(bmp));
            }
        }

        #endregion

        #region data

        private readonly SpanBitmap _Bitmap;
        private readonly AVALONIAFORMAT _ColorExact;
        private readonly AVALONIAALPHA _AlphaExact;

        #endregion

        #region properties

        public SpanBitmap Source => _Bitmap;

        #endregion

        #region API        

        public bool CopyTo(ref AVLWBITMAP dst, bool allowCompatibleFormats = true)
        {
            if (_Bitmap.IsEmpty)
            {
                var changed = dst != null;
                dst = null;
                return changed;
            }

            var dstHdr = dst != null ? _Implementation.GetBitmapInfo(dst) : default;

            if (dst != null && _Bitmap.Info != dstHdr)
            {
                if (!allowCompatibleFormats || dstHdr.Size != _Bitmap.Info.Size) dst = null;
                else
                {
                    throw new NotImplementedException("use exact formats");
                    // var expected = _Implementation.GetCompatiblePixelFormat(_Bitmap.Info.PixelFormat);
                    // if (expected != dst.Format) dst = null;
                }                
            }

            if (dst == null)
            {
                dst = CloneToWritableBitmap(allowCompatibleFormats);
                return true;
            }
            else
            {
                using(var l = dst.Lock())
                {
                    _Implementation.AsPointerBitmap(l).AsSpanBitmap().SetPixels(0, 0, _Bitmap);
                }
                
                return false;
            }
        }        

        public AVLWBITMAP CloneToWritableBitmap(bool allowCompatibleFormats = false)
        {
            return _Implementation.ToAvaloniaBitmap(_Bitmap);
        }                

        #endregion
    }
}
