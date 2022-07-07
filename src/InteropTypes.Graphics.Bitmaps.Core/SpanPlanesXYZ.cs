using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace InteropTypes.Graphics.Bitmaps
{
    /// <summary>
    /// Represents a bitmap where the R,G,B components have been split into separate planes.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public readonly ref partial struct SpanPlanesXYZ<TComponent>
        where TComponent : unmanaged
    {
        #region constructors

        private const string _Error_AllChannelsEqualSize = "all channels must have the same size";

        public SpanPlanesXYZ(SpanBitmap<TComponent> x, SpanBitmap<TComponent> y, SpanBitmap<TComponent> z)
        {
            if (x.Width != y.Width || x.Height != y.Height) throw new ArgumentException(_Error_AllChannelsEqualSize, nameof(y));
            if (x.Width != z.Width || x.Height != z.Height) throw new ArgumentException(_Error_AllChannelsEqualSize, nameof(z));

            X = x;
            Y = y;
            Z = z;
        }

        public unsafe SpanPlanesXYZ(int width, int height)
        {
            var dataX = System.Runtime.InteropServices.MemoryMarshal.Cast<TComponent, byte>(new TComponent[width * height]);
            var dataY = System.Runtime.InteropServices.MemoryMarshal.Cast<TComponent, byte>(new TComponent[width * height]);
            var dataZ = System.Runtime.InteropServices.MemoryMarshal.Cast<TComponent, byte>(new TComponent[width * height]);

            X = new SpanBitmap<TComponent>(dataX, width, height);
            Y = new SpanBitmap<TComponent>(dataY, width, height);
            Z = new SpanBitmap<TComponent>(dataZ, width, height);
        }

        #endregion

        #region data

        /// <summary>
        /// The plane X, usually the Red component.
        /// </summary>
        public readonly SpanBitmap<TComponent> X;

        /// <summary>
        /// The plane Y, usually the Green component.
        /// </summary>
        public readonly SpanBitmap<TComponent> Y;

        /// <summary>
        /// The plane Z, usually the Blue component.
        /// </summary>
        public readonly SpanBitmap<TComponent> Z;

        #endregion

        #region properties

        public BitmapBounds Bounds => X.Bounds;
        public int Width => X.Width;
        public int Height => X.Height;

        #endregion

        #region API        

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src, bool useBilinear)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            SetPixels(location, src, useBilinear, Vector3.One, Vector3.Zero);
        }

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src, bool useBilinear, Vector3 pixelMultiply, Vector3 pixelAddition)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            var xform = new Processing.PlanesTransform(location, useBilinear);
            xform.PixelMultiply = pixelMultiply;
            xform.PixelAddition = pixelAddition;

            if (typeof(TComponent) == typeof(float))
            {
                var xx = X.ReinterpretAs<float>();
                var yy = Y.ReinterpretAs<float>();
                var zz = Z.ReinterpretAs<float>();
                var dst = new SpanPlanesXYZ<float>(xx, yy, zz);                
                    
                xform.TryTransfer(src, dst);
                return;
            }

            throw new NotImplementedException();
        }

        public void CopyTo(ref MemoryBitmap<Pixel.BGR96F> dst)
        {
            if (this.Width != dst.Width || this.Height != dst.Height)
            {
                dst = new MemoryBitmap<Pixel.BGR96F>(this.Width, this.Height);
            }

            if (typeof(TComponent) == typeof(float))
            {
                var xx = X.ReinterpretAs<float>();
                var yy = Y.ReinterpretAs<float>();
                var zz = Z.ReinterpretAs<float>();
                var src = new SpanPlanesXYZ<float>(xx,yy,zz);

                _CopyTo(src, dst);
                return;
            }

            throw new NotImplementedException();
        }

        private static void _CopyTo(SpanPlanesXYZ<float> src, SpanBitmap<Pixel.BGR96F> dst)
        {
            for (int y = 0; y < dst.Height; ++y)
            {
                var dstRow = dst.UseScanlinePixels(y);
                var srcRowX = src.X.GetScanlinePixels(y);
                var srcRowY = src.Y.GetScanlinePixels(y);
                var srcRowZ = src.Z.GetScanlinePixels(y);

                for (int x = 0; x < dstRow.Length; ++x)
                {
                    ref var dstPix = ref dstRow[x];

                    dstPix.R = srcRowX[x];
                    dstPix.G = srcRowY[x];
                    dstPix.B = srcRowZ[x];
                }
            }
        }        

        #endregion
    }

}
