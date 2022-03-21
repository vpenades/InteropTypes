using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    public struct ImageSharpFactory
    {
        internal ImageSharpFactory(BitmapInfo binfo)
        {
            _Info = binfo;
            _Implementation.TryGetExactPixelType(binfo.PixelFormat, out _Exact);            
        }

        private readonly BitmapInfo _Info;
        private readonly Type _Exact;

        public SixLabors.ImageSharp.Image CreateImage()
        {
            return _Implementation.CreateImageSharp(_Info.PixelFormat, _Info.Width, _Info.Height);
        }
    }
}
