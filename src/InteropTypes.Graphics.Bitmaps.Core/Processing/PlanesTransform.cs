using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    public struct PlanesTransform
    {
        #region constructors

        public PlanesTransform(in TRANSFORM xform)
        {
            Transform = xform;            
            UseBilinear = false;
            PixelOp = (1,0);
        }

        public PlanesTransform(in TRANSFORM xform, bool useBilinear)
        {
            Transform = xform;            
            UseBilinear = useBilinear;
            PixelOp = (1, 0);
        }

        #endregion

        #region data

        public TRANSFORM Transform { get; set; }        
        public bool UseBilinear { get; set; }        
        public Pixel.RGB96F.MulAdd PixelOp { get; set; }

        #endregion

        #region API - SpanPlanesXYZ
        public bool TryTransfer<TSrcPixel>(SpanBitmap<TSrcPixel> source, SpanPlanesXYZ<float> target)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            _PixelsTransformImplementation.PixelsConvert(source, target, this);
            return true;
        }

        #endregion
    }
}
