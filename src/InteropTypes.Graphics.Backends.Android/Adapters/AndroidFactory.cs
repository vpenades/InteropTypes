using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

using ANDROIDBITMAP = Android.Graphics.Bitmap;
using ANDROIDGFX = Android.Graphics;
using ANDROIDMEDIA = Android.Media;

namespace InteropTypes.Graphics.Adapters
{
    public readonly struct AndroidFactory
    {
        #region lifecycle

        internal AndroidFactory(BitmapInfo binfo, ANDROIDBITMAP.Config defFmt = null)
        {
            if (defFmt == null) defFmt = ANDROIDBITMAP.Config.Argb8888;

            _Info = binfo;
            _Exact = _Implementation.ToAndroidBitmapConfig(_Info.PixelFormat, null);
            _Compatible = _Implementation.ToAndroidBitmapConfig(_Info.PixelFormat, defFmt);
        }

        #endregion

        #region data

        private readonly BitmapInfo _Info;
        private readonly ANDROIDBITMAP.Config _Exact;
        private readonly ANDROIDBITMAP.Config _Compatible;

        #endregion

        #region API

        public ANDROIDBITMAP CreateCompatibleBitmap()
        {
            return ANDROIDBITMAP.CreateBitmap(_Info.Width, _Info.Height, _Compatible);
        }

        #endregion
    }
}
