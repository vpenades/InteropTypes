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

        #region API - Cast

        public SpanPlanesXYZ<TOtherComponent> AsExplicit<TOtherComponent>()
            where TOtherComponent : unmanaged
        {
            var xx = X.AsExplicit<TOtherComponent>();
            var yy = Y.AsExplicit<TOtherComponent>();
            var zz = Z.AsExplicit<TOtherComponent>();

            return new SpanPlanesXYZ<TOtherComponent>(xx, yy, zz);
        }

        public SpanPlanesXYZ<TOtherComponent> ReinterpretAs<TOtherComponent>()
            where TOtherComponent : unmanaged
        {
            var xx = X.ReinterpretAs<TOtherComponent>();
            var yy = Y.ReinterpretAs<TOtherComponent>();
            var zz = Z.ReinterpretAs<TOtherComponent>();

            return new SpanPlanesXYZ<TOtherComponent>(xx, yy, zz);
        }

        #endregion

        #region API        

        public SpanPlanesXYZ<TComponent> Slice(in BitmapBounds rect)
        {
            var xx = X.Slice(rect);
            var yy = Y.Slice(rect);
            var zz = Z.Slice(rect);

            return new SpanPlanesXYZ<TComponent>(xx, yy, zz);
        }

        public void SetPixels<TSrcPixel>(SpanBitmap<TSrcPixel> src)
            where TSrcPixel:unmanaged, Pixel.IConvertTo
        {
            if (typeof(TComponent) == typeof(Byte)) { _SetPixels(src, AsExplicit<Byte>()); return; }
            if (typeof(TComponent) == typeof(float)) { _SetPixels(src, AsExplicit<float>()); return; }
        }

        private static void _SetPixels<TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanPlanesXYZ<Byte> dst)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            var w = Math.Min(src.Width, dst.Width);
            var h = Math.Min(src.Height, dst.Height);

            Pixel.BGR24 p = default;

            for (int y = 0; y < h; y++)
            {
                var srcRow = src.GetScanlinePixels(y);
                var dstRowX = dst.X.UseScanlinePixels(y);
                var dstRowY = dst.Y.UseScanlinePixels(y);
                var dstRowZ = dst.Z.UseScanlinePixels(y);

                for (int x = 0; x < w; ++x)
                {
                    srcRow[x].CopyTo(ref p);

                    dstRowX[x] = p.R;
                    dstRowY[x] = p.G;
                    dstRowZ[x] = p.B;
                }
            }
        }

        private static void _SetPixels<TSrcPixel>(SpanBitmap<TSrcPixel> src, SpanPlanesXYZ<Single> dst)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            var w = Math.Min(src.Width, dst.Width);
            var h = Math.Min(src.Height, dst.Height);

            Pixel.BGR96F p = default;

            for (int y = 0; y < h; y++)
            {
                var srcRow = src.GetScanlinePixels(y);
                var dstRowX = dst.X.UseScanlinePixels(y);
                var dstRowY = dst.Y.UseScanlinePixels(y);
                var dstRowZ = dst.Z.UseScanlinePixels(y);

                for(int x=0; x < w; ++x)
                {
                    srcRow[x].CopyTo(ref p);

                    dstRowX[x] = p.R;
                    dstRowY[x] = p.G;
                    dstRowZ[x] = p.B;
                }
            }
        }

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src, bool useBilinear)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            SetPixels(location, src, useBilinear, (1,0));
        }

        public void SetPixels<TSrcPixel>(in Matrix3x2 location, SpanBitmap<TSrcPixel> src, bool useBilinear, Pixel.RGB96F.MulAdd pixelOp)
            where TSrcPixel : unmanaged, Pixel.IConvertTo
        {
            var xform = new Processing.PlanesTransform(location, useBilinear);
            xform.PixelOp = pixelOp;

            // if (typeof(TComponent) == typeof(Byte)) { xform.TryTransfer(src, _GetExplicit<Byte>()); return; }
            if (typeof(TComponent) == typeof(float)) { xform.TryTransfer(src, AsExplicit<float>()); return; }

            throw new NotImplementedException();
        }

        public void CopyTo<TDstPixel>(ref MemoryBitmap<TDstPixel> dst)
            where TDstPixel:unmanaged
        {
            if (this.Width != dst.Width || this.Height != dst.Height)
            {
                dst = new MemoryBitmap<TDstPixel>(this.Width, this.Height);
            }

            if (typeof(TComponent) == typeof(Byte)) { _CopyTo<TDstPixel>(AsExplicit<Byte>(), dst); return; }
            if (typeof(TComponent) == typeof(float)) { _CopyTo<TDstPixel>(AsExplicit<float>(), dst); return; }

            throw new NotImplementedException();
        }

        private static void _CopyTo<TDstPixel>(SpanPlanesXYZ<Byte> src, SpanBitmap<TDstPixel> dst)
            where TDstPixel : unmanaged
        {
            Pixel.BGR24 pix = default;

            for (int y = 0; y < dst.Height; ++y)
            {
                var dstRow = dst.UseScanlinePixels(y);
                var srcRowX = src.X.GetScanlinePixels(y);
                var srcRowY = src.Y.GetScanlinePixels(y);
                var srcRowZ = src.Z.GetScanlinePixels(y);

                for (int x = 0; x < dstRow.Length; ++x)
                {
                    pix.R = srcRowX[x];
                    pix.G = srcRowY[x];
                    pix.B = srcRowZ[x];

                    pix.CopyTo(ref dstRow[x]);
                }
            }
        }

        private static void _CopyTo<TDstPixel>(SpanPlanesXYZ<Single> src, SpanBitmap<TDstPixel> dst)
            where TDstPixel : unmanaged
        {
            Pixel.BGR96F pix = default;

            for (int y = 0; y < dst.Height; ++y)
            {
                var dstRow = dst.UseScanlinePixels(y);
                var srcRowX = src.X.GetScanlinePixels(y);
                var srcRowY = src.Y.GetScanlinePixels(y);
                var srcRowZ = src.Z.GetScanlinePixels(y);

                for (int x = 0; x < dstRow.Length; ++x)
                {
                    pix.R = srcRowX[x];
                    pix.G = srcRowY[x];
                    pix.B = srcRowZ[x];

                    pix.CopyTo(ref dstRow[x]);
                }
            }
        }

        public void ApplyAddMultiply(Single multiply, Single addition)
        {
            ApplyAddMultiply(new Vector3(multiply), new Vector3(addition));
        }

        public void ApplyAddMultiply(Vector3 multiply, Vector3 addition)
        {
            ApplyMultiplyAdd(multiply, addition * multiply);
        }

        public void ApplyMultiplyAdd(Single multiply, Single addition)
        {
            ApplyMultiplyAdd(new Vector3(multiply), new Vector3(addition));
        }

        public void ApplyMultiplyAdd(Vector3 multiply, Vector3 addition)
        {
            this.X.ApplyMultiplyAdd(multiply.X, addition.X);
            this.Y.ApplyMultiplyAdd(multiply.Y, addition.Y);
            this.Z.ApplyMultiplyAdd(multiply.Z, addition.Z);
        }

        public void ApplyClamp(Vector3 min, Vector3 max)
        {
            this.X.ApplyClamp(min.X, max.X);
            this.Y.ApplyClamp(min.Y, max.Y);
            this.Z.ApplyClamp(min.Z, max.Z);
        }

        #endregion

        #region API - IO

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void Save(Action<Action<System.IO.FileInfo>> saveCallback, params Codecs.IBitmapEncoder[] factory)
        {
            MemoryBitmap<Pixel.BGR24> img = default;
            CopyTo(ref img);
            
            saveCallback(finfo => img.Save(finfo.FullName, factory));
        }

        public void Save(string filePath, params Codecs.IBitmapEncoder[] factory)
        {
            MemoryBitmap<Pixel.BGR24> img = default;
            CopyTo(ref img);

            img.AsSpanBitmap().Save(filePath, factory);
        }

        public void Write(System.IO.Stream stream, Codecs.CodecFormat format, params Codecs.IBitmapEncoder[] factory)
        {
            MemoryBitmap<Pixel.BGR24> img = default;
            CopyTo(ref img);

            img.AsSpanBitmap().Write(stream, format, factory);
        }

        #endregion        
    }

}
