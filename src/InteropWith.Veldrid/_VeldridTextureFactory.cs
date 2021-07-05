using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using Veldrid;

namespace InteropWith
{
    class _VeldridTextureFactory : IDisposable
    {
        #region lifecycle        
        public _VeldridTextureFactory(GraphicsDevice gd)
        {
            if (gd == null) throw new ArgumentNullException(nameof(gd));

            _Graphics = gd;
            _CommandList = _Graphics.ResourceFactory.CreateCommandList();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _CommandList?.Dispose();
                _CommandList = null;
                _Graphics = null;
            }
        }        

        private void CheckDisposed()
        {
            if (_Graphics == null) throw new ObjectDisposedException("Cannot use this texture storage after it has been disposed.");
        }

        #endregion

        #region data        

        private GraphicsDevice _Graphics;        

        private CommandList _CommandList;

        private Texture _Staging;

        #endregion

        #region API        

        public void SetData(Texture tex, InteropBitmaps.SpanBitmap src)
        {
            CheckDisposed();

            var bpp = src.Info.PixelFormat.ByteCount;            
            
            _CopyData(tex, src.ReadableBytes, 0, 0, tex.Width, tex.Height);
        }

        public void SetData<T>(Texture tex, in Rectangle subRect, ReadOnlySpan<T> data)
            where T : unmanaged
        {
            CheckDisposed();
            
            var bpp = tex.Format == PixelFormat.R8_UNorm ? 1 : 4;
            if (data.Length * Marshal.SizeOf<T>() != tex.Width * tex.Height * bpp)
                throw new ArgumentException($"Length of data (${data.Length * Marshal.SizeOf<T>()}) did not match width * height of the texture (${tex.Width * tex.Height * bpp}).", nameof(data));

            var byteSpan = MemoryMarshal.Cast<T, byte>(data);
            _CopyData(tex, byteSpan, (uint)subRect.X, (uint)subRect.Y, (uint)subRect.Width, (uint)subRect.Height);
        }

        private unsafe void _CopyData(Texture tex, ReadOnlySpan<byte> data, uint tx, uint ty, uint width, uint height)
        {
            var staging = _GetStagingTexture(width, height);            

            var bpp = tex.Format == PixelFormat.R8_UNorm ? 1 : 4;
            var rowBytes = (int)width * bpp;

            var map = _Graphics.Map(staging, MapMode.Write, 0);
            if (rowBytes == map.RowPitch)
            {
                var dst = new Span<byte>((byte*)map.Data.ToPointer() + rowBytes * ty, (int)(rowBytes * height));
                data.CopyTo(dst);
            }
            else
            {
                var stagingPtr = (byte*)map.Data.ToPointer();
                for (var y = 0; y < height; y++)
                {
                    var dst = new Span<byte>(stagingPtr + y * (int)map.RowPitch, rowBytes);
                    var src = data.Slice(y * rowBytes, rowBytes);
                    src.CopyTo(dst);
                }
            }

            _Graphics.Unmap(staging);

            _CommandList.Begin();
            _CommandList.CopyTexture(staging, 0, 0, 0, 0, 0, tex, tx, ty, 0, 0, 0, width, height, 1, 1);
            _CommandList.End();            

            _Graphics.SubmitCommands(_CommandList);
        }

        private unsafe void _CopyData(Texture dstTex, InteropBitmaps.SpanBitmap srcBitmap)
        {
            srcBitmap.PinReadablePointer(ptr => _CopyData(dstTex, ptr));
        }

        private unsafe void _CopyData(Texture dstTex, InteropBitmaps.PointerBitmap srcBitmap)
        {
            var tmpTex = _GetStagingTexture(dstTex.Width, dstTex.Height);

            _PinWritablePointer(tmpTex, dstSpan => dstSpan.AsSpanBitmap().SetPixels(0, 0, srcBitmap));

            _CommandList.Begin();
            _CommandList.CopyTexture(
                tmpTex, 0, 0, 0, 0, 0,
                dstTex, 0, 0, 0, 0, 0, dstTex.Width, dstTex.Height, 1, 1);
            _CommandList.End();

            _Graphics.SubmitCommands(_CommandList);
        }

        private void _PinWritablePointer(Texture srcTexture, Action<InteropBitmaps.PointerBitmap> writer)
        {
            var fmt = From(srcTexture.Format);            

            var map = _Graphics.Map(srcTexture, MapMode.Write, 0);

            var binfo = new InteropBitmaps.BitmapInfo((int)srcTexture.Width, (int)srcTexture.Height, fmt, (int)map.RowPitch);

            var span = new InteropBitmaps.PointerBitmap(map.Data, binfo);

            writer(span);

            _Graphics.Unmap(srcTexture);
        }

        private static InteropBitmaps.Pixel.Format From(PixelFormat srcFmt)
        {
            switch(srcFmt)
            {
                case PixelFormat.R8_G8_B8_A8_SInt: return InteropBitmaps.Pixel.BGRA32.Format;
                case PixelFormat.R8_G8_B8_A8_UInt: return InteropBitmaps.Pixel.BGRA32.Format;
                case PixelFormat.R8_G8_B8_A8_UNorm: return InteropBitmaps.Pixel.RGBA32.Format;
                case PixelFormat.R8_G8_B8_A8_UNorm_SRgb: return InteropBitmaps.Pixel.RGBA32.Format;
                
                case PixelFormat.B8_G8_R8_A8_UNorm: return InteropBitmaps.Pixel.BGRA32.Format;
                case PixelFormat.B8_G8_R8_A8_UNorm_SRgb: return InteropBitmaps.Pixel.BGRA32.Format;

                case PixelFormat.R32_G32_B32_A32_Float: return InteropBitmaps.Pixel.VectorRGBA.Format;                

                default: throw new NotImplementedException();
            }
                
        }

        private Texture _GetStagingTexture(uint width, uint height)
        {
            if (_Staging == null)
            {
                var td = TextureDescription.Texture2D(width, height, 1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Staging);
                _Staging = _Graphics.ResourceFactory.CreateTexture(ref td);
            }
            else if (_Staging.Width < width || _Staging.Height < height)
            {
                var newWidth = Math.Max(width , _Staging.Width);
                var newHeight = Math.Max(height , _Staging.Height);
                var td = TextureDescription.Texture2D(newWidth, newHeight, 1, 1, PixelFormat.R8_G8_B8_A8_UNorm, TextureUsage.Staging);
                _Graphics.DisposeWhenIdle(_Staging);
                _Staging = _Graphics.ResourceFactory.CreateTexture(ref td);
            }

            return _Staging;
        }

        #endregion
    }
}
