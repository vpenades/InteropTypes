using System;
using System.Collections.Generic;
using System.Text;

using INTEROP = InteropTypes.Graphics.Bitmaps;
using XNA = Microsoft.Xna.Framework.Graphics;

namespace InteropTypes.Graphics.Backends
{
    public static partial class MonoGameToolkit
    {
        private static UInt16[] _16BitBuffer;
        private static UInt32[] _32BitBuffer;

        public static XNA.SurfaceFormat ToSurfaceFormat(INTEROP.PixelFormat fmt)
        {
            switch(fmt.Code)
            {
                case INTEROP.Pixel.Alpha8.Code: return XNA.SurfaceFormat.Alpha8;
                case INTEROP.Pixel.BGR565.Code: return XNA.SurfaceFormat.Bgr565;
                case INTEROP.Pixel.BGRA4444.Code: return XNA.SurfaceFormat.Bgra4444;
                case INTEROP.Pixel.BGRA5551.Code: return XNA.SurfaceFormat.Bgra5551;
                case INTEROP.Pixel.BGRA32.Code: return XNA.SurfaceFormat.Bgra32;
                case INTEROP.Pixel.BGRA128F.Code: return XNA.SurfaceFormat.Vector4;
            }

            throw new NotSupportedException($"{fmt}");
        }

        public static INTEROP.PixelFormat ToInteropFormat(XNA.SurfaceFormat fmt)
        {
            switch(fmt)
            {
                case XNA.SurfaceFormat.Alpha8: return INTEROP.Pixel.Alpha8.Format;
                case XNA.SurfaceFormat.Bgr565: return INTEROP.Pixel.BGR565.Format;
                case XNA.SurfaceFormat.Bgra4444: return INTEROP.Pixel.BGRA4444.Format;
                case XNA.SurfaceFormat.Bgra5551: return INTEROP.Pixel.BGRA5551.Format;
                case XNA.SurfaceFormat.Bgra32: return INTEROP.Pixel.BGRA32.Format;
                case XNA.SurfaceFormat.Vector4: return INTEROP.Pixel.BGRA128F.Format;
            }

            throw new NotSupportedException($"{fmt}");
        }

        public static bool TryCreateTexture(INTEROP.SpanBitmap src, XNA.GraphicsDevice device, out XNA.Texture2D tex)
        {
            try
            {
                tex = null;
                Copy(src, ref tex, false, device, null, null);
                return true;
            }
            catch(ArgumentException) { tex = null; return false; }
        }

        public static void Copy(INTEROP.SpanBitmap src, ref XNA.Texture2D dst, bool fit, XNA.GraphicsDevice device, int? width = null, int? height = null, XNA.SurfaceFormat? fmt = null)
        {
            var fmtx = fmt.HasValue ? fmt.Value : ToSurfaceFormat(src.PixelFormat);

            Copy(src, ref dst, fit , (w,h)=> new XNA.Texture2D(device, width ?? w, height ?? h, false, fmtx));
        }

        public static void Copy(INTEROP.SpanBitmap src, ref XNA.Texture2D dst, bool fit, Func<int, int, XNA.Texture2D> texFactory)
        {
            if (dst == null || dst.Width != src.Width || dst.Height != src.Height)
            {
                if (dst != null) dst.Dispose();
                dst = null;
            }

            if (dst == null) dst = texFactory(src.Width,src.Height);

            if (dst.Format == XNA.SurfaceFormat.Bgr565)
            {
                _Copy16(src, dst, fit);
                return;
            }

            if (dst.Format == XNA.SurfaceFormat.Bgra32)
            {
                _Copy32(src, dst, fit);
                return;
            }
        }

        private static void _Copy16(INTEROP.SpanBitmap src, XNA.Texture2D dst, bool fit)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            var fmt = ToInteropFormat(dst.Format);
            if (fmt.ByteCount != 2) throw new ArgumentException("invalid pixel size", nameof(dst));

            var l = dst.Width * dst.Height;
            if (_16BitBuffer == null || _16BitBuffer.Length < l) Array.Resize(ref _16BitBuffer, l);

            var dstx = new INTEROP.SpanBitmap<UInt16>(_16BitBuffer, dst.Width, dst.Height, fmt);

            fit &= !(src.Width == dst.Width && src.Height == dst.Height);
            if (fit) dstx.AsTypeless().FitPixels(src);
            else dstx.AsTypeless().SetPixels(0, 0, src);

            dst.SetData(_16BitBuffer);
            return;
        }

        private static void _Copy32(INTEROP.SpanBitmap src, XNA.Texture2D dst, bool fit)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            var fmt = dst.Format == XNA.SurfaceFormat.Bgr32
                ? INTEROP.Pixel.BGRA32.Format
                : ToInteropFormat(dst.Format);
            if (fmt.ByteCount != 4) throw new ArgumentException("invalid pixel size", nameof(dst));

            var l = dst.Width * dst.Height;
            if (_32BitBuffer == null || _32BitBuffer.Length < l) Array.Resize(ref _32BitBuffer, l);            

            var dstx = new INTEROP.SpanBitmap<UInt32>(_32BitBuffer, dst.Width, dst.Height, fmt);

            fit &= !(src.Width == dst.Width && src.Height == dst.Height);
            if (fit) dstx.AsTypeless().FitPixels(src);
            else dstx.AsTypeless().SetPixels(0, 0, src);

            dst.SetData(_32BitBuffer);
        }        
    }
}
