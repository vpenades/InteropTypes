using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

using InteropTypes.Graphics.Bitmaps;

using TARGETBITMAP = System.Windows.Media.Imaging.WriteableBitmap;

namespace InteropTypes.Graphics.Backends.WPF
{
    public class MemoryBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (typeof(System.Windows.Media.ImageSource).IsAssignableFrom(targetType))
            {
                var targetValue = parameter as TARGETBITMAP;

                if (value is Bitmaps.SpanBitmap.ISource spanSource)
                {
                    _CopyBitmap(spanSource.AsSpanBitmap(), ref targetValue);

                    return targetValue;
                }

                if (value is Bitmaps.InterlockedBitmap interlockBmp)
                {
                    interlockBmp.TryDropAndDequeueLast(mb => _CopyBitmap(mb.AsSpanBitmap(), ref targetValue));
                    return targetValue;
                }
            }

            return null;
        }

        private static void _CopyBitmap(SpanBitmap src, ref TARGETBITMAP dst)
        {
            if (src.IsEmpty) { dst = null; return; }

            src.WithWPF().CopyTo(ref dst);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return default(MemoryBitmap);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// based on <see cref="System.Windows.ColorConvertedBitmapExtension"/>, <see cref="System.Windows.TemplateBindingExtension"/>
    /// </remarks>
    [MarkupExtensionReturnType(typeof(MemoryBitmapExtension))]
    public class MemoryBitmapExtension : MarkupExtension
    {
        public MemoryBitmapExtension()
        {            
        }

        // https://docs.microsoft.com/es-es/dotnet/api/system.windows.markup.markupextension.providevalue?view=windowsdesktop-6.0
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;            

            return null;

            var targetValue = null as System.Windows.Media.Imaging.WriteableBitmap;

            // _BitmapSource.AsSpanBitmap().WithWPF().CopyTo(ref targetValue);

            return targetValue;
        }
    }    
}
