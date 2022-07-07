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
            PixelMultiply = System.Numerics.Vector3.One;
            PixelAddition = System.Numerics.Vector3.Zero;
        }

        public PlanesTransform(in TRANSFORM xform, bool useBilinear)
        {
            Transform = xform;            
            UseBilinear = useBilinear;
            PixelMultiply = System.Numerics.Vector3.One;
            PixelAddition = System.Numerics.Vector3.Zero;
        }

        #endregion

        #region data

        public TRANSFORM Transform { get; set; }        
        public bool UseBilinear { get; set; }

        public System.Numerics.Vector3 PixelMultiply { get; set; }
        public System.Numerics.Vector3 PixelAddition { get; set; }

        #endregion

        #region API

        public bool TryTransfer<TSrcPixel>(SpanBitmap<TSrcPixel> source, SpanPlanesXYZ<float> target)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            _PixelsTransformImplementation.PixelsConvert(source, target, Transform, UseBilinear, PixelMultiply, PixelAddition);
            return true;
        }

        #endregion
    }
}
