using System;
using System.Collections.Generic;
using System.Text;

using GDIFMT = System.Drawing.Imaging.PixelFormat;
using ELEMENT = InteropBitmaps.PixelElementFormat;

namespace InteropBitmaps
{
    partial class GDIToolkit
    {
        public static PixelFormat ToInteropPixelFormat(this GDIFMT fmt)
        {
            switch (fmt)
            {
                case GDIFMT.Format8bppIndexed: return new PixelFormat(ELEMENT.Index8);

                case GDIFMT.Format16bppRgb565: return new PixelFormat(ELEMENT.Red5, ELEMENT.Green6, ELEMENT.Blue5);
                case GDIFMT.Format16bppRgb555: return new PixelFormat(ELEMENT.Red5, ELEMENT.Green5, ELEMENT.Blue5, ELEMENT.Undefined1);
                case GDIFMT.Format16bppArgb1555: return new PixelFormat(ELEMENT.Red5, ELEMENT.Green5, ELEMENT.Blue5, ELEMENT.Alpha1);

                case GDIFMT.Format24bppRgb: return new PixelFormat(ELEMENT.Red8, ELEMENT.Green8, ELEMENT.Blue8);

                case GDIFMT.Format32bppRgb: return new PixelFormat(ELEMENT.Red8, ELEMENT.Green8, ELEMENT.Blue8, ELEMENT.Undefined8);
                case GDIFMT.Format32bppArgb: return new PixelFormat(ELEMENT.Alpha8, ELEMENT.Red8, ELEMENT.Green8, ELEMENT.Blue8);

                case GDIFMT.PAlpha:
                case GDIFMT.Format32bppPArgb:
                case GDIFMT.Format64bppPArgb:
                    throw new NotSupportedException($"Premultiplied {fmt} not supported.");

                default: throw new NotImplementedException(fmt.ToString());
            }
        }

        public static int GetPixelSize(this GDIFMT fmt)
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
    }
}
