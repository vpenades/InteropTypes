using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

using InteropTypes.Graphics.Backends.Bitmaps;
using InteropTypes.Graphics.Bitmaps;

namespace InteropTypes.Graphics.Backends
{
    public class AvaloniaBitmapConverter : IValueConverter
    {
        public static IValueConverter Default { get; } = new AvaloniaBitmapConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!targetType.IsAssignableTo(typeof(Avalonia.Media.IImage))) return value;

            switch(value)
            {
                case MemoryBitmap mbmp: return _Implementation.CreateAvaloniaBitmap(mbmp.AsSpanBitmap());
                case AvaloniaBitmapSwapChain chain: return chain.FrontBuffer;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }        
    }
}
