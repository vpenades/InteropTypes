using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    public partial struct BitmapTransform : SpanBitmap.ITransfer
    {
        #region constructors
        public BitmapTransform(in TRANSFORM xform, float opacity = 1)
        {
            Transform = xform;
            Opacity = opacity;
            UseBilinear = false;
        }
        public BitmapTransform(in TRANSFORM xform, bool useBilinear, float opacity)
        {
            Transform = xform;
            Opacity = opacity;
            UseBilinear = useBilinear;
        }

        #endregion

        #region data

        public TRANSFORM Transform { get; set; }
        public float Opacity { get; set; }
        public bool UseBilinear { get; set; }

        #endregion

        #region API

        bool SpanBitmap.ITransfer.TryTransfer<TsrcPixel, TDstPixel>(SpanBitmap<TsrcPixel> source, SpanBitmap<TDstPixel> target)
        {
            if (this is SpanBitmap.ITransfer<TsrcPixel, TDstPixel> transferX)
            {
                return transferX.TryTransfer(source, target);
            }

            return false;
        }

        bool SpanBitmap.ITransfer.TryTransfer<TPixel>(SpanBitmap<TPixel> source, SpanBitmap<TPixel> target)
        {
            if (this is SpanBitmap.ITransfer<TPixel, TPixel> transferX)
            {
                return transferX.TryTransfer(source, target);
            }

            return false;
        }

        bool SpanBitmap.ITransfer.TryTransfer(SpanBitmap source, SpanBitmap target)
        {
            // TODO: try to infer the pixel size for basic overwrite transform

            return false;
        }        

        #endregion
    }    
}
