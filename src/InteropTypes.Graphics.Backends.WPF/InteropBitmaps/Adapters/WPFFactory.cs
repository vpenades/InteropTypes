using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    public struct WPFFactory
    {
        internal WPFFactory(BitmapInfo binfo)
        {
            _Info = binfo;

            if (!_Implementation.TryGetExactPixelFormat(binfo.PixelFormat, out _Exact))
            {
                throw new Diagnostics.PixelFormatNotSupportedException(binfo.PixelFormat, nameof(binfo));
            }            
        }

        private readonly BitmapInfo _Info;
        private readonly System.Windows.Media.PixelFormat _Exact;

        public System.Windows.Media.Imaging.WriteableBitmap CreateImage()
        {
            return new System.Windows.Media.Imaging.WriteableBitmap(_Info.Width, _Info.Height, 96, 96, _Exact, null);
        }
    }
}
