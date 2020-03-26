using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps.Adapters
{
    public struct WPFFactory
    {
        internal WPFFactory(BitmapInfo binfo)
        {
            _Info = binfo;
            _Exact = _Implementation.ToPixelFormat(binfo.PixelFormat);
        }

        private readonly BitmapInfo _Info;
        private readonly System.Windows.Media.PixelFormat _Exact;

        public System.Windows.Media.Imaging.WriteableBitmap CreateImage()
        {
            return new System.Windows.Media.Imaging.WriteableBitmap(_Info.Width, _Info.Height, 96, 96, _Exact, null);
        }
    }
}
