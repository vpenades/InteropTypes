using System;
using System.Collections.Generic;
using System.Text;

using InteropTypes.Graphics.Bitmaps;

using INTEROP = InteropTypes.Graphics.Bitmaps;


namespace InteropTypes.Graphics.Backends
{
    public static partial class MonoGameToolkit
    {
        public static SurfaceFormat ToSurfaceFormat(PixelFormat fmt)
        {
            switch(fmt.Code)
            {
                case INTEROP.Pixel.Luminance32F.Code: return SurfaceFormat.Single;
                case INTEROP.Pixel.Alpha8.Code: return SurfaceFormat.Alpha8;                
                case INTEROP.Pixel.BGR565.Code: return SurfaceFormat.Bgr565;
                case INTEROP.Pixel.BGRA32.Code: return SurfaceFormat.Bgra32;
                case INTEROP.Pixel.BGRA4444.Code: return SurfaceFormat.Bgra4444;
                case INTEROP.Pixel.BGRA5551.Code: return SurfaceFormat.Bgra5551;                
                case INTEROP.Pixel.BGRA128F.Code: return SurfaceFormat.Vector4;                
            }

            throw new NotSupportedException($"{fmt}");
        }

        public static PixelFormat ToInteropFormat(SurfaceFormat fmt)
        {
            switch(fmt)
            {
                case SurfaceFormat.Single: return Pixel.Luminance32F.Format;
                case SurfaceFormat.Alpha8: return Pixel.Alpha8.Format;
                case SurfaceFormat.Bgr565: return Pixel.BGR565.Format;                
                case SurfaceFormat.Bgra32: return Pixel.BGRA32.Format;
                case SurfaceFormat.Bgra4444: return Pixel.BGRA4444.Format;
                case SurfaceFormat.Bgra5551: return Pixel.BGRA5551.Format;
                case SurfaceFormat.Vector4: return Pixel.BGRA128F.Format;
            }

            throw new NotSupportedException($"{fmt}");
        }

        public static bool TryCreateTexture(SpanBitmap src, GraphicsDevice device, out Texture2D tex)
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

        public static void Copy(SpanBitmap src, ref Texture2D dst, bool fit, GraphicsDevice device, int? width = null, int? height = null, SurfaceFormat? fmt = null)
        {
            if (src.IsEmpty) { dst = null; return; }

            var fmtx = fmt ?? ToSurfaceFormat(src.PixelFormat);

            Copy(src, ref dst,fit ,(w,h)=> new Texture2D(device, width ?? w, height ?? h, false, fmtx));
        }

        public static void Copy(SpanBitmap src, ref Texture2D dst, bool fit, Func<int, int, Texture2D> texFactory)
        {
            if (src.IsEmpty) { dst = null; return; }

            if (dst == null || dst.Width != src.Width || dst.Height != src.Height)
            {
                dst?.Dispose();
                dst = null;
            }            

            dst ??= texFactory(src.Width,src.Height);

            if (dst.Format == SurfaceFormat.Alpha8)
            {
                _Copy8(src, dst, fit);
                return;
            }

            if (dst.Format == SurfaceFormat.Bgr565 || dst.Format == SurfaceFormat.Bgra4444 || dst.Format == SurfaceFormat.Bgra5551)
            {
                _Copy16(src, dst, fit);
                return;
            }

            if (dst.Format == SurfaceFormat.Bgra32 || dst.Format == SurfaceFormat.Single)
            {
                _Copy32(src, dst, fit);
                return;
            }

            throw new NotSupportedException($"{src.PixelFormat} not supported");
        }

        private static Byte[] _8BitBuffer;
        private static UInt16[] _16BitBuffer;
        private static UInt32[] _32BitBuffer;

        private static ArraySegment<Byte> _GetTemporal8(int length)
        {
            if (_8BitBuffer == null || _8BitBuffer.Length < length) Array.Resize(ref _8BitBuffer, length);

            return new ArraySegment<Byte>(_8BitBuffer, 0, length);
        }

        private static ArraySegment<UInt16> _GetTemporal16(int length)
        {
            if (_16BitBuffer == null || _16BitBuffer.Length < length) Array.Resize(ref _16BitBuffer, length);

            return new ArraySegment<UInt16>(_16BitBuffer, 0, length);
        }

        private static ArraySegment<UInt32> _GetTemporal32(int length)
        {
            if (_32BitBuffer == null || _32BitBuffer.Length < length) Array.Resize(ref _32BitBuffer, length);

            return new ArraySegment<UInt32>(_32BitBuffer, 0, length);
        }

        private static void _Copy8(SpanBitmap src, Texture2D dst, bool fit)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            var fmt = ToInteropFormat(dst.Format);
            if (fmt.ByteCount != 1) throw new ArgumentException("invalid pixel size", nameof(dst));            

            var tmp = _GetTemporal8(dst.Width * dst.Height);
            var dstx = new SpanBitmap<Byte>(tmp, dst.Width, dst.Height, fmt);

            fit &= !(src.Width == dst.Width && src.Height == dst.Height);
            if (fit) dstx.AsTypeless().FitPixels(src);
            else dstx.AsTypeless().SetPixels(0, 0, src);

            SetData(dst, tmp);
            return;
        }

        private static void _Copy16(SpanBitmap src, Texture2D dst, bool fit)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            var fmt = ToInteropFormat(dst.Format);
            if (fmt.ByteCount != 2) throw new ArgumentException("invalid pixel size", nameof(dst));

            var tmp = _GetTemporal16(dst.Width * dst.Height);
            var dstx = new SpanBitmap<ushort>(tmp, dst.Width, dst.Height, fmt);

            fit &= !(src.Width == dst.Width && src.Height == dst.Height);
            if (fit) dstx.AsTypeless().FitPixels(src);
            else dstx.AsTypeless().SetPixels(0, 0, src);

            SetData(dst, tmp);            
        }

        private static void _Copy32(SpanBitmap src, Texture2D dst, bool fit)
        {
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            var fmt = dst.Format == SurfaceFormat.Bgr32
                ? Pixel.BGRA32.Format
                : ToInteropFormat(dst.Format);
            if (fmt.ByteCount != 4) throw new ArgumentException("invalid pixel size", nameof(dst));

            var tmp = _GetTemporal32(dst.Width * dst.Height);
            var dstx = new SpanBitmap<UInt32>(tmp, dst.Width, dst.Height, fmt);

            fit &= !(src.Width == dst.Width && src.Height == dst.Height);
            if (fit) dstx.AsTypeless().FitPixels(src);
            else dstx.AsTypeless().SetPixels(0, 0, src);

            SetData(dst, tmp);
        }

        public static void SetData<T>(Texture2D texture, ArraySegment<T> data)
            where T : unmanaged
        {
            // MONOGAME_NEEDS a SetData(ReadOnlySpan<>)

            texture.SetData(data.Array, data.Offset, data.Count);
        }
    }
}
