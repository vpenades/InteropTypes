using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using GDIFMT = System.Drawing.Imaging.PixelFormat;
using INTEROPFMT = InteropBitmaps.PixelFormat;

namespace InteropBitmaps
{
    static class _Implementation
    {
        #region pixel formats

        public static PixelFormat ToInteropPixelFormat(GDIFMT fmt)
        {
            switch (fmt)
            {
                case GDIFMT.Format8bppIndexed: return INTEROPFMT.Standard.GRAY8;

                case GDIFMT.Format16bppGrayScale: return INTEROPFMT.Standard.GRAY16;
                case GDIFMT.Format16bppRgb565: return INTEROPFMT.Standard.BGR565;
                case GDIFMT.Format16bppRgb555: return INTEROPFMT.Standard.BGRA5551;
                case GDIFMT.Format16bppArgb1555: return INTEROPFMT.Standard.BGRA5551;

                case GDIFMT.Format24bppRgb: return INTEROPFMT.Standard.BGR24;

                case GDIFMT.Format32bppRgb: return INTEROPFMT.Standard.BGRA32;
                case GDIFMT.Format32bppArgb: return INTEROPFMT.Standard.BGRA32;

                case GDIFMT.PAlpha:
                case GDIFMT.Format32bppPArgb:
                case GDIFMT.Format64bppPArgb:
                    throw new NotSupportedException($"Premultiplied {fmt} not supported.");

                default: throw new NotImplementedException(fmt.ToString());
            }
        }

        public static GDIFMT ToGDIPixelFormat(INTEROPFMT fmt, bool allowCompatibleFormats = false)
        {
            switch (fmt)
            {
                case INTEROPFMT.Packed.GRAY16: return GDIFMT.Format16bppGrayScale;

                case INTEROPFMT.Packed.BGR565: return GDIFMT.Format16bppRgb565;
                case INTEROPFMT.Packed.BGRA5551: return GDIFMT.Format16bppArgb1555;

                case INTEROPFMT.Packed.BGR24: return GDIFMT.Format24bppRgb;

                case INTEROPFMT.Packed.BGRA32: return GDIFMT.Format32bppArgb;
            }

            if (allowCompatibleFormats)
            {
                switch (fmt)
                {
                    case INTEROPFMT.Packed.GRAY8: // return GDIFMT.Format16bppGrayScale;                   

                    case INTEROPFMT.Packed.RGB24: return GDIFMT.Format24bppRgb;

                    case INTEROPFMT.Packed.RGBA32: return GDIFMT.Format32bppArgb;
                    case INTEROPFMT.Packed.ARGB32: return GDIFMT.Format32bppArgb;

                    default: throw new NotImplementedException(fmt.ToString());
                }
            }

            throw new NotImplementedException(fmt.ToString());
        }

        public static int GetPixelSize(GDIFMT fmt)
        {
            switch (fmt)
            {
                case GDIFMT.Format8bppIndexed: return 1;

                case GDIFMT.Format16bppArgb1555: return 2;
                case GDIFMT.Format16bppGrayScale: return 2;
                case GDIFMT.Format16bppRgb555: return 2;
                case GDIFMT.Format16bppRgb565: return 2;

                case GDIFMT.Format24bppRgb: return 3;

                case GDIFMT.Format32bppRgb: return 4;
                case GDIFMT.Format32bppArgb: return 4;
                case GDIFMT.Format32bppPArgb: return 4;

                case GDIFMT.Format48bppRgb: return 6;

                case GDIFMT.Format64bppArgb: return 8;
                case GDIFMT.Format64bppPArgb: return 8;

                default: throw new NotImplementedException();
            }
        }        

        #endregion

        #region API

        public static Bitmap CloneToGDI(SpanBitmap src, bool allowCompatibleFormats = false)
        {
            var pixFmt = src.PixelFormat.ToGDIPixelFormat(allowCompatibleFormats);

            if (!allowCompatibleFormats && src.PixelSize != pixFmt.GetPixelSize()) throw new ArgumentException(nameof(pixFmt));

            var dst = new Bitmap(src.Width, src.Height, pixFmt);

            dst.SetPixels(0, 0, src);

            return dst;
        }

        #endregion

        #region SAVE

        public static void Save(string filePath, SpanBitmap src)
        {
            var finfo = new System.IO.FileInfo(filePath);
            Save(finfo, src);
        }

        public static void Save(System.IO.FileInfo finfo, SpanBitmap src)
        {
            using (var img = CloneToGDI(src, true))
            {
                img.Save(finfo.FullName);
            }
        }

        public static void Save(System.IO.FileInfo finfo, SpanBitmap src, System.Drawing.Imaging.ImageFormat imgFmt)
        {
            using (var img = CloneToGDI(src, true))
            {
                img.Save(finfo.FullName, imgFmt);
            }
        }

        #endregion
    }
}
