using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Adapters
{
    public readonly struct OpenCvSharp4Factory
    {
        internal OpenCvSharp4Factory(BitmapInfo binfo)
        {
            _Info = binfo;
            
        }

        private readonly BitmapInfo _Info;

        public readonly OpenCvSharp.Mat CreateMat()
        {
            var mtype = OpenCvSharp.MatType.CV_8UC(_Info.PixelByteSize);

            return new OpenCvSharp.Mat(_Info.Height, _Info.Width, mtype);
        }
    }
}
