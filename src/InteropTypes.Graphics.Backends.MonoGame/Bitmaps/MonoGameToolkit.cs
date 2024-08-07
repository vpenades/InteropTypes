﻿using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using INTEROP = InteropTypes.Graphics.Bitmaps;
using XNA = Microsoft.Xna.Framework.Graphics;

namespace InteropTypes.Graphics.Backends
{
    public static partial class MonoGameToolkit
    {
        public static XNA.SurfaceFormat ToSurfaceFormat(PixelFormat fmt)
        {
            switch(fmt.Code)
            {
                case INTEROP.Pixel.Alpha8.Code: return XNA.SurfaceFormat.Alpha8;
                case INTEROP.Pixel.BGR565.Code: return XNA.SurfaceFormat.Bgr565;
                case INTEROP.Pixel.BGRA32.Code: return XNA.SurfaceFormat.Bgra32;
                case INTEROP.Pixel.BGRA4444.Code: return XNA.SurfaceFormat.Bgra4444;
                case INTEROP.Pixel.BGRA5551.Code: return XNA.SurfaceFormat.Bgra5551;                
                case INTEROP.Pixel.BGRA128F.Code: return XNA.SurfaceFormat.Vector4;                
            }

            throw new NotSupportedException($"{fmt}");
        }

        public static PixelFormat ToInteropFormat(XNA.SurfaceFormat fmt)
        {
            switch(fmt)
            {
                case XNA.SurfaceFormat.Alpha8: return Pixel.Alpha8.Format;
                case XNA.SurfaceFormat.Bgr565: return Pixel.BGR565.Format;                
                case XNA.SurfaceFormat.Bgra32: return Pixel.BGRA32.Format;
                case XNA.SurfaceFormat.Bgra4444: return Pixel.BGRA4444.Format;
                case XNA.SurfaceFormat.Bgra5551: return Pixel.BGRA5551.Format;
                case XNA.SurfaceFormat.Vector4: return Pixel.BGRA128F.Format;
            }

            throw new NotSupportedException($"{fmt}");
        }

        public static bool TryCreateTexture(SpanBitmap src, XNA.GraphicsDevice device, out XNA.Texture2D tex)
        {
            try
            {
                tex = null;
                if (src.IsEmpty) return false;


                Copy(src, ref tex, false, device, null, null);
                return true;
            }
            catch(ArgumentException) { tex = null; return false; }
        }

        public static void Copy(SpanBitmap src, ref XNA.Texture2D dst, bool fit, XNA.GraphicsDevice device, int? width = null, int? height = null, XNA.SurfaceFormat? fmt = null)
        {
            if (src.IsEmpty) { dst = null; return; }

            var fmtx = fmt ?? ToSurfaceFormat(src.PixelFormat);

            Copy(src, ref dst,fit ,(w,h)=> new XNA.Texture2D(device, width ?? w, height ?? h, false, fmtx));
        }

        public static void Copy(SpanBitmap src, ref XNA.Texture2D dst, bool fit, Func<int, int, XNA.Texture2D> texFactory)
        {
            if (src.IsEmpty) { dst = null; return; }

            if (dst == null || dst.Width != src.Width || dst.Height != src.Height)
            {
                dst?.Dispose();
                dst = null;
            }            

            dst ??= texFactory(src.Width,src.Height);

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

            throw new NotSupportedException($"{src.PixelFormat} not supported");
        }

        private static UInt16[] _16BitBuffer;
        private static UInt32[] _32BitBuffer;

        private static void _Copy16(SpanBitmap src, XNA.Texture2D dst, bool fit)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            var fmt = ToInteropFormat(dst.Format);
            if (fmt.ByteCount != 2) throw new ArgumentException("invalid pixel size", nameof(dst));

            var l = dst.Width * dst.Height;
            if (_16BitBuffer == null || _16BitBuffer.Length < l) Array.Resize(ref _16BitBuffer, l);

            var dstx = new SpanBitmap<ushort>(_16BitBuffer, dst.Width, dst.Height, fmt);

            fit &= !(src.Width == dst.Width && src.Height == dst.Height);
            if (fit) dstx.AsTypeless().FitPixels(src);
            else dstx.AsTypeless().SetPixels(0, 0, src);

            dst.SetData(_16BitBuffer); // MONOGAME_NEEDS a SetData(ReadOnlySpan<>)
            return;
        }

        private static void _Copy32(SpanBitmap src, XNA.Texture2D dst, bool fit)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            var fmt = dst.Format == XNA.SurfaceFormat.Bgr32
                ? Pixel.BGRA32.Format
                : ToInteropFormat(dst.Format);
            if (fmt.ByteCount != 4) throw new ArgumentException("invalid pixel size", nameof(dst));

            var l = dst.Width * dst.Height;
            if (_32BitBuffer == null || _32BitBuffer.Length < l) Array.Resize(ref _32BitBuffer, l);            

            var dstx = new SpanBitmap<UInt32>(_32BitBuffer, dst.Width, dst.Height, fmt);

            fit &= !(src.Width == dst.Width && src.Height == dst.Height);
            if (fit) dstx.AsTypeless().FitPixels(src);
            else dstx.AsTypeless().SetPixels(0, 0, src);

            dst.SetData(_32BitBuffer); // MONOGAME_NEEDS a SetData(ReadOnlySpan<>)
        }
    }
}
