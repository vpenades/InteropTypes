using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteropBitmaps
{
    static partial class WIC
    {
        public static PixelFormat ToInteropFormat(this System.Windows.Media.PixelFormat fmt)
        {
            // if (fmt == System.Windows.Media.PixelFormats.Default) return PixelFormat.GetUndefinedOfSize(fmt.BitsPerPixel / 8);

            if (fmt == System.Windows.Media.PixelFormats.Gray8) return PixelFormat.Standard.GRAY8;

            if (fmt == System.Windows.Media.PixelFormats.Gray16) return PixelFormat.Standard.GRAY16;
            if (fmt == System.Windows.Media.PixelFormats.Bgr555) return PixelFormat.Standard.BGRA5551;
            if (fmt == System.Windows.Media.PixelFormats.Bgr565) return PixelFormat.Standard.BGR565;

            if (fmt == System.Windows.Media.PixelFormats.Bgr24) return PixelFormat.Standard.BGR24;
            if (fmt == System.Windows.Media.PixelFormats.Rgb24) return PixelFormat.Standard.RGB24;

            if (fmt == System.Windows.Media.PixelFormats.Bgr32) return PixelFormat.Standard.BGRA32;
            if (fmt == System.Windows.Media.PixelFormats.Bgra32) return PixelFormat.Standard.BGRA32;

            throw new NotImplementedException();
        }

        public static System.Windows.Media.PixelFormat ToWICFormat(this PixelFormat fmt)
        {
            switch (fmt.PackedFormat)
            {
                case PixelFormat.Packed.GRAY8: return System.Windows.Media.PixelFormats.Gray8;

                case PixelFormat.Packed.GRAY16: return System.Windows.Media.PixelFormats.Gray16;
                case PixelFormat.Packed.BGRA5551: return System.Windows.Media.PixelFormats.Bgr555;
                case PixelFormat.Packed.BGR565: return System.Windows.Media.PixelFormats.Bgr565;

                case PixelFormat.Packed.BGR24: return System.Windows.Media.PixelFormats.Bgr24;
                case PixelFormat.Packed.RGB24: return System.Windows.Media.PixelFormats.Rgb24;

                case PixelFormat.Packed.BGRA32: return System.Windows.Media.PixelFormats.Bgra32;

                default: throw new NotImplementedException();
            }
        }
    }
}
