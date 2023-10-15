using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace InteropTypes.IO
{
    /// <summary>
    /// A format provider that converts a file size to a friendly string.
    /// </summary>
    public class FileSizeFormatProvider : IFormatProvider, ICustomFormatter
    {
        public static IFormatProvider Default { get; } = new FileSizeFormatProvider();

        // Define the file size units
        private static readonly string[] _FileSizeUnits = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        // Get the format provider for the specified type
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : (object)null;
        }

        // Format the file size in a user-friendly way
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is IConvertible convertible)
            {
                arg = convertible.ToDouble(formatProvider);
            }

            // Check if the argument is a double value
            if (arg is double valDouble)
            {
                bool isNegative = valDouble < 0;
                if (isNegative) valDouble = -valDouble;

                // Find the appropriate unit index
                int unitIndex = 0;
                while (valDouble >= 1024 && unitIndex < _FileSizeUnits.Length)
                {
                    valDouble /= 1024;
                    unitIndex++;
                }

                // Return the formatted file size with the unit

                var text = (isNegative ? "-" : string.Empty);
                text += string.Format("{0:0.##} {1}", valDouble, _FileSizeUnits[unitIndex]);
                return text;
            }
            // If the argument is not a long value, use the default format provider
            else
            {
                if (arg is IFormattable formattable) return formattable.ToString(format, CultureInfo.CurrentCulture);
                else if (arg != null) return arg.ToString();
                else return string.Empty;
            }
        }
    }
}
