using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    public partial struct BitmapTransform : SpanBitmap.ITransfer
    {
        #region constructors
        public BitmapTransform(in TRANSFORM xform, in Pixel.RGB96F.MulAdd pixOp)
        {
            Transform = xform;
            PixelOp = pixOp;
            UseBilinear = false;
        }
        public BitmapTransform(in TRANSFORM xform, bool useBilinear, in Pixel.RGB96F.MulAdd pixOp)
        {
            Transform = xform;
            PixelOp = pixOp;
            UseBilinear = useBilinear;
        }

        #endregion

        #region data

        public TRANSFORM Transform { get; set; }        
        public bool UseBilinear { get; set; }
        public Pixel.RGB96F.MulAdd PixelOp { get; set; }

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

            if (PixelOp.IsIdentity && UseBilinear == false)
            {
                _PixelsTransformImplementation.OpaquePixelsDirect(source, target, Transform);
                return true;
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
