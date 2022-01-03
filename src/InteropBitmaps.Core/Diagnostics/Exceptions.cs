using System;
using System.Collections.Generic;
using System.Text;

namespace InteropBitmaps.Diagnostics
{
    public class PixelFormatNotSupportedException : ArgumentException
    {
        private static string _GetFormatString(Object format)
        {
            if (format == null) return "NULL";
            if (format is PixelFormat fmt) return fmt.ToString();
            return format.ToString();
        }

        public PixelFormatNotSupportedException(Object format, string paramName)
                    : base($"{_GetFormatString(format)} not supported.", paramName)
        {
            _Format = format;
        }

        private readonly Object _Format;
    }
}
