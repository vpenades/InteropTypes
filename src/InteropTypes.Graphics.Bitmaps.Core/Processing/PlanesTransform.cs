using System;
using System.Collections.Generic;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps.Processing
{
    using TRANSFORM = System.Numerics.Matrix3x2;

    public struct PlanesTransform : SpanBitmap.ITransfer,
        SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGB24>,
        SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGR24>,
        SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGR96F>,
        SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGB96F>,

        SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGB24>,
        SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGR24>,
        SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGR96F>,        
        SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGB96F>
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

        #region API - SpanBitmap.ITransfer

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

        #region API - SpanBitmap.ITransfer<RGB24,dst>

        bool SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGB24>.TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGB24> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            var pop = new Pixel.RGB24.MulAdd(this.PixelOp);

            _PixelsTransformImplementation.PixelsConvert(source, target, this, pop);
            return true;
        }

        bool SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGR24>.TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGR24> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            var pop = new Pixel.BGR24.MulAdd(this.PixelOp);

            _PixelsTransformImplementation.PixelsConvert(source, target, this, pop);
            return true;
        }

        bool SpanBitmap.ITransfer<Pixel.RGB24, Pixel.RGB96F>.TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.RGB96F> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            _PixelsTransformImplementation.PixelsConvert(source, target, this, this.PixelOp);
            return true;
        }

        bool SpanBitmap.ITransfer<Pixel.RGB24, Pixel.BGR96F>.TryTransfer(SpanBitmap<Pixel.RGB24> source, SpanBitmap<Pixel.BGR96F> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            var pop = new Pixel.BGR96F.MulAdd(this.PixelOp);

            _PixelsTransformImplementation.PixelsConvert(source, target, this, pop);
            return true;
        }

        #endregion

        #region API - SpanBitmap.ITransfer<BGR24,dst>
        bool SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGB24>.TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGB24> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            var pop = new Pixel.RGB24.MulAdd(this.PixelOp);

            _PixelsTransformImplementation.PixelsConvert(source, target, this, pop);
            return true;
        }
        bool SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGR24>.TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGR24> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            var pop = new Pixel.BGR24.MulAdd(this.PixelOp);

            _PixelsTransformImplementation.PixelsConvert(source, target, this, pop);
            return true;
        }
        
        bool SpanBitmap.ITransfer<Pixel.BGR24, Pixel.BGR96F>.TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.BGR96F> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            var pop = new Pixel.BGR96F.MulAdd(this.PixelOp);

            _PixelsTransformImplementation.PixelsConvert(source, target, this, pop);
            return true;
        }       
        bool SpanBitmap.ITransfer<Pixel.BGR24, Pixel.RGB96F>.TryTransfer(SpanBitmap<Pixel.BGR24> source, SpanBitmap<Pixel.RGB96F> target)
        {
            if (this.PixelOp.IsIdentity)
            {
                _PixelsTransformImplementation.OpaquePixelsConvert(source, target, this.Transform, this.UseBilinear);
                return true;
            }

            _PixelsTransformImplementation.PixelsConvert(source, target, this, this.PixelOp);
            return true;
        }        

        #endregion
    }
}
