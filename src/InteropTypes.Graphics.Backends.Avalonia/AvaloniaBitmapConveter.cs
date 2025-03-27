using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
{
    public class AvaloniaBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Avalonia.Media.IImage) && value is MemoryBitmap src)
            {
                return _Implementation.CreateAvaloniaBitmap(src.AsSpanBitmap());
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }        
    }
}
